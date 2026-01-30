using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BE6 RID: 3046
public class StructureTemperatureComponents : KGameObjectSplitComponentManager<StructureTemperatureHeader, StructureTemperaturePayload>
{
	// Token: 0x06005B3C RID: 23356 RVA: 0x0021035C File Offset: 0x0020E55C
	public HandleVector<int>.Handle Add(GameObject go)
	{
		StructureTemperaturePayload structureTemperaturePayload = new StructureTemperaturePayload(go);
		return base.Add(go, new StructureTemperatureHeader
		{
			dirty = false,
			simHandle = -1,
			isActiveBuilding = false
		}, ref structureTemperaturePayload);
	}

	// Token: 0x06005B3D RID: 23357 RVA: 0x0021039B File Offset: 0x0020E59B
	public static void ClearInstanceMap()
	{
		StructureTemperatureComponents.handleInstanceMap.Clear();
	}

	// Token: 0x06005B3F RID: 23359 RVA: 0x002103B0 File Offset: 0x0020E5B0
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		this.InitializeStatusItem();
		base.OnPrefabInit(handle);
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(StructureTemperatureComponents.OnGetTemperature);
		structureTemperaturePayload.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(StructureTemperatureComponents.OnSetTemperature);
		new_data.isActiveBuilding = (structureTemperaturePayload.building.Def.SelfHeatKilowattsWhenActive != 0f || structureTemperaturePayload.ExhaustKilowatts != 0f);
		base.SetHeader(handle, new_data);
	}

	// Token: 0x06005B40 RID: 23360 RVA: 0x00210440 File Offset: 0x0020E640
	private void InitializeStatusItem()
	{
		if (this.operatingEnergyStatusItem != null)
		{
			return;
		}
		this.operatingEnergyStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.operatingEnergyStatusItem.resolveStringCallback = delegate(string str, object ev_data)
		{
			int key = (int)ev_data;
			HandleVector<int>.Handle handle = StructureTemperatureComponents.handleInstanceMap[key];
			StructureTemperaturePayload payload = base.GetPayload(handle);
			if (str != BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP)
			{
				try
				{
					return string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				}
				catch (Exception obj)
				{
					global::Debug.LogWarning(obj);
					global::Debug.LogWarning(BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP);
					global::Debug.LogWarning(str);
					return str;
				}
			}
			string text = "";
			foreach (StructureTemperaturePayload.EnergySource energySource in payload.energySourcesKW)
			{
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, energySource.source, GameUtil.GetFormattedHeatEnergy(energySource.value * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
			}
			str = string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S), text);
			return str;
		};
	}

	// Token: 0x06005B41 RID: 23361 RVA: 0x00210498 File Offset: 0x0020E698
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		if (structureTemperaturePayload.operational != null && structureTemperatureHeader.isActiveBuilding)
		{
			structureTemperaturePayload.primaryElement.Subscribe(824508782, delegate(object ev_data)
			{
				StructureTemperatureComponents.OnActiveChanged(handle);
			});
		}
		structureTemperaturePayload.maxTemperature = ((structureTemperaturePayload.overheatable != null) ? structureTemperaturePayload.overheatable.OverheatTemperature : 10000f);
		if (structureTemperaturePayload.maxTemperature <= 0f)
		{
			global::Debug.LogError("invalid max temperature");
		}
		base.SetPayload(handle, ref structureTemperaturePayload);
		this.SimRegister(handle, ref structureTemperatureHeader, ref structureTemperaturePayload);
	}

	// Token: 0x06005B42 RID: 23362 RVA: 0x00210554 File Offset: 0x0020E754
	private static void OnActiveChanged(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = structureTemperaturePayload.Temperature;
		new_data.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, new_data);
	}

	// Token: 0x06005B43 RID: 23363 RVA: 0x00210597 File Offset: 0x0020E797
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		this.SimUnregister(handle);
		base.OnCleanUp(handle);
	}

	// Token: 0x06005B44 RID: 23364 RVA: 0x002105A8 File Offset: 0x0020E7A8
	public override void Sim200ms(float dt)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<StructureTemperatureHeader> list;
		List<StructureTemperaturePayload> list2;
		base.GetDataLists(out list, out list2);
		StructureTemperaturePayload structureTemperaturePayload = default(StructureTemperaturePayload);
		for (int num4 = 0; num4 != list.Count; num4++)
		{
			StructureTemperatureHeader structureTemperatureHeader = list[num4];
			if (Sim.IsValidHandle(structureTemperatureHeader.simHandle))
			{
				bool flag = structureTemperatureHeader.dirty || structureTemperatureHeader.isActiveBuilding;
				if (flag)
				{
					structureTemperaturePayload = list2[num4];
				}
				if (structureTemperatureHeader.dirty)
				{
					StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
					if (structureTemperaturePayload.pendingEnergyModifications != 0f)
					{
						SimMessages.ModifyBuildingEnergy(structureTemperaturePayload.simHandleCopy, structureTemperaturePayload.pendingEnergyModifications, 0f, 10000f);
						structureTemperaturePayload.pendingEnergyModifications = 0f;
					}
					structureTemperatureHeader.dirty = false;
					list[num4] = structureTemperatureHeader;
				}
				if (structureTemperatureHeader.isActiveBuilding)
				{
					if (structureTemperaturePayload.operational == null || structureTemperaturePayload.operational.IsActive)
					{
						num++;
						if (!structureTemperaturePayload.isActiveStatusItemSet)
						{
							num3++;
							structureTemperaturePayload.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, this.operatingEnergyStatusItem, structureTemperaturePayload.simHandleCopy);
							structureTemperaturePayload.isActiveStatusItemSet = true;
						}
						structureTemperaturePayload.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload.energySourcesKW, structureTemperaturePayload.OperatingKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING);
						if (structureTemperaturePayload.ExhaustKilowatts != 0f)
						{
							num2++;
							StructureTemperatureComponents.ExhaustHeat(structureTemperaturePayload.GetExtents(), structureTemperaturePayload.ExhaustKilowatts, structureTemperaturePayload.maxTemperature, dt);
							structureTemperaturePayload.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload.energySourcesKW, structureTemperaturePayload.ExhaustKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING);
						}
					}
					else if (structureTemperaturePayload.isActiveStatusItemSet)
					{
						num3++;
						structureTemperaturePayload.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, null, null);
						structureTemperaturePayload.isActiveStatusItemSet = false;
					}
				}
				if (flag)
				{
					list2[num4] = structureTemperaturePayload;
				}
			}
		}
	}

	// Token: 0x06005B45 RID: 23365 RVA: 0x002107B4 File Offset: 0x0020E9B4
	public static void ExhaustHeat(Extents extents, float kw, float maxTemperature, float dt)
	{
		int num = extents.width * extents.height;
		float num2 = kw * dt / (float)num;
		for (int i = 0; i < extents.height; i++)
		{
			int num3 = extents.y + i;
			for (int j = 0; j < extents.width; j++)
			{
				int num4 = extents.x + j;
				int num5 = num3 * Grid.WidthInCells + num4;
				float num6 = Mathf.Min(Grid.Mass[num5], 1.5f) / 1.5f;
				float kilojoules = num2 * num6;
				SimMessages.ModifyEnergy(num5, kilojoules, maxTemperature, SimMessages.EnergySourceID.StructureTemperature);
			}
		}
	}

	// Token: 0x06005B46 RID: 23366 RVA: 0x00210850 File Offset: 0x0020EA50
	private static void UpdateSimState(ref StructureTemperaturePayload payload)
	{
		DebugUtil.Assert(Sim.IsValidHandle(payload.simHandleCopy));
		float internalTemperature = payload.primaryElement.InternalTemperature;
		BuildingDef def = payload.building.Def;
		float mass = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		float overheat_temperature = (payload.overheatable != null) ? payload.overheatable.OverheatTemperature : 10000f;
		if (!payload.enabled || payload.bypass)
		{
			mass = 0f;
		}
		Extents extents = payload.GetExtents();
		ushort idx = payload.primaryElement.Element.idx;
		if (payload.heatEffect != null)
		{
			float num = (payload.operational == null || payload.operational.IsActive) ? payload.ExhaustKilowatts : 0f;
			payload.heatEffect.SetHeatBeingProducedValue(Mathf.Clamp(operatingKilowatts + num, 0f, float.MaxValue));
		}
		SimMessages.ModifyBuildingHeatExchange(payload.simHandleCopy, extents, mass, internalTemperature, def.ThermalConductivity, overheat_temperature, operatingKilowatts, idx);
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x00210958 File Offset: 0x0020EB58
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
		float result;
		if (Sim.IsValidHandle(payload.simHandleCopy) && payload.enabled)
		{
			if (!payload.bypass)
			{
				int handleIndex = Sim.GetHandleIndex(payload.simHandleCopy);
				result = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
			}
			else
			{
				int i = Grid.PosToCell(payload.primaryElement.transform.GetPosition());
				result = Grid.Temperature[i];
			}
		}
		else
		{
			result = payload.primaryElement.InternalTemperature;
		}
		return result;
	}

	// Token: 0x06005B48 RID: 23368 RVA: 0x00210A04 File Offset: 0x0020EC04
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = temperature;
		structureTemperatureHeader.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, structureTemperatureHeader);
		if (!structureTemperatureHeader.isActiveBuilding && Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
		{
			StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
			if (structureTemperaturePayload.pendingEnergyModifications != 0f)
			{
				SimMessages.ModifyBuildingEnergy(structureTemperaturePayload.simHandleCopy, structureTemperaturePayload.pendingEnergyModifications, 0f, 10000f);
				structureTemperaturePayload.pendingEnergyModifications = 0f;
				GameComps.StructureTemperatures.SetPayload(handle, ref structureTemperaturePayload);
			}
		}
	}

	// Token: 0x06005B49 RID: 23369 RVA: 0x00210AB0 File Offset: 0x0020ECB0
	public void ProduceEnergy(HandleVector<int>.Handle handle, float delta_kilojoules, string source, float display_dt)
	{
		StructureTemperaturePayload payload = base.GetPayload(handle);
		if (Sim.IsValidHandle(payload.simHandleCopy))
		{
			SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, delta_kilojoules, 0f, 10000f);
		}
		else
		{
			payload.pendingEnergyModifications += delta_kilojoules;
			StructureTemperatureHeader header = base.GetHeader(handle);
			header.dirty = true;
			base.SetHeader(handle, header);
		}
		payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, delta_kilojoules / display_dt, source);
		base.SetPayload(handle, ref payload);
	}

	// Token: 0x06005B4A RID: 23370 RVA: 0x00210B30 File Offset: 0x0020ED30
	private List<StructureTemperaturePayload.EnergySource> AccumulateProducedEnergyKW(List<StructureTemperaturePayload.EnergySource> sources, float kw, string source)
	{
		if (sources == null)
		{
			sources = new List<StructureTemperaturePayload.EnergySource>();
		}
		bool flag = false;
		for (int i = 0; i < sources.Count; i++)
		{
			if (sources[i].source == source)
			{
				sources[i].Accumulate(kw);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			sources.Add(new StructureTemperaturePayload.EnergySource(kw, source));
		}
		return sources;
	}

	// Token: 0x06005B4B RID: 23371 RVA: 0x00210B90 File Offset: 0x0020ED90
	public static void DoStateTransition(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			StructureTemperatureComponents.DoMelt(GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement);
		}
	}

	// Token: 0x06005B4C RID: 23372 RVA: 0x00210BCC File Offset: 0x0020EDCC
	public static void DoMelt(PrimaryElement primary_element)
	{
		Element element = primary_element.Element;
		if (element.highTempTransitionTarget != SimHashes.Unobtanium)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(primary_element.transform.GetPosition()), element.highTempTransitionTarget, CellEventLogger.Instance.OreMelted, primary_element.Mass, primary_element.Element.highTemp, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
			Building.CreateBuildingMeltedNotification(primary_element.gameObject);
			Util.KDestroyGameObject(primary_element.gameObject);
		}
	}

	// Token: 0x06005B4D RID: 23373 RVA: 0x00210C48 File Offset: 0x0020EE48
	public static void DoOverheat(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(1832602615, null);
		}
	}

	// Token: 0x06005B4E RID: 23374 RVA: 0x00210C90 File Offset: 0x0020EE90
	public static void DoNoLongerOverheated(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(171119937, null);
		}
	}

	// Token: 0x06005B4F RID: 23375 RVA: 0x00210CD5 File Offset: 0x0020EED5
	public bool IsEnabled(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).enabled;
	}

	// Token: 0x06005B50 RID: 23376 RVA: 0x00210CE4 File Offset: 0x0020EEE4
	private void Enable(HandleVector<int>.Handle handle, bool isEnabled)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.enabled = isEnabled;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x06005B51 RID: 23377 RVA: 0x00210D16 File Offset: 0x0020EF16
	public void Enable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, true);
	}

	// Token: 0x06005B52 RID: 23378 RVA: 0x00210D20 File Offset: 0x0020EF20
	public void Disable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, false);
	}

	// Token: 0x06005B53 RID: 23379 RVA: 0x00210D2A File Offset: 0x0020EF2A
	public bool IsBypassed(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).bypass;
	}

	// Token: 0x06005B54 RID: 23380 RVA: 0x00210D38 File Offset: 0x0020EF38
	private void Bypass(HandleVector<int>.Handle handle, bool bypass)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.bypass = bypass;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x06005B55 RID: 23381 RVA: 0x00210D6A File Offset: 0x0020EF6A
	public void Bypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, true);
	}

	// Token: 0x06005B56 RID: 23382 RVA: 0x00210D74 File Offset: 0x0020EF74
	public void UnBypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, false);
	}

	// Token: 0x06005B57 RID: 23383 RVA: 0x00210D80 File Offset: 0x0020EF80
	protected void SimRegister(HandleVector<int>.Handle handle, ref StructureTemperatureHeader header, ref StructureTemperaturePayload payload)
	{
		if (payload.simHandleCopy != -1)
		{
			return;
		}
		PrimaryElement primaryElement = payload.primaryElement;
		if (primaryElement.Mass <= 0f)
		{
			return;
		}
		if (primaryElement.Element == null)
		{
			primaryElement.ElementID = ElementLoader.GetElementID(GameTags.IronOre);
		}
		if (primaryElement.Element.IsTemperatureInsulated)
		{
			return;
		}
		payload.simHandleCopy = -2;
		string dbg_name = primaryElement.name;
		HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle2 = Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			StructureTemperatureComponents.OnSimRegistered(handle, sim_handle, dbg_name);
		}, null, "StructureTemperature.SimRegister");
		BuildingDef def = primaryElement.GetComponent<Building>().Def;
		float internalTemperature = primaryElement.InternalTemperature;
		float massForTemperatureModification = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		Extents extents = payload.GetExtents();
		ushort idx = primaryElement.Element.idx;
		SimMessages.AddBuildingHeatExchange(extents, massForTemperatureModification, internalTemperature, def.ThermalConductivity, operatingKilowatts, idx, handle2.index);
		header.simHandle = payload.simHandleCopy;
		base.SetData(handle, header, ref payload);
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x00210E88 File Offset: 0x0020F088
	private static void OnSimRegistered(HandleVector<int>.Handle handle, int sim_handle, string dbg_name)
	{
		if (!GameComps.StructureTemperatures.IsValid(handle))
		{
			return;
		}
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy == -2)
		{
			StructureTemperatureComponents.handleInstanceMap[sim_handle] = handle;
			new_data.simHandle = sim_handle;
			structureTemperaturePayload.simHandleCopy = sim_handle;
			GameComps.StructureTemperatures.SetData(handle, new_data, ref structureTemperaturePayload);
			structureTemperaturePayload.primaryElement.BoxingTrigger<int>(-1555603773, sim_handle);
			int cell = Grid.PosToCell(structureTemperaturePayload.building.transform.GetPosition());
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Created, structureTemperaturePayload.building, sim_handle));
			return;
		}
		SimMessages.RemoveBuildingHeatExchange(sim_handle, -1);
	}

	// Token: 0x06005B59 RID: 23385 RVA: 0x00210F4C File Offset: 0x0020F14C
	protected unsafe void SimUnregister(HandleVector<int>.Handle handle)
	{
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			KCrashReporter.Assert(false, "Handle version mismatch in StructureTemperature.SimUnregister", null);
			return;
		}
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy != -1)
		{
			int cell = Grid.PosToCell(structureTemperaturePayload.building);
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Destroyed, structureTemperaturePayload.building, structureTemperaturePayload.simHandleCopy));
			if (Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
			{
				int handleIndex = Sim.GetHandleIndex(structureTemperaturePayload.simHandleCopy);
				structureTemperaturePayload.primaryElement.InternalTemperature = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
				SimMessages.RemoveBuildingHeatExchange(structureTemperaturePayload.simHandleCopy, -1);
				StructureTemperatureComponents.handleInstanceMap.Remove(structureTemperaturePayload.simHandleCopy);
			}
			structureTemperaturePayload.simHandleCopy = -1;
			new_data.simHandle = -1;
			base.SetData(handle, new_data, ref structureTemperaturePayload);
		}
	}

	// Token: 0x04003CD8 RID: 15576
	private const float MAX_PRESSURE = 1.5f;

	// Token: 0x04003CD9 RID: 15577
	private static Dictionary<int, HandleVector<int>.Handle> handleInstanceMap = new Dictionary<int, HandleVector<int>.Handle>();

	// Token: 0x04003CDA RID: 15578
	private StatusItem operatingEnergyStatusItem;
}
