using System;
using System.Collections.Generic;

// Token: 0x02000B1C RID: 2844
public class RoleSlotUnlock
{
	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06005310 RID: 21264 RVA: 0x001E3B63 File Offset: 0x001E1D63
	// (set) Token: 0x06005311 RID: 21265 RVA: 0x001E3B6B File Offset: 0x001E1D6B
	public string id { get; protected set; }

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06005312 RID: 21266 RVA: 0x001E3B74 File Offset: 0x001E1D74
	// (set) Token: 0x06005313 RID: 21267 RVA: 0x001E3B7C File Offset: 0x001E1D7C
	public string name { get; protected set; }

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06005314 RID: 21268 RVA: 0x001E3B85 File Offset: 0x001E1D85
	// (set) Token: 0x06005315 RID: 21269 RVA: 0x001E3B8D File Offset: 0x001E1D8D
	public string description { get; protected set; }

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06005316 RID: 21270 RVA: 0x001E3B96 File Offset: 0x001E1D96
	// (set) Token: 0x06005317 RID: 21271 RVA: 0x001E3B9E File Offset: 0x001E1D9E
	public List<global::Tuple<string, int>> slots { get; protected set; }

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06005318 RID: 21272 RVA: 0x001E3BA7 File Offset: 0x001E1DA7
	// (set) Token: 0x06005319 RID: 21273 RVA: 0x001E3BAF File Offset: 0x001E1DAF
	public Func<bool> isSatisfied { get; protected set; }

	// Token: 0x0600531A RID: 21274 RVA: 0x001E3BB8 File Offset: 0x001E1DB8
	public RoleSlotUnlock(string id, string name, string description, List<global::Tuple<string, int>> slots, Func<bool> isSatisfied)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.slots = slots;
		this.isSatisfied = isSatisfied;
	}
}
