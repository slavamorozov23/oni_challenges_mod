using System;

// Token: 0x020003E9 RID: 1001
public class SweepBotTrappedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>
{
	// Token: 0x06001493 RID: 5267 RVA: 0x00074F84 File Offset: 0x00073184
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.blockedStates.evaluating;
		this.blockedStates.ToggleStatusItem(Db.Get().RobotStatusItems.CantReachStation, (SweepBotTrappedStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Robots.Behaviours.TrappedBehaviour, this.behaviourcomplete, true);
		this.blockedStates.evaluating.Enter(delegate(SweepBotTrappedStates.Instance smi)
		{
			if (smi.sm.GetSweepLocker(smi) == null)
			{
				smi.GoTo(this.blockedStates.noHome);
				return;
			}
			smi.GoTo(this.blockedStates.blocked);
		});
		this.blockedStates.blocked.ToggleChore((SweepBotTrappedStates.Instance smi) => new RescueSweepBotChore(smi.master, smi.master.gameObject, smi.sm.GetSweepLocker(smi).gameObject), this.behaviourcomplete, this.blockedStates.evaluating).PlayAnim("react_stuck", KAnim.PlayMode.Loop);
		this.blockedStates.noHome.PlayAnim("react_stuck", KAnim.PlayMode.Once).OnAnimQueueComplete(this.blockedStates.evaluating);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.TrappedBehaviour, false);
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x0007509C File Offset: 0x0007329C
	public Storage GetSweepLocker(SweepBotTrappedStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x04000C71 RID: 3185
	public SweepBotTrappedStates.BlockedStates blockedStates;

	// Token: 0x04000C72 RID: 3186
	public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State behaviourcomplete;

	// Token: 0x0200125F RID: 4703
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001260 RID: 4704
	public new class Instance : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.GameInstance
	{
		// Token: 0x060087D6 RID: 34774 RVA: 0x0034C956 File Offset: 0x0034AB56
		public Instance(Chore<SweepBotTrappedStates.Instance> chore, SweepBotTrappedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.TrappedBehaviour);
		}
	}

	// Token: 0x02001261 RID: 4705
	public class BlockedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State
	{
		// Token: 0x040067A5 RID: 26533
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State evaluating;

		// Token: 0x040067A6 RID: 26534
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State blocked;

		// Token: 0x040067A7 RID: 26535
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State noHome;
	}
}
