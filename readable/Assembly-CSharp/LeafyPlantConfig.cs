using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class LeafyPlantConfig : IEntityConfig
{
	// Token: 0x06000861 RID: 2145 RVA: 0x000389D4 File Offset: 0x00036BD4
	public GameObject CreatePrefab()
	{
		string id = "LeafyPlant";
		string name = STRINGS.CREATURES.SPECIES.LEAFYPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.LEAFYPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_leafy_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288f, 293.15f, 323.15f, 373f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.ChlorineGas,
			SimHashes.Hydrogen
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "LeafyPlantOriginal", STRINGS.CREATURES.SPECIES.LEAFYPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "LeafyPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.LEAFYPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.LEAFYPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_leafy_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.LEAFYPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, null, "", false), "LeafyPlant_preview", Assets.GetAnim("potted_leafy_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00038B53 File Offset: 0x00036D53
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00038B55 File Offset: 0x00036D55
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000652 RID: 1618
	public const string ID = "LeafyPlant";

	// Token: 0x04000653 RID: 1619
	public const string SEED_ID = "LeafyPlantSeed";

	// Token: 0x04000654 RID: 1620
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x04000655 RID: 1621
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
