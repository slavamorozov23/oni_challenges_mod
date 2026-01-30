using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class ToePlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600091A RID: 2330 RVA: 0x0003D338 File Offset: 0x0003B538
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0003D33F File Offset: 0x0003B53F
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0003D344 File Offset: 0x0003B544
	public GameObject CreatePrefab()
	{
		string id = "ToePlant";
		string name = STRINGS.CREATURES.SPECIES.TOEPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.TOEPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		KAnimFile anim = Assets.GetAnim("potted_toes_kanim");
		string initialAnim = "grow_seed";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 1;
		EffectorValues decor = positive_DECOR_EFFECT;
		float freezing_ = TUNING.CREATURES.TEMPERATURE.FREEZING_3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, null, freezing_);
		GameObject template = gameObject;
		SimHashes[] safe_elements = new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		};
		EntityTemplates.ExtendEntityToBasicPlant(template, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, safe_elements, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "ToePlantOriginal", STRINGS.CREATURES.SPECIES.TOEPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = ToePlantConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "ToePlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_potted_toes_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.TOEPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "ToePlant_preview", Assets.GetAnim("potted_toes_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0003D4C2 File Offset: 0x0003B6C2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0003D4C4 File Offset: 0x0003B6C4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006C7 RID: 1735
	public const string ID = "ToePlant";

	// Token: 0x040006C8 RID: 1736
	public const string SEED_ID = "ToePlantSeed";

	// Token: 0x040006C9 RID: 1737
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040006CA RID: 1738
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
