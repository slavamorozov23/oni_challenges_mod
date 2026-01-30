using System;
using UnityEngine;

// Token: 0x02000A31 RID: 2609
public class InspirationEffectMonitor : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>
{
	// Token: 0x06004C33 RID: 19507 RVA: 0x001BADB0 File Offset: 0x001B8FB0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.CatchyTune, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.OnCatchyTune)).ParamTransition<bool>(this.shouldCatchyTune, this.catchyTune, (InspirationEffectMonitor.Instance smi, bool shouldCatchyTune) => shouldCatchyTune);
		this.catchyTune.Exit(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.shouldCatchyTune.Set(false, smi, false);
		}).ToggleEffect("HeardJoySinger").ToggleThought(Db.Get().Thoughts.CatchyTune, null).EventHandler(GameHashes.StartWork, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.TryThinkCatchyTune)).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HeardJoySinger, null).Enter(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.SingCatchyTune(smi);
		}).Update(delegate(InspirationEffectMonitor.Instance smi, float dt)
		{
			this.TryThinkCatchyTune(smi, null);
			this.inspirationTimeRemaining.Delta(-dt, smi);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<float>(this.inspirationTimeRemaining, this.idle, (InspirationEffectMonitor.Instance smi, float p) => p <= 0f);
	}

	// Token: 0x06004C34 RID: 19508 RVA: 0x001BAECF File Offset: 0x001B90CF
	private void OnCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		this.inspirationTimeRemaining.Set(600f, smi, false);
		this.shouldCatchyTune.Set(true, smi, false);
	}

	// Token: 0x06004C35 RID: 19509 RVA: 0x001BAEF3 File Offset: 0x001B90F3
	private void TryThinkCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		if (UnityEngine.Random.Range(1, 101) > 66)
		{
			this.SingCatchyTune(smi);
		}
	}

	// Token: 0x06004C36 RID: 19510 RVA: 0x001BAF08 File Offset: 0x001B9108
	private void SingCatchyTune(InspirationEffectMonitor.Instance smi)
	{
		smi.ThoughtGraphInstance.AddThought(Db.Get().Thoughts.CatchyTune);
		if (!smi.SpeechMonitorInstance.IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.Kpid, smi.AnimController))
		{
			Db.Get().Thoughts.CatchyTune.PlayAsSpeech(smi.SpeechMonitorInstance);
		}
	}

	// Token: 0x0400329D RID: 12957
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.BoolParameter shouldCatchyTune;

	// Token: 0x0400329E RID: 12958
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.FloatParameter inspirationTimeRemaining;

	// Token: 0x0400329F RID: 12959
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State idle;

	// Token: 0x040032A0 RID: 12960
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State catchyTune;

	// Token: 0x02001AFC RID: 6908
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AFD RID: 6909
	public new class Instance : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameInstance
	{
		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600A7F9 RID: 43001 RVA: 0x003BE149 File Offset: 0x003BC349
		// (set) Token: 0x0600A7FA RID: 43002 RVA: 0x003BE151 File Offset: 0x003BC351
		public KPrefabID Kpid { get; private set; }

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600A7FB RID: 43003 RVA: 0x003BE15A File Offset: 0x003BC35A
		// (set) Token: 0x0600A7FC RID: 43004 RVA: 0x003BE162 File Offset: 0x003BC362
		public KBatchedAnimController AnimController { get; private set; }

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600A7FD RID: 43005 RVA: 0x003BE16B File Offset: 0x003BC36B
		public SpeechMonitor.Instance SpeechMonitorInstance
		{
			get
			{
				if (this.speechMonitorInstance == null)
				{
					this.speechMonitorInstance = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
				}
				return this.speechMonitorInstance;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600A7FE RID: 43006 RVA: 0x003BE191 File Offset: 0x003BC391
		public ThoughtGraph.Instance ThoughtGraphInstance
		{
			get
			{
				if (this.thoughtGraphInstance == null)
				{
					this.thoughtGraphInstance = base.master.gameObject.GetSMI<ThoughtGraph.Instance>();
				}
				return this.thoughtGraphInstance;
			}
		}

		// Token: 0x0600A7FF RID: 43007 RVA: 0x003BE1B7 File Offset: 0x003BC3B7
		public Instance(IStateMachineTarget master, InspirationEffectMonitor.Def def) : base(master, def)
		{
			this.Kpid = master.GetComponent<KPrefabID>();
			this.AnimController = master.GetComponent<KBatchedAnimController>();
		}

		// Token: 0x0400836B RID: 33643
		private SpeechMonitor.Instance speechMonitorInstance;

		// Token: 0x0400836C RID: 33644
		private ThoughtGraph.Instance thoughtGraphInstance;
	}
}
