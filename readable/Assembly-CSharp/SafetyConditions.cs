using System;
using System.Collections.Generic;

// Token: 0x0200050A RID: 1290
public class SafetyConditions
{
	// Token: 0x06001BDB RID: 7131 RVA: 0x00099BF8 File Offset: 0x00097DF8
	public SafetyConditions()
	{
		int num = 1;
		this.IsNearby = new SafetyChecker.Condition("IsNearby", num *= 2, (int cell, int cost, SafetyChecker.Context context) => cost > 5);
		this.IsNotLedge = new SafetyChecker.Condition("IsNotLedge", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int i = Grid.CellBelow(Grid.CellLeft(cell));
			if (Grid.Solid[i])
			{
				return false;
			}
			int i2 = Grid.CellBelow(Grid.CellRight(cell));
			return Grid.Solid[i2];
		});
		this.IsNotLiquid = new SafetyChecker.Condition("IsNotLiquid", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !Grid.Element[cell].IsLiquid);
		this.IsNotCoveredInLiquid = new SafetyChecker.Condition("IsNotCoveredInLiquid", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int num2 = Grid.CellAbove(cell);
			return Grid.IsValidCell(num2) && (!Grid.Element[cell].IsLiquid || !Grid.Element[num2].IsLiquid);
		});
		this.IsNotLadder = new SafetyChecker.Condition("IsNotLadder", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole));
		this.IsNotDoor = new SafetyChecker.Condition("IsNotDoor", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int num2 = Grid.CellAbove(cell);
			return !Grid.HasDoor[cell] && Grid.IsValidCell(num2) && !Grid.HasDoor[num2];
		});
		this.IsCorrectTemperature = new SafetyChecker.Condition("IsCorrectTemperature", num *= 2, (int cell, int cost, SafetyChecker.Context context) => Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f);
		this.IsWarming = new SafetyChecker.Condition("IsWarming", num *= 2, (int cell, int cost, SafetyChecker.Context context) => WarmthProvider.IsWarmCell(cell));
		this.IsCooling = new SafetyChecker.Condition("IsCooling", num *= 2, (int cell, int cost, SafetyChecker.Context context) => false);
		this.HasSomeOxygen = new SafetyChecker.Condition("HasSomeOxygen", num *= 2, (int cell, int cost, SafetyChecker.Context context) => context.oxygenBreather == null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, Grid.DefaultOffset, context.oxygenBreather).IsBreathable);
		this.HasSomeOxygenAround = new SafetyChecker.Condition("HasSomeOxygenAround", num *= 2, (int cell, int cost, SafetyChecker.Context context) => context.oxygenBreather == null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, context.oxygenBreather).IsBreathable);
		this.IsClear = new SafetyChecker.Condition("IsClear", num * 2, (int cell, int cost, SafetyChecker.Context context) => context.minionBrain.IsCellClear(cell));
		this.WarmUpChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsWarming
		}.ToArray());
		this.CoolDownChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsCooling
		}.ToArray());
		this.AbsorbCellCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsNotCoveredInLiquid,
			this.IsNotDoor,
			this.HasSomeOxygenAround
		}.ToArray());
		List<SafetyChecker.Condition> list = new List<SafetyChecker.Condition>();
		list.Add(this.HasSomeOxygen);
		list.Add(this.IsNotDoor);
		this.RecoverBreathChecker = new SafetyChecker(list.ToArray());
		List<SafetyChecker.Condition> list2 = new List<SafetyChecker.Condition>(list);
		list2.Add(this.IsNotLiquid);
		list2.Add(this.IsCorrectTemperature);
		this.SafeCellChecker = new SafetyChecker(list2.ToArray());
		this.IdleCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>(list2)
		{
			this.IsClear,
			this.IsNotLadder
		}.ToArray());
		this.VomitCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsNotLiquid,
			this.IsNotLedge,
			this.IsNearby
		}.ToArray());
	}

	// Token: 0x04001044 RID: 4164
	public SafetyChecker.Condition IsNotLiquid;

	// Token: 0x04001045 RID: 4165
	public SafetyChecker.Condition IsNotCoveredInLiquid;

	// Token: 0x04001046 RID: 4166
	public SafetyChecker.Condition IsNotLadder;

	// Token: 0x04001047 RID: 4167
	public SafetyChecker.Condition IsCorrectTemperature;

	// Token: 0x04001048 RID: 4168
	public SafetyChecker.Condition IsWarming;

	// Token: 0x04001049 RID: 4169
	public SafetyChecker.Condition IsCooling;

	// Token: 0x0400104A RID: 4170
	public SafetyChecker.Condition HasSomeOxygen;

	// Token: 0x0400104B RID: 4171
	public SafetyChecker.Condition HasSomeOxygenAround;

	// Token: 0x0400104C RID: 4172
	public SafetyChecker.Condition IsClear;

	// Token: 0x0400104D RID: 4173
	public SafetyChecker.Condition IsNotFoundation;

	// Token: 0x0400104E RID: 4174
	public SafetyChecker.Condition IsNotDoor;

	// Token: 0x0400104F RID: 4175
	public SafetyChecker.Condition IsNotLedge;

	// Token: 0x04001050 RID: 4176
	public SafetyChecker.Condition IsNearby;

	// Token: 0x04001051 RID: 4177
	public SafetyChecker WarmUpChecker;

	// Token: 0x04001052 RID: 4178
	public SafetyChecker CoolDownChecker;

	// Token: 0x04001053 RID: 4179
	public SafetyChecker RecoverBreathChecker;

	// Token: 0x04001054 RID: 4180
	public SafetyChecker AbsorbCellCellChecker;

	// Token: 0x04001055 RID: 4181
	public SafetyChecker VomitCellChecker;

	// Token: 0x04001056 RID: 4182
	public SafetyChecker SafeCellChecker;

	// Token: 0x04001057 RID: 4183
	public SafetyChecker IdleCellChecker;
}
