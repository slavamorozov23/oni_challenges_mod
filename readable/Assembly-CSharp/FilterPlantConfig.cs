using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class FilterPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007E4 RID: 2020 RVA: 0x00035BB4 File Offset: 0x00033DB4
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00035BBB File Offset: 0x00033DBB
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00035BC0 File Offset: 0x00033DC0
	public GameObject CreatePrefab()
	{
		string id = "FilterPlant";
		string name = STRINGS.CREATURES.SPECIES.FILTERPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FILTERPLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("cactus_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 348.15f);
		GameObject template = gameObject;
		float temperature_lethal_low = 253.15f;
		float temperature_warning_low = 293.15f;
		float temperature_warning_high = 383.15f;
		float temperature_lethal_high = 443.15f;
		string crop_id = SimHashes.Water.ToString();
		string text = STRINGS.CREATURES.SPECIES.FILTERPLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen
		}, true, 0f, 0.025f, crop_id, true, true, true, true, 2400f, 0f, 2200f, "FilterPlantOriginal", text);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sand.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.DirtyWater,
				massConsumptionRate = 0.108333334f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<SaltPlant>();
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.showDescriptor = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.Oxygen;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 4;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.008333334f;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "FilterPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.FILTERPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.FILTERPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_cactus_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.FILTERPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 21, text, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, null, "", false), "FilterPlant_preview", Assets.GetAnim("cactus_kanim"), "place", 1, 2);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00035E06 File Offset: 0x00034006
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00035E08 File Offset: 0x00034008
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F7 RID: 1527
	public const string ID = "FilterPlant";

	// Token: 0x040005F8 RID: 1528
	public const string SEED_ID = "FilterPlantSeed";

	// Token: 0x040005F9 RID: 1529
	public const float SAND_CONSUMPTION_RATE = 0.008333334f;

	// Token: 0x040005FA RID: 1530
	public const float WATER_CONSUMPTION_RATE = 0.108333334f;

	// Token: 0x040005FB RID: 1531
	public const float OXYGEN_CONSUMPTION_RATE = 0.008333334f;
}
