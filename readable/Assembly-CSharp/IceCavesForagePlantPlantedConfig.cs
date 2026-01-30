using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class IceCavesForagePlantPlantedConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600084E RID: 2126 RVA: 0x000384E4 File Offset: 0x000366E4
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x000384EB File Offset: 0x000366EB
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x000384F0 File Offset: 0x000366F0
	public GameObject CreatePrefab()
	{
		string id = "IceCavesForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("frozenberries_kanim");
		string initialAnim = "idle";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingBack;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 253.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<DrowningMonitor>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("IceCavesForagePlant", SeedProducer.ProductionType.DigOnly, 2);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x000385F9 File Offset: 0x000367F9
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x000385FB File Offset: 0x000367FB
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000643 RID: 1603
	public const string ID = "IceCavesForagePlantPlanted";
}
