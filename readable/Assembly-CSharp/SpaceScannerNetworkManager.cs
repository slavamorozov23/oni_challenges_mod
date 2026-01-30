using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000BD7 RID: 3031
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerNetworkManager : ISim1000ms
{
	// Token: 0x06005ACE RID: 23246 RVA: 0x0020E0C2 File Offset: 0x0020C2C2
	public Dictionary<int, SpaceScannerWorldData> DEBUG_GetWorldIdToDataMap()
	{
		return this.worldIdToDataMap;
	}

	// Token: 0x06005ACF RID: 23247 RVA: 0x0020E0CC File Offset: 0x0020C2CC
	public bool IsTargetDetectedOnWorld(int worldId, SpaceScannerTarget target)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		return this.worldIdToDataMap.TryGetValue(worldId, out spaceScannerWorldData) && spaceScannerWorldData.targetIdsDetected.Contains(target.id);
	}

	// Token: 0x06005AD0 RID: 23248 RVA: 0x0020E0FC File Offset: 0x0020C2FC
	public MathUtil.MinMax GetDetectTimeRangeForWorld(int worldId)
	{
		return SpaceScannerNetworkManager.GetDetectTimeRange(this.GetQualityForWorld(worldId));
	}

	// Token: 0x06005AD1 RID: 23249 RVA: 0x0020E10C File Offset: 0x0020C30C
	public float GetQualityForWorld(int worldId)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		if (this.worldIdToDataMap.TryGetValue(worldId, out spaceScannerWorldData))
		{
			return spaceScannerWorldData.networkQuality01;
		}
		return 0f;
	}

	// Token: 0x06005AD2 RID: 23250 RVA: 0x0020E138 File Offset: 0x0020C338
	private SpaceScannerWorldData GetOrCreateWorldData(int worldId)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		if (!this.worldIdToDataMap.TryGetValue(worldId, out spaceScannerWorldData))
		{
			spaceScannerWorldData = new SpaceScannerWorldData(worldId);
			this.worldIdToDataMap[worldId] = spaceScannerWorldData;
		}
		return spaceScannerWorldData;
	}

	// Token: 0x06005AD3 RID: 23251 RVA: 0x0020E16C File Offset: 0x0020C36C
	public void Sim1000ms(float dt)
	{
		SpaceScannerNetworkManager.UpdateWorldDataScratchpads(this.worldIdToDataMap);
		foreach (int id in Components.DetectorNetworks.GetWorldsIds())
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(id);
			if (!world.IsModuleInterior && world.IsDiscovered)
			{
				SpaceScannerWorldData orCreateWorldData = this.GetOrCreateWorldData(world.id);
				SpaceScannerNetworkManager.UpdateNetworkQualityFor(orCreateWorldData);
				SpaceScannerNetworkManager.UpdateDetectionOfTargetsFor(orCreateWorldData);
			}
		}
	}

	// Token: 0x06005AD4 RID: 23252 RVA: 0x0020E1FC File Offset: 0x0020C3FC
	private static void UpdateNetworkQualityFor(SpaceScannerWorldData worldData)
	{
		float num = SpaceScannerNetworkManager.CalcWorldNetworkQuality(worldData.GetWorld());
		foreach (object obj in Components.DetectorNetworks.CreateOrGetCmps(worldData.GetWorld().id))
		{
			((DetectorNetwork.Instance)obj).Internal_SetNetworkQuality(num);
		}
		worldData.networkQuality01 = num;
	}

	// Token: 0x06005AD5 RID: 23253 RVA: 0x0020E278 File Offset: 0x0020C478
	private static void UpdateDetectionOfTargetsFor(SpaceScannerWorldData worldData)
	{
		using (HashSetPool<string, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<string>())
		{
			using (HashSetPool<string, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet2 = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<string>())
			{
				foreach (string item in worldData.targetIdsDetected)
				{
					pooledHashSet.Add(item);
					pooledHashSet2.Add(item);
				}
				worldData.targetIdsDetected.Clear();
				if (SpaceScannerNetworkManager.IsDetectingAnyMeteorShower(worldData))
				{
					worldData.targetIdsDetected.Add(SpaceScannerTarget.MeteorShower().id);
				}
				if (SpaceScannerNetworkManager.IsDetectingAnyBallisticObject(worldData))
				{
					worldData.targetIdsDetected.Add(SpaceScannerTarget.BallisticObject().id);
				}
				foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
				{
					if (SpaceScannerNetworkManager.IsDetectingRocketBaseGame(worldData, spacecraft.launchConditions))
					{
						worldData.targetIdsDetected.Add(SpaceScannerTarget.RocketBaseGame(spacecraft.launchConditions).id);
					}
				}
				foreach (object obj in Components.Clustercrafts)
				{
					Clustercraft clustercraft = (Clustercraft)obj;
					if (SpaceScannerNetworkManager.IsDetectingRocketDlc1(worldData, clustercraft))
					{
						worldData.targetIdsDetected.Add(SpaceScannerTarget.RocketDlc1(clustercraft).id);
					}
				}
				foreach (string item2 in worldData.targetIdsDetected)
				{
					pooledHashSet2.Add(item2);
				}
				foreach (string text in pooledHashSet2)
				{
					bool flag = pooledHashSet.Contains(text);
					if (!worldData.targetIdsDetected.Contains(text) && flag)
					{
						worldData.targetIdToRandomValue01Map[text] = UnityEngine.Random.value;
					}
				}
			}
		}
	}

	// Token: 0x06005AD6 RID: 23254 RVA: 0x0020E52C File Offset: 0x0020C72C
	private static bool IsDetectingAnyMeteorShower(SpaceScannerWorldData worldData)
	{
		SpaceScannerNetworkManager.meteorShowerInstances.Clear();
		SaveGame.Instance.GetComponent<GameplayEventManager>().GetActiveEventsOfType<MeteorShowerEvent>(worldData.GetWorld().id, ref SpaceScannerNetworkManager.meteorShowerInstances);
		float detectTime = SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.MeteorShower());
		MeteorShowerEvent.StatesInstance candidateTarget = null;
		float num = float.MaxValue;
		foreach (GameplayEventInstance gameplayEventInstance in SpaceScannerNetworkManager.meteorShowerInstances)
		{
			MeteorShowerEvent.StatesInstance statesInstance = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
			if (statesInstance != null)
			{
				float num2 = statesInstance.TimeUntilNextShower();
				if (num2 < num)
				{
					num = num2;
					candidateTarget = statesInstance;
				}
				if (num2 <= detectTime)
				{
					worldData.scratchpad.lastDetectedMeteorShowers.Add(statesInstance);
				}
			}
		}
		return SpaceScannerNetworkManager.IsDetectedUsingStickyCheck<MeteorShowerEvent.StatesInstance>(candidateTarget, num <= detectTime, worldData.scratchpad.lastDetectedMeteorShowers);
	}

	// Token: 0x06005AD7 RID: 23255 RVA: 0x0020E608 File Offset: 0x0020C808
	private static bool IsDetectingAnyBallisticObject(SpaceScannerWorldData worldData)
	{
		float num = float.MaxValue;
		foreach (ClusterTraveler clusterTraveler in worldData.scratchpad.ballisticObjects)
		{
			num = Mathf.Min(num, clusterTraveler.TravelETA());
		}
		return num < SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.BallisticObject());
	}

	// Token: 0x06005AD8 RID: 23256 RVA: 0x0020E67C File Offset: 0x0020C87C
	private static bool IsDetectingRocketBaseGame(SpaceScannerWorldData worldData, LaunchConditionManager rocket)
	{
		Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(rocket);
		return SpaceScannerNetworkManager.IsDetectedUsingStickyCheck<LaunchConditionManager>(rocket, SpaceScannerNetworkManager.<IsDetectingRocketBaseGame>g__IsDetected|12_0(worldData, spacecraftFromLaunchConditionManager, rocket), worldData.scratchpad.lastDetectedRocketsBaseGame);
	}

	// Token: 0x06005AD9 RID: 23257 RVA: 0x0020E6B0 File Offset: 0x0020C8B0
	private static bool IsDetectingRocketDlc1(SpaceScannerWorldData worldData, Clustercraft clustercraft)
	{
		if (clustercraft.IsNullOrDestroyed())
		{
			return false;
		}
		ClusterTraveler component = clustercraft.GetComponent<ClusterTraveler>();
		bool flag = false;
		if (clustercraft.Status != Clustercraft.CraftStatus.Grounded)
		{
			bool flag2 = component.GetDestinationWorldID() == worldData.GetWorld().id;
			bool flag3 = component.IsTraveling();
			bool flag4 = clustercraft.HasResourcesToMove(1, Clustercraft.CombustionResource.All);
			float num = component.TravelETA();
			flag = ((flag2 && flag3 && flag4 && num < SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.RocketDlc1(clustercraft))) || (!flag3 && flag2 && clustercraft.Status == Clustercraft.CraftStatus.Landing));
			if (!flag)
			{
				ClusterGridEntity adjacentAsteroid = clustercraft.GetAdjacentAsteroid();
				flag = (((adjacentAsteroid != null) ? ClusterUtil.GetAsteroidWorldIdAtLocation(adjacentAsteroid.Location) : 255) == worldData.GetWorld().id && clustercraft.Status == Clustercraft.CraftStatus.Launching);
			}
		}
		return SpaceScannerNetworkManager.IsDetectedUsingStickyCheck<Clustercraft>(clustercraft, flag, worldData.scratchpad.lastDetectedRocketsDLC1);
	}

	// Token: 0x06005ADA RID: 23258 RVA: 0x0020E793 File Offset: 0x0020C993
	private static bool IsDetectedUsingStickyCheck<T>(T candidateTarget, bool isDetected, HashSet<T> existingDetections)
	{
		if (isDetected)
		{
			existingDetections.Add(candidateTarget);
		}
		else if (existingDetections.Contains(candidateTarget))
		{
			isDetected = true;
		}
		return isDetected;
	}

	// Token: 0x06005ADB RID: 23259 RVA: 0x0020E7B0 File Offset: 0x0020C9B0
	private static float GetDetectTime(SpaceScannerWorldData worldData, SpaceScannerTarget target)
	{
		float value;
		if (!worldData.targetIdToRandomValue01Map.TryGetValue(target.id, out value))
		{
			value = UnityEngine.Random.value;
			worldData.targetIdToRandomValue01Map[target.id] = value;
		}
		return SpaceScannerNetworkManager.GetDetectTimeRange(worldData.networkQuality01).Lerp(value);
	}

	// Token: 0x06005ADC RID: 23260 RVA: 0x0020E7FE File Offset: 0x0020C9FE
	private static MathUtil.MinMax GetDetectTimeRange(float networkQuality01)
	{
		return new MathUtil.MinMax(Mathf.Lerp(1f, 200f, networkQuality01), 200f);
	}

	// Token: 0x06005ADD RID: 23261 RVA: 0x0020E81C File Offset: 0x0020CA1C
	private static float CalcWorldNetworkQuality(WorldContainer world)
	{
		int width = world.Width;
		global::Debug.Assert(width <= 1024, "More world columns than expected");
		bool[] array = new bool[width];
		for (int i = 0; i < width; i++)
		{
			array[i] = false;
		}
		using (HashSetPool<int, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<int>())
		{
			foreach (object obj in Components.DetectorNetworks.CreateOrGetCmps(world.id))
			{
				DetectorNetwork.Instance instance = (DetectorNetwork.Instance)obj;
				if (instance.GetComponent<Operational>().IsOperational)
				{
					CometDetectorConfig.SKY_VISIBILITY_INFO.CollectVisibleCellsTo(pooledHashSet, Grid.PosToCell(instance.gameObject.transform.position), world);
				}
			}
			foreach (int cell in pooledHashSet)
			{
				int num = Grid.CellToXY(cell).x - world.WorldOffset.x;
				if (num >= 0 && num < world.Width)
				{
					array[num] = true;
				}
			}
		}
		int num2 = 0;
		for (int j = 0; j < width; j++)
		{
			if (array[j])
			{
				num2++;
			}
		}
		return Mathf.Clamp01(((float)num2 / (float)width).Remap(new ValueTuple<float, float>(0f, 0.5f), new ValueTuple<float, float>(0f, 1f)));
	}

	// Token: 0x06005ADE RID: 23262 RVA: 0x0020E9B0 File Offset: 0x0020CBB0
	private static void UpdateWorldDataScratchpads(Dictionary<int, SpaceScannerWorldData> worldIdToDataMap)
	{
		foreach (KeyValuePair<int, SpaceScannerWorldData> keyValuePair in worldIdToDataMap)
		{
			int num;
			SpaceScannerWorldData worldData2;
			keyValuePair.Deconstruct(out num, out worldData2);
			SpaceScannerWorldData worldData = worldData2;
			if (worldData.scratchpad == null)
			{
				worldData.scratchpad = new SpaceScannerWorldData.Scratchpad();
			}
			worldData.scratchpad.ballisticObjects.Clear();
			worldData.scratchpad.lastDetectedMeteorShowers.RemoveWhere((MeteorShowerEvent.StatesInstance meteorShower) => meteorShower.IsNullOrDestroyed() || meteorShower.IsNullOrStopped() || 200f < meteorShower.TimeUntilNextShower());
			worldData.scratchpad.lastDetectedRocketsBaseGame.RemoveWhere(delegate(LaunchConditionManager rocket)
			{
				if (rocket.IsNullOrDestroyed())
				{
					return true;
				}
				Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(rocket);
				return spacecraftFromLaunchConditionManager.IsNullOrDestroyed() || spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Destroyed || (spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Underway && 200f < spacecraftFromLaunchConditionManager.GetTimeLeft()) || spacecraftFromLaunchConditionManager.GetTimeLeft() < 1f;
			});
			worldData.scratchpad.lastDetectedRocketsDLC1.RemoveWhere(delegate(Clustercraft clustercraft)
			{
				if (clustercraft.IsNullOrDestroyed())
				{
					return true;
				}
				ClusterTraveler component = clustercraft.GetComponent<ClusterTraveler>();
				if (component.IsNullOrDestroyed())
				{
					return true;
				}
				if (component.IsTraveling())
				{
					if (component.GetDestinationWorldID() != worldData.worldId)
					{
						return true;
					}
					if (200f < component.TravelETA())
					{
						return true;
					}
				}
				return component.TravelETA() < 1f;
			});
		}
		if (Components.DetectorNetworks.GetWorldsIds().Count == 0)
		{
			return;
		}
		foreach (object obj in Components.ClusterTravelers)
		{
			ClusterTraveler clusterTraveler = (ClusterTraveler)obj;
			SpaceScannerWorldData spaceScannerWorldData;
			if (clusterTraveler.IsTraveling() && clusterTraveler.GetComponent<Clustercraft>().IsNullOrDestroyed() && worldIdToDataMap.TryGetValue(clusterTraveler.GetDestinationWorldID(), out spaceScannerWorldData))
			{
				spaceScannerWorldData.scratchpad.ballisticObjects.Add(clusterTraveler);
			}
		}
	}

	// Token: 0x06005AE1 RID: 23265 RVA: 0x0020EB8C File Offset: 0x0020CD8C
	[CompilerGenerated]
	internal static bool <IsDetectingRocketBaseGame>g__IsDetected|12_0(SpaceScannerWorldData worldData, Spacecraft spacecraft, LaunchConditionManager rocket)
	{
		if (spacecraft.IsNullOrDestroyed())
		{
			return false;
		}
		if (spacecraft.state == Spacecraft.MissionState.Destroyed)
		{
			return false;
		}
		switch (spacecraft.state)
		{
		case Spacecraft.MissionState.Launching:
		case Spacecraft.MissionState.WaitingToLand:
		case Spacecraft.MissionState.Landing:
			return true;
		case Spacecraft.MissionState.Underway:
			return spacecraft.GetTimeLeft() <= SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.RocketBaseGame(rocket));
		case Spacecraft.MissionState.Destroyed:
			return false;
		default:
			return false;
		}
	}

	// Token: 0x04003C8E RID: 15502
	[Serialize]
	private Dictionary<int, SpaceScannerWorldData> worldIdToDataMap = new Dictionary<int, SpaceScannerWorldData>();

	// Token: 0x04003C8F RID: 15503
	private static List<GameplayEventInstance> meteorShowerInstances = new List<GameplayEventInstance>();
}
