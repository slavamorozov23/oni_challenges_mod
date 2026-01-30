using System;
using UnityEngine;

// Token: 0x02000A11 RID: 2577
public class BlinkMonitor : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>
{
	// Token: 0x06004B97 RID: 19351 RVA: 0x001B74EC File Offset: 0x001B56EC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.CreateEyes)).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.DestroyEyes));
		this.satisfied.ScheduleGoTo(new Func<BlinkMonitor.Instance, float>(BlinkMonitor.GetRandomBlinkTime), this.blinking);
		this.blinking.EnterTransition(this.satisfied, GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Not(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Transition.ConditionCallback(BlinkMonitor.CanBlink))).Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.BeginBlinking)).Update(new Action<BlinkMonitor.Instance, float>(BlinkMonitor.UpdateBlinking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.eyes).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.EndBlinking));
	}

	// Token: 0x06004B98 RID: 19352 RVA: 0x001B75B6 File Offset: 0x001B57B6
	private static bool CanBlink(BlinkMonitor.Instance smi)
	{
		return !smi.eye_anim.IsNullOrWhiteSpace() && SpeechMonitor.IsAllowedToPlaySpeech(smi.Kpid, smi.AnimController) && smi.Navigator.CurrentNavType != NavType.Ladder;
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x001B75EB File Offset: 0x001B57EB
	private static float GetRandomBlinkTime(BlinkMonitor.Instance smi)
	{
		return UnityEngine.Random.Range(TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMin, TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMax);
	}

	// Token: 0x06004B9A RID: 19354 RVA: 0x001B7608 File Offset: 0x001B5808
	private static void CreateEyes(BlinkMonitor.Instance smi)
	{
		smi.eyes = Util.KInstantiate(Assets.GetPrefab(EyeAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.eyes.gameObject.SetActive(true);
		smi.sm.eyes.Set(smi.eyes.gameObject, smi, false);
	}

	// Token: 0x06004B9B RID: 19355 RVA: 0x001B7665 File Offset: 0x001B5865
	private static void DestroyEyes(BlinkMonitor.Instance smi)
	{
		if (smi.eyes != null)
		{
			Util.KDestroyGameObject(smi.eyes);
			smi.eyes = null;
		}
	}

	// Token: 0x06004B9C RID: 19356 RVA: 0x001B7687 File Offset: 0x001B5887
	public static void BeginBlinking(BlinkMonitor.Instance smi)
	{
		smi.eyes.Play(smi.eye_anim, KAnim.PlayMode.Once, 1f, 0f);
		BlinkMonitor.UpdateBlinking(smi, 0f);
	}

	// Token: 0x06004B9D RID: 19357 RVA: 0x001B76B5 File Offset: 0x001B58B5
	public static void EndBlinking(BlinkMonitor.Instance smi)
	{
		smi.SymbolOverrideController.RemoveSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, 3);
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x001B76CC File Offset: 0x001B58CC
	public static void UpdateBlinking(BlinkMonitor.Instance smi, float dt)
	{
		int currentFrameIndex = smi.eyes.GetCurrentFrameIndex();
		KAnimBatch batch = smi.eyes.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return;
		}
		KAnim.Anim.Frame frame;
		if (!smi.eyes.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return;
		}
		HashedString hash = HashedString.Invalid;
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					hash = frameElement.symbol;
					break;
				}
			}
		}
		smi.SymbolOverrideController.AddSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, smi.eyes.AnimFiles[0].GetData().build.GetSymbol(hash), 3);
	}

	// Token: 0x0400321A RID: 12826
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State satisfied;

	// Token: 0x0400321B RID: 12827
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State blinking;

	// Token: 0x0400321C RID: 12828
	public StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.TargetParameter eyes;

	// Token: 0x0400321D RID: 12829
	private static HashedString HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x02001AA5 RID: 6821
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AA6 RID: 6822
	public class Tuning : TuningData<BlinkMonitor.Tuning>
	{
		// Token: 0x04008259 RID: 33369
		public float randomBlinkIntervalMin;

		// Token: 0x0400825A RID: 33370
		public float randomBlinkIntervalMax;
	}

	// Token: 0x02001AA7 RID: 6823
	public new class Instance : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.GameInstance
	{
		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x0600A699 RID: 42649 RVA: 0x003BA746 File Offset: 0x003B8946
		// (set) Token: 0x0600A69A RID: 42650 RVA: 0x003BA74E File Offset: 0x003B894E
		public KPrefabID Kpid { get; private set; }

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x0600A69B RID: 42651 RVA: 0x003BA757 File Offset: 0x003B8957
		// (set) Token: 0x0600A69C RID: 42652 RVA: 0x003BA75F File Offset: 0x003B895F
		public KBatchedAnimController AnimController { get; private set; }

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x0600A69D RID: 42653 RVA: 0x003BA768 File Offset: 0x003B8968
		// (set) Token: 0x0600A69E RID: 42654 RVA: 0x003BA770 File Offset: 0x003B8970
		public Navigator Navigator { get; private set; }

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x0600A69F RID: 42655 RVA: 0x003BA779 File Offset: 0x003B8979
		// (set) Token: 0x0600A6A0 RID: 42656 RVA: 0x003BA781 File Offset: 0x003B8981
		public SymbolOverrideController SymbolOverrideController { get; private set; }

		// Token: 0x0600A6A1 RID: 42657 RVA: 0x003BA78A File Offset: 0x003B898A
		public Instance(IStateMachineTarget master, BlinkMonitor.Def def) : base(master, def)
		{
			this.Kpid = master.GetComponent<KPrefabID>();
			this.AnimController = master.GetComponent<KBatchedAnimController>();
			this.Navigator = master.GetComponent<Navigator>();
			this.SymbolOverrideController = master.GetComponent<SymbolOverrideController>();
		}

		// Token: 0x0600A6A2 RID: 42658 RVA: 0x003BA7C4 File Offset: 0x003B89C4
		public bool IsBlinking()
		{
			return base.IsInsideState(base.sm.blinking);
		}

		// Token: 0x0600A6A3 RID: 42659 RVA: 0x003BA7D7 File Offset: 0x003B89D7
		public void Blink()
		{
			this.GoTo(base.sm.blinking);
		}

		// Token: 0x0400825B RID: 33371
		public KBatchedAnimController eyes;

		// Token: 0x0400825C RID: 33372
		public string eye_anim;
	}
}
