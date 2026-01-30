using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class GardenForagePlantPlantedConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000822 RID: 2082 RVA: 0x00036EDC File Offset: 0x000350DC
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00036EE3 File Offset: 0x000350E3
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00036EE8 File Offset: 0x000350E8
	public GameObject CreatePrefab()
	{
		string id = "GardenForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.GARDENFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GARDENFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fatplant_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
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
		gameObject.AddOrGet<SeedProducer>().Configure("GardenForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00036FBF File Offset: 0x000351BF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00036FC1 File Offset: 0x000351C1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000615 RID: 1557
	public const string ID = "GardenForagePlantPlanted";
}
