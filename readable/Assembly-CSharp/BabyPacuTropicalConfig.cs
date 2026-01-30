using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000158 RID: 344
[EntityConfigOrder(4)]
public class BabyPacuTropicalConfig : IEntityConfig
{
	// Token: 0x0600068F RID: 1679 RVA: 0x0002F853 File Offset: 0x0002DA53
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuTropicalConfig.CreatePacu("PacuTropicalBaby", CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuTropical", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002F891 File Offset: 0x0002DA91
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0002F893 File Offset: 0x0002DA93
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F6 RID: 1270
	public const string ID = "PacuTropicalBaby";
}
