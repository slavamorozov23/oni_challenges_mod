using System;
using Database;

// Token: 0x02000580 RID: 1408
public interface IBlueprintInfo : IHasDlcRestrictions
{
	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06001F4F RID: 8015
	// (set) Token: 0x06001F50 RID: 8016
	string id { get; set; }

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06001F51 RID: 8017
	// (set) Token: 0x06001F52 RID: 8018
	string name { get; set; }

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06001F53 RID: 8019
	// (set) Token: 0x06001F54 RID: 8020
	string desc { get; set; }

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06001F55 RID: 8021
	PermitRarity rarity { get; }

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06001F56 RID: 8022
	// (set) Token: 0x06001F57 RID: 8023
	string animFile { get; set; }
}
