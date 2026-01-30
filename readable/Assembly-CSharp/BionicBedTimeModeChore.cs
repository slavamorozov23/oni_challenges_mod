using System;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class BionicBedTimeModeChore : Chore<BionicBedTimeModeChore.Instance>
{
	// Token: 0x060018EC RID: 6380 RVA: 0x0008A56C File Offset: 0x0008876C
	public BionicBedTimeModeChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BionicBedtimeMode, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicBedTimeModeChore.Instance(this, master.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x0008A5C4 File Offset: 0x000887C4
	public static void BeginWorkOnZone(BionicBedTimeModeChore.Instance smi)
	{
		WorkerBase workerBase = smi.sm.bionic.Get<WorkerBase>(smi);
		DefragmentationZone assignedDefragmentationZone = smi.GetAssignedDefragmentationZone();
		workerBase.StartWork(new WorkerBase.StartWorkInfo(assignedDefragmentationZone));
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x0008A5F4 File Offset: 0x000887F4
	public static bool HasDefragmentationZoneAssignedAndReachable(BionicBedTimeModeChore.Instance smi, GameObject defragmentationZone)
	{
		return defragmentationZone != null && smi.IsDefragmentationZoneReachable();
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x0008A607 File Offset: 0x00088807
	public static bool HasDefragmentationZoneAssignedAndReachable(BionicBedTimeModeChore.Instance smi)
	{
		return smi.sm.defragmentationZone.Get(smi) != null && smi.IsDefragmentationZoneReachable();
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x0008A62A File Offset: 0x0008882A
	public static bool IsBedTimeAllowed(BionicBedTimeModeChore.Instance smi)
	{
		return BionicBedTimeMonitor.CanGoToBedTime(smi.bedTimeMonitor);
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0008A637 File Offset: 0x00088837
	public static void UpdateAssignedDefragmentationZone(BionicBedTimeModeChore.Instance smi)
	{
		smi.UpdateAssignedDefragmentationZone(null);
	}

	// Token: 0x04000E64 RID: 3684
	public const string EFFECT_NAME = "BionicBedTimeEffect";

	// Token: 0x020012C2 RID: 4802
	public class States : GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore>
	{
		// Token: 0x06008955 RID: 35157 RVA: 0x00351B00 File Offset: 0x0034FD00
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			this.root.ToggleEffect("BionicBedTimeEffect");
			this.enter.Transition(null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed)), UpdateRate.SIM_200ms).ParamTransition<GameObject>(this.defragmentationZone, this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>.Callback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).GoTo(this.defragmentingWithoutAssignable);
			this.unassigning.ScheduleActionNextFrame("Frame delay on unassign", delegate(BionicBedTimeModeChore.Instance smi)
			{
				BionicBedTimeModeChore.UpdateAssignedDefragmentationZone(smi);
				smi.GoTo(this.enter);
			});
			this.approach.InitializeStates(this.bionic, this.defragmentationZone, this.defragmentingOnAssignable, null, null, null).OnSignal(this.defragmentationZoneUnassignined, this.unassigning).ScheduleChange(null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).EventTransition(GameHashes.BionicOffline, null, null);
			this.defragmentingOnAssignable.OnTargetLost(this.defragmentationZone, this.defragmentingWithoutAssignable).OnSignal(this.defragmentationZoneChangedSignal, this.enter).OnSignal(this.defragmentationZoneUnassignined, this.unassigning).EventTransition(GameHashes.BionicOffline, null, null).ScheduleChange(null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).ToggleWork("Defragmenting", new Action<BionicBedTimeModeChore.Instance>(BionicBedTimeModeChore.BeginWorkOnZone), (BionicBedTimeModeChore.Instance smi) => smi.GetAssignedDefragmentationZone() != null, this.end, null).ToggleTag(GameTags.BionicBedTime);
			this.defragmentingWithoutAssignable.ParamTransition<GameObject>(this.defragmentationZone, this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>.Callback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).EventTransition(GameHashes.AssignableReachabilityChanged, this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).ToggleAnims("anim_bionic_kanim", 0f).ToggleTag(GameTags.BionicBedTime).DefaultState(this.defragmentingWithoutAssignable.pre);
			this.defragmentingWithoutAssignable.pre.PlayAnim("low_power_pre").OnAnimQueueComplete(this.defragmentingWithoutAssignable.loop).ScheduleGoTo(1.5f, this.defragmentingWithoutAssignable.loop);
			this.defragmentingWithoutAssignable.loop.ScheduleChange(this.defragmentingWithoutAssignable.pst, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).EventTransition(GameHashes.BionicOffline, this.defragmentingWithoutAssignable.pst, null).PlayAnim("low_power_loop", KAnim.PlayMode.Loop);
			this.defragmentingWithoutAssignable.pst.PlayAnim("low_power_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.end);
			this.end.ReturnSuccess();
		}

		// Token: 0x040068CF RID: 26831
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040068D0 RID: 26832
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State defragmentingOnAssignable;

		// Token: 0x040068D1 RID: 26833
		public BionicBedTimeModeChore.States.DefragmentingStates defragmentingWithoutAssignable;

		// Token: 0x040068D2 RID: 26834
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State enter;

		// Token: 0x040068D3 RID: 26835
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State end;

		// Token: 0x040068D4 RID: 26836
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State unassigning;

		// Token: 0x040068D5 RID: 26837
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.TargetParameter bionic;

		// Token: 0x040068D6 RID: 26838
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.TargetParameter defragmentationZone;

		// Token: 0x040068D7 RID: 26839
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Signal defragmentationZoneChangedSignal;

		// Token: 0x040068D8 RID: 26840
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Signal defragmentationZoneUnassignined;

		// Token: 0x0200279D RID: 10141
		public class DefragmentingStates : GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State
		{
			// Token: 0x0400AFB5 RID: 44981
			public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State pre;

			// Token: 0x0400AFB6 RID: 44982
			public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State loop;

			// Token: 0x0400AFB7 RID: 44983
			public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State pst;
		}
	}

	// Token: 0x020012C3 RID: 4803
	public class Instance : GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.GameInstance
	{
		// Token: 0x06008958 RID: 35160 RVA: 0x00351DBE File Offset: 0x0034FFBE
		public DefragmentationZone GetAssignedDefragmentationZone()
		{
			return this.lastAssignedDefragmentationZone;
		}

		// Token: 0x06008959 RID: 35161 RVA: 0x00351DC8 File Offset: 0x0034FFC8
		public Instance(BionicBedTimeModeChore master, GameObject duplicant) : base(master)
		{
			this.bedTimeMonitor = duplicant.GetSMI<BionicBedTimeMonitor.Instance>();
			base.sm.bionic.Set(duplicant, this, false);
			this.ownables = base.GetComponent<MinionIdentity>().GetSoleOwner();
			base.gameObject.Subscribe(-1585839766, new Action<object>(this.UpdateAssignedDefragmentationZone));
			this.UpdateAssignedDefragmentationZone(null);
		}

		// Token: 0x0600895A RID: 35162 RVA: 0x00351E31 File Offset: 0x00350031
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1585839766, new Action<object>(this.UpdateAssignedDefragmentationZone));
			base.OnCleanUp();
		}

		// Token: 0x0600895B RID: 35163 RVA: 0x00351E55 File Offset: 0x00350055
		public override void StartSM()
		{
			this.UpdateAssignedDefragmentationZone(null);
			base.StartSM();
		}

		// Token: 0x0600895C RID: 35164 RVA: 0x00351E64 File Offset: 0x00350064
		public bool IsDefragmentationZoneReachable()
		{
			return base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>().IsReachable(Db.Get().AssignableSlots.Bed);
		}

		// Token: 0x0600895D RID: 35165 RVA: 0x00351E88 File Offset: 0x00350088
		public void UpdateAssignedDefragmentationZone(object slotInstanceObject)
		{
			DefragmentationZone y = null;
			AssignableSlotInstance assignableSlotInstance = (slotInstanceObject == null) ? null : ((AssignableSlotInstance)slotInstanceObject);
			Assignable assignable = this.ownables.GetAssignable(Db.Get().AssignableSlots.Bed);
			if (assignableSlotInstance != null && assignableSlotInstance.IsUnassigning())
			{
				base.sm.defragmentationZoneUnassignined.Trigger(this);
				return;
			}
			if (assignable == null)
			{
				assignable = this.ownables.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
			}
			if (assignable != null)
			{
				y = assignable.GetComponent<DefragmentationZone>();
			}
			if (this.lastAssignedDefragmentationZone != y)
			{
				AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
				if (sensor.IsEnabled)
				{
					sensor.Update();
				}
				this.lastAssignedDefragmentationZone = y;
				base.sm.defragmentationZone.Set(this.lastAssignedDefragmentationZone, this);
				base.sm.defragmentationZoneChangedSignal.Trigger(this);
			}
		}

		// Token: 0x040068D9 RID: 26841
		public BionicBedTimeMonitor.Instance bedTimeMonitor;

		// Token: 0x040068DA RID: 26842
		private DefragmentationZone lastAssignedDefragmentationZone;

		// Token: 0x040068DB RID: 26843
		private Ownables ownables;
	}
}
