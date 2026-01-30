using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class BasicForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x06000785 RID: 1925 RVA: 0x00033A50 File Offset: 0x00031C50
	public GameObject CreatePrefab()
	{
		string id = "BasicForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.BASICFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BASICFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("muckroot_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
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
		gameObject.AddOrGet<SeedProducer>().Configure("BasicForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00033B27 File Offset: 0x00031D27
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00033B29 File Offset: 0x00031D29
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005B0 RID: 1456
	public const string ID = "BasicForagePlantPlanted";
}
