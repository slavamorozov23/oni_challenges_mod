using System;
using System.Collections.Generic;

namespace Rendering.World
{
	// Token: 0x02000EF4 RID: 3828
	public abstract class TileRenderer : KMonoBehaviour
	{
		// Token: 0x06007B01 RID: 31489 RVA: 0x002FDEF4 File Offset: 0x002FC0F4
		protected override void OnSpawn()
		{
			this.Masks = this.GetMasks();
			this.TileGridWidth = Grid.WidthInCells + 1;
			this.TileGridHeight = Grid.HeightInCells + 1;
			this.BrushGrid = new int[this.TileGridWidth * this.TileGridHeight * 4];
			for (int i = 0; i < this.BrushGrid.Length; i++)
			{
				this.BrushGrid[i] = -1;
			}
			this.TileGrid = new Tile[this.TileGridWidth * this.TileGridHeight];
			for (int j = 0; j < this.TileGrid.Length; j++)
			{
				int tile_x = j % this.TileGridWidth;
				int tile_y = j / this.TileGridWidth;
				this.TileGrid[j] = new Tile(j, tile_x, tile_y, this.Masks.Length);
			}
			this.LoadBrushes();
			this.VisibleAreaUpdater = new VisibleAreaUpdater(new Action<int>(this.UpdateOutsideView), new Action<int>(this.UpdateInsideView), "TileRenderer");
		}

		// Token: 0x06007B02 RID: 31490 RVA: 0x002FDFE4 File Offset: 0x002FC1E4
		protected virtual Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 2, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, true, false),
				new Mask(this.Atlas, 2, false, false, false, false),
				new Mask(this.Atlas, 1, true, false, false, false),
				new Mask(this.Atlas, 3, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, false, false),
				new Mask(this.Atlas, 3, true, false, false, false),
				new Mask(this.Atlas, 1, true, false, true, false),
				new Mask(this.Atlas, 4, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, false, false),
				new Mask(this.Atlas, 4, false, true, false, false),
				new Mask(this.Atlas, 0, false, false, false, true)
			};
		}

		// Token: 0x06007B03 RID: 31491 RVA: 0x002FE170 File Offset: 0x002FC370
		private void UpdateInsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
				this.DirtyTiles.Add(item);
			}
		}

		// Token: 0x06007B04 RID: 31492 RVA: 0x002FE1B4 File Offset: 0x002FC3B4
		private void UpdateOutsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
			}
		}

		// Token: 0x06007B05 RID: 31493 RVA: 0x002FE1E8 File Offset: 0x002FC3E8
		private int[] GetCellTiles(int cell)
		{
			int num = 0;
			int num2 = 0;
			Grid.CellToXY(cell, out num, out num2);
			this.CellTiles[0] = num2 * this.TileGridWidth + num;
			this.CellTiles[1] = num2 * this.TileGridWidth + (num + 1);
			this.CellTiles[2] = (num2 + 1) * this.TileGridWidth + num;
			this.CellTiles[3] = (num2 + 1) * this.TileGridWidth + (num + 1);
			return this.CellTiles;
		}

		// Token: 0x06007B06 RID: 31494
		public abstract void LoadBrushes();

		// Token: 0x06007B07 RID: 31495 RVA: 0x002FE259 File Offset: 0x002FC459
		public void MarkDirty(int cell)
		{
			this.VisibleAreaUpdater.UpdateCell(cell);
		}

		// Token: 0x06007B08 RID: 31496 RVA: 0x002FE268 File Offset: 0x002FC468
		private void LateUpdate()
		{
			foreach (int num in this.ClearTiles)
			{
				this.Clear(ref this.TileGrid[num], this.Brushes, this.BrushGrid);
			}
			this.ClearTiles.Clear();
			foreach (int num2 in this.DirtyTiles)
			{
				this.MarkDirty(ref this.TileGrid[num2], this.Brushes, this.BrushGrid);
			}
			this.DirtyTiles.Clear();
			this.VisibleAreaUpdater.Update();
			foreach (Brush brush in this.DirtyBrushes)
			{
				brush.Refresh();
			}
			this.DirtyBrushes.Clear();
			foreach (Brush brush2 in this.ActiveBrushes)
			{
				brush2.Render();
			}
		}

		// Token: 0x06007B09 RID: 31497
		public abstract void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid);

		// Token: 0x06007B0A RID: 31498 RVA: 0x002FE3D8 File Offset: 0x002FC5D8
		public void Clear(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = tile.Idx * 4 + i;
				if (brush_grid[num] != -1)
				{
					brush_array[brush_grid[num]].Remove(tile.Idx);
				}
			}
		}

		// Token: 0x040055E9 RID: 21993
		private Tile[] TileGrid;

		// Token: 0x040055EA RID: 21994
		private int[] BrushGrid;

		// Token: 0x040055EB RID: 21995
		protected int TileGridWidth;

		// Token: 0x040055EC RID: 21996
		protected int TileGridHeight;

		// Token: 0x040055ED RID: 21997
		private int[] CellTiles = new int[4];

		// Token: 0x040055EE RID: 21998
		protected Brush[] Brushes;

		// Token: 0x040055EF RID: 21999
		protected Mask[] Masks;

		// Token: 0x040055F0 RID: 22000
		protected List<Brush> DirtyBrushes = new List<Brush>();

		// Token: 0x040055F1 RID: 22001
		protected List<Brush> ActiveBrushes = new List<Brush>();

		// Token: 0x040055F2 RID: 22002
		private VisibleAreaUpdater VisibleAreaUpdater;

		// Token: 0x040055F3 RID: 22003
		private HashSet<int> ClearTiles = new HashSet<int>();

		// Token: 0x040055F4 RID: 22004
		private HashSet<int> DirtyTiles = new HashSet<int>();

		// Token: 0x040055F5 RID: 22005
		public TextureAtlas Atlas;
	}
}
