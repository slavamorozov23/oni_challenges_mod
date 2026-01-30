using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class DinofernConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007D3 RID: 2003 RVA: 0x00035622 File Offset: 0x00033822
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00035629 File Offset: 0x00033829
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0003562C File Offset: 0x0003382C
	public GameObject CreatePrefab()
	{
		string id = "Dinofern";
		string name = STRINGS.CREATURES.SPECIES.DINOFERN.NAME;
		string desc = STRINGS.CREATURES.SPECIES.DINOFERN.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject template = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("prehistoric_fern_kanim"), "idle_full", Grid.SceneLayer.BuildingBack, 3, 3, tier, default(EffectorValues), SimHashes.Creature, null, 253.15f);
		float temperature_lethal_low = 218.15f;
		float temperature_warning_low = 228.15f;
		float temperature_warning_high = 288.15f;
		float temperature_lethal_high = 308.15f;
		string text = FernFoodConfig.ID;
		GameObject gameObject = EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.ChlorineGas
		}, true, 0f, 0.5f, text, true, false, true, true, 2400f, 0f, 2200f, "DinofernOriginal", STRINGS.CREATURES.SPECIES.DINOFERN.NAME);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<Dinofern>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.capacityKg = 1f;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.ChlorineGas;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 4;
		elementConsumer.EnableConsumption(false);
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.09f;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "DinofernSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.DINOFERN.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.DINOFERN.DESC;
		KAnimFile anim = Assets.GetAnim("seed_megafrond_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.DINOFERN.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, this, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 20, text, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "Dinofern_preview", Assets.GetAnim("prehistoric_fern_kanim"), "place", 3, 3);
		SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0003582C File Offset: 0x00033A2C
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<StandardCropPlant>().anims = new StandardCropPlant.AnimSet
		{
			pre_grow = "expand",
			grow = "grow",
			grow_pst = "grow_pst",
			idle_full = "idle_full",
			wilt_base = "wilt",
			harvest = "harvest",
			waning = "waning"
		};
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00035896 File Offset: 0x00033A96
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Dinofern>().SetConsumptionRate();
	}

	// Token: 0x040005ED RID: 1517
	public const string ID = "Dinofern";

	// Token: 0x040005EE RID: 1518
	public const string SEED_ID = "DinofernSeed";

	// Token: 0x040005EF RID: 1519
	public const float CHLORINE_CONSUMPTION_RATE = 0.09f;
}
