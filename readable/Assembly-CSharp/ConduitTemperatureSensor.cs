using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000734 RID: 1844
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitTemperatureSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06002E63 RID: 11875 RVA: 0x0010C490 File Offset: 0x0010A690
	private void GetContentsTemperature(out float temperature, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			temperature = contents.temperature;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			temperature = pickupable.PrimaryElement.Temperature;
			hasMass = true;
			return;
		}
		temperature = 0f;
		hasMass = false;
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06002E64 RID: 11876 RVA: 0x0010C530 File Offset: 0x0010A730
	public override float CurrentValue
	{
		get
		{
			float num;
			bool flag;
			this.GetContentsTemperature(out num, out flag);
			if (flag)
			{
				this.lastValue = num;
			}
			return this.lastValue;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06002E65 RID: 11877 RVA: 0x0010C557 File Offset: 0x0010A757
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06002E66 RID: 11878 RVA: 0x0010C55F File Offset: 0x0010A75F
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x0010C567 File Offset: 0x0010A767
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x0010C575 File Offset: 0x0010A775
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06002E69 RID: 11881 RVA: 0x0010C583 File Offset: 0x0010A783
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06002E6A RID: 11882 RVA: 0x0010C58A File Offset: 0x0010A78A
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06002E6B RID: 11883 RVA: 0x0010C591 File Offset: 0x0010A791
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06002E6C RID: 11884 RVA: 0x0010C59D File Offset: 0x0010A79D
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x0010C5A9 File Offset: 0x0010A7A9
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x0010C5B5 File Offset: 0x0010A7B5
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x0010C5BD File Offset: 0x0010A7BD
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06002E70 RID: 11888 RVA: 0x0010C5C8 File Offset: 0x0010A7C8
	public LocString ThresholdValueUnits()
	{
		LocString result = null;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			break;
		}
		return result;
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06002E71 RID: 11889 RVA: 0x0010C608 File Offset: 0x0010A808
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06002E72 RID: 11890 RVA: 0x0010C60B File Offset: 0x0010A80B
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06002E73 RID: 11891 RVA: 0x0010C610 File Offset: 0x0010A810
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(25f, 260f),
				new NonLinearSlider.Range(50f, 400f),
				new NonLinearSlider.Range(12f, 1500f),
				new NonLinearSlider.Range(13f, 10000f)
			};
		}
	}

	// Token: 0x04001B85 RID: 7045
	public float rangeMin;

	// Token: 0x04001B86 RID: 7046
	public float rangeMax = 373.15f;

	// Token: 0x04001B87 RID: 7047
	[Serialize]
	private float lastValue;
}
