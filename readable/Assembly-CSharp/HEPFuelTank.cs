using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000985 RID: 2437
public class HEPFuelTank : KMonoBehaviour, IFuelTank, IUserControlledCapacity
{
	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x060045D6 RID: 17878 RVA: 0x00193F10 File Offset: 0x00192110
	public IStorage Storage
	{
		get
		{
			return this.hepStorage;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x060045D7 RID: 17879 RVA: 0x00193F18 File Offset: 0x00192118
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x060045D8 RID: 17880 RVA: 0x00193F20 File Offset: 0x00192120
	public void DEBUG_FillTank()
	{
		this.hepStorage.Store(this.hepStorage.RemainingCapacity());
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x060045D9 RID: 17881 RVA: 0x00193F39 File Offset: 0x00192139
	// (set) Token: 0x060045DA RID: 17882 RVA: 0x00193F46 File Offset: 0x00192146
	public float UserMaxCapacity
	{
		get
		{
			return this.hepStorage.capacity;
		}
		set
		{
			this.hepStorage.capacity = value;
			base.Trigger(-795826715, this);
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x060045DB RID: 17883 RVA: 0x00193F60 File Offset: 0x00192160
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x060045DC RID: 17884 RVA: 0x00193F67 File Offset: 0x00192167
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x060045DD RID: 17885 RVA: 0x00193F6F File Offset: 0x0019216F
	public float AmountStored
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x060045DE RID: 17886 RVA: 0x00193F7C File Offset: 0x0019217C
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x060045DF RID: 17887 RVA: 0x00193F7F File Offset: 0x0019217F
	public LocString CapacityUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x060045E0 RID: 17888 RVA: 0x00193F88 File Offset: 0x00192188
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		this.m_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.m_meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<HEPFuelTank>(-795826715, HEPFuelTank.OnStorageChangedDelegate);
		base.Subscribe<HEPFuelTank>(-1837862626, HEPFuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x060045E1 RID: 17889 RVA: 0x00194031 File Offset: 0x00192231
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HEPFuelTank>(-905833192, HEPFuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x060045E2 RID: 17890 RVA: 0x0019404A File Offset: 0x0019224A
	private void OnStorageChange(object data)
	{
		this.m_meter.SetPositionPercent(this.hepStorage.Particles / Mathf.Max(1f, this.hepStorage.capacity));
	}

	// Token: 0x060045E3 RID: 17891 RVA: 0x00194078 File Offset: 0x00192278
	private void OnCopySettings(object data)
	{
		HEPFuelTank component = ((GameObject)data).GetComponent<HEPFuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x04002F0C RID: 12044
	[MyCmpReq]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04002F0D RID: 12045
	[Serialize]
	public float userMaxCapacity;

	// Token: 0x04002F0E RID: 12046
	public float physicalFuelCapacity;

	// Token: 0x04002F0F RID: 12047
	private MeterController m_meter;

	// Token: 0x04002F10 RID: 12048
	public bool consumeFuelOnLand;

	// Token: 0x04002F11 RID: 12049
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002F12 RID: 12050
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002F13 RID: 12051
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnCopySettings(data);
	});
}
