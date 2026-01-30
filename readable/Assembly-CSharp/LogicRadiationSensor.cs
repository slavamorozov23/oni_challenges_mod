using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicRadiationSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x0600330A RID: 13066 RVA: 0x00124694 File Offset: 0x00122894
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRadiationSensor>(-905833192, LogicRadiationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x001246B0 File Offset: 0x001228B0
	private void OnCopySettings(object data)
	{
		LogicRadiationSensor component = ((GameObject)data).GetComponent<LogicRadiationSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x001246EA File Offset: 0x001228EA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x00124720 File Offset: 0x00122920
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			this.radHistory[this.simUpdateCounter] = Grid.Radiation[i];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageRads = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageRads += this.radHistory[j];
		}
		this.averageRads /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageRads > this.thresholdRads && !base.IsSwitchedOn) || (this.averageRads <= this.thresholdRads && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageRads >= this.thresholdRads && base.IsSwitchedOn) || (this.averageRads < this.thresholdRads && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x00124825 File Offset: 0x00122A25
	public float GetAverageRads()
	{
		return this.averageRads;
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x0012482D File Offset: 0x00122A2D
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x0012483C File Offset: 0x00122A3C
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x0012485C File Offset: 0x00122A5C
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

	// Token: 0x06003312 RID: 13074 RVA: 0x001248E4 File Offset: 0x00122AE4
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06003313 RID: 13075 RVA: 0x00124937 File Offset: 0x00122B37
	// (set) Token: 0x06003314 RID: 13076 RVA: 0x0012493F File Offset: 0x00122B3F
	public float Threshold
	{
		get
		{
			return this.thresholdRads;
		}
		set
		{
			this.thresholdRads = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06003315 RID: 13077 RVA: 0x0012494F File Offset: 0x00122B4F
	// (set) Token: 0x06003316 RID: 13078 RVA: 0x00124957 File Offset: 0x00122B57
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

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06003317 RID: 13079 RVA: 0x00124967 File Offset: 0x00122B67
	public float CurrentValue
	{
		get
		{
			return this.GetAverageRads();
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06003318 RID: 13080 RVA: 0x0012496F File Offset: 0x00122B6F
	public float RangeMin
	{
		get
		{
			return this.minRads;
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06003319 RID: 13081 RVA: 0x00124977 File Offset: 0x00122B77
	public float RangeMax
	{
		get
		{
			return this.maxRads;
		}
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x0012497F File Offset: 0x00122B7F
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x0012498D File Offset: 0x00122B8D
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x0600331C RID: 13084 RVA: 0x0012499B File Offset: 0x00122B9B
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.RADIATIONSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x0600331D RID: 13085 RVA: 0x001249A2 File Offset: 0x00122BA2
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x0600331E RID: 13086 RVA: 0x001249A9 File Offset: 0x00122BA9
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x0600331F RID: 13087 RVA: 0x001249B5 File Offset: 0x00122BB5
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x001249C1 File Offset: 0x00122BC1
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x001249CA File Offset: 0x00122BCA
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x001249D2 File Offset: 0x00122BD2
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x001249D5 File Offset: 0x00122BD5
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06003324 RID: 13092 RVA: 0x001249E1 File Offset: 0x00122BE1
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06003325 RID: 13093 RVA: 0x001249E4 File Offset: 0x00122BE4
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06003326 RID: 13094 RVA: 0x001249E8 File Offset: 0x00122BE8
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(50f, 200f),
				new NonLinearSlider.Range(25f, 1000f),
				new NonLinearSlider.Range(25f, 5000f)
			};
		}
	}

	// Token: 0x04001EEA RID: 7914
	private int simUpdateCounter;

	// Token: 0x04001EEB RID: 7915
	[Serialize]
	public float thresholdRads = 280f;

	// Token: 0x04001EEC RID: 7916
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001EED RID: 7917
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001EEE RID: 7918
	public float minRads;

	// Token: 0x04001EEF RID: 7919
	public float maxRads = 5000f;

	// Token: 0x04001EF0 RID: 7920
	private const int NumFrameDelay = 8;

	// Token: 0x04001EF1 RID: 7921
	private float[] radHistory = new float[8];

	// Token: 0x04001EF2 RID: 7922
	private float averageRads;

	// Token: 0x04001EF3 RID: 7923
	private bool wasOn;

	// Token: 0x04001EF4 RID: 7924
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001EF5 RID: 7925
	private static readonly EventSystem.IntraObjectHandler<LogicRadiationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRadiationSensor>(delegate(LogicRadiationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
