using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000EED RID: 3821
	public class Brush
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06007AD7 RID: 31447 RVA: 0x002FD334 File Offset: 0x002FB534
		// (set) Token: 0x06007AD8 RID: 31448 RVA: 0x002FD33C File Offset: 0x002FB53C
		public int Id { get; private set; }

		// Token: 0x06007AD9 RID: 31449 RVA: 0x002FD348 File Offset: 0x002FB548
		public Brush(int id, string name, Material material, Mask mask, List<Brush> active_brushes, List<Brush> dirty_brushes, int width_in_tiles, MaterialPropertyBlock property_block)
		{
			this.Id = id;
			this.material = material;
			this.mask = mask;
			this.mesh = new DynamicMesh(name, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f)));
			this.activeBrushes = active_brushes;
			this.dirtyBrushes = dirty_brushes;
			this.layer = LayerMask.NameToLayer("World");
			this.widthInTiles = width_in_tiles;
			this.propertyBlock = property_block;
		}

		// Token: 0x06007ADA RID: 31450 RVA: 0x002FD3D6 File Offset: 0x002FB5D6
		public void Add(int tile_idx)
		{
			this.tiles.Add(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x06007ADB RID: 31451 RVA: 0x002FD400 File Offset: 0x002FB600
		public void Remove(int tile_idx)
		{
			this.tiles.Remove(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x06007ADC RID: 31452 RVA: 0x002FD42A File Offset: 0x002FB62A
		public void SetMaskOffset(int offset)
		{
			this.mask.SetOffset(offset);
		}

		// Token: 0x06007ADD RID: 31453 RVA: 0x002FD438 File Offset: 0x002FB638
		public void Refresh()
		{
			bool flag = this.mesh.Meshes.Length != 0;
			int count = this.tiles.Count;
			int vertex_count = count * 4;
			int triangle_count = count * 6;
			this.mesh.Reserve(vertex_count, triangle_count);
			if (this.mesh.SetTriangles)
			{
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					this.mesh.AddTriangle(num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(3 + num);
					num += 4;
				}
			}
			foreach (int num2 in this.tiles)
			{
				float num3 = (float)(num2 % this.widthInTiles);
				float num4 = (float)(num2 / this.widthInTiles);
				float z = 0f;
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 + 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 + 0.5f, z));
			}
			if (this.mesh.SetUVs)
			{
				for (int j = 0; j < count; j++)
				{
					this.mesh.AddUV(this.mask.UV0);
					this.mesh.AddUV(this.mask.UV1);
					this.mesh.AddUV(this.mask.UV2);
					this.mesh.AddUV(this.mask.UV3);
				}
			}
			this.dirty = false;
			this.mesh.Commit();
			if (this.mesh.Meshes.Length != 0)
			{
				if (!flag)
				{
					this.activeBrushes.Add(this);
					return;
				}
			}
			else if (flag)
			{
				this.activeBrushes.Remove(this);
			}
		}

		// Token: 0x06007ADE RID: 31454 RVA: 0x002FD694 File Offset: 0x002FB894
		public void Render()
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Ground));
			this.mesh.Render(position, Quaternion.identity, this.material, this.layer, this.propertyBlock);
		}

		// Token: 0x06007ADF RID: 31455 RVA: 0x002FD6DC File Offset: 0x002FB8DC
		public void SetMaterial(Material material, MaterialPropertyBlock property_block)
		{
			this.material = material;
			this.propertyBlock = property_block;
		}

		// Token: 0x040055AC RID: 21932
		private bool dirty;

		// Token: 0x040055AD RID: 21933
		private Material material;

		// Token: 0x040055AE RID: 21934
		private int layer;

		// Token: 0x040055AF RID: 21935
		private HashSet<int> tiles = new HashSet<int>();

		// Token: 0x040055B0 RID: 21936
		private List<Brush> activeBrushes;

		// Token: 0x040055B1 RID: 21937
		private List<Brush> dirtyBrushes;

		// Token: 0x040055B2 RID: 21938
		private int widthInTiles;

		// Token: 0x040055B3 RID: 21939
		private Mask mask;

		// Token: 0x040055B4 RID: 21940
		private DynamicMesh mesh;

		// Token: 0x040055B5 RID: 21941
		private MaterialPropertyBlock propertyBlock;
	}
}
