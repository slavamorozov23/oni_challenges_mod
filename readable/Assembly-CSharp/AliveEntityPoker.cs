using System;
using UnityEngine;

// Token: 0x020005B0 RID: 1456
public class AliveEntityPoker : GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>
{
	// Token: 0x0600216A RID: 8554 RVA: 0x000C2204 File Offset: 0x000C0404
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.approach;
		this.root.Enter(new StateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State.Callback(AliveEntityPoker.RefreshTarget)).TagTransition(AliveEntityPoker.BehaviourTag, null, true);
		this.approach.InitializeStates(this.poker, this.victim, (AliveEntityPoker.Instance smi) => smi.VictimPokeOffsets, this.poke, this.failed, null).ToggleMainStatusItem(new Func<AliveEntityPoker.Instance, StatusItem>(AliveEntityPoker.GetGoingToPokeStatusItem), null);
		this.poke.ToggleAnims((AliveEntityPoker.Instance smi) => smi.def.PokeAnimFileName).OnTargetLost(this.victim, null).DefaultState(this.poke.pre).ToggleMainStatusItem(new Func<AliveEntityPoker.Instance, StatusItem>(AliveEntityPoker.GetPokingStatusItem), null);
		this.poke.pre.PlayAnim((AliveEntityPoker.Instance smi) => smi.def.PokeAnim_Pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.poke.loop);
		this.poke.loop.PlayAnim((AliveEntityPoker.Instance smi) => smi.def.PokeAnim_Loop, KAnim.PlayMode.Once).OnAnimQueueComplete(this.poke.pst);
		this.poke.pst.PlayAnim((AliveEntityPoker.Instance smi) => smi.def.PokeAnim_Pst, KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
		this.complete.TriggerOnEnter(GameHashes.EntityPoked, (AliveEntityPoker.Instance smi) => smi.CurrentVictim).BehaviourComplete(AliveEntityPoker.BehaviourTag, false);
		this.failed.Target(this.poker).TriggerOnEnter(GameHashes.TargetLost, null).EnterGoTo(null);
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x000C240F File Offset: 0x000C060F
	public static StatusItem GetGoingToPokeStatusItem(AliveEntityPoker.Instance smi)
	{
		return AliveEntityPoker.GetStatusItem(smi, smi.def.statusItemSTR_goingToPoke);
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000C2422 File Offset: 0x000C0622
	public static StatusItem GetPokingStatusItem(AliveEntityPoker.Instance smi)
	{
		return AliveEntityPoker.GetStatusItem(smi, smi.def.statusItemSTR_poking);
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x000C2438 File Offset: 0x000C0638
	private static StatusItem GetStatusItem(AliveEntityPoker.Instance smi, string address)
	{
		string name = Strings.Get(address + ".NAME");
		string tooltip = Strings.Get(address + ".TOOLTIP");
		return new StatusItem(smi.GetCurrentState().longName, name, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, true, null);
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000C249B File Offset: 0x000C069B
	public static void ClearPreviousVictim(AliveEntityPoker.Instance smi)
	{
		smi.sm.victim.Set(null, smi);
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x000C24B0 File Offset: 0x000C06B0
	public static void RefreshTarget(AliveEntityPoker.Instance smi)
	{
		PokeMonitor.Instance smi2 = smi.GetSMI<PokeMonitor.Instance>();
		smi.sm.victim.Set(smi2.Target, smi, false);
		smi.VictimPokeOffsets = smi2.TargetOffsets;
	}

	// Token: 0x0400137E RID: 4990
	public static readonly Tag BehaviourTag = GameTags.Creatures.UrgeToPoke;

	// Token: 0x0400137F RID: 4991
	public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.ApproachSubState<Pickupable> approach;

	// Token: 0x04001380 RID: 4992
	public AliveEntityPoker.PokeStates poke;

	// Token: 0x04001381 RID: 4993
	public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State complete;

	// Token: 0x04001382 RID: 4994
	public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State failed;

	// Token: 0x04001383 RID: 4995
	public StateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.TargetParameter poker;

	// Token: 0x04001384 RID: 4996
	public StateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.TargetParameter victim;

	// Token: 0x0200143D RID: 5181
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006DEB RID: 28139
		public string PokeAnimFileName;

		// Token: 0x04006DEC RID: 28140
		public string PokeAnim_Pre;

		// Token: 0x04006DED RID: 28141
		public string PokeAnim_Loop;

		// Token: 0x04006DEE RID: 28142
		public string PokeAnim_Pst;

		// Token: 0x04006DEF RID: 28143
		public string statusItemSTR_goingToPoke;

		// Token: 0x04006DF0 RID: 28144
		public string statusItemSTR_poking;
	}

	// Token: 0x0200143E RID: 5182
	public class PokeStates : GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State
	{
		// Token: 0x04006DF1 RID: 28145
		public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State pre;

		// Token: 0x04006DF2 RID: 28146
		public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State loop;

		// Token: 0x04006DF3 RID: 28147
		public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State pst;
	}

	// Token: 0x0200143F RID: 5183
	public new class Instance : GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.GameInstance
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06008F12 RID: 36626 RVA: 0x0036A4E8 File Offset: 0x003686E8
		public GameObject CurrentVictim
		{
			get
			{
				return base.sm.victim.Get(this);
			}
		}

		// Token: 0x06008F13 RID: 36627 RVA: 0x0036A4FC File Offset: 0x003686FC
		public Instance(Chore<AliveEntityPoker.Instance> chore, AliveEntityPoker.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.UrgeToPoke);
			base.sm.poker.Set(base.smi.gameObject, base.smi, false);
		}

		// Token: 0x04006DF4 RID: 28148
		public CellOffset[] VictimPokeOffsets;
	}
}
