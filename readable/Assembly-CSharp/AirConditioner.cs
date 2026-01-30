using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000704 RID: 1796
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AirConditioner")]
public class AirConditioner : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor, ISim200ms
{
	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06002C7D RID: 11389 RVA: 0x00102D8C File Offset: 0x00100F8C
	// (set) Token: 0x06002C7E RID: 11390 RVA: 0x00102D94 File Offset: 0x00100F94
	public float lastEnvTemp { get; private set; }

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06002C7F RID: 11391 RVA: 0x00102D9D File Offset: 0x00100F9D
	// (set) Token: 0x06002C80 RID: 11392 RVA: 0x00102DA5 File Offset: 0x00100FA5
	public float lastGasTemp { get; private set; }

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06002C81 RID: 11393 RVA: 0x00102DAE File Offset: 0x00100FAE
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x00102DB6 File Offset: 0x00100FB6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AirConditioner>(-592767678, AirConditioner.OnOperationalChangedDelegate);
		base.Subscribe<AirConditioner>(824508782, AirConditioner.OnActiveChangedDelegate);
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x00102DE0 File Offset: 0x00100FE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.gameObject.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
		this.cooledAirOutputCell = this.building.GetUtilityOutputCell();
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x00102E6E File Offset: 0x0010106E
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.UpdateState(dt);
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x00102EA0 File Offset: 0x001010A0
	private static bool UpdateStateCb(int cell, object data)
	{
		AirConditioner airConditioner = data as AirConditioner;
		airConditioner.cellCount++;
		airConditioner.envTemp += Grid.Temperature[cell];
		return true;
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x00102ED0 File Offset: 0x001010D0
	private void UpdateState(float dt)
	{
		bool value = this.consumer.IsSatisfied;
		this.envTemp = 0f;
		this.cellCount = 0;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, AirConditioner.UpdateStateCbDelegate);
			this.envTemp /= (float)this.cellCount;
		}
		this.lastEnvTemp = this.envTemp;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < items.Count; i++)
		{
			PrimaryElement component = items[i].GetComponent<PrimaryElement>();
			if (component.Mass > 0f && (!this.isLiquidConditioner || !component.Element.IsGas) && (this.isLiquidConditioner || !component.Element.IsLiquid))
			{
				value = true;
				this.lastGasTemp = component.Temperature;
				float num = component.Temperature + this.temperatureDelta;
				if (num < 1f)
				{
					num = 1f;
					this.lowTempLag = Mathf.Min(this.lowTempLag + dt / 5f, 1f);
				}
				else
				{
					this.lowTempLag = Mathf.Min(this.lowTempLag - dt / 5f, 0f);
				}
				float num2 = (this.isLiquidConditioner ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.cooledAirOutputCell, component.ElementID, component.Mass, num, component.DiseaseIdx, component.DiseaseCount);
				component.KeepZeroMassObject = true;
				float num3 = num2 / component.Mass;
				int num4 = (int)((float)component.DiseaseCount * num3);
				component.Mass -= num2;
				component.ModifyDiseaseCount(-num4, "AirConditioner.UpdateState");
				float num5 = (num - component.Temperature) * component.Element.specificHeatCapacity * num2;
				float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
				this.lastSampleTime = Time.time;
				if (this.isLiquidConditioner && this.lastElement != component.ElementID)
				{
					Color color = component.Element.substance.colour;
					color.a = 1f;
					this.controller.SetSymbolTint(new KAnimHashedString("liquid"), color);
				}
				this.heatEffect.SetHeatBeingProducedValue(Mathf.Abs(num5));
				this.lastElement = component.ElementID;
				GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, -num5, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, display_dt);
				break;
			}
		}
		if (Time.time - this.lastSampleTime > 2f)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 0f, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time - this.lastSampleTime);
			this.lastSampleTime = Time.time;
		}
		this.operational.SetActive(value, false);
		this.UpdateStatus();
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x001031E2 File Offset: 0x001013E2
	private void OnOperationalChanged(object _)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateState(0f);
		}
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x001031FC File Offset: 0x001013FC
	private void OnActiveChanged(object _)
	{
		this.UpdateStatus();
		if (this.operational.IsActive)
		{
			this.heatEffect.enabled = true;
			return;
		}
		this.heatEffect.enabled = false;
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x0010322C File Offset: 0x0010142C
	private void UpdateStatus()
	{
		if (this.operational.IsActive)
		{
			if (this.lowTempLag >= 1f && !this.showingLowTemp)
			{
				this.statusHandle = (this.isLiquidConditioner ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdLiquid, this) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdGas, this));
				this.showingLowTemp = true;
				this.showingHotEnv = false;
				return;
			}
			if (this.lowTempLag <= 0f && (this.showingHotEnv || this.showingLowTemp))
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
			if (this.statusHandle == Guid.Empty)
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
		}
		else
		{
			this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x001033A0 File Offset: 0x001015A0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(this.temperatureDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
		Element element = ElementLoader.FindElementByName(this.isLiquidConditioner ? "Water" : "Oxygen");
		float num;
		if (this.isLiquidConditioner)
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 10000f);
		}
		else
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 1000f);
		}
		float dtu = num * 1f;
		Descriptor item = default(Descriptor);
		string txt = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		string tooltip = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Descriptor item2 = default(Descriptor);
		item2.SetupDescriptor(string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.GASCOOLING, formattedTemperature), string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.TOOLTIPS.GASCOOLING, formattedTemperature), Descriptor.DescriptorType.Effect);
		list.Add(item2);
		return list;
	}

	// Token: 0x04001A5E RID: 6750
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001A5F RID: 6751
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04001A60 RID: 6752
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04001A61 RID: 6753
	[MyCmpReq]
	private ConduitConsumer consumer;

	// Token: 0x04001A62 RID: 6754
	[MyCmpReq]
	private BuildingComplete building;

	// Token: 0x04001A63 RID: 6755
	[MyCmpGet]
	private OccupyArea occupyArea;

	// Token: 0x04001A64 RID: 6756
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04001A65 RID: 6757
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04001A66 RID: 6758
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001A67 RID: 6759
	public float temperatureDelta = -14f;

	// Token: 0x04001A68 RID: 6760
	public float maxEnvironmentDelta = -50f;

	// Token: 0x04001A69 RID: 6761
	private float lowTempLag;

	// Token: 0x04001A6A RID: 6762
	private bool showingLowTemp;

	// Token: 0x04001A6B RID: 6763
	public bool isLiquidConditioner;

	// Token: 0x04001A6C RID: 6764
	private bool showingHotEnv;

	// Token: 0x04001A6F RID: 6767
	private Guid statusHandle;

	// Token: 0x04001A70 RID: 6768
	[Serialize]
	private float targetTemperature;

	// Token: 0x04001A71 RID: 6769
	private int cooledAirOutputCell = -1;

	// Token: 0x04001A72 RID: 6770
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001A73 RID: 6771
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x04001A74 RID: 6772
	private float lastSampleTime = -1f;

	// Token: 0x04001A75 RID: 6773
	private SimHashes lastElement = SimHashes.Vacuum;

	// Token: 0x04001A76 RID: 6774
	private float envTemp;

	// Token: 0x04001A77 RID: 6775
	private int cellCount;

	// Token: 0x04001A78 RID: 6776
	private static readonly Func<int, object, bool> UpdateStateCbDelegate = (int cell, object data) => AirConditioner.UpdateStateCb(cell, data);
}
