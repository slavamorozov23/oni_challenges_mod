using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000175 RID: 373
[EntityConfigOrder(3)]
public class BabyStegoConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600073B RID: 1851 RVA: 0x000323D3 File Offset: 0x000305D3
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x000323DA File Offset: 0x000305DA
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x000323DD File Offset: 0x000305DD
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StegoConfig.CreateStego("StegoBaby", CREATURES.SPECIES.STEGO.BABY.NAME, CREATURES.SPECIES.STEGO.BABY.DESC, "baby_stego_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Stego", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0003241B File Offset: 0x0003061B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0003241D File Offset: 0x0003061D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000585 RID: 1413
	public const string ID = "StegoBaby";
}
