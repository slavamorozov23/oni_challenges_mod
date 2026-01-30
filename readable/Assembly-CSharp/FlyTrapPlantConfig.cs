using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class FlyTrapPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007EA RID: 2026 RVA: 0x00035E12 File Offset: 0x00034012
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00035E19 File Offset: 0x00034019
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00035E1C File Offset: 0x0003401C
	public GameObject CreatePrefab()
	{
		string id = "FlyTrapPlant";
		string name = STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("ceiling_carnie_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 291.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 283.15f, 328.15f, 348.15f, null, true, 0f, 0.15f, SimHashes.Amber.ToString(), true, true, true, true, 2400f, 0f, 7400f, "FlyTrapPlantOriginal", STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.NAME);
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<FlytrapConsumptionMonitor>();
		gameObject.AddOrGet<Growing>().MaxMaturityValuePercentageToSpawnWith = 0f;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "FlyTrapPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.FLYTRAPPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.FLYTRAPPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_ceiling_carnie_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.DOMESTICATEDDESC;
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 4, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "FlyTrapPlant_preview", Assets.GetAnim("ceiling_carnie_kanim"), "place", 1, 2), 1, 2);
		return gameObject;
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00035FCF File Offset: 0x000341CF
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<StandardCropPlant>().anims = FlyTrapPlantConfig.Default_StandardCropAnimSet;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00035FE1 File Offset: 0x000341E1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FC RID: 1532
	public const string ID = "FlyTrapPlant";

	// Token: 0x040005FD RID: 1533
	public const string SEED_ID = "FlyTrapPlantSeed";

	// Token: 0x040005FE RID: 1534
	public static readonly StandardCropPlant.AnimSet Default_StandardCropAnimSet = new StandardCropPlant.AnimSet
	{
		pre_grow = "grow_pre",
		grow = "grow",
		grow_pst = "grow_pst",
		idle_full = "idle_full",
		wilt_base = "wilt",
		harvest = "harvest",
		waning = "waning",
		grow_playmode = KAnim.PlayMode.Paused
	};

	// Token: 0x040005FF RID: 1535
	public const int DIGESTION_DURATION_CYCLES = 12;

	// Token: 0x04000600 RID: 1536
	public const float DIGESTION_DURATION = 7200f;

	// Token: 0x04000601 RID: 1537
	public const int AMBER_PER_HARVEST_KG = 264;
}
