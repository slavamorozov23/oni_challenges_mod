using System;

// Token: 0x02000A58 RID: 2648
public class WoundMonitor : GameStateMachine<WoundMonitor, WoundMonitor.Instance>
{
	// Token: 0x06004D02 RID: 19714 RVA: 0x001C02E0 File Offset: 0x001BE4E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		this.root.ToggleAnims("anim_hits_kanim", 0f).EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi, object data)
		{
			smi.OnHealthChanged(data);
		});
		this.healthy.EventTransition(GameHashes.HealthChanged, this.wounded, (WoundMonitor.Instance smi) => smi.health.State > Health.HealthState.Perfect);
		this.wounded.ToggleUrge(Db.Get().Urges.Heal).Enter(delegate(WoundMonitor.Instance smi)
		{
			switch (smi.health.State)
			{
			case Health.HealthState.Scuffed:
				smi.GoTo(this.wounded.light);
				return;
			case Health.HealthState.Injured:
				smi.GoTo(this.wounded.medium);
				return;
			case Health.HealthState.Critical:
				smi.GoTo(this.wounded.heavy);
				return;
			default:
				return;
			}
		}).EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi)
		{
			smi.GoToProperHeathState();
		});
		this.wounded.medium.ToggleAnims("anim_loco_wounded_kanim", 1f);
		this.wounded.heavy.ToggleAnims("anim_loco_wounded_kanim", 3f).Update("LookForAvailableClinic", delegate(WoundMonitor.Instance smi, float dt)
		{
			smi.FindAvailableMedicalBed();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x04003351 RID: 13137
	public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04003352 RID: 13138
	public WoundMonitor.Wounded wounded;

	// Token: 0x02001B68 RID: 7016
	public class Wounded : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040084E1 RID: 34017
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State light;

		// Token: 0x040084E2 RID: 34018
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State medium;

		// Token: 0x040084E3 RID: 34019
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State heavy;
	}

	// Token: 0x02001B69 RID: 7017
	public new class Instance : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A9E0 RID: 43488 RVA: 0x003C2B18 File Offset: 0x003C0D18
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.health = master.GetComponent<Health>();
			this.worker = master.GetComponent<WorkerBase>();
		}

		// Token: 0x0600A9E1 RID: 43489 RVA: 0x003C2B3C File Offset: 0x003C0D3C
		public void OnHealthChanged(object data)
		{
			float value = ((Boxed<float>)data).value;
			if (this.health.hitPoints != 0f && value < 0f)
			{
				this.PlayHitAnimation();
			}
		}

		// Token: 0x0600A9E2 RID: 43490 RVA: 0x003C2B78 File Offset: 0x003C0D78
		private void PlayHitAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("hit"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
				if (text.Contains("impact"))
				{
					return;
				}
			}
			string s = "hit";
			AttackChore.StatesInstance smi = base.gameObject.GetSMI<AttackChore.StatesInstance>();
			if (smi != null && smi.GetCurrentState() == smi.sm.attack)
			{
				s = smi.master.GetHitAnim();
			}
			if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				s = "hit_ladder";
			}
			else if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Pole)
			{
				s = "hit_pole";
			}
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x0600A9E3 RID: 43491 RVA: 0x003C2CFC File Offset: 0x003C0EFC
		public void PlayKnockedOverImpactAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("impact"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
			}
			string s = "impact";
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x0600A9E4 RID: 43492 RVA: 0x003C2E0C File Offset: 0x003C100C
		public void GoToProperHeathState()
		{
			switch (base.smi.health.State)
			{
			case Health.HealthState.Perfect:
				base.smi.GoTo(base.sm.healthy);
				return;
			case Health.HealthState.Alright:
				break;
			case Health.HealthState.Scuffed:
				base.smi.GoTo(base.sm.wounded.light);
				break;
			case Health.HealthState.Injured:
				base.smi.GoTo(base.sm.wounded.medium);
				return;
			case Health.HealthState.Critical:
				base.smi.GoTo(base.sm.wounded.heavy);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600A9E5 RID: 43493 RVA: 0x003C2EAF File Offset: 0x003C10AF
		public bool ShouldExitInfirmary()
		{
			return this.health.State == Health.HealthState.Perfect;
		}

		// Token: 0x0600A9E6 RID: 43494 RVA: 0x003C2EC4 File Offset: 0x003C10C4
		public void FindAvailableMedicalBed()
		{
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			Ownables soleOwner = base.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
			if (soleOwner.GetSlot(clinic).assignable == null)
			{
				soleOwner.AutoAssignSlot(clinic);
			}
		}

		// Token: 0x040084E4 RID: 34020
		public Health health;

		// Token: 0x040084E5 RID: 34021
		private WorkerBase worker;
	}
}
