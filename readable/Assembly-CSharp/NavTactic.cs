using System;
using UnityEngine;

// Token: 0x02000610 RID: 1552
public class NavTactic
{
	// Token: 0x06002458 RID: 9304 RVA: 0x000D1F2A File Offset: 0x000D012A
	public NavTactic(int preferredRange, int rangePenalty = 1, int overlapPenalty = 1, int pathCostPenalty = 1)
	{
		this._overlapPenalty = overlapPenalty;
		this._preferredRange = preferredRange;
		this._rangePenalty = rangePenalty;
		this._pathCostPenalty = pathCostPenalty;
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x000D1F64 File Offset: 0x000D0164
	public NavTactic(int preferredRange, int rangePenalty, int overlapPenalty, int pathCostPenalty, int xPenalty, int preferredX, int yPenalty, int preferredY)
	{
		this._overlapPenalty = overlapPenalty;
		this._preferredRange = preferredRange;
		this._rangePenalty = rangePenalty;
		this._pathCostPenalty = pathCostPenalty;
		this._pathXCostPenalty = xPenalty;
		this._preferredX = preferredX;
		this._pathYCostPenalty = yPenalty;
		this._preferredY = preferredY;
	}

	// Token: 0x0600245A RID: 9306 RVA: 0x000D1FCC File Offset: 0x000D01CC
	public int GetCellPreferences(int root, CellOffset[] offsets, Navigator navigator)
	{
		int result = NavigationReservations.InvalidReservation;
		int num = int.MaxValue;
		for (int i = 0; i < offsets.Length; i++)
		{
			int num2 = Grid.OffsetCell(root, offsets[i]);
			int num3 = 0;
			num3 += this._overlapPenalty * NavigationReservations.Instance.GetOccupancyCount(num2);
			num3 += this._rangePenalty * Mathf.Abs(this._preferredRange - Grid.GetCellDistance(root, num2));
			num3 += this._pathCostPenalty * Mathf.Max(navigator.GetNavigationCost(num2), 0);
			num3 += this._pathXCostPenalty * Mathf.Abs(this._preferredX - Mathf.Abs(Grid.CellColumn(root) - Grid.CellColumn(num2)));
			num3 += this._pathYCostPenalty * Mathf.Abs(this._preferredY - Mathf.Abs(Grid.CellRow(root) - Grid.CellRow(num2)));
			if (num3 < num && navigator.CanReach(num2))
			{
				num = num3;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x0400152E RID: 5422
	private int _overlapPenalty = 3;

	// Token: 0x0400152F RID: 5423
	private int _preferredRange;

	// Token: 0x04001530 RID: 5424
	private int _rangePenalty = 2;

	// Token: 0x04001531 RID: 5425
	private int _pathCostPenalty = 1;

	// Token: 0x04001532 RID: 5426
	private int _pathXCostPenalty;

	// Token: 0x04001533 RID: 5427
	private int _preferredX;

	// Token: 0x04001534 RID: 5428
	private int _pathYCostPenalty;

	// Token: 0x04001535 RID: 5429
	private int _preferredY;
}
