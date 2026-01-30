using System;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class ResearchDestinationConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600114F RID: 4431 RVA: 0x0006690A File Offset: 0x00064B0A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x00066911 File Offset: 0x00064B11
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x00066914 File Offset: 0x00064B14
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ResearchDestination", "ResearchDestination", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ResearchDestination>();
		return gameObject;
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x00066934 File Offset: 0x00064B34
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x00066936 File Offset: 0x00064B36
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AFF RID: 2815
	public const string ID = "ResearchDestination";
}
