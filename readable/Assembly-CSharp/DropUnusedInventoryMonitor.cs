using System;

// Token: 0x02000A22 RID: 2594
public class DropUnusedInventoryMonitor : GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance>
{
	// Token: 0x06004BEA RID: 19434 RVA: 0x001B92A0 File Offset: 0x001B74A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventTransition(GameHashes.OnStorageChange, this.hasinventory, (DropUnusedInventoryMonitor.Instance smi) => smi.GetComponent<Storage>().Count > 0);
		this.hasinventory.EventTransition(GameHashes.OnStorageChange, this.hasinventory, (DropUnusedInventoryMonitor.Instance smi) => smi.GetComponent<Storage>().Count == 0).ToggleChore((DropUnusedInventoryMonitor.Instance smi) => new DropUnusedInventoryChore(Db.Get().ChoreTypes.DropUnusedInventory, smi.master), this.satisfied);
	}

	// Token: 0x04003253 RID: 12883
	public GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003254 RID: 12884
	public GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.State hasinventory;

	// Token: 0x02001AD6 RID: 6870
	public new class Instance : GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A748 RID: 42824 RVA: 0x003BBF7F File Offset: 0x003BA17F
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
