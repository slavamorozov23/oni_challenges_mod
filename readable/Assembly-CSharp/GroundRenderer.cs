using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000C31 RID: 3121
[AddComponentMenu("KMonoBehaviour/scripts/GroundRenderer")]
public class GroundRenderer : KMonoBehaviour
{
	// Token: 0x06005E5F RID: 24159 RVA: 0x002273BC File Offset: 0x002255BC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		this.OnShadersReloaded();
		this.masks.Initialize();
		SubWorld.ZoneType[] array = (SubWorld.ZoneType[])Enum.GetValues(typeof(SubWorld.ZoneType));
		this.biomeMasks = new GroundMasks.BiomeMaskData[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SubWorld.ZoneType zone_type = array[i];
			this.biomeMasks[i] = this.GetBiomeMask(zone_type);
		}
	}

	// Token: 0x06005E60 RID: 24160 RVA: 0x00227438 File Offset: 0x00225638
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.size = new Vector2I((Grid.WidthInCells + 16 - 1) / 16, (Grid.HeightInCells + 16 - 1) / 16);
		this.dirtyChunks = new bool[this.size.x, this.size.y];
		this.worldChunks = new GroundRenderer.WorldChunk[this.size.x, this.size.y];
		for (int i = 0; i < this.size.y; i++)
		{
			for (int j = 0; j < this.size.x; j++)
			{
				this.worldChunks[j, i] = new GroundRenderer.WorldChunk(j, i);
				this.dirtyChunks[j, i] = true;
			}
		}
	}

	// Token: 0x06005E61 RID: 24161 RVA: 0x00227500 File Offset: 0x00225700
	public void Render(Vector2I vis_min, Vector2I vis_max, bool forceVisibleRebuild = false)
	{
		if (!base.enabled)
		{
			return;
		}
		int layer = LayerMask.NameToLayer("World");
		Vector2I vector2I = new Vector2I(vis_min.x / 16, vis_min.y / 16);
		Vector2I vector2I2 = new Vector2I((vis_max.x + 16 - 1) / 16, (vis_max.y + 16 - 1) / 16);
		for (int i = vector2I.y; i < vector2I2.y; i++)
		{
			for (int j = vector2I.x; j < vector2I2.x; j++)
			{
				GroundRenderer.WorldChunk worldChunk = this.worldChunks[j, i];
				if (this.dirtyChunks[j, i] || forceVisibleRebuild)
				{
					this.dirtyChunks[j, i] = false;
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
				worldChunk.Render(layer);
			}
		}
		this.RebuildDirtyChunks();
	}

	// Token: 0x06005E62 RID: 24162 RVA: 0x002275DF File Offset: 0x002257DF
	public void RenderAll()
	{
		this.Render(new Vector2I(0, 0), new Vector2I(this.worldChunks.GetLength(0) * 16, this.worldChunks.GetLength(1) * 16), true);
	}

	// Token: 0x06005E63 RID: 24163 RVA: 0x00227614 File Offset: 0x00225814
	private void RebuildDirtyChunks()
	{
		for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
		{
			for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
			{
				if (this.dirtyChunks[j, i])
				{
					this.dirtyChunks[j, i] = false;
					this.worldChunks[j, i].Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x06005E64 RID: 24164 RVA: 0x0022768C File Offset: 0x0022588C
	public void MarkDirty(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x / 16, vector2I.y / 16);
		this.dirtyChunks[vector2I2.x, vector2I2.y] = true;
		bool flag = vector2I.x % 16 == 0 && vector2I2.x > 0;
		bool flag2 = vector2I.x % 16 == 15 && vector2I2.x < this.size.x - 1;
		bool flag3 = vector2I.y % 16 == 0 && vector2I2.y > 0;
		bool flag4 = vector2I.y % 16 == 15 && vector2I2.y < this.size.y - 1;
		if (flag)
		{
			this.dirtyChunks[vector2I2.x - 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y + 1] = true;
			}
		}
		if (flag3)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y - 1] = true;
		}
		if (flag4)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y + 1] = true;
		}
		if (flag2)
		{
			this.dirtyChunks[vector2I2.x + 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y + 1] = true;
			}
		}
	}

	// Token: 0x06005E65 RID: 24165 RVA: 0x00227840 File Offset: 0x00225A40
	private Vector2I GetChunkIdx(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		return new Vector2I(vector2I.x / 16, vector2I.y / 16);
	}

	// Token: 0x06005E66 RID: 24166 RVA: 0x0022786C File Offset: 0x00225A6C
	private GroundMasks.BiomeMaskData GetBiomeMask(SubWorld.ZoneType zone_type)
	{
		GroundMasks.BiomeMaskData result = null;
		string key = zone_type.ToString().ToLower();
		this.masks.biomeMasks.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x06005E67 RID: 24167 RVA: 0x002278A4 File Offset: 0x00225AA4
	private void InitOpaqueMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_opaque";
		material.renderQueue = RenderQueues.WorldOpaque;
		material.EnableKeyword("OPAQUE");
		material.DisableKeyword("ALPHA");
		this.ConfigureMaterialShine(material);
		material.SetInt("_SrcAlpha", 1);
		material.SetInt("_DstAlpha", 0);
		material.SetInt("_ZWrite", 1);
		material.SetTexture("_AlphaTestMap", Texture2D.whiteTexture);
	}

	// Token: 0x06005E68 RID: 24168 RVA: 0x00227930 File Offset: 0x00225B30
	private void InitAlphaMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_alpha";
		material.renderQueue = RenderQueues.WorldTransparent;
		material.EnableKeyword("ALPHA");
		material.DisableKeyword("OPAQUE");
		this.ConfigureMaterialShine(material);
		material.SetTexture("_AlphaTestMap", this.masks.maskAtlas.texture);
		material.SetInt("_SrcAlpha", 5);
		material.SetInt("_DstAlpha", 10);
		material.SetInt("_ZWrite", 0);
	}

	// Token: 0x06005E69 RID: 24169 RVA: 0x002279C8 File Offset: 0x00225BC8
	private void ConfigureMaterialShine(Material material)
	{
		if (material.GetTexture("_ShineMask") != null)
		{
			material.DisableKeyword("MATTE");
			material.EnableKeyword("SHINY");
			return;
		}
		material.EnableKeyword("MATTE");
		material.DisableKeyword("SHINY");
	}

	// Token: 0x06005E6A RID: 24170 RVA: 0x00227A18 File Offset: 0x00225C18
	[ContextMenu("Reload Shaders")]
	public void OnShadersReloaded()
	{
		this.FreeMaterials();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				if (element.substance.material == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						element.name,
						"must have material associated with it in the substance table"
					});
				}
				Material material = new Material(element.substance.material);
				this.InitOpaqueMaterial(material, element);
				Material material2 = new Material(material);
				this.InitAlphaMaterial(material2, element);
				GroundRenderer.Materials value = new GroundRenderer.Materials(material, material2);
				this.elementMaterials[element.id] = value;
			}
		}
		if (this.worldChunks != null)
		{
			for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
			{
				for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
				{
					this.dirtyChunks[j, i] = true;
				}
			}
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int k = array.GetLowerBound(0); k <= upperBound; k++)
			{
				for (int l = array.GetLowerBound(1); l <= upperBound2; l++)
				{
					GroundRenderer.WorldChunk worldChunk = array[k, l];
					worldChunk.Clear();
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x06005E6B RID: 24171 RVA: 0x00227BB0 File Offset: 0x00225DB0
	public void FreeResources()
	{
		this.FreeMaterials();
		this.elementMaterials.Clear();
		if (this.worldChunks != null)
		{
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int i = array.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
				{
					GroundRenderer.WorldChunk worldChunk = array[i, j];
					worldChunk.FreeResources();
				}
			}
			this.worldChunks = null;
		}
	}

	// Token: 0x06005E6C RID: 24172 RVA: 0x00227C30 File Offset: 0x00225E30
	private void FreeMaterials()
	{
		foreach (GroundRenderer.Materials materials in this.elementMaterials.Values)
		{
			UnityEngine.Object.Destroy(materials.opaque);
			UnityEngine.Object.Destroy(materials.alpha);
		}
		this.elementMaterials.Clear();
	}

	// Token: 0x04003EBD RID: 16061
	[SerializeField]
	private GroundMasks masks;

	// Token: 0x04003EBE RID: 16062
	private GroundMasks.BiomeMaskData[] biomeMasks;

	// Token: 0x04003EBF RID: 16063
	private Dictionary<SimHashes, GroundRenderer.Materials> elementMaterials = new Dictionary<SimHashes, GroundRenderer.Materials>();

	// Token: 0x04003EC0 RID: 16064
	private bool[,] dirtyChunks;

	// Token: 0x04003EC1 RID: 16065
	private GroundRenderer.WorldChunk[,] worldChunks;

	// Token: 0x04003EC2 RID: 16066
	private const int ChunkEdgeSize = 16;

	// Token: 0x04003EC3 RID: 16067
	private Vector2I size;

	// Token: 0x02001DDB RID: 7643
	[Serializable]
	private struct Materials
	{
		// Token: 0x0600B266 RID: 45670 RVA: 0x003DFCBC File Offset: 0x003DDEBC
		public Materials(Material opaque, Material alpha)
		{
			this.opaque = opaque;
			this.alpha = alpha;
		}

		// Token: 0x04008C6A RID: 35946
		public Material opaque;

		// Token: 0x04008C6B RID: 35947
		public Material alpha;
	}

	// Token: 0x02001DDC RID: 7644
	private class ElementChunk
	{
		// Token: 0x0600B267 RID: 45671 RVA: 0x003DFCCC File Offset: 0x003DDECC
		public ElementChunk(SimHashes element, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			this.element = element;
			GroundRenderer.Materials materials2 = materials[element];
			this.alpha = new GroundRenderer.ElementChunk.RenderData(materials2.alpha);
			this.opaque = new GroundRenderer.ElementChunk.RenderData(materials2.opaque);
			this.Clear();
		}

		// Token: 0x0600B268 RID: 45672 RVA: 0x003DFD16 File Offset: 0x003DDF16
		public void Clear()
		{
			this.opaque.Clear();
			this.alpha.Clear();
			this.tileCount = 0;
		}

		// Token: 0x0600B269 RID: 45673 RVA: 0x003DFD35 File Offset: 0x003DDF35
		public void AddOpaqueQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.opaque.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x0600B26A RID: 45674 RVA: 0x003DFD53 File Offset: 0x003DDF53
		public void AddAlphaQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.alpha.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x0600B26B RID: 45675 RVA: 0x003DFD71 File Offset: 0x003DDF71
		public void Build()
		{
			this.opaque.Build();
			this.alpha.Build();
		}

		// Token: 0x0600B26C RID: 45676 RVA: 0x003DFD8C File Offset: 0x003DDF8C
		public void Render(int layer, int element_idx)
		{
			float num = Grid.GetLayerZ(Grid.SceneLayer.Ground);
			num -= 0.0001f * (float)element_idx;
			this.opaque.Render(new Vector3(0f, 0f, num), layer);
			this.alpha.Render(new Vector3(0f, 0f, num), layer);
		}

		// Token: 0x0600B26D RID: 45677 RVA: 0x003DFDE4 File Offset: 0x003DDFE4
		public void FreeResources()
		{
			this.alpha.FreeResources();
			this.opaque.FreeResources();
			this.alpha = null;
			this.opaque = null;
		}

		// Token: 0x04008C6C RID: 35948
		public SimHashes element;

		// Token: 0x04008C6D RID: 35949
		private GroundRenderer.ElementChunk.RenderData alpha;

		// Token: 0x04008C6E RID: 35950
		private GroundRenderer.ElementChunk.RenderData opaque;

		// Token: 0x04008C6F RID: 35951
		public int tileCount;

		// Token: 0x02002A53 RID: 10835
		private class RenderData
		{
			// Token: 0x0600D45B RID: 54363 RVA: 0x0043C154 File Offset: 0x0043A354
			public RenderData(Material material)
			{
				this.material = material;
				this.mesh = new Mesh();
				this.mesh.MarkDynamic();
				this.mesh.name = "ElementChunk";
				this.pos = new List<Vector3>();
				this.uv = new List<Vector2>();
				this.indices = new List<int>();
			}

			// Token: 0x0600D45C RID: 54364 RVA: 0x0043C1B5 File Offset: 0x0043A3B5
			public void ClearMesh()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
					UnityEngine.Object.DestroyImmediate(this.mesh);
					this.mesh = null;
				}
			}

			// Token: 0x0600D45D RID: 54365 RVA: 0x0043C1E4 File Offset: 0x0043A3E4
			public void Clear()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
				}
				if (this.pos != null)
				{
					this.pos.Clear();
				}
				if (this.uv != null)
				{
					this.uv.Clear();
				}
				if (this.indices != null)
				{
					this.indices.Clear();
				}
			}

			// Token: 0x0600D45E RID: 54366 RVA: 0x0043C243 File Offset: 0x0043A443
			public void FreeResources()
			{
				this.ClearMesh();
				this.Clear();
				this.pos = null;
				this.uv = null;
				this.indices = null;
				this.material = null;
			}

			// Token: 0x0600D45F RID: 54367 RVA: 0x0043C26D File Offset: 0x0043A46D
			public void Build()
			{
				this.mesh.SetVertices(this.pos);
				this.mesh.SetUVs(0, this.uv);
				this.mesh.SetTriangles(this.indices, 0);
			}

			// Token: 0x0600D460 RID: 54368 RVA: 0x0043C2A4 File Offset: 0x0043A4A4
			public void AddQuad(int x, int y, GroundMasks.UVData uvs)
			{
				int count = this.pos.Count;
				this.indices.Add(count);
				this.indices.Add(count + 1);
				this.indices.Add(count + 3);
				this.indices.Add(count);
				this.indices.Add(count + 3);
				this.indices.Add(count + 2);
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.uv.Add(uvs.bl);
				this.uv.Add(uvs.br);
				this.uv.Add(uvs.tl);
				this.uv.Add(uvs.tr);
			}

			// Token: 0x0600D461 RID: 54369 RVA: 0x0043C400 File Offset: 0x0043A600
			public void Render(Vector3 position, int layer)
			{
				if (this.pos.Count != 0)
				{
					Graphics.DrawMesh(this.mesh, position, Quaternion.identity, this.material, layer, null, 0, null, ShadowCastingMode.Off, false, null, false);
				}
			}

			// Token: 0x0400BB08 RID: 47880
			public Material material;

			// Token: 0x0400BB09 RID: 47881
			public Mesh mesh;

			// Token: 0x0400BB0A RID: 47882
			public List<Vector3> pos;

			// Token: 0x0400BB0B RID: 47883
			public List<Vector2> uv;

			// Token: 0x0400BB0C RID: 47884
			public List<int> indices;
		}
	}

	// Token: 0x02001DDD RID: 7645
	private struct WorldChunk
	{
		// Token: 0x0600B26E RID: 45678 RVA: 0x003DFE0A File Offset: 0x003DE00A
		public WorldChunk(int x, int y)
		{
			this.chunkX = x;
			this.chunkY = y;
			this.elementChunks = new List<GroundRenderer.ElementChunk>();
		}

		// Token: 0x0600B26F RID: 45679 RVA: 0x003DFE25 File Offset: 0x003DE025
		public void Clear()
		{
			this.elementChunks.Clear();
		}

		// Token: 0x0600B270 RID: 45680 RVA: 0x003DFE34 File Offset: 0x003DE034
		private static void InsertSorted(Element element, Element[] array, int size)
		{
			int id = (int)element.id;
			for (int i = 0; i < size; i++)
			{
				Element element2 = array[i];
				if (element2.id > (SimHashes)id)
				{
					array[i] = element;
					element = element2;
					id = (int)element2.id;
				}
			}
			array[size] = element;
		}

		// Token: 0x0600B271 RID: 45681 RVA: 0x003DFE74 File Offset: 0x003DE074
		public void Rebuild(GroundMasks.BiomeMaskData[] biomeMasks, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.Clear();
			}
			Vector2I vector2I = new Vector2I(this.chunkX * 16, this.chunkY * 16);
			Vector2I vector2I2 = new Vector2I(Math.Min(Grid.WidthInCells, (this.chunkX + 1) * 16), Math.Min(Grid.HeightInCells, (this.chunkY + 1) * 16));
			for (int i = vector2I.y; i < vector2I2.y; i++)
			{
				int num = Math.Max(0, i - 1);
				int num2 = i;
				for (int j = vector2I.x; j < vector2I2.x; j++)
				{
					int num3 = Math.Max(0, j - 1);
					int num4 = j;
					int num5 = num * Grid.WidthInCells + num3;
					int num6 = num * Grid.WidthInCells + num4;
					int num7 = num2 * Grid.WidthInCells + num3;
					int num8 = num2 * Grid.WidthInCells + num4;
					GroundRenderer.WorldChunk.elements[0] = Grid.Element[num5];
					GroundRenderer.WorldChunk.elements[1] = Grid.Element[num6];
					GroundRenderer.WorldChunk.elements[2] = Grid.Element[num7];
					GroundRenderer.WorldChunk.elements[3] = Grid.Element[num8];
					GroundRenderer.WorldChunk.substances[0] = ((Grid.RenderedByWorld[num5] && GroundRenderer.WorldChunk.elements[0].IsSolid) ? GroundRenderer.WorldChunk.elements[0].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[1] = ((Grid.RenderedByWorld[num6] && GroundRenderer.WorldChunk.elements[1].IsSolid) ? GroundRenderer.WorldChunk.elements[1].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[2] = ((Grid.RenderedByWorld[num7] && GroundRenderer.WorldChunk.elements[2].IsSolid) ? GroundRenderer.WorldChunk.elements[2].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[3] = ((Grid.RenderedByWorld[num8] && GroundRenderer.WorldChunk.elements[3].IsSolid) ? GroundRenderer.WorldChunk.elements[3].substance.idx : -1);
					GroundRenderer.WorldChunk.uniqueElements[0] = GroundRenderer.WorldChunk.elements[0];
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[1], GroundRenderer.WorldChunk.uniqueElements, 1);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[2], GroundRenderer.WorldChunk.uniqueElements, 2);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[3], GroundRenderer.WorldChunk.uniqueElements, 3);
					int num9 = -1;
					int biomeIdx = GroundRenderer.WorldChunk.GetBiomeIdx(i * Grid.WidthInCells + j);
					GroundMasks.BiomeMaskData biomeMaskData = biomeMasks[biomeIdx];
					if (biomeMaskData == null)
					{
						biomeMaskData = biomeMasks[3];
					}
					for (int k = 0; k < GroundRenderer.WorldChunk.uniqueElements.Length; k++)
					{
						Element element = GroundRenderer.WorldChunk.uniqueElements[k];
						if (element.IsSolid)
						{
							int idx = element.substance.idx;
							if (idx != num9)
							{
								num9 = idx;
								int num10 = ((GroundRenderer.WorldChunk.substances[2] >= idx) ? 1 : 0) << 3 | ((GroundRenderer.WorldChunk.substances[3] >= idx) ? 1 : 0) << 2 | ((GroundRenderer.WorldChunk.substances[0] >= idx) ? 1 : 0) << 1 | ((GroundRenderer.WorldChunk.substances[1] >= idx) ? 1 : 0);
								if (num10 > 0)
								{
									GroundMasks.UVData[] variationUVs = biomeMaskData.tiles[num10].variationUVs;
									float staticRandom = GroundRenderer.WorldChunk.GetStaticRandom(j, i);
									int num11 = Mathf.Min(variationUVs.Length - 1, (int)((float)variationUVs.Length * staticRandom));
									GroundMasks.UVData uvs = variationUVs[num11 % variationUVs.Length];
									GroundRenderer.ElementChunk elementChunk2 = this.GetElementChunk(element.id, materials);
									if (num10 == 15)
									{
										elementChunk2.AddOpaqueQuad(j, i, uvs);
									}
									else
									{
										elementChunk2.AddAlphaQuad(j, i, uvs);
									}
								}
							}
						}
					}
				}
			}
			foreach (GroundRenderer.ElementChunk elementChunk3 in this.elementChunks)
			{
				elementChunk3.Build();
			}
			for (int l = this.elementChunks.Count - 1; l >= 0; l--)
			{
				if (this.elementChunks[l].tileCount == 0)
				{
					int index = this.elementChunks.Count - 1;
					this.elementChunks[l] = this.elementChunks[index];
					this.elementChunks.RemoveAt(index);
				}
			}
		}

		// Token: 0x0600B272 RID: 45682 RVA: 0x003E02D0 File Offset: 0x003DE4D0
		private GroundRenderer.ElementChunk GetElementChunk(SimHashes elementID, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			GroundRenderer.ElementChunk elementChunk = null;
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				if (this.elementChunks[i].element == elementID)
				{
					elementChunk = this.elementChunks[i];
					break;
				}
			}
			if (elementChunk == null)
			{
				elementChunk = new GroundRenderer.ElementChunk(elementID, materials);
				this.elementChunks.Add(elementChunk);
			}
			return elementChunk;
		}

		// Token: 0x0600B273 RID: 45683 RVA: 0x003E0330 File Offset: 0x003DE530
		private static int GetBiomeIdx(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return 0;
			}
			SubWorld.ZoneType result = SubWorld.ZoneType.Sandstone;
			if (global::World.Instance != null && global::World.Instance.zoneRenderData != null)
			{
				result = global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell);
			}
			return (int)result;
		}

		// Token: 0x0600B274 RID: 45684 RVA: 0x003E037A File Offset: 0x003DE57A
		private static float GetStaticRandom(int x, int y)
		{
			return PerlinSimplexNoise.noise((float)x * GroundRenderer.WorldChunk.NoiseScale.x, (float)y * GroundRenderer.WorldChunk.NoiseScale.y);
		}

		// Token: 0x0600B275 RID: 45685 RVA: 0x003E039C File Offset: 0x003DE59C
		public void Render(int layer)
		{
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				GroundRenderer.ElementChunk elementChunk = this.elementChunks[i];
				elementChunk.Render(layer, ElementLoader.FindElementByHash(elementChunk.element).substance.idx);
			}
		}

		// Token: 0x0600B276 RID: 45686 RVA: 0x003E03E8 File Offset: 0x003DE5E8
		public void FreeResources()
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.FreeResources();
			}
			this.elementChunks.Clear();
			this.elementChunks = null;
		}

		// Token: 0x04008C70 RID: 35952
		public readonly int chunkX;

		// Token: 0x04008C71 RID: 35953
		public readonly int chunkY;

		// Token: 0x04008C72 RID: 35954
		private List<GroundRenderer.ElementChunk> elementChunks;

		// Token: 0x04008C73 RID: 35955
		private static Element[] elements = new Element[4];

		// Token: 0x04008C74 RID: 35956
		private static Element[] uniqueElements = new Element[4];

		// Token: 0x04008C75 RID: 35957
		private static int[] substances = new int[4];

		// Token: 0x04008C76 RID: 35958
		private static Vector2 NoiseScale = new Vector3(1f, 1f);
	}
}
