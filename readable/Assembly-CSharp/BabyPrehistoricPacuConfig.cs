using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200015A RID: 346
[EntityConfigOrder(3)]
public class BabyPrehistoricPacuConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600069A RID: 1690 RVA: 0x0002FA7C File Offset: 0x0002DC7C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0002FA83 File Offset: 0x0002DC83
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0002FA86 File Offset: 0x0002DC86
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PrehistoricPacuConfig.CreatePrehistoricPacu("PrehistoricPacuBaby", CREATURES.SPECIES.PREHISTORICPACU.BABY.NAME, CREATURES.SPECIES.PREHISTORICPACU.BABY.DESC, "baby_paculacanth_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PrehistoricPacu", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0002FAC4 File Offset: 0x0002DCC4
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0002FAC6 File Offset: 0x0002DCC6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004FB RID: 1275
	public const string ID = "PrehistoricPacuBaby";
}
