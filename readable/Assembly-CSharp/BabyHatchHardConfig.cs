using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200012F RID: 303
[EntityConfigOrder(4)]
public class BabyHatchHardConfig : IEntityConfig
{
	// Token: 0x060005B9 RID: 1465 RVA: 0x0002C512 File Offset: 0x0002A712
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchHardConfig.CreateHatch("HatchHardBaby", CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchHard", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0002C550 File Offset: 0x0002A750
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0002C552 File Offset: 0x0002A752
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000454 RID: 1108
	public const string ID = "HatchHardBaby";
}
