using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class BlueGrassConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000791 RID: 1937 RVA: 0x00033F30 File Offset: 0x00032130
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00033F37 File Offset: 0x00032137
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00033F3C File Offset: 0x0003213C
	public GameObject CreatePrefab()
	{
		string id = "BlueGrass";
		string name = STRINGS.CREATURES.SPECIES.BLUE_GRASS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BLUE_GRASS.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("bluegrass_kanim"), "idle_full", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 240f);
		GameObject template = gameObject;
		float temperature_lethal_low = 193.15f;
		float temperature_warning_low = 193.15f;
		float temperature_warning_high = 273.15f;
		float temperature_lethal_high = 273.15f;
		string text = STRINGS.CREATURES.SPECIES.BLUE_GRASS.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0f, "OxyRock", true, true, true, true, 2400f, 0f, 2200f, "BlueGrassOriginal", text);
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 2;
		elementConsumer.EnableConsumption(true);
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.0005f;
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Ice.CreateTag(),
				massConsumptionRate = 0.033333335f
			}
		});
		gameObject.GetComponent<UprootedMonitor>();
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<BlueGrass>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "BlueGrassSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.BLUE_GRASS.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.BLUE_GRASS.DESC;
		KAnimFile anim = Assets.GetAnim("seed_bluegrass_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.BLUE_GRASS.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 4, text, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "BlueGrass_preview", Assets.GetAnim("bluegrass_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00034132 File Offset: 0x00032332
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x00034134 File Offset: 0x00032334
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005BA RID: 1466
	public const string ID = "BlueGrass";

	// Token: 0x040005BB RID: 1467
	public const string SEED_ID = "BlueGrassSeed";

	// Token: 0x040005BC RID: 1468
	public const float CO2_RATE = 0.002f;

	// Token: 0x040005BD RID: 1469
	public const float FERTILIZATION_RATE = 20f;
}
