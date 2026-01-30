using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class HappySinger : GameStateMachine<HappySinger, HappySinger.Instance>
{
	// Token: 0x06001B20 RID: 6944 RVA: 0x00094F48 File Offset: 0x00093148
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsJoySinger").ToggleLoopingSound(this.soundPath, null, true, true, true).ToggleAnims("anim_loco_singer_kanim", 0f).ToggleAnims("anim_idle_singer_kanim", 0f).EventHandler(GameHashes.TagsChanged, delegate(HappySinger.Instance smi, object obj)
		{
			if (smi.musicParticleFX != null)
			{
				smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
			}
		}).Enter(delegate(HappySinger.Instance smi)
		{
			smi.musicParticleFX = Util.KInstantiate(EffectPrefabs.Instance.HappySingerFX, smi.master.transform.GetPosition() + this.offset);
			smi.musicParticleFX.transform.SetParent(smi.master.transform);
			smi.CreatePasserbyReactable();
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Update(delegate(HappySinger.Instance smi, float dt)
		{
			if (!smi.SpeechMonitorInstance.IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.Kpid, smi.AnimController))
			{
				Db.Get().Thoughts.CatchyTune.PlayAsSpeech(smi.SpeechMonitorInstance);
			}
		}, UpdateRate.SIM_1000ms, false).Exit(delegate(HappySinger.Instance smi)
		{
			smi.musicParticleFX.SetActive(false);
			Util.KDestroyGameObject(smi.musicParticleFX);
			smi.ClearPasserbyReactable();
		});
	}

	// Token: 0x04000F9B RID: 3995
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04000F9C RID: 3996
	public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000F9D RID: 3997
	public HappySinger.OverjoyedStates overjoyed;

	// Token: 0x04000F9E RID: 3998
	public string soundPath = GlobalAssets.GetSound("DupeSinging_NotesFX_LP", false);

	// Token: 0x02001373 RID: 4979
	public class OverjoyedStates : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B34 RID: 27444
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006B35 RID: 27445
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x02001374 RID: 4980
	public new class Instance : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06008BEB RID: 35819 RVA: 0x003601C0 File Offset: 0x0035E3C0
		// (set) Token: 0x06008BEC RID: 35820 RVA: 0x003601C8 File Offset: 0x0035E3C8
		public KPrefabID Kpid { get; private set; }

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06008BED RID: 35821 RVA: 0x003601D1 File Offset: 0x0035E3D1
		// (set) Token: 0x06008BEE RID: 35822 RVA: 0x003601D9 File Offset: 0x0035E3D9
		public KBatchedAnimController AnimController { get; private set; }

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06008BEF RID: 35823 RVA: 0x003601E2 File Offset: 0x0035E3E2
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

		// Token: 0x06008BF0 RID: 35824 RVA: 0x00360208 File Offset: 0x0035E408
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.Kpid = master.GetComponent<KPrefabID>();
			this.AnimController = master.GetComponent<KBatchedAnimController>();
		}

		// Token: 0x06008BF1 RID: 35825 RVA: 0x0036022C File Offset: 0x0035E42C
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote sing = Db.Get().Emotes.Minion.Sing;
				emoteReactable.SetEmote(sing).SetThought(Db.Get().Thoughts.CatchyTune).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x06008BF2 RID: 35826 RVA: 0x003602E6 File Offset: 0x0035E4E6
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.Trigger(-1278274506, null);
		}

		// Token: 0x06008BF3 RID: 35827 RVA: 0x003602F4 File Offset: 0x0035E4F4
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x06008BF4 RID: 35828 RVA: 0x003602FF File Offset: 0x0035E4FF
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x04006B36 RID: 27446
		private Reactable passerbyReactable;

		// Token: 0x04006B37 RID: 27447
		public GameObject musicParticleFX;

		// Token: 0x04006B3A RID: 27450
		private SpeechMonitor.Instance speechMonitorInstance;
	}
}
