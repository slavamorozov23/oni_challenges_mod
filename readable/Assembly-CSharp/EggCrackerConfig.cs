using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000080 RID: 128
[EntityConfigOrder(2)]
public class EggCrackerConfig : IBuildingConfig
{
	// Token: 0x06000265 RID: 613 RVA: 0x00010B2F File Offset: 0x0000ED2F
	public static void RegisterEgg(Tag eggPrefabTag, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC)
	{
		EggCrackerConfig.RegisterEgg(eggPrefabTag, name, description, mass, requiredDLC, forbiddenDLC, null);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00010B3F File Offset: 0x0000ED3F
	public static void RegisterEgg(Tag eggPrefabTag, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC, global::Tuple<Tag, float>[] customDrops)
	{
		EggCrackerConfig.RegisterEgg(eggPrefabTag, name, description, mass, requiredDLC, forbiddenDLC, customDrops, true);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00010B54 File Offset: 0x0000ED54
	public static void RegisterEgg(Tag eggPrefabTag, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC, global::Tuple<Tag, float>[] customDrops, bool allowCrackerRecipeCreation = true)
	{
		EggCrackerConfig.EggData eggData = new EggCrackerConfig.EggData(eggPrefabTag, name, description, mass, requiredDLC, forbiddenDLC, allowCrackerRecipeCreation);
		eggData.customOutput = customDrops;
		EggCrackerConfig.uncategorizedEggData.Add(eggData);
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00010B84 File Offset: 0x0000ED84
	public static void CategorizeEggs()
	{
		foreach (EggCrackerConfig.EggData eggData in EggCrackerConfig.uncategorizedEggData)
		{
			Tag spawnedCreature = Assets.GetPrefab(eggData.id).GetDef<IncubationMonitor.Def>().spawnedCreature;
			Tag species = Assets.GetPrefab(spawnedCreature).GetComponent<CreatureBrain>().species;
			eggData.isBaseMorph = Assets.GetPrefab(spawnedCreature).HasTag(GameTags.OriginalCreature);
			if (!EggCrackerConfig.EggsBySpecies.ContainsKey(species))
			{
				EggCrackerConfig.EggsBySpecies.Add(species, new List<EggCrackerConfig.EggData>());
			}
			EggCrackerConfig.EggsBySpecies[species].Add(eggData);
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00010C3C File Offset: 0x0000EE3C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "EggCracker";
		int width = 2;
		int height = 2;
		string anim = "egg_cracker_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
		return buildingDef;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00010CC0 File Offset: 0x0000EEC0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity("snapto_egg", false);
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.labelByResult = false;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.duplicantOperated = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		Workable workable = go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_egg_cracker_kanim")
		};
		complexFabricator.outputOffset = new Vector3(1f, 1f, 0f);
		Prioritizable.AddRef(go);
		go.AddOrGet<EggCracker>();
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00010D71 File Offset: 0x0000EF71
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00010D7A File Offset: 0x0000EF7A
	public override void ConfigurePost(BuildingDef def)
	{
		base.ConfigurePost(def);
		this.MakeRecipes();
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00010D8C File Offset: 0x0000EF8C
	public void MakeRecipes()
	{
		EggCrackerConfig.CategorizeEggs();
		foreach (KeyValuePair<Tag, List<EggCrackerConfig.EggData>> keyValuePair in EggCrackerConfig.EggsBySpecies)
		{
			Tag[] array = new Tag[keyValuePair.Value.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = keyValuePair.Value[i].id;
			}
			EggCrackerConfig.EggData eggData = keyValuePair.Value[0];
			if (eggData.hasCrackerRecipe)
			{
				string arg = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, eggData.name);
				ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
				{
					new ComplexRecipe.RecipeElement(array, 1f)
					{
						material = array[0]
					}
				};
				List<ComplexRecipe.RecipeElement> list = new List<ComplexRecipe.RecipeElement>
				{
					new ComplexRecipe.RecipeElement("RawEgg", 0.5f * eggData.mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
					new ComplexRecipe.RecipeElement("EggShell", 0.5f * eggData.mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
				};
				if (eggData.customOutput != null)
				{
					foreach (global::Tuple<Tag, float> tuple in eggData.customOutput)
					{
						list.Add(new ComplexRecipe.RecipeElement(tuple.first, tuple.second, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false));
					}
				}
				ComplexRecipe.RecipeElement[] array3 = list.ToArray();
				string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("EggCracker", "RawEgg");
				string text = ComplexRecipeManager.MakeRecipeID("EggCracker", array2, array3);
				ComplexRecipe complexRecipe = new ComplexRecipe(text, array2, array3, eggData.requiredDlcIds, eggData.forbiddenDlcIds);
				complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, eggData.name, arg);
				complexRecipe.fabricators = new List<Tag>
				{
					"EggCracker"
				};
				complexRecipe.time = 5f;
				complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom;
				complexRecipe.customName = keyValuePair.Key.ProperName();
				complexRecipe.customSpritePrefabID = ((array2[0].material != null) ? array2[0].material.Name : array2[0].possibleMaterials[0].Name);
				ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
			}
		}
	}

	// Token: 0x04000183 RID: 387
	public const string ID = "EggCracker";

	// Token: 0x04000184 RID: 388
	public static Dictionary<Tag, List<EggCrackerConfig.EggData>> EggsBySpecies = new Dictionary<Tag, List<EggCrackerConfig.EggData>>();

	// Token: 0x04000185 RID: 389
	private static List<EggCrackerConfig.EggData> uncategorizedEggData = new List<EggCrackerConfig.EggData>();

	// Token: 0x020010A8 RID: 4264
	public class EggData : IHasDlcRestrictions
	{
		// Token: 0x060082A3 RID: 33443 RVA: 0x00342356 File Offset: 0x00340556
		public EggData(Tag id, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC)
		{
			this.Config(id, name, description, mass, requiredDLC, forbiddenDLC, true);
		}

		// Token: 0x060082A4 RID: 33444 RVA: 0x0034236E File Offset: 0x0034056E
		public EggData(Tag id, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC, bool hasCrackerRecipe = true)
		{
			this.Config(id, name, description, mass, requiredDLC, forbiddenDLC, hasCrackerRecipe);
		}

		// Token: 0x060082A5 RID: 33445 RVA: 0x00342387 File Offset: 0x00340587
		private void Config(Tag id, string name, string description, float mass, string[] requiredDLC, string[] forbiddenDLC, bool hasCrackerRecipe = true)
		{
			this.id = id;
			this.name = name;
			this.description = description;
			this.mass = mass;
			this.requiredDlcIds = requiredDLC;
			this.forbiddenDlcIds = forbiddenDLC;
			this.hasCrackerRecipe = hasCrackerRecipe;
		}

		// Token: 0x060082A6 RID: 33446 RVA: 0x003423BE File Offset: 0x003405BE
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x060082A7 RID: 33447 RVA: 0x003423C6 File Offset: 0x003405C6
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x040062F3 RID: 25331
		public Tag id;

		// Token: 0x040062F4 RID: 25332
		public float mass;

		// Token: 0x040062F5 RID: 25333
		public string name;

		// Token: 0x040062F6 RID: 25334
		public string description;

		// Token: 0x040062F7 RID: 25335
		public string[] requiredDlcIds;

		// Token: 0x040062F8 RID: 25336
		public string[] forbiddenDlcIds;

		// Token: 0x040062F9 RID: 25337
		public bool hasCrackerRecipe;

		// Token: 0x040062FA RID: 25338
		public global::Tuple<Tag, float>[] customOutput;

		// Token: 0x040062FB RID: 25339
		public bool isBaseMorph;
	}
}
