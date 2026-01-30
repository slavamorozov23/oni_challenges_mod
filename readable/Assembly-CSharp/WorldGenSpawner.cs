using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using UnityEngine;

// Token: 0x02000C27 RID: 3111
[AddComponentMenu("KMonoBehaviour/scripts/WorldGenSpawner")]
public class WorldGenSpawner : KMonoBehaviour
{
	// Token: 0x06005DF2 RID: 24050 RVA: 0x0021FBD7 File Offset: 0x0021DDD7
	public bool SpawnsRemain()
	{
		return this.spawnables.Count > 0;
	}

	// Token: 0x06005DF3 RID: 24051 RVA: 0x0021FBE8 File Offset: 0x0021DDE8
	public void SpawnEverything()
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			this.spawnables[i].TrySpawn();
		}
	}

	// Token: 0x06005DF4 RID: 24052 RVA: 0x0021FC1C File Offset: 0x0021DE1C
	public void SpawnTag(string id)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (this.spawnables[i].spawnInfo.id == id)
			{
				this.spawnables[i].TrySpawn();
			}
		}
	}

	// Token: 0x06005DF5 RID: 24053 RVA: 0x0021FC70 File Offset: 0x0021DE70
	public void ClearSpawnersInArea(Vector2 root_position, CellOffset[] area)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (Grid.IsCellOffsetOf(Grid.PosToCell(root_position), this.spawnables[i].cell, area))
			{
				this.spawnables[i].FreeResources();
			}
		}
	}

	// Token: 0x06005DF6 RID: 24054 RVA: 0x0021FCC3 File Offset: 0x0021DEC3
	public IReadOnlyList<WorldGenSpawner.Spawnable> GetSpawnables()
	{
		return this.spawnables;
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x0021FCCC File Offset: 0x0021DECC
	protected override void OnSpawn()
	{
		if (!this.hasPlacedTemplates)
		{
			global::Debug.Assert(SaveLoader.Instance.Cluster != null, "Trying to place templates for an already-loaded save, no worldgen data available");
			this.DoReveal(SaveLoader.Instance.Cluster);
			this.PlaceTemplates(SaveLoader.Instance.Cluster);
			this.hasPlacedTemplates = true;
		}
		if (this.spawnInfos == null)
		{
			return;
		}
		for (int i = 0; i < this.spawnInfos.Length; i++)
		{
			this.AddSpawnable(this.spawnInfos[i]);
		}
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x0021FD4C File Offset: 0x0021DF4C
	[OnSerializing]
	private void OnSerializing()
	{
		List<Prefab> list = new List<Prefab>();
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			WorldGenSpawner.Spawnable spawnable = this.spawnables[i];
			if (!spawnable.isSpawned)
			{
				list.Add(spawnable.spawnInfo);
			}
		}
		this.spawnInfos = list.ToArray();
	}

	// Token: 0x06005DF9 RID: 24057 RVA: 0x0021FDA2 File Offset: 0x0021DFA2
	private void AddSpawnable(Prefab prefab)
	{
		this.spawnables.Add(new WorldGenSpawner.Spawnable(prefab));
	}

	// Token: 0x06005DFA RID: 24058 RVA: 0x0021FDB8 File Offset: 0x0021DFB8
	public void AddLegacySpawner(Tag tag, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.AddSpawnable(new Prefab(tag.Name, Prefab.Type.Other, vector2I.x, vector2I.y, SimHashes.Carbon, -1f, 1f, null, 0, Orientation.Neutral, null, null, 0, null));
	}

	// Token: 0x06005DFB RID: 24059 RVA: 0x0021FE04 File Offset: 0x0021E004
	public List<Tag> GetUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => !match.isSpawned && (int)Grid.WorldIdx[match.cell] == worldID && Assets.GetPrefab(match.spawnInfo.id) != null && Assets.GetPrefab(match.spawnInfo.id).GetComponent<T>() != null));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x0021FEA0 File Offset: 0x0021E0A0
	public List<WorldGenSpawner.Spawnable> GeInfoOfUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => !match.isSpawned && (int)Grid.WorldIdx[match.cell] == worldID && Assets.GetPrefab(match.spawnInfo.id) != null && Assets.GetPrefab(match.spawnInfo.id).GetComponent<T>() != null));
		}
		foreach (WorldGenSpawner.Spawnable item in list2.FindAll(match2))
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x0021FF30 File Offset: 0x0021E130
	public WorldGenSpawner.Spawnable GetSpawnableInCell(int cell)
	{
		return this.spawnables.Find((WorldGenSpawner.Spawnable s) => s.cell == cell);
	}

	// Token: 0x06005DFE RID: 24062 RVA: 0x0021FF64 File Offset: 0x0021E164
	public List<Tag> GetSpawnersWithTag(Tag tag, int worldID, bool includeSpawned = false)
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => (includeSpawned || !match.isSpawned) && (int)Grid.WorldIdx[match.cell] == worldID && match.spawnInfo.id == tag));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06005DFF RID: 24063 RVA: 0x00220010 File Offset: 0x0021E210
	public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(Tag tag, int worldID, bool includeSpawned = false)
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => (includeSpawned || !match.isSpawned) && (int)Grid.WorldIdx[match.cell] == worldID && match.spawnInfo.id == tag));
		}
		foreach (WorldGenSpawner.Spawnable item in list2.FindAll(match2))
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06005E00 RID: 24064 RVA: 0x002200AC File Offset: 0x0021E2AC
	public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(bool includeSpawned = false, params Tag[] tags)
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => includeSpawned || !match.isSpawned));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			foreach (Tag b in tags)
			{
				if (spawnable.spawnInfo.id == b)
				{
					list.Add(spawnable);
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06005E01 RID: 24065 RVA: 0x00220178 File Offset: 0x0021E378
	private void PlaceTemplates(Cluster clusterLayout)
	{
		this.spawnables = new List<WorldGenSpawner.Spawnable>();
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			foreach (Prefab prefab in worldGen.SpawnData.buildings)
			{
				prefab.location_x += worldGen.data.world.offset.x;
				prefab.location_y += worldGen.data.world.offset.y;
				prefab.type = Prefab.Type.Building;
				this.AddSpawnable(prefab);
			}
			foreach (Prefab prefab2 in worldGen.SpawnData.elementalOres)
			{
				prefab2.location_x += worldGen.data.world.offset.x;
				prefab2.location_y += worldGen.data.world.offset.y;
				prefab2.type = Prefab.Type.Ore;
				this.AddSpawnable(prefab2);
			}
			foreach (Prefab prefab3 in worldGen.SpawnData.otherEntities)
			{
				prefab3.location_x += worldGen.data.world.offset.x;
				prefab3.location_y += worldGen.data.world.offset.y;
				prefab3.type = Prefab.Type.Other;
				this.AddSpawnable(prefab3);
			}
			foreach (Prefab prefab4 in worldGen.SpawnData.pickupables)
			{
				prefab4.location_x += worldGen.data.world.offset.x;
				prefab4.location_y += worldGen.data.world.offset.y;
				prefab4.type = Prefab.Type.Pickupable;
				this.AddSpawnable(prefab4);
			}
			foreach (Tag tag in worldGen.SpawnData.discoveredResources)
			{
				DiscoveredResources.Instance.Discover(tag);
			}
			worldGen.SpawnData.buildings.Clear();
			worldGen.SpawnData.elementalOres.Clear();
			worldGen.SpawnData.otherEntities.Clear();
			worldGen.SpawnData.pickupables.Clear();
			worldGen.SpawnData.discoveredResources.Clear();
		}
	}

	// Token: 0x06005E02 RID: 24066 RVA: 0x00220514 File Offset: 0x0021E714
	private void DoReveal(Cluster clusterLayout)
	{
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			Game.Instance.Reset(worldGen.SpawnData, worldGen.WorldOffset);
		}
		for (int i = 0; i < Grid.CellCount; i++)
		{
			Grid.Revealed[i] = false;
			Grid.Spawnable[i] = 0;
		}
		float innerRadius = 16.5f;
		int radius = 18;
		Vector2I vector2I = clusterLayout.currentWorld.SpawnData.baseStartPos;
		vector2I += clusterLayout.currentWorld.WorldOffset;
		GridVisibility.Reveal(vector2I.x, vector2I.y, radius, innerRadius);
	}

	// Token: 0x04003E7F RID: 15999
	[Serialize]
	private Prefab[] spawnInfos;

	// Token: 0x04003E80 RID: 16000
	[Serialize]
	private bool hasPlacedTemplates;

	// Token: 0x04003E81 RID: 16001
	private List<WorldGenSpawner.Spawnable> spawnables = new List<WorldGenSpawner.Spawnable>();

	// Token: 0x02001DBE RID: 7614
	public class Spawnable
	{
		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x0600B20B RID: 45579 RVA: 0x003DEF69 File Offset: 0x003DD169
		// (set) Token: 0x0600B20C RID: 45580 RVA: 0x003DEF71 File Offset: 0x003DD171
		public Prefab spawnInfo { get; private set; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x0600B20D RID: 45581 RVA: 0x003DEF7A File Offset: 0x003DD17A
		// (set) Token: 0x0600B20E RID: 45582 RVA: 0x003DEF82 File Offset: 0x003DD182
		public bool isSpawned { get; private set; }

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x0600B20F RID: 45583 RVA: 0x003DEF8B File Offset: 0x003DD18B
		// (set) Token: 0x0600B210 RID: 45584 RVA: 0x003DEF93 File Offset: 0x003DD193
		public int cell { get; private set; }

		// Token: 0x0600B211 RID: 45585 RVA: 0x003DEF9C File Offset: 0x003DD19C
		public Spawnable(Prefab spawn_info)
		{
			this.spawnInfo = spawn_info;
			int num = Grid.XYToCell(this.spawnInfo.location_x, this.spawnInfo.location_y);
			GameObject prefab = Assets.GetPrefab(spawn_info.id);
			if (prefab != null)
			{
				WorldSpawnableMonitor.Def def = prefab.GetDef<WorldSpawnableMonitor.Def>();
				if (def != null && def.adjustSpawnLocationCb != null)
				{
					num = def.adjustSpawnLocationCb(num);
				}
			}
			this.cell = num;
			global::Debug.Assert(Grid.IsValidCell(this.cell));
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
				return;
			}
			this.fogOfWarPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnReveal", this, this.cell, GameScenePartitioner.Instance.fogOfWarChangedLayer, new Action<object>(this.OnReveal));
		}

		// Token: 0x0600B212 RID: 45586 RVA: 0x003DF06A File Offset: 0x003DD26A
		private void OnReveal(object data)
		{
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
			}
		}

		// Token: 0x0600B213 RID: 45587 RVA: 0x003DF081 File Offset: 0x003DD281
		private void OnSolidChanged(object data)
		{
			if (!Grid.Solid[this.cell])
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				this.Spawn();
			}
		}

		// Token: 0x0600B214 RID: 45588 RVA: 0x003DF0C0 File Offset: 0x003DD2C0
		public void FreeResources()
		{
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				if (Game.Instance != null)
				{
					Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				}
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			this.isSpawned = true;
		}

		// Token: 0x0600B215 RID: 45589 RVA: 0x003DF124 File Offset: 0x003DD324
		public void TrySpawn()
		{
			if (this.isSpawned)
			{
				return;
			}
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				return;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[this.cell]);
			bool flag = world != null && world.IsDiscovered;
			GameObject prefab = Assets.GetPrefab(this.GetPrefabTag());
			if (!(prefab != null))
			{
				if (flag)
				{
					GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
					this.Spawn();
				}
				return;
			}
			if (!(flag | prefab.HasTag(GameTags.WarpTech)))
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			bool flag2 = false;
			if (prefab.GetComponent<Pickupable>() != null && !prefab.HasTag(GameTags.Creatures.Digger))
			{
				flag2 = true;
			}
			else if (prefab.GetDef<BurrowMonitor.Def>() != null)
			{
				flag2 = true;
			}
			if (flag2 && Grid.Solid[this.cell])
			{
				this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnSolidChanged", this, this.cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
				Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(this.cell);
				return;
			}
			this.Spawn();
		}

		// Token: 0x0600B216 RID: 45590 RVA: 0x003DF258 File Offset: 0x003DD458
		private Tag GetPrefabTag()
		{
			Mob mob = SettingsCache.mobs.GetMob(this.spawnInfo.id);
			if (mob != null && mob.prefabName != null)
			{
				return new Tag(mob.prefabName);
			}
			return new Tag(this.spawnInfo.id);
		}

		// Token: 0x0600B217 RID: 45591 RVA: 0x003DF2A4 File Offset: 0x003DD4A4
		private void Spawn()
		{
			this.isSpawned = true;
			GameObject gameObject = WorldGenSpawner.Spawnable.GetSpawnableCallback(this.spawnInfo.type)(this.spawnInfo, 0);
			if (gameObject != null && gameObject)
			{
				gameObject.SetActive(true);
				gameObject.Trigger(1119167081, this.spawnInfo);
			}
			this.FreeResources();
		}

		// Token: 0x0600B218 RID: 45592 RVA: 0x003DF304 File Offset: 0x003DD504
		public static WorldGenSpawner.Spawnable.PlaceEntityFn GetSpawnableCallback(Prefab.Type type)
		{
			switch (type)
			{
			case Prefab.Type.Building:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceBuilding);
			case Prefab.Type.Ore:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceElementalOres);
			case Prefab.Type.Pickupable:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlacePickupables);
			case Prefab.Type.Other:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			default:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			}
		}

		// Token: 0x04008C26 RID: 35878
		private HandleVector<int>.Handle fogOfWarPartitionerEntry;

		// Token: 0x04008C27 RID: 35879
		private HandleVector<int>.Handle solidChangedPartitionerEntry;

		// Token: 0x02002A52 RID: 10834
		// (Invoke) Token: 0x0600D458 RID: 54360
		public delegate GameObject PlaceEntityFn(Prefab prefab, int root_cell);
	}
}
