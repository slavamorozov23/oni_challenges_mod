using System;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public static class KSelectableExtensions
{
	// Token: 0x0600236F RID: 9071 RVA: 0x000CCF4B File Offset: 0x000CB14B
	public static string GetProperName(this Component cmp)
	{
		if (cmp != null && cmp.gameObject != null)
		{
			return cmp.gameObject.GetProperName();
		}
		return "";
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000CCF78 File Offset: 0x000CB178
	public static string GetProperName(this GameObject go)
	{
		if (go != null)
		{
			KSelectable component = go.GetComponent<KSelectable>();
			if (component != null)
			{
				return component.GetName();
			}
		}
		return "";
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000CCFAA File Offset: 0x000CB1AA
	public static string GetProperName(this KSelectable cmp)
	{
		if (cmp != null)
		{
			return cmp.GetName();
		}
		return "";
	}
}
