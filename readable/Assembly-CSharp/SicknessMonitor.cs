using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A45 RID: 2629
public class SicknessMonitor : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance>
{
	// Token: 0x06004CA7 RID: 19623 RVA: 0x001BD890 File Offset: 0x001BBA90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.healthy;
		this.healthy.EventTransition(GameHashes.SicknessAdded, this.sick, (SicknessMonitor.Instance smi) => smi.IsSick());
		this.sick.DefaultState(this.sick.minor).EventTransition(GameHashes.SicknessCured, this.post_nocheer, (SicknessMonitor.Instance smi) => !smi.IsSick()).ToggleThought(Db.Get().Thoughts.GotInfected, null);
		this.sick.minor.EventTransition(GameHashes.SicknessAdded, this.sick.major, (SicknessMonitor.Instance smi) => smi.HasMajorDisease());
		this.sick.major.EventTransition(GameHashes.SicknessCured, this.sick.minor, (SicknessMonitor.Instance smi) => !smi.HasMajorDisease()).ToggleUrge(Db.Get().Urges.RestDueToDisease).Update("AutoAssignClinic", delegate(SicknessMonitor.Instance smi, float dt)
		{
			smi.AutoAssignClinic();
		}, UpdateRate.SIM_4000ms, false).Exit(delegate(SicknessMonitor.Instance smi)
		{
			smi.UnassignClinic();
		});
		this.post_nocheer.Enter(delegate(SicknessMonitor.Instance smi)
		{
			new SicknessCuredFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f)).StartSM();
			if (smi.IsSleepingOrSleepSchedule())
			{
				smi.GoTo(this.healthy);
				return;
			}
			smi.GoTo(this.post);
		});
		this.post.ToggleChore((SicknessMonitor.Instance smi) => new EmoteChore(smi.master, Db.Get().ChoreTypes.EmoteHighPriority, SicknessMonitor.SickPostKAnim, SicknessMonitor.SickPostAnims, KAnim.PlayMode.Once, false), this.healthy);
	}

	// Token: 0x040032FB RID: 13051
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x040032FC RID: 13052
	public SicknessMonitor.SickStates sick;

	// Token: 0x040032FD RID: 13053
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post;

	// Token: 0x040032FE RID: 13054
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post_nocheer;

	// Token: 0x040032FF RID: 13055
	private static readonly HashedString SickPostKAnim = "anim_cheer_kanim";

	// Token: 0x04003300 RID: 13056
	private static readonly HashedString[] SickPostAnims = new HashedString[]
	{
		"cheer_pre",
		"cheer_loop",
		"cheer_pst"
	};

	// Token: 0x02001B2F RID: 6959
	public class SickStates : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04008400 RID: 33792
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State minor;

		// Token: 0x04008401 RID: 33793
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State major;
	}

	// Token: 0x02001B30 RID: 6960
	public new class Instance : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A8BF RID: 43199 RVA: 0x003BFC1D File Offset: 0x003BDE1D
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
		}

		// Token: 0x0600A8C0 RID: 43200 RVA: 0x003BFC37 File Offset: 0x003BDE37
		private string OnGetToolTip(List<Notification> notifications, object data)
		{
			return DUPLICANTS.STATUSITEMS.HASDISEASE.TOOLTIP;
		}

		// Token: 0x0600A8C1 RID: 43201 RVA: 0x003BFC43 File Offset: 0x003BDE43
		public bool IsSick()
		{
			return this.sicknesses.Count > 0;
		}

		// Token: 0x0600A8C2 RID: 43202 RVA: 0x003BFC54 File Offset: 0x003BDE54
		public bool HasMajorDisease()
		{
			using (IEnumerator<SicknessInstance> enumerator = this.sicknesses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.modifier.severity >= Sickness.Severity.Major)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600A8C3 RID: 43203 RVA: 0x003BFCB0 File Offset: 0x003BDEB0
		public void AutoAssignClinic()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot == null)
			{
				return;
			}
			if (slot.assignable != null)
			{
				return;
			}
			soleOwner.AutoAssignSlot(clinic);
		}

		// Token: 0x0600A8C4 RID: 43204 RVA: 0x003BFD14 File Offset: 0x003BDF14
		public void UnassignClinic()
		{
			Assignables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot != null)
			{
				slot.Unassign(true);
			}
		}

		// Token: 0x0600A8C5 RID: 43205 RVA: 0x003BFD64 File Offset: 0x003BDF64
		public bool IsSleepingOrSleepSchedule()
		{
			Schedulable component = base.GetComponent<Schedulable>();
			if (component != null && component.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return true;
			}
			KPrefabID component2 = base.GetComponent<KPrefabID>();
			return component2 != null && component2.HasTag(GameTags.Asleep);
		}

		// Token: 0x04008402 RID: 33794
		private Sicknesses sicknesses;
	}
}
