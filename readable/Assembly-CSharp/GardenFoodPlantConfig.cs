using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class GardenFoodPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000810 RID: 2064 RVA: 0x00036BEB File Offset: 0x00034DEB
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00036BF2 File Offset: 0x00034DF2
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00036BF8 File Offset: 0x00034DF8
	public GameObject CreatePrefab()
	{
		string id = "GardenFoodPlant";
		string name = STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("spike_fruit_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 263.15f, 268.15f, 313.15f, 323.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "GardenFoodPlantFood", true, true, true, true, 2400f, 0f, 4600f, "GardenFoodPlantOriginal", STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGetDef<PollinationMonitor.Def>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "GardenFoodPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.GARDENFOODPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.GARDENFOODPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_spikefruit_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.DOMESTICATEDDESC;
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Peat.CreateTag(),
				massConsumptionRate = 0.016666668f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "GardenFoodPlant_preview", Assets.GetAnim("spike_fruit_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("spike_fruit_kanim", "spike_fruit_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("spike_fruit_kanim", "spike_fruit_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00036DD9 File Offset: 0x00034FD9
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00036DDB File Offset: 0x00034FDB
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000611 RID: 1553
	public const string ID = "GardenFoodPlant";

	// Token: 0x04000612 RID: 1554
	public const string SEED_ID = "GardenFoodPlantSeed";
}
