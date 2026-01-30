using System;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class RepairableStorageProxy : IEntityConfig
{
	// Token: 0x0600114A RID: 4426 RVA: 0x000668CE File Offset: 0x00064ACE
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID, true);
		gameObject.AddOrGet<Storage>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x000668F2 File Offset: 0x00064AF2
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x000668F4 File Offset: 0x00064AF4
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AFE RID: 2814
	public static string ID = "RepairableStorageProxy";
}
