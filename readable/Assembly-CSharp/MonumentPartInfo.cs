using System;
using Database;

// Token: 0x02000587 RID: 1415
public class MonumentPartInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x000AB3E8 File Offset: 0x000A95E8
	// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x000AB3F0 File Offset: 0x000A95F0
	public string id { get; set; }

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x000AB3F9 File Offset: 0x000A95F9
	// (set) Token: 0x06001FA4 RID: 8100 RVA: 0x000AB401 File Offset: 0x000A9601
	public string name { get; set; }

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x000AB40A File Offset: 0x000A960A
	// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x000AB412 File Offset: 0x000A9612
	public string desc { get; set; }

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x000AB41B File Offset: 0x000A961B
	public PermitRarity rarity
	{
		get
		{
			return this.rarity_;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000AB423 File Offset: 0x000A9623
	// (set) Token: 0x06001FA9 RID: 8105 RVA: 0x000AB42B File Offset: 0x000A962B
	public string animFile { get; set; }

	// Token: 0x06001FAA RID: 8106 RVA: 0x000AB434 File Offset: 0x000A9634
	public MonumentPartInfo(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity_ = rarity;
		this.animFile = animFilename;
		this.state = state;
		this.symbolName = symbolName;
		this.part = part;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x000AB494 File Offset: 0x000A9694
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x000AB49C File Offset: 0x000A969C
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001284 RID: 4740
	private readonly PermitRarity rarity_;

	// Token: 0x04001286 RID: 4742
	public string state;

	// Token: 0x04001287 RID: 4743
	public string symbolName;

	// Token: 0x04001288 RID: 4744
	public MonumentPartResource.Part part;

	// Token: 0x04001289 RID: 4745
	public string[] requiredDlcIds;

	// Token: 0x0400128A RID: 4746
	public string[] forbiddenDlcIds;
}
