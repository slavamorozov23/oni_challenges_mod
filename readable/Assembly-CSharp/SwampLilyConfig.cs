using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class SwampLilyConfig : IEntityConfig
{
	// Token: 0x06000915 RID: 2325 RVA: 0x0003D124 File Offset: 0x0003B324
	public GameObject CreatePrefab()
	{
		string id = "SwampLily";
		string name = STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPLILY.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swamplily_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 328.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 308.15f, 358.15f, 448.15f, new SimHashes[]
		{
			SimHashes.ChlorineGas
		}, true, 0f, 0.15f, SwampLilyFlowerConfig.ID, true, true, true, true, 2400f, 0f, 4600f, SwampLilyConfig.ID + "Original", STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<PlantFiberProducer>().amount = 24f;
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SwampLilySeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.DESC;
		KAnimFile anim = Assets.GetAnim("seed_swampLily_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.SWAMPLILY.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 21, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), SwampLilyConfig.ID + "_preview", Assets.GetAnim("swamplily_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death_bloom", NOISE_POLLUTION.CREATURES.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, SwampLilyConfig.ID);
		return gameObject;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0003D320 File Offset: 0x0003B520
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0003D322 File Offset: 0x0003B522
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006C4 RID: 1732
	public static string ID = "SwampLily";

	// Token: 0x040006C5 RID: 1733
	public const string SEED_ID = "SwampLilySeed";

	// Token: 0x040006C6 RID: 1734
	public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 24f;
}
