using System;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class TemporalTearConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600119D RID: 4509 RVA: 0x000678BA File Offset: 0x00065ABA
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x000678C1 File Offset: 0x00065AC1
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x000678C4 File Offset: 0x00065AC4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TemporalTear", "TemporalTear", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<TemporalTear>();
		return gameObject;
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x000678E4 File Offset: 0x00065AE4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x000678E6 File Offset: 0x00065AE6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B15 RID: 2837
	public const string ID = "TemporalTear";
}
