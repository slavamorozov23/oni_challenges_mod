using System;
using System.Collections.Generic;
using KMod;

namespace Klei
{
	// Token: 0x02001009 RID: 4105
	internal class SaveFileRoot
	{
		// Token: 0x06007F86 RID: 32646 RVA: 0x003352B9 File Offset: 0x003334B9
		public SaveFileRoot()
		{
			this.streamed = new Dictionary<string, byte[]>();
		}

		// Token: 0x04006098 RID: 24728
		public int WidthInCells;

		// Token: 0x04006099 RID: 24729
		public int HeightInCells;

		// Token: 0x0400609A RID: 24730
		public Dictionary<string, byte[]> streamed;

		// Token: 0x0400609B RID: 24731
		public string clusterID;

		// Token: 0x0400609C RID: 24732
		public List<ModInfo> requiredMods;

		// Token: 0x0400609D RID: 24733
		public List<Label> active_mods;
	}
}
