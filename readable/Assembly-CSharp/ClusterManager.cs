using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using ProcGenGame;
using TUNING;
using UnityEngine;

// Token: 0x0200084D RID: 2125
public class ClusterManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06003A2E RID: 14894 RVA: 0x00144D7C File Offset: 0x00142F7C
	public static void DestroyInstance()
	{
		ClusterManager.Instance = null;
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06003A2F RID: 14895 RVA: 0x00144D84 File Offset: 0x00142F84
	public int worldCount
	{
		get
		{
			return this.m_worldContainers.Count;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06003A30 RID: 14896 RVA: 0x00144D91 File Offset: 0x00142F91
	public int activeWorldId
	{
		get
		{
			return this.activeWorldIdx;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06003A31 RID: 14897 RVA: 0x00144D99 File Offset: 0x00142F99
	public List<WorldContainer> WorldContainers
	{
		get
		{
			return this.m_worldContainers;
		}
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x00144DA1 File Offset: 0x00142FA1
	public ClusterPOIManager GetClusterPOIManager()
	{
		return this.m_clusterPOIsManager;
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06003A33 RID: 14899 RVA: 0x00144DAC File Offset: 0x00142FAC
	public Dictionary<int, List<IAssignableIdentity>> MinionsByWorld
	{
		get
		{
			this.minionsByWorld.Clear();
			for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
			{
				if (!Components.MinionAssignablesProxy[i].GetTargetGameObject().HasTag(GameTags.Dead))
				{
					int id = Components.MinionAssignablesProxy[i].GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld().id;
					if (!this.minionsByWorld.ContainsKey(id))
					{
						this.minionsByWorld.Add(id, new List<IAssignableIdentity>());
					}
					this.minionsByWorld[id].Add(Components.MinionAssignablesProxy[i]);
				}
			}
			return this.minionsByWorld;
		}
	}

	// Token: 0x06003A34 RID: 14900 RVA: 0x00144E59 File Offset: 0x00143059
	public void RegisterWorldContainer(WorldContainer worldContainer)
	{
		this.m_worldContainers.Add(worldContainer);
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x00144E67 File Offset: 0x00143067
	public void UnregisterWorldContainer(WorldContainer worldContainer)
	{
		base.BoxingTrigger<int>(-1078710002, worldContainer.id);
		this.m_worldContainers.Remove(worldContainer);
	}

	// Token: 0x06003A36 RID: 14902 RVA: 0x00144E88 File Offset: 0x00143088
	public List<int> GetWorldIDsSorted()
	{
		ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
		pooledList.Sort((WorldContainer a, WorldContainer b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp));
		this._worldIDs.Clear();
		foreach (WorldContainer worldContainer in pooledList)
		{
			this._worldIDs.Add(worldContainer.id);
		}
		pooledList.Recycle();
		return this._worldIDs;
	}

	// Token: 0x06003A37 RID: 14903 RVA: 0x00144F28 File Offset: 0x00143128
	public List<int> GetDiscoveredAsteroidIDsSorted()
	{
		ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
		pooledList.Sort((WorldContainer a, WorldContainer b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp));
		this._discoveredAsteroidIds.Clear();
		for (int i = 0; i < pooledList.Count; i++)
		{
			if (pooledList[i].IsDiscovered && !pooledList[i].IsModuleInterior)
			{
				this._discoveredAsteroidIds.Add(pooledList[i].id);
			}
		}
		pooledList.Recycle();
		return this._discoveredAsteroidIds;
	}

	// Token: 0x06003A38 RID: 14904 RVA: 0x00144FC4 File Offset: 0x001431C4
	public WorldContainer GetStartWorld()
	{
		foreach (WorldContainer worldContainer in this.WorldContainers)
		{
			if (worldContainer.IsStartWorld)
			{
				return worldContainer;
			}
		}
		return this.WorldContainers[0];
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x0014502C File Offset: 0x0014322C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterManager.Instance = this;
		SaveLoader instance = SaveLoader.Instance;
		instance.OnWorldGenComplete = (Action<Cluster>)Delegate.Combine(instance.OnWorldGenComplete, new Action<Cluster>(this.OnWorldGenComplete));
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x00145060 File Offset: 0x00143260
	protected override void OnSpawn()
	{
		if (this.m_grid == null)
		{
			this.m_grid = new ClusterGrid(this.m_numRings);
		}
		this.UpdateWorldReverbSnapshot(this.activeWorldId);
		base.OnSpawn();
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x0014508D File Offset: 0x0014328D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06003A3C RID: 14908 RVA: 0x00145095 File Offset: 0x00143295
	public WorldContainer activeWorld
	{
		get
		{
			return this.GetWorld(this.activeWorldId);
		}
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x001450A4 File Offset: 0x001432A4
	private void OnWorldGenComplete(Cluster clusterLayout)
	{
		this.m_numRings = clusterLayout.numRings;
		this.m_grid = new ClusterGrid(this.m_numRings);
		AxialI location = AxialI.ZERO;
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			int id = this.CreateAsteroidWorldContainer(worldGen).id;
			Vector2I position = worldGen.GetPosition();
			Vector2I vector2I = position + worldGen.GetSize();
			if (worldGen.isStartingWorld)
			{
				location = worldGen.GetClusterLocation();
			}
			for (int i = position.y; i < vector2I.y; i++)
			{
				for (int j = position.x; j < vector2I.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)id;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			if (worldGen.isStartingWorld)
			{
				this.activeWorldIdx = id;
			}
		}
		this.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 1, 2);
		this.m_clusterPOIsManager.PopulatePOIsFromWorldGen(clusterLayout);
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x001451D0 File Offset: 0x001433D0
	private int GetNextWorldId()
	{
		HashSetPool<int, ClusterManager>.PooledHashSet pooledHashSet = HashSetPool<int, ClusterManager>.Allocate();
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			pooledHashSet.Add(worldContainer.id);
		}
		global::Debug.Assert(this.m_worldContainers.Count < 255, "Oh no! We're trying to generate our 255th world in this save, things are going to start going badly...");
		for (int i = 0; i < 255; i++)
		{
			if (!pooledHashSet.Contains(i))
			{
				pooledHashSet.Recycle();
				return i;
			}
		}
		pooledHashSet.Recycle();
		return 255;
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x00145278 File Offset: 0x00143478
	private WorldContainer CreateAsteroidWorldContainer(WorldGen world)
	{
		int nextWorldId = this.GetNextWorldId();
		GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);
		WorldContainer component = gameObject.GetComponent<WorldContainer>();
		component.SetID(nextWorldId);
		component.SetWorldDetails(world);
		AsteroidGridEntity component2 = gameObject.GetComponent<AsteroidGridEntity>();
		if (world != null)
		{
			AxialI clusterLocation = world.GetClusterLocation();
			component2.Init(component.GetRandomName(), clusterLocation, world.Settings.world.asteroidIcon);
		}
		else
		{
			component2.Init("", AxialI.ZERO, "");
		}
		if (component.IsStartWorld)
		{
			OrbitalMechanics component3 = gameObject.GetComponent<OrbitalMechanics>();
			if (component3 != null)
			{
				component3.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.backgroundEarth.Id);
			}
		}
		gameObject.SetActive(true);
		return component;
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x0014533C File Offset: 0x0014353C
	private void CreateDefaultAsteroidWorldContainer()
	{
		if (this.m_worldContainers.Count == 0)
		{
			global::Debug.LogWarning("Cluster manager has no world containers, create a default using Grid settings.");
			WorldContainer worldContainer = this.CreateAsteroidWorldContainer(null);
			int id = worldContainer.id;
			int num = (int)worldContainer.minimumBounds.y;
			while ((float)num <= worldContainer.maximumBounds.y)
			{
				int num2 = (int)worldContainer.minimumBounds.x;
				while ((float)num2 <= worldContainer.maximumBounds.x)
				{
					int num3 = Grid.XYToCell(num2, num);
					Grid.WorldIdx[num3] = (byte)id;
					Pathfinding.Instance.AddDirtyNavGridCell(num3);
					num2++;
				}
				num++;
			}
		}
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x001453D4 File Offset: 0x001435D4
	public void InitializeWorldGrid()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.CreateDefaultAsteroidWorldContainer();
		}
		bool flag = false;
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			Vector2I worldOffset = worldContainer.WorldOffset;
			Vector2I vector2I = worldOffset + worldContainer.WorldSize;
			for (int i = worldOffset.y; i < vector2I.y; i++)
			{
				for (int j = worldOffset.x; j < vector2I.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)worldContainer.id;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			flag |= worldContainer.IsDiscovered;
		}
		if (!flag)
		{
			global::Debug.LogWarning("No worlds have been discovered. Setting the active world to discovered");
			this.activeWorld.SetDiscovered(false);
		}
	}

	// Token: 0x06003A42 RID: 14914 RVA: 0x001454DC File Offset: 0x001436DC
	public void SetActiveWorld(int worldIdx)
	{
		int num = this.activeWorldIdx;
		if (num != worldIdx)
		{
			this.activeWorldIdx = worldIdx;
			this.activeWorldChangedData.first = this.activeWorldIdx;
			this.activeWorldChangedData.second = num;
			Game.Instance.Trigger(1983128072, this.activeWorldChangedData);
			this.UpdateRocketInteriorAudio();
		}
	}

	// Token: 0x06003A43 RID: 14915 RVA: 0x00145533 File Offset: 0x00143733
	public void TimelapseModeOverrideActiveWorld(int overrideValue)
	{
		this.activeWorldIdx = overrideValue;
	}

	// Token: 0x06003A44 RID: 14916 RVA: 0x0014553C File Offset: 0x0014373C
	public WorldContainer GetWorld(int id)
	{
		for (int i = 0; i < this.m_worldContainers.Count; i++)
		{
			if (this.m_worldContainers[i].id == id)
			{
				return this.m_worldContainers[i];
			}
		}
		return null;
	}

	// Token: 0x06003A45 RID: 14917 RVA: 0x00145584 File Offset: 0x00143784
	public WorldContainer GetWorldFromPosition(Vector3 position)
	{
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			if (worldContainer.ContainsPoint(position))
			{
				return worldContainer;
			}
		}
		return null;
	}

	// Token: 0x06003A46 RID: 14918 RVA: 0x001455E8 File Offset: 0x001437E8
	public float CountAllRations()
	{
		float result = 0f;
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, worldContainer.worldInventory, true);
		}
		return result;
	}

	// Token: 0x06003A47 RID: 14919 RVA: 0x00145650 File Offset: 0x00143850
	public Dictionary<Tag, float> GetAllWorldsAccessibleAmounts()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			foreach (KeyValuePair<Tag, float> keyValuePair in worldContainer.worldInventory.GetAccessibleAmounts())
			{
				if (dictionary.ContainsKey(keyValuePair.Key))
				{
					Dictionary<Tag, float> dictionary2 = dictionary;
					Tag key = keyValuePair.Key;
					dictionary2[key] += keyValuePair.Value;
				}
				else
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x00145734 File Offset: 0x00143934
	public void MigrateMinion(MinionIdentity minion, int targetID)
	{
		this.MigrateMinion(minion, targetID, minion.GetMyWorldId());
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x00145744 File Offset: 0x00143944
	public void MigrateCritter(GameObject critter, int targetID)
	{
		this.MigrateCritter(critter, targetID, critter.GetMyWorldId());
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x00145754 File Offset: 0x00143954
	public void MigrateCritter(GameObject critter, int targetID, int prevID)
	{
		this.critterMigrationEvArg.entity = critter;
		this.critterMigrationEvArg.prevWorldId = prevID;
		this.critterMigrationEvArg.targetWorldId = targetID;
		Game.Instance.Trigger(1142724171, this.critterMigrationEvArg);
	}

	// Token: 0x06003A4B RID: 14923 RVA: 0x00145790 File Offset: 0x00143990
	public void MigrateMinion(MinionIdentity minion, int targetID, int prevID)
	{
		if (!ClusterManager.Instance.GetWorld(targetID).IsDiscovered)
		{
			ClusterManager.Instance.GetWorld(targetID).SetDiscovered(false);
		}
		if (!ClusterManager.Instance.GetWorld(targetID).IsDupeVisited)
		{
			ClusterManager.Instance.GetWorld(targetID).SetDupeVisited();
		}
		this.migrationEvArg.minionId = minion;
		this.migrationEvArg.prevWorldId = prevID;
		this.migrationEvArg.targetWorldId = targetID;
		Game.Instance.assignmentManager.RemoveFromWorld(minion, this.migrationEvArg.prevWorldId);
		Game.Instance.Trigger(586301400, this.migrationEvArg);
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x00145838 File Offset: 0x00143A38
	public int GetLandingBeaconLocation(int worldId)
	{
		foreach (object obj in Components.LandingBeacons)
		{
			LandingBeacon.Instance instance = (LandingBeacon.Instance)obj;
			if (instance.GetMyWorldId() == worldId && instance.CanBeTargeted())
			{
				return Grid.PosToCell(instance);
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x001458AC File Offset: 0x00143AAC
	public int GetRandomClearCell(int worldId)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < 1000)
		{
			num++;
			int num2 = UnityEngine.Random.Range(0, Grid.CellCount);
			if (!Grid.Solid[num2] && !Grid.IsLiquid(num2) && (int)Grid.WorldIdx[num2] == worldId)
			{
				return num2;
			}
		}
		num = 0;
		while (!flag && num < 1000)
		{
			num++;
			int num3 = UnityEngine.Random.Range(0, Grid.CellCount);
			if (!Grid.Solid[num3] && (int)Grid.WorldIdx[num3] == worldId)
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x00145938 File Offset: 0x00143B38
	private bool NotObstructedCell(int x, int y)
	{
		int cell = Grid.XYToCell(x, y);
		return Grid.IsValidCell(cell) && Grid.Objects[cell, 1] == null;
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x0014596C File Offset: 0x00143B6C
	private int LowestYThatSeesSky(int topCellYPos, int x)
	{
		int num = topCellYPos;
		while (!this.ValidSurfaceCell(x, num))
		{
			num--;
		}
		return num;
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x0014598C File Offset: 0x00143B8C
	private bool ValidSurfaceCell(int x, int y)
	{
		int i = Grid.XYToCell(x, y - 1);
		return Grid.Solid[i] || Grid.Foundation[i];
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x001459C0 File Offset: 0x00143BC0
	public int GetRandomSurfaceCell(int worldID, int width = 1, bool excludeTopBorderHeight = true)
	{
		WorldContainer worldContainer = this.m_worldContainers.Find((WorldContainer match) => match.id == worldID);
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + (float)(worldContainer.Width / 10), worldContainer.maximumBounds.x - (float)(worldContainer.Width / 10)));
		int num2 = Mathf.RoundToInt(worldContainer.maximumBounds.y);
		if (excludeTopBorderHeight)
		{
			num2 -= Grid.TopBorderHeight;
		}
		int num3 = num;
		int num4 = this.LowestYThatSeesSky(num2, num3);
		int num5;
		if (this.NotObstructedCell(num3, num4))
		{
			num5 = 1;
		}
		else
		{
			num5 = 0;
		}
		while (num3 + 1 != num && num5 < width)
		{
			num3++;
			if ((float)num3 > worldContainer.maximumBounds.x)
			{
				num5 = 0;
				num3 = (int)worldContainer.minimumBounds.x;
			}
			int num6 = this.LowestYThatSeesSky(num2, num3);
			bool flag = this.NotObstructedCell(num3, num6);
			if (num6 == num4 && flag)
			{
				num5++;
			}
			else if (flag)
			{
				num5 = 1;
			}
			else
			{
				num5 = 0;
			}
			num4 = num6;
		}
		if (num5 < width)
		{
			return -1;
		}
		return Grid.XYToCell(num3, num4);
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x00145AEC File Offset: 0x00143CEC
	public bool IsPositionInActiveWorld(Vector3 pos)
	{
		if (this.activeWorld != null && !CameraController.Instance.ignoreClusterFX)
		{
			Vector2 vector = this.activeWorld.maximumBounds * Grid.CellSizeInMeters + new Vector2(1f, 1f);
			Vector2 vector2 = this.activeWorld.minimumBounds * Grid.CellSizeInMeters;
			if (pos.x < vector2.x || pos.x > vector.x || pos.y < vector2.y || pos.y > vector.y)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x00145B94 File Offset: 0x00143D94
	public WorldContainer CreateRocketInteriorWorld(GameObject craft_go, string interiorTemplateName, System.Action callback)
	{
		Vector2I rocket_INTERIOR_SIZE = ROCKETRY.ROCKET_INTERIOR_SIZE;
		Vector2I vector2I;
		if (Grid.GetFreeGridSpace(rocket_INTERIOR_SIZE, out vector2I))
		{
			int nextWorldId = this.GetNextWorldId();
			craft_go.AddComponent<WorldInventory>();
			WorldContainer worldContainer = craft_go.AddComponent<WorldContainer>();
			worldContainer.SetRocketInteriorWorldDetails(nextWorldId, rocket_INTERIOR_SIZE, vector2I);
			Vector2I vector2I2 = vector2I + rocket_INTERIOR_SIZE;
			for (int i = vector2I.y; i < vector2I2.y; i++)
			{
				for (int j = vector2I.x; j < vector2I2.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)nextWorldId;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			global::Debug.Log(string.Format("Created new rocket interior id: {0}, at {1} with size {2}", nextWorldId, vector2I, rocket_INTERIOR_SIZE));
			worldContainer.PlaceInteriorTemplate(interiorTemplateName, delegate
			{
				if (callback != null)
				{
					callback();
				}
				craft_go.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.RocketInteriorComplete, null);
			});
			craft_go.AddOrGet<OrbitalMechanics>().CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
			base.BoxingTrigger<int>(-1280433810, worldContainer.id);
			return worldContainer;
		}
		global::Debug.LogError("Failed to create rocket interior.");
		return null;
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x00145CCC File Offset: 0x00143ECC
	public void DestoryRocketInteriorWorld(int world_id, ClustercraftExteriorDoor door)
	{
		WorldContainer world = this.GetWorld(world_id);
		if (world == null || !world.IsModuleInterior)
		{
			global::Debug.LogError(string.Format("Attempting to destroy world id {0}. The world is not a valid rocket interior", world_id));
			return;
		}
		GameObject gameObject = door.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
		if (this.activeWorldId == world_id)
		{
			if (gameObject.GetComponent<WorldContainer>().ParentWorldId == world_id)
			{
				this.SetActiveWorld(ClusterManager.Instance.GetStartWorld().id);
			}
			else
			{
				this.SetActiveWorld(gameObject.GetComponent<WorldContainer>().ParentWorldId);
			}
		}
		OrbitalMechanics component = gameObject.GetComponent<OrbitalMechanics>();
		if (!component.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(component);
		}
		bool flag = gameObject.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.InFlight;
		PrimaryElement moduleElemet = door.GetComponent<PrimaryElement>();
		AxialI clusterLocation = world.GetComponent<ClusterGridEntity>().Location;
		Vector3 rocketModuleWorldPos = door.transform.position;
		if (!flag)
		{
			world.EjectAllDupes(rocketModuleWorldPos);
		}
		else
		{
			world.SpacePodAllDupes(clusterLocation, moduleElemet.ElementID);
		}
		world.CancelChores();
		HashSet<int> noRefundTiles;
		world.DestroyWorldBuildings(out noRefundTiles);
		this.UnregisterWorldContainer(world);
		if (!flag)
		{
			GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToParentWorld", delegate(object obj)
			{
				world.TransferResourcesToParentWorld(rocketModuleWorldPos + new Vector3(0f, 0.5f, 0f), noRefundTiles);
			}, null, null);
			GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", delegate(object obj)
			{
				this.DeleteWorldObjects(world);
			}, null, null);
			return;
		}
		GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToDebris", delegate(object obj)
		{
			world.TransferResourcesToDebris(clusterLocation, noRefundTiles, moduleElemet.ElementID);
		}, null, null);
		GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", delegate(object obj)
		{
			this.DeleteWorldObjects(world);
		}, null, null);
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x00145EA0 File Offset: 0x001440A0
	public void UpdateWorldReverbSnapshot(int worldId)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SmallRocketInteriorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MediumRocketInteriorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
		}
		AudioMixer.instance.PauseSpaceVisibleSnapshot(false);
		WorldContainer world = this.GetWorld(worldId);
		if (world.IsModuleInterior)
		{
			PassengerRocketModule passengerModule = world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule();
			AudioMixer.instance.Start(passengerModule.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			this.UpdateRocketInteriorAudio();
		}
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x00145F30 File Offset: 0x00144130
	public void UpdateRocketInteriorAudio()
	{
		WorldContainer activeWorld = this.activeWorld;
		if (activeWorld != null && activeWorld.IsModuleInterior)
		{
			activeWorld.minimumBounds + new Vector2((float)activeWorld.Width * Grid.CellSizeInMeters, (float)activeWorld.Height * Grid.CellSizeInMeters) / 2f;
			Clustercraft component = activeWorld.GetComponent<Clustercraft>();
			ClusterManager.RocketStatesForAudio rocketInteriorState = ClusterManager.RocketStatesForAudio.Grounded;
			switch (component.Status)
			{
			case Clustercraft.CraftStatus.Grounded:
				rocketInteriorState = (component.LaunchRequested ? ClusterManager.RocketStatesForAudio.ReadyForLaunch : ClusterManager.RocketStatesForAudio.Grounded);
				break;
			case Clustercraft.CraftStatus.Launching:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.Launching;
				break;
			case Clustercraft.CraftStatus.InFlight:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.InSpace;
				break;
			case Clustercraft.CraftStatus.Landing:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.Landing;
				break;
			}
			ClusterManager.RocketInteriorState = rocketInteriorState;
		}
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x00145FDC File Offset: 0x001441DC
	private void DeleteWorldObjects(WorldContainer world)
	{
		Grid.FreeGridSpace(world.WorldSize, world.WorldOffset);
		WorldInventory worldInventory = null;
		if (world != null)
		{
			worldInventory = world.GetComponent<WorldInventory>();
		}
		if (worldInventory != null)
		{
			UnityEngine.Object.Destroy(worldInventory);
		}
		if (world != null)
		{
			UnityEngine.Object.Destroy(world);
		}
	}

	// Token: 0x0400237D RID: 9085
	public static int MAX_ROCKET_INTERIOR_COUNT = 16;

	// Token: 0x0400237E RID: 9086
	public static ClusterManager.RocketStatesForAudio RocketInteriorState = ClusterManager.RocketStatesForAudio.Grounded;

	// Token: 0x0400237F RID: 9087
	public static ClusterManager Instance;

	// Token: 0x04002380 RID: 9088
	private ClusterGrid m_grid;

	// Token: 0x04002381 RID: 9089
	[Serialize]
	private int m_numRings = 9;

	// Token: 0x04002382 RID: 9090
	[Serialize]
	private int activeWorldIdx;

	// Token: 0x04002383 RID: 9091
	public const byte INVALID_WORLD_IDX = 255;

	// Token: 0x04002384 RID: 9092
	public static Color[] worldColors = new Color[]
	{
		Color.HSVToRGB(0.15f, 0.3f, 0.5f),
		Color.HSVToRGB(0.3f, 0.3f, 0.5f),
		Color.HSVToRGB(0.45f, 0.3f, 0.5f),
		Color.HSVToRGB(0.6f, 0.3f, 0.5f),
		Color.HSVToRGB(0.75f, 0.3f, 0.5f),
		Color.HSVToRGB(0.9f, 0.3f, 0.5f)
	};

	// Token: 0x04002385 RID: 9093
	private List<WorldContainer> m_worldContainers = new List<WorldContainer>();

	// Token: 0x04002386 RID: 9094
	[MyCmpGet]
	private ClusterPOIManager m_clusterPOIsManager;

	// Token: 0x04002387 RID: 9095
	private Dictionary<int, List<IAssignableIdentity>> minionsByWorld = new Dictionary<int, List<IAssignableIdentity>>();

	// Token: 0x04002388 RID: 9096
	private MinionMigrationEventArgs migrationEvArg = new MinionMigrationEventArgs();

	// Token: 0x04002389 RID: 9097
	private MigrationEventArgs critterMigrationEvArg = new MigrationEventArgs();

	// Token: 0x0400238A RID: 9098
	private List<int> _worldIDs = new List<int>();

	// Token: 0x0400238B RID: 9099
	private List<int> _discoveredAsteroidIds = new List<int>();

	// Token: 0x0400238C RID: 9100
	private global::Tuple<int, int> activeWorldChangedData = new global::Tuple<int, int>(0, 0);

	// Token: 0x020017FC RID: 6140
	public enum RocketStatesForAudio
	{
		// Token: 0x04007958 RID: 31064
		Grounded,
		// Token: 0x04007959 RID: 31065
		ReadyForLaunch,
		// Token: 0x0400795A RID: 31066
		Launching,
		// Token: 0x0400795B RID: 31067
		InSpace,
		// Token: 0x0400795C RID: 31068
		Landing
	}
}
