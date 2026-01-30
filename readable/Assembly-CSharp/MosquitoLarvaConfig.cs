using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200014B RID: 331
[EntityConfigOrder(3)]
public class MosquitoLarvaConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600064F RID: 1615 RVA: 0x0002EC1E File Offset: 0x0002CE1E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0002EC25 File Offset: 0x0002CE25
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0002EC28 File Offset: 0x0002CE28
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MosquitoConfig.CreateMosquito("MosquitoBaby", CREATURES.SPECIES.MOSQUITO.BABY.NAME, CREATURES.SPECIES.MOSQUITO.BABY.DESC, "baby_mosquito_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Mosquito", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0002EC66 File Offset: 0x0002CE66
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0002EC68 File Offset: 0x0002CE68
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C5 RID: 1221
	public const string ID = "MosquitoBaby";
}
