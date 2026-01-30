using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007E1 RID: 2017
[SerializationConfig(MemberSerialization.OptIn)]
public class PressureSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x0600359C RID: 13724 RVA: 0x0012ED00 File Offset: 0x0012CF00
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			float num2 = Grid.Element[num].IsState(this.desiredState) ? Grid.Mass[num] : 0f;
			this.samples[this.sampleIdx] = num2;
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x0012EDC8 File Offset: 0x0012CFC8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x0600359E RID: 13726 RVA: 0x0012EE1B File Offset: 0x0012D01B
	// (set) Token: 0x0600359F RID: 13727 RVA: 0x0012EE23 File Offset: 0x0012D023
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060035A0 RID: 13728 RVA: 0x0012EE2C File Offset: 0x0012D02C
	// (set) Token: 0x060035A1 RID: 13729 RVA: 0x0012EE34 File Offset: 0x0012D034
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x060035A2 RID: 13730 RVA: 0x0012EE40 File Offset: 0x0012D040
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x060035A3 RID: 13731 RVA: 0x0012EE71 File Offset: 0x0012D071
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x060035A4 RID: 13732 RVA: 0x0012EE79 File Offset: 0x0012D079
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x0012EE81 File Offset: 0x0012D081
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x0012EE9F File Offset: 0x0012D09F
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x060035A7 RID: 13735 RVA: 0x0012EEBD File Offset: 0x0012D0BD
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060035A8 RID: 13736 RVA: 0x0012EEC4 File Offset: 0x0012D0C4
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060035A9 RID: 13737 RVA: 0x0012EECB File Offset: 0x0012D0CB
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060035AA RID: 13738 RVA: 0x0012EED7 File Offset: 0x0012D0D7
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x0012EEE4 File Offset: 0x0012D0E4
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat;
		if (this.desiredState == Element.State.Gas)
		{
			massFormat = GameUtil.MetricMassFormat.Gram;
		}
		else
		{
			massFormat = GameUtil.MetricMassFormat.Kilogram;
		}
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x0012EF10 File Offset: 0x0012D110
	public float ProcessedSliderValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input = Mathf.Round(input * 1000f) / 1000f;
		}
		else
		{
			input = Mathf.Round(input);
		}
		return input;
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x0012EF3A File Offset: 0x0012D13A
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x0012EF4F File Offset: 0x0012D14F
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060035AF RID: 13743 RVA: 0x0012EF5F File Offset: 0x0012D15F
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060035B0 RID: 13744 RVA: 0x0012EF62 File Offset: 0x0012D162
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060035B1 RID: 13745 RVA: 0x0012EF65 File Offset: 0x0012D165
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x0400207C RID: 8316
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x0400207D RID: 8317
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x0400207E RID: 8318
	public float rangeMin;

	// Token: 0x0400207F RID: 8319
	public float rangeMax = 1f;

	// Token: 0x04002080 RID: 8320
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04002081 RID: 8321
	private const int WINDOW_SIZE = 8;

	// Token: 0x04002082 RID: 8322
	private float[] samples = new float[8];

	// Token: 0x04002083 RID: 8323
	private int sampleIdx;
}
