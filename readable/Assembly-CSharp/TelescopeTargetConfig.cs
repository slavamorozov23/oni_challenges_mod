using System;
using UnityEngine;

// Token: 0x0200034D RID: 845
public class TelescopeTargetConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001197 RID: 4503 RVA: 0x0006788B File Offset: 0x00065A8B
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x00067892 File Offset: 0x00065A92
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00067895 File Offset: 0x00065A95
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TelescopeTarget", "TelescopeTarget", true);
		gameObject.AddOrGet<TelescopeTarget>();
		return gameObject;
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x000678AE File Offset: 0x00065AAE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x000678B0 File Offset: 0x00065AB0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B14 RID: 2836
	public const string ID = "TelescopeTarget";
}
