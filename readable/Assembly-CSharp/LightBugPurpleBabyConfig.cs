using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000143 RID: 323
[EntityConfigOrder(4)]
public class LightBugPurpleBabyConfig : IEntityConfig
{
	// Token: 0x06000622 RID: 1570 RVA: 0x0002DFE8 File Offset: 0x0002C1E8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurpleBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPurple", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002E026 File Offset: 0x0002C226
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002E028 File Offset: 0x0002C228
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400049F RID: 1183
	public const string ID = "LightBugPurpleBaby";
}
