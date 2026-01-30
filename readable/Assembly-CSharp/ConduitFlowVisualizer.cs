using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000C50 RID: 3152
public class ConduitFlowVisualizer
{
	// Token: 0x06005FC5 RID: 24517 RVA: 0x00232118 File Offset: 0x00230318
	public ConduitFlowVisualizer(ConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, ConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.InitializeResources();
	}

	// Token: 0x06005FC6 RID: 24518 RVA: 0x002321A4 File Offset: 0x002303A4
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x06005FC7 RID: 24519 RVA: 0x002321BC File Offset: 0x002303BC
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x06005FC8 RID: 24520 RVA: 0x00232224 File Offset: 0x00230424
	private Color32 GetContentsColor(Element element, Color32 default_color)
	{
		if (element != null)
		{
			Color c = element.substance.conduitColour;
			c.a = 128f;
			return c;
		}
		return default_color;
	}

	// Token: 0x06005FC9 RID: 24521 RVA: 0x00232259 File Offset: 0x00230459
	private Color32 GetTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.tint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName);
	}

	// Token: 0x06005FCA RID: 24522 RVA: 0x00232289 File Offset: 0x00230489
	private Color32 GetInsulatedTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.insulatedTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName);
	}

	// Token: 0x06005FCB RID: 24523 RVA: 0x002322B9 File Offset: 0x002304B9
	private Color32 GetRadiantTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.radiantTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayRadiantTintName);
	}

	// Token: 0x06005FCC RID: 24524 RVA: 0x002322EC File Offset: 0x002304EC
	private Color32 GetCellTintColour(int cell)
	{
		Color32 result;
		if (this.insulatedCells.Contains(cell))
		{
			result = this.GetInsulatedTintColour();
		}
		else if (this.radiantCells.Contains(cell))
		{
			result = this.GetRadiantTintColour();
		}
		else
		{
			result = this.GetTintColour();
		}
		return result;
	}

	// Token: 0x06005FCD RID: 24525 RVA: 0x00232330 File Offset: 0x00230530
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<ConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % 10;
				this.audioInfo[i] = audioInfo;
			}
		}
		if (this.tuning.renderMesh)
		{
			this.RenderMesh(z, render_layer, lerp_percent, trigger_audio);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x06005FCE RID: 24526 RVA: 0x002323E4 File Offset: 0x002305E4
	private void RenderMesh(float z, int render_layer, float lerp_percent, bool trigger_audio)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I max = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		ConduitFlowVisualizer.RenderMeshContext renderMeshContext = new ConduitFlowVisualizer.RenderMeshContext(this, lerp_percent, min, max);
		if (renderMeshContext.visible_conduits.Count == 0)
		{
			renderMeshContext.Finish();
			return;
		}
		ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Reset(renderMeshContext);
		GlobalJobManager.Run(ConduitFlowVisualizer.RenderMeshBatchJob.Instance);
		float z2 = 0f;
		if (this.showContents)
		{
			z2 = 1f;
		}
		float w = (float)((int)(this.animTime / (1.0 / (double)this.tuning.framesPerSecond)) % (int)this.tuning.spriteCount) * (1f / this.tuning.spriteCount);
		this.movingBallMesh.Begin();
		this.movingBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.movingBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.movingBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, w));
		this.movingBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		this.staticBallMesh.Begin();
		this.staticBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.staticBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.staticBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, 0f));
		this.staticBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		Vector3 position = CameraController.Instance.transform.GetPosition();
		ConduitFlowVisualizer visualizer = trigger_audio ? this : null;
		ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Finish(this.movingBallMesh, this.staticBallMesh, position, visualizer);
		this.movingBallMesh.End(z, this.layer);
		this.staticBallMesh.End(z, this.layer);
		ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Reset(ConduitFlowVisualizer.RenderMeshContext.EmptyContext);
	}

	// Token: 0x06005FCF RID: 24527 RVA: 0x002326D7 File Offset: 0x002308D7
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x06005FD0 RID: 24528 RVA: 0x002326F8 File Offset: 0x002308F8
	private void AddAudioSource(ConduitFlow.Conduit conduit, Vector3 camera_pos)
	{
		UtilityNetwork network = this.flowManager.GetNetwork(conduit);
		if (network == null)
		{
			return;
		}
		Vector3 vector = Grid.CellToPosCCC(conduit.GetCell(this.flowManager), Grid.SceneLayer.Building);
		float num = Vector3.SqrMagnitude(vector - camera_pos);
		bool flag = false;
		for (int i = 0; i < this.audioInfo.Count; i++)
		{
			ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
			if (audioInfo.networkID == network.id)
			{
				if (num < audioInfo.distance)
				{
					audioInfo.distance = num;
					audioInfo.position = vector;
					this.audioInfo[i] = audioInfo;
				}
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			ConduitFlowVisualizer.AudioInfo item = default(ConduitFlowVisualizer.AudioInfo);
			item.networkID = network.id;
			item.position = vector;
			item.distance = num;
			item.blobCount = 0;
			this.audioInfo.Add(item);
		}
	}

	// Token: 0x06005FD1 RID: 24529 RVA: 0x002327DC File Offset: 0x002309DC
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<ConduitFlowVisualizer.AudioInfo> list = new List<ConduitFlowVisualizer.AudioInfo>();
		for (int i = 0; i < this.audioInfo.Count; i++)
		{
			if (instance.IsVisiblePos(this.audioInfo[i].position))
			{
				list.Add(this.audioInfo[i]);
				num++;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ConduitFlowVisualizer.AudioInfo audioInfo = list[j];
			if (audioInfo.distance != float.PositiveInfinity)
			{
				Vector3 position = audioInfo.position;
				position.z = 0f;
				EventInstance instance2 = SoundEvent.BeginOneShot(this.overlaySound, position, 1f, false);
				instance2.setParameterByName("blobCount", (float)audioInfo.blobCount, false);
				instance2.setParameterByName("networkCount", (float)num, false);
				SoundEvent.EndOneShot(instance2);
			}
		}
	}

	// Token: 0x06005FD2 RID: 24530 RVA: 0x002328CE File Offset: 0x00230ACE
	public void AddThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Add(cell);
		}
	}

	// Token: 0x06005FD3 RID: 24531 RVA: 0x002328FB File Offset: 0x00230AFB
	public void RemoveThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Remove(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Remove(cell);
		}
	}

	// Token: 0x06005FD4 RID: 24532 RVA: 0x00232928 File Offset: 0x00230B28
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04003FF8 RID: 16376
	private ConduitFlow flowManager;

	// Token: 0x04003FF9 RID: 16377
	private EventReference overlaySound;

	// Token: 0x04003FFA RID: 16378
	private bool showContents;

	// Token: 0x04003FFB RID: 16379
	private double animTime;

	// Token: 0x04003FFC RID: 16380
	private int layer;

	// Token: 0x04003FFD RID: 16381
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x04003FFE RID: 16382
	private List<ConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x04003FFF RID: 16383
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x04004000 RID: 16384
	private HashSet<int> radiantCells = new HashSet<int>();

	// Token: 0x04004001 RID: 16385
	private Game.ConduitVisInfo visInfo;

	// Token: 0x04004002 RID: 16386
	private ConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x04004003 RID: 16387
	private ConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x04004004 RID: 16388
	private int highlightedCell = -1;

	// Token: 0x04004005 RID: 16389
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x04004006 RID: 16390
	private ConduitFlowVisualizer.Tuning tuning;

	// Token: 0x02001DFC RID: 7676
	[Serializable]
	public class Tuning
	{
		// Token: 0x04008CDA RID: 36058
		public bool renderMesh;

		// Token: 0x04008CDB RID: 36059
		public float size;

		// Token: 0x04008CDC RID: 36060
		public float spriteCount;

		// Token: 0x04008CDD RID: 36061
		public float framesPerSecond;

		// Token: 0x04008CDE RID: 36062
		public Texture2D backgroundTexture;

		// Token: 0x04008CDF RID: 36063
		public Texture2D foregroundTexture;
	}

	// Token: 0x02001DFD RID: 7677
	private class ConduitFlowMesh
	{
		// Token: 0x0600B2C7 RID: 45767 RVA: 0x003E11A4 File Offset: 0x003DF3A4
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x0600B2C8 RID: 45768 RVA: 0x003E1214 File Offset: 0x003DF414
		public void AddQuad(Vector2 pos, Color32 color, float size, float is_foreground, float highlight, Vector2I uvbl, Vector2I uvtl, Vector2I uvbr, Vector2I uvtr)
		{
			float num = size * 0.5f;
			this.positions.Add(new Vector3(pos.x - num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x - num, pos.y + num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y + num, 0f));
			this.uvs.Add(new Vector4((float)uvbl.x, (float)uvbl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtl.x, (float)uvtl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvbr.x, (float)uvbr.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtr.x, (float)uvtr.y, is_foreground, highlight));
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.triangles.Add(this.quadIndex * 4);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 3);
			this.quadIndex++;
		}

		// Token: 0x0600B2C9 RID: 45769 RVA: 0x003E1407 File Offset: 0x003DF607
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x0600B2CA RID: 45770 RVA: 0x003E1416 File Offset: 0x003DF616
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x0600B2CB RID: 45771 RVA: 0x003E1425 File Offset: 0x003DF625
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x0600B2CC RID: 45772 RVA: 0x003E145C File Offset: 0x003DF65C
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(ConduitFlowVisualizer.GRID_OFFSET.x, ConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x0600B2CD RID: 45773 RVA: 0x003E14F2 File Offset: 0x003DF6F2
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x04008CE0 RID: 36064
		private Mesh mesh;

		// Token: 0x04008CE1 RID: 36065
		private Material material;

		// Token: 0x04008CE2 RID: 36066
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x04008CE3 RID: 36067
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x04008CE4 RID: 36068
		private List<int> triangles = new List<int>();

		// Token: 0x04008CE5 RID: 36069
		private List<Color32> colors = new List<Color32>();

		// Token: 0x04008CE6 RID: 36070
		private int quadIndex;
	}

	// Token: 0x02001DFE RID: 7678
	private struct AudioInfo
	{
		// Token: 0x04008CE7 RID: 36071
		public int networkID;

		// Token: 0x04008CE8 RID: 36072
		public int blobCount;

		// Token: 0x04008CE9 RID: 36073
		public float distance;

		// Token: 0x04008CEA RID: 36074
		public Vector3 position;
	}

	// Token: 0x02001DFF RID: 7679
	private struct RenderMeshContext
	{
		// Token: 0x0600B2CE RID: 45774 RVA: 0x003E1518 File Offset: 0x003DF718
		public RenderMeshContext(ConduitFlowVisualizer outer, float lerp_percent, Vector2I min, Vector2I max)
		{
			this.outer = outer;
			this.lerp_percent = lerp_percent;
			this.visible_conduits = ListPool<int, ConduitFlowVisualizer>.Allocate();
			this.visible_conduits.Capacity = Math.Max(outer.flowManager.soaInfo.NumEntries, this.visible_conduits.Capacity);
			for (int num = 0; num != outer.flowManager.soaInfo.NumEntries; num++)
			{
				Vector2I vector2I = Grid.CellToXY(outer.flowManager.soaInfo.GetCell(num));
				if (min <= vector2I && vector2I <= max)
				{
					this.visible_conduits.Add(num);
				}
			}
		}

		// Token: 0x0600B2CF RID: 45775 RVA: 0x003E15BA File Offset: 0x003DF7BA
		public void Finish()
		{
			this.visible_conduits.Recycle();
		}

		// Token: 0x04008CEB RID: 36075
		public static ConduitFlowVisualizer.RenderMeshContext EmptyContext;

		// Token: 0x04008CEC RID: 36076
		public ListPool<int, ConduitFlowVisualizer>.PooledList visible_conduits;

		// Token: 0x04008CED RID: 36077
		public ConduitFlowVisualizer outer;

		// Token: 0x04008CEE RID: 36078
		public float lerp_percent;
	}

	// Token: 0x02001E00 RID: 7680
	private class RenderMeshPerThreadData
	{
		// Token: 0x0600B2D1 RID: 45777 RVA: 0x003E15CC File Offset: 0x003DF7CC
		public void Finish(ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh, ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh, Vector3 camera_pos, ConduitFlowVisualizer visualizer)
		{
			for (int num = 0; num != this.moving_balls.Count; num++)
			{
				this.moving_balls[num].Consume(moving_ball_mesh);
			}
			this.moving_balls.Clear();
			for (int num2 = 0; num2 != this.static_balls.Count; num2++)
			{
				this.static_balls[num2].Consume(static_ball_mesh);
			}
			this.static_balls.Clear();
			if (visualizer != null)
			{
				foreach (ConduitFlow.Conduit conduit in this.moving_conduits)
				{
					visualizer.AddAudioSource(conduit, camera_pos);
				}
			}
			this.moving_conduits.Clear();
		}

		// Token: 0x04008CEF RID: 36079
		public List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball> moving_balls = new List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball>();

		// Token: 0x04008CF0 RID: 36080
		public List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball> static_balls = new List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball>();

		// Token: 0x04008CF1 RID: 36081
		public List<ConduitFlow.Conduit> moving_conduits = new List<ConduitFlow.Conduit>();

		// Token: 0x02002A56 RID: 10838
		public struct Ball
		{
			// Token: 0x0600D463 RID: 54371 RVA: 0x0043C449 File Offset: 0x0043A649
			public Ball(ConduitFlow.FlowDirections direction, Vector2 pos, Color32 color, float size, bool foreground, bool highlight)
			{
				this.pos = pos;
				this.size = size;
				this.color = color;
				this.direction = direction;
				this.foreground = foreground;
				this.highlight = highlight;
			}

			// Token: 0x0600D464 RID: 54372 RVA: 0x0043C478 File Offset: 0x0043A678
			public static void InitializeResources()
			{
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.None] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Left] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Right] = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Left];
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Up] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack
				{
					bl = new Vector2I(1, 0),
					tl = new Vector2I(0, 0),
					br = new Vector2I(1, 1),
					tr = new Vector2I(0, 1)
				};
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Down] = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Up];
			}

			// Token: 0x0600D465 RID: 54373 RVA: 0x0043C57D File Offset: 0x0043A77D
			private static ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack GetUVPack(ConduitFlow.FlowDirections direction)
			{
				return ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[direction];
			}

			// Token: 0x0600D466 RID: 54374 RVA: 0x0043C58C File Offset: 0x0043A78C
			public void Consume(ConduitFlowVisualizer.ConduitFlowMesh mesh)
			{
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack uvpack = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.GetUVPack(this.direction);
				mesh.AddQuad(this.pos, this.color, this.size, (float)(this.foreground ? 1 : 0), (float)(this.highlight ? 1 : 0), uvpack.bl, uvpack.tl, uvpack.br, uvpack.tr);
			}

			// Token: 0x0400BB14 RID: 47892
			private Vector2 pos;

			// Token: 0x0400BB15 RID: 47893
			private float size;

			// Token: 0x0400BB16 RID: 47894
			private Color32 color;

			// Token: 0x0400BB17 RID: 47895
			private ConduitFlow.FlowDirections direction;

			// Token: 0x0400BB18 RID: 47896
			private bool foreground;

			// Token: 0x0400BB19 RID: 47897
			private bool highlight;

			// Token: 0x0400BB1A RID: 47898
			private static Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack> uv_packs = new Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack>();

			// Token: 0x02003A56 RID: 14934
			private class UVPack
			{
				// Token: 0x0400EB9D RID: 60317
				public Vector2I bl;

				// Token: 0x0400EB9E RID: 60318
				public Vector2I tl;

				// Token: 0x0400EB9F RID: 60319
				public Vector2I br;

				// Token: 0x0400EBA0 RID: 60320
				public Vector2I tr;
			}
		}
	}

	// Token: 0x02001E01 RID: 7681
	private class RenderMeshBatchJob : WorkItemCollectionWithThreadContex<ConduitFlowVisualizer.RenderMeshContext, ConduitFlowVisualizer.RenderMeshPerThreadData>
	{
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x0600B2D3 RID: 45779 RVA: 0x003E16C9 File Offset: 0x003DF8C9
		public static ConduitFlowVisualizer.RenderMeshBatchJob Instance
		{
			get
			{
				if (ConduitFlowVisualizer.RenderMeshBatchJob.instance == null || ConduitFlowVisualizer.RenderMeshBatchJob.instance.threadContexts.Count != GlobalJobManager.ThreadCount)
				{
					ConduitFlowVisualizer.RenderMeshBatchJob.instance = new ConduitFlowVisualizer.RenderMeshBatchJob();
				}
				return ConduitFlowVisualizer.RenderMeshBatchJob.instance;
			}
		}

		// Token: 0x0600B2D4 RID: 45780 RVA: 0x003E16F8 File Offset: 0x003DF8F8
		public RenderMeshBatchJob()
		{
			this.threadContexts = new List<ConduitFlowVisualizer.RenderMeshPerThreadData>();
			for (int i = 0; i < GlobalJobManager.ThreadCount; i++)
			{
				this.threadContexts.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData());
			}
		}

		// Token: 0x0600B2D5 RID: 45781 RVA: 0x003E1736 File Offset: 0x003DF936
		public void Reset(ConduitFlowVisualizer.RenderMeshContext context)
		{
			this.sharedData = context;
			if (context.visible_conduits == null)
			{
				this.count = 0;
				return;
			}
			this.count = (context.visible_conduits.Count + 32 - 1) / 32;
		}

		// Token: 0x0600B2D6 RID: 45782 RVA: 0x003E1768 File Offset: 0x003DF968
		public override void RunItem(int item, ref ConduitFlowVisualizer.RenderMeshContext shared_data, ConduitFlowVisualizer.RenderMeshPerThreadData thread_context, int threadIndex)
		{
			Element element = null;
			int num = item * 32;
			int num2 = Math.Min(shared_data.visible_conduits.Count, num + 32);
			for (int i = num; i < num2; i++)
			{
				ConduitFlow.Conduit conduit = shared_data.outer.flowManager.soaInfo.GetConduit(shared_data.visible_conduits[i]);
				ConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(shared_data.outer.flowManager);
				ConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(shared_data.outer.flowManager);
				if (lastFlowInfo.contents.mass > 0f)
				{
					int cell = conduit.GetCell(shared_data.outer.flowManager);
					int cellFromDirection = ConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
					Vector2I vector2I = Grid.CellToXY(cell);
					Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
					Vector2 vector = (cell == -1) ? vector2I : Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), shared_data.lerp_percent);
					Color32 cellTintColour = shared_data.outer.GetCellTintColour(cell);
					Color32 cellTintColour2 = shared_data.outer.GetCellTintColour(cellFromDirection);
					Color32 color = Color32.Lerp(cellTintColour, cellTintColour2, shared_data.lerp_percent);
					bool highlight = false;
					if (shared_data.outer.showContents)
					{
						if (lastFlowInfo.contents.mass >= initialContents.mass)
						{
							thread_context.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(lastFlowInfo.direction, vector, color, shared_data.outer.tuning.size, false, false));
						}
						if (element == null || lastFlowInfo.contents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(lastFlowInfo.contents.element);
						}
					}
					else
					{
						element = null;
						highlight = (Grid.PosToCell(new Vector3(vector.x + ConduitFlowVisualizer.GRID_OFFSET.x, vector.y + ConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == shared_data.outer.highlightedCell);
					}
					Color32 contentsColor = shared_data.outer.GetContentsColor(element, color);
					float num3 = 1f;
					if (shared_data.outer.showContents || lastFlowInfo.contents.mass < initialContents.mass)
					{
						num3 = shared_data.outer.CalculateMassScale(lastFlowInfo.contents.mass);
					}
					thread_context.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(lastFlowInfo.direction, vector, contentsColor, shared_data.outer.tuning.size * num3, true, highlight));
					thread_context.moving_conduits.Add(conduit);
				}
				if (initialContents.mass > lastFlowInfo.contents.mass && initialContents.mass > 0f)
				{
					int cell2 = conduit.GetCell(shared_data.outer.flowManager);
					Vector2 pos = Grid.CellToXY(cell2);
					float mass = initialContents.mass - lastFlowInfo.contents.mass;
					bool highlight2 = false;
					Color32 cellTintColour3 = shared_data.outer.GetCellTintColour(cell2);
					float num4 = shared_data.outer.CalculateMassScale(mass);
					if (shared_data.outer.showContents)
					{
						thread_context.static_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(ConduitFlow.FlowDirections.None, pos, cellTintColour3, shared_data.outer.tuning.size * num4, false, false));
						if (element == null || initialContents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(initialContents.element);
						}
					}
					else
					{
						element = null;
						highlight2 = (cell2 == shared_data.outer.highlightedCell);
					}
					Color32 contentsColor2 = shared_data.outer.GetContentsColor(element, cellTintColour3);
					thread_context.static_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(ConduitFlow.FlowDirections.None, pos, contentsColor2, shared_data.outer.tuning.size * num4, true, highlight2));
				}
			}
		}

		// Token: 0x0600B2D7 RID: 45783 RVA: 0x003E1B24 File Offset: 0x003DFD24
		public void Finish(ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh, ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh, Vector3 camera_pos, ConduitFlowVisualizer visualizer)
		{
			foreach (ConduitFlowVisualizer.RenderMeshPerThreadData renderMeshPerThreadData in this.threadContexts)
			{
				renderMeshPerThreadData.Finish(moving_ball_mesh, static_ball_mesh, camera_pos, visualizer);
			}
			this.sharedData.Finish();
		}

		// Token: 0x04008CF2 RID: 36082
		private const int kBatchSize = 32;

		// Token: 0x04008CF3 RID: 36083
		private static ConduitFlowVisualizer.RenderMeshBatchJob instance;
	}
}
