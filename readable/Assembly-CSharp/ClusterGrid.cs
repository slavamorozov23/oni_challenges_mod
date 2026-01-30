using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x02000BB0 RID: 2992
public class ClusterGrid
{
	// Token: 0x060059B5 RID: 22965 RVA: 0x00209110 File Offset: 0x00207310
	public static void DestroyInstance()
	{
		ClusterGrid.Instance = null;
	}

	// Token: 0x060059B6 RID: 22966 RVA: 0x00209118 File Offset: 0x00207318
	private ClusterFogOfWarManager.Instance GetFOWManager()
	{
		if (this.m_fowManager == null)
		{
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
		}
		return this.m_fowManager;
	}

	// Token: 0x060059B7 RID: 22967 RVA: 0x00209138 File Offset: 0x00207338
	public bool IsValidCell(AxialI cell)
	{
		return this.cellContents.ContainsKey(cell);
	}

	// Token: 0x060059B8 RID: 22968 RVA: 0x00209146 File Offset: 0x00207346
	public ClusterGrid(int numRings)
	{
		ClusterGrid.Instance = this;
		this.GenerateGrid(numRings);
		this.m_onClusterLocationChangedDelegate = new Action<object>(this.OnClusterLocationChanged);
	}

	// Token: 0x060059B9 RID: 22969 RVA: 0x00209178 File Offset: 0x00207378
	public ClusterRevealLevel GetCellRevealLevel(AxialI cell)
	{
		return this.GetFOWManager().GetCellRevealLevel(cell);
	}

	// Token: 0x060059BA RID: 22970 RVA: 0x00209186 File Offset: 0x00207386
	public bool IsCellVisible(AxialI cell)
	{
		return this.GetFOWManager().IsLocationRevealed(cell);
	}

	// Token: 0x060059BB RID: 22971 RVA: 0x00209194 File Offset: 0x00207394
	public float GetRevealCompleteFraction(AxialI cell)
	{
		return this.GetFOWManager().GetRevealCompleteFraction(cell);
	}

	// Token: 0x060059BC RID: 22972 RVA: 0x002091A2 File Offset: 0x002073A2
	public bool IsVisible(ClusterGridEntity entity)
	{
		return entity.IsVisible && this.IsCellVisible(entity.Location);
	}

	// Token: 0x060059BD RID: 22973 RVA: 0x002091BC File Offset: 0x002073BC
	public List<ClusterGridEntity> GetVisibleEntitiesAtCell(AxialI cell)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x060059BE RID: 22974 RVA: 0x0020921C File Offset: 0x0020741C
	public ClusterGridEntity GetVisibleEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			foreach (ClusterGridEntity clusterGridEntity in this.cellContents[cell])
			{
				if (clusterGridEntity.IsVisible && clusterGridEntity.Layer == entityLayer)
				{
					return clusterGridEntity;
				}
			}
		}
		return null;
	}

	// Token: 0x060059BF RID: 22975 RVA: 0x002092A0 File Offset: 0x002074A0
	public ClusterGridEntity GetVisibleEntityOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetVisibleEntitiesAtCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x060059C0 RID: 22976 RVA: 0x002092E4 File Offset: 0x002074E4
	public List<ClusterGridEntity> GetHiddenEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x060059C1 RID: 22977 RVA: 0x0020932C File Offset: 0x0020752C
	public List<ClusterGridEntity> GetEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x060059C2 RID: 22978 RVA: 0x00209374 File Offset: 0x00207574
	public StarmapHexCellInventory AddOrGetHexCellInventory(AxialI cell)
	{
		StarmapHexCellInventory starmapHexCellInventory = null;
		if (!StarmapHexCellInventory.AllInventories.TryGetValue(cell, out starmapHexCellInventory))
		{
			StarmapHexCellInventoryVisuals component = Util.KInstantiate(Assets.GetPrefab("StarmapHexCellInventory"), null, null).GetComponent<StarmapHexCellInventoryVisuals>();
			component.gameObject.name = component.gameObject.name + " [" + cell.ToString() + "]";
			component.Location = cell;
			component.gameObject.SetActive(true);
			starmapHexCellInventory = component.inventory;
			StarmapHexCellInventory.AllInventories.Add(cell, starmapHexCellInventory);
		}
		return starmapHexCellInventory;
	}

	// Token: 0x060059C3 RID: 22979 RVA: 0x00209408 File Offset: 0x00207608
	public ClusterGridEntity GetEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x060059C4 RID: 22980 RVA: 0x0020944C File Offset: 0x0020764C
	public List<ClusterGridEntity> GetHiddenEntitiesAtCell(AxialI cell)
	{
		if (this.cellContents.ContainsKey(cell) && !this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x060059C5 RID: 22981 RVA: 0x002094B0 File Offset: 0x002076B0
	public List<ClusterGridEntity> GetNotVisibleEntitiesAtAdjacentCell(AxialI cell)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell)).ToList<ClusterGridEntity>();
	}

	// Token: 0x060059C6 RID: 22982 RVA: 0x002094CF File Offset: 0x002076CF
	public AxialI GetRandomVisibleAdjacentCellLocation(AxialI cell)
	{
		return this.GetRandomVisibleAdjacentCellLocation(cell, AxialI.INVALID);
	}

	// Token: 0x060059C7 RID: 22983 RVA: 0x002094E0 File Offset: 0x002076E0
	public AxialI GetRandomVisibleAdjacentCellLocation(AxialI cell, AxialI forbidden)
	{
		List<AxialI> list = AxialUtil.GetRing(cell, 1).FindAll((AxialI e) => this.IsCellVisible(e) && (forbidden == AxialI.INVALID || forbidden != e));
		if (list != null)
		{
			return list.GetRandom<AxialI>();
		}
		return AxialI.INVALID;
	}

	// Token: 0x060059C8 RID: 22984 RVA: 0x0020952C File Offset: 0x0020772C
	public List<ClusterGridEntity> GetNotVisibleEntitiesOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x060059C9 RID: 22985 RVA: 0x00209574 File Offset: 0x00207774
	public bool GetVisibleUnidentifiedMeteorShowerWithinRadius(AxialI center, int radius, out ClusterMapMeteorShower.Instance result)
	{
		for (int i = 0; i <= radius; i++)
		{
			foreach (AxialI axialI in AxialUtil.GetRing(center, i))
			{
				if (this.IsValidCell(axialI) && this.GetFOWManager().IsLocationRevealed(axialI))
				{
					foreach (ClusterGridEntity cmp in this.GetEntitiesOfLayerAtCell(axialI, EntityLayer.Meteor))
					{
						ClusterMapMeteorShower.Instance smi = cmp.GetSMI<ClusterMapMeteorShower.Instance>();
						if (smi != null && !smi.HasBeenIdentified)
						{
							result = smi;
							return true;
						}
					}
				}
			}
		}
		result = null;
		return false;
	}

	// Token: 0x060059CA RID: 22986 RVA: 0x0020964C File Offset: 0x0020784C
	public ClusterGridEntity GetAsteroidAtCell(AxialI cell)
	{
		if (!this.cellContents.ContainsKey(cell))
		{
			return null;
		}
		return (from e in this.cellContents[cell]
		where e.Layer == EntityLayer.Asteroid
		select e).FirstOrDefault<ClusterGridEntity>();
	}

	// Token: 0x060059CB RID: 22987 RVA: 0x0020969E File Offset: 0x0020789E
	public bool HasVisibleAsteroidAtCell(AxialI cell)
	{
		return this.GetVisibleEntityOfLayerAtCell(cell, EntityLayer.Asteroid) != null;
	}

	// Token: 0x060059CC RID: 22988 RVA: 0x002096AE File Offset: 0x002078AE
	public void RegisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Add(entity);
		entity.Subscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x060059CD RID: 22989 RVA: 0x002096D9 File Offset: 0x002078D9
	public void UnregisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Remove(entity);
		entity.Unsubscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x060059CE RID: 22990 RVA: 0x00209704 File Offset: 0x00207904
	public void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.oldLocation), string.Format("ChangeEntityCell move order FROM invalid location: {0} {1}", clusterLocationChangedEvent.oldLocation, clusterLocationChangedEvent.entity));
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.newLocation), string.Format("ChangeEntityCell move order TO invalid location: {0} {1}", clusterLocationChangedEvent.newLocation, clusterLocationChangedEvent.entity));
		this.cellContents[clusterLocationChangedEvent.oldLocation].Remove(clusterLocationChangedEvent.entity);
		this.cellContents[clusterLocationChangedEvent.newLocation].Add(clusterLocationChangedEvent.entity);
	}

	// Token: 0x060059CF RID: 22991 RVA: 0x002097A9 File Offset: 0x002079A9
	private AxialI GetNeighbor(AxialI cell, AxialI direction)
	{
		return cell + direction;
	}

	// Token: 0x060059D0 RID: 22992 RVA: 0x002097B4 File Offset: 0x002079B4
	public int GetCellRing(AxialI cell)
	{
		Vector3I vector3I = cell.ToCube();
		Vector3I vector3I2 = new Vector3I(vector3I.x, vector3I.y, vector3I.z);
		Vector3I vector3I3 = new Vector3I(0, 0, 0);
		return (int)((float)((Mathf.Abs(vector3I2.x - vector3I3.x) + Mathf.Abs(vector3I2.y - vector3I3.y) + Mathf.Abs(vector3I2.z - vector3I3.z)) / 2));
	}

	// Token: 0x060059D1 RID: 22993 RVA: 0x00209828 File Offset: 0x00207A28
	private void CleanUpGrid()
	{
		this.cellContents.Clear();
	}

	// Token: 0x060059D2 RID: 22994 RVA: 0x00209838 File Offset: 0x00207A38
	private int GetHexDistance(AxialI a, AxialI b)
	{
		Vector3I vector3I = a.ToCube();
		Vector3I vector3I2 = b.ToCube();
		return Mathf.Max(new int[]
		{
			Mathf.Abs(vector3I.x - vector3I2.x),
			Mathf.Abs(vector3I.y - vector3I2.y),
			Mathf.Abs(vector3I.z - vector3I2.z)
		});
	}

	// Token: 0x060059D3 RID: 22995 RVA: 0x002098A0 File Offset: 0x00207AA0
	public List<ClusterGridEntity> GetEntitiesInRange(AxialI center, int range = 1)
	{
		List<ClusterGridEntity> list = new List<ClusterGridEntity>();
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in this.cellContents)
		{
			if (this.GetHexDistance(keyValuePair.Key, center) <= range)
			{
				list.AddRange(keyValuePair.Value);
			}
		}
		return list;
	}

	// Token: 0x060059D4 RID: 22996 RVA: 0x00209914 File Offset: 0x00207B14
	public List<ClusterGridEntity> GetEntitiesOnCell(AxialI cell)
	{
		return this.cellContents[cell];
	}

	// Token: 0x060059D5 RID: 22997 RVA: 0x00209922 File Offset: 0x00207B22
	public bool IsInRange(AxialI a, AxialI b, int range = 1)
	{
		return this.GetHexDistance(a, b) <= range;
	}

	// Token: 0x060059D6 RID: 22998 RVA: 0x00209934 File Offset: 0x00207B34
	private void GenerateGrid(int rings)
	{
		this.CleanUpGrid();
		this.numRings = rings;
		for (int i = -rings + 1; i < rings; i++)
		{
			for (int j = -rings + 1; j < rings; j++)
			{
				for (int k = -rings + 1; k < rings; k++)
				{
					if (i + j + k == 0)
					{
						AxialI key = new AxialI(i, j);
						this.cellContents.Add(key, new List<ClusterGridEntity>());
					}
				}
			}
		}
	}

	// Token: 0x060059D7 RID: 22999 RVA: 0x0020999C File Offset: 0x00207B9C
	public AxialI GetRandomCellAtEdgeOfUniverse()
	{
		int num = this.numRings - 1;
		List<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, num, num);
		return rings.ElementAt(UnityEngine.Random.Range(0, rings.Count));
	}

	// Token: 0x060059D8 RID: 23000 RVA: 0x002099D4 File Offset: 0x00207BD4
	public Vector3 GetPosition(ClusterGridEntity entity)
	{
		float r = (float)entity.Location.R;
		float q = (float)entity.Location.Q;
		List<ClusterGridEntity> list = this.cellContents[entity.Location];
		if (list.Count <= 1 || !entity.SpaceOutInSameHex())
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		int num = 0;
		int num2 = 0;
		foreach (ClusterGridEntity clusterGridEntity in list)
		{
			if (entity == clusterGridEntity)
			{
				num = num2;
			}
			if (clusterGridEntity.SpaceOutInSameHex())
			{
				num2++;
			}
		}
		if (list.Count > num2)
		{
			num2 += 5;
			num += 5;
		}
		else if (num2 > 0)
		{
			num2++;
			num++;
		}
		if (num2 == 0 || num2 == 1)
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		float num3 = Mathf.Min(Mathf.Pow((float)num2, 0.5f), 1f) * 0.5f;
		float num4 = Mathf.Pow((float)num / (float)num2, 0.5f);
		float num5 = 0.81f;
		float num6 = Mathf.Pow((float)num2, 0.5f) * num5;
		float f = 6.2831855f * num6 * num4;
		float x = Mathf.Cos(f) * num3 * num4;
		float y = Mathf.Sin(f) * num3 * num4;
		return AxialUtil.AxialToWorld(r, q) + new Vector3(x, y, 0f);
	}

	// Token: 0x060059D9 RID: 23001 RVA: 0x00209B58 File Offset: 0x00207D58
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector)
	{
		string text;
		return this.GetPath(start, end, destination_selector, out text, false);
	}

	// Token: 0x060059DA RID: 23002 RVA: 0x00209B74 File Offset: 0x00207D74
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector, out string fail_reason, bool dodgeHiddenAsteroids = false)
	{
		ClusterGrid.<>c__DisplayClass44_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.destination_selector = destination_selector;
		CS$<>8__locals1.start = start;
		CS$<>8__locals1.end = end;
		CS$<>8__locals1.dodgeHiddenAsteroids = dodgeHiddenAsteroids;
		fail_reason = null;
		if (!CS$<>8__locals1.destination_selector.canNavigateFogOfWar && !this.IsCellVisible(CS$<>8__locals1.end))
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR;
			return null;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = this.GetVisibleEntityOfLayerAtCell(CS$<>8__locals1.end, EntityLayer.Asteroid);
		if (visibleEntityOfLayerAtCell != null && CS$<>8__locals1.destination_selector.requireLaunchPadOnAsteroidDestination)
		{
			bool flag = false;
			using (IEnumerator enumerator = Components.LaunchPads.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((LaunchPad)enumerator.Current).GetMyWorldLocation() == visibleEntityOfLayerAtCell.Location)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD;
				return null;
			}
		}
		if (visibleEntityOfLayerAtCell == null && CS$<>8__locals1.destination_selector.requireAsteroidDestination)
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID;
			return null;
		}
		if (CS$<>8__locals1.destination_selector.requiredEntityLayer != EntityLayer.None && this.GetVisibleEntityOfLayerAtCell(CS$<>8__locals1.end, CS$<>8__locals1.destination_selector.requiredEntityLayer) == null)
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_METEOR_TARGET;
			return null;
		}
		CS$<>8__locals1.frontier = new HashSet<AxialI>();
		CS$<>8__locals1.visited = new HashSet<AxialI>();
		CS$<>8__locals1.buffer = new HashSet<AxialI>();
		CS$<>8__locals1.cameFrom = new Dictionary<AxialI, AxialI>();
		CS$<>8__locals1.frontier.Add(CS$<>8__locals1.start);
		while (!CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end) && CS$<>8__locals1.frontier.Count > 0)
		{
			this.<GetPath>g__ExpandFrontier|44_0(ref CS$<>8__locals1);
		}
		if (CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end))
		{
			List<AxialI> list = new List<AxialI>();
			AxialI axialI = CS$<>8__locals1.end;
			while (axialI != CS$<>8__locals1.start)
			{
				list.Add(axialI);
				axialI = CS$<>8__locals1.cameFrom[axialI];
			}
			list.Reverse();
			return list;
		}
		fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_PATH;
		return null;
	}

	// Token: 0x060059DB RID: 23003 RVA: 0x00209DA0 File Offset: 0x00207FA0
	public void GetLocationDescription(AxialI location, out Sprite sprite, out string label, out string sublabel)
	{
		EntityLayer entityLayer;
		this.GetLocationDescription(location, out sprite, out label, out sublabel, out entityLayer);
	}

	// Token: 0x060059DC RID: 23004 RVA: 0x00209DBC File Offset: 0x00207FBC
	public void GetLocationDescription(AxialI location, out Sprite sprite, out string label, out string sublabel, out EntityLayer locationEntity)
	{
		locationEntity = EntityLayer.None;
		List<ClusterGridEntity> visibleEntitiesAtCell = this.GetVisibleEntitiesAtCell(location);
		ClusterGridEntity clusterGridEntity = visibleEntitiesAtCell.Find((ClusterGridEntity x) => x.Layer == EntityLayer.Asteroid);
		ClusterGridEntity clusterGridEntity2 = visibleEntitiesAtCell.Find((ClusterGridEntity x) => x.Layer == EntityLayer.POI);
		ClusterGridEntity visibleEntityOfLayerAtAdjacentCell = this.GetVisibleEntityOfLayerAtAdjacentCell(location, EntityLayer.Asteroid);
		if (clusterGridEntity != null)
		{
			sprite = clusterGridEntity.GetUISprite();
			label = clusterGridEntity.Name;
			WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component.worldType);
			locationEntity = EntityLayer.Asteroid;
			return;
		}
		if (clusterGridEntity2 != null)
		{
			sprite = clusterGridEntity2.GetUISprite();
			label = clusterGridEntity2.Name;
			sublabel = clusterGridEntity2.Name;
			locationEntity = EntityLayer.POI;
			return;
		}
		if (visibleEntityOfLayerAtAdjacentCell != null)
		{
			sprite = visibleEntityOfLayerAtAdjacentCell.GetUISprite();
			label = UI.SPACEDESTINATIONS.ORBIT.NAME_FMT.Replace("{Name}", visibleEntityOfLayerAtAdjacentCell.Name);
			WorldContainer component2 = visibleEntityOfLayerAtAdjacentCell.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component2.worldType);
			locationEntity = EntityLayer.None;
			return;
		}
		if (this.IsCellVisible(location))
		{
			sprite = Assets.GetSprite("hex_empty");
			label = UI.SPACEDESTINATIONS.EMPTY_SPACE.NAME;
			sublabel = "";
			return;
		}
		sprite = Assets.GetSprite("unknown_far");
		label = UI.SPACEDESTINATIONS.FOG_OF_WAR_SPACE.NAME;
		sublabel = "";
	}

	// Token: 0x060059DD RID: 23005 RVA: 0x00209F24 File Offset: 0x00208124
	[CompilerGenerated]
	private void <GetPath>g__ExpandFrontier|44_0(ref ClusterGrid.<>c__DisplayClass44_0 A_1)
	{
		A_1.buffer.Clear();
		foreach (AxialI axialI in A_1.frontier)
		{
			foreach (AxialI direction in AxialI.DIRECTIONS)
			{
				AxialI neighbor = this.GetNeighbor(axialI, direction);
				if (!A_1.visited.Contains(neighbor) && this.IsValidCell(neighbor) && (this.IsCellVisible(neighbor) || A_1.destination_selector.canNavigateFogOfWar) && (!this.HasVisibleAsteroidAtCell(neighbor) || !(neighbor != A_1.start) || !(neighbor != A_1.end)) && (!A_1.dodgeHiddenAsteroids || !(ClusterGrid.Instance.GetAsteroidAtCell(neighbor) != null) || ClusterGrid.Instance.GetAsteroidAtCell(neighbor).IsVisibleInFOW == ClusterRevealLevel.Visible || !(neighbor != A_1.start) || !(neighbor != A_1.end)))
				{
					A_1.buffer.Add(neighbor);
					if (!A_1.cameFrom.ContainsKey(neighbor))
					{
						A_1.cameFrom.Add(neighbor, axialI);
					}
				}
			}
			A_1.visited.Add(axialI);
		}
		HashSet<AxialI> frontier = A_1.frontier;
		A_1.frontier = A_1.buffer;
		A_1.buffer = frontier;
	}

	// Token: 0x04003C11 RID: 15377
	public static ClusterGrid Instance;

	// Token: 0x04003C12 RID: 15378
	public const float NodeDistanceScale = 600f;

	// Token: 0x04003C13 RID: 15379
	private const float MAX_OFFSET_RADIUS = 0.5f;

	// Token: 0x04003C14 RID: 15380
	public int numRings;

	// Token: 0x04003C15 RID: 15381
	private ClusterFogOfWarManager.Instance m_fowManager;

	// Token: 0x04003C16 RID: 15382
	private Action<object> m_onClusterLocationChangedDelegate;

	// Token: 0x04003C17 RID: 15383
	public Dictionary<AxialI, List<ClusterGridEntity>> cellContents = new Dictionary<AxialI, List<ClusterGridEntity>>();
}
