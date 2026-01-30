using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000133 RID: 307
[EntityConfigOrder(4)]
public class BabyHatchVeggieConfig : IEntityConfig
{
	// Token: 0x060005CE RID: 1486 RVA: 0x0002C9DE File Offset: 0x0002ABDE
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchVeggieConfig.CreateHatch("HatchVeggieBaby", CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchVeggie", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002CA1C File Offset: 0x0002AC1C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0002CA1E File Offset: 0x0002AC1E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000465 RID: 1125
	public const string ID = "HatchVeggieBaby";
}
