using System;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class DeliverToSweepLockerStates : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>
{
	// Token: 0x06001484 RID: 5252 RVA: 0x000748BC File Offset: 0x00072ABC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.movingToStorage;
		this.idle.ScheduleGoTo(1f, this.movingToStorage);
		this.movingToStorage.MoveTo(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			if (!(this.GetSweepLocker(smi) == null))
			{
				return Grid.PosToCell(this.GetSweepLocker(smi));
			}
			return Grid.InvalidCell;
		}, this.unloading, this.idle, false);
		this.unloading.Enter(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			Storage storage = smi.master.gameObject.GetComponents<Storage>()[1];
			float num = Mathf.Max(0f, Mathf.Min(storage.ExactMassStored(), sweepLocker.RemainingCapacity()));
			for (int i = storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = storage.items[i];
				if (!(gameObject == null))
				{
					float num2 = Mathf.Min(gameObject.GetComponent<PrimaryElement>().Mass, num);
					if (num2 != 0f)
					{
						storage.Transfer(sweepLocker, gameObject.GetComponent<KPrefabID>().PrefabTag, num2, false, false);
					}
					num -= num2;
					if (num <= 0f)
					{
						break;
					}
				}
			}
			smi.master.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			smi.master.GetComponent<KBatchedAnimController>().FlipX = false;
			sweepLocker.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			if (storage.MassStored() > 0f)
			{
				smi.ScheduleGoTo(2f, this.lockerFull);
				return;
			}
			smi.ScheduleGoTo(2f, this.behaviourcomplete);
		});
		this.lockerFull.PlayAnim("react_bored", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToStorage);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.UnloadBehaviour, false);
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x00074954 File Offset: 0x00072B54
	public Storage GetSweepLocker(DeliverToSweepLockerStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x04000C67 RID: 3175
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State idle;

	// Token: 0x04000C68 RID: 3176
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State movingToStorage;

	// Token: 0x04000C69 RID: 3177
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State unloading;

	// Token: 0x04000C6A RID: 3178
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State lockerFull;

	// Token: 0x04000C6B RID: 3179
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State behaviourcomplete;

	// Token: 0x02001259 RID: 4697
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200125A RID: 4698
	public new class Instance : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.GameInstance
	{
		// Token: 0x060087C5 RID: 34757 RVA: 0x0034C773 File Offset: 0x0034A973
		public Instance(Chore<DeliverToSweepLockerStates.Instance> chore, DeliverToSweepLockerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.UnloadBehaviour);
		}

		// Token: 0x060087C6 RID: 34758 RVA: 0x0034C797 File Offset: 0x0034A997
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.UnloadingStorage, base.gameObject);
		}

		// Token: 0x060087C7 RID: 34759 RVA: 0x0034C7CF File Offset: 0x0034A9CF
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.UnloadingStorage, false);
		}
	}
}
