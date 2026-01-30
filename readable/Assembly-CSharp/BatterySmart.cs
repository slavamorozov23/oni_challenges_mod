using System;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200070E RID: 1806
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class BatterySmart : Battery, IActivationRangeTarget
{
	// Token: 0x06002CEA RID: 11498 RVA: 0x00104E73 File Offset: 0x00103073
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<BatterySmart>(-905833192, BatterySmart.OnCopySettingsDelegate);
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x00104E8C File Offset: 0x0010308C
	private void OnCopySettings(object data)
	{
		BatterySmart component = ((GameObject)data).GetComponent<BatterySmart>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x00104EC6 File Offset: 0x001030C6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CreateLogicMeter();
		base.Subscribe<BatterySmart>(-801688580, BatterySmart.OnLogicValueChangedDelegate);
		base.Subscribe<BatterySmart>(-592767678, BatterySmart.UpdateLogicCircuitDelegate);
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x00104EF6 File Offset: 0x001030F6
	private void CreateLogicMeter()
	{
		this.logicMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002CEE RID: 11502 RVA: 0x00104F1B File Offset: 0x0010311B
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x06002CEF RID: 11503 RVA: 0x00104F2C File Offset: 0x0010312C
	private void UpdateLogicCircuit(object _)
	{
		float num = (float)Mathf.RoundToInt(base.PercentFull * 100f);
		if (this.activated)
		{
			if (num >= (float)this.deactivateValue)
			{
				this.activated = false;
			}
		}
		else if (num <= (float)this.activateValue)
		{
			this.activated = true;
		}
		bool isOperational = this.operational.IsOperational;
		bool flag = this.activated && isOperational;
		this.logicPorts.SendSignal(BatterySmart.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x00104FA4 File Offset: 0x001031A4
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == BatterySmart.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x00104FDC File Offset: 0x001031DC
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x00105000 File Offset: 0x00103200
	// (set) Token: 0x06002CF3 RID: 11507 RVA: 0x00105009 File Offset: 0x00103209
	public float ActivateValue
	{
		get
		{
			return (float)this.deactivateValue;
		}
		set
		{
			this.deactivateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x0010501A File Offset: 0x0010321A
	// (set) Token: 0x06002CF5 RID: 11509 RVA: 0x00105023 File Offset: 0x00103223
	public float DeactivateValue
	{
		get
		{
			return (float)this.activateValue;
		}
		set
		{
			this.activateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x00105034 File Offset: 0x00103234
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x0010503B File Offset: 0x0010323B
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x00105042 File Offset: 0x00103242
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x00105045 File Offset: 0x00103245
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06002CFA RID: 11514 RVA: 0x00105051 File Offset: 0x00103251
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06002CFB RID: 11515 RVA: 0x0010505D File Offset: 0x0010325D
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06002CFC RID: 11516 RVA: 0x00105069 File Offset: 0x00103269
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06002CFD RID: 11517 RVA: 0x00105075 File Offset: 0x00103275
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x04001AC0 RID: 6848
	public static readonly HashedString PORT_ID = "BatterySmartLogicPort";

	// Token: 0x04001AC1 RID: 6849
	[Serialize]
	private int activateValue;

	// Token: 0x04001AC2 RID: 6850
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x04001AC3 RID: 6851
	[Serialize]
	private bool activated;

	// Token: 0x04001AC4 RID: 6852
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04001AC5 RID: 6853
	private MeterController logicMeter;

	// Token: 0x04001AC6 RID: 6854
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001AC7 RID: 6855
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001AC8 RID: 6856
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001AC9 RID: 6857
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
