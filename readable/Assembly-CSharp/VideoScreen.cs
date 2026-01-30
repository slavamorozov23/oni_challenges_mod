using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000EC3 RID: 3779
public class VideoScreen : KModalScreen
{
	// Token: 0x06007903 RID: 30979 RVA: 0x002E849C File Offset: 0x002E669C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
		this.proceedButton.onClick += delegate()
		{
			this.Stop();
		};
		this.videoPlayer.isLooping = false;
		this.videoPlayer.loopPointReached += delegate(VideoPlayer data)
		{
			if (this.victoryLoopQueued)
			{
				base.StartCoroutine(this.SwitchToVictoryLoop());
				return;
			}
			if (!this.videoPlayer.isLooping)
			{
				this.Stop();
			}
		};
		VideoScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06007904 RID: 30980 RVA: 0x002E8514 File Offset: 0x002E6714
	protected override void OnForcedCleanUp()
	{
		VideoScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06007905 RID: 30981 RVA: 0x002E8522 File Offset: 0x002E6722
	protected override void OnShow(bool show)
	{
		base.transform.SetAsLastSibling();
		base.OnShow(show);
		this.screen = this.videoPlayer.gameObject.GetComponent<RawImage>();
	}

	// Token: 0x06007906 RID: 30982 RVA: 0x002E854C File Offset: 0x002E674C
	public void DisableAllMedia()
	{
		this.overlayContainer.gameObject.SetActive(false);
		this.videoPlayer.gameObject.SetActive(false);
		this.slideshow.gameObject.SetActive(false);
	}

	// Token: 0x06007907 RID: 30983 RVA: 0x002E8584 File Offset: 0x002E6784
	public void PlaySlideShow(Sprite[] sprites)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.preloadedSprites;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetSprites(sprites);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x06007908 RID: 30984 RVA: 0x002E85D4 File Offset: 0x002E67D4
	public void PlaySlideShow(string[] files)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetFiles(files, 0);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x06007909 RID: 30985 RVA: 0x002E8624 File Offset: 0x002E6824
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			if (this.slideshow.gameObject.activeSelf && e.TryConsume(global::Action.Escape))
			{
				this.Stop();
				return;
			}
			if (e.TryConsume(global::Action.Escape))
			{
				if (this.videoSkippable)
				{
					this.Stop();
				}
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600790A RID: 30986 RVA: 0x002E867C File Offset: 0x002E687C
	public void PlayVideo(VideoClip clip, bool unskippable = false, EventReference overrideAudioSnapshot = default(EventReference), bool showProceedButton = false, bool syncAudio = true)
	{
		global::Debug.Assert(clip != null);
		for (int i = 0; i < this.overlayContainer.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.overlayContainer.GetChild(i).gameObject);
		}
		this.Show(true);
		this.videoPlayer.isLooping = false;
		this.activeAudioSnapshot = (overrideAudioSnapshot.IsNull ? AudioMixerSnapshots.Get().TutorialVideoPlayingSnapshot : overrideAudioSnapshot);
		AudioMixer.instance.Start(this.activeAudioSnapshot);
		this.DisableAllMedia();
		this.videoPlayer.gameObject.SetActive(true);
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.screen.texture = this.renderTexture;
		this.videoPlayer.targetTexture = this.renderTexture;
		this.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.videoPlayer.clip = clip;
		this.videoPlayer.timeReference = (syncAudio ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun);
		this.videoPlayer.Play();
		if (this.audioHandle.isValid())
		{
			KFMOD.EndOneShot(this.audioHandle);
			this.audioHandle.clearHandle();
		}
		this.audioHandle = KFMOD.BeginOneShot(GlobalAssets.GetSound("vid_" + clip.name, false), Vector3.zero, 1f);
		KFMOD.EndOneShot(this.audioHandle);
		this.videoSkippable = !unskippable;
		this.closeButton.gameObject.SetActive(this.videoSkippable);
		this.proceedButton.gameObject.SetActive(showProceedButton && this.videoSkippable);
	}

	// Token: 0x0600790B RID: 30987 RVA: 0x002E882C File Offset: 0x002E6A2C
	public void QueueVictoryVideoLoop(bool queue, string message = "", string victoryAchievement = "", string loopVideo = "", bool showAchievements = true, bool syncAudio = false)
	{
		this.victoryLoopQueued = queue;
		this.victoryLoopMessage = message;
		this.victoryLoopClip = loopVideo;
		this.victoryLoopSyncAudio = syncAudio;
		this.OnStop = (System.Action)Delegate.Combine(this.OnStop, new System.Action(delegate()
		{
			if (showAchievements)
			{
				RetireColonyUtility.SaveColonySummaryData();
				MainMenu.ActivateRetiredColoniesScreenFromData(this.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
			}
		}));
	}

	// Token: 0x0600790C RID: 30988 RVA: 0x002E8890 File Offset: 0x002E6A90
	public void SetOverlayText(string overlayTemplate, List<string> strings)
	{
		VideoOverlay videoOverlay = null;
		foreach (VideoOverlay videoOverlay2 in this.overlayPrefabs)
		{
			if (videoOverlay2.name == overlayTemplate)
			{
				videoOverlay = videoOverlay2;
				break;
			}
		}
		DebugUtil.Assert(videoOverlay != null, "Could not find a template named ", overlayTemplate);
		global::Util.KInstantiateUI<VideoOverlay>(videoOverlay.gameObject, this.overlayContainer.gameObject, true).SetText(strings);
		this.overlayContainer.gameObject.SetActive(true);
	}

	// Token: 0x0600790D RID: 30989 RVA: 0x002E8930 File Offset: 0x002E6B30
	private IEnumerator SwitchToVictoryLoop()
	{
		this.victoryLoopQueued = false;
		Color color = this.fadeOverlay.color;
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 1f);
		MusicManager.instance.PlaySong("Music_Victory_03_StoryAndSummary", false);
		MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 1f, true);
		this.closeButton.gameObject.SetActive(true);
		this.proceedButton.gameObject.SetActive(true);
		this.SetOverlayText("VictoryEnd", new List<string>
		{
			this.victoryLoopMessage
		});
		this.videoPlayer.clip = Assets.GetVideo(this.victoryLoopClip);
		this.videoPlayer.isLooping = true;
		this.videoPlayer.Play();
		this.proceedButton.gameObject.SetActive(true);
		this.videoPlayer.timeReference = (this.victoryLoopSyncAudio ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		for (float i = 1f; i >= 0f; i -= Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 0f);
		yield break;
	}

	// Token: 0x0600790E RID: 30990 RVA: 0x002E8940 File Offset: 0x002E6B40
	public void Stop()
	{
		this.videoPlayer.Stop();
		this.screen.texture = null;
		this.videoPlayer.targetTexture = null;
		if (!this.activeAudioSnapshot.IsNull)
		{
			AudioMixer.instance.Stop(this.activeAudioSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.audioHandle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.OnStop != null)
		{
			this.OnStop();
		}
		this.Show(false);
	}

	// Token: 0x0600790F RID: 30991 RVA: 0x002E89B8 File Offset: 0x002E6BB8
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.audioHandle.isValid())
		{
			int num;
			this.audioHandle.getTimelinePosition(out num);
			this.videoPlayer.externalReferenceTime = (double)((float)num / 1000f);
		}
	}

	// Token: 0x04005459 RID: 21593
	public static VideoScreen Instance;

	// Token: 0x0400545A RID: 21594
	[SerializeField]
	private VideoPlayer videoPlayer;

	// Token: 0x0400545B RID: 21595
	[SerializeField]
	private Slideshow slideshow;

	// Token: 0x0400545C RID: 21596
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400545D RID: 21597
	[SerializeField]
	private KButton proceedButton;

	// Token: 0x0400545E RID: 21598
	[SerializeField]
	private RectTransform overlayContainer;

	// Token: 0x0400545F RID: 21599
	[SerializeField]
	private List<VideoOverlay> overlayPrefabs;

	// Token: 0x04005460 RID: 21600
	private RawImage screen;

	// Token: 0x04005461 RID: 21601
	private RenderTexture renderTexture;

	// Token: 0x04005462 RID: 21602
	private EventReference activeAudioSnapshot;

	// Token: 0x04005463 RID: 21603
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x04005464 RID: 21604
	private EventInstance audioHandle;

	// Token: 0x04005465 RID: 21605
	private bool victoryLoopQueued;

	// Token: 0x04005466 RID: 21606
	private string victoryLoopMessage = "";

	// Token: 0x04005467 RID: 21607
	private string victoryLoopClip = "";

	// Token: 0x04005468 RID: 21608
	private bool victoryLoopSyncAudio;

	// Token: 0x04005469 RID: 21609
	private bool videoSkippable = true;

	// Token: 0x0400546A RID: 21610
	public System.Action OnStop;
}
