using System;
using UnityEngine;

// Token: 0x02000881 RID: 2177
public static class CreatureHelpers
{
	// Token: 0x06003BE7 RID: 15335 RVA: 0x0014F78C File Offset: 0x0014D98C
	public static bool isClear(int cell)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && !Grid.IsSubstantialLiquid(cell, 0.9f) && (!Grid.IsValidCell(Grid.CellBelow(cell)) || !Grid.IsLiquid(cell) || !Grid.IsLiquid(Grid.CellBelow(cell)));
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x0014F7E4 File Offset: 0x0014D9E4
	public static int FindNearbyBreathableCell(int currentLocation, SimHashes breathableElement)
	{
		return currentLocation;
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0014F7E8 File Offset: 0x0014D9E8
	public static bool cellsAreClear(int[] cells)
	{
		for (int i = 0; i < cells.Length; i++)
		{
			if (!Grid.IsValidCell(cells[i]))
			{
				return false;
			}
			if (!CreatureHelpers.isClear(cells[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x0014F81C File Offset: 0x0014DA1C
	public static Vector3 PositionOfCurrentCell(Vector3 transformPosition)
	{
		return Grid.CellToPos(Grid.PosToCell(transformPosition));
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x0014F829 File Offset: 0x0014DA29
	public static Vector3 CenterPositionOfCell(int cell)
	{
		return Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, -2f);
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x0014F84C File Offset: 0x0014DA4C
	public static void DeselectCreature(GameObject creature)
	{
		KSelectable component = creature.GetComponent<KSelectable>();
		if (component != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(null, false);
		}
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x0014F887 File Offset: 0x0014DA87
	public static bool isSwimmable(int cell)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && Grid.IsSubstantialLiquid(cell, 0.35f);
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x0014F8B2 File Offset: 0x0014DAB2
	public static bool isSolidGround(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x0014F8CE File Offset: 0x0014DACE
	public static void FlipAnim(KAnimControllerBase anim, Vector3 heading)
	{
		if (heading.x < 0f)
		{
			anim.FlipX = true;
			return;
		}
		if (heading.x > 0f)
		{
			anim.FlipX = false;
		}
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x0014F8F9 File Offset: 0x0014DAF9
	public static void FlipAnim(KBatchedAnimController anim, Vector3 heading)
	{
		if (heading.x < 0f)
		{
			anim.FlipX = true;
			return;
		}
		if (heading.x > 0f)
		{
			anim.FlipX = false;
		}
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x0014F924 File Offset: 0x0014DB24
	public static Vector3 GetWalkMoveTarget(Transform transform, Vector2 Heading)
	{
		int cell = Grid.PosToCell(transform.GetPosition());
		if (Heading.x == 1f)
		{
			if (CreatureHelpers.isClear(Grid.CellRight(cell)) && CreatureHelpers.isClear(Grid.CellDownRight(cell)) && CreatureHelpers.isClear(Grid.CellRight(Grid.CellRight(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.right * 2f + Vector3.down)))
			{
				return transform.GetPosition() + Vector3.right * 2f;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.CellRight(cell),
				Grid.CellDownRight(cell)
			}) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownRight(cell))))
			{
				return transform.GetPosition() + Vector3.right + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, 1, 0),
				Grid.OffsetCell(cell, 1, -1),
				Grid.OffsetCell(cell, 1, -2)
			}) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, 1, -3)))
			{
				return transform.GetPosition() + Vector3.right + Vector3.down + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, 1, 0),
				Grid.OffsetCell(cell, 1, -1),
				Grid.OffsetCell(cell, 1, -2),
				Grid.OffsetCell(cell, 1, -3)
			}))
			{
				return transform.GetPosition();
			}
			if (CreatureHelpers.isClear(Grid.CellRight(cell)))
			{
				return transform.GetPosition() + Vector3.right;
			}
			if (CreatureHelpers.isClear(Grid.CellUpRight(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellRight(cell)])
			{
				return transform.GetPosition() + Vector3.up + Vector3.right;
			}
			if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && Grid.Solid[Grid.CellAbove(Grid.CellRight(cell))] && CreatureHelpers.isClear(Grid.CellRight(Grid.CellAbove(Grid.CellAbove(cell)))))
			{
				return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.right;
			}
		}
		if (Heading.x == -1f)
		{
			if (CreatureHelpers.isClear(Grid.CellLeft(cell)) && CreatureHelpers.isClear(Grid.CellDownLeft(cell)) && CreatureHelpers.isClear(Grid.CellLeft(Grid.CellLeft(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.left * 2f + Vector3.down)))
			{
				return transform.GetPosition() + Vector3.left * 2f;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.CellLeft(cell),
				Grid.CellDownLeft(cell)
			}) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownLeft(cell))))
			{
				return transform.GetPosition() + Vector3.left + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, -1, 0),
				Grid.OffsetCell(cell, -1, -1),
				Grid.OffsetCell(cell, -1, -2)
			}) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, -1, -3)))
			{
				return transform.GetPosition() + Vector3.left + Vector3.down + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, -1, 0),
				Grid.OffsetCell(cell, -1, -1),
				Grid.OffsetCell(cell, -1, -2),
				Grid.OffsetCell(cell, -1, -3)
			}))
			{
				return transform.GetPosition();
			}
			if (CreatureHelpers.isClear(Grid.CellLeft(Grid.PosToCell(transform.GetPosition()))))
			{
				return transform.GetPosition() + Vector3.left;
			}
			if (CreatureHelpers.isClear(Grid.CellUpLeft(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellLeft(cell)])
			{
				return transform.GetPosition() + Vector3.up + Vector3.left;
			}
			if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && Grid.Solid[Grid.CellAbove(Grid.CellLeft(cell))] && CreatureHelpers.isClear(Grid.CellLeft(Grid.CellAbove(Grid.CellAbove(cell)))))
			{
				return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.left;
			}
		}
		return transform.GetPosition();
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0014FE0C File Offset: 0x0014E00C
	public static bool CrewNearby(Transform transform, int range = 6)
	{
		int cell = Grid.PosToCell(transform.gameObject);
		for (int i = 1; i < range; i++)
		{
			int cell2 = Grid.OffsetCell(cell, i, 0);
			int cell3 = Grid.OffsetCell(cell, -i, 0);
			if (Grid.Objects[cell2, 0] != null)
			{
				return true;
			}
			if (Grid.Objects[cell3, 0] != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x0014FE74 File Offset: 0x0014E074
	public static bool CheckHorizontalClear(Vector3 startPosition, Vector3 endPosition)
	{
		int cell = Grid.PosToCell(startPosition);
		int num = 1;
		if (endPosition.x < startPosition.x)
		{
			num = -1;
		}
		float num2 = Mathf.Abs(endPosition.x - startPosition.x);
		int num3 = 0;
		while ((float)num3 < num2)
		{
			int i = Grid.OffsetCell(cell, num3 * num, 0);
			if (Grid.Solid[i])
			{
				return false;
			}
			num3++;
		}
		return true;
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0014FED8 File Offset: 0x0014E0D8
	public static GameObject GetFleeTargetLocatorObject(GameObject self, GameObject threat)
	{
		if (threat == null)
		{
			global::Debug.LogWarning(self.name + " is trying to flee, bus has no threats");
			return null;
		}
		CreatureHelpers.fleeThreatInfo fleeThreatInfo;
		fleeThreatInfo.threatCell = Grid.PosToCell(threat);
		fleeThreatInfo.selfCell = Grid.PosToCell(self);
		fleeThreatInfo.nav = self.GetComponent<Navigator>();
		if (fleeThreatInfo.nav == null)
		{
			global::Debug.LogWarning(self.name + " is trying to flee, bus has no navigator component attached.");
			return null;
		}
		int num = GameUtil.FloodFillFindBest<CreatureHelpers.fleeThreatInfo>(CreatureHelpers.fleeCellRater, fleeThreatInfo, CreatureHelpers.fleeCellVaidator, Grid.PosToCell(self), 300);
		if (num != -1)
		{
			return ChoreHelpers.CreateLocator("GoToLocator", Grid.CellToPos(num));
		}
		return null;
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x0014FF84 File Offset: 0x0014E184
	private static bool isInFavoredFleeDirection(int targetFleeCell, int threatCell, int selfCell)
	{
		bool flag = Grid.CellToPos(threatCell).x < Grid.CellToPos(selfCell).x;
		bool flag2 = Grid.CellToPos(threatCell).x < Grid.CellToPos(targetFleeCell).x;
		return flag == flag2;
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x0014FFCD File Offset: 0x0014E1CD
	private static bool CanFleeTo(int cell, Navigator nav)
	{
		return nav.GetNavigationCost(cell, OffsetGroups.Use) != -1;
	}

	// Token: 0x04002503 RID: 9475
	private static Func<int, CreatureHelpers.fleeThreatInfo, float> fleeCellRater = (int cell, CreatureHelpers.fleeThreatInfo threat) => (float)Grid.GetCellDistance(cell, threat.threatCell) + (CreatureHelpers.isInFavoredFleeDirection(cell, threat.threatCell, threat.selfCell) ? 2f : 0f);

	// Token: 0x04002504 RID: 9476
	private static Func<int, CreatureHelpers.fleeThreatInfo, bool> fleeCellVaidator = (int cell, CreatureHelpers.fleeThreatInfo info) => CreatureHelpers.CanFleeTo(cell, info.nav);

	// Token: 0x02001844 RID: 6212
	private struct fleeThreatInfo
	{
		// Token: 0x04007A7D RID: 31357
		public int threatCell;

		// Token: 0x04007A7E RID: 31358
		public int selfCell;

		// Token: 0x04007A7F RID: 31359
		public Navigator nav;
	}
}
