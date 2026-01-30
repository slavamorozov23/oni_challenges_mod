using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000139 RID: 313
[EntityConfigOrder(4)]
public class LightBugBlueBabyConfig : IEntityConfig
{
	// Token: 0x060005F0 RID: 1520 RVA: 0x0002D268 File Offset: 0x0002B468
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlueConfig.CreateLightBug("LightBugBlueBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlue", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0002D2A6 File Offset: 0x0002B4A6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0002D2A8 File Offset: 0x0002B4A8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400047C RID: 1148
	public const string ID = "LightBugBlueBaby";
}
