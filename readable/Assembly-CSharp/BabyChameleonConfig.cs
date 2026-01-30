using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000D2 RID: 210
[EntityConfigOrder(3)]
public class BabyChameleonConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060003B9 RID: 953 RVA: 0x000201D5 File Offset: 0x0001E3D5
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060003BA RID: 954 RVA: 0x000201DC File Offset: 0x0001E3DC
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060003BB RID: 955 RVA: 0x000201DF File Offset: 0x0001E3DF
	public GameObject CreatePrefab()
	{
		GameObject gameObject = ChameleonConfig.CreateChameleon("ChameleonBaby", CREATURES.SPECIES.CHAMELEON.BABY.NAME, CREATURES.SPECIES.CHAMELEON.BABY.DESC, "baby_chameleo_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Chameleon", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0002021D File Offset: 0x0001E41D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0002021F File Offset: 0x0001E41F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002E9 RID: 745
	public const string ID = "ChameleonBaby";
}
