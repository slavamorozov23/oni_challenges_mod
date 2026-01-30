using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200012D RID: 301
[EntityConfigOrder(3)]
public class BabyHatchConfig : IEntityConfig
{
	// Token: 0x060005AF RID: 1455 RVA: 0x0002C2AC File Offset: 0x0002A4AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchConfig.CreateHatch("HatchBaby", CREATURES.SPECIES.HATCH.BABY.NAME, CREATURES.SPECIES.HATCH.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Hatch", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0002C2EA File Offset: 0x0002A4EA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0002C2EC File Offset: 0x0002A4EC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400044B RID: 1099
	public const string ID = "HatchBaby";
}
