using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000792 RID: 1938
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCritterCountSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060031B3 RID: 12723 RVA: 0x0011ECFA File Offset: 0x0011CEFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectable = base.GetComponent<KSelectable>();
		base.Subscribe<LogicCritterCountSensor>(-905833192, LogicCritterCountSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x0011ED20 File Offset: 0x0011CF20
	private void OnCopySettings(object data)
	{
		LogicCritterCountSensor component = ((GameObject)data).GetComponent<LogicCritterCountSensor>();
		if (component != null)
		{
			this.countThreshold = component.countThreshold;
			this.activateOnGreaterThan = component.activateOnGreaterThan;
			this.countCritters = component.countCritters;
			this.countEggs = component.countEggs;
		}
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x0011ED72 File Offset: 0x0011CF72
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x0011EDA8 File Offset: 0x0011CFA8
	public void Sim200ms(float dt)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			this.currentCount = 0;
			if (this.countCritters)
			{
				this.currentCount += roomOfGameObject.cavity.creatures.Count;
			}
			if (this.countEggs)
			{
				this.currentCount += roomOfGameObject.cavity.eggs.Count;
			}
			bool state = this.activateOnGreaterThan ? (this.currentCount > this.countThreshold) : (this.currentCount < this.countThreshold);
			this.SetState(state);
			if (this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.selectable.RemoveStatusItem(this.roomStatusGUID, false);
				return;
			}
		}
		else
		{
			if (!this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.roomStatusGUID = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom, null);
			}
			this.SetState(false);
		}
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x0011EEC4 File Offset: 0x0011D0C4
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x0011EED3 File Offset: 0x0011D0D3
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x0011EEF4 File Offset: 0x0011D0F4
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.switchedOn)
			{
				component.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			component.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x0011EF94 File Offset: 0x0011D194
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x060031BB RID: 12731 RVA: 0x0011EFE7 File Offset: 0x0011D1E7
	// (set) Token: 0x060031BC RID: 12732 RVA: 0x0011EFF0 File Offset: 0x0011D1F0
	public float Threshold
	{
		get
		{
			return (float)this.countThreshold;
		}
		set
		{
			this.countThreshold = (int)value;
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x060031BD RID: 12733 RVA: 0x0011EFFA File Offset: 0x0011D1FA
	// (set) Token: 0x060031BE RID: 12734 RVA: 0x0011F002 File Offset: 0x0011D202
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnGreaterThan;
		}
		set
		{
			this.activateOnGreaterThan = value;
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x060031BF RID: 12735 RVA: 0x0011F00B File Offset: 0x0011D20B
	public float CurrentValue
	{
		get
		{
			return (float)this.currentCount;
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x060031C0 RID: 12736 RVA: 0x0011F014 File Offset: 0x0011D214
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x060031C1 RID: 12737 RVA: 0x0011F01B File Offset: 0x0011D21B
	public float RangeMax
	{
		get
		{
			return 64f;
		}
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x0011F022 File Offset: 0x0011D222
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x0011F02A File Offset: 0x0011D22A
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060031C4 RID: 12740 RVA: 0x0011F032 File Offset: 0x0011D232
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TITLE;
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060031C5 RID: 12741 RVA: 0x0011F039 File Offset: 0x0011D239
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.VALUE_NAME;
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060031C6 RID: 12742 RVA: 0x0011F040 File Offset: 0x0011D240
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060031C7 RID: 12743 RVA: 0x0011F04C File Offset: 0x0011D24C
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_BELOW;
		}
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x0011F058 File Offset: 0x0011D258
	public string Format(float value, bool units)
	{
		return value.ToString();
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x0011F061 File Offset: 0x0011D261
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060031CA RID: 12746 RVA: 0x0011F069 File Offset: 0x0011D269
	public float ProcessedInputValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x0011F071 File Offset: 0x0011D271
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060031CC RID: 12748 RVA: 0x0011F07D File Offset: 0x0011D27D
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x060031CD RID: 12749 RVA: 0x0011F080 File Offset: 0x0011D280
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x060031CE RID: 12750 RVA: 0x0011F083 File Offset: 0x0011D283
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001DF1 RID: 7665
	private bool wasOn;

	// Token: 0x04001DF2 RID: 7666
	[Serialize]
	public bool countEggs = true;

	// Token: 0x04001DF3 RID: 7667
	[Serialize]
	public bool countCritters = true;

	// Token: 0x04001DF4 RID: 7668
	[Serialize]
	public int countThreshold;

	// Token: 0x04001DF5 RID: 7669
	[Serialize]
	public bool activateOnGreaterThan = true;

	// Token: 0x04001DF6 RID: 7670
	[Serialize]
	public int currentCount;

	// Token: 0x04001DF7 RID: 7671
	private KSelectable selectable;

	// Token: 0x04001DF8 RID: 7672
	private Guid roomStatusGUID;

	// Token: 0x04001DF9 RID: 7673
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001DFA RID: 7674
	private static readonly EventSystem.IntraObjectHandler<LogicCritterCountSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCritterCountSensor>(delegate(LogicCritterCountSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
