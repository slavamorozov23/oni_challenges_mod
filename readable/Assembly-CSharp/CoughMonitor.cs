using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000A18 RID: 2584
public class CoughMonitor : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>
{
	// Token: 0x06004BC4 RID: 19396 RVA: 0x001B8508 File Offset: 0x001B6708
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.PoorAirQuality, new GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameEvent.Callback(this.OnBreatheDirtyAir)).ParamTransition<bool>(this.shouldCough, this.coughing, (CoughMonitor.Instance smi, bool bShouldCough) => bShouldCough);
		this.coughing.ToggleStatusItem(Db.Get().DuplicantStatusItems.Coughing, null).ToggleReactable((CoughMonitor.Instance smi) => smi.GetReactable()).ParamTransition<bool>(this.shouldCough, this.idle, (CoughMonitor.Instance smi, bool bShouldCough) => !bShouldCough);
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x001B85E4 File Offset: 0x001B67E4
	private void OnBreatheDirtyAir(CoughMonitor.Instance smi, object data)
	{
		float timeInCycles = GameClock.Instance.GetTimeInCycles();
		if (timeInCycles > 0.1f && timeInCycles - smi.lastCoughTime <= 0.1f)
		{
			return;
		}
		float value = ((Boxed<float>)data).value;
		float num = (smi.lastConsumeTime <= 0f) ? 0f : (timeInCycles - smi.lastConsumeTime);
		smi.lastConsumeTime = timeInCycles;
		smi.amountConsumed -= 0.05f * num;
		smi.amountConsumed = Mathf.Max(smi.amountConsumed, 0f);
		smi.amountConsumed += value;
		if (smi.amountConsumed >= 1f)
		{
			this.shouldCough.Set(true, smi, false);
			smi.lastConsumeTime = 0f;
			smi.amountConsumed = 0f;
		}
	}

	// Token: 0x04003238 RID: 12856
	private const float amountToCough = 1f;

	// Token: 0x04003239 RID: 12857
	private const float decayRate = 0.05f;

	// Token: 0x0400323A RID: 12858
	private const float coughInterval = 0.1f;

	// Token: 0x0400323B RID: 12859
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State idle;

	// Token: 0x0400323C RID: 12860
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State coughing;

	// Token: 0x0400323D RID: 12861
	public StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter shouldCough = new StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter(false);

	// Token: 0x02001AB9 RID: 6841
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001ABA RID: 6842
	public new class Instance : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameInstance
	{
		// Token: 0x0600A6E7 RID: 42727 RVA: 0x003BAF6E File Offset: 0x003B916E
		public Instance(IStateMachineTarget master, CoughMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A6E8 RID: 42728 RVA: 0x003BAF78 File Offset: 0x003B9178
		public Reactable GetReactable()
		{
			Emote cough_Small = Db.Get().Emotes.Minion.Cough_Small;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "BadAirCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(cough_Small);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable.RegisterEmoteStepCallbacks("react_small", null, new Action<GameObject>(this.FinishedCoughing));
		}

		// Token: 0x0600A6E9 RID: 42729 RVA: 0x003BB004 File Offset: 0x003B9204
		private void FinishedCoughing(GameObject cougher)
		{
			cougher.GetComponent<Effects>().Add("ContaminatedLungs", true);
			base.sm.shouldCough.Set(false, base.smi, false);
			base.smi.lastCoughTime = GameClock.Instance.GetTimeInCycles();
		}

		// Token: 0x04008291 RID: 33425
		[Serialize]
		public float lastCoughTime;

		// Token: 0x04008292 RID: 33426
		[Serialize]
		public float lastConsumeTime;

		// Token: 0x04008293 RID: 33427
		[Serialize]
		public float amountConsumed;
	}
}
