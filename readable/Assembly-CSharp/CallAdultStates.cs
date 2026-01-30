using System;
using STRINGS;

// Token: 0x020000E1 RID: 225
public class CallAdultStates : GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>
{
	// Token: 0x06000416 RID: 1046 RVA: 0x00022188 File Offset: 0x00020388
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.pre.QueueAnim("call_pre", false, null).OnAnimQueueComplete(this.loop);
		this.loop.QueueAnim("call_loop", false, null).OnAnimQueueComplete(this.pst);
		this.pst.QueueAnim("call_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.CallAdultBehaviour, false);
	}

	// Token: 0x04000310 RID: 784
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State pre;

	// Token: 0x04000311 RID: 785
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State loop;

	// Token: 0x04000312 RID: 786
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State pst;

	// Token: 0x04000313 RID: 787
	public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State behaviourcomplete;

	// Token: 0x020010F9 RID: 4345
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010FA RID: 4346
	public new class Instance : GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.GameInstance
	{
		// Token: 0x06008367 RID: 33639 RVA: 0x003435EE File Offset: 0x003417EE
		public Instance(Chore<CallAdultStates.Instance> chore, CallAdultStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.CallAdultBehaviour);
		}
	}
}
