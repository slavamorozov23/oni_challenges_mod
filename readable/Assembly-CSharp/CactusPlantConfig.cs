using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class CactusPlantConfig : IEntityConfig
{
	// Token: 0x060007A1 RID: 1953 RVA: 0x00034540 File Offset: 0x00032740
	public GameObject CreatePrefab()
	{
		string id = "CactusPlant";
		string name = STRINGS.CREATURES.SPECIES.CACTUSPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CACTUSPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cactus_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 200f, 273.15f, 373.15f, 400f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, false, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "CactusPlantOriginal", STRINGS.CREATURES.SPECIES.CACTUSPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "CactusPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_cactus_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.CACTUSPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 13, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "CactusPlant_preview", Assets.GetAnim("potted_cactus_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x000346BF File Offset: 0x000328BF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x000346C1 File Offset: 0x000328C1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005C4 RID: 1476
	public const string ID = "CactusPlant";

	// Token: 0x040005C5 RID: 1477
	public const string SEED_ID = "CactusPlantSeed";

	// Token: 0x040005C6 RID: 1478
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040005C7 RID: 1479
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
