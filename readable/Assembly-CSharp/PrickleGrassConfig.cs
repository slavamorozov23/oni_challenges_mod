using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class PrickleGrassConfig : IEntityConfig
{
	// Token: 0x0600087F RID: 2175 RVA: 0x00039630 File Offset: 0x00037830
	public GameObject CreatePrefab()
	{
		string id = "PrickleGrass";
		string name = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = PrickleGrassConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("bristlebriar_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 900f, "PrickleGrassOriginal", STRINGS.CREATURES.SPECIES.PRICKLEGRASS.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = PrickleGrassConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = PrickleGrassConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "PrickleGrassSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.DESC;
		KAnimFile anim = Assets.GetAnim("seed_bristlebriar_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 10, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "PrickleGrass_preview", Assets.GetAnim("bristlebriar_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x000397AC File Offset: 0x000379AC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x000397AE File Offset: 0x000379AE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000667 RID: 1639
	public const string ID = "PrickleGrass";

	// Token: 0x04000668 RID: 1640
	public const string SEED_ID = "PrickleGrassSeed";

	// Token: 0x04000669 RID: 1641
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x0400066A RID: 1642
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
