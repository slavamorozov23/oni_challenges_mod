using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class HardSkinBerryPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000842 RID: 2114 RVA: 0x0003826A File Offset: 0x0003646A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00038271 File Offset: 0x00036471
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00038274 File Offset: 0x00036474
	public GameObject CreatePrefab()
	{
		string id = "HardSkinBerryPlant";
		string name = STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("ice_berry_bush_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 118.149994f, 218.15f, 259.15f, 269.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "HardSkinBerry", true, true, true, true, 2400f, 0f, 4600f, "HardSkinBerryPlantOriginal", STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "HardSkinBerryPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.HARDSKINBERRYPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.HARDSKINBERRYPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_ice_berry_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.DOMESTICATEDDESC;
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Phosphorite.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "HardSkinBerryPlant_preview", Assets.GetAnim("ice_berry_bush_kanim"), "place", 1, 2);
		gameObject.AddOrGet<PlantFiberProducer>().amount = 12f;
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0003845E File Offset: 0x0003665E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00038460 File Offset: 0x00036660
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400063A RID: 1594
	public const string ID = "HardSkinBerryPlant";

	// Token: 0x0400063B RID: 1595
	public const string SEED_ID = "HardSkinBerryPlantSeed";

	// Token: 0x0400063C RID: 1596
	public const float Temperature_lethal_low = 118.149994f;

	// Token: 0x0400063D RID: 1597
	public const float Temperature_warning_low = 218.15f;

	// Token: 0x0400063E RID: 1598
	public const float Temperature_lethal_high = 269.15f;

	// Token: 0x0400063F RID: 1599
	public const float Temperature_warning_high = 259.15f;

	// Token: 0x04000640 RID: 1600
	public const float FERTILIZATION_RATE = 0.008333334f;

	// Token: 0x04000641 RID: 1601
	public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 12f;
}
