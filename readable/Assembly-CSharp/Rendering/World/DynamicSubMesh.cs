using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000EEF RID: 3823
	public class DynamicSubMesh
	{
		// Token: 0x06007AE8 RID: 31464 RVA: 0x002FD980 File Offset: 0x002FBB80
		public DynamicSubMesh(string name, Bounds bounds, int idx_offset)
		{
			this.IdxOffset = idx_offset;
			this.Mesh = new Mesh();
			this.Mesh.name = name;
			this.Mesh.bounds = bounds;
			this.Mesh.MarkDynamic();
		}

		// Token: 0x06007AE9 RID: 31465 RVA: 0x002FD9EC File Offset: 0x002FBBEC
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.Vertices.Length)
			{
				this.Vertices = new Vector3[vertex_count];
				this.UVs = new Vector2[vertex_count];
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.Triangles.Length != triangle_count)
			{
				this.Triangles = new int[triangle_count];
				this.SetTriangles = true;
				return;
			}
			this.SetTriangles = false;
		}

		// Token: 0x06007AEA RID: 31466 RVA: 0x002FDA52 File Offset: 0x002FBC52
		public bool AreTrianglesFull()
		{
			return this.Triangles.Length == this.TriangleIdx;
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x002FDA64 File Offset: 0x002FBC64
		public bool AreVerticesFull()
		{
			return this.Vertices.Length == this.VertexIdx;
		}

		// Token: 0x06007AEC RID: 31468 RVA: 0x002FDA76 File Offset: 0x002FBC76
		public bool AreUVsFull()
		{
			return this.UVs.Length == this.UVIdx;
		}

		// Token: 0x06007AED RID: 31469 RVA: 0x002FDA88 File Offset: 0x002FBC88
		public void Commit()
		{
			if (this.SetTriangles)
			{
				this.Mesh.Clear();
			}
			this.Mesh.vertices = this.Vertices;
			if (this.SetUVs || this.SetTriangles)
			{
				this.Mesh.uv = this.UVs;
			}
			if (this.SetTriangles)
			{
				this.Mesh.triangles = this.Triangles;
			}
			this.VertexIdx = 0;
			this.UVIdx = 0;
			this.TriangleIdx = 0;
		}

		// Token: 0x06007AEE RID: 31470 RVA: 0x002FDB08 File Offset: 0x002FBD08
		public void AddTriangle(int triangle)
		{
			int[] triangles = this.Triangles;
			int triangleIdx = this.TriangleIdx;
			this.TriangleIdx = triangleIdx + 1;
			triangles[triangleIdx] = triangle + this.IdxOffset;
		}

		// Token: 0x06007AEF RID: 31471 RVA: 0x002FDB38 File Offset: 0x002FBD38
		public void AddUV(Vector2 uv)
		{
			Vector2[] uvs = this.UVs;
			int uvidx = this.UVIdx;
			this.UVIdx = uvidx + 1;
			uvs[uvidx] = uv;
		}

		// Token: 0x06007AF0 RID: 31472 RVA: 0x002FDB64 File Offset: 0x002FBD64
		public void AddVertex(Vector3 vertex)
		{
			Vector3[] vertices = this.Vertices;
			int vertexIdx = this.VertexIdx;
			this.VertexIdx = vertexIdx + 1;
			vertices[vertexIdx] = vertex;
		}

		// Token: 0x06007AF1 RID: 31473 RVA: 0x002FDB90 File Offset: 0x002FBD90
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			Graphics.DrawMesh(this.Mesh, position, rotation, material, layer, null, 0, property_block, false, false);
		}

		// Token: 0x040055C5 RID: 21957
		public Vector3[] Vertices = new Vector3[0];

		// Token: 0x040055C6 RID: 21958
		public Vector2[] UVs = new Vector2[0];

		// Token: 0x040055C7 RID: 21959
		public int[] Triangles = new int[0];

		// Token: 0x040055C8 RID: 21960
		public Mesh Mesh;

		// Token: 0x040055C9 RID: 21961
		public bool SetUVs;

		// Token: 0x040055CA RID: 21962
		public bool SetTriangles;

		// Token: 0x040055CB RID: 21963
		private int VertexIdx;

		// Token: 0x040055CC RID: 21964
		private int UVIdx;

		// Token: 0x040055CD RID: 21965
		private int TriangleIdx;

		// Token: 0x040055CE RID: 21966
		private int IdxOffset;
	}
}
