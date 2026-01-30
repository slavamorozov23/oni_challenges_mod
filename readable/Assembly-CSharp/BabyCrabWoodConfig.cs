using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011D RID: 285
[EntityConfigOrder(4)]
public class BabyCrabWoodConfig : IEntityConfig
{
	// Token: 0x0600054D RID: 1357 RVA: 0x0002A6CC File Offset: 0x000288CC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWoodBaby", CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.DESC, "baby_pincher_kanim", true, "CrabWoodShell", 10f);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabWood", "CrabWoodShell", false, 5f);
		gameObject.AddOrGetDef<BabyMonitor.Def>().onGrowDropUnits = 50f;
		return gameObject;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0002A733 File Offset: 0x00028933
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0002A735 File Offset: 0x00028935
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003DB RID: 987
	public const string ID = "CrabWoodBaby";
}
