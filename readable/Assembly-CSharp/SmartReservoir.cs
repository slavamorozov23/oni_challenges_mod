using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B54 RID: 2900
[AddComponentMenu("KMonoBehaviour/scripts/SmartReservoir")]
public class SmartReservoir : KMonoBehaviour, IActivationRangeTarget, ISim200ms
{
	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x060055A1 RID: 21921 RVA: 0x001F3CEE File Offset: 0x001F1EEE
	public float PercentFull
	{
		get
		{
			return this.storage.MassStored() / this.storage.Capacity();
		}
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x001F3D07 File Offset: 0x001F1F07
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SmartReservoir>(-801688580, SmartReservoir.OnLogicValueChangedDelegate);
		base.Subscribe<SmartReservoir>(-592767678, SmartReservoir.UpdateLogicCircuitDelegate);
	}

	// Token: 0x060055A3 RID: 21923 RVA: 0x001F3D31 File Offset: 0x001F1F31
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SmartReservoir>(-905833192, SmartReservoir.OnCopySettingsDelegate);
	}

	// Token: 0x060055A4 RID: 21924 RVA: 0x001F3D4A File Offset: 0x001F1F4A
	public void Sim200ms(float dt)
	{
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x001F3D54 File Offset: 0x001F1F54
	private void UpdateLogicCircuit(object data)
	{
		float num = this.PercentFull * 100f;
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
		bool flag = this.activated;
		this.logicPorts.SendSignal(SmartReservoir.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x060055A6 RID: 21926 RVA: 0x001F3DB8 File Offset: 0x001F1FB8
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == SmartReservoir.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x001F3DF0 File Offset: 0x001F1FF0
	private void OnCopySettings(object data)
	{
		SmartReservoir component = ((GameObject)data).GetComponent<SmartReservoir>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x001F3E2A File Offset: 0x001F202A
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x060055A9 RID: 21929 RVA: 0x001F3E4E File Offset: 0x001F204E
	// (set) Token: 0x060055AA RID: 21930 RVA: 0x001F3E57 File Offset: 0x001F2057
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

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x060055AB RID: 21931 RVA: 0x001F3E68 File Offset: 0x001F2068
	// (set) Token: 0x060055AC RID: 21932 RVA: 0x001F3E71 File Offset: 0x001F2071
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

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x060055AD RID: 21933 RVA: 0x001F3E82 File Offset: 0x001F2082
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x060055AE RID: 21934 RVA: 0x001F3E89 File Offset: 0x001F2089
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x060055AF RID: 21935 RVA: 0x001F3E90 File Offset: 0x001F2090
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x060055B0 RID: 21936 RVA: 0x001F3E93 File Offset: 0x001F2093
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x060055B1 RID: 21937 RVA: 0x001F3E9F File Offset: 0x001F209F
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x060055B2 RID: 21938 RVA: 0x001F3EAB File Offset: 0x001F20AB
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x060055B3 RID: 21939 RVA: 0x001F3EB7 File Offset: 0x001F20B7
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x060055B4 RID: 21940 RVA: 0x001F3EC3 File Offset: 0x001F20C3
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x040039CF RID: 14799
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040039D0 RID: 14800
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040039D1 RID: 14801
	[Serialize]
	private int activateValue;

	// Token: 0x040039D2 RID: 14802
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x040039D3 RID: 14803
	[Serialize]
	private bool activated;

	// Token: 0x040039D4 RID: 14804
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x040039D5 RID: 14805
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040039D6 RID: 14806
	private MeterController logicMeter;

	// Token: 0x040039D7 RID: 14807
	public static readonly HashedString PORT_ID = "SmartReservoirLogicPort";

	// Token: 0x040039D8 RID: 14808
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040039D9 RID: 14809
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040039DA RID: 14810
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
