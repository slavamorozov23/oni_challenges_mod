using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Delaunay.Geo;
using Klei;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using TUNING;
using UnityEngine;

// Token: 0x02000C25 RID: 3109
[SerializationConfig(MemberSerialization.OptIn)]
public class WorldContainer : KMonoBehaviour
{
	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06005D89 RID: 23945 RVA: 0x0021D2A5 File Offset: 0x0021B4A5
	// (set) Token: 0x06005D8A RID: 23946 RVA: 0x0021D2AD File Offset: 0x0021B4AD
	[Serialize]
	public WorldInventory worldInventory { get; private set; }

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06005D8B RID: 23947 RVA: 0x0021D2B6 File Offset: 0x0021B4B6
	// (set) Token: 0x06005D8C RID: 23948 RVA: 0x0021D2BE File Offset: 0x0021B4BE
	public Dictionary<Tag, float> materialNeeds { get; private set; }

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06005D8D RID: 23949 RVA: 0x0021D2C7 File Offset: 0x0021B4C7
	public bool IsModuleInterior
	{
		get
		{
			return this.isModuleInterior;
		}
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06005D8E RID: 23950 RVA: 0x0021D2CF File Offset: 0x0021B4CF
	public bool IsDiscovered
	{
		get
		{
			return this.isDiscovered || DebugHandler.RevealFogOfWar;
		}
	}

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06005D8F RID: 23951 RVA: 0x0021D2E0 File Offset: 0x0021B4E0
	public bool IsStartWorld
	{
		get
		{
			return this.isStartWorld;
		}
	}

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06005D90 RID: 23952 RVA: 0x0021D2E8 File Offset: 0x0021B4E8
	public bool IsDupeVisited
	{
		get
		{
			return this.isDupeVisited;
		}
	}

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06005D91 RID: 23953 RVA: 0x0021D2F0 File Offset: 0x0021B4F0
	public float DupeVisitedTimestamp
	{
		get
		{
			return this.dupeVisitedTimestamp;
		}
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06005D92 RID: 23954 RVA: 0x0021D2F8 File Offset: 0x0021B4F8
	public float DiscoveryTimestamp
	{
		get
		{
			return this.discoveryTimestamp;
		}
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06005D93 RID: 23955 RVA: 0x0021D300 File Offset: 0x0021B500
	public bool IsRoverVisted
	{
		get
		{
			return this.isRoverVisited;
		}
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06005D94 RID: 23956 RVA: 0x0021D308 File Offset: 0x0021B508
	public bool IsSurfaceRevealed
	{
		get
		{
			return this.isSurfaceRevealed;
		}
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06005D95 RID: 23957 RVA: 0x0021D310 File Offset: 0x0021B510
	public Dictionary<string, int> SunlightFixedTraits
	{
		get
		{
			return this.sunlightFixedTraits;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06005D96 RID: 23958 RVA: 0x0021D318 File Offset: 0x0021B518
	public Dictionary<string, int> NorthernLightsFixedTraits
	{
		get
		{
			return this.northernLightsFixedTraits;
		}
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06005D97 RID: 23959 RVA: 0x0021D320 File Offset: 0x0021B520
	public Dictionary<string, int> LargeImpactorFragmentsFixedTraits
	{
		get
		{
			return this.largeImpactorFragmentsFixedTraits;
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06005D98 RID: 23960 RVA: 0x0021D328 File Offset: 0x0021B528
	public Dictionary<string, int> CosmicRadiationFixedTraits
	{
		get
		{
			return this.cosmicRadiationFixedTraits;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06005D99 RID: 23961 RVA: 0x0021D330 File Offset: 0x0021B530
	public List<string> Biomes
	{
		get
		{
			return this.m_subworldNames;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06005D9A RID: 23962 RVA: 0x0021D338 File Offset: 0x0021B538
	public List<string> GeneratedBiomes
	{
		get
		{
			return this.m_generatedSubworlds;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06005D9B RID: 23963 RVA: 0x0021D340 File Offset: 0x0021B540
	public List<string> WorldTraitIds
	{
		get
		{
			return this.m_worldTraitIds;
		}
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06005D9C RID: 23964 RVA: 0x0021D348 File Offset: 0x0021B548
	public List<string> StoryTraitIds
	{
		get
		{
			return this.m_storyTraitIds;
		}
	}

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06005D9D RID: 23965 RVA: 0x0021D350 File Offset: 0x0021B550
	public AlertStateManager.Instance AlertManager
	{
		get
		{
			if (this.m_alertManager == null)
			{
				StateMachineController component = base.GetComponent<StateMachineController>();
				this.m_alertManager = component.GetSMI<AlertStateManager.Instance>();
			}
			global::Debug.Assert(this.m_alertManager != null, "AlertStateManager should never be null.");
			return this.m_alertManager;
		}
	}

	// Token: 0x06005D9E RID: 23966 RVA: 0x0021D391 File Offset: 0x0021B591
	public void AddTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		if (!this.yellowAlertTasks.Contains(prioritizable))
		{
			this.yellowAlertTasks.Add(prioritizable);
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x06005D9F RID: 23967 RVA: 0x0021D3B4 File Offset: 0x0021B5B4
	public void RemoveTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		for (int i = this.yellowAlertTasks.Count - 1; i >= 0; i--)
		{
			if (this.yellowAlertTasks[i] == prioritizable || this.yellowAlertTasks[i].Equals(null))
			{
				this.yellowAlertTasks.RemoveAt(i);
			}
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06005DA0 RID: 23968 RVA: 0x0021D413 File Offset: 0x0021B613
	// (set) Token: 0x06005DA1 RID: 23969 RVA: 0x0021D41B File Offset: 0x0021B61B
	public int ParentWorldId { get; private set; }

	// Token: 0x06005DA2 RID: 23970 RVA: 0x0021D424 File Offset: 0x0021B624
	public ICollection<int> GetChildWorldIds()
	{
		return this.m_childWorlds;
	}

	// Token: 0x06005DA3 RID: 23971 RVA: 0x0021D42C File Offset: 0x0021B62C
	private void OnWorldRemoved(object data)
	{
		int value = ((Boxed<int>)data).value;
		if (value != 255)
		{
			this.m_childWorlds.Remove(value);
		}
	}

	// Token: 0x06005DA4 RID: 23972 RVA: 0x0021D45C File Offset: 0x0021B65C
	private void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null)
		{
			return;
		}
		if (worldParentChangedEventArgs.world.ParentWorldId == this.id)
		{
			this.m_childWorlds.Add(worldParentChangedEventArgs.world.id);
		}
		if (worldParentChangedEventArgs.lastParentId == this.ParentWorldId)
		{
			this.m_childWorlds.Remove(worldParentChangedEventArgs.world.id);
		}
	}

	// Token: 0x06005DA5 RID: 23973 RVA: 0x0021D4C4 File Offset: 0x0021B6C4
	public Quadrant[] GetQuadrantOfCell(int cell, int depth = 1)
	{
		Vector2 vector = new Vector2((float)this.WorldSize.x * Grid.CellSizeInMeters, (float)this.worldSize.y * Grid.CellSizeInMeters);
		Vector2 vector2 = Grid.CellToPos2D(Grid.XYToCell(this.WorldOffset.x, this.WorldOffset.y));
		Vector2 vector3 = Grid.CellToPos2D(cell);
		Quadrant[] array = new Quadrant[depth];
		Vector2 vector4 = new Vector2(vector2.x, (float)this.worldOffset.y + vector.y);
		Vector2 vector5 = new Vector2(vector2.x + vector.x, (float)this.worldOffset.y);
		for (int i = 0; i < depth; i++)
		{
			float num = vector5.x - vector4.x;
			float num2 = vector4.y - vector5.y;
			float num3 = num * 0.5f;
			float num4 = num2 * 0.5f;
			if (vector3.x >= vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NE;
			}
			if (vector3.x >= vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SE;
			}
			if (vector3.x < vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SW;
			}
			if (vector3.x < vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NW;
			}
			switch (array[i])
			{
			case Quadrant.NE:
				vector4.x += num3;
				vector5.y += num4;
				break;
			case Quadrant.NW:
				vector5.x -= num3;
				vector5.y += num4;
				break;
			case Quadrant.SW:
				vector4.y -= num4;
				vector5.x -= num3;
				break;
			case Quadrant.SE:
				vector4.x += num3;
				vector4.y -= num4;
				break;
			}
		}
		return array;
	}

	// Token: 0x06005DA6 RID: 23974 RVA: 0x0021D6F1 File Offset: 0x0021B8F1
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ParentWorldId = this.id;
	}

	// Token: 0x06005DA7 RID: 23975 RVA: 0x0021D700 File Offset: 0x0021B900
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.worldInventory = base.GetComponent<WorldInventory>();
		this.materialNeeds = new Dictionary<Tag, float>();
		ClusterManager.Instance.RegisterWorldContainer(this);
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
	}

	// Token: 0x06005DA8 RID: 23976 RVA: 0x0021D770 File Offset: 0x0021B970
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.gameObject.AddOrGet<InfoDescription>().DescriptionLocString = this.worldDescription;
		this.RefreshHasTopPriorityChore();
		this.UpgradeFixedTraits();
		this.RefreshFixedTraits();
		if (DlcManager.IsPureVanilla())
		{
			this.isStartWorld = true;
			this.isDupeVisited = true;
		}
	}

	// Token: 0x06005DA9 RID: 23977 RVA: 0x0021D7C0 File Offset: 0x0021B9C0
	protected override void OnCleanUp()
	{
		SaveGame.Instance.materialSelectorSerializer.WipeWorldSelectionData(this.id);
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		base.OnCleanUp();
	}

	// Token: 0x06005DAA RID: 23978 RVA: 0x0021D820 File Offset: 0x0021BA20
	private void UpgradeFixedTraits()
	{
		if (this.sunlightFixedTrait == null || this.sunlightFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					160000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				},
				{
					0,
					FIXEDTRAITS.SUNLIGHT.NAME.NONE
				},
				{
					10000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW
				},
				{
					20000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW
				},
				{
					30000,
					FIXEDTRAITS.SUNLIGHT.NAME.LOW
				},
				{
					35000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW
				},
				{
					40000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED
				},
				{
					50000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH
				},
				{
					60000,
					FIXEDTRAITS.SUNLIGHT.NAME.HIGH
				},
				{
					80000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH
				},
				{
					120000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.sunlight, out this.sunlightFixedTrait);
		}
		if (this.cosmicRadiationFixedTrait == null || this.cosmicRadiationFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					0,
					FIXEDTRAITS.COSMICRADIATION.NAME.NONE
				},
				{
					6,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW
				},
				{
					12,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW
				},
				{
					18,
					FIXEDTRAITS.COSMICRADIATION.NAME.LOW
				},
				{
					21,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW
				},
				{
					25,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED
				},
				{
					31,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH
				},
				{
					37,
					FIXEDTRAITS.COSMICRADIATION.NAME.HIGH
				},
				{
					50,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH
				},
				{
					75,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.cosmicRadiation, out this.cosmicRadiationFixedTrait);
		}
	}

	// Token: 0x06005DAB RID: 23979 RVA: 0x0021D9C1 File Offset: 0x0021BBC1
	private void RefreshFixedTraits()
	{
		this.sunlight = this.GetSunlightValueFromFixedTrait();
		this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
		this.northernlights = this.GetNorthernlightValueFromFixedTrait();
		this.largeImpactorFragments = this.GetLargeImpactorFragmentsValueFromFixedTrait();
	}

	// Token: 0x06005DAC RID: 23980 RVA: 0x0021D9F3 File Offset: 0x0021BBF3
	private void RefreshHasTopPriorityChore()
	{
		if (this.AlertManager != null)
		{
			this.AlertManager.SetHasTopPriorityChore(this.yellowAlertTasks.Count > 0);
		}
	}

	// Token: 0x06005DAD RID: 23981 RVA: 0x0021DA16 File Offset: 0x0021BC16
	public List<string> GetSeasonIds()
	{
		return this.m_seasonIds;
	}

	// Token: 0x06005DAE RID: 23982 RVA: 0x0021DA1E File Offset: 0x0021BC1E
	public bool IsRedAlert()
	{
		return this.m_alertManager.IsRedAlert();
	}

	// Token: 0x06005DAF RID: 23983 RVA: 0x0021DA2B File Offset: 0x0021BC2B
	public bool IsYellowAlert()
	{
		return this.m_alertManager.IsYellowAlert();
	}

	// Token: 0x06005DB0 RID: 23984 RVA: 0x0021DA38 File Offset: 0x0021BC38
	public string GetRandomName()
	{
		if (!this.overrideName.IsNullOrWhiteSpace())
		{
			return Strings.Get(this.overrideName);
		}
		return GameUtil.GenerateRandomWorldName(this.nameTables);
	}

	// Token: 0x06005DB1 RID: 23985 RVA: 0x0021DA63 File Offset: 0x0021BC63
	public void SetID(int id)
	{
		this.id = id;
		this.ParentWorldId = id;
	}

	// Token: 0x06005DB2 RID: 23986 RVA: 0x0021DA74 File Offset: 0x0021BC74
	public void SetParentIdx(int parentIdx)
	{
		this.parentChangeArgs.lastParentId = this.ParentWorldId;
		this.parentChangeArgs.world = this;
		this.ParentWorldId = parentIdx;
		Game.Instance.Trigger(880851192, this.parentChangeArgs);
		this.parentChangeArgs.lastParentId = 255;
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06005DB3 RID: 23987 RVA: 0x0021DACA File Offset: 0x0021BCCA
	public Vector2 minimumBounds
	{
		get
		{
			return new Vector2((float)this.worldOffset.x, (float)this.worldOffset.y);
		}
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06005DB4 RID: 23988 RVA: 0x0021DAEC File Offset: 0x0021BCEC
	public Vector2 maximumBounds
	{
		get
		{
			return new Vector2((float)(this.worldOffset.x + (this.worldSize.x - 1)), (float)(this.worldOffset.y + (this.worldSize.y - this.hiddenYOffset - 1)));
		}
	}

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06005DB5 RID: 23989 RVA: 0x0021DB39 File Offset: 0x0021BD39
	public Vector2I WorldSize
	{
		get
		{
			return this.worldSize;
		}
	}

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06005DB6 RID: 23990 RVA: 0x0021DB41 File Offset: 0x0021BD41
	public Vector2I WorldOffset
	{
		get
		{
			return this.worldOffset;
		}
	}

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06005DB7 RID: 23991 RVA: 0x0021DB49 File Offset: 0x0021BD49
	public int HiddenYOffset
	{
		get
		{
			return this.hiddenYOffset;
		}
	}

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06005DB8 RID: 23992 RVA: 0x0021DB51 File Offset: 0x0021BD51
	public bool FullyEnclosedBorder
	{
		get
		{
			return this.fullyEnclosedBorder;
		}
	}

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06005DB9 RID: 23993 RVA: 0x0021DB59 File Offset: 0x0021BD59
	public int Height
	{
		get
		{
			return this.worldSize.y;
		}
	}

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06005DBA RID: 23994 RVA: 0x0021DB66 File Offset: 0x0021BD66
	public int Width
	{
		get
		{
			return this.worldSize.x;
		}
	}

	// Token: 0x06005DBB RID: 23995 RVA: 0x0021DB73 File Offset: 0x0021BD73
	public void SetDiscovered(bool reveal_surface = false)
	{
		if (!this.isDiscovered)
		{
			this.discoveryTimestamp = GameUtil.GetCurrentTimeInCycles();
		}
		this.isDiscovered = true;
		if (reveal_surface)
		{
			this.LookAtSurface();
		}
		Game.Instance.Trigger(-521212405, this);
	}

	// Token: 0x06005DBC RID: 23996 RVA: 0x0021DBA8 File Offset: 0x0021BDA8
	public void SetDupeVisited()
	{
		if (!this.isDupeVisited)
		{
			this.dupeVisitedTimestamp = GameUtil.GetCurrentTimeInCycles();
			this.isDupeVisited = true;
			Game.Instance.Trigger(-434755240, this);
		}
	}

	// Token: 0x06005DBD RID: 23997 RVA: 0x0021DBD4 File Offset: 0x0021BDD4
	public void SetRoverLanded()
	{
		this.isRoverVisited = true;
	}

	// Token: 0x06005DBE RID: 23998 RVA: 0x0021DBDD File Offset: 0x0021BDDD
	public void SetRocketInteriorWorldDetails(int world_id, Vector2I size, Vector2I offset)
	{
		this.SetID(world_id);
		this.fullyEnclosedBorder = true;
		this.worldOffset = offset;
		this.worldSize = size;
		this.isDiscovered = true;
		this.isModuleInterior = true;
		this.m_seasonIds = new List<string>();
	}

	// Token: 0x06005DBF RID: 23999 RVA: 0x0021DC14 File Offset: 0x0021BE14
	private static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
	{
		if (first == second)
		{
			return 0;
		}
		Vector2 vector = first - origin;
		Vector2 vector2 = second - origin;
		float num = Mathf.Atan2(vector.x, vector.y);
		float num2 = Mathf.Atan2(vector2.x, vector2.y);
		if (num < num2)
		{
			return 1;
		}
		if (num > num2)
		{
			return -1;
		}
		if (vector.sqrMagnitude >= vector2.sqrMagnitude)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06005DC0 RID: 24000 RVA: 0x0021DC80 File Offset: 0x0021BE80
	public void PlaceInteriorTemplate(string template_name, System.Action callback)
	{
		TemplateContainer template = TemplateCache.GetTemplate(template_name);
		Vector2 pos = new Vector2((float)(this.worldSize.x / 2 + this.worldOffset.x), (float)(this.worldSize.y / 2 + this.worldOffset.y));
		TemplateLoader.Stamp(template, pos, callback);
		float val = template.info.size.X / 2f;
		float val2 = template.info.size.Y / 2f;
		float num = Math.Max(val, val2);
		GridVisibility.Reveal((int)pos.x, (int)pos.y, (int)num + 3 + 5, num + 3f);
		WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
		this.overworldCell = new WorldDetailSave.OverworldCell();
		List<Vector2> list = new List<Vector2>(template.cells.Count);
		foreach (Prefab prefab in template.buildings)
		{
			if (prefab.id == "RocketWallTile")
			{
				Vector2 vector = new Vector2((float)prefab.location_x + pos.x, (float)prefab.location_y + pos.y);
				if (vector.x > pos.x)
				{
					vector.x += 0.5f;
				}
				if (vector.y > pos.y)
				{
					vector.y += 0.5f;
				}
				list.Add(vector);
			}
		}
		list.Sort((Vector2 v1, Vector2 v2) => WorldContainer.IsClockwise(v1, v2, pos));
		Polygon polygon = new Polygon(list);
		this.overworldCell.poly = polygon;
		this.overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
		this.overworldCell.tags = new TagSet
		{
			WorldGenTags.RocketInterior
		};
		clusterDetailSave.overworldCells.Add(this.overworldCell);
		for (int i = 0; i < this.worldSize.y; i++)
		{
			for (int j = 0; j < this.worldSize.x; j++)
			{
				Vector2I vector2I = new Vector2I(this.worldOffset.x + j, this.worldOffset.y + i);
				int num2 = Grid.XYToCell(vector2I.x, vector2I.y);
				if (polygon.Contains(new Vector2((float)vector2I.x, (float)vector2I.y)))
				{
					SimMessages.ModifyCellWorldZone(num2, 14);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.RocketInterior;
				}
				else
				{
					SimMessages.ModifyCellWorldZone(num2, byte.MaxValue);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.Space;
				}
			}
		}
	}

	// Token: 0x06005DC1 RID: 24001 RVA: 0x0021DF8C File Offset: 0x0021C18C
	private int GetDefaultValueForFixedTraitCategory(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.largeImpactorFragmentsFixedTraits)
		{
			return FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.DEFAULT_VALUE;
		}
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}
		return 0;
	}

	// Token: 0x06005DC2 RID: 24002 RVA: 0x0021DFCB File Offset: 0x0021C1CB
	private string GetDefaultFixedTraitFor(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.largeImpactorFragmentsFixedTraits)
		{
			return FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NAME.DEFAULT;
		}
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.NAME.DEFAULT;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT;
		}
		return null;
	}

	// Token: 0x06005DC3 RID: 24003 RVA: 0x0021E00C File Offset: 0x0021C20C
	private string GetFixedTraitsFor(Dictionary<string, int> traitCategory, WorldGen world)
	{
		foreach (string text in world.Settings.world.fixedTraits)
		{
			if (traitCategory.ContainsKey(text))
			{
				return text;
			}
		}
		return this.GetDefaultFixedTraitFor(traitCategory);
	}

	// Token: 0x06005DC4 RID: 24004 RVA: 0x0021E078 File Offset: 0x0021C278
	private int GetFixedTraitValueForTrait(Dictionary<string, int> traitCategory, ref string trait)
	{
		if (trait == null)
		{
			trait = this.GetDefaultFixedTraitFor(traitCategory);
		}
		if (traitCategory.ContainsKey(trait))
		{
			return traitCategory[trait];
		}
		return this.GetDefaultValueForFixedTraitCategory(traitCategory);
	}

	// Token: 0x06005DC5 RID: 24005 RVA: 0x0021E0A1 File Offset: 0x0021C2A1
	private string GetLargeImpactorFragmentsFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.LargeImpactorFragmentsFixedTraits, world);
	}

	// Token: 0x06005DC6 RID: 24006 RVA: 0x0021E0B0 File Offset: 0x0021C2B0
	private string GetNorthernlightFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.northernLightsFixedTraits, world);
	}

	// Token: 0x06005DC7 RID: 24007 RVA: 0x0021E0BF File Offset: 0x0021C2BF
	private string GetSunlightFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.sunlightFixedTraits, world);
	}

	// Token: 0x06005DC8 RID: 24008 RVA: 0x0021E0CE File Offset: 0x0021C2CE
	private string GetCosmicRadiationFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.cosmicRadiationFixedTraits, world);
	}

	// Token: 0x06005DC9 RID: 24009 RVA: 0x0021E0DD File Offset: 0x0021C2DD
	private int GetLargeImpactorFragmentsValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.largeImpactorFragmentsFixedTraits, ref this.largeImpactorFragmentsFixedTrait);
	}

	// Token: 0x06005DCA RID: 24010 RVA: 0x0021E0F1 File Offset: 0x0021C2F1
	private int GetNorthernlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.northernLightsFixedTraits, ref this.northernLightFixedTrait);
	}

	// Token: 0x06005DCB RID: 24011 RVA: 0x0021E105 File Offset: 0x0021C305
	private int GetSunlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.sunlightFixedTraits, ref this.sunlightFixedTrait);
	}

	// Token: 0x06005DCC RID: 24012 RVA: 0x0021E119 File Offset: 0x0021C319
	private int GetCosmicRadiationValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.cosmicRadiationFixedTraits, ref this.cosmicRadiationFixedTrait);
	}

	// Token: 0x06005DCD RID: 24013 RVA: 0x0021E130 File Offset: 0x0021C330
	public void SetWorldDetails(WorldGen world)
	{
		if (world != null)
		{
			this.fullyEnclosedBorder = (world.Settings.GetBoolSetting("DrawWorldBorder") && world.Settings.GetBoolSetting("DrawWorldBorderOverVacuum"));
			this.worldOffset = world.GetPosition();
			this.worldSize = world.GetSize();
			this.hiddenYOffset = world.HiddenYOffset;
			this.isDiscovered = world.isStartingWorld;
			this.isStartWorld = world.isStartingWorld;
			this.worldName = world.Settings.world.filePath;
			this.nameTables = world.Settings.world.nameTables;
			this.worldTags = ((world.Settings.world.worldTags != null) ? world.Settings.world.worldTags.ToArray().ToTagArray() : new Tag[0]);
			this.worldDescription = world.Settings.world.description;
			this.worldType = world.Settings.world.name;
			this.isModuleInterior = world.Settings.world.moduleInterior;
			this.m_seasonIds = new List<string>(world.Settings.world.seasons);
			this.m_generatedSubworlds = world.Settings.world.generatedSubworlds;
			this.largeImpactorFragmentsFixedTrait = this.GetLargeImpactorFragmentsFixedTraits(world);
			this.northernLightFixedTrait = this.GetNorthernlightFixedTraits(world);
			this.sunlightFixedTrait = this.GetSunlightFromFixedTraits(world);
			this.cosmicRadiationFixedTrait = this.GetCosmicRadiationFromFixedTraits(world);
			this.sunlight = this.GetSunlightValueFromFixedTrait();
			this.northernlights = this.GetNorthernlightValueFromFixedTrait();
			this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
			this.currentCosmicIntensity = (float)this.cosmicRadiation;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string text in world.Settings.world.generatedSubworlds)
			{
				text = text.Substring(0, text.LastIndexOf('/'));
				text = text.Substring(text.LastIndexOf('/') + 1, text.Length - (text.LastIndexOf('/') + 1));
				hashSet.Add(text);
			}
			this.m_subworldNames = hashSet.ToList<string>();
			this.m_worldTraitIds = new List<string>();
			this.m_worldTraitIds.AddRange(world.Settings.GetWorldTraitIDs());
			this.m_storyTraitIds = new List<string>();
			this.m_storyTraitIds.AddRange(world.Settings.GetStoryTraitIDs());
			return;
		}
		this.fullyEnclosedBorder = false;
		this.worldOffset = Vector2I.zero;
		this.worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
		this.isDiscovered = true;
		this.isStartWorld = true;
		this.isDupeVisited = true;
		this.m_seasonIds = new List<string>
		{
			Db.Get().GameplaySeasons.MeteorShowers.Id
		};
	}

	// Token: 0x06005DCE RID: 24014 RVA: 0x0021E424 File Offset: 0x0021C624
	public bool ContainsPoint(Vector2 point)
	{
		return point.x >= (float)this.worldOffset.x && point.y >= (float)this.worldOffset.y && point.x < (float)(this.worldOffset.x + this.worldSize.x) && point.y < (float)(this.worldOffset.y + this.worldSize.y);
	}

	// Token: 0x06005DCF RID: 24015 RVA: 0x0021E49C File Offset: 0x0021C69C
	public void LookAtSurface()
	{
		if (!this.IsDupeVisited)
		{
			this.RevealSurface();
		}
		Vector3? vector = this.SetSurfaceCameraPos();
		if (ClusterManager.Instance.activeWorldId == this.id && vector != null)
		{
			CameraController.Instance.SnapTo(vector.Value);
		}
	}

	// Token: 0x06005DD0 RID: 24016 RVA: 0x0021E4EC File Offset: 0x0021C6EC
	public void RevealSurface()
	{
		if (this.isSurfaceRevealed)
		{
			return;
		}
		this.isSurfaceRevealed = true;
		for (int i = 0; i < this.worldSize.x; i++)
		{
			for (int j = this.worldSize.y - 1; j >= 0; j--)
			{
				int cell = Grid.XYToCell(i + this.worldOffset.x, j + this.worldOffset.y);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || Grid.IsLiquid(cell))
				{
					break;
				}
				GridVisibility.Reveal(i + this.worldOffset.X, j + this.worldOffset.y, 7, 1f);
			}
		}
	}

	// Token: 0x06005DD1 RID: 24017 RVA: 0x0021E597 File Offset: 0x0021C797
	public void RevealHiddenY()
	{
		this.hiddenYOffset = 0;
	}

	// Token: 0x06005DD2 RID: 24018 RVA: 0x0021E5A0 File Offset: 0x0021C7A0
	private Vector3? SetSurfaceCameraPos()
	{
		if (SaveGame.Instance != null)
		{
			int num = (int)this.maximumBounds.y;
			for (int i = 0; i < this.worldSize.X; i++)
			{
				for (int j = this.worldSize.y - 1; j >= 0; j--)
				{
					int num2 = j + this.worldOffset.y;
					int num3 = Grid.XYToCell(i + this.worldOffset.x, num2);
					if (Grid.IsValidCell(num3) && (Grid.Solid[num3] || Grid.IsLiquid(num3)))
					{
						num = Math.Min(num, num2);
						break;
					}
				}
			}
			int num4 = (num + this.worldOffset.y + this.worldSize.y) / 2;
			Vector3 vector = new Vector3((float)(this.WorldOffset.x + this.Width / 2), (float)num4, 0f);
			SaveGame.Instance.GetComponent<UserNavigation>().SetWorldCameraStartPosition(this.id, vector);
			return new Vector3?(vector);
		}
		return null;
	}

	// Token: 0x06005DD3 RID: 24019 RVA: 0x0021E6B4 File Offset: 0x0021C8B4
	public void EjectAllDupes(Vector3 spawn_pos)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			minionIdentity.transform.SetLocalPosition(spawn_pos);
		}
	}

	// Token: 0x06005DD4 RID: 24020 RVA: 0x0021E718 File Offset: 0x0021C918
	public void SpacePodAllDupes(AxialI sourceLocation, SimHashes podElement)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			if (!minionIdentity.HasTag(GameTags.Dead))
			{
				Vector3 position = new Vector3(-1f, -1f, 0f);
				GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("EscapePod"), position);
				gameObject.GetComponent<PrimaryElement>().SetElement(podElement, true);
				gameObject.SetActive(true);
				gameObject.GetComponent<MinionStorage>().SerializeMinion(minionIdentity.gameObject);
				TravellingCargoLander.StatesInstance smi = gameObject.GetSMI<TravellingCargoLander.StatesInstance>();
				smi.StartSM();
				smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
			}
		}
	}

	// Token: 0x06005DD5 RID: 24021 RVA: 0x0021E7F0 File Offset: 0x0021C9F0
	public void DestroyWorldBuildings(out HashSet<int> noRefundTiles)
	{
		this.TransferBuildingMaterials(out noRefundTiles);
		foreach (ClustercraftInteriorDoor cmp in Components.ClusterCraftInteriorDoors.GetWorldItems(this.id, false))
		{
			cmp.DeleteObject();
		}
		this.ClearWorldZones();
	}

	// Token: 0x06005DD6 RID: 24022 RVA: 0x0021E858 File Offset: 0x0021CA58
	public void TransferResourcesToParentWorld(Vector3 spawn_pos, HashSet<int> noRefundTiles)
	{
		this.TransferPickupables(spawn_pos);
		this.TransferLiquidsSolidsAndGases(spawn_pos, noRefundTiles);
	}

	// Token: 0x06005DD7 RID: 24023 RVA: 0x0021E86C File Offset: 0x0021CA6C
	public void TransferResourcesToDebris(AxialI sourceLocation, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		List<Storage> list = new List<Storage>();
		this.TransferPickupablesToDebris(ref list, debrisContainerElement);
		this.TransferLiquidsSolidsAndGasesToDebris(ref list, noRefundTiles, debrisContainerElement);
		foreach (Storage cmp in list)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
		}
	}

	// Token: 0x06005DD8 RID: 24024 RVA: 0x0021E8E8 File Offset: 0x0021CAE8
	private void TransferBuildingMaterials(out HashSet<int> noRefundTiles)
	{
		HashSet<int> retTemplateFoundationCells = new HashSet<int>();
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.completeBuildings, pooledList);
		Action<int> <>9__0;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			BuildingComplete buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
			if (buildingComplete != null)
			{
				Deconstructable component = buildingComplete.GetComponent<Deconstructable>();
				if (component != null && !buildingComplete.HasTag(GameTags.NoRocketRefund))
				{
					PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
					float temperature = component2.Temperature;
					byte diseaseIdx = component2.DiseaseIdx;
					int diseaseCount = component2.DiseaseCount;
					int num = 0;
					while (num < component.constructionElements.Length && buildingComplete.Def.Mass.Length > num)
					{
						Element element = ElementLoader.GetElement(component.constructionElements[num]);
						if (element != null)
						{
							element.substance.SpawnResource(buildingComplete.transform.GetPosition(), buildingComplete.Def.Mass[num], temperature, diseaseIdx, diseaseCount, false, false, false);
						}
						else
						{
							GameObject prefab = Assets.GetPrefab(component.constructionElements[num]);
							int num2 = 0;
							while ((float)num2 < buildingComplete.Def.Mass[num])
							{
								GameUtil.KInstantiate(prefab, buildingComplete.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
								num2++;
							}
						}
						num++;
					}
				}
				SimCellOccupier component3 = buildingComplete.GetComponent<SimCellOccupier>();
				if (component3 != null && component3.doReplaceElement)
				{
					Building building = buildingComplete;
					Action<int> callback;
					if ((callback = <>9__0) == null)
					{
						callback = (<>9__0 = delegate(int cell)
						{
							retTemplateFoundationCells.Add(cell);
						});
					}
					building.RunOnArea(callback);
				}
				Storage component4 = buildingComplete.GetComponent<Storage>();
				if (component4 != null)
				{
					component4.DropAll(false, false, default(Vector3), true, null);
				}
				PlantablePlot component5 = buildingComplete.GetComponent<PlantablePlot>();
				if (component5 != null)
				{
					SeedProducer seedProducer = (component5.Occupant != null) ? component5.Occupant.GetComponent<SeedProducer>() : null;
					if (seedProducer != null)
					{
						seedProducer.DropSeed(null);
					}
				}
				buildingComplete.DeleteObject();
			}
		}
		pooledList.Clear();
		noRefundTiles = retTemplateFoundationCells;
	}

	// Token: 0x06005DD9 RID: 24025 RVA: 0x0021EB78 File Offset: 0x0021CD78
	private void TransferPickupables(Vector3 pos)
	{
		int cell = Grid.PosToCell(pos);
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					pickupable.gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06005DDA RID: 24026 RVA: 0x0021EC44 File Offset: 0x0021CE44
	private void TransferLiquidsSolidsAndGases(Vector3 pos, HashSet<int> noRefundTiles)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						element.substance.SpawnResource(pos, Grid.Mass[num3], Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, false, false);
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06005DDB RID: 24027 RVA: 0x0021ECFC File Offset: 0x0021CEFC
	private void TransferPickupablesToDebris(ref List<Storage> debrisObjects, SimHashes debrisContainerElement)
	{
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					if (pickupable.KPrefabID.HasTag(GameTags.BaseMinion))
					{
						global::Util.KDestroyGameObject(pickupable.gameObject);
					}
					else
					{
						pickupable.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(pickupable.PrimaryElement.Units * 0.5f));
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && pickupable.PrimaryElement.Mass > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (pickupable.PrimaryElement.Mass > storage.RemainingCapacity())
						{
							int num = Mathf.Max(1, Mathf.RoundToInt(storage.RemainingCapacity() / pickupable.PrimaryElement.MassPerUnit));
							Pickupable pickupable2 = pickupable.Take((float)num);
							storage.Store(pickupable2.gameObject, false, false, true, false);
							storage = CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement);
							debrisObjects.Add(storage);
						}
						if (pickupable.PrimaryElement.Mass > 0f)
						{
							storage.Store(pickupable.gameObject, false, false, true, false);
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06005DDC RID: 24028 RVA: 0x0021EEF0 File Offset: 0x0021D0F0
	private void TransferLiquidsSolidsAndGasesToDebris(ref List<Storage> debrisObjects, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						float num4 = Grid.Mass[num3];
						num4 *= 0.5f;
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && num4 > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (num4 > 0f)
						{
							float num5 = Mathf.Min(num4, storage.RemainingCapacity());
							num4 -= num5;
							storage.AddOre(element.id, num5, Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, true);
							if (num4 > 0f)
							{
								storage = CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement);
								debrisObjects.Add(storage);
							}
						}
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06005DDD RID: 24029 RVA: 0x0021F058 File Offset: 0x0021D258
	public void CancelChores()
	{
		for (int i = 0; i < 45; i++)
		{
			int num = (int)this.minimumBounds.x;
			while ((float)num <= this.maximumBounds.x)
			{
				int num2 = (int)this.minimumBounds.y;
				while ((float)num2 <= this.maximumBounds.y)
				{
					int cell = Grid.XYToCell(num, num2);
					GameObject gameObject = Grid.Objects[cell, i];
					if (gameObject != null)
					{
						gameObject.Trigger(2127324410, BoxedBools.True);
					}
					num2++;
				}
				num++;
			}
		}
		List<Chore> list;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(this.id, out list);
		int num3 = 0;
		while (list != null && num3 < list.Count)
		{
			Chore chore = list[num3];
			if (chore != null && chore.target != null && !chore.isNull)
			{
				chore.Cancel("World destroyed");
			}
			num3++;
		}
		List<FetchChore> list2;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(this.id, out list2);
		int num4 = 0;
		while (list2 != null && num4 < list2.Count)
		{
			FetchChore fetchChore = list2[num4];
			if (fetchChore != null && fetchChore.target != null && !fetchChore.isNull)
			{
				fetchChore.Cancel("World destroyed");
			}
			num4++;
		}
	}

	// Token: 0x06005DDE RID: 24030 RVA: 0x0021F1AC File Offset: 0x0021D3AC
	public void ClearWorldZones()
	{
		if (this.overworldCell != null)
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			int num = -1;
			for (int i = 0; i < SaveLoader.Instance.clusterDetailSave.overworldCells.Count; i++)
			{
				WorldDetailSave.OverworldCell overworldCell = SaveLoader.Instance.clusterDetailSave.overworldCells[i];
				if (overworldCell.zoneType == this.overworldCell.zoneType && overworldCell.tags != null && this.overworldCell.tags != null && overworldCell.tags.ContainsAll(this.overworldCell.tags) && overworldCell.poly.bounds == this.overworldCell.poly.bounds)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				clusterDetailSave.overworldCells.RemoveAt(num);
			}
		}
		int num2 = (int)this.minimumBounds.y;
		while ((float)num2 <= this.maximumBounds.y)
		{
			int num3 = (int)this.minimumBounds.x;
			while ((float)num3 <= this.maximumBounds.x)
			{
				SimMessages.ModifyCellWorldZone(Grid.XYToCell(num3, num2), byte.MaxValue);
				num3++;
			}
			num2++;
		}
	}

	// Token: 0x06005DDF RID: 24031 RVA: 0x0021F2E4 File Offset: 0x0021D4E4
	public int GetSafeCell()
	{
		if (this.IsModuleInterior)
		{
			using (List<RocketControlStation>.Enumerator enumerator = Components.RocketControlStations.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketControlStation rocketControlStation = enumerator.Current;
					if (rocketControlStation.GetMyWorldId() == this.id)
					{
						return Grid.PosToCell(rocketControlStation);
					}
				}
				goto IL_A2;
			}
		}
		foreach (Telepad telepad in Components.Telepads.Items)
		{
			if (telepad.GetMyWorldId() == this.id)
			{
				return Grid.PosToCell(telepad);
			}
		}
		IL_A2:
		return Grid.XYToCell(this.worldOffset.x + this.worldSize.x / 2, this.worldOffset.y + this.worldSize.y / 2);
	}

	// Token: 0x06005DE0 RID: 24032 RVA: 0x0021F3E8 File Offset: 0x0021D5E8
	public string GetStatus()
	{
		return ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultStatus(this.id);
	}

	// Token: 0x04003E47 RID: 15943
	[Serialize]
	public int id = -1;

	// Token: 0x04003E48 RID: 15944
	[Serialize]
	public Tag prefabTag;

	// Token: 0x04003E4B RID: 15947
	[Serialize]
	private Vector2I worldOffset;

	// Token: 0x04003E4C RID: 15948
	[Serialize]
	private Vector2I worldSize;

	// Token: 0x04003E4D RID: 15949
	[Serialize]
	private bool fullyEnclosedBorder;

	// Token: 0x04003E4E RID: 15950
	[Serialize]
	private int hiddenYOffset;

	// Token: 0x04003E4F RID: 15951
	[Serialize]
	private bool isModuleInterior;

	// Token: 0x04003E50 RID: 15952
	[Serialize]
	private WorldDetailSave.OverworldCell overworldCell;

	// Token: 0x04003E51 RID: 15953
	[Serialize]
	private bool isDiscovered;

	// Token: 0x04003E52 RID: 15954
	[Serialize]
	private bool isStartWorld;

	// Token: 0x04003E53 RID: 15955
	[Serialize]
	private bool isDupeVisited;

	// Token: 0x04003E54 RID: 15956
	[Serialize]
	private float dupeVisitedTimestamp = -1f;

	// Token: 0x04003E55 RID: 15957
	[Serialize]
	private float discoveryTimestamp = -1f;

	// Token: 0x04003E56 RID: 15958
	[Serialize]
	private bool isRoverVisited;

	// Token: 0x04003E57 RID: 15959
	[Serialize]
	private bool isSurfaceRevealed;

	// Token: 0x04003E58 RID: 15960
	[Serialize]
	public string worldName;

	// Token: 0x04003E59 RID: 15961
	[Serialize]
	public string[] nameTables;

	// Token: 0x04003E5A RID: 15962
	[Serialize]
	public Tag[] worldTags;

	// Token: 0x04003E5B RID: 15963
	[Serialize]
	public string overrideName;

	// Token: 0x04003E5C RID: 15964
	[Serialize]
	public string worldType;

	// Token: 0x04003E5D RID: 15965
	[Serialize]
	public string worldDescription;

	// Token: 0x04003E5E RID: 15966
	[Serialize]
	public int northernlights = FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;

	// Token: 0x04003E5F RID: 15967
	[Serialize]
	public int largeImpactorFragments = FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.DEFAULT_VALUE;

	// Token: 0x04003E60 RID: 15968
	[Serialize]
	public int sunlight = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;

	// Token: 0x04003E61 RID: 15969
	[Serialize]
	public int cosmicRadiation = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04003E62 RID: 15970
	[Serialize]
	public float currentSunlightIntensity;

	// Token: 0x04003E63 RID: 15971
	[Serialize]
	public float currentCosmicIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04003E64 RID: 15972
	[Serialize]
	public string sunlightFixedTrait;

	// Token: 0x04003E65 RID: 15973
	[Serialize]
	public string cosmicRadiationFixedTrait;

	// Token: 0x04003E66 RID: 15974
	[Serialize]
	public string northernLightFixedTrait;

	// Token: 0x04003E67 RID: 15975
	[Serialize]
	public string largeImpactorFragmentsFixedTrait;

	// Token: 0x04003E68 RID: 15976
	[Serialize]
	public int fixedTraitsUpdateVersion = 1;

	// Token: 0x04003E69 RID: 15977
	private Dictionary<string, int> sunlightFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.SUNLIGHT.NAME.NONE,
			FIXEDTRAITS.SUNLIGHT.NONE
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.LOW,
			FIXEDTRAITS.SUNLIGHT.LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW,
			FIXEDTRAITS.SUNLIGHT.MED_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED,
			FIXEDTRAITS.SUNLIGHT.MED
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH,
			FIXEDTRAITS.SUNLIGHT.MED_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.HIGH,
			FIXEDTRAITS.SUNLIGHT.HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_VERY_HIGH
		}
	};

	// Token: 0x04003E6A RID: 15978
	private Dictionary<string, int> northernLightsFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE,
			FIXEDTRAITS.NORTHERNLIGHTS.NONE
		},
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.ENABLED,
			FIXEDTRAITS.NORTHERNLIGHTS.ENABLED
		}
	};

	// Token: 0x04003E6B RID: 15979
	private Dictionary<string, int> largeImpactorFragmentsFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NAME.NONE,
			FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NONE
		},
		{
			FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NAME.ALLOWED,
			FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.ALLOWED
		}
	};

	// Token: 0x04003E6C RID: 15980
	private Dictionary<string, int> cosmicRadiationFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.NONE,
			FIXEDTRAITS.COSMICRADIATION.NONE
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.LOW,
			FIXEDTRAITS.COSMICRADIATION.LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW,
			FIXEDTRAITS.COSMICRADIATION.MED_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED,
			FIXEDTRAITS.COSMICRADIATION.MED
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH,
			FIXEDTRAITS.COSMICRADIATION.MED_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.HIGH,
			FIXEDTRAITS.COSMICRADIATION.HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_HIGH
		}
	};

	// Token: 0x04003E6D RID: 15981
	[Serialize]
	private List<string> m_seasonIds;

	// Token: 0x04003E6E RID: 15982
	[Serialize]
	private List<string> m_subworldNames;

	// Token: 0x04003E6F RID: 15983
	[Serialize]
	private List<string> m_worldTraitIds;

	// Token: 0x04003E70 RID: 15984
	[Serialize]
	private List<string> m_storyTraitIds;

	// Token: 0x04003E71 RID: 15985
	[Serialize]
	private List<string> m_generatedSubworlds;

	// Token: 0x04003E72 RID: 15986
	private WorldParentChangedEventArgs parentChangeArgs = new WorldParentChangedEventArgs();

	// Token: 0x04003E73 RID: 15987
	[MySmiReq]
	private AlertStateManager.Instance m_alertManager;

	// Token: 0x04003E74 RID: 15988
	private List<Prioritizable> yellowAlertTasks = new List<Prioritizable>();

	// Token: 0x04003E76 RID: 15990
	private List<int> m_childWorlds = new List<int>();
}
