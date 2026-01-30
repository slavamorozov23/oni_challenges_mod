using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007A8 RID: 1960
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTemperatureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06003368 RID: 13160 RVA: 0x00125798 File Offset: 0x00123998
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x001257BD File Offset: 0x001239BD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTemperatureSensor>(-905833192, LogicTemperatureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x001257D8 File Offset: 0x001239D8
	private void OnCopySettings(object data)
	{
		LogicTemperatureSensor component = ((GameObject)data).GetComponent<LogicTemperatureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x00125814 File Offset: 0x00123A14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x00125868 File Offset: 0x00123A68
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.temperatures[this.simUpdateCounter] = Grid.Temperature[i];
				this.simUpdateCounter++;
			}
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageTemp = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageTemp += this.temperatures[j];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp <= this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp >= this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x0012597F File Offset: 0x00123B7F
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x00125987 File Offset: 0x00123B87
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x00125996 File Offset: 0x00123B96
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x001259B4 File Offset: 0x00123BB4
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x00125A3C File Offset: 0x00123C3C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06003372 RID: 13170 RVA: 0x00125A8F File Offset: 0x00123C8F
	// (set) Token: 0x06003373 RID: 13171 RVA: 0x00125A97 File Offset: 0x00123C97
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06003374 RID: 13172 RVA: 0x00125AA7 File Offset: 0x00123CA7
	// (set) Token: 0x06003375 RID: 13173 RVA: 0x00125AAF File Offset: 0x00123CAF
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06003376 RID: 13174 RVA: 0x00125ABF File Offset: 0x00123CBF
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06003377 RID: 13175 RVA: 0x00125AC7 File Offset: 0x00123CC7
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06003378 RID: 13176 RVA: 0x00125ACF File Offset: 0x00123CCF
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x00125AD7 File Offset: 0x00123CD7
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x00125AE5 File Offset: 0x00123CE5
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x0600337B RID: 13179 RVA: 0x00125AF3 File Offset: 0x00123CF3
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x0600337C RID: 13180 RVA: 0x00125AFA File Offset: 0x00123CFA
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x0600337D RID: 13181 RVA: 0x00125B01 File Offset: 0x00123D01
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x0600337E RID: 13182 RVA: 0x00125B0D File Offset: 0x00123D0D
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x00125B19 File Offset: 0x00123D19
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, true);
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x00125B25 File Offset: 0x00123D25
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x00125B2D File Offset: 0x00123D2D
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x00125B38 File Offset: 0x00123D38
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

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06003383 RID: 13187 RVA: 0x00125B78 File Offset: 0x00123D78
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06003384 RID: 13188 RVA: 0x00125B7B File Offset: 0x00123D7B
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06003385 RID: 13189 RVA: 0x00125B80 File Offset: 0x00123D80
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

	// Token: 0x04001F1E RID: 7966
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001F1F RID: 7967
	private int simUpdateCounter;

	// Token: 0x04001F20 RID: 7968
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04001F21 RID: 7969
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001F22 RID: 7970
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001F23 RID: 7971
	public float minTemp;

	// Token: 0x04001F24 RID: 7972
	public float maxTemp = 373.15f;

	// Token: 0x04001F25 RID: 7973
	private const int NumFrameDelay = 8;

	// Token: 0x04001F26 RID: 7974
	private float[] temperatures = new float[8];

	// Token: 0x04001F27 RID: 7975
	private float averageTemp;

	// Token: 0x04001F28 RID: 7976
	private bool wasOn;

	// Token: 0x04001F29 RID: 7977
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F2A RID: 7978
	private static readonly EventSystem.IntraObjectHandler<LogicTemperatureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTemperatureSensor>(delegate(LogicTemperatureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
