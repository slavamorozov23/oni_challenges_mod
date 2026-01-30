using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000168 RID: 360
[EntityConfigOrder(3)]
public class BabySealConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060006EC RID: 1772 RVA: 0x000311C1 File Offset: 0x0002F3C1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x000311C8 File Offset: 0x0002F3C8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x000311CB File Offset: 0x0002F3CB
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SealConfig.CreateSeal("SealBaby", CREATURES.SPECIES.SEAL.BABY.NAME, CREATURES.SPECIES.SEAL.BABY.DESC, "baby_seal_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Seal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00031209 File Offset: 0x0002F409
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0003120B File Offset: 0x0002F40B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000547 RID: 1351
	public const string ID = "SealBaby";
}
