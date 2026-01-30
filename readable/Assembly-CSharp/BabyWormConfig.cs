using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000122 RID: 290
[EntityConfigOrder(3)]
public class BabyWormConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600056F RID: 1391 RVA: 0x0002AEC9 File Offset: 0x000290C9
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0002AED0 File Offset: 0x000290D0
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0002AED3 File Offset: 0x000290D3
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentWormConfig.CreateWorm("DivergentWormBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.DESC, "baby_worm_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentWorm", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0002AF11 File Offset: 0x00029111
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0002AF13 File Offset: 0x00029113
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003F9 RID: 1017
	public const string ID = "DivergentWormBaby";
}
