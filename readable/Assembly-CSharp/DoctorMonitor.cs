using System;

// Token: 0x02000A21 RID: 2593
public class DoctorMonitor : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance>
{
	// Token: 0x06004BE8 RID: 19432 RVA: 0x001B926B File Offset: 0x001B746B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleUrge(Db.Get().Urges.Doctor);
	}

	// Token: 0x02001AD5 RID: 6869
	public new class Instance : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A747 RID: 42823 RVA: 0x003BBF76 File Offset: 0x003BA176
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
