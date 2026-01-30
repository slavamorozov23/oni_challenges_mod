using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200013F RID: 319
[EntityConfigOrder(4)]
public class LightBugOrangeBabyConfig : IEntityConfig
{
	// Token: 0x0600060E RID: 1550 RVA: 0x0002DA68 File Offset: 0x0002BC68
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugOrangeConfig.CreateLightBug("LightBugOrangeBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugOrange", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0002DAA6 File Offset: 0x0002BCA6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002DAA8 File Offset: 0x0002BCA8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000491 RID: 1169
	public const string ID = "LightBugOrangeBaby";
}
