using System;
using STRINGS;

// Token: 0x0200002D RID: 45
internal static class BiodieselEngineHelper
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060000D0 RID: 208 RVA: 0x00007128 File Offset: 0x00005328
	public static string ID
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return "BiodieselEngineCluster";
			}
			return "BiodieselEngine";
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000713C File Offset: 0x0000533C
	public static string CODEXID
	{
		get
		{
			return BiodieselEngineHelper.ID.ToUpperInvariant();
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060000D2 RID: 210 RVA: 0x00007148 File Offset: 0x00005348
	public static string NAME
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return BUILDINGS.PREFABS.BIODIESELENGINECLUSTER.NAME;
			}
			return BUILDINGS.PREFABS.BIODIESELENGINE.NAME;
		}
	}
}
