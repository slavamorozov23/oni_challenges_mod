using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
	// Token: 0x02000EEC RID: 3820
	public class BlockTileRenderer : MonoBehaviour
	{
		// Token: 0x06007AC4 RID: 31428 RVA: 0x002FCAF9 File Offset: 0x002FACF9
		public static BlockTileRenderer.RenderInfoLayer GetRenderInfoLayer(bool isReplacement, SimHashes element)
		{
			if (isReplacement)
			{
				return BlockTileRenderer.RenderInfoLayer.Replacement;
			}
			if (element == SimHashes.Void)
			{
				return BlockTileRenderer.RenderInfoLayer.UnderConstruction;
			}
			return BlockTileRenderer.RenderInfoLayer.Built;
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06007AC5 RID: 31429 RVA: 0x002FCB0B File Offset: 0x002FAD0B
		public bool ForceRebuild
		{
			get
			{
				return this.forceRebuild;
			}
		}

		// Token: 0x06007AC6 RID: 31430 RVA: 0x002FCB14 File Offset: 0x002FAD14
		public BlockTileRenderer()
		{
			this.forceRebuild = false;
		}

		// Token: 0x06007AC7 RID: 31431 RVA: 0x002FCB98 File Offset: 0x002FAD98
		public void FreeResources()
		{
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.FreeResources();
				}
			}
			this.renderInfo.Clear();
		}

		// Token: 0x06007AC8 RID: 31432 RVA: 0x002FCC04 File Offset: 0x002FAE04
		private static bool MatchesDef(GameObject go, BuildingDef def)
		{
			return go != null && go.GetComponent<Building>().Def == def;
		}

		// Token: 0x06007AC9 RID: 31433 RVA: 0x002FCC24 File Offset: 0x002FAE24
		public virtual BlockTileRenderer.Bits GetConnectionBits(int x, int y, int query_layer)
		{
			BlockTileRenderer.Bits bits = (BlockTileRenderer.Bits)0;
			GameObject gameObject = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
			BuildingDef def = (gameObject != null) ? gameObject.GetComponent<Building>().Def : null;
			if (y > 0)
			{
				int num = (y - 1) * Grid.WidthInCells + x;
				if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num - 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.DownLeft;
				}
				if (BlockTileRenderer.MatchesDef(Grid.Objects[num, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.Down;
				}
				if (x < Grid.WidthInCells - 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num + 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.DownRight;
				}
			}
			int num2 = y * Grid.WidthInCells + x;
			if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num2 - 1, query_layer], def))
			{
				bits |= BlockTileRenderer.Bits.Left;
			}
			if (x < Grid.WidthInCells - 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num2 + 1, query_layer], def))
			{
				bits |= BlockTileRenderer.Bits.Right;
			}
			if (y < Grid.HeightInCells - 1)
			{
				int num3 = (y + 1) * Grid.WidthInCells + x;
				if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num3 - 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.UpLeft;
				}
				if (BlockTileRenderer.MatchesDef(Grid.Objects[num3, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.Up;
				}
				if (x < Grid.WidthInCells + 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num3 + 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.UpRight;
				}
			}
			return bits;
		}

		// Token: 0x06007ACA RID: 31434 RVA: 0x002FCD98 File Offset: 0x002FAF98
		private bool IsDecorConnectable(GameObject src, GameObject target)
		{
			if (src != null && target != null)
			{
				IBlockTileInfo component = src.GetComponent<IBlockTileInfo>();
				IBlockTileInfo component2 = target.GetComponent<IBlockTileInfo>();
				if (component != null && component2 != null)
				{
					return component.GetBlockTileConnectorID() == component2.GetBlockTileConnectorID();
				}
			}
			return false;
		}

		// Token: 0x06007ACB RID: 31435 RVA: 0x002FCDDC File Offset: 0x002FAFDC
		public virtual BlockTileRenderer.Bits GetDecorConnectionBits(int x, int y, int query_layer)
		{
			BlockTileRenderer.Bits bits = (BlockTileRenderer.Bits)0;
			GameObject src = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
			if (y > 0)
			{
				int num = (y - 1) * Grid.WidthInCells + x;
				if (x > 0 && Grid.Objects[num - 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.DownLeft;
				}
				if (Grid.Objects[num, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.Down;
				}
				if (x < Grid.WidthInCells - 1 && Grid.Objects[num + 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.DownRight;
				}
			}
			int num2 = y * Grid.WidthInCells + x;
			if (x > 0 && this.IsDecorConnectable(src, Grid.Objects[num2 - 1, query_layer]))
			{
				bits |= BlockTileRenderer.Bits.Left;
			}
			if (x < Grid.WidthInCells - 1 && this.IsDecorConnectable(src, Grid.Objects[num2 + 1, query_layer]))
			{
				bits |= BlockTileRenderer.Bits.Right;
			}
			if (y < Grid.HeightInCells - 1)
			{
				int num3 = (y + 1) * Grid.WidthInCells + x;
				if (x > 0 && Grid.Objects[num3 - 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.UpLeft;
				}
				if (Grid.Objects[num3, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.Up;
				}
				if (x < Grid.WidthInCells + 1 && Grid.Objects[num3 + 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.UpRight;
				}
			}
			return bits;
		}

		// Token: 0x06007ACC RID: 31436 RVA: 0x002FCF34 File Offset: 0x002FB134
		public void LateUpdate()
		{
			this.Render();
		}

		// Token: 0x06007ACD RID: 31437 RVA: 0x002FCF3C File Offset: 0x002FB13C
		private void Render()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			if (GameUtil.IsCapturingTimeLapse())
			{
				vector2I = new Vector2I(0, 0);
				vector2I2 = new Vector2I(Grid.WidthInCells / 16, Grid.HeightInCells / 16);
			}
			else
			{
				GridArea visibleArea = GridVisibleArea.GetVisibleArea();
				vector2I = new Vector2I(visibleArea.Min.x / 16, visibleArea.Min.y / 16);
				vector2I2 = new Vector2I((visibleArea.Max.x + 16 - 1) / 16, (visibleArea.Max.y + 16 - 1) / 16);
			}
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				BlockTileRenderer.RenderInfo value = keyValuePair.Value;
				for (int i = vector2I.y; i < vector2I2.y; i++)
				{
					for (int j = vector2I.x; j < vector2I2.x; j++)
					{
						value.Rebuild(this, j, i, MeshUtil.vertices, MeshUtil.uvs, MeshUtil.indices, MeshUtil.colours);
						value.Render(j, i);
					}
				}
			}
		}

		// Token: 0x06007ACE RID: 31438 RVA: 0x002FD074 File Offset: 0x002FB274
		public Color GetCellColour(int cell, SimHashes element)
		{
			Color white;
			if (cell == this.selectedCell)
			{
				white = this.selectColour;
			}
			else if (cell == this.invalidPlaceCell && element == SimHashes.Void)
			{
				white = this.invalidPlaceColour;
			}
			else if (cell == this.highlightCell)
			{
				white = this.highlightColour;
			}
			else
			{
				white = Color.white;
			}
			return white;
		}

		// Token: 0x06007ACF RID: 31439 RVA: 0x002FD0C8 File Offset: 0x002FB2C8
		public static Vector2I GetChunkIdx(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(cell);
			return new Vector2I(vector2I.x / 16, vector2I.y / 16);
		}

		// Token: 0x06007AD0 RID: 31440 RVA: 0x002FD0F4 File Offset: 0x002FB2F4
		public void AddBlock(int renderLayer, BuildingDef def, bool isReplacement, SimHashes element, int cell)
		{
			KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer> key = new KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>(def, BlockTileRenderer.GetRenderInfoLayer(isReplacement, element));
			BlockTileRenderer.RenderInfo renderInfo;
			if (!this.renderInfo.TryGetValue(key, out renderInfo))
			{
				int queryLayer = (int)(isReplacement ? def.ReplacementLayer : def.TileLayer);
				renderInfo = new BlockTileRenderer.RenderInfo(this, queryLayer, renderLayer, def, element);
				this.renderInfo[key] = renderInfo;
			}
			renderInfo.AddCell(cell);
		}

		// Token: 0x06007AD1 RID: 31441 RVA: 0x002FD154 File Offset: 0x002FB354
		public void RemoveBlock(BuildingDef def, bool isReplacement, SimHashes element, int cell)
		{
			KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer> key = new KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>(def, BlockTileRenderer.GetRenderInfoLayer(isReplacement, element));
			BlockTileRenderer.RenderInfo renderInfo;
			if (this.renderInfo.TryGetValue(key, out renderInfo))
			{
				renderInfo.RemoveCell(cell);
			}
		}

		// Token: 0x06007AD2 RID: 31442 RVA: 0x002FD188 File Offset: 0x002FB388
		public void Rebuild(ObjectLayer layer, int cell)
		{
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				if (keyValuePair.Key.Key.TileLayer == layer)
				{
					keyValuePair.Value.MarkDirty(cell);
				}
			}
		}

		// Token: 0x06007AD3 RID: 31443 RVA: 0x002FD1F8 File Offset: 0x002FB3F8
		public void SelectCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.selectedCell, cell, enabled);
		}

		// Token: 0x06007AD4 RID: 31444 RVA: 0x002FD208 File Offset: 0x002FB408
		public void HighlightCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.highlightCell, cell, enabled);
		}

		// Token: 0x06007AD5 RID: 31445 RVA: 0x002FD218 File Offset: 0x002FB418
		public void SetInvalidPlaceCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.invalidPlaceCell, cell, enabled);
		}

		// Token: 0x06007AD6 RID: 31446 RVA: 0x002FD228 File Offset: 0x002FB428
		private void UpdateCellStatus(ref int cell_status, int cell, bool enabled)
		{
			if (enabled)
			{
				if (cell == cell_status)
				{
					return;
				}
				if (cell_status != -1)
				{
					foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
					{
						keyValuePair.Value.MarkDirtyIfOccupied(cell_status);
					}
				}
				cell_status = cell;
				using (Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo>.Enumerator enumerator = this.renderInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair2 = enumerator.Current;
						keyValuePair2.Value.MarkDirtyIfOccupied(cell_status);
					}
					return;
				}
			}
			if (cell_status == cell)
			{
				foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair3 in this.renderInfo)
				{
					keyValuePair3.Value.MarkDirty(cell_status);
				}
				cell_status = -1;
			}
		}

		// Token: 0x040055A0 RID: 21920
		[SerializeField]
		private bool forceRebuild;

		// Token: 0x040055A1 RID: 21921
		[SerializeField]
		private Color highlightColour = new Color(1.25f, 1.25f, 1.25f, 1f);

		// Token: 0x040055A2 RID: 21922
		[SerializeField]
		private Color selectColour = new Color(1.5f, 1.5f, 1.5f, 1f);

		// Token: 0x040055A3 RID: 21923
		[SerializeField]
		private Color invalidPlaceColour = Color.red;

		// Token: 0x040055A4 RID: 21924
		private const float TILE_ATLAS_WIDTH = 2048f;

		// Token: 0x040055A5 RID: 21925
		private const float TILE_ATLAS_HEIGHT = 2048f;

		// Token: 0x040055A6 RID: 21926
		private const int chunkEdgeSize = 16;

		// Token: 0x040055A7 RID: 21927
		protected Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> renderInfo = new Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo>();

		// Token: 0x040055A8 RID: 21928
		private int selectedCell = -1;

		// Token: 0x040055A9 RID: 21929
		private int highlightCell = -1;

		// Token: 0x040055AA RID: 21930
		private int invalidPlaceCell = -1;

		// Token: 0x02002179 RID: 8569
		public enum RenderInfoLayer
		{
			// Token: 0x0400994F RID: 39247
			Built,
			// Token: 0x04009950 RID: 39248
			UnderConstruction,
			// Token: 0x04009951 RID: 39249
			Replacement
		}

		// Token: 0x0200217A RID: 8570
		[Flags]
		public enum Bits
		{
			// Token: 0x04009953 RID: 39251
			UpLeft = 128,
			// Token: 0x04009954 RID: 39252
			Up = 64,
			// Token: 0x04009955 RID: 39253
			UpRight = 32,
			// Token: 0x04009956 RID: 39254
			Left = 16,
			// Token: 0x04009957 RID: 39255
			Right = 8,
			// Token: 0x04009958 RID: 39256
			DownLeft = 4,
			// Token: 0x04009959 RID: 39257
			Down = 2,
			// Token: 0x0400995A RID: 39258
			DownRight = 1
		}

		// Token: 0x0200217B RID: 8571
		protected class RenderInfo
		{
			// Token: 0x0600BC49 RID: 48201 RVA: 0x003FF0F0 File Offset: 0x003FD2F0
			public RenderInfo(BlockTileRenderer renderer, int queryLayer, int renderLayer, BuildingDef def, SimHashes element)
			{
				this.queryLayer = queryLayer;
				this.renderLayer = renderLayer;
				this.rootPosition = new Vector3(0f, 0f, Grid.GetLayerZ(def.SceneLayer));
				this.element = element;
				this.material = new Material(def.BlockTileMaterial);
				if (def.BlockTileIsTransparent)
				{
					this.material.renderQueue = RenderQueues.Liquid;
					this.decorZOffset = Grid.GetLayerZ(Grid.SceneLayer.TileFront) - Grid.GetLayerZ(Grid.SceneLayer.Liquid) - 1f;
				}
				else if (def.SceneLayer == Grid.SceneLayer.TileMain)
				{
					this.material.renderQueue = RenderQueues.BlockTiles;
				}
				this.material.DisableKeyword("ENABLE_SHINE");
				if (element != SimHashes.Void)
				{
					this.material.SetTexture("_MainTex", def.BlockTileAtlas.texture);
					this.material.name = def.BlockTileAtlas.name + "Mat";
					if (def.BlockTileShineAtlas != null)
					{
						this.material.SetTexture("_SpecularTex", def.BlockTileShineAtlas.texture);
						this.material.EnableKeyword("ENABLE_SHINE");
					}
				}
				else
				{
					this.material.SetTexture("_MainTex", def.BlockTilePlaceAtlas.texture);
					this.material.name = def.BlockTilePlaceAtlas.name + "Mat";
				}
				int num = Grid.WidthInCells / 16 + 1;
				int num2 = Grid.HeightInCells / 16 + 1;
				this.meshChunks = new Mesh[num, num2];
				this.dirtyChunks = new bool[num, num2];
				for (int i = 0; i < num2; i++)
				{
					for (int j = 0; j < num; j++)
					{
						this.dirtyChunks[j, i] = true;
					}
				}
				BlockTileDecorInfo blockTileDecorInfo = (element == SimHashes.Void) ? def.DecorPlaceBlockTileInfo : def.DecorBlockTileInfo;
				if (blockTileDecorInfo)
				{
					this.decorRenderInfo = new BlockTileRenderer.DecorRenderInfo(num, num2, queryLayer, def, blockTileDecorInfo);
				}
				int num3 = def.BlockTileAtlas.items[0].name.Length - 4 - 8;
				int startIndex = num3 - 1 - 8;
				this.atlasInfo = new BlockTileRenderer.RenderInfo.AtlasInfo[def.BlockTileAtlas.items.Length];
				for (int k = 0; k < this.atlasInfo.Length; k++)
				{
					TextureAtlas.Item item = def.BlockTileAtlas.items[k];
					string value = item.name.Substring(startIndex, 8);
					string value2 = item.name.Substring(num3, 8);
					int requiredConnections = Convert.ToInt32(value, 2);
					int forbiddenConnections = Convert.ToInt32(value2, 2);
					this.atlasInfo[k].requiredConnections = (BlockTileRenderer.Bits)requiredConnections;
					this.atlasInfo[k].forbiddenConnections = (BlockTileRenderer.Bits)forbiddenConnections;
					this.atlasInfo[k].uvBox = item.uvBox;
					this.atlasInfo[k].name = item.name;
				}
				this.trimUVSize = new Vector2(0.03125f, 0.03125f);
			}

			// Token: 0x0600BC4A RID: 48202 RVA: 0x003FF42C File Offset: 0x003FD62C
			public void FreeResources()
			{
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
				this.atlasInfo = null;
				for (int i = 0; i < this.meshChunks.GetLength(0); i++)
				{
					for (int j = 0; j < this.meshChunks.GetLength(1); j++)
					{
						if (this.meshChunks[i, j] != null)
						{
							UnityEngine.Object.DestroyImmediate(this.meshChunks[i, j]);
							this.meshChunks[i, j] = null;
						}
					}
				}
				this.meshChunks = null;
				this.decorRenderInfo = null;
				this.occupiedCells.Clear();
			}

			// Token: 0x0600BC4B RID: 48203 RVA: 0x003FF4D0 File Offset: 0x003FD6D0
			public void AddCell(int cell)
			{
				int num = 0;
				this.occupiedCells.TryGetValue(cell, out num);
				this.occupiedCells[cell] = num + 1;
				this.MarkDirty(cell);
			}

			// Token: 0x0600BC4C RID: 48204 RVA: 0x003FF504 File Offset: 0x003FD704
			public void RemoveCell(int cell)
			{
				int num = 0;
				this.occupiedCells.TryGetValue(cell, out num);
				if (num > 1)
				{
					this.occupiedCells[cell] = num - 1;
				}
				else
				{
					this.occupiedCells.Remove(cell);
				}
				this.MarkDirty(cell);
			}

			// Token: 0x0600BC4D RID: 48205 RVA: 0x003FF54C File Offset: 0x003FD74C
			public void MarkDirty(int cell)
			{
				Vector2I chunkIdx = BlockTileRenderer.GetChunkIdx(cell);
				this.dirtyChunks[chunkIdx.x, chunkIdx.y] = true;
			}

			// Token: 0x0600BC4E RID: 48206 RVA: 0x003FF578 File Offset: 0x003FD778
			public void MarkDirtyIfOccupied(int cell)
			{
				if (this.occupiedCells.ContainsKey(cell))
				{
					this.MarkDirty(cell);
				}
			}

			// Token: 0x0600BC4F RID: 48207 RVA: 0x003FF590 File Offset: 0x003FD790
			public void Render(int x, int y)
			{
				if (this.meshChunks[x, y] != null)
				{
					Graphics.DrawMesh(this.meshChunks[x, y], this.rootPosition, Quaternion.identity, this.material, this.renderLayer);
				}
				if (this.decorRenderInfo != null)
				{
					this.decorRenderInfo.Render(x, y, this.rootPosition - new Vector3(0f, 0f, 0.5f), this.renderLayer);
				}
			}

			// Token: 0x0600BC50 RID: 48208 RVA: 0x003FF618 File Offset: 0x003FD818
			public void Rebuild(BlockTileRenderer renderer, int chunk_x, int chunk_y, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
			{
				if (!this.dirtyChunks[chunk_x, chunk_y] && !renderer.ForceRebuild)
				{
					return;
				}
				this.dirtyChunks[chunk_x, chunk_y] = false;
				vertices.Clear();
				uvs.Clear();
				indices.Clear();
				colours.Clear();
				for (int i = chunk_y * 16; i < chunk_y * 16 + 16; i++)
				{
					for (int j = chunk_x * 16; j < chunk_x * 16 + 16; j++)
					{
						int num = i * Grid.WidthInCells + j;
						if (this.occupiedCells.ContainsKey(num))
						{
							BlockTileRenderer.Bits connectionBits = renderer.GetConnectionBits(j, i, this.queryLayer);
							for (int k = 0; k < this.atlasInfo.Length; k++)
							{
								bool flag = (this.atlasInfo[k].requiredConnections & connectionBits) == this.atlasInfo[k].requiredConnections;
								bool flag2 = (this.atlasInfo[k].forbiddenConnections & connectionBits) > (BlockTileRenderer.Bits)0;
								if (flag && !flag2)
								{
									Color cellColour = renderer.GetCellColour(num, this.element);
									this.AddVertexInfo(this.atlasInfo[k], this.trimUVSize, j, i, connectionBits, cellColour, vertices, uvs, indices, colours);
									break;
								}
							}
						}
					}
				}
				Mesh mesh = this.meshChunks[chunk_x, chunk_y];
				if (vertices.Count > 0)
				{
					if (mesh == null)
					{
						mesh = new Mesh();
						mesh.name = "BlockTile";
						this.meshChunks[chunk_x, chunk_y] = mesh;
					}
					mesh.Clear();
					mesh.SetVertices(vertices);
					mesh.SetUVs(0, uvs);
					mesh.SetColors(colours);
					mesh.SetTriangles(indices, 0);
				}
				else if (mesh != null)
				{
					this.meshChunks[chunk_x, chunk_y] = null;
				}
				if (this.decorRenderInfo != null)
				{
					this.decorRenderInfo.Rebuild(renderer, this.occupiedCells, chunk_x, chunk_y, this.decorZOffset, 16, vertices, uvs, colours, indices, this.element);
				}
			}

			// Token: 0x0600BC51 RID: 48209 RVA: 0x003FF81C File Offset: 0x003FDA1C
			private void AddVertexInfo(BlockTileRenderer.RenderInfo.AtlasInfo atlas_info, Vector2 uv_trim_size, int x, int y, BlockTileRenderer.Bits connection_bits, Color color, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
			{
				Vector2 vector = new Vector2((float)x, (float)y);
				Vector2 vector2 = vector + new Vector2(1f, 1f);
				Vector2 vector3 = new Vector2(atlas_info.uvBox.x, atlas_info.uvBox.w);
				Vector2 vector4 = new Vector2(atlas_info.uvBox.z, atlas_info.uvBox.y);
				if ((connection_bits & BlockTileRenderer.Bits.Left) == (BlockTileRenderer.Bits)0)
				{
					vector.x -= 0.25f;
				}
				else
				{
					vector3.x += uv_trim_size.x;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Right) == (BlockTileRenderer.Bits)0)
				{
					vector2.x += 0.25f;
				}
				else
				{
					vector4.x -= uv_trim_size.x;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Up) == (BlockTileRenderer.Bits)0)
				{
					vector2.y += 0.25f;
				}
				else
				{
					vector4.y -= uv_trim_size.y;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Down) == (BlockTileRenderer.Bits)0)
				{
					vector.y -= 0.25f;
				}
				else
				{
					vector3.y += uv_trim_size.y;
				}
				int count = vertices.Count;
				vertices.Add(vector);
				vertices.Add(new Vector2(vector2.x, vector.y));
				vertices.Add(vector2);
				vertices.Add(new Vector2(vector.x, vector2.y));
				uvs.Add(vector3);
				uvs.Add(new Vector2(vector4.x, vector3.y));
				uvs.Add(vector4);
				uvs.Add(new Vector2(vector3.x, vector4.y));
				indices.Add(count);
				indices.Add(count + 1);
				indices.Add(count + 2);
				indices.Add(count);
				indices.Add(count + 2);
				indices.Add(count + 3);
				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
			}

			// Token: 0x0400995B RID: 39259
			private BlockTileRenderer.RenderInfo.AtlasInfo[] atlasInfo;

			// Token: 0x0400995C RID: 39260
			private bool[,] dirtyChunks;

			// Token: 0x0400995D RID: 39261
			private int queryLayer;

			// Token: 0x0400995E RID: 39262
			private Material material;

			// Token: 0x0400995F RID: 39263
			private int renderLayer;

			// Token: 0x04009960 RID: 39264
			private Mesh[,] meshChunks;

			// Token: 0x04009961 RID: 39265
			private BlockTileRenderer.DecorRenderInfo decorRenderInfo;

			// Token: 0x04009962 RID: 39266
			private Vector2 trimUVSize;

			// Token: 0x04009963 RID: 39267
			private Vector3 rootPosition;

			// Token: 0x04009964 RID: 39268
			private Dictionary<int, int> occupiedCells = new Dictionary<int, int>();

			// Token: 0x04009965 RID: 39269
			private SimHashes element;

			// Token: 0x04009966 RID: 39270
			private float decorZOffset = -1f;

			// Token: 0x04009967 RID: 39271
			private const float scale = 0.5f;

			// Token: 0x04009968 RID: 39272
			private const float core_size = 256f;

			// Token: 0x04009969 RID: 39273
			private const float trim_size = 64f;

			// Token: 0x0400996A RID: 39274
			private const float cell_size = 1f;

			// Token: 0x0400996B RID: 39275
			private const float world_trim_size = 0.25f;

			// Token: 0x02002A88 RID: 10888
			private struct AtlasInfo
			{
				// Token: 0x0400BB9B RID: 48027
				public BlockTileRenderer.Bits requiredConnections;

				// Token: 0x0400BB9C RID: 48028
				public BlockTileRenderer.Bits forbiddenConnections;

				// Token: 0x0400BB9D RID: 48029
				public Vector4 uvBox;

				// Token: 0x0400BB9E RID: 48030
				public string name;
			}
		}

		// Token: 0x0200217C RID: 8572
		private class DecorRenderInfo
		{
			// Token: 0x0600BC52 RID: 48210 RVA: 0x003FFA2C File Offset: 0x003FDC2C
			public DecorRenderInfo(int num_x_chunks, int num_y_chunks, int query_layer, BuildingDef def, BlockTileDecorInfo decorInfo)
			{
				this.decorInfo = decorInfo;
				this.queryLayer = query_layer;
				this.material = new Material(def.BlockTileMaterial);
				if (def.BlockTileIsTransparent)
				{
					this.material.renderQueue = RenderQueues.Liquid;
				}
				else if (def.SceneLayer == Grid.SceneLayer.TileMain)
				{
					this.material.renderQueue = RenderQueues.BlockTiles;
				}
				this.material.SetTexture("_MainTex", decorInfo.atlas.texture);
				if (decorInfo.atlasSpec != null)
				{
					this.material.SetTexture("_SpecularTex", decorInfo.atlasSpec.texture);
					this.material.EnableKeyword("ENABLE_SHINE");
				}
				else
				{
					this.material.DisableKeyword("ENABLE_SHINE");
				}
				this.meshChunks = new Mesh[num_x_chunks, num_y_chunks];
			}

			// Token: 0x0600BC53 RID: 48211 RVA: 0x003FFB18 File Offset: 0x003FDD18
			public void FreeResources()
			{
				this.decorInfo = null;
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
				for (int i = 0; i < this.meshChunks.GetLength(0); i++)
				{
					for (int j = 0; j < this.meshChunks.GetLength(1); j++)
					{
						if (this.meshChunks[i, j] != null)
						{
							UnityEngine.Object.DestroyImmediate(this.meshChunks[i, j]);
							this.meshChunks[i, j] = null;
						}
					}
				}
				this.meshChunks = null;
				this.triangles.Clear();
			}

			// Token: 0x0600BC54 RID: 48212 RVA: 0x003FFBB3 File Offset: 0x003FDDB3
			public void Render(int x, int y, Vector3 position, int renderLayer)
			{
				if (this.meshChunks[x, y] != null)
				{
					Graphics.DrawMesh(this.meshChunks[x, y], position, Quaternion.identity, this.material, renderLayer);
				}
			}

			// Token: 0x0600BC55 RID: 48213 RVA: 0x003FFBEC File Offset: 0x003FDDEC
			public void Rebuild(BlockTileRenderer renderer, Dictionary<int, int> occupiedCells, int chunk_x, int chunk_y, float z_offset, int chunkEdgeSize, List<Vector3> vertices, List<Vector2> uvs, List<Color> colours, List<int> indices, SimHashes element)
			{
				vertices.Clear();
				uvs.Clear();
				this.triangles.Clear();
				colours.Clear();
				indices.Clear();
				for (int i = chunk_y * chunkEdgeSize; i < chunk_y * chunkEdgeSize + chunkEdgeSize; i++)
				{
					for (int j = chunk_x * chunkEdgeSize; j < chunk_x * chunkEdgeSize + chunkEdgeSize; j++)
					{
						int num = i * Grid.WidthInCells + j;
						if (occupiedCells.ContainsKey(num))
						{
							Color cellColour = renderer.GetCellColour(num, element);
							BlockTileRenderer.Bits decorConnectionBits = renderer.GetDecorConnectionBits(j, i, this.queryLayer);
							this.AddDecor(j, i, z_offset, decorConnectionBits, cellColour, vertices, uvs, this.triangles, colours);
						}
					}
				}
				if (vertices.Count > 0)
				{
					Mesh mesh = this.meshChunks[chunk_x, chunk_y];
					if (mesh == null)
					{
						mesh = new Mesh();
						mesh.name = "DecorRender";
						this.meshChunks[chunk_x, chunk_y] = mesh;
					}
					this.triangles.Sort((BlockTileRenderer.DecorRenderInfo.TriangleInfo a, BlockTileRenderer.DecorRenderInfo.TriangleInfo b) => a.sortOrder.CompareTo(b.sortOrder));
					for (int k = 0; k < this.triangles.Count; k++)
					{
						indices.Add(this.triangles[k].i0);
						indices.Add(this.triangles[k].i1);
						indices.Add(this.triangles[k].i2);
					}
					mesh.Clear();
					mesh.SetVertices(vertices);
					mesh.SetUVs(0, uvs);
					mesh.SetColors(colours);
					mesh.SetTriangles(indices, 0);
					return;
				}
				this.meshChunks[chunk_x, chunk_y] = null;
			}

			// Token: 0x0600BC56 RID: 48214 RVA: 0x003FFDA8 File Offset: 0x003FDFA8
			private void AddDecor(int x, int y, float z_offset, BlockTileRenderer.Bits connection_bits, Color colour, List<Vector3> vertices, List<Vector2> uvs, List<BlockTileRenderer.DecorRenderInfo.TriangleInfo> triangles, List<Color> colours)
			{
				for (int i = 0; i < this.decorInfo.decor.Length; i++)
				{
					BlockTileDecorInfo.Decor decor = this.decorInfo.decor[i];
					if (decor.variants != null && decor.variants.Length != 0)
					{
						bool flag = (connection_bits & decor.requiredConnections) == decor.requiredConnections;
						bool flag2 = (connection_bits & decor.forbiddenConnections) > (BlockTileRenderer.Bits)0;
						if (flag && !flag2)
						{
							float num = PerlinSimplexNoise.noise((float)(i + x + connection_bits) * BlockTileRenderer.DecorRenderInfo.simplex_scale.x, (float)(i + y + connection_bits) * BlockTileRenderer.DecorRenderInfo.simplex_scale.y);
							if (num >= decor.probabilityCutoff)
							{
								int num2 = (int)((float)(decor.variants.Length - 1) * num);
								int count = vertices.Count;
								Vector3 b = new Vector3((float)x, (float)y, z_offset) + decor.variants[num2].offset;
								foreach (Vector3 a in decor.variants[num2].atlasItem.vertices)
								{
									vertices.Add(a + b);
									colours.Add(colour);
								}
								uvs.AddRange(decor.variants[num2].atlasItem.uvs);
								int[] indices = decor.variants[num2].atlasItem.indices;
								for (int k = 0; k < indices.Length; k += 3)
								{
									triangles.Add(new BlockTileRenderer.DecorRenderInfo.TriangleInfo
									{
										sortOrder = decor.sortOrder,
										i0 = indices[k] + count,
										i1 = indices[k + 1] + count,
										i2 = indices[k + 2] + count
									});
								}
							}
						}
					}
				}
			}

			// Token: 0x0400996C RID: 39276
			private int queryLayer;

			// Token: 0x0400996D RID: 39277
			private BlockTileDecorInfo decorInfo;

			// Token: 0x0400996E RID: 39278
			private Mesh[,] meshChunks;

			// Token: 0x0400996F RID: 39279
			private Material material;

			// Token: 0x04009970 RID: 39280
			private List<BlockTileRenderer.DecorRenderInfo.TriangleInfo> triangles = new List<BlockTileRenderer.DecorRenderInfo.TriangleInfo>();

			// Token: 0x04009971 RID: 39281
			private static Vector2 simplex_scale = new Vector2(92.41f, 87.16f);

			// Token: 0x02002A89 RID: 10889
			private struct TriangleInfo
			{
				// Token: 0x0400BB9F RID: 48031
				public int sortOrder;

				// Token: 0x0400BBA0 RID: 48032
				public int i0;

				// Token: 0x0400BBA1 RID: 48033
				public int i1;

				// Token: 0x0400BBA2 RID: 48034
				public int i2;
			}
		}
	}
}
