using System;
using Database;

// Token: 0x02000584 RID: 1412
public class BalloonArtistFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000AB1D0 File Offset: 0x000A93D0
	// (set) Token: 0x06001F7D RID: 8061 RVA: 0x000AB1D8 File Offset: 0x000A93D8
	public string id { get; set; }

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06001F7E RID: 8062 RVA: 0x000AB1E1 File Offset: 0x000A93E1
	// (set) Token: 0x06001F7F RID: 8063 RVA: 0x000AB1E9 File Offset: 0x000A93E9
	public string name { get; set; }

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06001F80 RID: 8064 RVA: 0x000AB1F2 File Offset: 0x000A93F2
	// (set) Token: 0x06001F81 RID: 8065 RVA: 0x000AB1FA File Offset: 0x000A93FA
	public string desc { get; set; }

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06001F82 RID: 8066 RVA: 0x000AB203 File Offset: 0x000A9403
	// (set) Token: 0x06001F83 RID: 8067 RVA: 0x000AB20B File Offset: 0x000A940B
	public PermitRarity rarity { get; set; }

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06001F84 RID: 8068 RVA: 0x000AB214 File Offset: 0x000A9414
	// (set) Token: 0x06001F85 RID: 8069 RVA: 0x000AB21C File Offset: 0x000A941C
	public string animFile { get; set; }

	// Token: 0x06001F86 RID: 8070 RVA: 0x000AB228 File Offset: 0x000A9428
	public BalloonArtistFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.balloonFacadeType = balloonFacadeType;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000AB278 File Offset: 0x000A9478
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x000AB280 File Offset: 0x000A9480
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x0400126D RID: 4717
	public BalloonArtistFacadeType balloonFacadeType;

	// Token: 0x0400126E RID: 4718
	public string[] requiredDlcIds;

	// Token: 0x0400126F RID: 4719
	public string[] forbiddenDlcIds;
}
