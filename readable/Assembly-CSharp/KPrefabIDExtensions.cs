using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009D8 RID: 2520
public static class KPrefabIDExtensions
{
	// Token: 0x06004932 RID: 18738 RVA: 0x001A7E3E File Offset: 0x001A603E
	public static Tag PrefabID(this Component cmp)
	{
		return cmp.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06004933 RID: 18739 RVA: 0x001A7E4B File Offset: 0x001A604B
	public static Tag PrefabID(this GameObject go)
	{
		return go.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06004934 RID: 18740 RVA: 0x001A7E58 File Offset: 0x001A6058
	public static Tag PrefabID(this StateMachine.Instance smi)
	{
		return smi.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06004935 RID: 18741 RVA: 0x001A7E65 File Offset: 0x001A6065
	public static bool IsPrefabID(this Component cmp, Tag id)
	{
		return cmp.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06004936 RID: 18742 RVA: 0x001A7E73 File Offset: 0x001A6073
	public static bool IsPrefabID(this GameObject go, Tag id)
	{
		return go.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06004937 RID: 18743 RVA: 0x001A7E81 File Offset: 0x001A6081
	public static bool HasTag(this Component cmp, Tag tag)
	{
		return cmp.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06004938 RID: 18744 RVA: 0x001A7E8F File Offset: 0x001A608F
	public static bool HasTag(this GameObject go, Tag tag)
	{
		return go.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06004939 RID: 18745 RVA: 0x001A7E9D File Offset: 0x001A609D
	public static bool HasAnyTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x0600493A RID: 18746 RVA: 0x001A7EAB File Offset: 0x001A60AB
	public static bool HasAnyTags(this Component cmp, List<Tag> tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x0600493B RID: 18747 RVA: 0x001A7EB9 File Offset: 0x001A60B9
	public static bool HasAnyTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x0600493C RID: 18748 RVA: 0x001A7EC7 File Offset: 0x001A60C7
	public static bool HasAnyTags(this GameObject go, List<Tag> tags)
	{
		return go.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x0600493D RID: 18749 RVA: 0x001A7ED5 File Offset: 0x001A60D5
	public static bool HasAllTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x0600493E RID: 18750 RVA: 0x001A7EE3 File Offset: 0x001A60E3
	public static bool HasAllTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x0600493F RID: 18751 RVA: 0x001A7EF1 File Offset: 0x001A60F1
	public static void AddTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06004940 RID: 18752 RVA: 0x001A7F00 File Offset: 0x001A6100
	public static void AddTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06004941 RID: 18753 RVA: 0x001A7F0F File Offset: 0x001A610F
	public static void RemoveTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().RemoveTag(tag);
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x001A7F1D File Offset: 0x001A611D
	public static void RemoveTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().RemoveTag(tag);
	}
}
