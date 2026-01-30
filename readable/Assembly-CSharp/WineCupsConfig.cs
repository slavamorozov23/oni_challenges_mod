using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class WineCupsConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000939 RID: 2361 RVA: 0x0003E021 File Offset: 0x0003C221
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0003E028 File Offset: 0x0003C228
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0003E02C File Offset: 0x0003C22C
	public GameObject CreatePrefab()
	{
		string id = "WineCups";
		string name = STRINGS.CREATURES.SPECIES.WINECUPS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.WINECUPS.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = WineCupsConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cups_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 900f, "WineCupsOriginal", STRINGS.CREATURES.SPECIES.WINECUPS.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = WineCupsConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = WineCupsConfig.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "WineCupsSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_cups_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.WINECUPS.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 11, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "WineCups_preview", Assets.GetAnim("potted_cups_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0003E1A3 File Offset: 0x0003C3A3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0003E1A5 File Offset: 0x0003C3A5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006E6 RID: 1766
	public const string ID = "WineCups";

	// Token: 0x040006E7 RID: 1767
	public const string SEED_ID = "WineCupsSeed";

	// Token: 0x040006E8 RID: 1768
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040006E9 RID: 1769
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
