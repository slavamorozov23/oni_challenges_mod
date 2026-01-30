using System;
using UnityEngine;

// Token: 0x02000AE1 RID: 2785
public class FullScreenQuad
{
	// Token: 0x060050F2 RID: 20722 RVA: 0x001D563C File Offset: 0x001D383C
	public FullScreenQuad(string name, Camera camera, bool invert = false)
	{
		this.Camera = camera;
		this.Layer = LayerMask.NameToLayer("ForceDraw");
		this.Mesh = new Mesh();
		this.Mesh.name = name;
		this.Mesh.vertices = new Vector3[]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f)
		};
		float y = 1f;
		float y2 = 0f;
		if (invert)
		{
			y = 0f;
			y2 = 1f;
		}
		this.Mesh.uv = new Vector2[]
		{
			new Vector2(0f, y2),
			new Vector2(0f, y),
			new Vector2(1f, y2),
			new Vector2(1f, y)
		};
		this.Mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		this.Mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
		this.Material = new Material(Shader.Find("Klei/PostFX/FullScreen"));
		this.Camera.cullingMask = (this.Camera.cullingMask | LayerMask.GetMask(new string[]
		{
			"ForceDraw"
		}));
	}

	// Token: 0x060050F3 RID: 20723 RVA: 0x001D57F4 File Offset: 0x001D39F4
	public void Draw(Texture texture)
	{
		this.Material.mainTexture = texture;
		Graphics.DrawMesh(this.Mesh, Vector3.zero, Quaternion.identity, this.Material, this.Layer, this.Camera, 0, null, false, false);
	}

	// Token: 0x040035FD RID: 13821
	private Mesh Mesh;

	// Token: 0x040035FE RID: 13822
	private Camera Camera;

	// Token: 0x040035FF RID: 13823
	private Material Material;

	// Token: 0x04003600 RID: 13824
	private int Layer;
}
