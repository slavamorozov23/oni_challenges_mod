using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000914 RID: 2324
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConsumer : SimComponent, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x1400001A RID: 26
	// (add) Token: 0x060040A5 RID: 16549 RVA: 0x0016DE04 File Offset: 0x0016C004
	// (remove) Token: 0x060040A6 RID: 16550 RVA: 0x0016DE3C File Offset: 0x0016C03C
	public event Action<Sim.ConsumedMassInfo> OnElementConsumed;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x060040A7 RID: 16551 RVA: 0x0016DE74 File Offset: 0x0016C074
	// (remove) Token: 0x060040A8 RID: 16552 RVA: 0x0016DEAC File Offset: 0x0016C0AC
	public event Action<Sim.ConsumedMassInfo> OnElementConsumedStored;

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0016DEE1 File Offset: 0x0016C0E1
	public float AverageConsumeRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x060040AA RID: 16554 RVA: 0x0016DEF8 File Offset: 0x0016C0F8
	public static void ClearInstanceMap()
	{
		ElementConsumer.handleInstanceMap.Clear();
	}

	// Token: 0x060040AB RID: 16555 RVA: 0x0016DF04 File Offset: 0x0016C104
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		if (this.elementToConsume == SimHashes.Void)
		{
			throw new ArgumentException("No consumable elements specified");
		}
		if (!this.ignoreActiveChanged)
		{
			base.Subscribe<ElementConsumer>(824508782, ElementConsumer.OnActiveChangedDelegate);
		}
		if (this.capacityKG != float.PositiveInfinity)
		{
			this.hasAvailableCapacity = !this.IsStorageFull();
			base.Subscribe<ElementConsumer>(-1697596308, ElementConsumer.OnStorageChangeDelegate);
		}
	}

	// Token: 0x060040AC RID: 16556 RVA: 0x0016DF90 File Offset: 0x0016C190
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x060040AD RID: 16557 RVA: 0x0016DFAE File Offset: 0x0016C1AE
	protected virtual bool IsActive()
	{
		return this.operational == null || this.operational.IsActive;
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x0016DFCC File Offset: 0x0016C1CC
	public void EnableConsumption(bool enabled)
	{
		bool flag = this.consumptionEnabled;
		this.consumptionEnabled = enabled;
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		if (enabled != flag)
		{
			this.UpdateSimData();
		}
	}

	// Token: 0x060040AF RID: 16559 RVA: 0x0016E000 File Offset: 0x0016C200
	private bool IsStorageFull()
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.elementToConsume);
		return primaryElement != null && primaryElement.Mass >= this.capacityKG;
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x0016E03B File Offset: 0x0016C23B
	public void RefreshConsumptionRate()
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimData();
	}

	// Token: 0x060040B1 RID: 16561 RVA: 0x0016E054 File Offset: 0x0016C254
	private void UpdateSimData()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		int sampleCell = this.GetSampleCell();
		float num = (this.consumptionEnabled && this.hasAvailableCapacity) ? this.consumptionRate : 0f;
		SimMessages.SetElementConsumerData(this.simHandle, sampleCell, num);
		this.UpdateStatusItem();
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x0016E0AC File Offset: 0x0016C2AC
	public static void AddMass(Sim.ConsumedMassInfo consumed_info)
	{
		if (!Sim.IsValidHandle(consumed_info.simHandle))
		{
			return;
		}
		ElementConsumer elementConsumer;
		if (ElementConsumer.handleInstanceMap.TryGetValue(consumed_info.simHandle, out elementConsumer))
		{
			elementConsumer.AddMassInternal(consumed_info);
		}
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x0016E0E2 File Offset: 0x0016C2E2
	private int GetSampleCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.sampleCellOffset);
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x0016E100 File Offset: 0x0016C300
	private void AddMassInternal(Sim.ConsumedMassInfo consumed_info)
	{
		if (consumed_info.mass > 0f)
		{
			if (this.storeOnConsume)
			{
				Element element = ElementLoader.elements[(int)consumed_info.removedElemIdx];
				if (this.elementToConsume == SimHashes.Vacuum || this.elementToConsume == element.id)
				{
					if (element.IsLiquid)
					{
						this.storage.AddLiquid(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
					else if (element.IsGas)
					{
						this.storage.AddGasChunk(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
					if (this.OnElementConsumedStored != null)
					{
						this.OnElementConsumedStored(consumed_info);
					}
				}
			}
			else
			{
				this.consumedTemperature = GameUtil.GetFinalTemperature(consumed_info.temperature, consumed_info.mass, this.consumedTemperature, this.consumedMass);
				this.consumedMass += consumed_info.mass;
				if (this.OnElementConsumed != null)
				{
					this.OnElementConsumed(consumed_info);
				}
			}
		}
		Game.Instance.accumulators.Accumulate(this.accumulator, consumed_info.mass);
	}

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x060040B5 RID: 16565 RVA: 0x0016E23C File Offset: 0x0016C43C
	public bool IsElementAvailable
	{
		get
		{
			int sampleCell = this.GetSampleCell();
			SimHashes id = Grid.Element[sampleCell].id;
			return this.elementToConsume == id && Grid.Mass[sampleCell] >= this.minimumMass;
		}
	}

	// Token: 0x060040B6 RID: 16566 RVA: 0x0016E280 File Offset: 0x0016C480
	private void UpdateStatusItem()
	{
		if (this.showInStatusPanel)
		{
			if (this.statusHandle == Guid.Empty && this.IsActive() && this.consumptionEnabled)
			{
				this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ElementConsumer, this);
				return;
			}
			if (this.statusHandle != Guid.Empty && !this.consumptionEnabled)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
				return;
			}
		}
		else if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
			this.statusHandle = Guid.Empty;
		}
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x0016E344 File Offset: 0x0016C544
	private void OnStorageChange(object data)
	{
		bool flag = !this.IsStorageFull();
		if (flag != this.hasAvailableCapacity)
		{
			this.hasAvailableCapacity = flag;
			this.RefreshConsumptionRate();
		}
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x0016E371 File Offset: 0x0016C571
	protected override void OnCmpEnable()
	{
		if (!base.isSpawned)
		{
			return;
		}
		if (!this.IsActive())
		{
			return;
		}
		this.UpdateStatusItem();
	}

	// Token: 0x060040B9 RID: 16569 RVA: 0x0016E38B File Offset: 0x0016C58B
	protected override void OnCmpDisable()
	{
		this.UpdateStatusItem();
	}

	// Token: 0x060040BA RID: 16570 RVA: 0x0016E394 File Offset: 0x0016C594
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.isRequired && this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESELEMENT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESELEMENT, arg), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060040BB RID: 16571 RVA: 0x0016E44C File Offset: 0x0016C64C
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060040BC RID: 16572 RVA: 0x0016E538 File Offset: 0x0016C738
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060040BD RID: 16573 RVA: 0x0016E5D4 File Offset: 0x0016C7D4
	private void OnActiveChanged(object data)
	{
		bool isActive = this.operational.IsActive;
		this.EnableConsumption(isActive);
	}

	// Token: 0x060040BE RID: 16574 RVA: 0x0016E5F4 File Offset: 0x0016C7F4
	protected override void OnSimUnregister()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		ElementConsumer.handleInstanceMap.Remove(this.simHandle);
		ElementConsumer.StaticUnregister(this.simHandle);
	}

	// Token: 0x060040BF RID: 16575 RVA: 0x0016E622 File Offset: 0x0016C822
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		SimMessages.AddElementConsumer(this.GetSampleCell(), this.configuration, this.elementToConsume, this.consumptionRadius, cb_handle.index);
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x0016E648 File Offset: 0x0016C848
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementConsumer.StaticUnregister);
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x0016E656 File Offset: 0x0016C856
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementConsumer(-1, sim_handle);
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x0016E66A File Offset: 0x0016C86A
	protected override void OnSimRegistered()
	{
		if (this.consumptionEnabled)
		{
			this.UpdateSimData();
		}
		ElementConsumer.handleInstanceMap[this.simHandle] = this;
	}

	// Token: 0x04002866 RID: 10342
	[HashedEnum]
	[SerializeField]
	public SimHashes elementToConsume = SimHashes.Vacuum;

	// Token: 0x04002867 RID: 10343
	[SerializeField]
	public float consumptionRate;

	// Token: 0x04002868 RID: 10344
	[SerializeField]
	public byte consumptionRadius = 1;

	// Token: 0x04002869 RID: 10345
	[SerializeField]
	public float minimumMass;

	// Token: 0x0400286A RID: 10346
	[SerializeField]
	public bool showInStatusPanel = true;

	// Token: 0x0400286B RID: 10347
	[SerializeField]
	public Vector3 sampleCellOffset;

	// Token: 0x0400286C RID: 10348
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x0400286D RID: 10349
	[SerializeField]
	public ElementConsumer.Configuration configuration;

	// Token: 0x0400286E RID: 10350
	[Serialize]
	[NonSerialized]
	public float consumedMass;

	// Token: 0x0400286F RID: 10351
	[Serialize]
	[NonSerialized]
	public float consumedTemperature;

	// Token: 0x04002870 RID: 10352
	[SerializeField]
	public bool storeOnConsume;

	// Token: 0x04002871 RID: 10353
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04002872 RID: 10354
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002873 RID: 10355
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04002876 RID: 10358
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002877 RID: 10359
	public bool ignoreActiveChanged;

	// Token: 0x04002878 RID: 10360
	private Guid statusHandle;

	// Token: 0x04002879 RID: 10361
	public bool showDescriptor = true;

	// Token: 0x0400287A RID: 10362
	public bool isRequired = true;

	// Token: 0x0400287B RID: 10363
	private bool consumptionEnabled;

	// Token: 0x0400287C RID: 10364
	private bool hasAvailableCapacity = true;

	// Token: 0x0400287D RID: 10365
	private static Dictionary<int, ElementConsumer> handleInstanceMap = new Dictionary<int, ElementConsumer>();

	// Token: 0x0400287E RID: 10366
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x0400287F RID: 10367
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001913 RID: 6419
	public enum Configuration
	{
		// Token: 0x04007CD5 RID: 31957
		Element,
		// Token: 0x04007CD6 RID: 31958
		AllLiquid,
		// Token: 0x04007CD7 RID: 31959
		AllGas
	}
}
