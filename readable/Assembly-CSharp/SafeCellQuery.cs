using System;

// Token: 0x0200050D RID: 1293
public class SafeCellQuery : PathFinderQuery
{
	// Token: 0x06001BE4 RID: 7140 RVA: 0x0009A119 File Offset: 0x00098319
	public SafeCellQuery Reset(MinionBrain brain, bool avoid_light, SafeCellQuery.SafeFlags ignoredFlags = (SafeCellQuery.SafeFlags)0)
	{
		this.brain = brain;
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetCellFlags = (SafeCellQuery.SafeFlags)0;
		this.avoid_light = avoid_light;
		this.ignoredFlags = ignoredFlags;
		return this;
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x0009A150 File Offset: 0x00098350
	public static SafeCellQuery.SafeFlags GetFlags(int cell, MinionBrain brain, bool avoid_light = false, SafeCellQuery.SafeFlags ignoredFlags = (SafeCellQuery.SafeFlags)0)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(num))
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.Solid[cell] || Grid.Solid[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		bool flag = brain.IsCellClear(cell);
		bool flag2 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags)0 || !Grid.Element[cell].IsLiquid;
		bool flag3 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace) != (SafeCellQuery.SafeFlags)0 || !Grid.Element[num].IsLiquid;
		bool flag4 = (ignoredFlags & SafeCellQuery.SafeFlags.CorrectTemperature) != (SafeCellQuery.SafeFlags)0 || (Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f);
		bool flag5 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotRadiated) != (SafeCellQuery.SafeFlags)0 || Grid.Radiation[cell] < 250f;
		bool flag6 = (ignoredFlags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 || brain.OxygenBreather == null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, Grid.DefaultOffset, brain.OxygenBreather).IsBreathable;
		bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
		bool flag8 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
		bool flag9 = !avoid_light || SleepChore.IsDarkAtCell(cell);
		if (cell == Grid.PosToCell(brain))
		{
			flag6 = ((ignoredFlags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 || brain.OxygenBreather == null || brain.OxygenBreather.HasOxygen);
		}
		SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags)0;
		if (flag)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsClear;
		}
		if (flag4)
		{
			safeFlags |= SafeCellQuery.SafeFlags.CorrectTemperature;
		}
		if (flag5)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotRadiated;
		}
		if (flag6)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsBreathable;
		}
		if (flag7)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLadder;
		}
		if (flag8)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotTube;
		}
		if (flag2)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquid;
		}
		if (flag3)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
		}
		if (flag9)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsLightOk;
		}
		return safeFlags;
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x0009A358 File Offset: 0x00098558
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light, this.ignoredFlags);
		bool flag = flags > this.targetCellFlags;
		bool flag2 = flags == this.targetCellFlags && cost < this.targetCost;
		if (flag || flag2)
		{
			this.targetCellFlags = flags;
			this.targetCost = cost;
			this.targetCell = cell;
		}
		return (SafeCellQuery.SafeFlags.AllSafeFlags & ~(flags | this.ignoredFlags)) == (SafeCellQuery.SafeFlags)0;
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x0009A3C8 File Offset: 0x000985C8
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001060 RID: 4192
	private MinionBrain brain;

	// Token: 0x04001061 RID: 4193
	private int targetCell;

	// Token: 0x04001062 RID: 4194
	private int targetCost;

	// Token: 0x04001063 RID: 4195
	public SafeCellQuery.SafeFlags targetCellFlags;

	// Token: 0x04001064 RID: 4196
	private bool avoid_light;

	// Token: 0x04001065 RID: 4197
	private SafeCellQuery.SafeFlags ignoredFlags;

	// Token: 0x0200139D RID: 5021
	public enum SafeFlags
	{
		// Token: 0x04006BED RID: 27629
		IsClear = 1,
		// Token: 0x04006BEE RID: 27630
		IsLightOk,
		// Token: 0x04006BEF RID: 27631
		IsNotLadder = 4,
		// Token: 0x04006BF0 RID: 27632
		IsNotTube = 8,
		// Token: 0x04006BF1 RID: 27633
		CorrectTemperature = 16,
		// Token: 0x04006BF2 RID: 27634
		IsNotRadiated = 32,
		// Token: 0x04006BF3 RID: 27635
		IsBreathable = 64,
		// Token: 0x04006BF4 RID: 27636
		IsNotLiquidOnMyFace = 128,
		// Token: 0x04006BF5 RID: 27637
		IsNotLiquid = 256,
		// Token: 0x04006BF6 RID: 27638
		AllSafeFlags = 511
	}
}
