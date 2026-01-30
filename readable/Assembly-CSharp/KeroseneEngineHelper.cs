using System;
using STRINGS;

// Token: 0x02000284 RID: 644
internal static class KeroseneEngineHelper
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0004DDD9 File Offset: 0x0004BFD9
	public static string ID
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return "KeroseneEngineCluster";
			}
			return "KeroseneEngine";
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0004DDED File Offset: 0x0004BFED
	public static string CODEXID
	{
		get
		{
			return KeroseneEngineHelper.ID.ToUpperInvariant();
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0004DDF9 File Offset: 0x0004BFF9
	public static string NAME
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return BUILDINGS.PREFABS.KEROSENEENGINECLUSTER.NAME;
			}
			return BUILDINGS.PREFABS.KEROSENEENGINE.NAME;
		}
	}
}
