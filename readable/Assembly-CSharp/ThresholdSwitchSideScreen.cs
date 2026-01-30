using System;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000E89 RID: 3721
public class ThresholdSwitchSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06007677 RID: 30327 RVA: 0x002D2E18 File Offset: 0x002D1018
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.aboveToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(true);
		};
		this.belowToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(false);
		};
		LocText component = this.aboveToggle.transform.GetChild(0).GetComponent<LocText>();
		TMP_Text component2 = this.belowToggle.transform.GetChild(0).GetComponent<LocText>();
		component.SetText(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.ABOVE_BUTTON);
		component2.SetText(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BELOW_BUTTON);
		this.thresholdSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.thresholdSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.thresholdSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value)));
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x06007678 RID: 30328 RVA: 0x002D2F0D File Offset: 0x002D110D
	public void Render200ms(float dt)
	{
		if (this.target == null)
		{
			this.target = null;
			return;
		}
		this.UpdateLabels();
	}

	// Token: 0x06007679 RID: 30329 RVA: 0x002D2F2B File Offset: 0x002D112B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IThresholdSwitch>() != null;
	}

	// Token: 0x0600767A RID: 30330 RVA: 0x002D2F38 File Offset: 0x002D1138
	public override void SetTarget(GameObject new_target)
	{
		this.target = null;
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target;
		this.thresholdSwitch = this.target.GetComponent<IThresholdSwitch>();
		if (this.thresholdSwitch == null)
		{
			this.target = null;
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.UpdateLabels();
		if (this.target.GetComponent<IThresholdSwitch>().LayoutType == ThresholdScreenLayoutType.SliderBar)
		{
			this.thresholdSlider.gameObject.SetActive(true);
			this.thresholdSlider.minValue = 0f;
			this.thresholdSlider.maxValue = 100f;
			this.thresholdSlider.SetRanges(this.thresholdSwitch.GetRanges);
			this.thresholdSlider.value = this.thresholdSlider.GetPercentageFromValue(this.thresholdSwitch.Threshold);
			this.thresholdSlider.GetComponentInChildren<ToolTip>();
		}
		else
		{
			this.thresholdSlider.gameObject.SetActive(false);
		}
		MultiToggle incrementMinorToggle = this.incrementMinor.GetComponent<MultiToggle>();
		incrementMinorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold + (float)this.thresholdSwitch.IncrementScale);
			incrementMinorToggle.ChangeState(1);
		};
		incrementMinorToggle.onStopHold = delegate()
		{
			incrementMinorToggle.ChangeState(0);
		};
		MultiToggle incrementMajorToggle = this.incrementMajor.GetComponent<MultiToggle>();
		incrementMajorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold + 10f * (float)this.thresholdSwitch.IncrementScale);
			incrementMajorToggle.ChangeState(1);
		};
		incrementMajorToggle.onStopHold = delegate()
		{
			incrementMajorToggle.ChangeState(0);
		};
		MultiToggle decrementMinorToggle = this.decrementMinor.GetComponent<MultiToggle>();
		decrementMinorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold - (float)this.thresholdSwitch.IncrementScale);
			decrementMinorToggle.ChangeState(1);
		};
		decrementMinorToggle.onStopHold = delegate()
		{
			decrementMinorToggle.ChangeState(0);
		};
		MultiToggle decrementMajorToggle = this.decrementMajor.GetComponent<MultiToggle>();
		decrementMajorToggle.onClick = delegate()
		{
			this.UpdateThresholdValue(this.thresholdSwitch.Threshold - 10f * (float)this.thresholdSwitch.IncrementScale);
			decrementMajorToggle.ChangeState(1);
		};
		decrementMajorToggle.onStopHold = delegate()
		{
			decrementMajorToggle.ChangeState(0);
		};
		this.unitsLabel.text = this.thresholdSwitch.ThresholdValueUnits();
		this.numberInput.minValue = this.thresholdSwitch.GetRangeMinInputField();
		this.numberInput.maxValue = this.thresholdSwitch.GetRangeMaxInputField();
		this.numberInput.Activate();
		this.UpdateTargetThresholdLabel();
		this.OnConditionButtonClicked(this.thresholdSwitch.ActivateAboveThreshold);
	}

	// Token: 0x0600767B RID: 30331 RVA: 0x002D31A3 File Offset: 0x002D13A3
	private void OnThresholdValueChanged(float new_value)
	{
		this.thresholdSwitch.Threshold = new_value;
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x0600767C RID: 30332 RVA: 0x002D31B8 File Offset: 0x002D13B8
	private void OnConditionButtonClicked(bool activate_above_threshold)
	{
		this.thresholdSwitch.ActivateAboveThreshold = activate_above_threshold;
		if (activate_above_threshold)
		{
			this.belowToggle.isOn = true;
			this.aboveToggle.isOn = false;
			this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
			this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		}
		else
		{
			this.belowToggle.isOn = false;
			this.aboveToggle.isOn = true;
			this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
			this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
		}
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x0600767D RID: 30333 RVA: 0x002D3250 File Offset: 0x002D1450
	private void UpdateTargetThresholdLabel()
	{
		this.numberInput.SetDisplayValue(this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, false) + this.thresholdSwitch.ThresholdValueUnits());
		if (this.thresholdSwitch.ActivateAboveThreshold)
		{
			this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.AboveToolTip, this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
			this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0f, 25f);
			return;
		}
		this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.BelowToolTip, this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
		this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0f, 25f);
	}

	// Token: 0x0600767E RID: 30334 RVA: 0x002D334E File Offset: 0x002D154E
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateThresholdValue(this.thresholdSwitch.ProcessedSliderValue(newValue));
	}

	// Token: 0x0600767F RID: 30335 RVA: 0x002D3362 File Offset: 0x002D1562
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateThresholdValue(this.thresholdSwitch.ProcessedInputValue(newValue));
	}

	// Token: 0x06007680 RID: 30336 RVA: 0x002D3378 File Offset: 0x002D1578
	private void UpdateThresholdValue(float newValue)
	{
		if (newValue < this.thresholdSwitch.RangeMin)
		{
			newValue = this.thresholdSwitch.RangeMin;
		}
		if (newValue > this.thresholdSwitch.RangeMax)
		{
			newValue = this.thresholdSwitch.RangeMax;
		}
		this.thresholdSwitch.Threshold = newValue;
		NonLinearSlider nonLinearSlider = this.thresholdSlider;
		if (nonLinearSlider != null)
		{
			this.thresholdSlider.value = nonLinearSlider.GetPercentageFromValue(newValue);
		}
		else
		{
			this.thresholdSlider.value = newValue;
		}
		this.UpdateTargetThresholdLabel();
	}

	// Token: 0x06007681 RID: 30337 RVA: 0x002D33FD File Offset: 0x002D15FD
	private void UpdateLabels()
	{
		this.currentValue.text = string.Format(UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CURRENT_VALUE, this.thresholdSwitch.ThresholdValueName, this.thresholdSwitch.Format(this.thresholdSwitch.CurrentValue, true));
	}

	// Token: 0x06007682 RID: 30338 RVA: 0x002D343B File Offset: 0x002D163B
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return this.thresholdSwitch.Title;
		}
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
	}

	// Token: 0x040051F6 RID: 20982
	private GameObject target;

	// Token: 0x040051F7 RID: 20983
	private IThresholdSwitch thresholdSwitch;

	// Token: 0x040051F8 RID: 20984
	[SerializeField]
	private LocText currentValue;

	// Token: 0x040051F9 RID: 20985
	[SerializeField]
	private LocText thresholdValue;

	// Token: 0x040051FA RID: 20986
	[SerializeField]
	private KToggle aboveToggle;

	// Token: 0x040051FB RID: 20987
	[SerializeField]
	private KToggle belowToggle;

	// Token: 0x040051FC RID: 20988
	[Header("Slider")]
	[SerializeField]
	private NonLinearSlider thresholdSlider;

	// Token: 0x040051FD RID: 20989
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040051FE RID: 20990
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x040051FF RID: 20991
	[Header("Increment Buttons")]
	[SerializeField]
	private GameObject incrementMinor;

	// Token: 0x04005200 RID: 20992
	[SerializeField]
	private GameObject incrementMajor;

	// Token: 0x04005201 RID: 20993
	[SerializeField]
	private GameObject decrementMinor;

	// Token: 0x04005202 RID: 20994
	[SerializeField]
	private GameObject decrementMajor;
}
