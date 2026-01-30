using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class BaggedStates : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>
{
	// Token: 0x060003D4 RID: 980 RVA: 0x00020878 File Offset: 0x0001EA78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.bagged;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.BAGGED.NAME;
		string tooltip = CREATURES.STATUSITEMS.BAGGED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.bagged.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagStart)).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<BaggedStates.Instance, string>(BaggedStates.GetBaggedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Bagged, null, true).Transition(this.escape, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.Transition.ConditionCallback(BaggedStates.ShouldEscape), UpdateRate.SIM_4000ms).EventHandler(GameHashes.OnStore, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.OnStore)).Exit(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagEnd));
		this.escape.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.Unbag)).PlayAnim("escape").OnAnimQueueComplete(null);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x00020984 File Offset: 0x0001EB84
	public static string GetBaggedAnimName(BaggedStates.Instance smi)
	{
		return Baggable.GetBaggedAnimName(smi.gameObject);
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00020991 File Offset: 0x0001EB91
	private static void BagStart(BaggedStates.Instance smi)
	{
		if (smi.baggedTime == 0f)
		{
			smi.baggedTime = GameClock.Instance.GetTime();
		}
		smi.UpdateFaller(true);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x000209B7 File Offset: 0x0001EBB7
	private static void BagEnd(BaggedStates.Instance smi)
	{
		smi.baggedTime = 0f;
		smi.UpdateFaller(false);
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x000209CC File Offset: 0x0001EBCC
	private static void Unbag(BaggedStates.Instance smi)
	{
		Baggable component = smi.gameObject.GetComponent<Baggable>();
		if (component)
		{
			component.Free();
		}
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x000209F3 File Offset: 0x0001EBF3
	private static void OnStore(BaggedStates.Instance smi)
	{
		smi.UpdateFaller(true);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x000209FC File Offset: 0x0001EBFC
	private static bool ShouldEscape(BaggedStates.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Stored) && GameClock.Instance.GetTime() - smi.baggedTime >= smi.def.escapeTime;
	}

	// Token: 0x040002F8 RID: 760
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State bagged;

	// Token: 0x040002F9 RID: 761
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape;

	// Token: 0x020010D7 RID: 4311
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006363 RID: 25443
		public float escapeTime = 300f;
	}

	// Token: 0x020010D8 RID: 4312
	public new class Instance : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.GameInstance
	{
		// Token: 0x06008326 RID: 33574 RVA: 0x00342EAF File Offset: 0x003410AF
		public Instance(Chore<BaggedStates.Instance> chore, BaggedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(BaggedStates.Instance.IsBagged, null);
		}

		// Token: 0x06008327 RID: 33575 RVA: 0x00342EC8 File Offset: 0x003410C8
		public void UpdateFaller(bool bagged)
		{
			bool flag = bagged && !base.gameObject.HasTag(GameTags.Stored);
			bool flag2 = GameComps.Fallers.Has(base.gameObject);
			if (flag != flag2)
			{
				if (flag)
				{
					GameComps.Fallers.Add(base.gameObject, Vector2.zero);
					return;
				}
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x04006364 RID: 25444
		[Serialize]
		public float baggedTime;

		// Token: 0x04006365 RID: 25445
		public static readonly Chore.Precondition IsBagged = new Chore.Precondition
		{
			id = "IsBagged",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.Bagged);
			}
		};
	}
}
