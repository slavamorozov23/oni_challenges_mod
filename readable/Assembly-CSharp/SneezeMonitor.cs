using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A48 RID: 2632
public class SneezeMonitor : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance>
{
	// Token: 0x06004CB8 RID: 19640 RVA: 0x001BE15C File Offset: 0x001BC35C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.ParamTransition<bool>(this.isSneezy, this.sneezy, (SneezeMonitor.Instance smi, bool p) => p);
		this.sneezy.ParamTransition<bool>(this.isSneezy, this.idle, (SneezeMonitor.Instance smi, bool p) => !p).ToggleReactable((SneezeMonitor.Instance smi) => smi.GetReactable());
	}

	// Token: 0x0400330E RID: 13070
	public StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSneezy = new StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);

	// Token: 0x0400330F RID: 13071
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04003310 RID: 13072
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State taking_medicine;

	// Token: 0x04003311 RID: 13073
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State sneezy;

	// Token: 0x04003312 RID: 13074
	public const float SINGLE_SNEEZE_TIME_MINOR = 140f;

	// Token: 0x04003313 RID: 13075
	public const float SINGLE_SNEEZE_TIME_MAJOR = 70f;

	// Token: 0x04003314 RID: 13076
	public const float SNEEZE_TIME_VARIANCE = 0.3f;

	// Token: 0x04003315 RID: 13077
	public const float SHORT_SNEEZE_THRESHOLD = 5f;

	// Token: 0x02001B39 RID: 6969
	public new class Instance : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A8ED RID: 43245 RVA: 0x003C03EC File Offset: 0x003BE5EC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sneezyness = Db.Get().Attributes.Sneezyness.Lookup(master.gameObject);
			this.OnSneezyChange();
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
		}

		// Token: 0x0600A8EE RID: 43246 RVA: 0x003C044D File Offset: 0x003BE64D
		public override void StopSM(string reason)
		{
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
			base.StopSM(reason);
		}

		// Token: 0x0600A8EF RID: 43247 RVA: 0x003C0480 File Offset: 0x003BE680
		public float NextSneezeInterval()
		{
			if (this.sneezyness.GetTotalValue() <= 0f)
			{
				return 70f;
			}
			float num = (this.IsMinorSneeze() ? 140f : 70f) / this.sneezyness.GetTotalValue();
			return UnityEngine.Random.Range(num * 0.7f, num * 1.3f);
		}

		// Token: 0x0600A8F0 RID: 43248 RVA: 0x003C04D9 File Offset: 0x003BE6D9
		public bool IsMinorSneeze()
		{
			return this.sneezyness.GetTotalValue() <= 5f;
		}

		// Token: 0x0600A8F1 RID: 43249 RVA: 0x003C04F0 File Offset: 0x003BE6F0
		private void OnSneezyChange()
		{
			base.smi.sm.isSneezy.Set(this.sneezyness.GetTotalValue() > 0f, base.smi, false);
		}

		// Token: 0x0600A8F2 RID: 43250 RVA: 0x003C0524 File Offset: 0x003BE724
		public Reactable GetReactable()
		{
			float localCooldown = this.NextSneezeInterval();
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Sneeze", Db.Get().ChoreTypes.Cough, 0f, localCooldown, float.PositiveInfinity, 0f);
			string s = "sneeze";
			string s2 = "sneeze_pst";
			Emote emote = Db.Get().Emotes.Minion.Sneeze;
			if (this.IsMinorSneeze())
			{
				s = "sneeze_short";
				s2 = "sneeze_short_pst";
				emote = Db.Get().Emotes.Minion.Sneeze_Short;
			}
			selfEmoteReactable.SetEmote(emote);
			return selfEmoteReactable.RegisterEmoteStepCallbacks(s, new Action<GameObject>(this.TriggerDisurbance), null).RegisterEmoteStepCallbacks(s2, null, new Action<GameObject>(this.ResetSneeze));
		}

		// Token: 0x0600A8F3 RID: 43251 RVA: 0x003C05F3 File Offset: 0x003BE7F3
		private void TriggerDisurbance(GameObject go)
		{
			if (this.IsMinorSneeze())
			{
				AcousticDisturbance.Emit(go, 2);
				return;
			}
			AcousticDisturbance.Emit(go, 3);
		}

		// Token: 0x0600A8F4 RID: 43252 RVA: 0x003C060C File Offset: 0x003BE80C
		private void ResetSneeze(GameObject go)
		{
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x04008420 RID: 33824
		private AttributeInstance sneezyness;

		// Token: 0x04008421 RID: 33825
		private StatusItem statusItem;
	}
}
