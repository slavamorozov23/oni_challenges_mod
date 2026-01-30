using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class IncubatingStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>
{
	// Token: 0x060004D0 RID: 1232 RVA: 0x00026F50 File Offset: 0x00025150
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubator;
		GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.IN_INCUBATOR.NAME;
		string tooltip = CREATURES.STATUSITEMS.IN_INCUBATOR.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.incubator.DefaultState(this.incubator.idle).ToggleTag(GameTags.Creatures.Deliverable).TagTransition(GameTags.Creatures.InIncubator, null, true);
		this.incubator.idle.Enter("VariantUpdate", new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State.Callback(IncubatingStates.VariantUpdate)).PlayAnim("incubator_idle_loop").OnAnimQueueComplete(this.incubator.choose);
		this.incubator.choose.Transition(this.incubator.variant, new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant), UpdateRate.SIM_200ms).Transition(this.incubator.idle, GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Not(new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant)), UpdateRate.SIM_200ms);
		this.incubator.variant.PlayAnim("incubator_variant").OnAnimQueueComplete(this.incubator.idle);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00027088 File Offset: 0x00025288
	public static bool DoVariant(IncubatingStates.Instance smi)
	{
		return smi.variant_time == 0;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00027093 File Offset: 0x00025293
	public static void VariantUpdate(IncubatingStates.Instance smi)
	{
		if (smi.variant_time <= 0)
		{
			smi.variant_time = UnityEngine.Random.Range(3, 7);
			return;
		}
		smi.variant_time--;
	}

	// Token: 0x04000384 RID: 900
	public IncubatingStates.IncubatorStates incubator;

	// Token: 0x0200116E RID: 4462
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200116F RID: 4463
	public new class Instance : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.GameInstance
	{
		// Token: 0x06008486 RID: 33926 RVA: 0x00345234 File Offset: 0x00343434
		public Instance(Chore<IncubatingStates.Instance> chore, IncubatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(IncubatingStates.Instance.IsInIncubator, null);
		}

		// Token: 0x040064A7 RID: 25767
		public int variant_time = 3;

		// Token: 0x040064A8 RID: 25768
		public static readonly Chore.Precondition IsInIncubator = new Chore.Precondition
		{
			id = "IsInIncubator",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.InIncubator);
			}
		};
	}

	// Token: 0x02001170 RID: 4464
	public class IncubatorStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State
	{
		// Token: 0x040064A9 RID: 25769
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle;

		// Token: 0x040064AA RID: 25770
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State choose;

		// Token: 0x040064AB RID: 25771
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State variant;
	}
}
