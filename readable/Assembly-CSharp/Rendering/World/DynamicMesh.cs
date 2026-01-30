using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000EEE RID: 3822
	public class DynamicMesh
	{
		// Token: 0x06007AE0 RID: 31456 RVA: 0x002FD6EC File Offset: 0x002FB8EC
		public DynamicMesh(string name, Bounds bounds)
		{
			this.Name = name;
			this.Bounds = bounds;
		}

		// Token: 0x06007AE1 RID: 31457 RVA: 0x002FD710 File Offset: 0x002FB910
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.VertexCount)
			{
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.TriangleCount != triangle_count)
			{
				this.SetTriangles = true;
			}
			else
			{
				this.SetTriangles = false;
			}
			int num = (int)Mathf.Ceil((float)triangle_count / (float)DynamicMesh.TrianglesPerMesh);
			if (num != this.Meshes.Length)
			{
				this.Meshes = new DynamicSubMesh[num];
				for (int i = 0; i < this.Meshes.Length; i++)
				{
					int idx_offset = -i * DynamicMesh.VerticesPerMesh;
					this.Meshes[i] = new DynamicSubMesh(this.Name, this.Bounds, idx_offset);
				}
				this.SetUVs = true;
				this.SetTriangles = true;
			}
			for (int j = 0; j < this.Meshes.Length; j++)
			{
				if (j == this.Meshes.Length - 1)
				{
					this.Meshes[j].Reserve(vertex_count % DynamicMesh.VerticesPerMesh, triangle_count % DynamicMesh.TrianglesPerMesh);
				}
				else
				{
					this.Meshes[j].Reserve(DynamicMesh.VerticesPerMesh, DynamicMesh.TrianglesPerMesh);
				}
			}
			this.VertexCount = vertex_count;
			this.TriangleCount = triangle_count;
		}

		// Token: 0x06007AE2 RID: 31458 RVA: 0x002FD81C File Offset: 0x002FBA1C
		public void Commit()
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Commit();
			}
			this.TriangleMeshIdx = 0;
			this.UVMeshIdx = 0;
			this.VertexMeshIdx = 0;
		}

		// Token: 0x06007AE3 RID: 31459 RVA: 0x002FD85C File Offset: 0x002FBA5C
		public void AddTriangle(int triangle)
		{
			if (this.Meshes[this.TriangleMeshIdx].AreTrianglesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.TriangleMeshIdx + 1;
				this.TriangleMeshIdx = num;
				object obj = meshes[num];
			}
			this.Meshes[this.TriangleMeshIdx].AddTriangle(triangle);
		}

		// Token: 0x06007AE4 RID: 31460 RVA: 0x002FD8AC File Offset: 0x002FBAAC
		public void AddUV(Vector2 uv)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.UVMeshIdx];
			if (dynamicSubMesh.AreUVsFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.UVMeshIdx + 1;
				this.UVMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddUV(uv);
		}

		// Token: 0x06007AE5 RID: 31461 RVA: 0x002FD8F0 File Offset: 0x002FBAF0
		public void AddVertex(Vector3 vertex)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.VertexMeshIdx];
			if (dynamicSubMesh.AreVerticesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.VertexMeshIdx + 1;
				this.VertexMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddVertex(vertex);
		}

		// Token: 0x06007AE6 RID: 31462 RVA: 0x002FD934 File Offset: 0x002FBB34
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Render(position, rotation, material, layer, property_block);
			}
		}

		// Token: 0x040055B6 RID: 21942
		private static int TrianglesPerMesh = 65004;

		// Token: 0x040055B7 RID: 21943
		private static int VerticesPerMesh = 4 * DynamicMesh.TrianglesPerMesh / 6;

		// Token: 0x040055B8 RID: 21944
		public bool SetUVs;

		// Token: 0x040055B9 RID: 21945
		public bool SetTriangles;

		// Token: 0x040055BA RID: 21946
		public string Name;

		// Token: 0x040055BB RID: 21947
		public Bounds Bounds;

		// Token: 0x040055BC RID: 21948
		public DynamicSubMesh[] Meshes = new DynamicSubMesh[0];

		// Token: 0x040055BD RID: 21949
		private int VertexCount;

		// Token: 0x040055BE RID: 21950
		private int TriangleCount;

		// Token: 0x040055BF RID: 21951
		private int VertexIdx;

		// Token: 0x040055C0 RID: 21952
		private int UVIdx;

		// Token: 0x040055C1 RID: 21953
		private int TriangleIdx;

		// Token: 0x040055C2 RID: 21954
		private int TriangleMeshIdx;

		// Token: 0x040055C3 RID: 21955
		private int VertexMeshIdx;

		// Token: 0x040055C4 RID: 21956
		private int UVMeshIdx;
	}
}
