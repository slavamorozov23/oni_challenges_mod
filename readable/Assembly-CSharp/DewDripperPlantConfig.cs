using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class DewDripperPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007CD RID: 1997 RVA: 0x0003540F File Offset: 0x0003360F
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00035416 File Offset: 0x00033616
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0003541C File Offset: 0x0003361C
	public GameObject CreatePrefab()
	{
		string id = "DewDripperPlant";
		string name = STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		KAnimFile anim = Assets.GetAnim("brackwood_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 253.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 238.15f, 278.15f, 308.15f, null, true, 0f, 0.25f, DewDripConfig.ID, true, false, true, true, 2400f, 0f, 4600f, "DewDripperPlantOriginal", STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.NAME);
		PressureVulnerable pressureVulnerable = gameObject.AddOrGet<PressureVulnerable>();
		pressureVulnerable.pressureWarning_High = 2f;
		pressureVulnerable.pressureLethal_High = 10f;
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<PlantFiberProducer>().amount = 2f;
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.BrineIce.CreateTag(),
				massConsumptionRate = 0.016666668f
			}
		});
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "DewDripperPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.DEWDRIPPERPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.DEWDRIPPERPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_brackwood_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.DOMESTICATEDDESC;
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 5, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "DewDripperPlant_preview", Assets.GetAnim("brackwood_kanim"), "place", 1, 2), 1, 2);
		return gameObject;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00035616 File Offset: 0x00033816
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00035618 File Offset: 0x00033818
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E8 RID: 1512
	public const string ID = "DewDripperPlant";

	// Token: 0x040005E9 RID: 1513
	public const string SEED_ID = "DewDripperPlantSeed";

	// Token: 0x040005EA RID: 1514
	public const float GROWTH_TIME = 1200f;

	// Token: 0x040005EB RID: 1515
	public const float FERTILIZER_RATE = 0.016666668f;

	// Token: 0x040005EC RID: 1516
	public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 2f;
}
