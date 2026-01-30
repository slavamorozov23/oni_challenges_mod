using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000153 RID: 339
[EntityConfigOrder(4)]
public class BabyPacuCleanerConfig : IEntityConfig
{
	// Token: 0x06000679 RID: 1657 RVA: 0x0002F5B8 File Offset: 0x0002D7B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuCleanerConfig.CreatePacu("PacuCleanerBaby", CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuCleaner", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0002F5F6 File Offset: 0x0002D7F6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0002F5F8 File Offset: 0x0002D7F8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004EA RID: 1258
	public const string ID = "PacuCleanerBaby";
}
