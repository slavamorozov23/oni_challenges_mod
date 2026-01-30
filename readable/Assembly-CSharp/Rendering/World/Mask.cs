using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000EF1 RID: 3825
	public struct Mask
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06007AF2 RID: 31474 RVA: 0x002FDBB3 File Offset: 0x002FBDB3
		// (set) Token: 0x06007AF3 RID: 31475 RVA: 0x002FDBBB File Offset: 0x002FBDBB
		public Vector2 UV0 { readonly get; private set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06007AF4 RID: 31476 RVA: 0x002FDBC4 File Offset: 0x002FBDC4
		// (set) Token: 0x06007AF5 RID: 31477 RVA: 0x002FDBCC File Offset: 0x002FBDCC
		public Vector2 UV1 { readonly get; private set; }

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06007AF6 RID: 31478 RVA: 0x002FDBD5 File Offset: 0x002FBDD5
		// (set) Token: 0x06007AF7 RID: 31479 RVA: 0x002FDBDD File Offset: 0x002FBDDD
		public Vector2 UV2 { readonly get; private set; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06007AF8 RID: 31480 RVA: 0x002FDBE6 File Offset: 0x002FBDE6
		// (set) Token: 0x06007AF9 RID: 31481 RVA: 0x002FDBEE File Offset: 0x002FBDEE
		public Vector2 UV3 { readonly get; private set; }

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06007AFA RID: 31482 RVA: 0x002FDBF7 File Offset: 0x002FBDF7
		// (set) Token: 0x06007AFB RID: 31483 RVA: 0x002FDBFF File Offset: 0x002FBDFF
		public bool IsOpaque { readonly get; private set; }

		// Token: 0x06007AFC RID: 31484 RVA: 0x002FDC08 File Offset: 0x002FBE08
		public Mask(TextureAtlas atlas, int texture_idx, bool transpose, bool flip_x, bool flip_y, bool is_opaque)
		{
			this = default(Mask);
			this.atlas = atlas;
			this.texture_idx = texture_idx;
			this.transpose = transpose;
			this.flip_x = flip_x;
			this.flip_y = flip_y;
			this.atlas_offset = 0;
			this.IsOpaque = is_opaque;
			this.Refresh();
		}

		// Token: 0x06007AFD RID: 31485 RVA: 0x002FDC56 File Offset: 0x002FBE56
		public void SetOffset(int offset)
		{
			this.atlas_offset = offset;
			this.Refresh();
		}

		// Token: 0x06007AFE RID: 31486 RVA: 0x002FDC68 File Offset: 0x002FBE68
		public void Refresh()
		{
			int num = this.atlas_offset * 4 + this.atlas_offset;
			if (num + this.texture_idx >= this.atlas.items.Length)
			{
				num = 0;
			}
			Vector4 uvBox = this.atlas.items[num + this.texture_idx].uvBox;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			if (this.transpose)
			{
				float x = uvBox.x;
				float x2 = uvBox.z;
				if (this.flip_x)
				{
					x = uvBox.z;
					x2 = uvBox.x;
				}
				zero.x = x;
				zero2.x = x;
				zero3.x = x2;
				zero4.x = x2;
				float y = uvBox.y;
				float y2 = uvBox.w;
				if (this.flip_y)
				{
					y = uvBox.w;
					y2 = uvBox.y;
				}
				zero.y = y;
				zero2.y = y2;
				zero3.y = y;
				zero4.y = y2;
			}
			else
			{
				float x3 = uvBox.x;
				float x4 = uvBox.z;
				if (this.flip_x)
				{
					x3 = uvBox.z;
					x4 = uvBox.x;
				}
				zero.x = x3;
				zero2.x = x4;
				zero3.x = x3;
				zero4.x = x4;
				float y3 = uvBox.y;
				float y4 = uvBox.w;
				if (this.flip_y)
				{
					y3 = uvBox.w;
					y4 = uvBox.y;
				}
				zero.y = y4;
				zero2.y = y4;
				zero3.y = y3;
				zero4.y = y3;
			}
			this.UV0 = zero;
			this.UV1 = zero2;
			this.UV2 = zero3;
			this.UV3 = zero4;
		}

		// Token: 0x040055DB RID: 21979
		private TextureAtlas atlas;

		// Token: 0x040055DC RID: 21980
		private int texture_idx;

		// Token: 0x040055DD RID: 21981
		private bool transpose;

		// Token: 0x040055DE RID: 21982
		private bool flip_x;

		// Token: 0x040055DF RID: 21983
		private bool flip_y;

		// Token: 0x040055E0 RID: 21984
		private int atlas_offset;

		// Token: 0x040055E1 RID: 21985
		private const int TILES_PER_SET = 4;
	}
}
