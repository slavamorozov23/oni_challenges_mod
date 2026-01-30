using System;

namespace Klei
{
	// Token: 0x02001007 RID: 4103
	public struct SolidInfo
	{
		// Token: 0x06007F83 RID: 32643 RVA: 0x00335246 File Offset: 0x00333446
		public SolidInfo(int cellIdx, bool isSolid)
		{
			this.cellIdx = cellIdx;
			this.isSolid = isSolid;
		}

		// Token: 0x04006095 RID: 24725
		public int cellIdx;

		// Token: 0x04006096 RID: 24726
		public bool isSolid;
	}
}
