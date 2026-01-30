using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class VineMotherConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000932 RID: 2354 RVA: 0x0003DDE8 File Offset: 0x0003BFE8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0003DDEF File Offset: 0x0003BFEF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0003DDF4 File Offset: 0x0003BFF4
	public GameObject CreatePrefab()
	{
		string id = "VineMother";
		string name = STRINGS.CREATURES.SPECIES.VINEMOTHER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.VINEMOTHER.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("vine_mother_kanim"), "object", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 308.15f);
		string text = "VineMotherOriginal";
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 298.15f, 318.15f, 378.15f, VineMotherConfig.ALLOWED_ELEMENTS, false, 0f, 0.15f, null, true, false, true, false, 2400f, 0f, 2200f, text, STRINGS.CREATURES.SPECIES.VINEMOTHER.NAME);
		WiltCondition component = gameObject.GetComponent<WiltCondition>();
		component.WiltDelay = 0f;
		component.RecoveryDelay = 0f;
		KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component2.PrefabID().ToString());
		gameObject.AddOrGet<Traits>();
		Db.Get().traits.Get(text);
		gameObject.GetComponent<Modifiers>().initialTraits.Add(text);
		VineMother.Def def = gameObject.AddOrGetDef<VineMother.Def>();
		def.BRANCH_PREFAB_NAME = "VineBranch";
		def.MAX_BRANCH_COUNT = 24;
		gameObject.AddOrGet<HarvestDesignatable>();
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Water,
				massConsumptionRate = 0.15f
			}
		});
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "VineMotherSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.VINEMOTHER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.VINEMOTHER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_vine_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.VINEMOTHER.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, null, "", false), "VineMother_preview", Assets.GetAnim("vine_mother_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0003DFFD File Offset: 0x0003C1FD
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0003DFFF File Offset: 0x0003C1FF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006DD RID: 1757
	public const string ID = "VineMother";

	// Token: 0x040006DE RID: 1758
	public const string SEED_ID = "VineMotherSeed";

	// Token: 0x040006DF RID: 1759
	public const int MAX_BRANCH_NETWORK_COUNT = 12;

	// Token: 0x040006E0 RID: 1760
	public static SimHashes[] ALLOWED_ELEMENTS = new SimHashes[]
	{
		SimHashes.Oxygen,
		SimHashes.CarbonDioxide,
		SimHashes.ContaminatedOxygen
	};

	// Token: 0x040006E1 RID: 1761
	public const float IRRIGATION_RATE = 0.15f;

	// Token: 0x040006E2 RID: 1762
	public const float TEMPERATURE_LETHAL_LOW = 273.15f;

	// Token: 0x040006E3 RID: 1763
	public const float TEMPERATURE_WARNING_LOW = 298.15f;

	// Token: 0x040006E4 RID: 1764
	public const float TEMPERATURE_WARNING_HIGH = 318.15f;

	// Token: 0x040006E5 RID: 1765
	public const float TEMPERATURE_LETHAL_HIGH = 378.15f;
}
