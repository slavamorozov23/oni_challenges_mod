using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class PrickleFlowerConfig : IEntityConfig
{
	// Token: 0x0600087B RID: 2171 RVA: 0x00039364 File Offset: 0x00037564
	public GameObject CreatePrefab()
	{
		string id = "PrickleFlower";
		string name = STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("bristleblossom_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 278.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, PrickleFruitConfig.ID, true, true, true, true, 2400f, 0f, 4600f, "PrickleFlowerOriginal", STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.NAME);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Water,
				massConsumptionRate = 0.033333335f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		DiseaseDropper.Def def = gameObject.AddOrGetDef<DiseaseDropper.Def>();
		def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
		def.singleEmitQuantity = 1000000;
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "PollenGerms";
		Modifiers component = gameObject.GetComponent<Modifiers>();
		Db.Get().traits.Get(component.initialTraits[0]).Add(new AttributeModifier(Db.Get().PlantAttributes.MinLightLux.Id, 200f, STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.NAME, false, false, true));
		component.initialAttributes.Add(Db.Get().PlantAttributes.MinLightLux.Id);
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(false);
		gameObject.AddOrGet<BlightVulnerable>();
		gameObject.AddOrGet<PlantFiberProducer>().amount = 48f;
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "PrickleFlowerSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_bristleblossom_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 2, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "PrickleFlower_preview", Assets.GetAnim("bristleblossom_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_grow", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00039612 File Offset: 0x00037812
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<PrimaryElement>().Temperature = 288.15f;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00039624 File Offset: 0x00037824
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000663 RID: 1635
	public const float WATER_RATE = 0.033333335f;

	// Token: 0x04000664 RID: 1636
	public const string ID = "PrickleFlower";

	// Token: 0x04000665 RID: 1637
	public const string SEED_ID = "PrickleFlowerSeed";

	// Token: 0x04000666 RID: 1638
	public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 48f;
}
