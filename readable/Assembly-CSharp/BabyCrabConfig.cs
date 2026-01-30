using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000119 RID: 281
[EntityConfigOrder(3)]
public class BabyCrabConfig : IEntityConfig
{
	// Token: 0x06000539 RID: 1337 RVA: 0x0002A034 File Offset: 0x00028234
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("CrabBaby", CREATURES.SPECIES.CRAB.BABY.NAME, CREATURES.SPECIES.CRAB.BABY.DESC, "baby_pincher_kanim", true, "CrabShell", 5f);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Crab", "CrabShell", false, 5f);
		gameObject.AddOrGetDef<BabyMonitor.Def>().onGrowDropUnits = 5f;
		return gameObject;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0002A09B File Offset: 0x0002829B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0002A09D File Offset: 0x0002829D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003C7 RID: 967
	public const string ID = "CrabBaby";
}
