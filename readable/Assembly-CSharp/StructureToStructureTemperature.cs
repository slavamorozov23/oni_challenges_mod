using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BE7 RID: 3047
public class StructureToStructureTemperature : KMonoBehaviour
{
	// Token: 0x06005B5C RID: 23388 RVA: 0x00211168 File Offset: 0x0020F368
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<StructureToStructureTemperature>(-1555603773, StructureToStructureTemperature.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x06005B5D RID: 23389 RVA: 0x00211181 File Offset: 0x0020F381
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DefineConductiveCells();
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
	}

	// Token: 0x06005B5E RID: 23390 RVA: 0x002111AF File Offset: 0x0020F3AF
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
		this.UnregisterToSIM();
		base.OnCleanUp();
	}

	// Token: 0x06005B5F RID: 23391 RVA: 0x002111E0 File Offset: 0x0020F3E0
	private void OnStructureTemperatureRegistered(object _sim_handle)
	{
		int value = ((Boxed<int>)_sim_handle).value;
		this.RegisterToSIM(value);
	}

	// Token: 0x06005B60 RID: 23392 RVA: 0x00211200 File Offset: 0x0020F400
	private void RegisterToSIM(int sim_handle)
	{
		string name = this.building.Def.Name;
		SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle2, Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			this.OnSimRegistered(sim_handle);
		}, null, "StructureToStructureTemperature.SimRegister").index);
	}

	// Token: 0x06005B61 RID: 23393 RVA: 0x0021124D File Offset: 0x0020F44D
	private void OnSimRegistered(int sim_handle)
	{
		if (sim_handle != -1)
		{
			this.selfHandle = sim_handle;
			this.hasBeenRegister = true;
			if (this.buildingDestroyed)
			{
				this.UnregisterToSIM();
				return;
			}
			this.Refresh_InContactBuildings();
		}
	}

	// Token: 0x06005B62 RID: 23394 RVA: 0x00211276 File Offset: 0x0020F476
	private void UnregisterToSIM()
	{
		if (this.hasBeenRegister)
		{
			SimMessages.RemoveBuildingToBuildingHeatExchange(this.selfHandle, -1);
		}
		this.buildingDestroyed = true;
	}

	// Token: 0x06005B63 RID: 23395 RVA: 0x00211294 File Offset: 0x0020F494
	private void DefineConductiveCells()
	{
		this.conductiveCells = new List<int>(this.building.PlacementCells);
		this.conductiveCells.Remove(this.building.GetUtilityInputCell());
		this.conductiveCells.Remove(this.building.GetUtilityOutputCell());
	}

	// Token: 0x06005B64 RID: 23396 RVA: 0x002112E5 File Offset: 0x0020F4E5
	private void Add(StructureToStructureTemperature.InContactBuildingData buildingData)
	{
		if (this.inContactBuildings.Add(buildingData.buildingInContact))
		{
			SimMessages.AddBuildingToBuildingHeatExchange(this.selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
		}
	}

	// Token: 0x06005B65 RID: 23397 RVA: 0x00211311 File Offset: 0x0020F511
	private void Remove(int building)
	{
		if (this.inContactBuildings.Contains(building))
		{
			this.inContactBuildings.Remove(building);
			SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
		}
	}

	// Token: 0x06005B66 RID: 23398 RVA: 0x0021133C File Offset: 0x0020F53C
	private void OnAnyBuildingChanged(int _cell, object _data)
	{
		if (this.hasBeenRegister)
		{
			StructureToStructureTemperature.BuildingChangedObj buildingChangedObj = (StructureToStructureTemperature.BuildingChangedObj)_data;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < buildingChangedObj.building.PlacementCells.Length; i++)
			{
				int item = buildingChangedObj.building.PlacementCells[i];
				if (this.conductiveCells.Contains(item))
				{
					flag = true;
					num++;
				}
			}
			if (flag)
			{
				int simHandler = buildingChangedObj.simHandler;
				StructureToStructureTemperature.BuildingChangeType changeType = buildingChangedObj.changeType;
				if (changeType == StructureToStructureTemperature.BuildingChangeType.Created)
				{
					StructureToStructureTemperature.InContactBuildingData buildingData = new StructureToStructureTemperature.InContactBuildingData
					{
						buildingInContact = simHandler,
						cellsInContact = num
					};
					this.Add(buildingData);
					return;
				}
				if (changeType != StructureToStructureTemperature.BuildingChangeType.Destroyed)
				{
					return;
				}
				this.Remove(simHandler);
			}
		}
	}

	// Token: 0x06005B67 RID: 23399 RVA: 0x002113E8 File Offset: 0x0020F5E8
	private void Refresh_InContactBuildings()
	{
		foreach (StructureToStructureTemperature.InContactBuildingData buildingData in this.GetAll_InContact_Buildings())
		{
			this.Add(buildingData);
		}
	}

	// Token: 0x06005B68 RID: 23400 RVA: 0x0021143C File Offset: 0x0020F63C
	private List<StructureToStructureTemperature.InContactBuildingData> GetAll_InContact_Buildings()
	{
		Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
		List<StructureToStructureTemperature.InContactBuildingData> list = new List<StructureToStructureTemperature.InContactBuildingData>();
		List<GameObject> buildingsInCell = new List<GameObject>();
		using (List<int>.Enumerator enumerator = this.conductiveCells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int cell = enumerator.Current;
				buildingsInCell.Clear();
				Action<int> action = delegate(int layer)
				{
					GameObject gameObject = Grid.Objects[cell, layer];
					if (gameObject != null && !buildingsInCell.Contains(gameObject))
					{
						buildingsInCell.Add(gameObject);
					}
				};
				action(1);
				action(26);
				action(27);
				action(31);
				action(32);
				action(30);
				action(12);
				action(13);
				action(16);
				action(17);
				action(24);
				action(2);
				for (int i = 0; i < buildingsInCell.Count; i++)
				{
					Building building = (buildingsInCell[i] == null) ? null : buildingsInCell[i].GetComponent<Building>();
					if (building != null && building.Def.UseStructureTemperature && building.PlacementCellsContainCell(cell))
					{
						if (!dictionary.ContainsKey(building))
						{
							dictionary.Add(building, 0);
						}
						Dictionary<Building, int> dictionary2 = dictionary;
						Building key = building;
						int num = dictionary2[key];
						dictionary2[key] = num + 1;
					}
				}
			}
		}
		foreach (Building building2 in dictionary.Keys)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(building2);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
				StructureToStructureTemperature.InContactBuildingData item = new StructureToStructureTemperature.InContactBuildingData
				{
					buildingInContact = simHandleCopy,
					cellsInContact = dictionary[building2]
				};
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x04003CDB RID: 15579
	[MyCmpGet]
	private Building building;

	// Token: 0x04003CDC RID: 15580
	private List<int> conductiveCells;

	// Token: 0x04003CDD RID: 15581
	private HashSet<int> inContactBuildings = new HashSet<int>();

	// Token: 0x04003CDE RID: 15582
	private bool hasBeenRegister;

	// Token: 0x04003CDF RID: 15583
	private bool buildingDestroyed;

	// Token: 0x04003CE0 RID: 15584
	private int selfHandle;

	// Token: 0x04003CE1 RID: 15585
	protected static readonly EventSystem.IntraObjectHandler<StructureToStructureTemperature> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<StructureToStructureTemperature>(delegate(StructureToStructureTemperature component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x02001D7B RID: 7547
	public enum BuildingChangeType
	{
		// Token: 0x04008B73 RID: 35699
		Created,
		// Token: 0x04008B74 RID: 35700
		Destroyed,
		// Token: 0x04008B75 RID: 35701
		Moved
	}

	// Token: 0x02001D7C RID: 7548
	public struct InContactBuildingData
	{
		// Token: 0x04008B76 RID: 35702
		public int buildingInContact;

		// Token: 0x04008B77 RID: 35703
		public int cellsInContact;
	}

	// Token: 0x02001D7D RID: 7549
	public struct BuildingChangedObj
	{
		// Token: 0x0600B13F RID: 45375 RVA: 0x003DCB7B File Offset: 0x003DAD7B
		public BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType _changeType, Building _building, int sim_handler)
		{
			this.changeType = _changeType;
			this.building = _building;
			this.simHandler = sim_handler;
		}

		// Token: 0x04008B78 RID: 35704
		public StructureToStructureTemperature.BuildingChangeType changeType;

		// Token: 0x04008B79 RID: 35705
		public int simHandler;

		// Token: 0x04008B7A RID: 35706
		public Building building;
	}
}
