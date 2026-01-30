using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007AB RID: 1963
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicWattageSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x0600339D RID: 13213 RVA: 0x001260E0 File Offset: 0x001242E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicWattageSensor>(-905833192, LogicWattageSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x001260FC File Offset: 0x001242FC
	private void OnCopySettings(object data)
	{
		LogicWattageSensor component = ((GameObject)data).GetComponent<LogicWattageSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x00126136 File Offset: 0x00124336
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x0012616C File Offset: 0x0012436C
	public void Sim200ms(float dt)
	{
		float wattsUsedByCircuit = Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(Grid.PosToCell(this)));
		if (wattsUsedByCircuit < 0f)
		{
			return;
		}
		this.currentWattage = wattsUsedByCircuit;
		if (this.activateOnHigherThan)
		{
			if ((this.currentWattage > this.thresholdWattage && !base.IsSwitchedOn) || (this.currentWattage <= this.thresholdWattage && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.currentWattage >= this.thresholdWattage && base.IsSwitchedOn) || (this.currentWattage < this.thresholdWattage && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x0012621B File Offset: 0x0012441B
	public float GetWattageUsed()
	{
		return this.currentWattage;
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x00126223 File Offset: 0x00124423
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x00126232 File Offset: 0x00124432
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x00126250 File Offset: 0x00124450
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

	// Token: 0x060033A5 RID: 13221 RVA: 0x001262D8 File Offset: 0x001244D8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x060033A6 RID: 13222 RVA: 0x0012632B File Offset: 0x0012452B
	// (set) Token: 0x060033A7 RID: 13223 RVA: 0x00126333 File Offset: 0x00124533
	public float Threshold
	{
		get
		{
			return this.thresholdWattage;
		}
		set
		{
			this.thresholdWattage = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x060033A8 RID: 13224 RVA: 0x00126343 File Offset: 0x00124543
	// (set) Token: 0x060033A9 RID: 13225 RVA: 0x0012634B File Offset: 0x0012454B
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnHigherThan;
		}
		set
		{
			this.activateOnHigherThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x060033AA RID: 13226 RVA: 0x0012635B File Offset: 0x0012455B
	public float CurrentValue
	{
		get
		{
			return this.GetWattageUsed();
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x060033AB RID: 13227 RVA: 0x00126363 File Offset: 0x00124563
	public float RangeMin
	{
		get
		{
			return this.minWattage;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x060033AC RID: 13228 RVA: 0x0012636B File Offset: 0x0012456B
	public float RangeMax
	{
		get
		{
			return this.maxWattage;
		}
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x00126373 File Offset: 0x00124573
	public float GetRangeMinInputField()
	{
		return this.minWattage;
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x0012637B File Offset: 0x0012457B
	public float GetRangeMaxInputField()
	{
		return this.maxWattage;
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x060033AF RID: 13231 RVA: 0x00126383 File Offset: 0x00124583
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.WATTAGESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x060033B0 RID: 13232 RVA: 0x0012638A File Offset: 0x0012458A
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x060033B1 RID: 13233 RVA: 0x00126391 File Offset: 0x00124591
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x060033B2 RID: 13234 RVA: 0x0012639D File Offset: 0x0012459D
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x001263A9 File Offset: 0x001245A9
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Watts, units);
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x001263B3 File Offset: 0x001245B3
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x001263BB File Offset: 0x001245BB
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x001263BE File Offset: 0x001245BE
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.ELECTRICAL.WATT;
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x060033B7 RID: 13239 RVA: 0x001263C5 File Offset: 0x001245C5
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x060033B8 RID: 13240 RVA: 0x001263C8 File Offset: 0x001245C8
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x060033B9 RID: 13241 RVA: 0x001263CC File Offset: 0x001245CC
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(5f, 5f),
				new NonLinearSlider.Range(35f, 1000f),
				new NonLinearSlider.Range(50f, 3000f),
				new NonLinearSlider.Range(10f, this.maxWattage)
			};
		}
	}

	// Token: 0x04001F37 RID: 7991
	[Serialize]
	public float thresholdWattage;

	// Token: 0x04001F38 RID: 7992
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x04001F39 RID: 7993
	[Serialize]
	public bool dirty = true;

	// Token: 0x04001F3A RID: 7994
	private readonly float minWattage;

	// Token: 0x04001F3B RID: 7995
	private readonly float maxWattage = 1.5f * Wire.GetMaxWattageAsFloat(Wire.WattageRating.Max50000);

	// Token: 0x04001F3C RID: 7996
	private float currentWattage;

	// Token: 0x04001F3D RID: 7997
	private bool wasOn;

	// Token: 0x04001F3E RID: 7998
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F3F RID: 7999
	private static readonly EventSystem.IntraObjectHandler<LogicWattageSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicWattageSensor>(delegate(LogicWattageSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
