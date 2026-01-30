using System;
using System.Collections.Generic;
using Klei;
using Rendering;
using UnityEngine;

// Token: 0x02000C23 RID: 3107
[AddComponentMenu("KMonoBehaviour/scripts/World")]
public class World : KMonoBehaviour
{
	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06005D7C RID: 23932 RVA: 0x0021CE18 File Offset: 0x0021B018
	// (set) Token: 0x06005D7D RID: 23933 RVA: 0x0021CE1F File Offset: 0x0021B01F
	public static World Instance { get; private set; }

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06005D7E RID: 23934 RVA: 0x0021CE27 File Offset: 0x0021B027
	// (set) Token: 0x06005D7F RID: 23935 RVA: 0x0021CE2F File Offset: 0x0021B02F
	public SubworldZoneRenderData zoneRenderData { get; private set; }

	// Token: 0x06005D80 RID: 23936 RVA: 0x0021CE38 File Offset: 0x0021B038
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(World.Instance == null);
		World.Instance = this;
		this.blockTileRenderer = base.GetComponent<BlockTileRenderer>();
	}

	// Token: 0x06005D81 RID: 23937 RVA: 0x0021CE5C File Offset: 0x0021B05C
	protected override void OnSpawn()
	{
		base.GetComponent<SimDebugView>().OnReset();
		base.GetComponent<PropertyTextures>().OnReset(null);
		this.zoneRenderData = base.GetComponent<SubworldZoneRenderData>();
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(this.OnReveal));
	}

	// Token: 0x06005D82 RID: 23938 RVA: 0x0021CEAC File Offset: 0x0021B0AC
	protected override void OnLoadLevel()
	{
		World.Instance = null;
		if (this.blockTileRenderer != null)
		{
			this.blockTileRenderer.FreeResources();
		}
		this.blockTileRenderer = null;
		if (this.groundRenderer != null)
		{
			this.groundRenderer.FreeResources();
		}
		this.groundRenderer = null;
		this.revealedCells.Clear();
		this.revealedCells = null;
		base.OnLoadLevel();
	}

	// Token: 0x06005D83 RID: 23939 RVA: 0x0021CF18 File Offset: 0x0021B118
	public unsafe void UpdateCellInfo(List<SolidInfo> solidInfo, List<CallbackInfo> callbackInfo, int num_solid_substance_change_info, Sim.SolidSubstanceChangeInfo* solid_substance_change_info, int num_liquid_change_info, Sim.LiquidChangeInfo* liquid_change_info)
	{
		int count = solidInfo.Count;
		this.changedCells.Clear();
		for (int i = 0; i < count; i++)
		{
			int cellIdx = solidInfo[i].cellIdx;
			if (!this.changedCells.Contains(cellIdx))
			{
				this.changedCells.Add(cellIdx);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(cellIdx);
			WorldDamage.Instance.OnSolidStateChanged(cellIdx);
			if (this.OnSolidChanged != null)
			{
				this.OnSolidChanged(cellIdx);
			}
		}
		if (this.changedCells.Count != 0)
		{
			SaveGame.Instance.entombedItemManager.OnSolidChanged(this.changedCells);
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
		int count2 = callbackInfo.Count;
		for (int j = 0; j < count2; j++)
		{
			callbackInfo[j].Release();
		}
		for (int k = 0; k < num_solid_substance_change_info; k++)
		{
			int cellIdx2 = solid_substance_change_info[k].cellIdx;
			if (!Grid.IsValidCell(cellIdx2))
			{
				global::Debug.LogError(cellIdx2);
			}
			else
			{
				Grid.RenderedByWorld[cellIdx2] = (Grid.Element[cellIdx2].substance.renderedByWorld && Grid.Objects[cellIdx2, 9] == null);
				this.groundRenderer.MarkDirty(cellIdx2);
			}
		}
		GameScenePartitioner instance = GameScenePartitioner.Instance;
		this.changedCells.Clear();
		for (int l = 0; l < num_liquid_change_info; l++)
		{
			int cellIdx3 = liquid_change_info[l].cellIdx;
			this.changedCells.Add(cellIdx3);
			if (this.OnLiquidChanged != null)
			{
				this.OnLiquidChanged(cellIdx3);
			}
		}
		instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.liquidChangedLayer, null);
	}

	// Token: 0x06005D84 RID: 23940 RVA: 0x0021D0ED File Offset: 0x0021B2ED
	private void OnReveal(int cell)
	{
		this.revealedCells.Add(cell);
	}

	// Token: 0x06005D85 RID: 23941 RVA: 0x0021D0FC File Offset: 0x0021B2FC
	private void LateUpdate()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		if (GameUtil.IsCapturingTimeLapse())
		{
			this.groundRenderer.RenderAll();
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		else
		{
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			this.groundRenderer.Render(visibleArea.Min, visibleArea.Max, false);
			Vector2I vis_chunk_min;
			Vector2I vis_chunk_max;
			Singleton<KBatchedAnimUpdater>.Instance.GetVisibleArea(out vis_chunk_min, out vis_chunk_max);
			KAnimBatchManager.Instance().UpdateActiveArea(vis_chunk_min, vis_chunk_max);
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		if (Camera.main != null)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -Camera.main.transform.GetPosition().z));
			Shader.SetGlobalVector("_CursorPos", new Vector4(vector.x, vector.y, vector.z, 0f));
		}
		FallingWater.instance.UpdateParticles(Time.deltaTime);
		FallingWater.instance.Render();
		if (this.revealedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.revealedCells, GameScenePartitioner.Instance.fogOfWarChangedLayer, null);
			this.revealedCells.Clear();
		}
	}

	// Token: 0x04003E3D RID: 15933
	public Action<int> OnSolidChanged;

	// Token: 0x04003E3E RID: 15934
	public Action<int> OnLiquidChanged;

	// Token: 0x04003E40 RID: 15936
	public BlockTileRenderer blockTileRenderer;

	// Token: 0x04003E41 RID: 15937
	[MyCmpGet]
	[NonSerialized]
	public GroundRenderer groundRenderer;

	// Token: 0x04003E42 RID: 15938
	private List<int> revealedCells = new List<int>();

	// Token: 0x04003E43 RID: 15939
	public static int DebugCellID = -1;

	// Token: 0x04003E44 RID: 15940
	private List<int> changedCells = new List<int>();
}
