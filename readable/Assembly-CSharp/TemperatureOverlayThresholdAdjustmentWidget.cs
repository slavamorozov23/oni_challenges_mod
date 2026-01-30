using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C7A RID: 3194
public class TemperatureOverlayThresholdAdjustmentWidget : KMonoBehaviour
{
	// Token: 0x06006185 RID: 24965 RVA: 0x0023F3FD File Offset: 0x0023D5FD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.scrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06006186 RID: 24966 RVA: 0x0023F424 File Offset: 0x0023D624
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.scrollbar.size = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange;
		this.scrollbar.value = this.KelvinToScrollPercentage(SaveGame.Instance.relativeTemperatureOverlaySliderValue);
		this.defaultButton.onClick += this.OnDefaultPressed;
	}

	// Token: 0x06006187 RID: 24967 RVA: 0x0023F47F File Offset: 0x0023D67F
	private void OnValueChanged(float data)
	{
		this.SetUserConfig(data);
	}

	// Token: 0x06006188 RID: 24968 RVA: 0x0023F488 File Offset: 0x0023D688
	private float KelvinToScrollPercentage(float kelvin)
	{
		kelvin -= TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature;
		if (kelvin < 1f)
		{
			kelvin = 1f;
		}
		return Mathf.Clamp01(kelvin / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange);
	}

	// Token: 0x06006189 RID: 24969 RVA: 0x0023F4B0 File Offset: 0x0023D6B0
	private void SetUserConfig(float scrollPercentage)
	{
		float num = TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature + TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange * scrollPercentage;
		float num2 = num - TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		float num3 = num + TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		SimDebugView.Instance.user_temperatureThresholds[0] = num2;
		SimDebugView.Instance.user_temperatureThresholds[1] = num3;
		this.scrollBarRangeCenterText.SetText(GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeLowText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num2), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeHighText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num3), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		SaveGame.Instance.relativeTemperatureOverlaySliderValue = num;
	}

	// Token: 0x0600618A RID: 24970 RVA: 0x0023F55F File Offset: 0x0023D75F
	private void OnDefaultPressed()
	{
		this.scrollbar.value = this.KelvinToScrollPercentage(294.15f);
	}

	// Token: 0x04004141 RID: 16705
	public const float DEFAULT_TEMPERATURE = 294.15f;

	// Token: 0x04004142 RID: 16706
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x04004143 RID: 16707
	[SerializeField]
	private LocText scrollBarRangeLowText;

	// Token: 0x04004144 RID: 16708
	[SerializeField]
	private LocText scrollBarRangeCenterText;

	// Token: 0x04004145 RID: 16709
	[SerializeField]
	private LocText scrollBarRangeHighText;

	// Token: 0x04004146 RID: 16710
	[SerializeField]
	private KButton defaultButton;

	// Token: 0x04004147 RID: 16711
	private static float maxTemperatureRange = 700f;

	// Token: 0x04004148 RID: 16712
	private static float temperatureWindowSize = 200f;

	// Token: 0x04004149 RID: 16713
	private static float minimumSelectionTemperature = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
}
