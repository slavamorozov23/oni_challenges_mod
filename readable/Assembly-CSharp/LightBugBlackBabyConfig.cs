using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000137 RID: 311
[EntityConfigOrder(4)]
public class LightBugBlackBabyConfig : IEntityConfig
{
	// Token: 0x060005E6 RID: 1510 RVA: 0x0002CF9C File Offset: 0x0002B19C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlackBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlack", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002CFDA File Offset: 0x0002B1DA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0002CFDC File Offset: 0x0002B1DC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000475 RID: 1141
	public const string ID = "LightBugBlackBaby";
}
