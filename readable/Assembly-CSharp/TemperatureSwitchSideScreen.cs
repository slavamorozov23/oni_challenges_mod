using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000E87 RID: 3719
public class TemperatureSwitchSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06007665 RID: 30309 RVA: 0x002D2A88 File Offset: 0x002D0C88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.coolerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(false);
		};
		this.warmerToggle.onClick += delegate()
		{
			this.OnConditionButtonClicked(true);
		};
		LocText component = this.coolerToggle.transform.GetChild(0).GetComponent<LocText>();
		TMP_Text component2 = this.warmerToggle.transform.GetChild(0).GetComponent<LocText>();
		component.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.COLDER_BUTTON);
		component2.SetText(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.WARMER_BUTTON);
		Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
		sliderEvent.AddListener(new UnityAction<float>(this.OnTargetTemperatureChanged));
		this.targetTemperatureSlider.onValueChanged = sliderEvent;
	}

	// Token: 0x06007666 RID: 30310 RVA: 0x002D2B39 File Offset: 0x002D0D39
	public void Render200ms(float dt)
	{
		if (this.targetTemperatureSwitch == null)
		{
			return;
		}
		this.UpdateLabels();
	}

	// Token: 0x06007667 RID: 30311 RVA: 0x002D2B50 File Offset: 0x002D0D50
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TemperatureControlledSwitch>() != null;
	}

	// Token: 0x06007668 RID: 30312 RVA: 0x002D2B60 File Offset: 0x002D0D60
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targetTemperatureSwitch = target.GetComponent<TemperatureControlledSwitch>();
		if (this.targetTemperatureSwitch == null)
		{
			global::Debug.LogError("The gameObject received does not contain a TimedSwitch component");
			return;
		}
		this.UpdateLabels();
		this.UpdateTargetTemperatureLabel();
		this.OnConditionButtonClicked(this.targetTemperatureSwitch.activateOnWarmerThan);
	}

	// Token: 0x06007669 RID: 30313 RVA: 0x002D2BC3 File Offset: 0x002D0DC3
	private void OnTargetTemperatureChanged(float new_value)
	{
		this.targetTemperatureSwitch.thresholdTemperature = new_value;
		this.UpdateTargetTemperatureLabel();
	}

	// Token: 0x0600766A RID: 30314 RVA: 0x002D2BD8 File Offset: 0x002D0DD8
	private void OnConditionButtonClicked(bool isWarmer)
	{
		this.targetTemperatureSwitch.activateOnWarmerThan = isWarmer;
		if (isWarmer)
		{
			this.coolerToggle.isOn = false;
			this.warmerToggle.isOn = true;
			this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
			this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
			return;
		}
		this.coolerToggle.isOn = true;
		this.warmerToggle.isOn = false;
		this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
	}

	// Token: 0x0600766B RID: 30315 RVA: 0x002D2C69 File Offset: 0x002D0E69
	private void UpdateTargetTemperatureLabel()
	{
		this.targetTemperature.text = GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.thresholdTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
	}

	// Token: 0x0600766C RID: 30316 RVA: 0x002D2C8A File Offset: 0x002D0E8A
	private void UpdateLabels()
	{
		this.currentTemperature.text = string.Format(UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.CURRENT_TEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x040051EF RID: 20975
	private TemperatureControlledSwitch targetTemperatureSwitch;

	// Token: 0x040051F0 RID: 20976
	[SerializeField]
	private LocText currentTemperature;

	// Token: 0x040051F1 RID: 20977
	[SerializeField]
	private LocText targetTemperature;

	// Token: 0x040051F2 RID: 20978
	[SerializeField]
	private KToggle coolerToggle;

	// Token: 0x040051F3 RID: 20979
	[SerializeField]
	private KToggle warmerToggle;

	// Token: 0x040051F4 RID: 20980
	[SerializeField]
	private KSlider targetTemperatureSlider;
}
