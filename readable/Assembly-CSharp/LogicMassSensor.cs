using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200079F RID: 1951
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
	// Token: 0x060032BE RID: 12990 RVA: 0x00123848 File Offset: 0x00121A48
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x00123864 File Offset: 0x00121A64
	private void OnCopySettings(object data)
	{
		LogicMassSensor component = ((GameObject)data).GetComponent<LogicMassSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x001238A0 File Offset: 0x00121AA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisualState(true);
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", base.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new Action<object>(this.OnActivatorsChanged));
		base.OnToggle += this.SwitchToggled;
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x0012396E File Offset: 0x00121B6E
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x060032C2 RID: 12994 RVA: 0x001239A8 File Offset: 0x00121BA8
	private void Update()
	{
		this.toggleCooldown = Mathf.Max(0f, this.toggleCooldown - Time.deltaTime);
		if (this.toggleCooldown == 0f)
		{
			float currentValue = this.CurrentValue;
			if ((this.activateAboveThreshold ? (currentValue > this.threshold) : (currentValue < this.threshold)) != base.IsSwitchedOn)
			{
				this.Toggle();
				this.toggleCooldown = 0.15f;
			}
			this.UpdateVisualState(false);
		}
	}

	// Token: 0x060032C3 RID: 12995 RVA: 0x00123A24 File Offset: 0x00121C24
	private void OnSolidChanged(object data)
	{
		int i = Grid.CellAbove(this.NaturalBuildingCell());
		if (Grid.Solid[i])
		{
			this.massSolid = Grid.Mass[i];
			return;
		}
		this.massSolid = 0f;
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x00123A68 File Offset: 0x00121C68
	private void OnPickupablesChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (!(pickupable == null) && !pickupable.wasAbsorbed)
			{
				KPrefabID kprefabID = pickupable.KPrefabID;
				if (!kprefabID.HasTag(GameTags.Creature) || (kprefabID.HasTag(GameTags.Creatures.Walker) || kprefabID.HasTag(GameTags.Creatures.Hoverer) || kprefabID.HasTag(GameTags.Creatures.Flopping)))
				{
					num += pickupable.PrimaryElement.Mass;
				}
			}
		}
		pooledList.Recycle();
		this.massPickupables = num;
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x00123B54 File Offset: 0x00121D54
	private void OnActivatorsChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			FloorSwitchActivator floorSwitchActivator = pooledList[i].obj as FloorSwitchActivator;
			if (!(floorSwitchActivator == null))
			{
				num += floorSwitchActivator.PrimaryElement.Mass;
			}
		}
		pooledList.Recycle();
		this.massActivators = num;
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060032C6 RID: 12998 RVA: 0x00123BF0 File Offset: 0x00121DF0
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060032C7 RID: 12999 RVA: 0x00123BF7 File Offset: 0x00121DF7
	// (set) Token: 0x060032C8 RID: 13000 RVA: 0x00123BFF File Offset: 0x00121DFF
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

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x060032C9 RID: 13001 RVA: 0x00123C08 File Offset: 0x00121E08
	// (set) Token: 0x060032CA RID: 13002 RVA: 0x00123C10 File Offset: 0x00121E10
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

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x060032CB RID: 13003 RVA: 0x00123C19 File Offset: 0x00121E19
	public float CurrentValue
	{
		get
		{
			return this.massSolid + this.massPickupables + this.massActivators;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x060032CC RID: 13004 RVA: 0x00123C2F File Offset: 0x00121E2F
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x060032CD RID: 13005 RVA: 0x00123C37 File Offset: 0x00121E37
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x060032CE RID: 13006 RVA: 0x00123C3F File Offset: 0x00121E3F
	public float GetRangeMinInputField()
	{
		return this.rangeMin;
	}

	// Token: 0x060032CF RID: 13007 RVA: 0x00123C47 File Offset: 0x00121E47
	public float GetRangeMaxInputField()
	{
		return this.rangeMax;
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x00123C4F File Offset: 0x00121E4F
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x060032D1 RID: 13009 RVA: 0x00123C56 File Offset: 0x00121E56
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x060032D2 RID: 13010 RVA: 0x00123C62 File Offset: 0x00121E62
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x00123C70 File Offset: 0x00121E70
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.Kilogram;
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x00123C8F File Offset: 0x00121E8F
	public float ProcessedSliderValue(float input)
	{
		input = Mathf.Round(input);
		return input;
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x00123C9A File Offset: 0x00121E9A
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x00123C9D File Offset: 0x00121E9D
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(false);
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x060032D7 RID: 13015 RVA: 0x00123CA5 File Offset: 0x00121EA5
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x060032D8 RID: 13016 RVA: 0x00123CA8 File Offset: 0x00121EA8
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x060032D9 RID: 13017 RVA: 0x00123CAB File Offset: 0x00121EAB
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x00123CB8 File Offset: 0x00121EB8
	private void SwitchToggled(bool toggled_on)
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggled_on ? 1 : 0);
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x00123CD4 File Offset: 0x00121ED4
	private void UpdateVisualState(bool force = false)
	{
		bool flag = this.CurrentValue > this.threshold;
		if (flag != this.was_pressed || this.was_on != base.IsSwitchedOn || force)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (flag)
			{
				if (force)
				{
					component.Play(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(base.IsSwitchedOn ? "on_down_pre" : "off_down_pre", KAnim.PlayMode.Once, 1f, 0f);
					component.Queue(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			else if (force)
			{
				component.Play(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				component.Play(base.IsSwitchedOn ? "on_up_pre" : "off_up_pre", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.was_pressed = flag;
			this.was_on = base.IsSwitchedOn;
		}
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x00123E44 File Offset: 0x00122044
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001EC1 RID: 7873
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001EC2 RID: 7874
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001EC3 RID: 7875
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04001EC4 RID: 7876
	private bool was_pressed;

	// Token: 0x04001EC5 RID: 7877
	private bool was_on;

	// Token: 0x04001EC6 RID: 7878
	public float rangeMin;

	// Token: 0x04001EC7 RID: 7879
	public float rangeMax = 1f;

	// Token: 0x04001EC8 RID: 7880
	[Serialize]
	private float massSolid;

	// Token: 0x04001EC9 RID: 7881
	[Serialize]
	private float massPickupables;

	// Token: 0x04001ECA RID: 7882
	[Serialize]
	private float massActivators;

	// Token: 0x04001ECB RID: 7883
	private const float MIN_TOGGLE_TIME = 0.15f;

	// Token: 0x04001ECC RID: 7884
	private float toggleCooldown = 0.15f;

	// Token: 0x04001ECD RID: 7885
	private HandleVector<int>.Handle solidChangedEntry;

	// Token: 0x04001ECE RID: 7886
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04001ECF RID: 7887
	private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;

	// Token: 0x04001ED0 RID: 7888
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001ED1 RID: 7889
	private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>(delegate(LogicMassSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
