using System;
using UnityEngine;

// Token: 0x02000964 RID: 2404
public static class GameTagExtensions
{
	// Token: 0x0600437C RID: 17276 RVA: 0x0018373F File Offset: 0x0018193F
	public static GameObject Prefab(this Tag tag)
	{
		return Assets.GetPrefab(tag);
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x00183747 File Offset: 0x00181947
	public static string ProperName(this Tag tag)
	{
		return TagManager.GetProperName(tag, false);
	}

	// Token: 0x0600437E RID: 17278 RVA: 0x00183750 File Offset: 0x00181950
	public static string ProperNameStripLink(this Tag tag)
	{
		return TagManager.GetProperName(tag, true);
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x00183759 File Offset: 0x00181959
	public static Tag Create(SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}

	// Token: 0x06004380 RID: 17280 RVA: 0x0018376D File Offset: 0x0018196D
	public static Tag CreateTag(this SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}
}
