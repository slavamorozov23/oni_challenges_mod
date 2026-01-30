using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E8B RID: 3723
public class TimerSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x06007695 RID: 30357 RVA: 0x002D380B File Offset: 0x002D1A0B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderOnDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.ON;
		this.labelHeaderOffDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.OFF;
	}

	// Token: 0x06007696 RID: 30358 RVA: 0x002D3840 File Offset: 0x002D1A40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.modeButton.onClick += delegate()
		{
			this.ToggleMode();
		};
		this.resetButton.onClick += this.ResetTimer;
		this.onDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.onDurationNumberInput.currentValue, this.onDurationSlider);
		};
		this.offDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.offDurationNumberInput.currentValue, this.offDurationSlider);
		};
		this.onDurationSlider.wholeNumbers = false;
		this.offDurationSlider.wholeNumbers = false;
	}

	// Token: 0x06007697 RID: 30359 RVA: 0x002D38C7 File Offset: 0x002D1AC7
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimerSensor>() != null;
	}

	// Token: 0x06007698 RID: 30360 RVA: 0x002D38D8 File Offset: 0x002D1AD8
	public override void SetTarget(GameObject target)
	{
		this.greenActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.redActiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimerSensor>();
		this.onDurationSlider.onValueChanged.RemoveAllListeners();
		this.offDurationSlider.onValueChanged.RemoveAllListeners();
		this.cyclesMode = this.targetTimedSwitch.displayCyclesMode;
		this.UpdateVisualsForNewTarget();
		this.ReconfigureRingVisuals();
		this.onDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.offDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x06007699 RID: 30361 RVA: 0x002D39AC File Offset: 0x002D1BAC
	private void UpdateVisualsForNewTarget()
	{
		float onDuration = this.targetTimedSwitch.onDuration;
		float offDuration = this.targetTimedSwitch.offDuration;
		bool displayCyclesMode = this.targetTimedSwitch.displayCyclesMode;
		if (displayCyclesMode)
		{
			this.onDurationSlider.minValue = this.minCycles;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxCycles;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 2;
			this.offDurationSlider.minValue = this.minCycles;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxCycles;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 2;
			this.onDurationSlider.value = onDuration / 600f;
			this.offDurationSlider.value = offDuration / 600f;
			this.onDurationNumberInput.SetAmount(onDuration / 600f);
			this.offDurationNumberInput.SetAmount(offDuration / 600f);
		}
		else
		{
			this.onDurationSlider.minValue = this.minSeconds;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxSeconds;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 1;
			this.offDurationSlider.minValue = this.minSeconds;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxSeconds;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 1;
			this.onDurationSlider.value = onDuration;
			this.offDurationSlider.value = offDuration;
			this.onDurationNumberInput.SetAmount(onDuration);
			this.offDurationNumberInput.SetAmount(offDuration);
		}
		this.modeButton.GetComponentInChildren<LocText>().text = (displayCyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x0600769A RID: 30362 RVA: 0x002D3BEC File Offset: 0x002D1DEC
	private void ToggleMode()
	{
		this.cyclesMode = !this.cyclesMode;
		this.targetTimedSwitch.displayCyclesMode = this.cyclesMode;
		float num = this.onDurationSlider.value;
		float num2 = this.offDurationSlider.value;
		if (this.cyclesMode)
		{
			num = this.onDurationSlider.value / 600f;
			num2 = this.offDurationSlider.value / 600f;
		}
		else
		{
			num = this.onDurationSlider.value * 600f;
			num2 = this.offDurationSlider.value * 600f;
		}
		this.onDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
		this.onDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
		this.onDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.offDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
		this.offDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
		this.offDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.onDurationSlider.value = num;
		this.offDurationSlider.value = num2;
		this.onDurationNumberInput.SetAmount(num);
		this.offDurationNumberInput.SetAmount(num2);
		this.modeButton.GetComponentInChildren<LocText>().text = (this.cyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x0600769B RID: 30363 RVA: 0x002D3DE8 File Offset: 0x002D1FE8
	private void ChangeSetting()
	{
		this.targetTimedSwitch.onDuration = (this.cyclesMode ? (this.onDurationSlider.value * 600f) : this.onDurationSlider.value);
		this.targetTimedSwitch.offDuration = (this.cyclesMode ? (this.offDurationSlider.value * 600f) : this.offDurationSlider.value);
		this.ReconfigureRingVisuals();
		this.onDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.onDuration / 600f).ToString("F2") : this.targetTimedSwitch.onDuration.ToString());
		this.offDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.offDuration / 600f).ToString("F2") : this.targetTimedSwitch.offDuration.ToString());
		this.onDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.GREEN_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.onDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.onDuration, "F0")));
		this.offDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.RED_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.offDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.offDuration, "F0")));
		if (this.phaseLength == 0f)
		{
			this.timeLeft.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.DISABLED;
			if (this.targetTimedSwitch.IsSwitchedOn)
			{
				this.greenActiveZone.fillAmount = 1f;
				this.redActiveZone.fillAmount = 0f;
			}
			else
			{
				this.greenActiveZone.fillAmount = 0f;
				this.redActiveZone.fillAmount = 1f;
			}
			this.targetTimedSwitch.timeElapsedInCurrentState = 0f;
			this.currentTimeMarker.rotation = Quaternion.identity;
			this.currentTimeMarker.Rotate(0f, 0f, 0f);
		}
	}

	// Token: 0x0600769C RID: 30364 RVA: 0x002D4030 File Offset: 0x002D2230
	private void ReconfigureRingVisuals()
	{
		this.phaseLength = this.targetTimedSwitch.onDuration + this.targetTimedSwitch.offDuration;
		this.greenActiveZone.fillAmount = this.targetTimedSwitch.onDuration / this.phaseLength;
		this.redActiveZone.fillAmount = this.targetTimedSwitch.offDuration / this.phaseLength;
	}

	// Token: 0x0600769D RID: 30365 RVA: 0x002D4094 File Offset: 0x002D2294
	public void RenderEveryTick(float dt)
	{
		if (this.phaseLength == 0f)
		{
			return;
		}
		float timeElapsedInCurrentState = this.targetTimedSwitch.timeElapsedInCurrentState;
		if (this.cyclesMode)
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedCycles(timeElapsedInCurrentState, "F2", false), GameUtil.GetFormattedCycles(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F2", false));
		}
		else
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedTime(timeElapsedInCurrentState, "F1"), GameUtil.GetFormattedTime(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F1"));
		}
		this.currentTimeMarker.rotation = Quaternion.identity;
		if (this.targetTimedSwitch.IsSwitchedOn)
		{
			this.currentTimeMarker.Rotate(0f, 0f, this.targetTimedSwitch.timeElapsedInCurrentState / this.phaseLength * -360f);
			return;
		}
		this.currentTimeMarker.Rotate(0f, 0f, (this.targetTimedSwitch.onDuration + this.targetTimedSwitch.timeElapsedInCurrentState) / this.phaseLength * -360f);
	}

	// Token: 0x0600769E RID: 30366 RVA: 0x002D41F4 File Offset: 0x002D23F4
	private void UpdateDurationValueFromTextInput(float newValue, KSlider slider)
	{
		if (newValue < slider.minValue)
		{
			newValue = slider.minValue;
		}
		if (newValue > slider.maxValue)
		{
			newValue = slider.maxValue;
		}
		slider.value = newValue;
		NonLinearSlider nonLinearSlider = slider as NonLinearSlider;
		if (nonLinearSlider != null)
		{
			slider.value = nonLinearSlider.GetPercentageFromValue(newValue);
			return;
		}
		slider.value = newValue;
	}

	// Token: 0x0600769F RID: 30367 RVA: 0x002D424F File Offset: 0x002D244F
	private void ResetTimer()
	{
		this.targetTimedSwitch.ResetTimer();
	}

	// Token: 0x0400520E RID: 21006
	public Image greenActiveZone;

	// Token: 0x0400520F RID: 21007
	public Image redActiveZone;

	// Token: 0x04005210 RID: 21008
	private LogicTimerSensor targetTimedSwitch;

	// Token: 0x04005211 RID: 21009
	public KToggle modeButton;

	// Token: 0x04005212 RID: 21010
	public KButton resetButton;

	// Token: 0x04005213 RID: 21011
	public KSlider onDurationSlider;

	// Token: 0x04005214 RID: 21012
	[SerializeField]
	private KNumberInputField onDurationNumberInput;

	// Token: 0x04005215 RID: 21013
	public KSlider offDurationSlider;

	// Token: 0x04005216 RID: 21014
	[SerializeField]
	private KNumberInputField offDurationNumberInput;

	// Token: 0x04005217 RID: 21015
	public RectTransform endIndicator;

	// Token: 0x04005218 RID: 21016
	public RectTransform currentTimeMarker;

	// Token: 0x04005219 RID: 21017
	public LocText labelHeaderOnDuration;

	// Token: 0x0400521A RID: 21018
	public LocText labelHeaderOffDuration;

	// Token: 0x0400521B RID: 21019
	public LocText labelValueOnDuration;

	// Token: 0x0400521C RID: 21020
	public LocText labelValueOffDuration;

	// Token: 0x0400521D RID: 21021
	public LocText timeLeft;

	// Token: 0x0400521E RID: 21022
	public float phaseLength;

	// Token: 0x0400521F RID: 21023
	private bool cyclesMode;

	// Token: 0x04005220 RID: 21024
	[SerializeField]
	private float minSeconds;

	// Token: 0x04005221 RID: 21025
	[SerializeField]
	private float maxSeconds = 600f;

	// Token: 0x04005222 RID: 21026
	[SerializeField]
	private float minCycles;

	// Token: 0x04005223 RID: 21027
	[SerializeField]
	private float maxCycles = 10f;

	// Token: 0x04005224 RID: 21028
	private const int CYCLEMODE_DECIMALS = 2;

	// Token: 0x04005225 RID: 21029
	private const int SECONDSMODE_DECIMALS = 1;
}
