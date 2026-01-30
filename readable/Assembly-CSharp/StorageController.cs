using System;

// Token: 0x0200005F RID: 95
public class StorageController : GameStateMachine<StorageController, StorageController.Instance>
{
	// Token: 0x060001C7 RID: 455 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OnStorageInteracted, this.working, null);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StorageController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StorageController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working.PlayAnim("working").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04000121 RID: 289
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000122 RID: 290
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x04000123 RID: 291
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x0200109C RID: 4252
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200109D RID: 4253
	public new class Instance : GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008284 RID: 33412 RVA: 0x00341FA2 File Offset: 0x003401A2
		public Instance(IStateMachineTarget master, StorageController.Def def) : base(master)
		{
		}
	}
}
