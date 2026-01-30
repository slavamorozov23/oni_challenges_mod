using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000278 RID: 632
public abstract class DatabankHelper
{
	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0004C9C9 File Offset: 0x0004ABC9
	public static string ID
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return "OrbitalResearchDatabank";
			}
			return "ResearchDatabank";
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0004C9DD File Offset: 0x0004ABDD
	public static Tag TAG
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return OrbitalResearchDatabankConfig.TAG;
			}
			return ResearchDatabankConfig.TAG;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0004C9F1 File Offset: 0x0004ABF1
	public static string RESEARCH_NAME
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return RESEARCH.TYPES.ORBITAL.NAME;
			}
			return RESEARCH.TYPES.GAMMA.NAME;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0004CA0F File Offset: 0x0004AC0F
	public static string RESEARCH_CODEXID
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return "RESEARCHDLC1";
			}
			return "RESEARCH";
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0004CA23 File Offset: 0x0004AC23
	public static string NAME
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME;
			}
			return ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0004CA41 File Offset: 0x0004AC41
	public static string NAME_PLURAL
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME_PLURAL;
			}
			return ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME_PLURAL;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0004CA5F File Offset: 0x0004AC5F
	public static string DESC
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.DESC;
			}
			return ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.DESC;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0004CA7D File Offset: 0x0004AC7D
	public static Sprite SPRITE
	{
		get
		{
			return Assets.GetSprite("ui_databank");
		}
	}

	// Token: 0x040008E1 RID: 2273
	public const string CODEXID = "Databank";
}
