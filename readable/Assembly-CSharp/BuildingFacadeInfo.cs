using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;

// Token: 0x02000583 RID: 1411
[DebuggerDisplay("{id} - {name}")]
public class BuildingFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06001F70 RID: 8048 RVA: 0x000AB11B File Offset: 0x000A931B
	// (set) Token: 0x06001F71 RID: 8049 RVA: 0x000AB123 File Offset: 0x000A9323
	public string id { get; set; }

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06001F72 RID: 8050 RVA: 0x000AB12C File Offset: 0x000A932C
	// (set) Token: 0x06001F73 RID: 8051 RVA: 0x000AB134 File Offset: 0x000A9334
	public string name { get; set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06001F74 RID: 8052 RVA: 0x000AB13D File Offset: 0x000A933D
	// (set) Token: 0x06001F75 RID: 8053 RVA: 0x000AB145 File Offset: 0x000A9345
	public string desc { get; set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06001F76 RID: 8054 RVA: 0x000AB14E File Offset: 0x000A934E
	public PermitRarity rarity
	{
		get
		{
			return this.rarity_;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06001F77 RID: 8055 RVA: 0x000AB156 File Offset: 0x000A9356
	// (set) Token: 0x06001F78 RID: 8056 RVA: 0x000AB15E File Offset: 0x000A935E
	public string animFile { get; set; }

	// Token: 0x06001F79 RID: 8057 RVA: 0x000AB168 File Offset: 0x000A9368
	public BuildingFacadeInfo(string id, string name, string desc, PermitRarity rarity, string prefabId, string animFile, Dictionary<string, string> workables = null, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity_ = rarity;
		this.prefabId = prefabId;
		this.animFile = animFile;
		this.workables = workables;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x000AB1C0 File Offset: 0x000A93C0
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000AB1C8 File Offset: 0x000A93C8
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001262 RID: 4706
	private readonly PermitRarity rarity_;

	// Token: 0x04001263 RID: 4707
	public string prefabId;

	// Token: 0x04001265 RID: 4709
	public Dictionary<string, string> workables;

	// Token: 0x04001266 RID: 4710
	public string[] requiredDlcIds;

	// Token: 0x04001267 RID: 4711
	public string[] forbiddenDlcIds;
}
