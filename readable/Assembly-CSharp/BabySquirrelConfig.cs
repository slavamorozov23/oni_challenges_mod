using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200016B RID: 363
[EntityConfigOrder(3)]
public class BabySquirrelConfig : IEntityConfig
{
	// Token: 0x060006FC RID: 1788 RVA: 0x00031502 File Offset: 0x0002F702
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelConfig.CreateSquirrel("SquirrelBaby", CREATURES.SPECIES.SQUIRREL.BABY.NAME, CREATURES.SPECIES.SQUIRREL.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Squirrel", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00031540 File Offset: 0x0002F740
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00031542 File Offset: 0x0002F742
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000554 RID: 1364
	public const string ID = "SquirrelBaby";
}
