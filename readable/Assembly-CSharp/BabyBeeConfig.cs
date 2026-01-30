using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000CF RID: 207
[EntityConfigOrder(3)]
public class BabyBeeConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060003A4 RID: 932 RVA: 0x0001FDB7 File Offset: 0x0001DFB7
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001FDBE File Offset: 0x0001DFBE
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001FDC4 File Offset: 0x0001DFC4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BeeConfig.CreateBee("BeeBaby", CREATURES.SPECIES.BEE.BABY.NAME, CREATURES.SPECIES.BEE.BABY.DESC, "baby_blarva_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Bee", null, true, 2f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		return gameObject;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001FE1E File Offset: 0x0001E01E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001FE20 File Offset: 0x0001E020
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040002DB RID: 731
	public const string ID = "BeeBaby";
}
