using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public class PrioritizableRenderer
{
	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x0600255D RID: 9565 RVA: 0x000D65F3 File Offset: 0x000D47F3
	// (set) Token: 0x0600255E RID: 9566 RVA: 0x000D65FB File Offset: 0x000D47FB
	public PrioritizeTool currentTool
	{
		get
		{
			return this.tool;
		}
		set
		{
			this.tool = value;
		}
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000D6604 File Offset: 0x000D4804
	public PrioritizableRenderer()
	{
		this.layer = LayerMask.NameToLayer("UI");
		Shader shader = Shader.Find("Klei/Prioritizable");
		Texture2D texture = Assets.GetTexture("priority_overlay_atlas");
		this.material = new Material(shader);
		this.material.SetTexture(Shader.PropertyToID("_MainTex"), texture);
		this.prioritizables = new List<Prioritizable>();
		this.mesh = new Mesh();
		this.mesh.name = "Prioritizables";
		this.mesh.MarkDynamic();
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x000D6690 File Offset: 0x000D4890
	public void Cleanup()
	{
		this.material = null;
		this.vertices = null;
		this.uvs = null;
		this.prioritizables = null;
		this.triangles = null;
		UnityEngine.Object.DestroyImmediate(this.mesh);
		this.mesh = null;
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x000D66C8 File Offset: 0x000D48C8
	private static Util.IterationInstruction renderEveryTickVisitHelper(object obj, PrioritizableRenderer self)
	{
		Prioritizable prioritizable = (Prioritizable)obj;
		if (prioritizable != null && prioritizable.showIcon && prioritizable.IsPrioritizable() && self.tool.IsActiveLayer(self.tool.GetFilterLayerFromGameObject(prioritizable.gameObject)) && prioritizable.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
		{
			self.prioritizables.Add(prioritizable);
		}
		return Util.IterationInstruction.Continue;
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x000D6734 File Offset: 0x000D4934
	public void RenderEveryTick()
	{
		if (GameScreenManager.Instance == null)
		{
			return;
		}
		if (SimDebugView.Instance == null || SimDebugView.Instance.GetMode() != OverlayModes.Priorities.ID)
		{
			return;
		}
		this.prioritizables.Clear();
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleExtents(out vector2I, out vector2I2);
		int height = vector2I2.y - vector2I.y;
		int width = vector2I2.x - vector2I.x;
		Extents extents = new Extents(vector2I.x, vector2I.y, width, height);
		GameScenePartitioner.Instance.VisitEntries<PrioritizableRenderer>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.prioritizableObjects, new Func<object, PrioritizableRenderer, Util.IterationInstruction>(PrioritizableRenderer.renderEveryTickVisitHelper), this);
		if (this.prioritizableCount != this.prioritizables.Count)
		{
			this.prioritizableCount = this.prioritizables.Count;
			this.vertices = new Vector3[4 * this.prioritizableCount];
			this.uvs = new Vector2[4 * this.prioritizableCount];
			this.triangles = new int[6 * this.prioritizableCount];
		}
		if (this.prioritizableCount == 0)
		{
			return;
		}
		for (int i = 0; i < this.prioritizables.Count; i++)
		{
			Prioritizable prioritizable = this.prioritizables[i];
			Vector3 vector = Vector3.zero;
			KAnimControllerBase component = prioritizable.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				vector = component.GetWorldPivot();
			}
			else
			{
				vector = prioritizable.transform.GetPosition();
			}
			vector.x += prioritizable.iconOffset.x;
			vector.y += prioritizable.iconOffset.y;
			Vector2 vector2 = new Vector2(0.2f, 0.3f) * prioritizable.iconScale;
			float z = -5f;
			int num = 4 * i;
			this.vertices[num] = new Vector3(vector.x - vector2.x, vector.y - vector2.y, z);
			this.vertices[1 + num] = new Vector3(vector.x - vector2.x, vector.y + vector2.y, z);
			this.vertices[2 + num] = new Vector3(vector.x + vector2.x, vector.y - vector2.y, z);
			this.vertices[3 + num] = new Vector3(vector.x + vector2.x, vector.y + vector2.y, z);
			float num2 = 0.1f;
			PrioritySetting masterPriority = prioritizable.GetMasterPriority();
			float num3 = -1f;
			if (masterPriority.priority_class >= PriorityScreen.PriorityClass.high)
			{
				num3 += 9f;
			}
			if (masterPriority.priority_class >= PriorityScreen.PriorityClass.topPriority)
			{
				num3 += 0f;
			}
			num3 += (float)masterPriority.priority_value;
			float num4 = num2 * num3;
			float num5 = 0f;
			float num6 = num2;
			float num7 = 1f;
			this.uvs[num] = new Vector2(num4, num5);
			this.uvs[1 + num] = new Vector2(num4, num5 + num7);
			this.uvs[2 + num] = new Vector2(num4 + num6, num5);
			this.uvs[3 + num] = new Vector2(num4 + num6, num5 + num7);
			int num8 = 6 * i;
			this.triangles[num8] = num;
			this.triangles[1 + num8] = num + 1;
			this.triangles[2 + num8] = num + 2;
			this.triangles[3 + num8] = num + 2;
			this.triangles[4 + num8] = num + 1;
			this.triangles[5 + num8] = num + 3;
		}
		this.mesh.Clear();
		this.mesh.vertices = this.vertices;
		this.mesh.uv = this.uvs;
		this.mesh.SetTriangles(this.triangles, 0);
		this.mesh.RecalculateBounds();
		Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, this.material, this.layer, GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera, 0, null, false, false);
	}

	// Token: 0x040015DB RID: 5595
	private Mesh mesh;

	// Token: 0x040015DC RID: 5596
	private int layer;

	// Token: 0x040015DD RID: 5597
	private Material material;

	// Token: 0x040015DE RID: 5598
	private int prioritizableCount;

	// Token: 0x040015DF RID: 5599
	private Vector3[] vertices;

	// Token: 0x040015E0 RID: 5600
	private Vector2[] uvs;

	// Token: 0x040015E1 RID: 5601
	private int[] triangles;

	// Token: 0x040015E2 RID: 5602
	private List<Prioritizable> prioritizables;

	// Token: 0x040015E3 RID: 5603
	private PrioritizeTool tool;
}
