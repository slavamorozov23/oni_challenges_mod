using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x0200100B RID: 4107
	public class WorldGenSave
	{
		// Token: 0x06007F88 RID: 32648 RVA: 0x00335337 File Offset: 0x00333537
		public WorldGenSave()
		{
			this.data = new Data();
		}

		// Token: 0x040060AB RID: 24747
		public Vector2I version;

		// Token: 0x040060AC RID: 24748
		public Data data;

		// Token: 0x040060AD RID: 24749
		public string worldID;

		// Token: 0x040060AE RID: 24750
		public List<string> traitIDs;

		// Token: 0x040060AF RID: 24751
		public List<string> storyTraitIDs;
	}
}
