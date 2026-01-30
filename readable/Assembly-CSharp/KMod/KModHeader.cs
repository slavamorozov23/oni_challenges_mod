using System;

namespace KMod
{
	// Token: 0x02000FB9 RID: 4025
	public class KModHeader
	{
		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06007E59 RID: 32345 RVA: 0x00326CE0 File Offset: 0x00324EE0
		// (set) Token: 0x06007E5A RID: 32346 RVA: 0x00326CE8 File Offset: 0x00324EE8
		public string staticID { get; set; }

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06007E5B RID: 32347 RVA: 0x00326CF1 File Offset: 0x00324EF1
		// (set) Token: 0x06007E5C RID: 32348 RVA: 0x00326CF9 File Offset: 0x00324EF9
		public string title { get; set; }

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06007E5D RID: 32349 RVA: 0x00326D02 File Offset: 0x00324F02
		// (set) Token: 0x06007E5E RID: 32350 RVA: 0x00326D0A File Offset: 0x00324F0A
		public string description { get; set; }
	}
}
