using System;

namespace Rendering.World
{
	// Token: 0x02000EF2 RID: 3826
	public struct TileCells
	{
		// Token: 0x06007AFF RID: 31487 RVA: 0x002FDE34 File Offset: 0x002FC034
		public TileCells(int tile_x, int tile_y)
		{
			int val = Grid.WidthInCells - 1;
			int val2 = Grid.HeightInCells - 1;
			this.Cell0 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell1 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell2 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(tile_y, val2));
			this.Cell3 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(tile_y, val2));
		}

		// Token: 0x040055E2 RID: 21986
		public int Cell0;

		// Token: 0x040055E3 RID: 21987
		public int Cell1;

		// Token: 0x040055E4 RID: 21988
		public int Cell2;

		// Token: 0x040055E5 RID: 21989
		public int Cell3;
	}
}
