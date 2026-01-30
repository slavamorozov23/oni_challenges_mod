using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200013D RID: 317
[EntityConfigOrder(4)]
public class LightBugCrystalBabyConfig : IEntityConfig
{
	// Token: 0x06000604 RID: 1540 RVA: 0x0002D7C0 File Offset: 0x0002B9C0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugCrystalConfig.CreateLightBug("LightBugCrystalBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugCrystal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0002D7FE File Offset: 0x0002B9FE
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002D800 File Offset: 0x0002BA00
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400048A RID: 1162
	public const string ID = "LightBugCrystalBaby";
}
