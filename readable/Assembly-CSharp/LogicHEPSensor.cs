using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200079C RID: 1948
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHEPSensor : Switch, ISaveLoadable, IThresholdSwitch, ISimEveryTick
{
	// Token: 0x06003278 RID: 12920 RVA: 0x00122B46 File Offset: 0x00120D46
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicHEPSensor>(-905833192, LogicHEPSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x00122B60 File Offset: 0x00120D60
	private void OnCopySettings(object data)
	{
		LogicHEPSensor component = ((GameObject)data).GetComponent<LogicHEPSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x00122B9C File Offset: 0x00120D9C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x00122C05 File Offset: 0x00120E05
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x00122C38 File Offset: 0x00120E38
	public void SimEveryTick(float dt)
	{
		if (this.waitForLogicTick)
		{
			return;
		}
		Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
		ListPool<ScenePartitionerEntry, LogicHEPSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicHEPSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		float num = 0f;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			HighEnergyParticle component = (scenePartitionerEntry.obj as KCollider2D).gameObject.GetComponent<HighEnergyParticle>();
			if (!(component == null) && component.isCollideable)
			{
				num += component.payload;
			}
		}
		pooledList.Recycle();
		this.foundPayload = num;
		bool flag = (this.activateOnHigherThan && num > this.thresholdPayload) || (!this.activateOnHigherThan && num < this.thresholdPayload);
		if (flag != this.switchedOn)
		{
			this.waitForLogicTick = true;
		}
		this.SetState(flag);
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x00122D44 File Offset: 0x00120F44
	private void LogicTick()
	{
		this.waitForLogicTick = false;
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x00122D4D File Offset: 0x00120F4D
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x00122D5C File Offset: 0x00120F5C
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x00122D7C File Offset: 0x00120F7C
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

	// Token: 0x06003281 RID: 12929 RVA: 0x00122E04 File Offset: 0x00121004
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06003282 RID: 12930 RVA: 0x00122E57 File Offset: 0x00121057
	// (set) Token: 0x06003283 RID: 12931 RVA: 0x00122E5F File Offset: 0x0012105F
	public float Threshold
	{
		get
		{
			return this.thresholdPayload;
		}
		set
		{
			this.thresholdPayload = value;
			this.dirty = true;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06003284 RID: 12932 RVA: 0x00122E6F File Offset: 0x0012106F
	// (set) Token: 0x06003285 RID: 12933 RVA: 0x00122E77 File Offset: 0x00121077
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

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06003286 RID: 12934 RVA: 0x00122E87 File Offset: 0x00121087
	public float CurrentValue
	{
		get
		{
			return this.foundPayload;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06003287 RID: 12935 RVA: 0x00122E8F File Offset: 0x0012108F
	public float RangeMin
	{
		get
		{
			return this.minPayload;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06003288 RID: 12936 RVA: 0x00122E97 File Offset: 0x00121097
	public float RangeMax
	{
		get
		{
			return this.maxPayload;
		}
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x00122E9F File Offset: 0x0012109F
	public float GetRangeMinInputField()
	{
		return this.minPayload;
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x00122EA7 File Offset: 0x001210A7
	public float GetRangeMaxInputField()
	{
		return this.maxPayload;
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x0600328B RID: 12939 RVA: 0x00122EAF File Offset: 0x001210AF
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.HEPSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x0600328C RID: 12940 RVA: 0x00122EB6 File Offset: 0x001210B6
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS;
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x0600328D RID: 12941 RVA: 0x00122EBD File Offset: 0x001210BD
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x0600328E RID: 12942 RVA: 0x00122EC9 File Offset: 0x001210C9
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x00122ED5 File Offset: 0x001210D5
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedHighEnergyParticles(value, GameUtil.TimeSlice.None, units);
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x00122EDF File Offset: 0x001210DF
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x00122EE7 File Offset: 0x001210E7
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x00122EEA File Offset: 0x001210EA
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06003293 RID: 12947 RVA: 0x00122EF1 File Offset: 0x001210F1
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06003294 RID: 12948 RVA: 0x00122EF4 File Offset: 0x001210F4
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06003295 RID: 12949 RVA: 0x00122EF8 File Offset: 0x001210F8
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(30f, 50f),
				new NonLinearSlider.Range(30f, 200f),
				new NonLinearSlider.Range(40f, 500f)
			};
		}
	}

	// Token: 0x04001E9C RID: 7836
	[Serialize]
	public float thresholdPayload;

	// Token: 0x04001E9D RID: 7837
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x04001E9E RID: 7838
	[Serialize]
	public bool dirty = true;

	// Token: 0x04001E9F RID: 7839
	private readonly float minPayload;

	// Token: 0x04001EA0 RID: 7840
	private readonly float maxPayload = 500f;

	// Token: 0x04001EA1 RID: 7841
	private float foundPayload;

	// Token: 0x04001EA2 RID: 7842
	private bool waitForLogicTick;

	// Token: 0x04001EA3 RID: 7843
	private bool wasOn;

	// Token: 0x04001EA4 RID: 7844
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001EA5 RID: 7845
	private static readonly EventSystem.IntraObjectHandler<LogicHEPSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicHEPSensor>(delegate(LogicHEPSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
