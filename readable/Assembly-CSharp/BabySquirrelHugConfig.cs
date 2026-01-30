using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200016D RID: 365
[EntityConfigOrder(4)]
public class BabySquirrelHugConfig : IEntityConfig
{
	// Token: 0x06000706 RID: 1798 RVA: 0x00031762 File Offset: 0x0002F962
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelHugConfig.CreateSquirrelHug("SquirrelHugBaby", CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.NAME, CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "SquirrelHug", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x000317A0 File Offset: 0x0002F9A0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x000317A2 File Offset: 0x0002F9A2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000560 RID: 1376
	public const string ID = "SquirrelHugBaby";
}
