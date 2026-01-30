using System;
using Database;

// Token: 0x02000582 RID: 1410
public class ClothingItemInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06001F64 RID: 8036 RVA: 0x000AB034 File Offset: 0x000A9234
	// (set) Token: 0x06001F65 RID: 8037 RVA: 0x000AB03C File Offset: 0x000A923C
	public string id { get; set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06001F66 RID: 8038 RVA: 0x000AB045 File Offset: 0x000A9245
	// (set) Token: 0x06001F67 RID: 8039 RVA: 0x000AB04D File Offset: 0x000A924D
	public string name { get; set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06001F68 RID: 8040 RVA: 0x000AB056 File Offset: 0x000A9256
	// (set) Token: 0x06001F69 RID: 8041 RVA: 0x000AB05E File Offset: 0x000A925E
	public string desc { get; set; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06001F6A RID: 8042 RVA: 0x000AB067 File Offset: 0x000A9267
	public PermitRarity rarity
	{
		get
		{
			return this.rarity_;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06001F6B RID: 8043 RVA: 0x000AB06F File Offset: 0x000A926F
	// (set) Token: 0x06001F6C RID: 8044 RVA: 0x000AB077 File Offset: 0x000A9277
	public string animFile { get; set; }

	// Token: 0x06001F6D RID: 8045 RVA: 0x000AB080 File Offset: 0x000A9280
	public ClothingItemInfo(string id, string name, string desc, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		Option<ClothingOutfitUtility.OutfitType> outfitTypeFor = PermitCategories.GetOutfitTypeFor(category);
		if (outfitTypeFor.IsNone())
		{
			throw new Exception(string.Format("Expected permit category {0} on ClothingItemResource \"{1}\" to have an {2} but none found.", category, id, "OutfitType"));
		}
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.outfitType = outfitTypeFor.Unwrap();
		this.category = category;
		this.rarity_ = rarity;
		this.animFile = animFile;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000AB10B File Offset: 0x000A930B
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x000AB113 File Offset: 0x000A9313
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001259 RID: 4697
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x0400125A RID: 4698
	public PermitCategory category;

	// Token: 0x0400125B RID: 4699
	private readonly PermitRarity rarity_;

	// Token: 0x0400125D RID: 4701
	private string[] requiredDlcIds;

	// Token: 0x0400125E RID: 4702
	private string[] forbiddenDlcIds;
}
