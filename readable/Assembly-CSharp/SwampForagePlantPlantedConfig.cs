using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class SwampForagePlantPlantedConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000909 RID: 2313 RVA: 0x0003CE78 File Offset: 0x0003B078
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0003CE7F File Offset: 0x0003B07F
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0003CE84 File Offset: 0x0003B084
	public GameObject CreatePrefab()
	{
		string id = "SwampForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.SWAMPFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swamptuber_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("SwampForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003CF54 File Offset: 0x0003B154
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0003CF56 File Offset: 0x0003B156
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006C0 RID: 1728
	public const string ID = "SwampForagePlantPlanted";
}
