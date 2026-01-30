using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class SpiceVineConfig : IEntityConfig
{
	// Token: 0x060008F8 RID: 2296 RVA: 0x0003CAB8 File Offset: 0x0003ACB8
	public GameObject CreatePrefab()
	{
		string id = "SpiceVine";
		string name = STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SPICE_VINE.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("vinespicenut_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 3;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 320f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 3);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 308.15f, 358.15f, 448.15f, null, true, 0f, 0.15f, SpiceNutConfig.ID, true, true, true, true, 2400f, 0f, 9800f, "SpiceVineOriginal", STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.058333334f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Phosphorite,
				massConsumptionRate = 0.0016666667f
			}
		});
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SpiceVineSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_spicenut_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.SPICE_VINE.DOMESTICATEDDESC;
		GameObject template = EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 4, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "SpiceVine_preview", Assets.GetAnim("vinespicenut_kanim"), "place", 1, 3);
		gameObject.AddOrGet<PlantFiberProducer>().amount = 16f;
		EntityTemplates.MakeHangingOffsets(template, 1, 3);
		return gameObject;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0003CCDA File Offset: 0x0003AEDA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0003CCDC File Offset: 0x0003AEDC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006B4 RID: 1716
	public const string ID = "SpiceVine";

	// Token: 0x040006B5 RID: 1717
	public const string SEED_ID = "SpiceVineSeed";

	// Token: 0x040006B6 RID: 1718
	public const float FERTILIZATION_RATE = 0.0016666667f;

	// Token: 0x040006B7 RID: 1719
	public const float WATER_RATE = 0.058333334f;

	// Token: 0x040006B8 RID: 1720
	public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 16f;
}
