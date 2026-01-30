using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000141 RID: 321
[EntityConfigOrder(4)]
public class LightBugPinkBabyConfig : IEntityConfig
{
	// Token: 0x06000618 RID: 1560 RVA: 0x0002DD30 File Offset: 0x0002BF30
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPinkConfig.CreateLightBug("LightBugPinkBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPink", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0002DD6E File Offset: 0x0002BF6E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0002DD70 File Offset: 0x0002BF70
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000498 RID: 1176
	public const string ID = "LightBugPinkBaby";
}
