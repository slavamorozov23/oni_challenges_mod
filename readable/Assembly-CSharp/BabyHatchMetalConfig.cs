using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000131 RID: 305
[EntityConfigOrder(4)]
public class BabyHatchMetalConfig : IEntityConfig
{
	// Token: 0x060005C4 RID: 1476 RVA: 0x0002C77A File Offset: 0x0002A97A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchMetalConfig.CreateHatch("HatchMetalBaby", CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchMetal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0002C7B8 File Offset: 0x0002A9B8
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x0002C7BA File Offset: 0x0002A9BA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400045C RID: 1116
	public const string ID = "HatchMetalBaby";
}
