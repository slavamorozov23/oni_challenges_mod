using System;
using STRINGS;

// Token: 0x0200010F RID: 271
public class RobotDeathStates : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x000281B8 File Offset: 0x000263B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State state = this.loop;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).PlayAnim((RobotDeathStates.Instance smi) => smi.def.deathAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, (RobotDeathStates.Instance smi) => smi.gameObject).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x0400039D RID: 925
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State loop;

	// Token: 0x0400039E RID: 926
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State pst;

	// Token: 0x0200118C RID: 4492
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006500 RID: 25856
		public string deathAnim = "death";
	}

	// Token: 0x0200118D RID: 4493
	public new class Instance : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.GameInstance
	{
		// Token: 0x060084DD RID: 34013 RVA: 0x00345FFC File Offset: 0x003441FC
		public Instance(Chore<RobotDeathStates.Instance> chore, RobotDeathStates.Def def) : base(chore, def)
		{
			chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
			chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}
	}
}
