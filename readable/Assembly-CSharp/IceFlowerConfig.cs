using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class IceFlowerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000854 RID: 2132 RVA: 0x00038605 File Offset: 0x00036805
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0003860C File Offset: 0x0003680C
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00038610 File Offset: 0x00036810
	public GameObject CreatePrefab()
	{
		string id = "IceFlower";
		string name = STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.ICEFLOWER.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_ice_flower_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 243.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 203.15f, 278.15f, 318.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.ChlorineGas,
			SimHashes.Hydrogen
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "IceFlowerOriginal", STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "IceFlowerSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_ice_flower_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.ICEFLOWER.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, null, "", false), "IceFlower_preview", Assets.GetAnim("potted_ice_flower_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0003878A File Offset: 0x0003698A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0003878C File Offset: 0x0003698C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000644 RID: 1604
	public const string ID = "IceFlower";

	// Token: 0x04000645 RID: 1605
	public const string SEED_ID = "IceFlowerSeed";

	// Token: 0x04000646 RID: 1606
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x04000647 RID: 1607
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
