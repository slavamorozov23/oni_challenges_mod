using System;
using Database;

// Token: 0x02000585 RID: 1413
public class StickerBombFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06001F89 RID: 8073 RVA: 0x000AB288 File Offset: 0x000A9488
	// (set) Token: 0x06001F8A RID: 8074 RVA: 0x000AB290 File Offset: 0x000A9490
	public string id { get; set; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06001F8B RID: 8075 RVA: 0x000AB299 File Offset: 0x000A9499
	// (set) Token: 0x06001F8C RID: 8076 RVA: 0x000AB2A1 File Offset: 0x000A94A1
	public string name { get; set; }

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06001F8D RID: 8077 RVA: 0x000AB2AA File Offset: 0x000A94AA
	// (set) Token: 0x06001F8E RID: 8078 RVA: 0x000AB2B2 File Offset: 0x000A94B2
	public string desc { get; set; }

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000AB2BB File Offset: 0x000A94BB
	public PermitRarity rarity
	{
		get
		{
			return this.rarity_;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06001F90 RID: 8080 RVA: 0x000AB2C3 File Offset: 0x000A94C3
	// (set) Token: 0x06001F91 RID: 8081 RVA: 0x000AB2CB File Offset: 0x000A94CB
	public string animFile { get; set; }

	// Token: 0x06001F92 RID: 8082 RVA: 0x000AB2D4 File Offset: 0x000A94D4
	public StickerBombFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string sticker, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity_ = rarity;
		this.animFile = animFile;
		this.sticker = sticker;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x000AB324 File Offset: 0x000A9524
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x000AB32C File Offset: 0x000A952C
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001273 RID: 4723
	private readonly PermitRarity rarity_;

	// Token: 0x04001275 RID: 4725
	public string sticker;

	// Token: 0x04001276 RID: 4726
	public string[] requiredDlcIds;

	// Token: 0x04001277 RID: 4727
	public string[] forbiddenDlcIds;
}
