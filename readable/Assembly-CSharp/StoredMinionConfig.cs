using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class StoredMinionConfig : IEntityConfig
{
	// Token: 0x0600118E RID: 4494 RVA: 0x0006779C File Offset: 0x0006599C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(StoredMinionConfig.ID, StoredMinionConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<KPrefabID>();
		gameObject.AddOrGet<Traits>();
		gameObject.AddOrGet<Schedulable>();
		gameObject.AddOrGet<StoredMinionIdentity>();
		gameObject.AddOrGet<KSelectable>().IsSelectable = false;
		gameObject.AddOrGet<MinionModifiers>().addBaseTraits = false;
		return gameObject;
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x000677F4 File Offset: 0x000659F4
	public void OnPrefabInit(GameObject go)
	{
		GameObject prefab = Assets.GetPrefab(BionicMinionConfig.ID);
		if (prefab != null)
		{
			StoredMinionIdentity.IStoredMinionExtension[] components = prefab.GetComponents<StoredMinionIdentity.IStoredMinionExtension>();
			if (components != null)
			{
				for (int i = 0; i < components.Length; i++)
				{
					components[i].AddStoredMinionGameObjectRequirements(go);
				}
			}
		}
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x0006783B File Offset: 0x00065A3B
	public void OnSpawn(GameObject go)
	{
		go.Trigger(1589886948, go);
	}

	// Token: 0x04000B11 RID: 2833
	public static string ID = "StoredMinion";
}
