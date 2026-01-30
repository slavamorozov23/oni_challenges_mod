using System;
using System.Collections.Generic;
using FMOD.Studio;
using Rendering;
using STRINGS;
using UnityEngine;

// Token: 0x020009A0 RID: 2464
public class BuildTool : DragTool
{
	// Token: 0x060046F3 RID: 18163 RVA: 0x0019AD95 File Offset: 0x00198F95
	public static void DestroyInstance()
	{
		BuildTool.Instance = null;
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x0019AD9D File Offset: 0x00198F9D
	protected override void OnPrefabInit()
	{
		BuildTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
		this.buildingCount = UnityEngine.Random.Range(1, 14);
		this.canChangeDragAxis = false;
	}

	// Token: 0x060046F5 RID: 18165 RVA: 0x0019ADC8 File Offset: 0x00198FC8
	protected override void OnActivateTool()
	{
		this.lastDragCell = -1;
		if (this.visualizer != null)
		{
			this.ClearTilePreview();
			UnityEngine.Object.Destroy(this.visualizer);
		}
		this.active = true;
		base.OnActivateTool();
		Vector3 vector = base.ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
		this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, vector, Grid.SceneLayer.Ore, null, LayerMask.NameToLayer("Place"));
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.visibilityType = KAnimControllerBase.VisibilityType.Always;
			component.isMovable = true;
			component.Offset = this.def.GetVisualizerOffset();
			component.name = component.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
		}
		if (!this.facadeID.IsNullOrWhiteSpace() && this.facadeID != "DEFAULT_FACADE")
		{
			this.visualizer.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeID), false);
		}
		Rotatable component2 = this.visualizer.GetComponent<Rotatable>();
		if (component2 != null)
		{
			this.buildingOrientation = this.def.InitialOrientation;
			component2.SetOrientation(this.buildingOrientation);
		}
		this.visualizer.SetActive(true);
		this.UpdateVis(vector);
		base.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		if (component == null)
		{
			this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		}
		else
		{
			component.SetLayer(LayerMask.NameToLayer("Place"));
		}
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x060046F6 RID: 18166 RVA: 0x0019AF78 File Offset: 0x00199178
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.lastDragCell = -1;
		if (!this.active)
		{
			return;
		}
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.buildingOrientation = Orientation.Neutral;
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		this.ClearTilePreview();
		UnityEngine.Object.Destroy(this.visualizer);
		if (new_tool == SelectTool.Instance)
		{
			Game.Instance.Trigger(-1190690038, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x060046F7 RID: 18167 RVA: 0x0019AFF3 File Offset: 0x001991F3
	public void Activate(BuildingDef def, IList<Tag> selected_elements)
	{
		this.selectedElements = selected_elements;
		this.def = def;
		this.viewMode = def.ViewMode;
		ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
		PlayerController.Instance.ActivateTool(this);
		this.OnActivateTool();
	}

	// Token: 0x060046F8 RID: 18168 RVA: 0x0019B031 File Offset: 0x00199231
	public void Activate(BuildingDef def, IList<Tag> selected_elements, string facadeID)
	{
		this.facadeID = facadeID;
		this.Activate(def, selected_elements);
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x0019B042 File Offset: 0x00199242
	public void Deactivate()
	{
		this.selectedElements = null;
		SelectTool.Instance.Activate();
		this.def = null;
		this.facadeID = null;
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x060046FA RID: 18170 RVA: 0x0019B06D File Offset: 0x0019926D
	public int GetLastCell
	{
		get
		{
			return this.lastCell;
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x060046FB RID: 18171 RVA: 0x0019B075 File Offset: 0x00199275
	public Orientation GetBuildingOrientation
	{
		get
		{
			return this.buildingOrientation;
		}
	}

	// Token: 0x060046FC RID: 18172 RVA: 0x0019B080 File Offset: 0x00199280
	private void ClearTilePreview()
	{
		if (Grid.IsValidBuildingCell(this.lastCell) && this.def.IsTilePiece)
		{
			GameObject gameObject = Grid.Objects[this.lastCell, (int)this.def.TileLayer];
			if (this.visualizer == gameObject)
			{
				Grid.Objects[this.lastCell, (int)this.def.TileLayer] = null;
			}
			if (this.def.isKAnimTile)
			{
				GameObject x = null;
				if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
				{
					x = Grid.Objects[this.lastCell, (int)this.def.ReplacementLayer];
				}
				if ((gameObject == null || gameObject.GetComponent<Constructable>() == null) && (x == null || x == this.visualizer))
				{
					World.Instance.blockTileRenderer.RemoveBlock(this.def, false, SimHashes.Void, this.lastCell);
					World.Instance.blockTileRenderer.RemoveBlock(this.def, true, SimHashes.Void, this.lastCell);
					TileVisualizer.RefreshCell(this.lastCell, this.def.TileLayer, this.def.ReplacementLayer);
				}
			}
		}
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x0019B1C1 File Offset: 0x001993C1
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		this.UpdateVis(cursorPos);
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x0019B1E4 File Offset: 0x001993E4
	private void UpdateVis(Vector3 pos)
	{
		string text;
		bool flag = this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text);
		bool flag2 = this.def.IsValidReplaceLocation(pos, this.buildingOrientation, this.def.ReplacementLayer, this.def.ObjectLayer);
		flag = (flag || flag2);
		if (this.visualizer != null)
		{
			Color c = Color.white;
			float strength = 0f;
			if (!flag)
			{
				c = Color.red;
				strength = 1f;
			}
			this.SetColor(this.visualizer, c, strength);
		}
		int num = Grid.PosToCell(pos);
		if (this.def != null)
		{
			Vector3 vector = Grid.CellToPosCBC(num, this.def.SceneLayer);
			this.visualizer.transform.SetPosition(vector);
			base.transform.SetPosition(vector - Vector3.up * 0.5f);
			if (this.def.IsTilePiece)
			{
				this.ClearTilePreview();
				if (Grid.IsValidBuildingCell(num))
				{
					GameObject gameObject = Grid.Objects[num, (int)this.def.TileLayer];
					if (gameObject == null)
					{
						Grid.Objects[num, (int)this.def.TileLayer] = this.visualizer;
					}
					if (this.def.isKAnimTile)
					{
						GameObject x = null;
						if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
						{
							x = Grid.Objects[num, (int)this.def.ReplacementLayer];
						}
						if (gameObject == null || (gameObject.GetComponent<Constructable>() == null && x == null))
						{
							TileVisualizer.RefreshCell(num, this.def.TileLayer, this.def.ReplacementLayer);
							if (this.def.BlockTileAtlas != null)
							{
								int renderLayer = LayerMask.NameToLayer("Overlay");
								BlockTileRenderer blockTileRenderer = World.Instance.blockTileRenderer;
								blockTileRenderer.SetInvalidPlaceCell(num, !flag);
								if (this.lastCell != num)
								{
									blockTileRenderer.SetInvalidPlaceCell(this.lastCell, false);
								}
								blockTileRenderer.AddBlock(renderLayer, this.def, flag2, SimHashes.Void, num);
							}
						}
					}
				}
			}
			if (this.lastCell != num)
			{
				this.lastCell = num;
			}
		}
	}

	// Token: 0x060046FF RID: 18175 RVA: 0x0019B428 File Offset: 0x00199628
	public PermittedRotations? GetPermittedRotations()
	{
		if (this.visualizer == null)
		{
			return null;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return null;
		}
		return new PermittedRotations?(component.permittedRotations);
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x0019B477 File Offset: 0x00199677
	public bool CanRotate()
	{
		return !(this.visualizer == null) && !(this.visualizer.GetComponent<Rotatable>() == null);
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x0019B4A0 File Offset: 0x001996A0
	public void TryRotate()
	{
		if (this.visualizer == null)
		{
			return;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Rotate", false));
		this.buildingOrientation = component.Rotate();
		if (Grid.IsValidBuildingCell(this.lastCell))
		{
			Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
			this.UpdateVis(pos);
		}
		if (base.Dragging && this.lastDragCell != -1)
		{
			this.TryBuild(this.lastDragCell);
		}
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x0019B52D File Offset: 0x0019972D
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.RotateBuilding))
		{
			this.TryRotate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x0019B54A File Offset: 0x0019974A
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.TryBuild(cell);
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x0019B554 File Offset: 0x00199754
	private void TryBuild(int cell)
	{
		if (this.visualizer == null)
		{
			return;
		}
		if (cell == this.lastDragCell && this.buildingOrientation == this.lastDragOrientation)
		{
			return;
		}
		if (Grid.PosToCell(this.visualizer) != cell)
		{
			if (this.def.BuildingComplete.GetComponent<LogicPorts>())
			{
				return;
			}
			if (this.def.BuildingComplete.GetComponent<LogicGateBase>())
			{
				return;
			}
		}
		this.lastDragCell = cell;
		this.lastDragOrientation = this.buildingOrientation;
		this.ClearTilePreview();
		Vector3 pos = Grid.CellToPosCBC(cell, Grid.SceneLayer.Building);
		GameObject gameObject = null;
		PlanScreen.Instance.LastSelectedBuildingFacade = this.facadeID;
		bool flag = DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild);
		string text;
		if (!flag)
		{
			gameObject = this.def.TryPlace(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
		}
		else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, false) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text))
		{
			if (this.def.ObjectLayer == ObjectLayer.Building)
			{
				this.def.RunOnArea(cell, this.buildingOrientation, delegate(int offset_cell)
				{
					Uprootable uprootable;
					if (Uprootable.CanUproot(Grid.Objects[offset_cell, (int)this.def.ObjectLayer], out uprootable))
					{
						uprootable.CompleteWork(null);
					}
				});
			}
			float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			gameObject = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
		}
		if (gameObject == null && this.def.ReplacementLayer != ObjectLayer.NumLayers)
		{
			GameObject replacementCandidate = this.def.GetReplacementCandidate(cell);
			if (replacementCandidate != null && !this.def.IsReplacementLayerOccupied(cell))
			{
				BuildingComplete component = replacementCandidate.GetComponent<BuildingComplete>();
				if (component != null && component.Def.Replaceable && this.def.CanReplace(replacementCandidate))
				{
					Tag b2 = replacementCandidate.GetComponent<PrimaryElement>().Element.tag;
					if (b2.GetHash() == 1542131326)
					{
						b2 = SimHashes.Snow.CreateTag();
					}
					if (component.Def != this.def || this.selectedElements[0] != b2)
					{
						string text2;
						if (!flag)
						{
							gameObject = this.def.TryReplaceTile(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
							Grid.Objects[cell, (int)this.def.ReplacementLayer] = gameObject;
						}
						else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, true) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, true, out text2))
						{
							gameObject = this.InstantBuildReplace(cell, pos, replacementCandidate);
						}
					}
				}
			}
		}
		this.PostProcessBuild(flag, pos, gameObject);
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x0019B874 File Offset: 0x00199A74
	private GameObject InstantBuildReplace(int cell, Vector3 pos, GameObject tile)
	{
		if (tile.GetComponent<SimCellOccupier>() == null)
		{
			UnityEngine.Object.Destroy(tile);
			float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			return this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
		}
		tile.GetComponent<SimCellOccupier>().DestroySelf(delegate
		{
			UnityEngine.Object.Destroy(tile);
			float b2 = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			GameObject builtItem = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b2), this.facadeID, false, GameClock.Instance.GetTime());
			this.PostProcessBuild(true, pos, builtItem);
		});
		return null;
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x0019B934 File Offset: 0x00199B34
	private void PostProcessBuild(bool instantBuild, Vector3 pos, GameObject builtItem)
	{
		if (builtItem == null)
		{
			return;
		}
		if (!instantBuild)
		{
			Prioritizable component = builtItem.GetComponent<Prioritizable>();
			if (component != null)
			{
				if (BuildMenu.Instance != null)
				{
					component.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
				}
				if (PlanScreen.Instance != null)
				{
					component.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
				}
			}
		}
		if (this.def.MaterialsAvailable(this.selectedElements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
		{
			this.placeSound = GlobalAssets.GetSound("Place_Building_" + this.def.AudioSize, false);
			if (this.placeSound != null)
			{
				this.buildingCount = this.buildingCount % 14 + 1;
				Vector3 pos2 = pos;
				pos2.z = 0f;
				EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, pos2, 1f, false);
				if (this.def.AudioSize == "small")
				{
					instance.setParameterByName("tileCount", (float)this.buildingCount, false);
				}
				SoundEvent.EndOneShot(instance);
			}
		}
		else
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, pos, 1.5f, false, false);
		}
		if (this.def.OnePerWorld)
		{
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x0019BA96 File Offset: 0x00199C96
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06004708 RID: 18184 RVA: 0x0019BA9C File Offset: 0x00199C9C
	private void SetColor(GameObject root, Color c, float strength)
	{
		KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.TintColour = c;
		}
	}

	// Token: 0x06004709 RID: 18185 RVA: 0x0019BAC5 File Offset: 0x00199CC5
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x0600470A RID: 18186 RVA: 0x0019BAD7 File Offset: 0x00199CD7
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x0019BAEC File Offset: 0x00199CEC
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x0019BB26 File Offset: 0x00199D26
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x0019BB2D File Offset: 0x00199D2D
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x0019BB36 File Offset: 0x00199D36
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x0019BB40 File Offset: 0x00199D40
	public void SetToolOrientation(Orientation orientation)
	{
		if (this.visualizer != null)
		{
			Rotatable component = this.visualizer.GetComponent<Rotatable>();
			if (component != null)
			{
				this.buildingOrientation = orientation;
				component.SetOrientation(orientation);
				if (Grid.IsValidBuildingCell(this.lastCell))
				{
					Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
					this.UpdateVis(pos);
				}
				if (base.Dragging && this.lastDragCell != -1)
				{
					this.TryBuild(this.lastDragCell);
				}
			}
		}
	}

	// Token: 0x04002FB4 RID: 12212
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x04002FB5 RID: 12213
	private int lastCell = -1;

	// Token: 0x04002FB6 RID: 12214
	private int lastDragCell = -1;

	// Token: 0x04002FB7 RID: 12215
	private Orientation lastDragOrientation;

	// Token: 0x04002FB8 RID: 12216
	private IList<Tag> selectedElements;

	// Token: 0x04002FB9 RID: 12217
	private BuildingDef def;

	// Token: 0x04002FBA RID: 12218
	private Orientation buildingOrientation;

	// Token: 0x04002FBB RID: 12219
	private string facadeID;

	// Token: 0x04002FBC RID: 12220
	private ToolTip tooltip;

	// Token: 0x04002FBD RID: 12221
	public static BuildTool Instance;

	// Token: 0x04002FBE RID: 12222
	private bool active;

	// Token: 0x04002FBF RID: 12223
	private int buildingCount;
}
