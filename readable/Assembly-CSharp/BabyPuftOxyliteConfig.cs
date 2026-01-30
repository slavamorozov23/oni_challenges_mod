using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000162 RID: 354
[EntityConfigOrder(4)]
public class BabyPuftOxyliteConfig : IEntityConfig
{
	// Token: 0x060006C4 RID: 1732 RVA: 0x00030570 File Offset: 0x0002E770
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftOxyliteConfig.CreatePuftOxylite("PuftOxyliteBaby", CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftOxylite", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x000305AE File Offset: 0x0002E7AE
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x000305B0 File Offset: 0x0002E7B0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000527 RID: 1319
	public const string ID = "PuftOxyliteBaby";
}
