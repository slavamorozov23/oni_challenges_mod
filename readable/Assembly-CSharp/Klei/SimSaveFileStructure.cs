using System;

namespace Klei
{
	// Token: 0x0200100D RID: 4109
	public class SimSaveFileStructure
	{
		// Token: 0x06007F8A RID: 32650 RVA: 0x0033535D File Offset: 0x0033355D
		public SimSaveFileStructure()
		{
			this.worldDetail = new WorldDetailSave();
		}

		// Token: 0x040060B5 RID: 24757
		public int WidthInCells;

		// Token: 0x040060B6 RID: 24758
		public int HeightInCells;

		// Token: 0x040060B7 RID: 24759
		public int x;

		// Token: 0x040060B8 RID: 24760
		public int y;

		// Token: 0x040060B9 RID: 24761
		public byte[] Sim;

		// Token: 0x040060BA RID: 24762
		public WorldDetailSave worldDetail;
	}
}
