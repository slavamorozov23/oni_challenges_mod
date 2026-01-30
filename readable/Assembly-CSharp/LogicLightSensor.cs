using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200079E RID: 1950
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060032A0 RID: 12960 RVA: 0x001234C0 File Offset: 0x001216C0
	private void OnCopySettings(object data)
	{
		LogicLightSensor component = ((GameObject)data).GetComponent<LogicLightSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x001234FA File Offset: 0x001216FA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicLightSensor>(-905833192, LogicLightSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x00123513 File Offset: 0x00121713
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x00123548 File Offset: 0x00121748
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 4)
		{
			this.levels[this.simUpdateCounter] = (float)Grid.LightIntensity[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageBrightness = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.averageBrightness += this.levels[i];
		}
		this.averageBrightness /= 4f;
		if (this.activateOnBrighterThan)
		{
			if ((this.averageBrightness > this.thresholdBrightness && !base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageBrightness > this.thresholdBrightness && base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x0012363D File Offset: 0x0012183D
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x0012364C File Offset: 0x0012184C
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x0012366C File Offset: 0x0012186C
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

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x060032A7 RID: 12967 RVA: 0x001236F3 File Offset: 0x001218F3
	// (set) Token: 0x060032A8 RID: 12968 RVA: 0x001236FB File Offset: 0x001218FB
	public float Threshold
	{
		get
		{
			return this.thresholdBrightness;
		}
		set
		{
			this.thresholdBrightness = value;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x060032A9 RID: 12969 RVA: 0x00123704 File Offset: 0x00121904
	// (set) Token: 0x060032AA RID: 12970 RVA: 0x0012370C File Offset: 0x0012190C
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnBrighterThan;
		}
		set
		{
			this.activateOnBrighterThan = value;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x060032AB RID: 12971 RVA: 0x00123715 File Offset: 0x00121915
	public float CurrentValue
	{
		get
		{
			return this.averageBrightness;
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060032AC RID: 12972 RVA: 0x0012371D File Offset: 0x0012191D
	public float RangeMin
	{
		get
		{
			return this.minBrightness;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060032AD RID: 12973 RVA: 0x00123725 File Offset: 0x00121925
	public float RangeMax
	{
		get
		{
			return this.maxBrightness;
		}
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x0012372D File Offset: 0x0012192D
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x00123735 File Offset: 0x00121935
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060032B0 RID: 12976 RVA: 0x0012373D File Offset: 0x0012193D
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.BRIGHTNESSSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x060032B1 RID: 12977 RVA: 0x00123744 File Offset: 0x00121944
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x060032B2 RID: 12978 RVA: 0x0012374B File Offset: 0x0012194B
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x060032B3 RID: 12979 RVA: 0x00123757 File Offset: 0x00121957
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x00123763 File Offset: 0x00121963
	public string Format(float value, bool units)
	{
		if (units)
		{
			return GameUtil.GetFormattedLux((int)value);
		}
		return string.Format("{0}", (int)value);
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x00123781 File Offset: 0x00121981
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x00123789 File Offset: 0x00121989
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x0012378C File Offset: 0x0012198C
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.LIGHT.LUX;
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x060032B8 RID: 12984 RVA: 0x00123793 File Offset: 0x00121993
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060032B9 RID: 12985 RVA: 0x00123796 File Offset: 0x00121996
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060032BA RID: 12986 RVA: 0x00123799 File Offset: 0x00121999
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x001237A8 File Offset: 0x001219A8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001EB6 RID: 7862
	private int simUpdateCounter;

	// Token: 0x04001EB7 RID: 7863
	[Serialize]
	public float thresholdBrightness = 280f;

	// Token: 0x04001EB8 RID: 7864
	[Serialize]
	public bool activateOnBrighterThan = true;

	// Token: 0x04001EB9 RID: 7865
	public float minBrightness;

	// Token: 0x04001EBA RID: 7866
	public float maxBrightness = 15000f;

	// Token: 0x04001EBB RID: 7867
	private const int NumFrameDelay = 4;

	// Token: 0x04001EBC RID: 7868
	private float[] levels = new float[4];

	// Token: 0x04001EBD RID: 7869
	private float averageBrightness;

	// Token: 0x04001EBE RID: 7870
	private bool wasOn;

	// Token: 0x04001EBF RID: 7871
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001EC0 RID: 7872
	private static readonly EventSystem.IntraObjectHandler<LogicLightSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicLightSensor>(delegate(LogicLightSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
