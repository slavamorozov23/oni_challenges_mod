using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B7B RID: 2939
public class FuelTank : KMonoBehaviour, IUserControlledCapacity, IFuelTank
{
	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x0600579C RID: 22428 RVA: 0x001FE3A9 File Offset: 0x001FC5A9
	public IStorage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x0600579D RID: 22429 RVA: 0x001FE3B1 File Offset: 0x001FC5B1
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x0600579E RID: 22430 RVA: 0x001FE3B9 File Offset: 0x001FC5B9
	// (set) Token: 0x0600579F RID: 22431 RVA: 0x001FE3C4 File Offset: 0x001FC5C4
	public float UserMaxCapacity
	{
		get
		{
			return this.targetFillMass;
		}
		set
		{
			this.targetFillMass = value;
			this.storage.capacityKg = this.targetFillMass;
			ConduitConsumer component = base.GetComponent<ConduitConsumer>();
			if (component != null)
			{
				component.capacityKG = this.targetFillMass;
			}
			ManualDeliveryKG component2 = base.GetComponent<ManualDeliveryKG>();
			if (component2 != null)
			{
				component2.capacity = (component2.refillMass = this.targetFillMass);
			}
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x060057A0 RID: 22432 RVA: 0x001FE436 File Offset: 0x001FC636
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x060057A1 RID: 22433 RVA: 0x001FE43D File Offset: 0x001FC63D
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x060057A2 RID: 22434 RVA: 0x001FE445 File Offset: 0x001FC645
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x060057A3 RID: 22435 RVA: 0x001FE452 File Offset: 0x001FC652
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x060057A4 RID: 22436 RVA: 0x001FE455 File Offset: 0x001FC655
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x060057A5 RID: 22437 RVA: 0x001FE45D File Offset: 0x001FC65D
	// (set) Token: 0x060057A6 RID: 22438 RVA: 0x001FE468 File Offset: 0x001FC668
	public Tag FuelType
	{
		get
		{
			return this.fuelType;
		}
		set
		{
			this.fuelType = value;
			if (this.storage.storageFilters == null)
			{
				this.storage.storageFilters = new List<Tag>();
			}
			this.storage.storageFilters.Add(this.fuelType);
			ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
			if (component != null)
			{
				component.RequestedItemTag = this.fuelType;
			}
		}
	}

	// Token: 0x060057A7 RID: 22439 RVA: 0x001FE4CB File Offset: 0x001FC6CB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FuelTank>(-905833192, FuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x060057A8 RID: 22440 RVA: 0x001FE4E4 File Offset: 0x001FC6E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetFillMass == -1f)
		{
			this.targetFillMass = this.physicalFuelCapacity;
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		}
		base.Subscribe<FuelTank>(-887025858, FuelTank.OnRocketLandedDelegate);
		this.UserMaxCapacity = this.UserMaxCapacity;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<FuelTank>(-1697596308, FuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x060057A9 RID: 22441 RVA: 0x001FE5D9 File Offset: 0x001FC7D9
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
	}

	// Token: 0x060057AA RID: 22442 RVA: 0x001FE5FD File Offset: 0x001FC7FD
	private void OnRocketLanded(object data)
	{
		if (this.ConsumeFuelOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
	}

	// Token: 0x060057AB RID: 22443 RVA: 0x001FE614 File Offset: 0x001FC814
	private void OnCopySettings(object data)
	{
		FuelTank component = ((GameObject)data).GetComponent<FuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x060057AC RID: 22444 RVA: 0x001FE644 File Offset: 0x001FC844
	public void DEBUG_FillTank()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			RocketEngine rocketEngine = null;
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				rocketEngine = gameObject.GetComponent<RocketEngine>();
				if (rocketEngine != null && rocketEngine.mainEngine)
				{
					break;
				}
			}
			if (rocketEngine != null)
			{
				Element element = ElementLoader.GetElement(rocketEngine.fuelTag);
				if (element.IsLiquid)
				{
					this.storage.AddLiquid(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsGas)
				{
					this.storage.AddGasChunk(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsSolid)
				{
					this.storage.AddOre(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
			}
			else
			{
				global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
			}
			return;
		}
		RocketEngineCluster rocketEngineCluster = null;
		foreach (GameObject gameObject2 in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			rocketEngineCluster = gameObject2.GetComponent<RocketEngineCluster>();
			if (rocketEngineCluster != null && rocketEngineCluster.mainEngine)
			{
				break;
			}
		}
		if (rocketEngineCluster != null)
		{
			Element element2 = ElementLoader.GetElement(rocketEngineCluster.fuelTag);
			if (element2.IsLiquid)
			{
				this.storage.AddLiquid(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsGas)
			{
				this.storage.AddGasChunk(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsSolid)
			{
				this.storage.AddOre(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			rocketEngineCluster.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().UpdateStatusItem();
			return;
		}
		global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
	}

	// Token: 0x04003AC7 RID: 15047
	public Storage storage;

	// Token: 0x04003AC8 RID: 15048
	private MeterController meter;

	// Token: 0x04003AC9 RID: 15049
	[Serialize]
	public float targetFillMass = -1f;

	// Token: 0x04003ACA RID: 15050
	[SerializeField]
	public float physicalFuelCapacity;

	// Token: 0x04003ACB RID: 15051
	public bool consumeFuelOnLand;

	// Token: 0x04003ACC RID: 15052
	[SerializeField]
	private Tag fuelType;

	// Token: 0x04003ACD RID: 15053
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003ACE RID: 15054
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04003ACF RID: 15055
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
