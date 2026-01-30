using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class BulbPlantConfig : IEntityConfig
{
	// Token: 0x06000797 RID: 1943 RVA: 0x00034140 File Offset: 0x00032340
	public GameObject CreatePrefab()
	{
		string id = "BulbPlant";
		string name = STRINGS.CREATURES.SPECIES.BULBPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BULBPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_bulb_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288f, 293.15f, 313.15f, 333.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "BulbPlantOriginal", STRINGS.CREATURES.SPECIES.BULBPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		gameObject.AddOrGetDef<DecorPlantMonitor.Def>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "BulbPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_potted_bulb_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.DecorSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.BULBPLANT.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 12, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, null, "", false), "BulbPlant_preview", Assets.GetAnim("potted_bulb_kanim"), "place", 1, 1);
		DiseaseDropper.Def def = gameObject.AddOrGetDef<DiseaseDropper.Def>();
		def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
		def.singleEmitQuantity = 0;
		def.averageEmitPerSecond = 5000;
		def.emitFrequency = 5f;
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "PollenGerms";
		return gameObject;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0003431A File Offset: 0x0003251A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0003431C File Offset: 0x0003251C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005BE RID: 1470
	public const string ID = "BulbPlant";

	// Token: 0x040005BF RID: 1471
	public const string SEED_ID = "BulbPlantSeed";

	// Token: 0x040005C0 RID: 1472
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER1;

	// Token: 0x040005C1 RID: 1473
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
