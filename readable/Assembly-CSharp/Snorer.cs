using System;
using UnityEngine;

// Token: 0x02000B55 RID: 2901
[SkipSaveFileSerialization]
public class Snorer : StateMachineComponent<Snorer.StatesInstance>
{
	// Token: 0x060055B7 RID: 21943 RVA: 0x001F3F4A File Offset: 0x001F214A
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x040039DB RID: 14811
	private static readonly HashedString HeadHash = "snapTo_mouth";

	// Token: 0x02001CBB RID: 7355
	public class StatesInstance : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.GameInstance
	{
		// Token: 0x0600AE6A RID: 44650 RVA: 0x003D354D File Offset: 0x003D174D
		public StatesInstance(Snorer master) : base(master)
		{
		}

		// Token: 0x0600AE6B RID: 44651 RVA: 0x003D3558 File Offset: 0x003D1758
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x0600AE6C RID: 44652 RVA: 0x003D357C File Offset: 0x003D177C
		public void StartSmallSnore()
		{
			this.snoreHandle = GameScheduler.Instance.Schedule("snorelines", 2f, new Action<object>(this.StartSmallSnoreInternal), null, null);
		}

		// Token: 0x0600AE6D RID: 44653 RVA: 0x003D35A8 File Offset: 0x003D17A8
		private void StartSmallSnoreInternal(object data)
		{
			this.snoreHandle.ClearScheduler();
			bool flag;
			Matrix4x4 symbolTransform = base.smi.master.GetComponent<KBatchedAnimController>().GetSymbolTransform(Snorer.HeadHash, out flag);
			if (flag)
			{
				Vector3 position = symbolTransform.GetColumn(3);
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				this.snoreEffect = FXHelpers.CreateEffect("snore_fx_kanim", position, null, false, Grid.SceneLayer.Front, false);
				this.snoreEffect.destroyOnAnimComplete = true;
				this.snoreEffect.Play("snore", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x0600AE6E RID: 44654 RVA: 0x003D363E File Offset: 0x003D183E
		public void StopSmallSnore()
		{
			this.snoreHandle.ClearScheduler();
			if (this.snoreEffect != null)
			{
				this.snoreEffect.PlayMode = KAnim.PlayMode.Once;
			}
			this.snoreEffect = null;
		}

		// Token: 0x0600AE6F RID: 44655 RVA: 0x003D366C File Offset: 0x003D186C
		public void StartSnoreBGEffect()
		{
			AcousticDisturbance.Emit(base.smi.master.gameObject, 3);
		}

		// Token: 0x0600AE70 RID: 44656 RVA: 0x003D3684 File Offset: 0x003D1884
		public void StopSnoreBGEffect()
		{
		}

		// Token: 0x0400890C RID: 35084
		private SchedulerHandle snoreHandle;

		// Token: 0x0400890D RID: 35085
		private KBatchedAnimController snoreEffect;

		// Token: 0x0400890E RID: 35086
		private KBatchedAnimController snoreBGEffect;

		// Token: 0x0400890F RID: 35087
		private const float BGEmissionRadius = 3f;
	}

	// Token: 0x02001CBC RID: 7356
	public class States : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer>
	{
		// Token: 0x0600AE71 RID: 44657 RVA: 0x003D3688 File Offset: 0x003D1888
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.sleeping, (Snorer.StatesInstance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.DefaultState(this.sleeping.quiet).Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSmallSnore();
			}).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSmallSnore();
			}).Transition(this.idle, (Snorer.StatesInstance smi) => !smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.quiet.Enter("ScheduleNextSnore", delegate(Snorer.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.sleeping.snoring);
			});
			this.sleeping.snoring.Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSnoreBGEffect();
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.sleeping.quiet).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSnoreBGEffect();
			});
		}

		// Token: 0x0600AE72 RID: 44658 RVA: 0x003D380C File Offset: 0x003D1A0C
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(5f, 1f), 3f), 10f);
		}

		// Token: 0x04008910 RID: 35088
		public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State idle;

		// Token: 0x04008911 RID: 35089
		public Snorer.States.SleepStates sleeping;

		// Token: 0x02002A25 RID: 10789
		public class SleepStates : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State
		{
			// Token: 0x0400BA48 RID: 47688
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State quiet;

			// Token: 0x0400BA49 RID: 47689
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State snoring;
		}
	}
}
