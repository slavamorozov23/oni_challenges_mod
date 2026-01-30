using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000677 RID: 1655
public class Db : EntityModifierSet
{
	// Token: 0x06002886 RID: 10374 RVA: 0x000E8EBC File Offset: 0x000E70BC
	public static string GetPath(string dlcId, string folder)
	{
		string result;
		if (dlcId == "")
		{
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, folder));
		}
		else
		{
			string contentDirectoryName = DlcManager.GetContentDirectoryName(dlcId);
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, "dlc", contentDirectoryName, folder));
		}
		return result;
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000E8F08 File Offset: 0x000E7108
	public static Db Get()
	{
		if (Db._Instance == null)
		{
			Db._Instance = Resources.Load<Db>("Db");
			Db._Instance.Initialize();
		}
		return Db._Instance;
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000E8F35 File Offset: 0x000E7135
	public static BuildingFacades GetBuildingFacades()
	{
		return Db.Get().Permits.BuildingFacades;
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000E8F46 File Offset: 0x000E7146
	public static ArtableStages GetArtableStages()
	{
		return Db.Get().Permits.ArtableStages;
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x000E8F57 File Offset: 0x000E7157
	public static EquippableFacades GetEquippableFacades()
	{
		return Db.Get().Permits.EquippableFacades;
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x000E8F68 File Offset: 0x000E7168
	public static StickerBombs GetStickerBombs()
	{
		return Db.Get().Permits.StickerBombs;
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x000E8F79 File Offset: 0x000E7179
	public static MonumentParts GetMonumentParts()
	{
		return Db.Get().Permits.MonumentParts;
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x000E8F8C File Offset: 0x000E718C
	public override void Initialize()
	{
		base.Initialize();
		this.Urges = new Urges();
		this.AssignableSlots = new AssignableSlots();
		this.StateMachineCategories = new StateMachineCategories();
		this.Personalities = new Personalities();
		this.Faces = new Faces();
		this.Shirts = new Shirts();
		this.Expressions = new Expressions(this.Root);
		this.Emotes = new Emotes(this.Root);
		this.Thoughts = new Thoughts(this.Root);
		this.CritterEmotions = new CritterEmotions(this.Root);
		this.Dreams = new Dreams(this.Root);
		this.Deaths = new Deaths(this.Root);
		this.StatusItemCategories = new StatusItemCategories(this.Root);
		this.TechTreeTitles = new TechTreeTitles(this.Root);
		this.TechTreeTitles.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.Techs = new Techs(this.Root);
		this.TechItems = new TechItems(this.Root);
		this.Techs.Init();
		this.Techs.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.TechItems.Init();
		this.Accessories = new Accessories(this.Root);
		this.AccessorySlots = new AccessorySlots(this.Root);
		this.ScheduleBlockTypes = new ScheduleBlockTypes(this.Root);
		this.ScheduleGroups = new ScheduleGroups(this.Root);
		this.RoomTypeCategories = new RoomTypeCategories(this.Root);
		this.RoomTypes = new RoomTypes(this.Root);
		this.ArtifactDropRates = new ArtifactDropRates(this.Root);
		this.SpaceDestinationTypes = new SpaceDestinationTypes(this.Root);
		this.Diseases = new Diseases(this.Root, false);
		this.Sicknesses = new Database.Sicknesses(this.Root);
		this.SkillPerks = new SkillPerks(this.Root);
		this.SkillGroups = new SkillGroups(this.Root);
		this.Skills = new Skills(this.Root);
		this.ColonyAchievements = new ColonyAchievements(this.Root);
		this.MiscStatusItems = new MiscStatusItems(this.Root);
		this.CreatureStatusItems = new CreatureStatusItems(this.Root);
		this.BuildingStatusItems = new BuildingStatusItems(this.Root);
		this.RobotStatusItems = new RobotStatusItems(this.Root);
		this.ChoreTypes = new ChoreTypes(this.Root);
		this.Quests = new Quests(this.Root);
		this.GameplayEvents = new GameplayEvents(this.Root);
		this.GameplaySeasons = new GameplaySeasons(this.Root);
		this.Stories = new Stories(this.Root);
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			this.PlantMutations = new PlantMutations(this.Root);
		}
		this.OrbitalTypeCategories = new OrbitalTypeCategories(this.Root);
		this.ArtableStatuses = new ArtableStatuses(this.Root);
		this.Permits = new PermitResources(this.Root);
		Effect effect = new Effect("CenterOfAttention", DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier("StressDelta", -0.008333334f, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, false, false, true));
		this.effects.Add(effect);
		this.Spices = new Spices(this.Root);
		this.CollectResources(this.Root, this.ResourceTable);
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x000E933E File Offset: 0x000E753E
	public void PostProcess()
	{
		this.Techs.PostProcess();
		this.Permits.PostProcess();
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000E9358 File Offset: 0x000E7558
	private void CollectResources(Resource resource, List<Resource> resource_table)
	{
		if (resource.Guid != null)
		{
			resource_table.Add(resource);
		}
		ResourceSet resourceSet = resource as ResourceSet;
		if (resourceSet != null)
		{
			for (int i = 0; i < resourceSet.Count; i++)
			{
				this.CollectResources(resourceSet.GetResource(i), resource_table);
			}
		}
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000E93A4 File Offset: 0x000E75A4
	public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource
	{
		Resource resource = this.ResourceTable.FirstOrDefault((Resource s) => s.Guid == guid);
		if (resource == null)
		{
			string str = "Could not find resource: ";
			ResourceGuid guid2 = guid;
			global::Debug.LogWarning(str + ((guid2 != null) ? guid2.ToString() : null));
			return default(ResourceType);
		}
		ResourceType resourceType = (ResourceType)((object)resource);
		if (resourceType == null)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Resource type mismatch for resource: ",
				resource.Id,
				"\nExpecting Type: ",
				typeof(ResourceType).Name,
				"\nGot Type: ",
				resource.GetType().Name
			}));
			return default(ResourceType);
		}
		return resourceType;
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000E946F File Offset: 0x000E766F
	public void ResetProblematicDbs()
	{
		this.Emotes.ResetProblematicReferences();
	}

	// Token: 0x040017C9 RID: 6089
	private static Db _Instance;

	// Token: 0x040017CA RID: 6090
	public TextAsset researchTreeFileVanilla;

	// Token: 0x040017CB RID: 6091
	public TextAsset researchTreeFileExpansion1;

	// Token: 0x040017CC RID: 6092
	public Diseases Diseases;

	// Token: 0x040017CD RID: 6093
	public Database.Sicknesses Sicknesses;

	// Token: 0x040017CE RID: 6094
	public Urges Urges;

	// Token: 0x040017CF RID: 6095
	public AssignableSlots AssignableSlots;

	// Token: 0x040017D0 RID: 6096
	public StateMachineCategories StateMachineCategories;

	// Token: 0x040017D1 RID: 6097
	public Personalities Personalities;

	// Token: 0x040017D2 RID: 6098
	public Faces Faces;

	// Token: 0x040017D3 RID: 6099
	public Shirts Shirts;

	// Token: 0x040017D4 RID: 6100
	public Expressions Expressions;

	// Token: 0x040017D5 RID: 6101
	public Emotes Emotes;

	// Token: 0x040017D6 RID: 6102
	public Thoughts Thoughts;

	// Token: 0x040017D7 RID: 6103
	public CritterEmotions CritterEmotions;

	// Token: 0x040017D8 RID: 6104
	public Dreams Dreams;

	// Token: 0x040017D9 RID: 6105
	public BuildingStatusItems BuildingStatusItems;

	// Token: 0x040017DA RID: 6106
	public MiscStatusItems MiscStatusItems;

	// Token: 0x040017DB RID: 6107
	public CreatureStatusItems CreatureStatusItems;

	// Token: 0x040017DC RID: 6108
	public RobotStatusItems RobotStatusItems;

	// Token: 0x040017DD RID: 6109
	public StatusItemCategories StatusItemCategories;

	// Token: 0x040017DE RID: 6110
	public Deaths Deaths;

	// Token: 0x040017DF RID: 6111
	public ChoreTypes ChoreTypes;

	// Token: 0x040017E0 RID: 6112
	public TechItems TechItems;

	// Token: 0x040017E1 RID: 6113
	public AccessorySlots AccessorySlots;

	// Token: 0x040017E2 RID: 6114
	public Accessories Accessories;

	// Token: 0x040017E3 RID: 6115
	public ScheduleBlockTypes ScheduleBlockTypes;

	// Token: 0x040017E4 RID: 6116
	public ScheduleGroups ScheduleGroups;

	// Token: 0x040017E5 RID: 6117
	public RoomTypeCategories RoomTypeCategories;

	// Token: 0x040017E6 RID: 6118
	public RoomTypes RoomTypes;

	// Token: 0x040017E7 RID: 6119
	public ArtifactDropRates ArtifactDropRates;

	// Token: 0x040017E8 RID: 6120
	public SpaceDestinationTypes SpaceDestinationTypes;

	// Token: 0x040017E9 RID: 6121
	public SkillPerks SkillPerks;

	// Token: 0x040017EA RID: 6122
	public SkillGroups SkillGroups;

	// Token: 0x040017EB RID: 6123
	public Skills Skills;

	// Token: 0x040017EC RID: 6124
	public ColonyAchievements ColonyAchievements;

	// Token: 0x040017ED RID: 6125
	public Quests Quests;

	// Token: 0x040017EE RID: 6126
	public GameplayEvents GameplayEvents;

	// Token: 0x040017EF RID: 6127
	public GameplaySeasons GameplaySeasons;

	// Token: 0x040017F0 RID: 6128
	public PlantMutations PlantMutations;

	// Token: 0x040017F1 RID: 6129
	public Spices Spices;

	// Token: 0x040017F2 RID: 6130
	public Techs Techs;

	// Token: 0x040017F3 RID: 6131
	public TechTreeTitles TechTreeTitles;

	// Token: 0x040017F4 RID: 6132
	public OrbitalTypeCategories OrbitalTypeCategories;

	// Token: 0x040017F5 RID: 6133
	public PermitResources Permits;

	// Token: 0x040017F6 RID: 6134
	public ArtableStatuses ArtableStatuses;

	// Token: 0x040017F7 RID: 6135
	public Stories Stories;

	// Token: 0x02001550 RID: 5456
	[Serializable]
	public class SlotInfo : Resource
	{
	}
}
