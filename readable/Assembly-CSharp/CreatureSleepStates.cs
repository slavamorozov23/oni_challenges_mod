using System;
using STRINGS;

// Token: 0x020000E5 RID: 229
public class CreatureSleepStates : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>
{
	// Token: 0x0600042D RID: 1069 RVA: 0x00022D80 File Offset: 0x00020F80
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.pre.QueueAnim("sleep_pre", false, null).OnAnimQueueComplete(this.loop);
		this.loop.QueueAnim("sleep_loop", true, null).Transition(this.pst, new StateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.Transition.ConditionCallback(CreatureSleepStates.ShouldWakeUp), UpdateRate.SIM_1000ms);
		this.pst.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.SleepBehaviour, false);
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00022E57 File Offset: 0x00021057
	public static bool ShouldWakeUp(CreatureSleepStates.Instance smi)
	{
		return !GameClock.Instance.IsNighttime();
	}

	// Token: 0x0400031C RID: 796
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pre;

	// Token: 0x0400031D RID: 797
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State loop;

	// Token: 0x0400031E RID: 798
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pst;

	// Token: 0x0400031F RID: 799
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State behaviourcomplete;

	// Token: 0x02001108 RID: 4360
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001109 RID: 4361
	public new class Instance : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.GameInstance
	{
		// Token: 0x06008392 RID: 33682 RVA: 0x0034398E File Offset: 0x00341B8E
		public Instance(Chore<CreatureSleepStates.Instance> chore, CreatureSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.SleepBehaviour);
		}
	}
}
