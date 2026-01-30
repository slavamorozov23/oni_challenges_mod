using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class GardenDecorPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600080A RID: 2058 RVA: 0x00036A5E File Offset: 0x00034C5E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00036A65 File Offset: 0x00034C65
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00036A68 File Offset: 0x00034C68
	public GameObject CreatePrefab()
	{
		string id = "GardenDecorPlant";
		string name = STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("discplant_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 263.15f, 268.15f, 313.15f, 323.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, false, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "GardenDecorPlantOriginal", STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = DECOR.BONUS.TIER3;
		prickleGrass.negative_decor_effect = DECOR.PENALTY.TIER3;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "GardenDecorPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.GARDENDECORPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.GARDENDECORPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_discplant_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 13, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "GardenDecorPlant_preview", Assets.GetAnim("discplant_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00036BDF File Offset: 0x00034DDF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00036BE1 File Offset: 0x00034DE1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400060F RID: 1551
	public const string ID = "GardenDecorPlant";

	// Token: 0x04000610 RID: 1552
	public const string SEED_ID = "GardenDecorPlantSeed";
}
