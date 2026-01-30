using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B97 RID: 2967
[AddComponentMenu("KMonoBehaviour/scripts/OxidizerTank")]
public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06005891 RID: 22673 RVA: 0x00202366 File Offset: 0x00200566
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06005892 RID: 22674 RVA: 0x0020236E File Offset: 0x0020056E
	// (set) Token: 0x06005893 RID: 22675 RVA: 0x00202378 File Offset: 0x00200578
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
			base.Trigger(-945020481, this);
			this.OnStorageCapacityChanged(this.targetFillMass);
			if (this.filteredStorage != null)
			{
				this.filteredStorage.FilterChanged();
			}
		}
	}

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06005894 RID: 22676 RVA: 0x002023E4 File Offset: 0x002005E4
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06005895 RID: 22677 RVA: 0x002023EB File Offset: 0x002005EB
	public float MaxCapacity
	{
		get
		{
			return this.maxFillMass;
		}
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06005896 RID: 22678 RVA: 0x002023F3 File Offset: 0x002005F3
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06005897 RID: 22679 RVA: 0x00202400 File Offset: 0x00200600
	public float TotalOxidizerPower
	{
		get
		{
			float num = 0f;
			foreach (GameObject gameObject in this.storage.items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2;
				if (DlcManager.FeatureClusterSpaceEnabled())
				{
					num2 = Clustercraft.dlc1OxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				else
				{
					num2 = RocketStats.oxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				num += component.Mass * num2;
			}
			return num;
		}
	}

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06005898 RID: 22680 RVA: 0x002024A4 File Offset: 0x002006A4
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06005899 RID: 22681 RVA: 0x002024A7 File Offset: 0x002006A7
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x0600589A RID: 22682 RVA: 0x002024B0 File Offset: 0x002006B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxidizerTank>(-905833192, OxidizerTank.OnCopySettingsDelegate);
		if (this.supportsMultipleOxidizers)
		{
			this.filteredStorage = new FilteredStorage(this, null, this, false, Db.Get().ChoreTypes.Fetch);
			this.filteredStorage.FilterChanged();
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			return;
		}
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
		component.matchParentOffset = true;
		component.forceAlwaysAlive = true;
	}

	// Token: 0x0600589B RID: 22683 RVA: 0x00202580 File Offset: 0x00200780
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.discoverResourcesOnSpawn != null)
		{
			foreach (SimHashes hash in this.discoverResourcesOnSpawn)
			{
				Element element = ElementLoader.FindElementByHash(hash);
				DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
			}
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			global::Debug.Assert(DlcManager.IsExpansion1Active(), "EXP1 not active but trying to use EXP1 rockety system");
			component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionSufficientOxidizer(this));
		}
		this.UserMaxCapacity = Mathf.Min(this.UserMaxCapacity, this.maxFillMass);
		base.Subscribe<OxidizerTank>(-887025858, OxidizerTank.OnRocketLandedDelegate);
		base.Subscribe<OxidizerTank>(-1697596308, OxidizerTank.OnStorageChangeDelegate);
	}

	// Token: 0x0600589C RID: 22684 RVA: 0x0020267C File Offset: 0x0020087C
	public float GetTotalOxidizerAvailable()
	{
		float num = 0f;
		foreach (Tag tag in this.oxidizerTypes)
		{
			num += this.storage.GetAmountAvailable(tag);
		}
		return num;
	}

	// Token: 0x0600589D RID: 22685 RVA: 0x002026BC File Offset: 0x002008BC
	public Dictionary<Tag, float> GetOxidizersAvailable()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (Tag tag in this.oxidizerTypes)
		{
			dictionary[tag] = this.storage.GetAmountAvailable(tag);
		}
		return dictionary;
	}

	// Token: 0x0600589E RID: 22686 RVA: 0x00202700 File Offset: 0x00200900
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x0600589F RID: 22687 RVA: 0x00202708 File Offset: 0x00200908
	private void OnStorageCapacityChanged(float newCapacity)
	{
		this.RefreshMeter();
	}

	// Token: 0x060058A0 RID: 22688 RVA: 0x00202710 File Offset: 0x00200910
	private void RefreshMeter()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
		}
	}

	// Token: 0x060058A1 RID: 22689 RVA: 0x0020274F File Offset: 0x0020094F
	private void OnRocketLanded(object data)
	{
		if (this.consumeOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x060058A2 RID: 22690 RVA: 0x00202778 File Offset: 0x00200978
	private void OnCopySettings(object data)
	{
		OxidizerTank component = ((GameObject)data).GetComponent<OxidizerTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x060058A3 RID: 22691 RVA: 0x002027A8 File Offset: 0x002009A8
	[ContextMenu("Fill Tank")]
	public void DEBUG_FillTank(SimHashes element)
	{
		base.GetComponent<FlatTagFilterable>().selectedTags.Add(element.CreateTag());
		if (ElementLoader.FindElementByHash(element).IsLiquid)
		{
			this.storage.AddLiquid(element, this.targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, 0, 0, false, true);
			return;
		}
		if (ElementLoader.FindElementByHash(element).IsSolid)
		{
			GameObject go = ElementLoader.FindElementByHash(element).substance.SpawnResource(base.gameObject.transform.GetPosition(), this.targetFillMass, 300f, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, false, false, true, false);
		}
	}

	// Token: 0x060058A4 RID: 22692 RVA: 0x00202854 File Offset: 0x00200A54
	public OxidizerTank()
	{
		Tag[] array2;
		if (!DlcManager.IsExpansion1Active())
		{
			Tag[] array = new Tag[2];
			array[0] = SimHashes.OxyRock.CreateTag();
			array2 = array;
			array[1] = SimHashes.LiquidOxygen.CreateTag();
		}
		else
		{
			Tag[] array3 = new Tag[3];
			array3[0] = SimHashes.OxyRock.CreateTag();
			array3[1] = SimHashes.LiquidOxygen.CreateTag();
			array2 = array3;
			array3[2] = SimHashes.Fertilizer.CreateTag();
		}
		this.oxidizerTypes = array2;
		base..ctor();
	}

	// Token: 0x04003B70 RID: 15216
	public Storage storage;

	// Token: 0x04003B71 RID: 15217
	public bool supportsMultipleOxidizers;

	// Token: 0x04003B72 RID: 15218
	private MeterController meter;

	// Token: 0x04003B73 RID: 15219
	private bool isSuspended;

	// Token: 0x04003B74 RID: 15220
	public bool consumeOnLand = true;

	// Token: 0x04003B75 RID: 15221
	[Serialize]
	public float maxFillMass;

	// Token: 0x04003B76 RID: 15222
	[Serialize]
	public float targetFillMass;

	// Token: 0x04003B77 RID: 15223
	public List<SimHashes> discoverResourcesOnSpawn;

	// Token: 0x04003B78 RID: 15224
	[SerializeField]
	private Tag[] oxidizerTypes;

	// Token: 0x04003B79 RID: 15225
	private FilteredStorage filteredStorage;

	// Token: 0x04003B7A RID: 15226
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003B7B RID: 15227
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04003B7C RID: 15228
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
