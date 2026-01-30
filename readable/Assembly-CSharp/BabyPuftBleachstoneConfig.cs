using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200015E RID: 350
[EntityConfigOrder(4)]
public class BabyPuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x060006B0 RID: 1712 RVA: 0x00030064 File Offset: 0x0002E264
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstoneBaby", CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftBleachstone", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x000300A2 File Offset: 0x0002E2A2
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x000300A4 File Offset: 0x0002E2A4
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000511 RID: 1297
	public const string ID = "PuftBleachstoneBaby";
}
