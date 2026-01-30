using System;

// Token: 0x02000B14 RID: 2836
public class StorageUnloadMonitor : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>
{
	// Token: 0x060052A6 RID: 21158 RVA: 0x001E0F30 File Offset: 0x001DF130
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notFull;
		this.notFull.Transition(this.full, new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload), UpdateRate.SIM_200ms);
		this.full.ToggleStatusItem(Db.Get().RobotStatusItems.DustBinFull, (StorageUnloadMonitor.Instance smi) => smi.gameObject).ToggleBehaviour(GameTags.Robots.Behaviours.UnloadBehaviour, (StorageUnloadMonitor.Instance data) => true, null).Transition(this.notFull, GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Not(new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload)), UpdateRate.SIM_1000ms).Enter(delegate(StorageUnloadMonitor.Instance smi)
		{
			if (smi.master.gameObject.GetComponents<Storage>()[1].RemainingCapacity() <= 0f)
			{
				smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_full");
			}
		});
	}

	// Token: 0x060052A7 RID: 21159 RVA: 0x001E100C File Offset: 0x001DF20C
	public static bool WantsToUnload(StorageUnloadMonitor.Instance smi)
	{
		Storage storage = smi.sm.sweepLocker.Get(smi);
		return !(storage == null) && !(smi.sm.internalStorage.Get(smi) == null) && !smi.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && (smi.sm.internalStorage.Get(smi).IsFull() || (storage != null && !smi.sm.internalStorage.Get(smi).IsEmpty() && Grid.PosToCell(storage) == Grid.PosToCell(smi)));
	}

	// Token: 0x040037D5 RID: 14293
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage>();

	// Token: 0x040037D6 RID: 14294
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> sweepLocker;

	// Token: 0x040037D7 RID: 14295
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State notFull;

	// Token: 0x040037D8 RID: 14296
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State full;

	// Token: 0x02001C55 RID: 7253
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001C56 RID: 7254
	public new class Instance : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.GameInstance
	{
		// Token: 0x0600AD26 RID: 44326 RVA: 0x003CEC8E File Offset: 0x003CCE8E
		public Instance(IStateMachineTarget master, StorageUnloadMonitor.Def def) : base(master, def)
		{
		}
	}
}
