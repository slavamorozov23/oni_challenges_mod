using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200013B RID: 315
[EntityConfigOrder(3)]
public class LightBugBabyConfig : IEntityConfig
{
	// Token: 0x060005FA RID: 1530 RVA: 0x0002D523 File Offset: 0x0002B723
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugConfig.CreateLightBug("LightBugBaby", CREATURES.SPECIES.LIGHTBUG.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBug", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0002D561 File Offset: 0x0002B761
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0002D563 File Offset: 0x0002B763
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000483 RID: 1155
	public const string ID = "LightBugBaby";
}
