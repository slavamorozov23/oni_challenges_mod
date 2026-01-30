using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000962 RID: 2402
public class GameNavGrids
{
	// Token: 0x0600436A RID: 17258 RVA: 0x0017DEE4 File Offset: 0x0017C0E4
	public GameNavGrids(Pathfinding pathfinding)
	{
		this.CreateDuplicantNavigation(pathfinding);
		this.WalkerGrid1x1 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerBabyGrid1x1 = this.CreateWalkerBabyNavigation(pathfinding, "WalkerBabyNavGrid", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerGrid1x2 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.WalkerGrid2x2 = this.CreateWalkerLargeNavigation(pathfinding, "WalkerNavGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.CreateDreckoNavigation(pathfinding);
		this.CreateDreckoBabyNavigation(pathfinding);
		this.CreateFloaterNavigation(pathfinding);
		this.FlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		}, false);
		this.RobotFlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, "RobotFlyerGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		}, true);
		this.FlyerGrid1x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		}, false);
		this.FlyerGrid2x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(1, 1)
		}, false);
		this.SwimmerGrid = this.CreateSwimmerNavigation(pathfinding, "SwimmerNavGrid", new CellOffset[]
		{
			new CellOffset(0, 0)
		}, false);
		this.SwimmerGrid2x2 = this.CreateLargeSwimmerNavigation(pathfinding, "SwimmerGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(1, 1)
		});
		this.CreateDiggerNavigation(pathfinding);
		this.CreateSquirrelNavigation(pathfinding);
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x0017E120 File Offset: 0x0017C320
	private void CreateDuplicantNavigation(Pathfinding pathfinding)
	{
		NavOffset[] invalid_nav_offsets = new NavOffset[]
		{
			new NavOffset(NavType.Floor, 1, 0),
			new NavOffset(NavType.Ladder, 1, 0),
			new NavOffset(NavType.Pole, 1, 0)
		};
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		};
		NavGrid.Transition[] setA = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Teleport, 0, 0, NavAxis.NA, false, false, false, 14, "fall_pre", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Teleport, NavType.Floor, 0, 0, NavAxis.NA, false, false, false, 1, "fall_pst", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Ladder, 0, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, 1, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, -1, NavAxis.NA, true, true, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 25, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Pole, 0, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, 1, NavAxis.NA, true, true, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, -1, NavAxis.NA, true, true, false, 6, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Tube, 0, 2, NavAxis.NA, false, false, false, 40, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 0, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, 1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, -1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, 1)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, -1)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, false, false, 8, "hover_hover_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, false, false, 8, "hover_hover_0_-1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, false, false, 9, "hover_hover_1_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, 1)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, false, false, 9, "hover_hover_1_-1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, -1)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 1, 0, NavAxis.NA, false, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 0, 1, NavAxis.NA, false, false, false, 8, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] setB = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f, false)
		};
		NavGrid.Transition[] transitions = this.MirrorTransitions(this.CombineTransitions(setA, setB));
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_default"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ladder,
				idleAnim = "ladder_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Pole,
				idleAnim = "pole_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Tube,
				idleAnim = "tube_idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "hover_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Teleport,
				idleAnim = "idle_default"
			}
		};
		this.DuplicantGrid = new NavGrid("MinionNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator(),
			new GameNavGrids.PoleValidator(),
			new GameNavGrids.TubeValidator(),
			new GameNavGrids.TeleporterValidator(),
			new GameNavGrids.FlyingValidator(true, true, true)
		}, 2, 3, 32);
		this.DuplicantGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.DuplicantGrid);
		NavGrid.Transition[] setB2 = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "climb_up_2_1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(this.CombineTransitions(setA, setB2));
		this.RobotGrid = new NavGrid("RobotNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator()
		}, 2, 3, 22);
		this.RobotGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.RobotGrid);
	}

	// Token: 0x0600436C RID: 17260 RVA: 0x00180714 File Offset: 0x0017E914
	private NavGrid CreateWalkerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x0600436D RID: 17261 RVA: 0x00180950 File Offset: 0x0017EB50
	private NavGrid CreateWalkerBabyNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x0600436E RID: 17262 RVA: 0x001809FC File Offset: 0x0017EBFC
	private NavGrid CreateWalkerLargeNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, true),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 1, "floor_floor_2_1", new CellOffset[]
			{
				new CellOffset(1, 1),
				new CellOffset(1, 2),
				new CellOffset(2, 1),
				new CellOffset(0, 2)
			}, new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0)
			}, new NavOffset[0], new NavOffset[0], true, 1f, true),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, true, 1, "floor_floor_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(2, -1),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, -2),
				new CellOffset(2, -2)
			}, new NavOffset[0], new NavOffset[0], true, 1f, true)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x0600436F RID: 17263 RVA: 0x00180BC4 File Offset: 0x0017EDC4
	private void CreateDreckoNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 2, NavAxis.NA, false, false, true, 5, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 2)
			}, new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0),
				new NavOffset(NavType.Floor, 2, 2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, -1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, -1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -2, -1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, -1),
				new CellOffset(-1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, -1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -2, -1)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(-1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 2, 1)
			}, true, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoGrid = new NavGrid("DreckoNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoGrid);
	}

	// Token: 0x06004370 RID: 17264 RVA: 0x0018152C File Offset: 0x0017F72C
	private void CreateDreckoBabyNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoBabyGrid = new NavGrid("DreckoBabyNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoBabyGrid);
	}

	// Token: 0x06004371 RID: 17265 RVA: 0x001819BC File Offset: 0x0017FBBC
	private void CreateFloaterNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, 0)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -1)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, -1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 1)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -3)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "swim_idle_loop"
			}
		};
		this.FloaterGrid = new NavGrid("FloaterNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.HoverValidator(),
			new GameNavGrids.SwimValidator()
		}, 2, 3, 22);
		pathfinding.AddNavGrid(this.FloaterGrid);
	}

	// Token: 0x06004372 RID: 17266 RVA: 0x00181FFC File Offset: 0x001801FC
	private NavGrid CreateFlyerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets, bool allow_door_traversal = false)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 2, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FlyingValidator(false, false, allow_door_traversal),
			new GameNavGrids.SwimValidator()
		}, 2, 2, 16);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x001823AC File Offset: 0x001805AC
	private NavGrid CreateSwimmerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets, bool use_x_offset = false)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, use_x_offset),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, use_x_offset),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, use_x_offset),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SwimValidator()
		}, 1, 2, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x00182578 File Offset: 0x00180778
	private NavGrid CreateLargeSwimmerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SwimValidator()
		}, 2, 2, 16);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x00182730 File Offset: 0x00180930
	private void CreateDiggerNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 0, NavAxis.NA, false, false, true, 1, "idle1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 1, NavAxis.NA, false, false, true, 1, "idle2", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 0, 1, NavAxis.NA, false, false, true, 1, "idle3", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, -1, NavAxis.NA, false, false, true, 1, "idle4", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Solid, 0, -1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 1, "drill_out", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Solid, 0, 1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Ceiling, 0, -1, NavAxis.NA, false, false, true, 1, "drill_out_ceiling", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.LeftWall, 1, 0, NavAxis.NA, false, false, true, 1, "drill_out_left_wall", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Solid, -1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.RightWall, -1, 0, NavAxis.NA, false, false, true, 1, "drill_out_right_wall", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Solid, 1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Solid,
				idleAnim = "idle1"
			}
		};
		this.DiggerGrid = new NavGrid("DiggerNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SolidValidator(),
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 22);
		pathfinding.AddNavGrid(this.DiggerGrid);
	}

	// Token: 0x06004376 RID: 17270 RVA: 0x00182EC8 File Offset: 0x001810C8
	private void CreateSquirrelNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.SquirrelGrid = new NavGrid("SquirrelNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 20);
		pathfinding.AddNavGrid(this.SquirrelGrid);
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x001834EC File Offset: 0x001816EC
	private CellOffset[] MirrorOffsets(CellOffset[] offsets)
	{
		List<CellOffset> list = new List<CellOffset>();
		foreach (CellOffset cellOffset in offsets)
		{
			cellOffset.x = -cellOffset.x;
			list.Add(cellOffset);
		}
		return list.ToArray();
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x00183534 File Offset: 0x00181734
	private NavOffset[] MirrorNavOffsets(NavOffset[] offsets)
	{
		List<NavOffset> list = new List<NavOffset>();
		foreach (NavOffset navOffset in offsets)
		{
			navOffset.navType = NavGrid.MirrorNavType(navOffset.navType);
			navOffset.offset.x = -navOffset.offset.x;
			list.Add(navOffset);
		}
		return list.ToArray();
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x00183598 File Offset: 0x00181798
	private NavGrid.Transition[] MirrorTransitions(NavGrid.Transition[] transitions)
	{
		List<NavGrid.Transition> list = new List<NavGrid.Transition>();
		foreach (NavGrid.Transition transition in transitions)
		{
			list.Add(transition);
			if (transition.x != 0 || transition.start == NavType.RightWall || transition.end == NavType.RightWall || transition.start == NavType.LeftWall || transition.end == NavType.LeftWall)
			{
				NavGrid.Transition transition2 = transition;
				transition2.x = -transition2.x;
				transition2.voidOffsets = this.MirrorOffsets(transition.voidOffsets);
				transition2.solidOffsets = this.MirrorOffsets(transition.solidOffsets);
				transition2.validNavOffsets = this.MirrorNavOffsets(transition.validNavOffsets);
				transition2.invalidNavOffsets = this.MirrorNavOffsets(transition.invalidNavOffsets);
				transition2.start = NavGrid.MirrorNavType(transition2.start);
				transition2.end = NavGrid.MirrorNavType(transition2.end);
				list.Add(transition2);
			}
		}
		list.Sort((NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return list.ToArray();
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x001836B8 File Offset: 0x001818B8
	private NavGrid.Transition[] CombineTransitions(NavGrid.Transition[] setA, NavGrid.Transition[] setB)
	{
		NavGrid.Transition[] array = new NavGrid.Transition[setA.Length + setB.Length];
		Array.Copy(setA, array, setA.Length);
		Array.Copy(setB, 0, array, setA.Length, setB.Length);
		Array.Sort<NavGrid.Transition>(array, (NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return array;
	}

	// Token: 0x04002C99 RID: 11417
	public NavGrid DuplicantGrid;

	// Token: 0x04002C9A RID: 11418
	public NavGrid WalkerGrid1x1;

	// Token: 0x04002C9B RID: 11419
	public NavGrid WalkerBabyGrid1x1;

	// Token: 0x04002C9C RID: 11420
	public NavGrid WalkerGrid1x2;

	// Token: 0x04002C9D RID: 11421
	public NavGrid WalkerGrid2x2;

	// Token: 0x04002C9E RID: 11422
	public NavGrid DreckoGrid;

	// Token: 0x04002C9F RID: 11423
	public NavGrid DreckoBabyGrid;

	// Token: 0x04002CA0 RID: 11424
	public NavGrid FloaterGrid;

	// Token: 0x04002CA1 RID: 11425
	public NavGrid FlyerGrid1x2;

	// Token: 0x04002CA2 RID: 11426
	public NavGrid FlyerGrid1x1;

	// Token: 0x04002CA3 RID: 11427
	public NavGrid FlyerGrid2x2;

	// Token: 0x04002CA4 RID: 11428
	public NavGrid SwimmerGrid;

	// Token: 0x04002CA5 RID: 11429
	public NavGrid SwimmerGrid2x2;

	// Token: 0x04002CA6 RID: 11430
	public NavGrid DiggerGrid;

	// Token: 0x04002CA7 RID: 11431
	public NavGrid SquirrelGrid;

	// Token: 0x04002CA8 RID: 11432
	public NavGrid RobotGrid;

	// Token: 0x04002CA9 RID: 11433
	public NavGrid RobotFlyerGrid1x1;

	// Token: 0x0200196A RID: 6506
	public class SwimValidator : NavTableValidator
	{
		// Token: 0x0600A233 RID: 41523 RVA: 0x003ADE10 File Offset: 0x003AC010
		public SwimValidator()
		{
			World instance = World.Instance;
			instance.OnLiquidChanged = (Action<int>)Delegate.Combine(instance.OnLiquidChanged, new Action<int>(this.OnLiquidChanged));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[9], new Action<int, object>(this.OnFoundationTileChanged));
		}

		// Token: 0x0600A234 RID: 41524 RVA: 0x003ADE6C File Offset: 0x003AC06C
		private void OnFoundationTileChanged(int cell, object unused)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600A235 RID: 41525 RVA: 0x003ADE84 File Offset: 0x003AC084
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (!flag)
			{
				flag = Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f);
			}
			bool is_valid = Grid.IsWorldValidCell(cell) && flag && base.IsClear(cell, bounding_offsets, false);
			nav_table.SetValid(cell, NavType.Swim, is_valid);
		}

		// Token: 0x0600A236 RID: 41526 RVA: 0x003ADED1 File Offset: 0x003AC0D1
		private void OnLiquidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}
	}

	// Token: 0x0200196B RID: 6507
	public class FloorValidator : NavTableValidator
	{
		// Token: 0x0600A237 RID: 41527 RVA: 0x003ADEE8 File Offset: 0x003AC0E8
		public FloorValidator(bool is_dupe)
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
			this.isDupe = is_dupe;
		}

		// Token: 0x0600A238 RID: 41528 RVA: 0x003ADF4C File Offset: 0x003AC14C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.FloorValidator.IsWalkableCell(cell, Grid.CellBelow(cell), this.isDupe);
			nav_table.SetValid(cell, NavType.Floor, flag && base.IsClear(cell, bounding_offsets, this.isDupe));
		}

		// Token: 0x0600A239 RID: 41529 RVA: 0x003ADF88 File Offset: 0x003AC188
		public static bool IsWalkableCell(int cell, int anchor_cell, bool is_dupe)
		{
			if (!Grid.IsWorldValidCell(cell))
			{
				return false;
			}
			if (!Grid.IsWorldValidCell(anchor_cell))
			{
				return false;
			}
			if (!NavTableValidator.IsCellPassable(cell, is_dupe))
			{
				return false;
			}
			if (Grid.FakeFloor[anchor_cell])
			{
				return true;
			}
			if (Grid.Solid[anchor_cell])
			{
				return !Grid.DupePassable[anchor_cell];
			}
			return is_dupe && (Grid.NavValidatorMasks[cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) == (Grid.NavValidatorFlags)0 && (Grid.NavValidatorMasks[anchor_cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) > (Grid.NavValidatorFlags)0;
		}

		// Token: 0x0600A23A RID: 41530 RVA: 0x003AE000 File Offset: 0x003AC200
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A23B RID: 41531 RVA: 0x003AE028 File Offset: 0x003AC228
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A23C RID: 41532 RVA: 0x003AE050 File Offset: 0x003AC250
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600A23D RID: 41533 RVA: 0x003AE068 File Offset: 0x003AC268
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x04007DD7 RID: 32215
		private bool isDupe;
	}

	// Token: 0x0200196C RID: 6508
	public class WallValidator : NavTableValidator
	{
		// Token: 0x0600A23E RID: 41534 RVA: 0x003AE0BD File Offset: 0x003AC2BD
		public WallValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x0600A23F RID: 41535 RVA: 0x003AE0EC File Offset: 0x003AC2EC
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellRight(cell));
			bool flag2 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellLeft(cell));
			nav_table.SetValid(cell, NavType.RightWall, flag && base.IsClear(cell, bounding_offsets, false));
			nav_table.SetValid(cell, NavType.LeftWall, flag2 && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x0600A240 RID: 41536 RVA: 0x003AE141 File Offset: 0x003AC341
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.CritterImpassable[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A241 RID: 41537 RVA: 0x003AE17D File Offset: 0x003AC37D
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600A242 RID: 41538 RVA: 0x003AE193 File Offset: 0x003AC393
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200196D RID: 6509
	public class CeilingValidator : NavTableValidator
	{
		// Token: 0x0600A243 RID: 41539 RVA: 0x003AE1BB File Offset: 0x003AC3BB
		public CeilingValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x0600A244 RID: 41540 RVA: 0x003AE1EC File Offset: 0x003AC3EC
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.CeilingValidator.IsWalkableCell(cell, Grid.CellAbove(cell));
			nav_table.SetValid(cell, NavType.Ceiling, flag && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x0600A245 RID: 41541 RVA: 0x003AE220 File Offset: 0x003AC420
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[cell] && !Grid.FakeFloor[cell])
				{
					return false;
				}
				if (Grid.FakeFloor[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A246 RID: 41542 RVA: 0x003AE292 File Offset: 0x003AC492
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600A247 RID: 41543 RVA: 0x003AE2A8 File Offset: 0x003AC4A8
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200196E RID: 6510
	public class LadderValidator : NavTableValidator
	{
		// Token: 0x0600A248 RID: 41544 RVA: 0x003AE2D0 File Offset: 0x003AC4D0
		public LadderValidator()
		{
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x0600A249 RID: 41545 RVA: 0x003AE2FC File Offset: 0x003AC4FC
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A24A RID: 41546 RVA: 0x003AE324 File Offset: 0x003AC524
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A24B RID: 41547 RVA: 0x003AE34C File Offset: 0x003AC54C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Ladder, base.IsClear(cell, bounding_offsets, true) && Grid.HasLadder[cell]);
		}

		// Token: 0x0600A24C RID: 41548 RVA: 0x003AE36F File Offset: 0x003AC56F
		public override void Clear()
		{
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}
	}

	// Token: 0x0200196F RID: 6511
	public class PoleValidator : GameNavGrids.LadderValidator
	{
		// Token: 0x0600A24D RID: 41549 RVA: 0x003AE393 File Offset: 0x003AC593
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Pole, base.IsClear(cell, bounding_offsets, true) && Grid.HasPole[cell]);
		}
	}

	// Token: 0x02001970 RID: 6512
	public class TubeValidator : NavTableValidator
	{
		// Token: 0x0600A24F RID: 41551 RVA: 0x003AE3BE File Offset: 0x003AC5BE
		public TubeValidator()
		{
			Components.ITravelTubePieces.Register(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}

		// Token: 0x0600A250 RID: 41552 RVA: 0x003AE3E8 File Offset: 0x003AC5E8
		private void OnAddLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A251 RID: 41553 RVA: 0x003AE418 File Offset: 0x003AC618
		private void OnRemoveLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A252 RID: 41554 RVA: 0x003AE445 File Offset: 0x003AC645
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Tube, Grid.HasTube[cell]);
		}

		// Token: 0x0600A253 RID: 41555 RVA: 0x003AE45A File Offset: 0x003AC65A
		public override void Clear()
		{
			Components.ITravelTubePieces.Unregister(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}
	}

	// Token: 0x02001971 RID: 6513
	public class TeleporterValidator : NavTableValidator
	{
		// Token: 0x0600A254 RID: 41556 RVA: 0x003AE47E File Offset: 0x003AC67E
		public TeleporterValidator()
		{
			Components.NavTeleporters.Register(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}

		// Token: 0x0600A255 RID: 41557 RVA: 0x003AE4A8 File Offset: 0x003AC6A8
		private void OnAddTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A256 RID: 41558 RVA: 0x003AE4D0 File Offset: 0x003AC6D0
		private void OnRemoveTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600A257 RID: 41559 RVA: 0x003AE4F8 File Offset: 0x003AC6F8
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = Grid.IsWorldValidCell(cell) && Grid.HasNavTeleporter[cell];
			nav_table.SetValid(cell, NavType.Teleport, is_valid);
		}

		// Token: 0x0600A258 RID: 41560 RVA: 0x003AE526 File Offset: 0x003AC726
		public override void Clear()
		{
			Components.NavTeleporters.Unregister(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}
	}

	// Token: 0x02001972 RID: 6514
	public class FlyingValidator : NavTableValidator
	{
		// Token: 0x0600A259 RID: 41561 RVA: 0x003AE54C File Offset: 0x003AC74C
		public FlyingValidator(bool exclude_floor = false, bool exclude_jet_suit_blockers = false, bool allow_door_traversal = false)
		{
			this.exclude_floor = exclude_floor;
			this.exclude_jet_suit_blockers = exclude_jet_suit_blockers;
			this.allow_door_traversal = allow_door_traversal;
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x0600A25A RID: 41562 RVA: 0x003AE5E4 File Offset: 0x003AC7E4
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = false;
			if (Grid.IsWorldValidCell(Grid.CellAbove(cell)))
			{
				flag = (!Grid.IsSubstantialLiquid(cell, 0.35f) && base.IsClear(cell, bounding_offsets, this.allow_door_traversal));
				if (flag && this.exclude_floor)
				{
					int cell2 = Grid.CellBelow(cell);
					if (Grid.IsWorldValidCell(cell2))
					{
						flag = base.IsClear(cell2, bounding_offsets, this.allow_door_traversal);
					}
				}
				if (flag && this.exclude_jet_suit_blockers)
				{
					GameObject gameObject = Grid.Objects[cell, 1];
					flag = (gameObject == null || !gameObject.HasTag(GameTags.JetSuitBlocker));
				}
			}
			nav_table.SetValid(cell, NavType.Hover, flag);
		}

		// Token: 0x0600A25B RID: 41563 RVA: 0x003AE684 File Offset: 0x003AC884
		private void OnBuildingChange(int cell, object data)
		{
			this.MarkCellDirty(cell);
		}

		// Token: 0x0600A25C RID: 41564 RVA: 0x003AE68D File Offset: 0x003AC88D
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600A25D RID: 41565 RVA: 0x003AE6A4 File Offset: 0x003AC8A4
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x04007DD8 RID: 32216
		private bool exclude_floor;

		// Token: 0x04007DD9 RID: 32217
		private bool exclude_jet_suit_blockers;

		// Token: 0x04007DDA RID: 32218
		private bool allow_door_traversal;

		// Token: 0x04007DDB RID: 32219
		private HandleVector<int>.Handle buildingParititonerEntry;
	}

	// Token: 0x02001973 RID: 6515
	public class HoverValidator : NavTableValidator
	{
		// Token: 0x0600A25E RID: 41566 RVA: 0x003AE720 File Offset: 0x003AC920
		public HoverValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}

		// Token: 0x0600A25F RID: 41567 RVA: 0x003AE780 File Offset: 0x003AC980
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			int num = Grid.CellBelow(cell);
			if (Grid.IsWorldValidCell(num))
			{
				bool flag = Grid.Solid[num] || Grid.FakeFloor[num] || Grid.IsSubstantialLiquid(num, 0.35f);
				nav_table.SetValid(cell, NavType.Hover, !Grid.IsSubstantialLiquid(cell, 0.35f) && flag && base.IsClear(cell, bounding_offsets, false));
			}
		}

		// Token: 0x0600A260 RID: 41568 RVA: 0x003AE7EB File Offset: 0x003AC9EB
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600A261 RID: 41569 RVA: 0x003AE804 File Offset: 0x003ACA04
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}
	}

	// Token: 0x02001974 RID: 6516
	public class SolidValidator : NavTableValidator
	{
		// Token: 0x0600A262 RID: 41570 RVA: 0x003AE85D File Offset: 0x003ACA5D
		public SolidValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x0600A263 RID: 41571 RVA: 0x003AE88C File Offset: 0x003ACA8C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = GameNavGrids.SolidValidator.IsDiggable(cell, Grid.CellBelow(cell));
			nav_table.SetValid(cell, NavType.Solid, is_valid);
		}

		// Token: 0x0600A264 RID: 41572 RVA: 0x003AE8B0 File Offset: 0x003ACAB0
		public static bool IsDiggable(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x0600A265 RID: 41573 RVA: 0x003AE975 File Offset: 0x003ACB75
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600A266 RID: 41574 RVA: 0x003AE98B File Offset: 0x003ACB8B
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}
}
