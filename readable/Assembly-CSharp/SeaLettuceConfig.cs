using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class SeaLettuceConfig : IEntityConfig
{
	// Token: 0x0600088F RID: 2191 RVA: 0x00039BF0 File Offset: 0x00037DF0
	public GameObject CreatePrefab()
	{
		string id = SeaLettuceConfig.ID;
		string name = STRINGS.CREATURES.SPECIES.SEALETTUCE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SEALETTUCE.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("sea_lettuce_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 308.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 248.15f, 295.15f, 338.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Water,
			SimHashes.SaltWater,
			SimHashes.Brine
		}, false, 0f, 0.15f, "Lettuce", true, true, true, true, 2400f, 0f, 7400f, SeaLettuceConfig.ID + "Original", STRINGS.CREATURES.SPECIES.SEALETTUCE.NAME);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.SaltWater.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.BleachStone.CreateTag(),
				massConsumptionRate = 0.00083333335f
			}
		});
		gameObject.GetComponent<DrowningMonitor>().canDrownToDeath = false;
		gameObject.GetComponent<DrowningMonitor>().livesUnderWater = true;
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SeaLettuceSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.DESC;
		KAnimFile anim = Assets.GetAnim("seed_sealettuce_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.WaterSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.SEALETTUCE.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 3, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), SeaLettuceConfig.ID + "_preview", Assets.GetAnim("sea_lettuce_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00039E2B File Offset: 0x0003802B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00039E2D File Offset: 0x0003802D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000676 RID: 1654
	public static string ID = "SeaLettuce";

	// Token: 0x04000677 RID: 1655
	public const string SEED_ID = "SeaLettuceSeed";

	// Token: 0x04000678 RID: 1656
	public const float WATER_RATE = 0.008333334f;

	// Token: 0x04000679 RID: 1657
	public const float FERTILIZATION_RATE = 0.00083333335f;
}
