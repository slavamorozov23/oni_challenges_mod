using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPressureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060032EC RID: 13036 RVA: 0x0012429B File Offset: 0x0012249B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicPressureSensor>(-905833192, LogicPressureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x001242B4 File Offset: 0x001224B4
	private void OnCopySettings(object data)
	{
		LogicPressureSensor component = ((GameObject)data).GetComponent<LogicPressureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060032EE RID: 13038 RVA: 0x001242EE File Offset: 0x001224EE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060032EF RID: 13039 RVA: 0x00124324 File Offset: 0x00122524
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

	// Token: 0x060032F0 RID: 13040 RVA: 0x001243EC File Offset: 0x001225EC
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x060032F1 RID: 13041 RVA: 0x001243FB File Offset: 0x001225FB
	// (set) Token: 0x060032F2 RID: 13042 RVA: 0x00124403 File Offset: 0x00122603
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

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x060032F3 RID: 13043 RVA: 0x0012440C File Offset: 0x0012260C
	// (set) Token: 0x060032F4 RID: 13044 RVA: 0x00124414 File Offset: 0x00122614
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

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x060032F5 RID: 13045 RVA: 0x00124420 File Offset: 0x00122620
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

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x060032F6 RID: 13046 RVA: 0x00124451 File Offset: 0x00122651
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x060032F7 RID: 13047 RVA: 0x00124459 File Offset: 0x00122659
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x00124461 File Offset: 0x00122661
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x0012447F File Offset: 0x0012267F
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x060032FA RID: 13050 RVA: 0x0012449D File Offset: 0x0012269D
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x060032FB RID: 13051 RVA: 0x001244A4 File Offset: 0x001226A4
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x060032FC RID: 13052 RVA: 0x001244B0 File Offset: 0x001226B0
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060032FD RID: 13053 RVA: 0x001244BC File Offset: 0x001226BC
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

	// Token: 0x060032FE RID: 13054 RVA: 0x001244E8 File Offset: 0x001226E8
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

	// Token: 0x060032FF RID: 13055 RVA: 0x00124512 File Offset: 0x00122712
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x00124527 File Offset: 0x00122727
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06003301 RID: 13057 RVA: 0x00124537 File Offset: 0x00122737
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06003302 RID: 13058 RVA: 0x0012453E File Offset: 0x0012273E
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06003303 RID: 13059 RVA: 0x00124541 File Offset: 0x00122741
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06003304 RID: 13060 RVA: 0x00124544 File Offset: 0x00122744
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x00124551 File Offset: 0x00122751
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x00124570 File Offset: 0x00122770
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

	// Token: 0x06003307 RID: 13063 RVA: 0x001245F8 File Offset: 0x001227F8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001EDF RID: 7903
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001EE0 RID: 7904
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001EE1 RID: 7905
	private bool wasOn;

	// Token: 0x04001EE2 RID: 7906
	public float rangeMin;

	// Token: 0x04001EE3 RID: 7907
	public float rangeMax = 1f;

	// Token: 0x04001EE4 RID: 7908
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04001EE5 RID: 7909
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001EE6 RID: 7910
	private float[] samples = new float[8];

	// Token: 0x04001EE7 RID: 7911
	private int sampleIdx;

	// Token: 0x04001EE8 RID: 7912
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001EE9 RID: 7913
	private static readonly EventSystem.IntraObjectHandler<LogicPressureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicPressureSensor>(delegate(LogicPressureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
