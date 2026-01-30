using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000CDB RID: 3291
public class ColonyDestinationAsteroidBeltData
{
	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x06006586 RID: 25990 RVA: 0x002639DC File Offset: 0x00261BDC
	// (set) Token: 0x06006587 RID: 25991 RVA: 0x002639E4 File Offset: 0x00261BE4
	public float TargetScale { get; set; }

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x06006588 RID: 25992 RVA: 0x002639ED File Offset: 0x00261BED
	// (set) Token: 0x06006589 RID: 25993 RVA: 0x002639F5 File Offset: 0x00261BF5
	public float Scale { get; set; }

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x0600658A RID: 25994 RVA: 0x002639FE File Offset: 0x00261BFE
	// (set) Token: 0x0600658B RID: 25995 RVA: 0x00263A06 File Offset: 0x00261C06
	public int seed { get; private set; }

	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x0600658C RID: 25996 RVA: 0x00263A0F File Offset: 0x00261C0F
	public string startWorldPath
	{
		get
		{
			return this.startWorld.filePath;
		}
	}

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x0600658D RID: 25997 RVA: 0x00263A1C File Offset: 0x00261C1C
	// (set) Token: 0x0600658E RID: 25998 RVA: 0x00263A24 File Offset: 0x00261C24
	public Sprite sprite { get; private set; }

	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x0600658F RID: 25999 RVA: 0x00263A2D File Offset: 0x00261C2D
	// (set) Token: 0x06006590 RID: 26000 RVA: 0x00263A35 File Offset: 0x00261C35
	public int difficulty { get; private set; }

	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x06006591 RID: 26001 RVA: 0x00263A3E File Offset: 0x00261C3E
	public string startWorldName
	{
		get
		{
			return Strings.Get(this.startWorld.name);
		}
	}

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x06006592 RID: 26002 RVA: 0x00263A55 File Offset: 0x00261C55
	public string properName
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return "";
			}
			return this.clusterLayout.name;
		}
	}

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06006593 RID: 26003 RVA: 0x00263A70 File Offset: 0x00261C70
	public string beltPath
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return WorldGenSettings.ClusterDefaultName;
			}
			return this.clusterLayout.filePath;
		}
	}

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x06006594 RID: 26004 RVA: 0x00263A8B File Offset: 0x00261C8B
	// (set) Token: 0x06006595 RID: 26005 RVA: 0x00263A93 File Offset: 0x00261C93
	public List<ProcGen.World> worlds { get; private set; }

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x06006596 RID: 26006 RVA: 0x00263A9C File Offset: 0x00261C9C
	public ClusterLayout Layout
	{
		get
		{
			if (this.mutatedClusterLayout != null)
			{
				return this.mutatedClusterLayout.layout;
			}
			return this.clusterLayout;
		}
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x06006597 RID: 26007 RVA: 0x00263AB8 File Offset: 0x00261CB8
	public ProcGen.World GetStartWorld
	{
		get
		{
			return this.startWorld;
		}
	}

	// Token: 0x06006598 RID: 26008 RVA: 0x00263AC0 File Offset: 0x00261CC0
	public ColonyDestinationAsteroidBeltData(string staringWorldName, int seed, string clusterPath)
	{
		this.startWorld = SettingsCache.worlds.GetWorldData(staringWorldName);
		this.Scale = (this.TargetScale = this.startWorld.iconScale);
		this.worlds = new List<ProcGen.World>();
		if (clusterPath != null)
		{
			this.clusterLayout = SettingsCache.clusterLayouts.GetClusterData(clusterPath);
		}
		this.ReInitialize(seed);
	}

	// Token: 0x06006599 RID: 26009 RVA: 0x00263B3C File Offset: 0x00261D3C
	public static Sprite GetUISprite(string filename)
	{
		if (filename.IsNullOrWhiteSpace())
		{
			filename = (DlcManager.FeatureClusterSpaceEnabled() ? "asteroid_sandstone_start_kanim" : "Asteroid_sandstone");
		}
		KAnimFile kanimFile;
		Assets.TryGetAnim(filename, out kanimFile);
		if (kanimFile != null)
		{
			return Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "");
		}
		return Assets.GetSprite(filename);
	}

	// Token: 0x0600659A RID: 26010 RVA: 0x00263B9C File Offset: 0x00261D9C
	public void ReInitialize(int seed)
	{
		this.seed = seed;
		this.paramDescriptors.Clear();
		this.traitDescriptors.Clear();
		this.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(this.startWorld.asteroidIcon);
		this.difficulty = this.clusterLayout.difficulty;
		this.mutatedClusterLayout = WorldgenMixing.DoWorldMixing(this.clusterLayout, seed, true, true);
		this.RemixClusterLayout();
	}

	// Token: 0x0600659B RID: 26011 RVA: 0x00263C08 File Offset: 0x00261E08
	public void RemixClusterLayout()
	{
		if (!WorldgenMixing.RefreshWorldMixing(this.mutatedClusterLayout, this.seed, true, true))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"World remix failed, using default cluster instead."
			});
			this.mutatedClusterLayout = new MutatedClusterLayout(this.clusterLayout);
		}
		this.worlds.Clear();
		for (int i = 0; i < this.Layout.worldPlacements.Count; i++)
		{
			if (i != this.Layout.startWorldIndex)
			{
				this.worlds.Add(SettingsCache.worlds.GetWorldData(this.Layout.worldPlacements[i].world));
			}
		}
	}

	// Token: 0x0600659C RID: 26012 RVA: 0x00263CAD File Offset: 0x00261EAD
	public List<AsteroidDescriptor> GetParamDescriptors()
	{
		if (this.paramDescriptors.Count == 0)
		{
			this.paramDescriptors = this.GenerateParamDescriptors();
		}
		return this.paramDescriptors;
	}

	// Token: 0x0600659D RID: 26013 RVA: 0x00263CCE File Offset: 0x00261ECE
	public List<AsteroidDescriptor> GetTraitDescriptors()
	{
		if (this.traitDescriptors.Count == 0)
		{
			this.traitDescriptors = this.GenerateTraitDescriptors();
		}
		return this.traitDescriptors;
	}

	// Token: 0x0600659E RID: 26014 RVA: 0x00263CF0 File Offset: 0x00261EF0
	private List<AsteroidDescriptor> GenerateParamDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		if (this.clusterLayout != null && DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, Strings.Get(this.clusterLayout.name)), Strings.Get(this.clusterLayout.description), Color.white, null, null));
		}
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.PLANETNAME, this.startWorldName), null, Color.white, null, null));
		list.Add(new AsteroidDescriptor(Strings.Get(this.startWorld.description), null, Color.white, null, null));
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.MOONNAMES, Array.Empty<object>()), null, Color.white, null, null));
			foreach (ProcGen.World world in this.worlds)
			{
				list.Add(new AsteroidDescriptor(string.Format("{0}", Strings.Get(world.name)), Strings.Get(world.description), Color.white, null, null));
			}
		}
		int index = Mathf.Clamp(this.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third), null, Color.white, null, null));
		return list;
	}

	// Token: 0x0600659F RID: 26015 RVA: 0x00263EA8 File Offset: 0x002620A8
	private List<AsteroidDescriptor> GenerateTraitDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			ProcGen.World world = list2[i];
			if (DlcManager.IsExpansion1Active())
			{
				list.Add(new AsteroidDescriptor("", null, Color.white, null, null));
				list.Add(new AsteroidDescriptor(string.Format("<b>{0}</b>", Strings.Get(world.name)), null, Color.white, null, null));
			}
			List<WorldTrait> worldTraits = this.GetWorldTraits(world);
			foreach (WorldTrait worldTrait in worldTraits)
			{
				string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
				list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
			}
			if (worldTraits.Count == 0)
			{
				list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
			}
		}
		return list;
	}

	// Token: 0x060065A0 RID: 26016 RVA: 0x00264024 File Offset: 0x00262224
	public List<AsteroidDescriptor> GenerateTraitDescriptors(ProcGen.World singleWorld, bool includeDefaultTrait = true)
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World singleWorld2 = list2[i];
				List<WorldTrait> worldTraits = this.GetWorldTraits(singleWorld2);
				foreach (WorldTrait worldTrait in worldTraits)
				{
					string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
					list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
				}
				if (worldTraits.Count == 0 && includeDefaultTrait)
				{
					list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
				}
			}
		}
		return list;
	}

	// Token: 0x060065A1 RID: 26017 RVA: 0x0026416C File Offset: 0x0026236C
	public List<WorldTrait> GetWorldTraits(ProcGen.World singleWorld)
	{
		List<WorldTrait> list = new List<WorldTrait>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World world = list2[i];
				int num = this.seed;
				if (num > 0)
				{
					num += this.clusterLayout.worldPlacements.FindIndex((WorldPlacement x) => x.world == world.filePath);
				}
				foreach (string name in SettingsCache.GetRandomTraits(num, world))
				{
					WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(name, true);
					list.Add(cachedWorldTrait);
				}
			}
		}
		return list;
	}

	// Token: 0x040044D7 RID: 17623
	private ProcGen.World startWorld;

	// Token: 0x040044D8 RID: 17624
	private ClusterLayout clusterLayout;

	// Token: 0x040044D9 RID: 17625
	private MutatedClusterLayout mutatedClusterLayout;

	// Token: 0x040044DA RID: 17626
	private List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x040044DB RID: 17627
	private List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x040044DC RID: 17628
	public static List<global::Tuple<string, string, string>> survivalOptions = new List<global::Tuple<string, string, string>>
	{
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.MOSTHOSPITABLE, "", "D2F40C"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYHIGH, "", "7DE419"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.HIGH, "", "36D246"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.NEUTRAL, "", "63C2B7"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LOW, "", "6A8EB1"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYLOW, "", "937890"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LEASTHOSPITABLE, "", "9636DF")
	};
}
