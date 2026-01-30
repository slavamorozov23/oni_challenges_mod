using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200016F RID: 367
[EntityConfigOrder(3)]
public class BabyStaterpillarConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000712 RID: 1810 RVA: 0x000319F1 File Offset: 0x0002FBF1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x000319F8 File Offset: 0x0002FBF8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x000319FB File Offset: 0x0002FBFB
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarConfig.CreateStaterpillar("StaterpillarBaby", CREATURES.SPECIES.STATERPILLAR.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Staterpillar", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00031A39 File Offset: 0x0002FC39
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00031A3B File Offset: 0x0002FC3B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000567 RID: 1383
	public const string ID = "StaterpillarBaby";
}
