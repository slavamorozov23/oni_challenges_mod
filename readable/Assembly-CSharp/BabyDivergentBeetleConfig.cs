using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000120 RID: 288
[EntityConfigOrder(2)]
public class BabyDivergentBeetleConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000561 RID: 1377 RVA: 0x0002AB26 File Offset: 0x00028D26
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0002AB2D File Offset: 0x00028D2D
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0002AB30 File Offset: 0x00028D30
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetleBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.DESC, "baby_critter_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentBeetle", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0002AB6E File Offset: 0x00028D6E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0002AB70 File Offset: 0x00028D70
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003E9 RID: 1001
	public const string ID = "DivergentBeetleBaby";
}
