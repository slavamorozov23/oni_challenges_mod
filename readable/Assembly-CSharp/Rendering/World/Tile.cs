using System;

namespace Rendering.World
{
	// Token: 0x02000EF3 RID: 3827
	public struct Tile
	{
		// Token: 0x06007B00 RID: 31488 RVA: 0x002FDED5 File Offset: 0x002FC0D5
		public Tile(int idx, int tile_x, int tile_y, int mask_count)
		{
			this.Idx = idx;
			this.TileCells = new TileCells(tile_x, tile_y);
			this.MaskCount = mask_count;
		}

		// Token: 0x040055E6 RID: 21990
		public int Idx;

		// Token: 0x040055E7 RID: 21991
		public TileCells TileCells;

		// Token: 0x040055E8 RID: 21992
		public int MaskCount;
	}
}
