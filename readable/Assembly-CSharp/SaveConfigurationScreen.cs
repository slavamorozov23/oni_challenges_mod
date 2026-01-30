using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000DFC RID: 3580
[Serializable]
public class SaveConfigurationScreen
{
	// Token: 0x06007161 RID: 29025 RVA: 0x002B524C File Offset: 0x002B344C
	public void ToggleDisabledContent(bool enable)
	{
		if (enable)
		{
			this.disabledContentPanel.SetActive(true);
			this.disabledContentWarning.SetActive(false);
			this.perSaveWarning.SetActive(true);
			return;
		}
		this.disabledContentPanel.SetActive(false);
		this.disabledContentWarning.SetActive(true);
		this.perSaveWarning.SetActive(false);
	}

	// Token: 0x06007162 RID: 29026 RVA: 0x002B52A8 File Offset: 0x002B34A8
	public void Init()
	{
		this.autosaveFrequencySlider.minValue = 0f;
		this.autosaveFrequencySlider.maxValue = (float)(this.sliderValueToCycleCount.Length - 1);
		this.autosaveFrequencySlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnAutosaveValueChanged(Mathf.FloorToInt(val));
		});
		this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
		this.timelapseResolutionSlider.minValue = 0f;
		this.timelapseResolutionSlider.maxValue = (float)(this.sliderValueToResolution.Length - 1);
		this.timelapseResolutionSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnTimelapseValueChanged(Mathf.FloorToInt(val));
		});
		this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
		this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
	}

	// Token: 0x06007163 RID: 29027 RVA: 0x002B5388 File Offset: 0x002B3588
	public void Show(bool show)
	{
		if (show)
		{
			this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
			this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
			this.OnAutosaveValueChanged(Mathf.FloorToInt(this.autosaveFrequencySlider.value));
			this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
		}
	}

	// Token: 0x06007164 RID: 29028 RVA: 0x002B53FC File Offset: 0x002B35FC
	private void OnTimelapseValueChanged(int sliderValue)
	{
		Vector2I vector2I = this.SliderValueToResolution(sliderValue);
		if (vector2I.x <= 0)
		{
			this.timelapseDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_DISABLED_DESCRIPTION);
		}
		else
		{
			this.timelapseDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_RESOLUTION_DESCRIPTION, vector2I.x, vector2I.y));
		}
		SaveGame.Instance.TimelapseResolution = vector2I;
		Game.Instance.Trigger(75424175, null);
	}

	// Token: 0x06007165 RID: 29029 RVA: 0x002B547C File Offset: 0x002B367C
	private void OnAutosaveValueChanged(int sliderValue)
	{
		int num = this.SliderValueToCycleCount(sliderValue);
		if (sliderValue == 0)
		{
			this.autosaveDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_NEVER);
		}
		else
		{
			this.autosaveDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_FREQUENCY_DESCRIPTION, num));
		}
		SaveGame.Instance.AutoSaveCycleInterval = num;
	}

	// Token: 0x06007166 RID: 29030 RVA: 0x002B54D6 File Offset: 0x002B36D6
	private int SliderValueToCycleCount(int sliderValue)
	{
		return this.sliderValueToCycleCount[sliderValue];
	}

	// Token: 0x06007167 RID: 29031 RVA: 0x002B54E0 File Offset: 0x002B36E0
	private int CycleCountToSlider(int count)
	{
		for (int i = 0; i < this.sliderValueToCycleCount.Length; i++)
		{
			if (this.sliderValueToCycleCount[i] == count)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06007168 RID: 29032 RVA: 0x002B550E File Offset: 0x002B370E
	private Vector2I SliderValueToResolution(int sliderValue)
	{
		return this.sliderValueToResolution[sliderValue];
	}

	// Token: 0x06007169 RID: 29033 RVA: 0x002B551C File Offset: 0x002B371C
	private int ResolutionToSliderValue(Vector2I resolution)
	{
		for (int i = 0; i < this.sliderValueToResolution.Length; i++)
		{
			if (this.sliderValueToResolution[i] == resolution)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x04004E42 RID: 20034
	[SerializeField]
	private KSlider autosaveFrequencySlider;

	// Token: 0x04004E43 RID: 20035
	[SerializeField]
	private LocText timelapseDescriptionLabel;

	// Token: 0x04004E44 RID: 20036
	[SerializeField]
	private KSlider timelapseResolutionSlider;

	// Token: 0x04004E45 RID: 20037
	[SerializeField]
	private LocText autosaveDescriptionLabel;

	// Token: 0x04004E46 RID: 20038
	private int[] sliderValueToCycleCount = new int[]
	{
		-1,
		50,
		20,
		10,
		5,
		2,
		1
	};

	// Token: 0x04004E47 RID: 20039
	private Vector2I[] sliderValueToResolution = new Vector2I[]
	{
		new Vector2I(-1, -1),
		new Vector2I(256, 384),
		new Vector2I(512, 768),
		new Vector2I(1024, 1536),
		new Vector2I(2048, 3072),
		new Vector2I(4096, 6144),
		new Vector2I(8192, 12288)
	};

	// Token: 0x04004E48 RID: 20040
	[SerializeField]
	private GameObject disabledContentPanel;

	// Token: 0x04004E49 RID: 20041
	[SerializeField]
	private GameObject disabledContentWarning;

	// Token: 0x04004E4A RID: 20042
	[SerializeField]
	private GameObject perSaveWarning;
}
