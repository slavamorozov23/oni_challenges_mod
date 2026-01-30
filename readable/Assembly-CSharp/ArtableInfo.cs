using System;
using Database;

// Token: 0x02000581 RID: 1409
public class ArtableInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000AAF5E File Offset: 0x000A915E
	// (set) Token: 0x06001F59 RID: 8025 RVA: 0x000AAF66 File Offset: 0x000A9166
	public string id { get; set; }

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06001F5A RID: 8026 RVA: 0x000AAF6F File Offset: 0x000A916F
	// (set) Token: 0x06001F5B RID: 8027 RVA: 0x000AAF77 File Offset: 0x000A9177
	public string name { get; set; }

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06001F5C RID: 8028 RVA: 0x000AAF80 File Offset: 0x000A9180
	// (set) Token: 0x06001F5D RID: 8029 RVA: 0x000AAF88 File Offset: 0x000A9188
	public string desc { get; set; }

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06001F5E RID: 8030 RVA: 0x000AAF91 File Offset: 0x000A9191
	public PermitRarity rarity
	{
		get
		{
			return this.rarity_;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06001F5F RID: 8031 RVA: 0x000AAF99 File Offset: 0x000A9199
	// (set) Token: 0x06001F60 RID: 8032 RVA: 0x000AAFA1 File Offset: 0x000A91A1
	public string animFile { get; set; }

	// Token: 0x06001F61 RID: 8033 RVA: 0x000AAFAC File Offset: 0x000A91AC
	public ArtableInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname = "", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity_ = rarity;
		this.animFile = animFile;
		this.anim = anim;
		this.decor_value = decor_value;
		this.cheer_on_complete = cheer_on_complete;
		this.status_id = status_id;
		this.prefabId = prefabId;
		this.symbolname = symbolname;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x000AB024 File Offset: 0x000A9224
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001F63 RID: 8035 RVA: 0x000AB02C File Offset: 0x000A922C
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x0400124C RID: 4684
	private readonly PermitRarity rarity_;

	// Token: 0x0400124E RID: 4686
	public string anim;

	// Token: 0x0400124F RID: 4687
	public int decor_value;

	// Token: 0x04001250 RID: 4688
	public bool cheer_on_complete;

	// Token: 0x04001251 RID: 4689
	public string status_id;

	// Token: 0x04001252 RID: 4690
	public string prefabId;

	// Token: 0x04001253 RID: 4691
	public string symbolname;

	// Token: 0x04001254 RID: 4692
	public string[] requiredDlcIds;

	// Token: 0x04001255 RID: 4693
	public string[] forbiddenDlcIds;
}
