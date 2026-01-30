using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A46 RID: 2630
public class SleepChoreMonitor : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance>
{
	// Token: 0x06004CAB RID: 19627 RVA: 0x001BDB24 File Offset: 0x001BBD24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.AssignablesChanged, delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
		});
		this.satisfied.EventTransition(GameHashes.AddUrge, this.checkforbed, (SleepChoreMonitor.Instance smi) => smi.HasSleepUrge());
		this.checkforbed.Enter("SetBed", delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
			if (smi.GetSMI<StaminaMonitor.Instance>().NeedsToSleep())
			{
				if (this.bed.Get(smi) != null && smi.IsBedReachable())
				{
					smi.GoTo(this.passingout_bedassigned);
					return;
				}
				smi.GoTo(this.passingout);
				return;
			}
			else
			{
				if (this.bed.Get(smi) == null || !smi.IsBedReachable())
				{
					smi.GoTo(this.sleeponfloor);
					return;
				}
				smi.GoTo(this.bedassigned);
				return;
			}
		});
		this.passingout.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventHandlerTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi, object data) => smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreatePassingOutChore), this.satisfied, this.satisfied);
		this.passingout_bedassigned.ParamTransition<GameObject>(this.bed, this.checkforbed, (SleepChoreMonitor.Instance smi, GameObject p) => p == null).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi) => !smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateExhaustedSleepChore), this.satisfied, this.satisfied);
		this.sleeponfloor.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventHandlerTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi, object data) => smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepOnFloorChore), this.satisfied, this.satisfied);
		this.bedassigned.ParamTransition<GameObject>(this.bed, this.checkforbed, (SleepChoreMonitor.Instance smi, GameObject p) => p == null).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi) => !smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepChore), this.satisfied, this.satisfied);
	}

	// Token: 0x06004CAC RID: 19628 RVA: 0x001BDDB4 File Offset: 0x001BBFB4
	private Chore CreatePassingOutChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreatePassedOutLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, false);
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x001BDDE8 File Offset: 0x001BBFE8
	private Chore CreateSleepOnFloorChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreateFloorLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, true);
	}

	// Token: 0x06004CAE RID: 19630 RVA: 0x001BDE19 File Offset: 0x001BC019
	private Chore CreateSleepChore(SleepChoreMonitor.Instance smi)
	{
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true);
	}

	// Token: 0x06004CAF RID: 19631 RVA: 0x001BDE44 File Offset: 0x001BC044
	private Chore CreateExhaustedSleepChore(SleepChoreMonitor.Instance smi)
	{
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true, new StatusItem[]
		{
			Db.Get().DuplicantStatusItems.SleepingExhausted
		});
	}

	// Token: 0x04003301 RID: 13057
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003302 RID: 13058
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State checkforbed;

	// Token: 0x04003303 RID: 13059
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout;

	// Token: 0x04003304 RID: 13060
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout_bedassigned;

	// Token: 0x04003305 RID: 13061
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State sleeponfloor;

	// Token: 0x04003306 RID: 13062
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State bedassigned;

	// Token: 0x04003307 RID: 13063
	public StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.TargetParameter bed;

	// Token: 0x02001B32 RID: 6962
	public new class Instance : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A8CF RID: 43215 RVA: 0x003BFE2C File Offset: 0x003BE02C
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A8D0 RID: 43216 RVA: 0x003BFE38 File Offset: 0x003BE038
		public void UpdateBed()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			Assignable assignable = soleOwner.GetAssignable(Db.Get().AssignableSlots.MedicalBed);
			Assignable assignable2;
			if (assignable != null && assignable.CanAutoAssignTo(base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().assignableProxy.Get()))
			{
				assignable2 = assignable;
			}
			else
			{
				assignable2 = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);
				if (assignable2 == null)
				{
					assignable2 = soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
					if (assignable2 != null)
					{
						AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
						if (sensor.IsEnabled)
						{
							sensor.Update();
						}
					}
				}
			}
			base.smi.sm.bed.Set(assignable2, base.smi);
		}

		// Token: 0x0600A8D1 RID: 43217 RVA: 0x003BFF2C File Offset: 0x003BE12C
		public bool HasSleepUrge()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x0600A8D2 RID: 43218 RVA: 0x003BFF48 File Offset: 0x003BE148
		public bool IsBedReachable()
		{
			AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
			return sensor.IsReachable(Db.Get().AssignableSlots.Bed) || sensor.IsReachable(Db.Get().AssignableSlots.MedicalBed);
		}

		// Token: 0x0600A8D3 RID: 43219 RVA: 0x003BFF8F File Offset: 0x003BE18F
		public GameObject CreatePassedOutLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "PassedOutSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x0600A8D4 RID: 43220 RVA: 0x003BFFCE File Offset: 0x003BE1CE
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "FloorSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x0400840B RID: 33803
		private int locatorCell;

		// Token: 0x0400840C RID: 33804
		public GameObject locator;
	}
}
