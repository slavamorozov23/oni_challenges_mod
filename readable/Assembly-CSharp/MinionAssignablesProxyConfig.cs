using System;
using UnityEngine;

// Token: 0x02000338 RID: 824
public class MinionAssignablesProxyConfig : IEntityConfig
{
	// Token: 0x06001108 RID: 4360 RVA: 0x00065642 File Offset: 0x00063842
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<Ownables>();
		gameObject.AddOrGet<Equipment>();
		gameObject.AddOrGet<MinionAssignablesProxy>();
		return gameObject;
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x00065670 File Offset: 0x00063870
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x00065672 File Offset: 0x00063872
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AEA RID: 2794
	public static string ID = "MinionAssignablesProxy";
}
