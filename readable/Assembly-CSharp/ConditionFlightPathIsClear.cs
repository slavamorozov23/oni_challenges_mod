using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BBE RID: 3006
public class ConditionFlightPathIsClear : ProcessCondition
{
	// Token: 0x06005A3E RID: 23102 RVA: 0x0020BDA8 File Offset: 0x00209FA8
	public ConditionFlightPathIsClear(GameObject module, int bufferWidth)
	{
		this.module = module.GetComponent<RocketModule>();
		if (this.module is RocketModuleCluster)
		{
			this.moduleInterface = (this.module as RocketModuleCluster).CraftInterface;
		}
		this.bufferWidth = bufferWidth;
	}

	// Token: 0x06005A3F RID: 23103 RVA: 0x0020BDF8 File Offset: 0x00209FF8
	public override ProcessCondition.Status EvaluateCondition()
	{
		this.Update();
		if (!this.hasClearSky)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A40 RID: 23104 RVA: 0x0020BE0B File Offset: 0x0020A00B
	public override StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Failure)
		{
			return Db.Get().BuildingStatusItems.PathNotClear;
		}
		return null;
	}

	// Token: 0x06005A41 RID: 23105 RVA: 0x0020BE24 File Offset: 0x0020A024
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.FAILURE;
		}
		if (status != ProcessCondition.Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationText;
		}
		global::Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	// Token: 0x06005A42 RID: 23106 RVA: 0x0020BE78 File Offset: 0x0020A078
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.FAILURE;
		}
		if (status != ProcessCondition.Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationTooltipText;
		}
		global::Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	// Token: 0x06005A43 RID: 23107 RVA: 0x0020BECA File Offset: 0x0020A0CA
	public override bool ShowInUI()
	{
		return DlcManager.FeatureClusterSpaceEnabled();
	}

	// Token: 0x06005A44 RID: 23108 RVA: 0x0020BED4 File Offset: 0x0020A0D4
	public void Update()
	{
		List<Building> list = new List<Building>();
		if (this.moduleInterface != null)
		{
			using (List<Ref<RocketModuleCluster>>.Enumerator enumerator = new List<Ref<RocketModuleCluster>>(this.moduleInterface.ClusterModules).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<RocketModuleCluster> @ref = enumerator.Current;
					list.Add(@ref.Get().GetComponent<Building>());
				}
				goto IL_A6;
			}
		}
		foreach (RocketModule rocketModule in this.module.FindLaunchConditionManager().rocketModules)
		{
			list.Add(rocketModule.GetComponent<Building>());
		}
		IL_A6:
		list.Sort(delegate(Building a, Building b)
		{
			int y = Grid.PosToXY(a.transform.GetPosition()).y;
			int y2 = Grid.PosToXY(b.transform.GetPosition()).y;
			return y.CompareTo(y2);
		});
		if (this.moduleInterface != null && this.moduleInterface.CurrentPad == null)
		{
			this.hasClearSky = false;
			return;
		}
		this.hasClearSky = true;
		int num = -1;
		int num2 = 0;
		while (this.hasClearSky && num2 < list.Count)
		{
			Building building = list[num2];
			this.hasClearSky = ConditionFlightPathIsClear.HasModuleAccessToSpace(building, out num);
			num2++;
		}
	}

	// Token: 0x06005A45 RID: 23109 RVA: 0x0020C030 File Offset: 0x0020A230
	public static bool HasModuleAccessToSpace(Building module, out int obstructionCell)
	{
		WorldContainer myWorld = module.GetMyWorld();
		obstructionCell = -1;
		if (myWorld.id == 255)
		{
			return false;
		}
		int num = (int)myWorld.maximumBounds.y;
		Extents extents = module.GetExtents();
		int cell = Grid.XYToCell(extents.x, extents.y);
		bool result = true;
		for (int i = 0; i < extents.width; i++)
		{
			int num2 = Grid.OffsetCell(cell, new CellOffset(i, 0));
			while (!Grid.IsSolidCell(num2) && Grid.CellToXY(num2).y < num)
			{
				num2 = Grid.CellAbove(num2);
			}
			if (Grid.IsSolidCell(num2) || Grid.CellToXY(num2).y != num)
			{
				obstructionCell = num2;
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005A46 RID: 23110 RVA: 0x0020C0EC File Offset: 0x0020A2EC
	public static int PadTopEdgeDistanceToOutOfScreenEdge(GameObject launchpad)
	{
		WorldContainer myWorld = launchpad.GetMyWorld();
		Vector2 maximumBounds = myWorld.maximumBounds;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return (int)CameraController.GetHighestVisibleCell_Height((byte)myWorld.ParentWorldId) - y + 10;
	}

	// Token: 0x06005A47 RID: 23111 RVA: 0x0020C130 File Offset: 0x0020A330
	public static int PadTopEdgeDistanceToCeilingEdge(GameObject launchpad)
	{
		Vector2 maximumBounds = launchpad.GetMyWorld().maximumBounds;
		int num = (int)launchpad.GetMyWorld().maximumBounds.y;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return num - Grid.TopBorderHeight - y + 1;
	}

	// Token: 0x06005A48 RID: 23112 RVA: 0x0020C17C File Offset: 0x0020A37C
	public static bool CheckFlightPathClear(CraftModuleInterface craft, GameObject launchpad, out int obstruction)
	{
		Vector2I vector2I = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition);
		int num = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(launchpad);
		foreach (Ref<RocketModuleCluster> @ref in craft.ClusterModules)
		{
			Building component = @ref.Get().GetComponent<Building>();
			int widthInCells = component.Def.WidthInCells;
			int moduleRelativeVerticalPosition = craft.GetModuleRelativeVerticalPosition(@ref.Get().gameObject);
			if (moduleRelativeVerticalPosition + component.Def.HeightInCells > num)
			{
				int num2 = Grid.XYToCell(vector2I.x, moduleRelativeVerticalPosition + vector2I.y);
				obstruction = num2;
				return false;
			}
			for (int i = moduleRelativeVerticalPosition; i < num; i++)
			{
				for (int j = 0; j < widthInCells; j++)
				{
					int num3 = Grid.XYToCell(j + (vector2I.x - widthInCells / 2), i + vector2I.y);
					bool flag = Grid.Solid[num3];
					if (!Grid.IsValidCell(num3) || Grid.WorldIdx[num3] != Grid.WorldIdx[launchpad.GetComponent<LaunchPad>().RocketBottomPosition] || flag)
					{
						obstruction = num3;
						return false;
					}
				}
			}
		}
		obstruction = -1;
		return true;
	}

	// Token: 0x06005A49 RID: 23113 RVA: 0x0020C2DC File Offset: 0x0020A4DC
	private static bool CanReachSpace(int startCell, out int obstruction, out int highestCellInSky)
	{
		WorldContainer worldContainer = (startCell >= 0) ? ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[startCell]) : null;
		int num = (worldContainer == null) ? Grid.HeightInCells : ((int)worldContainer.maximumBounds.y);
		highestCellInSky = num;
		obstruction = -1;
		int num2 = startCell;
		while (Grid.CellRow(num2) < num)
		{
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
			{
				obstruction = num2;
				return false;
			}
			num2 = Grid.CellAbove(num2);
		}
		return true;
	}

	// Token: 0x06005A4A RID: 23114 RVA: 0x0020C354 File Offset: 0x0020A554
	public string GetObstruction()
	{
		if (this.obstructedTile == -1)
		{
			return null;
		}
		if (Grid.Objects[this.obstructedTile, 1] != null)
		{
			return Grid.Objects[this.obstructedTile, 1].GetComponent<Building>().Def.Name;
		}
		return string.Format(BUILDING.STATUSITEMS.PATH_NOT_CLEAR.TILE_FORMAT, Grid.Element[this.obstructedTile].tag.ProperName());
	}

	// Token: 0x04003C59 RID: 15449
	private CraftModuleInterface moduleInterface;

	// Token: 0x04003C5A RID: 15450
	private RocketModule module;

	// Token: 0x04003C5B RID: 15451
	private int bufferWidth;

	// Token: 0x04003C5C RID: 15452
	private bool hasClearSky;

	// Token: 0x04003C5D RID: 15453
	private int obstructedTile = -1;

	// Token: 0x04003C5E RID: 15454
	public const int MAXIMUM_ROCKET_HEIGHT = 35;

	// Token: 0x04003C5F RID: 15455
	public const float FIRE_FX_HEIGHT = 10f;
}
