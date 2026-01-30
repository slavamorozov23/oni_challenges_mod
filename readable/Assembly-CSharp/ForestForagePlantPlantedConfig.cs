using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class ForestForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x060007F5 RID: 2037 RVA: 0x000360C8 File Offset: 0x000342C8
	public GameObject CreatePrefab()
	{
		string id = "ForestForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("podmelon_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<DrowningMonitor>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("ForestForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0003619F File Offset: 0x0003439F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x000361A1 File Offset: 0x000343A1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000603 RID: 1539
	public const string ID = "ForestForagePlantPlanted";
}
