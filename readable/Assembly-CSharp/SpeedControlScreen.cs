using System;
using System.Collections;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000E9D RID: 3741
public class SpeedControlScreen : KScreen
{
	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x060077A1 RID: 30625 RVA: 0x002DE31B File Offset: 0x002DC51B
	// (set) Token: 0x060077A2 RID: 30626 RVA: 0x002DE322 File Offset: 0x002DC522
	public static SpeedControlScreen Instance { get; private set; }

	// Token: 0x060077A3 RID: 30627 RVA: 0x002DE32A File Offset: 0x002DC52A
	public static void DestroyInstance()
	{
		SpeedControlScreen.Instance = null;
	}

	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x060077A4 RID: 30628 RVA: 0x002DE332 File Offset: 0x002DC532
	public bool IsPaused
	{
		get
		{
			return this.pauseCount > 0;
		}
	}

	// Token: 0x060077A5 RID: 30629 RVA: 0x002DE340 File Offset: 0x002DC540
	protected override void OnPrefabInit()
	{
		SpeedControlScreen.Instance = this;
		this.pauseButton = this.pauseButtonWidget.GetComponent<KToggle>();
		this.slowButton = this.speedButtonWidget_slow.GetComponent<KToggle>();
		this.mediumButton = this.speedButtonWidget_medium.GetComponent<KToggle>();
		this.fastButton = this.speedButtonWidget_fast.GetComponent<KToggle>();
		KToggle[] array = new KToggle[]
		{
			this.pauseButton,
			this.slowButton,
			this.mediumButton,
			this.fastButton
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i].soundPlayer.Enabled = false;
		}
		this.slowButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(1f);
			this.SetSpeed(0);
		};
		this.mediumButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(2f);
			this.SetSpeed(1);
		};
		this.fastButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(3f);
			this.SetSpeed(2);
		};
		this.pauseButton.onClick += delegate()
		{
			this.TogglePause(true);
		};
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.playButtonWidget.GetComponent<KButton>().onClick += delegate()
		{
			this.TogglePause(true);
		};
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
	}

	// Token: 0x060077A6 RID: 30630 RVA: 0x002DE4E1 File Offset: 0x002DC6E1
	protected override void OnSpawn()
	{
		if (SaveGame.Instance != null)
		{
			this.speed = SaveGame.Instance.GetSpeed();
			this.SetSpeed(this.speed);
		}
		base.OnSpawn();
		this.OnChanged();
	}

	// Token: 0x060077A7 RID: 30631 RVA: 0x002DE518 File Offset: 0x002DC718
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x060077A8 RID: 30632 RVA: 0x002DE536 File Offset: 0x002DC736
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x060077A9 RID: 30633 RVA: 0x002DE540 File Offset: 0x002DC740
	public void SetSpeed(int Speed)
	{
		this.speed = Speed % 3;
		switch (this.speed)
		{
		case 0:
			this.slowButton.Select();
			this.slowButton.isOn = true;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = false;
			break;
		case 1:
			this.mediumButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = true;
			this.fastButton.isOn = false;
			break;
		case 2:
			this.fastButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = true;
			break;
		}
		this.OnSpeedChange();
	}

	// Token: 0x060077AA RID: 30634 RVA: 0x002DE60B File Offset: 0x002DC80B
	public void ToggleRidiculousSpeed()
	{
		if (this.ultraSpeed == 3f)
		{
			this.ultraSpeed = 10f;
		}
		else
		{
			this.ultraSpeed = 3f;
		}
		this.speed = 2;
		this.OnChanged();
	}

	// Token: 0x060077AB RID: 30635 RVA: 0x002DE63F File Offset: 0x002DC83F
	public void TogglePause(bool playsound = true)
	{
		if (this.IsPaused)
		{
			this.Unpause(playsound);
			return;
		}
		this.Pause(playsound, false);
	}

	// Token: 0x060077AC RID: 30636 RVA: 0x002DE65C File Offset: 0x002DC85C
	public void ResetToolTip()
	{
		this.speedButtonWidget_slow.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_medium.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_fast.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		if (this.pauseButton.isOn)
		{
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			return;
		}
		this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
	}

	// Token: 0x060077AD RID: 30637 RVA: 0x002DE78C File Offset: 0x002DC98C
	public void Pause(bool playSound = true, bool isCrashed = false)
	{
		this.pauseCount++;
		if (this.pauseCount == 1)
		{
			if (playSound)
			{
				if (isCrashed)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Crash_Screen", false));
				}
				else
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Pause", false));
				}
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(0f);
				}
			}
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().SpeedPausedMigrated);
			MusicManager.instance.SetDynamicMusicPaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = true;
			this.OnPause();
		}
	}

	// Token: 0x060077AE RID: 30638 RVA: 0x002DE860 File Offset: 0x002DCA60
	public void Unpause(bool playSound = true)
	{
		this.pauseCount = Mathf.Max(0, this.pauseCount - 1);
		if (this.pauseCount == 0)
		{
			if (playSound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Unpause", false));
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(1f);
				}
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SpeedPausedMigrated, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicUnpaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = false;
			this.SetSpeed(this.speed);
			this.OnPlay();
		}
	}

	// Token: 0x060077AF RID: 30639 RVA: 0x002DE930 File Offset: 0x002DCB30
	private void OnPause()
	{
		this.OnChanged();
	}

	// Token: 0x060077B0 RID: 30640 RVA: 0x002DE938 File Offset: 0x002DCB38
	private void OnPlay()
	{
		this.OnChanged();
	}

	// Token: 0x060077B1 RID: 30641 RVA: 0x002DE940 File Offset: 0x002DCB40
	public void OnSpeedChange()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.OnChanged();
	}

	// Token: 0x060077B2 RID: 30642 RVA: 0x002DE950 File Offset: 0x002DCB50
	private void OnChanged()
	{
		if (this.IsPaused)
		{
			Time.timeScale = 0f;
			return;
		}
		if (this.speed == 0)
		{
			Time.timeScale = this.normalSpeed;
			return;
		}
		if (this.speed == 1)
		{
			Time.timeScale = this.fastSpeed;
			return;
		}
		if (this.speed == 2)
		{
			Time.timeScale = this.ultraSpeed;
		}
	}

	// Token: 0x060077B3 RID: 30643 RVA: 0x002DE9B0 File Offset: 0x002DCBB0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.TogglePause))
		{
			this.TogglePause(true);
			return;
		}
		if (e.TryConsume(global::Action.CycleSpeed))
		{
			this.PlaySpeedChangeSound((float)((this.speed + 1) % 3 + 1));
			this.SetSpeed(this.speed + 1);
			this.OnSpeedChange();
			return;
		}
		if (e.TryConsume(global::Action.SpeedUp))
		{
			this.speed++;
			this.speed = Math.Min(this.speed, 2);
			this.SetSpeed(this.speed);
			return;
		}
		if (e.TryConsume(global::Action.SlowDown))
		{
			this.speed--;
			this.speed = Math.Max(this.speed, 0);
			this.SetSpeed(this.speed);
		}
	}

	// Token: 0x060077B4 RID: 30644 RVA: 0x002DEA70 File Offset: 0x002DCC70
	private void PlaySpeedChangeSound(float speed)
	{
		string sound = GlobalAssets.GetSound("Speed_Change", false);
		if (sound != null)
		{
			EventInstance instance = SoundEvent.BeginOneShot(sound, Vector3.zero, 1f, false);
			instance.setParameterByName("Speed", speed, false);
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x060077B5 RID: 30645 RVA: 0x002DEAB8 File Offset: 0x002DCCB8
	public void DebugStepFrame()
	{
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Stepping one frame {0} ({1})", GameClock.Instance.GetTime(), GameClock.Instance.GetTime() / 600f)
		});
		this.stepTime = Time.time;
		this.Unpause(false);
		base.StartCoroutine(this.DebugStepFrameDelay());
	}

	// Token: 0x060077B6 RID: 30646 RVA: 0x002DEB20 File Offset: 0x002DCD20
	private IEnumerator DebugStepFrameDelay()
	{
		yield return null;
		DebugUtil.LogArgs(new object[]
		{
			"Stepped one frame",
			Time.time - this.stepTime,
			"seconds"
		});
		this.Pause(false, false);
		yield break;
	}

	// Token: 0x0400532D RID: 21293
	public GameObject playButtonWidget;

	// Token: 0x0400532E RID: 21294
	public GameObject pauseButtonWidget;

	// Token: 0x0400532F RID: 21295
	public Image playIcon;

	// Token: 0x04005330 RID: 21296
	public Image pauseIcon;

	// Token: 0x04005331 RID: 21297
	[SerializeField]
	private TextStyleSetting TooltipTextStyle;

	// Token: 0x04005332 RID: 21298
	public GameObject speedButtonWidget_slow;

	// Token: 0x04005333 RID: 21299
	public GameObject speedButtonWidget_medium;

	// Token: 0x04005334 RID: 21300
	public GameObject speedButtonWidget_fast;

	// Token: 0x04005335 RID: 21301
	public GameObject mainMenuWidget;

	// Token: 0x04005336 RID: 21302
	public float normalSpeed;

	// Token: 0x04005337 RID: 21303
	public float fastSpeed;

	// Token: 0x04005338 RID: 21304
	public float ultraSpeed;

	// Token: 0x04005339 RID: 21305
	private KToggle pauseButton;

	// Token: 0x0400533A RID: 21306
	private KToggle slowButton;

	// Token: 0x0400533B RID: 21307
	private KToggle mediumButton;

	// Token: 0x0400533C RID: 21308
	private KToggle fastButton;

	// Token: 0x0400533D RID: 21309
	private int speed;

	// Token: 0x0400533E RID: 21310
	private int pauseCount;

	// Token: 0x04005340 RID: 21312
	private float stepTime;
}
