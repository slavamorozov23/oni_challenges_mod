using System;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class SleepLocator : IEntityConfig
{
	// Token: 0x060010E7 RID: 4327 RVA: 0x00064FAA File Offset: 0x000631AA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SleepLocator.ID, SleepLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Sleepable>().isNormalBed = false;
		return gameObject;
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x00064FDA File Offset: 0x000631DA
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x00064FDC File Offset: 0x000631DC
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AC8 RID: 2760
	public static readonly string ID = "SleepLocator";
}
