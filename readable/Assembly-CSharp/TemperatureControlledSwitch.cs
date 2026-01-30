using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000816 RID: 2070
[SerializationConfig(MemberSerialization.OptIn)]
public class TemperatureControlledSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x0600381B RID: 14363 RVA: 0x0013A5AC File Offset: 0x001387AC
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x0013A5D1 File Offset: 0x001387D1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x0013A5F0 File Offset: 0x001387F0
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8)
		{
			this.temperatures[this.simUpdateCounter] = Grid.Temperature[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageTemp = 0f;
		for (int i = 0; i < 8; i++)
		{
			this.averageTemp += this.temperatures[i];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp > this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x0013A6E4 File Offset: 0x001388E4
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x0600381F RID: 14367 RVA: 0x0013A6EC File Offset: 0x001388EC
	// (set) Token: 0x06003820 RID: 14368 RVA: 0x0013A6F4 File Offset: 0x001388F4
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06003821 RID: 14369 RVA: 0x0013A6FD File Offset: 0x001388FD
	// (set) Token: 0x06003822 RID: 14370 RVA: 0x0013A705 File Offset: 0x00138905
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06003823 RID: 14371 RVA: 0x0013A70E File Offset: 0x0013890E
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06003824 RID: 14372 RVA: 0x0013A716 File Offset: 0x00138916
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06003825 RID: 14373 RVA: 0x0013A71E File Offset: 0x0013891E
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x0013A726 File Offset: 0x00138926
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x0013A734 File Offset: 0x00138934
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06003828 RID: 14376 RVA: 0x0013A742 File Offset: 0x00138942
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06003829 RID: 14377 RVA: 0x0013A749 File Offset: 0x00138949
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x0600382A RID: 14378 RVA: 0x0013A750 File Offset: 0x00138950
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600382B RID: 14379 RVA: 0x0013A75C File Offset: 0x0013895C
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x0013A768 File Offset: 0x00138968
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x0013A774 File Offset: 0x00138974
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x0013A77C File Offset: 0x0013897C
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x0013A784 File Offset: 0x00138984
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

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06003830 RID: 14384 RVA: 0x0013A7C4 File Offset: 0x001389C4
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.InputField;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06003831 RID: 14385 RVA: 0x0013A7C7 File Offset: 0x001389C7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06003832 RID: 14386 RVA: 0x0013A7CA File Offset: 0x001389CA
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x0400220E RID: 8718
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x0400220F RID: 8719
	private int simUpdateCounter;

	// Token: 0x04002210 RID: 8720
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04002211 RID: 8721
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04002212 RID: 8722
	public float minTemp;

	// Token: 0x04002213 RID: 8723
	public float maxTemp = 373.15f;

	// Token: 0x04002214 RID: 8724
	private const int NumFrameDelay = 8;

	// Token: 0x04002215 RID: 8725
	private float[] temperatures = new float[8];

	// Token: 0x04002216 RID: 8726
	private float averageTemp;
}
