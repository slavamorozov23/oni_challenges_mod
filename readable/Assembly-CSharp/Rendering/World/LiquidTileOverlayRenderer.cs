using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000EF5 RID: 3829
	public class LiquidTileOverlayRenderer : TileRenderer
	{
		// Token: 0x06007B0C RID: 31500 RVA: 0x002FE452 File Offset: 0x002FC652
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x002FE46C File Offset: 0x002FC66C
		protected override Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 0, false, true, false, false),
				new Mask(this.Atlas, 1, false, false, false, false)
			};
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x002FE4C4 File Offset: 0x002FC6C4
		public void OnShadersReloaded()
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (element.IsLiquid && element.substance != null && element.substance.material != null)
				{
					Material material = new Material(element.substance.material);
					this.InitAlphaMaterial(material, element);
					int idx = element.substance.idx;
					for (int i = 0; i < this.Masks.Length; i++)
					{
						int num = idx * this.Masks.Length + i;
						element.substance.RefreshPropertyBlock();
						this.Brushes[num].SetMaterial(material, element.substance.propertyBlock);
					}
				}
			}
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x002FE5B0 File Offset: 0x002FC7B0
		public override void LoadBrushes()
		{
			this.Brushes = new Brush[ElementLoader.elements.Count * this.Masks.Length];
			foreach (Element element in ElementLoader.elements)
			{
				if (element.IsLiquid && element.substance != null && element.substance.material != null)
				{
					Material material = new Material(element.substance.material);
					this.InitAlphaMaterial(material, element);
					int idx = element.substance.idx;
					for (int i = 0; i < this.Masks.Length; i++)
					{
						int num = idx * this.Masks.Length + i;
						element.substance.RefreshPropertyBlock();
						this.Brushes[num] = new Brush(num, element.id.ToString(), material, this.Masks[i], this.ActiveBrushes, this.DirtyBrushes, this.TileGridWidth, element.substance.propertyBlock);
					}
				}
			}
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x002FE6F0 File Offset: 0x002FC8F0
		private void InitAlphaMaterial(Material alpha_material, Element element)
		{
			alpha_material.name = element.name;
			alpha_material.renderQueue = RenderQueues.BlockTiles + element.substance.idx;
			alpha_material.EnableKeyword("ALPHA");
			alpha_material.DisableKeyword("OPAQUE");
			alpha_material.SetTexture("_AlphaTestMap", this.Atlas.texture);
			alpha_material.SetInt("_SrcAlpha", 5);
			alpha_material.SetInt("_DstAlpha", 10);
			alpha_material.SetInt("_ZWrite", 0);
			alpha_material.SetColor("_Colour", element.substance.colour);
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x002FE78C File Offset: 0x002FC98C
		private bool RenderLiquid(int cell, int cell_above)
		{
			bool result = false;
			if (Grid.Element[cell].IsSolid)
			{
				Element element = Grid.Element[cell_above];
				if (element.IsLiquid && element.substance.material != null)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06007B12 RID: 31506 RVA: 0x002FE7D0 File Offset: 0x002FC9D0
		private void SetBrushIdx(int i, ref Tile tile, int substance_idx, LiquidTileOverlayRenderer.LiquidConnections connections, Brush[] brush_array, int[] brush_grid)
		{
			if (connections == LiquidTileOverlayRenderer.LiquidConnections.Empty)
			{
				brush_grid[tile.Idx * 4 + i] = -1;
				return;
			}
			Brush brush = brush_array[substance_idx * tile.MaskCount + connections - LiquidTileOverlayRenderer.LiquidConnections.Left];
			brush.Add(tile.Idx);
			brush_grid[tile.Idx * 4 + i] = brush.Id;
		}

		// Token: 0x06007B13 RID: 31507 RVA: 0x002FE828 File Offset: 0x002FCA28
		public override void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			if (!this.RenderLiquid(tile.TileCells.Cell0, tile.TileCells.Cell2))
			{
				if (this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
				{
					this.SetBrushIdx(1, ref tile, Grid.Element[tile.TileCells.Cell3].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Right, brush_array, brush_grid);
				}
				return;
			}
			if (this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
			{
				this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Both, brush_array, brush_grid);
				return;
			}
			this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Left, brush_array, brush_grid);
		}

		// Token: 0x0200217D RID: 8573
		private enum LiquidConnections
		{
			// Token: 0x04009973 RID: 39283
			Left = 1,
			// Token: 0x04009974 RID: 39284
			Right,
			// Token: 0x04009975 RID: 39285
			Both,
			// Token: 0x04009976 RID: 39286
			Empty = 128
		}
	}
}
