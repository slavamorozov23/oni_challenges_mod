using System;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class TargetLocator : IEntityConfig
{
	// Token: 0x060010DD RID: 4317 RVA: 0x00064F39 File Offset: 0x00063139
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(TargetLocator.ID, TargetLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00064F56 File Offset: 0x00063156
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x00064F58 File Offset: 0x00063158
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AC6 RID: 2758
	public static readonly string ID = "TargetLocator";
}
