using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011B RID: 283
[EntityConfigOrder(4)]
public class BabyCrabFreshWaterConfig : IEntityConfig
{
	// Token: 0x06000543 RID: 1347 RVA: 0x0002A40C File Offset: 0x0002860C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWaterBaby", CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.DESC, "baby_pincher_kanim", true, "ShellfishMeat", 4);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabFreshWater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002A45B File Offset: 0x0002865B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0002A45D File Offset: 0x0002865D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003D1 RID: 977
	public const string ID = "CrabFreshWaterBaby";
}
