using System;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class ApproachableLocator : IEntityConfig
{
	// Token: 0x060010E2 RID: 4322 RVA: 0x00064F6E File Offset: 0x0006316E
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ApproachableLocator.ID, ApproachableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		return gameObject;
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x00064F92 File Offset: 0x00063192
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x00064F94 File Offset: 0x00063194
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AC7 RID: 2759
	public static readonly string ID = "ApproachableLocator";
}
