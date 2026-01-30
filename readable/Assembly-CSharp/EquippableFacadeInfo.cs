using System;
using Database;

// Token: 0x02000586 RID: 1414
public class EquippableFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000AB334 File Offset: 0x000A9534
	// (set) Token: 0x06001F96 RID: 8086 RVA: 0x000AB33C File Offset: 0x000A953C
	public string id { get; set; }

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06001F97 RID: 8087 RVA: 0x000AB345 File Offset: 0x000A9545
	// (set) Token: 0x06001F98 RID: 8088 RVA: 0x000AB34D File Offset: 0x000A954D
	public string name { get; set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000AB356 File Offset: 0x000A9556
	// (set) Token: 0x06001F9A RID: 8090 RVA: 0x000AB35E File Offset: 0x000A955E
	public string desc { get; set; }

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000AB367 File Offset: 0x000A9567
	public PermitRarity rarity
	{
		get
		{
			return this.rarity_;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000AB36F File Offset: 0x000A956F
	// (set) Token: 0x06001F9D RID: 8093 RVA: 0x000AB377 File Offset: 0x000A9577
	public string animFile { get; set; }

	// Token: 0x06001F9E RID: 8094 RVA: 0x000AB380 File Offset: 0x000A9580
	public EquippableFacadeInfo(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity_ = rarity;
		this.defID = defID;
		this.buildOverride = buildOverride;
		this.animFile = animFile;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x000AB3D8 File Offset: 0x000A95D8
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x000AB3E0 File Offset: 0x000A95E0
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x0400127B RID: 4731
	private readonly PermitRarity rarity_;

	// Token: 0x0400127C RID: 4732
	public string buildOverride;

	// Token: 0x0400127D RID: 4733
	public string defID;

	// Token: 0x0400127F RID: 4735
	public string[] requiredDlcIds;

	// Token: 0x04001280 RID: 4736
	public string[] forbiddenDlcIds;
}
