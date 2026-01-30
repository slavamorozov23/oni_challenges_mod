using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BD RID: 445
public class SuperWormPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060008FC RID: 2300 RVA: 0x0003CCE6 File Offset: 0x0003AEE6
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0003CCED File Offset: 0x0003AEED
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0003CCF0 File Offset: 0x0003AEF0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("SuperWormPlant", STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.NAME, STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.DESC, "wormwood_kanim", SuperWormPlantConfig.SUPER_DECOR, "WormSuperFruit");
		gameObject.AddOrGet<SeedProducer>().Configure("WormPlantSeed", SeedProducer.ProductionType.Harvest, 1);
		gameObject.AddOrGet<PlantFiberProducer>().amount = 16f;
		return gameObject;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0003CD4C File Offset: 0x0003AF4C
	public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.SubscribeToTransformEvent(GameHashes.HarvestComplete);
		transformingPlant.transformPlantId = "WormPlant";
		prefab.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("flower", false);
		prefab.AddOrGet<StandardCropPlant>().anims = SuperWormPlantConfig.animSet;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0003CD9A File Offset: 0x0003AF9A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006B9 RID: 1721
	public const string ID = "SuperWormPlant";

	// Token: 0x040006BA RID: 1722
	public static readonly EffectorValues SUPER_DECOR = DECOR.BONUS.TIER1;

	// Token: 0x040006BB RID: 1723
	public const string SUPER_CROP_ID = "WormSuperFruit";

	// Token: 0x040006BC RID: 1724
	public const int CROP_YIELD = 8;

	// Token: 0x040006BD RID: 1725
	public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 16f;

	// Token: 0x040006BE RID: 1726
	private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "super_grow",
		grow_pst = "super_grow_pst",
		idle_full = "super_idle_full",
		wilt_base = "super_wilt",
		harvest = "super_harvest"
	};
}
