using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000A49 RID: 2633
public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
	// Token: 0x06004CBA RID: 19642 RVA: 0x001BE218 File Offset: 0x001BC418
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
		this.satisfied.DoNothing();
		this.talking.Enter(delegate(SpeechMonitor.Instance smi)
		{
			SpeechMonitor.StartAudio(smi);
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			if (smi.Kpid.HasTag(GameTags.DoNotInterruptMe))
			{
				smi.GoTo(this.talking.animGoverned);
				return;
			}
			if (smi.ev.isValid())
			{
				smi.GoTo(this.talking.audioGoverned);
				return;
			}
			smi.GoTo(this.talking.fallback);
		}).Exit(delegate(SpeechMonitor.Instance smi)
		{
			smi.SymbolOverrideController.RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);
		});
		this.talking.audioGoverned.Transition(this.satisfied, new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.Transition.ConditionCallback(SpeechMonitor.IsAudioStopped), UpdateRate.SIM_200ms).Update(new Action<SpeechMonitor.Instance, float>(SpeechMonitor.LipFlap), UpdateRate.RENDER_EVERY_TICK, false);
		this.talking.animGoverned.TagTransition(GameTags.DoNotInterruptMe, this.satisfied, true).Update(new Action<SpeechMonitor.Instance, float>(SpeechMonitor.LipFlap), UpdateRate.RENDER_EVERY_TICK, false).Update(delegate(SpeechMonitor.Instance smi, float dt)
		{
			if (SpeechMonitor.IsAudioStopped(smi))
			{
				SpeechMonitor.StartAudio(smi);
			}
		}, UpdateRate.RENDER_EVERY_TICK, false);
		this.talking.fallback.Enter(delegate(SpeechMonitor.Instance smi)
		{
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
		}).Target(this.mouth).OnAnimQueueComplete(this.satisfied);
	}

	// Token: 0x06004CBB RID: 19643 RVA: 0x001BE374 File Offset: 0x001BC574
	private static void CreateMouth(SpeechMonitor.Instance smi)
	{
		smi.mouth = global::Util.KInstantiate(Assets.GetPrefab(MouthAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.mouth.gameObject.SetActive(true);
		smi.sm.mouth.Set(smi.mouth.gameObject, smi, false);
		smi.SetMouthId();
	}

	// Token: 0x06004CBC RID: 19644 RVA: 0x001BE3D7 File Offset: 0x001BC5D7
	private static void DestroyMouth(SpeechMonitor.Instance smi)
	{
		if (smi.mouth != null)
		{
			global::Util.KDestroyGameObject(smi.mouth);
			smi.mouth = null;
		}
	}

	// Token: 0x06004CBD RID: 19645 RVA: 0x001BE3FC File Offset: 0x001BC5FC
	private static string GetRandomSpeechAnim(SpeechMonitor.Instance smi)
	{
		return smi.speechPrefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString() + smi.mouthId;
	}

	// Token: 0x06004CBE RID: 19646 RVA: 0x001BE434 File Offset: 0x001BC634
	public static bool IsAllowedToPlaySpeech(KPrefabID prefabID, KBatchedAnimController controller)
	{
		if (prefabID.HasTag(GameTags.Dead))
		{
			return false;
		}
		if (prefabID.HasTag(GameTags.Incapacitated))
		{
			return false;
		}
		KAnim.Anim currentAnim = controller.GetCurrentAnim();
		return currentAnim == null || (GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim) && SpeechMonitor.CanOverrideHead(controller));
	}

	// Token: 0x06004CBF RID: 19647 RVA: 0x001BE480 File Offset: 0x001BC680
	private static bool CanOverrideHead(KBatchedAnimController kbac)
	{
		bool result = true;
		KAnim.Anim currentAnim = kbac.GetCurrentAnim();
		if (currentAnim == null)
		{
			result = false;
		}
		else if (currentAnim.animFile.name != SpeechMonitor.GENERIC_CONVO_ANIM_NAME)
		{
			int currentFrameIndex = kbac.GetCurrentFrameIndex();
			KAnim.Anim.Frame frame;
			if (currentFrameIndex <= 0)
			{
				result = false;
			}
			else if (KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag).TryGetFrame(currentFrameIndex, out frame) && frame.hasHead)
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x001BE4F4 File Offset: 0x001BC6F4
	private static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
	{
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		if (currentFrameIndex == -1)
		{
			return SpeechMonitor.INVALID_FRAME_ELEMENT;
		}
		KAnimBatch batch = controller.GetBatch();
		if (batch == null)
		{
			return SpeechMonitor.INVALID_FRAME_ELEMENT;
		}
		KAnim.Anim.Frame frame;
		if (!batch.group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return SpeechMonitor.INVALID_FRAME_ELEMENT;
		}
		List<KAnim.Anim.FrameElement> frameElements = batch.group.data.frameElements;
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			bool flag = num < frameElements.Count;
			DebugUtil.DevAssert(flag, "Frame element index out of range", null);
			if (flag)
			{
				KAnim.Anim.FrameElement frameElement = frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					return frameElement;
				}
			}
		}
		return SpeechMonitor.INVALID_FRAME_ELEMENT;
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x001BE5AB File Offset: 0x001BC7AB
	private static void StartAudio(SpeechMonitor.Instance smi)
	{
		smi.ev.clearHandle();
		if (smi.voiceEvent != null)
		{
			smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.AnimController, 0f, false, false);
		}
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x001BE5E0 File Offset: 0x001BC7E0
	private static bool IsAudioStopped(SpeechMonitor.Instance smi)
	{
		if (!smi.ev.isValid())
		{
			return true;
		}
		PLAYBACK_STATE playback_STATE;
		smi.ev.getPlaybackState(out playback_STATE);
		if (playback_STATE == PLAYBACK_STATE.STOPPING || playback_STATE == PLAYBACK_STATE.STOPPED)
		{
			smi.ev.clearHandle();
			return true;
		}
		return false;
	}

	// Token: 0x06004CC3 RID: 19651 RVA: 0x001BE620 File Offset: 0x001BC820
	private static void LipFlap(SpeechMonitor.Instance smi, float dt)
	{
		if (smi.mouth.IsStopped())
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			DebugUtil.DevAssert(!smi.mouth.IsStopped(), "Mouth animation should be playing", null);
		}
	}

	// Token: 0x04003316 RID: 13078
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;

	// Token: 0x04003317 RID: 13079
	public SpeechMonitor.Playing talking;

	// Token: 0x04003318 RID: 13080
	public static string PREFIX_SAD = "sad";

	// Token: 0x04003319 RID: 13081
	public static string PREFIX_HAPPY = "happy";

	// Token: 0x0400331A RID: 13082
	public static string PREFIX_SINGER = "sing";

	// Token: 0x0400331B RID: 13083
	public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;

	// Token: 0x0400331C RID: 13084
	private static HashedString HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x0400331D RID: 13085
	private static HashedString GENERIC_CONVO_ANIM_NAME = new HashedString("anim_generic_convo_kanim");

	// Token: 0x0400331E RID: 13086
	private static KAnim.Anim.FrameElement INVALID_FRAME_ELEMENT = new KAnim.Anim.FrameElement
	{
		symbol = HashedString.Invalid
	};

	// Token: 0x02001B3B RID: 6971
	public class Playing : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State
	{
		// Token: 0x04008426 RID: 33830
		public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State audioGoverned;

		// Token: 0x04008427 RID: 33831
		public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State animGoverned;

		// Token: 0x04008428 RID: 33832
		public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State fallback;
	}

	// Token: 0x02001B3C RID: 6972
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B3D RID: 6973
	public class Tuning : TuningData<SpeechMonitor.Tuning>
	{
		// Token: 0x04008429 RID: 33833
		public float randomSpeechIntervalMin;

		// Token: 0x0400842A RID: 33834
		public float randomSpeechIntervalMax;

		// Token: 0x0400842B RID: 33835
		public int speechCount;
	}

	// Token: 0x02001B3E RID: 6974
	public new class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
	{
		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600A8FD RID: 43261 RVA: 0x003C0661 File Offset: 0x003BE861
		// (set) Token: 0x0600A8FE RID: 43262 RVA: 0x003C0669 File Offset: 0x003BE869
		public KBatchedAnimController AnimController { get; private set; }

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x0600A8FF RID: 43263 RVA: 0x003C0672 File Offset: 0x003BE872
		// (set) Token: 0x0600A900 RID: 43264 RVA: 0x003C067A File Offset: 0x003BE87A
		public SymbolOverrideController SymbolOverrideController { get; private set; }

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x0600A901 RID: 43265 RVA: 0x003C0683 File Offset: 0x003BE883
		// (set) Token: 0x0600A902 RID: 43266 RVA: 0x003C068B File Offset: 0x003BE88B
		public MinionIdentity MinionIdentity { get; private set; }

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x0600A903 RID: 43267 RVA: 0x003C0694 File Offset: 0x003BE894
		// (set) Token: 0x0600A904 RID: 43268 RVA: 0x003C069C File Offset: 0x003BE89C
		public KPrefabID Kpid { get; private set; }

		// Token: 0x0600A905 RID: 43269 RVA: 0x003C06A8 File Offset: 0x003BE8A8
		public Instance(IStateMachineTarget master, SpeechMonitor.Def def) : base(master, def)
		{
			this.AnimController = master.GetComponent<KBatchedAnimController>();
			this.SymbolOverrideController = master.GetComponent<SymbolOverrideController>();
			this.MinionIdentity = master.GetComponent<MinionIdentity>();
			this.Kpid = master.GetComponent<KPrefabID>();
		}

		// Token: 0x0600A906 RID: 43270 RVA: 0x003C06F8 File Offset: 0x003BE8F8
		public bool IsPlayingSpeech()
		{
			return base.IsInsideState(base.sm.talking);
		}

		// Token: 0x0600A907 RID: 43271 RVA: 0x003C070B File Offset: 0x003BE90B
		public void PlaySpeech(string speech_prefix, string voice_event)
		{
			this.speechPrefix = speech_prefix;
			this.voiceEvent = voice_event;
			this.GoTo(base.sm.talking);
		}

		// Token: 0x0600A908 RID: 43272 RVA: 0x003C072C File Offset: 0x003BE92C
		public void DrawMouth()
		{
			KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(base.smi.mouth);
			bool flag = firstFrameElement.symbol != HashedString.Invalid;
			DebugUtil.DevAssert(flag, "Mouth frame element invalid", null);
			if (!flag)
			{
				return;
			}
			KAnim.Build build = base.smi.mouth.AnimFiles[0].GetData().build;
			KAnim.Build.Symbol symbol = build.GetSymbol(firstFrameElement.symbol);
			this.SymbolOverrideController.AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, symbol, 3);
			KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(this.AnimController.batchGroupID).GetSymbol(SpeechMonitor.HASH_SNAPTO_MOUTH);
			DebugUtil.DevAssert(build == symbol.build, "Mouth build mismatch", null);
			KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(build.batchTag).symbolFrameInstances[symbol.firstFrameIdx + firstFrameElement.frame];
			symbolFrameInstance.buildImageIdx = this.SymbolOverrideController.GetAtlasIdx(build.GetTexture(0));
			this.AnimController.SetSymbolOverride(symbol2.firstFrameIdx, ref symbolFrameInstance);
		}

		// Token: 0x0600A909 RID: 43273 RVA: 0x003C0834 File Offset: 0x003BEA34
		public void SetMouthId()
		{
			Personality personality = Db.Get().Personalities.Get(this.MinionIdentity.personalityResourceId);
			if (personality.speech_mouth > 0)
			{
				base.smi.mouthId = string.Format("_{0:000}", personality.speech_mouth);
			}
		}

		// Token: 0x0400842C RID: 33836
		public KBatchedAnimController mouth;

		// Token: 0x0400842D RID: 33837
		public string speechPrefix = "happy";

		// Token: 0x0400842E RID: 33838
		public string voiceEvent;

		// Token: 0x0400842F RID: 33839
		public EventInstance ev;

		// Token: 0x04008430 RID: 33840
		public string mouthId;
	}
}
