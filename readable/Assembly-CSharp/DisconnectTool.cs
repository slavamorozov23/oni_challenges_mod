using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009A9 RID: 2473
public class DisconnectTool : FilteredDragTool
{
	// Token: 0x06004755 RID: 18261 RVA: 0x0019CB59 File Offset: 0x0019AD59
	public static void DestroyInstance()
	{
		DisconnectTool.Instance = null;
	}

	// Token: 0x06004756 RID: 18262 RVA: 0x0019CB64 File Offset: 0x0019AD64
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisconnectTool.Instance = this;
		this.disconnectVisPool = new GameObjectPool(new Func<GameObject>(this.InstantiateDisconnectVis), delegate(GameObject _)
		{
		}, this.singleDisconnectMode ? 1 : 10);
		if (this.singleDisconnectMode)
		{
			this.lineModeMaxLength = 2;
		}
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x0019CBCF File Offset: 0x0019ADCF
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004758 RID: 18264 RVA: 0x0019CBDC File Offset: 0x0019ADDC
	protected override DragTool.Mode GetMode()
	{
		if (!this.singleDisconnectMode)
		{
			return DragTool.Mode.Box;
		}
		return DragTool.Mode.Line;
	}

	// Token: 0x06004759 RID: 18265 RVA: 0x0019CBE9 File Offset: 0x0019ADE9
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		if (this.singleDisconnectMode)
		{
			upPos = base.SnapToLine(upPos);
		}
		this.RunOnRegion(downPos, upPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.DisconnectCellsAction));
		this.ClearVisualizers();
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x0019CC16 File Offset: 0x0019AE16
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.lastRefreshedCell = -1;
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x0019CC28 File Offset: 0x0019AE28
	private void DisconnectCellsAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		Building component = objectOnCell.GetComponent<Building>();
		KAnimGraphTileVisualizer component2 = objectOnCell.GetComponent<KAnimGraphTileVisualizer>();
		if (component2 != null)
		{
			UtilityConnections new_connections = utilityComponent.GetNetworkManager().GetConnections(cell, false) & ~removeConnections;
			component2.UpdateConnections(new_connections);
			component2.Refresh();
		}
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		utilityComponent.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x0019CC94 File Offset: 0x0019AE94
	private void RunOnRegion(Vector3 pos1, Vector3 pos2, Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections> action)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(pos1, pos2), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(pos1, pos2), false);
		Vector2I vector2I = new Vector2I((int)regularizedPos.x, (int)regularizedPos.y);
		Vector2I vector2I2 = new Vector2I((int)regularizedPos2.x, (int)regularizedPos2.y);
		for (int i = vector2I.x; i < vector2I2.x; i++)
		{
			for (int j = vector2I.y; j < vector2I2.y; j++)
			{
				int num = Grid.XYToCell(i, j);
				if (Grid.IsVisible(num))
				{
					for (int k = 0; k < 45; k++)
					{
						GameObject gameObject = Grid.Objects[num, k];
						if (!(gameObject == null))
						{
							string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
							if (base.IsActiveLayer(filterLayerFromGameObject))
							{
								Building component = gameObject.GetComponent<Building>();
								if (!(component == null))
								{
									IHaveUtilityNetworkMgr component2 = component.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
									if (!component2.IsNullOrDestroyed())
									{
										UtilityConnections connections = component2.GetNetworkManager().GetConnections(num, false);
										UtilityConnections utilityConnections = (UtilityConnections)0;
										if ((connections & UtilityConnections.Left) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, -1, 0))
										{
											utilityConnections |= UtilityConnections.Left;
										}
										if ((connections & UtilityConnections.Right) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 1, 0))
										{
											utilityConnections |= UtilityConnections.Right;
										}
										if ((connections & UtilityConnections.Up) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, 1))
										{
											utilityConnections |= UtilityConnections.Up;
										}
										if ((connections & UtilityConnections.Down) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, -1))
										{
											utilityConnections |= UtilityConnections.Down;
										}
										if (utilityConnections > (UtilityConnections)0)
										{
											action(num, gameObject, component2, utilityConnections);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x0019CE60 File Offset: 0x0019B060
	private bool IsInsideRegion(Vector2I min, Vector2I max, int cell, int xoff, int yoff)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, xoff, yoff), out num, out num2);
		return num >= min.x && num < max.x && num2 >= min.y && num2 < max.y;
	}

	// Token: 0x0600475E RID: 18270 RVA: 0x0019CEA8 File Offset: 0x0019B0A8
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		if (!base.Dragging)
		{
			return;
		}
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.singleDisconnectMode)
		{
			cursorPos = base.SnapToLine(cursorPos);
		}
		int num = Grid.PosToCell(cursorPos);
		if (this.lastRefreshedCell == num)
		{
			return;
		}
		this.lastRefreshedCell = num;
		this.ClearVisualizers();
		this.RunOnRegion(this.downPos, cursorPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.VisualizeAction));
	}

	// Token: 0x0600475F RID: 18271 RVA: 0x0019CF20 File Offset: 0x0019B120
	private GameObject InstantiateDisconnectVis()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.singleDisconnectMode ? this.disconnectVisSingleModePrefab : this.disconnectVisMultiModePrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06004760 RID: 18272 RVA: 0x0019CF48 File Offset: 0x0019B148
	private void VisualizeAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		if ((removeConnections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellBelow(cell), true);
		}
		if ((removeConnections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellRight(cell), false);
		}
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x0019CF74 File Offset: 0x0019B174
	private void CreateVisualizer(int cell1, int cell2, bool rotate)
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			if (visData.Equals(cell1, cell2))
			{
				return;
			}
		}
		Vector3 a = Grid.CellToPosCCC(cell1, Grid.SceneLayer.FXFront);
		Vector3 b = Grid.CellToPosCCC(cell2, Grid.SceneLayer.FXFront);
		GameObject instance = this.disconnectVisPool.GetInstance();
		instance.transform.rotation = Quaternion.Euler(0f, 0f, (float)(rotate ? 90 : 0));
		instance.transform.SetPosition(Vector3.Lerp(a, b, 0.5f));
		instance.SetActive(true);
		this.visualizersInUse.Add(new DisconnectTool.VisData(cell1, cell2, instance));
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x0019D044 File Offset: 0x0019B244
	private void ClearVisualizers()
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			visData.go.SetActive(false);
			this.disconnectVisPool.ReleaseInstance(visData.go);
		}
		this.visualizersInUse.Clear();
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x0019D0B8 File Offset: 0x0019B2B8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.ClearVisualizers();
	}

	// Token: 0x06004764 RID: 18276 RVA: 0x0019D0C7 File Offset: 0x0019B2C7
	protected override string GetConfirmSound()
	{
		return "OutletDisconnected";
	}

	// Token: 0x06004765 RID: 18277 RVA: 0x0019D0CE File Offset: 0x0019B2CE
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06004766 RID: 18278 RVA: 0x0019D0D8 File Offset: 0x0019B2D8
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x04002FCE RID: 12238
	[SerializeField]
	private GameObject disconnectVisSingleModePrefab;

	// Token: 0x04002FCF RID: 12239
	[SerializeField]
	private GameObject disconnectVisMultiModePrefab;

	// Token: 0x04002FD0 RID: 12240
	private GameObjectPool disconnectVisPool;

	// Token: 0x04002FD1 RID: 12241
	private List<DisconnectTool.VisData> visualizersInUse = new List<DisconnectTool.VisData>();

	// Token: 0x04002FD2 RID: 12242
	private int lastRefreshedCell;

	// Token: 0x04002FD3 RID: 12243
	private bool singleDisconnectMode = true;

	// Token: 0x04002FD4 RID: 12244
	public static DisconnectTool Instance;

	// Token: 0x02001A0B RID: 6667
	public struct VisData
	{
		// Token: 0x0600A41B RID: 42011 RVA: 0x003B3B46 File Offset: 0x003B1D46
		public VisData(int cell1, int cell2, GameObject go)
		{
			this.cell1 = cell1;
			this.cell2 = cell2;
			this.go = go;
		}

		// Token: 0x0600A41C RID: 42012 RVA: 0x003B3B5D File Offset: 0x003B1D5D
		public bool Equals(int cell1, int cell2)
		{
			return (this.cell1 == cell1 && this.cell2 == cell2) || (this.cell1 == cell2 && this.cell2 == cell1);
		}

		// Token: 0x0400805F RID: 32863
		public readonly int cell1;

		// Token: 0x04008060 RID: 32864
		public readonly int cell2;

		// Token: 0x04008061 RID: 32865
		public GameObject go;
	}
}
