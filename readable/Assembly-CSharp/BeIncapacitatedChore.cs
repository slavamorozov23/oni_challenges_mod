using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public class BeIncapacitatedChore : Chore<BeIncapacitatedChore.StatesInstance>
{
	// Token: 0x060018E2 RID: 6370 RVA: 0x0008A270 File Offset: 0x00088470
	public void FindAvailableMedicalBed(Navigator navigator)
	{
		Clinic clinic = null;
		AssignableSlot clinic2 = Db.Get().AssignableSlots.Clinic;
		Ownables soleOwner = this.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
		AssignableSlotInstance slot = soleOwner.GetSlot(clinic2);
		if (slot.assignable == null)
		{
			Assignable assignable = soleOwner.AutoAssignSlot(clinic2);
			if (assignable != null)
			{
				clinic = assignable.GetComponent<Clinic>();
			}
		}
		else
		{
			clinic = slot.assignable.GetComponent<Clinic>();
		}
		if (clinic != null && navigator.CanReach(clinic))
		{
			base.smi.sm.clinic.Set(clinic.gameObject, base.smi, false);
			base.smi.GoTo(base.smi.sm.incapacitation_root.rescue.waitingForPickup);
		}
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x0008A339 File Offset: 0x00088539
	public GameObject GetChosenClinic()
	{
		return base.smi.sm.clinic.Get(base.smi);
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x0008A358 File Offset: 0x00088558
	public BeIncapacitatedChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeIncapacitated, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeIncapacitatedChore.StatesInstance(this);
	}

	// Token: 0x04000E5D RID: 3677
	private static string IncapacitatedDuplicantAnim_pre = "incapacitate_pre";

	// Token: 0x04000E5E RID: 3678
	private static string IncapacitatedDuplicantAnim_loop = "incapacitate_loop";

	// Token: 0x04000E5F RID: 3679
	private static string IncapacitatedDuplicantAnim_death = "incapacitate_death";

	// Token: 0x04000E60 RID: 3680
	private static string IncapacitatedDuplicantAnim_carry = "carry_loop";

	// Token: 0x04000E61 RID: 3681
	private static string IncapacitatedDuplicantAnim_place = "place";

	// Token: 0x020012BC RID: 4796
	public class StatesInstance : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.GameInstance
	{
		// Token: 0x06008943 RID: 35139 RVA: 0x0035124C File Offset: 0x0034F44C
		public StatesInstance(BeIncapacitatedChore master) : base(master)
		{
		}
	}

	// Token: 0x020012BD RID: 4797
	public class States : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore>
	{
		// Token: 0x06008944 RID: 35140 RVA: 0x00351258 File Offset: 0x0034F458
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAnims("anim_incapacitated_kanim", 0f).ToggleStatusItem(Db.Get().DuplicantStatusItems.Incapacitated, (BeIncapacitatedChore.StatesInstance smi) => smi.master.gameObject.GetSMI<IncapacitationMonitor.Instance>()).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.GoTo(this.incapacitation_root.lookingForBed);
			});
			this.incapacitation_root.EventHandler(GameHashes.Died, delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("died");
			});
			this.incapacitation_root.lookingForBed.Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				smi.master.FindAvailableMedicalBed(smi.master.GetComponent<Navigator>());
			}, UpdateRate.SIM_1000ms, false).Enter("PlayAnim", delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.sm.clinic.Set(null, smi);
				smi.Play(BeIncapacitatedChore.IncapacitatedDuplicantAnim_pre, KAnim.PlayMode.Once);
				smi.Queue(BeIncapacitatedChore.IncapacitatedDuplicantAnim_loop, KAnim.PlayMode.Loop);
			});
			this.incapacitation_root.rescue.ToggleChore((BeIncapacitatedChore.StatesInstance smi) => new RescueIncapacitatedChore(smi.master, this.masterTarget.Get(smi)), this.incapacitation_root.recovering, this.incapacitation_root.lookingForBed);
			this.incapacitation_root.rescue.waitingForPickup.EventTransition(GameHashes.OnStore, this.incapacitation_root.rescue.carried, null).Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				bool flag = false;
				if (smi.sm.clinic.Get(smi) == null)
				{
					flag = true;
				}
				else if (!smi.master.gameObject.GetComponent<Navigator>().CanReach(this.clinic.Get(smi).GetComponent<Clinic>()))
				{
					flag = true;
				}
				else if (!this.clinic.Get(smi).GetComponent<Assignable>().IsAssignedTo(smi.master.GetComponent<IAssignableIdentity>()))
				{
					flag = true;
				}
				if (flag)
				{
					smi.GoTo(this.incapacitation_root.lookingForBed);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.incapacitation_root.rescue.carried.Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				bool flag = false;
				if (smi.sm.clinic.Get(smi) == null)
				{
					flag = true;
				}
				else if (!this.clinic.Get(smi).GetComponent<Assignable>().IsAssignedTo(smi.master.GetComponent<IAssignableIdentity>()))
				{
					flag = true;
				}
				if (flag)
				{
					smi.GoTo(this.incapacitation_root.lookingForBed);
				}
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Queue(BeIncapacitatedChore.IncapacitatedDuplicantAnim_carry, KAnim.PlayMode.Loop);
			}).Exit(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Play(BeIncapacitatedChore.IncapacitatedDuplicantAnim_place, KAnim.PlayMode.Once);
			});
			this.incapacitation_root.death.PlayAnim(BeIncapacitatedChore.IncapacitatedDuplicantAnim_death).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("died");
			});
			this.incapacitation_root.recovering.ToggleUrge(Db.Get().Urges.HealCritical).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Trigger(-1256572400, null);
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("recovering");
			});
		}

		// Token: 0x040068BE RID: 26814
		public BeIncapacitatedChore.States.IncapacitatedStates incapacitation_root;

		// Token: 0x040068BF RID: 26815
		public StateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.TargetParameter clinic;

		// Token: 0x02002798 RID: 10136
		public class IncapacitatedStates : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State
		{
			// Token: 0x0400AFA1 RID: 44961
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State lookingForBed;

			// Token: 0x0400AFA2 RID: 44962
			public BeIncapacitatedChore.States.BeingRescued rescue;

			// Token: 0x0400AFA3 RID: 44963
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State death;

			// Token: 0x0400AFA4 RID: 44964
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State recovering;
		}

		// Token: 0x02002799 RID: 10137
		public class BeingRescued : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State
		{
			// Token: 0x0400AFA5 RID: 44965
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State waitingForPickup;

			// Token: 0x0400AFA6 RID: 44966
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State carried;
		}
	}
}
