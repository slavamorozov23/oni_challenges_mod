using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class CylindricaConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007BF RID: 1983 RVA: 0x000351A3 File Offset: 0x000333A3
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x000351AA File Offset: 0x000333AA
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x000351B0 File Offset: 0x000333B0
	public GameObject CreatePrefab()
	{
		string id = "Cylindrica";
		string name = STRINGS.CREATURES.SPECIES.CYLINDRICA.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CYLINDRICA.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = CylindricaConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cylindricafan_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288.15f, 293.15f, 323.15f, 373.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "CylindricaOriginal", STRINGS.CREATURES.SPECIES.CYLINDRICA.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = CylindricaConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = CylindricaConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "CylindricaSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CYLINDRICA.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CYLINDRICA.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_cylindricafan_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.CYLINDRICA.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "Cylindrica_preview", Assets.GetAnim("potted_cylindricafan_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00035327 File Offset: 0x00033527
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00035329 File Offset: 0x00033529
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E2 RID: 1506
	public const string ID = "Cylindrica";

	// Token: 0x040005E3 RID: 1507
	public const string SEED_ID = "CylindricaSeed";

	// Token: 0x040005E4 RID: 1508
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040005E5 RID: 1509
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
