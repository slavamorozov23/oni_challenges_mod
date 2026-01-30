using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000160 RID: 352
[EntityConfigOrder(3)]
public class BabyPuftConfig : IEntityConfig
{
	// Token: 0x060006BA RID: 1722 RVA: 0x000302E6 File Offset: 0x0002E4E6
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftConfig.CreatePuft("PuftBaby", CREATURES.SPECIES.PUFT.BABY.NAME, CREATURES.SPECIES.PUFT.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Puft", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00030324 File Offset: 0x0002E524
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00030326 File Offset: 0x0002E526
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400051D RID: 1309
	public const string ID = "PuftBaby";
}
