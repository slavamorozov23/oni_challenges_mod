using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000156 RID: 342
[EntityConfigOrder(3)]
public class BabyPacuConfig : IEntityConfig
{
	// Token: 0x06000685 RID: 1669 RVA: 0x0002F712 File Offset: 0x0002D912
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuConfig.CreatePacu("PacuBaby", CREATURES.SPECIES.PACU.BABY.NAME, CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Pacu", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0002F750 File Offset: 0x0002D950
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0002F752 File Offset: 0x0002D952
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F0 RID: 1264
	public const string ID = "PacuBaby";
}
