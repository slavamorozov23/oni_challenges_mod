using System;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CAA RID: 3242
public class AudioOptionsScreen : KModalScreen
{
	// Token: 0x0600632C RID: 25388 RVA: 0x0024C998 File Offset: 0x0024AB98
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.doneButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.sliderPool = new UIPool<SliderContainer>(this.sliderPrefab);
		foreach (KeyValuePair<string, AudioMixer.UserVolumeBus> keyValuePair in AudioMixer.instance.userVolumeSettings)
		{
			SliderContainer newSlider = this.sliderPool.GetFreeElement(this.sliderGroup, true);
			this.sliderBusMap.Add(newSlider.slider, keyValuePair.Key);
			newSlider.slider.value = keyValuePair.Value.busLevel;
			newSlider.nameLabel.text = keyValuePair.Value.labelString;
			newSlider.UpdateSliderLabel(keyValuePair.Value.busLevel);
			newSlider.slider.ClearReleaseHandleEvent();
			newSlider.slider.onValueChanged.AddListener(delegate(float value)
			{
				this.OnReleaseHandle(newSlider.slider);
			});
			if (keyValuePair.Key == "Master")
			{
				newSlider.transform.SetSiblingIndex(2);
				newSlider.slider.onValueChanged.AddListener(new UnityAction<float>(this.CheckMasterValue));
				this.CheckMasterValue(keyValuePair.Value.busLevel);
			}
		}
		HierarchyReferences component = this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>();
		GameObject gameObject = component.GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE_TOOLTIP);
		component.GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayMusic();
		};
		component.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, 1);
		}
		HierarchyReferences component2 = this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>();
		GameObject gameObject2 = component2.GetReference("Button").gameObject;
		gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS_TOOLTIP);
		gameObject2.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayAutomation();
		};
		component2.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS);
		component2.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.MuteOnFocusLost))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, 0);
		}
		HierarchyReferences component3 = this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>();
		GameObject gameObject3 = component3.GetReference("Button").gameObject;
		gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST_TOOLTIP);
		gameObject3.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleMuteOnFocusLost();
		};
		component3.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST);
		component3.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x0600632D RID: 25389 RVA: 0x0024CD10 File Offset: 0x0024AF10
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600632E RID: 25390 RVA: 0x0024CD32 File Offset: 0x0024AF32
	private void CheckMasterValue(float value)
	{
		this.jambell.enabled = (value == 0f);
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x0024CD47 File Offset: 0x0024AF47
	private void OnReleaseHandle(KSlider slider)
	{
		AudioMixer.instance.SetUserVolume(this.sliderBusMap[slider], slider.value);
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x0024CD68 File Offset: 0x0024AF68
	private void ToggleAlwaysPlayMusic()
	{
		MusicManager.instance.alwaysPlayMusic = !MusicManager.instance.alwaysPlayMusic;
		this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayMusicKey, MusicManager.instance.alwaysPlayMusic ? 1 : 0);
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x0024CDD0 File Offset: 0x0024AFD0
	private void ToggleAlwaysPlayAutomation()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1) ? 0 : 1);
		this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x0024CE28 File Offset: 0x0024B028
	private void ToggleMuteOnFocusLost()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, (KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1) ? 0 : 1);
		this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x0024CE80 File Offset: 0x0024B080
	private void BuildAudioDeviceList()
	{
		this.audioDevices.Clear();
		this.audioDeviceOptions.Clear();
		int num;
		RuntimeManager.CoreSystem.getNumDrivers(out num);
		for (int i = 0; i < num; i++)
		{
			KFMOD.AudioDevice audioDevice = default(KFMOD.AudioDevice);
			string name;
			RuntimeManager.CoreSystem.getDriverInfo(i, out name, 64, out audioDevice.guid, out audioDevice.systemRate, out audioDevice.speakerMode, out audioDevice.speakerModeChannels);
			audioDevice.name = name;
			audioDevice.fmod_id = i;
			this.audioDevices.Add(audioDevice);
			this.audioDeviceOptions.Add(new Dropdown.OptionData(audioDevice.name));
		}
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x0024CF2C File Offset: 0x0024B12C
	private void OnAudioDeviceChanged(int idx)
	{
		RuntimeManager.CoreSystem.setDriver(idx);
		for (int i = 0; i < this.audioDevices.Count; i++)
		{
			if (idx == this.audioDevices[i].fmod_id)
			{
				KFMOD.currentDevice = this.audioDevices[i];
				KPlayerPrefs.SetString("AudioDeviceGuid", KFMOD.currentDevice.guid.ToString());
				return;
			}
		}
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x0024CFA3 File Offset: 0x0024B1A3
	private void OnClose(GameObject go)
	{
		this.alwaysPlayMusicMetric[AudioOptionsScreen.AlwaysPlayMusicKey] = MusicManager.instance.alwaysPlayMusic;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.alwaysPlayMusicMetric, "AudioOptionsScreen");
		UnityEngine.Object.Destroy(go);
	}

	// Token: 0x04004344 RID: 17220
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004345 RID: 17221
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04004346 RID: 17222
	[SerializeField]
	private SliderContainer sliderPrefab;

	// Token: 0x04004347 RID: 17223
	[SerializeField]
	private GameObject sliderGroup;

	// Token: 0x04004348 RID: 17224
	[SerializeField]
	private Image jambell;

	// Token: 0x04004349 RID: 17225
	[SerializeField]
	private GameObject alwaysPlayMusicButton;

	// Token: 0x0400434A RID: 17226
	[SerializeField]
	private GameObject alwaysPlayAutomationButton;

	// Token: 0x0400434B RID: 17227
	[SerializeField]
	private GameObject muteOnFocusLostToggle;

	// Token: 0x0400434C RID: 17228
	[SerializeField]
	private Dropdown deviceDropdown;

	// Token: 0x0400434D RID: 17229
	private UIPool<SliderContainer> sliderPool;

	// Token: 0x0400434E RID: 17230
	private Dictionary<KSlider, string> sliderBusMap = new Dictionary<KSlider, string>();

	// Token: 0x0400434F RID: 17231
	public static readonly string AlwaysPlayMusicKey = "AlwaysPlayMusic";

	// Token: 0x04004350 RID: 17232
	public static readonly string AlwaysPlayAutomation = "AlwaysPlayAutomation";

	// Token: 0x04004351 RID: 17233
	public static readonly string MuteOnFocusLost = "MuteOnFocusLost";

	// Token: 0x04004352 RID: 17234
	private Dictionary<string, object> alwaysPlayMusicMetric = new Dictionary<string, object>
	{
		{
			AudioOptionsScreen.AlwaysPlayMusicKey,
			null
		}
	};

	// Token: 0x04004353 RID: 17235
	private List<KFMOD.AudioDevice> audioDevices = new List<KFMOD.AudioDevice>();

	// Token: 0x04004354 RID: 17236
	private List<Dropdown.OptionData> audioDeviceOptions = new List<Dropdown.OptionData>();
}
