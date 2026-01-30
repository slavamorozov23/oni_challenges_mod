using System;
using TUNING;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class AbsorbCellQuery : PathFinderQuery
{
	// Token: 0x06001BE9 RID: 7145 RVA: 0x0009A3D8 File Offset: 0x000985D8
	public AbsorbCellQuery()
	{
		this.checker = Game.Instance.safetyConditions.AbsorbCellCellChecker;
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x0009A408 File Offset: 0x00098608
	public AbsorbCellQuery Reset(MinionBrain brain, bool criticalMode, float currentOxygenTankMass, float breathPercentage, int allowCellEvenIfReserved, bool isRecoveringFromSuffocation)
	{
		this.brain = brain;
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetOxygenScore = float.MinValue;
		this.targetCellSafetyFlags = (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		this.targetBreathableMassAvailable = 0f;
		this.criticalMode = criticalMode;
		this.bionicOxygenRemaining = currentOxygenTankMass;
		this.breathPercentage = breathPercentage;
		this.allowCellEvenIfReserved = allowCellEvenIfReserved;
		this.context = new SafetyChecker.Context(brain);
		ScaldingMonitor.Instance instance = (brain == null) ? null : brain.GetSMI<ScaldingMonitor.Instance>();
		this.scaldingTreshold = ((instance == null) ? -1f : instance.GetScaldingThreshold());
		this.isRecoveringFromSuffocation = isRecoveringFromSuffocation;
		return this;
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x0009A4AC File Offset: 0x000986AC
	public static AbsorbCellQuery.AbsorbOxygenSafeCellFlags GetAbsorbOxygenFlags(int cell, MinionBrain brain, float scaldingTreshold, out float totalBreathableMassAroundCell, out float breathableCellRatioInSample, int allowCellEvenIfReserved)
	{
		totalBreathableMassAroundCell = 0f;
		breathableCellRatioInSample = 0f;
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(num))
		{
			return (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		}
		if (Grid.Solid[cell] || Grid.Solid[num])
		{
			return (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		}
		if (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[num])
		{
			return (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		}
		bool flag = cell == allowCellEvenIfReserved || brain.IsCellClear(cell);
		bool flag2 = !Grid.Element[cell].IsLiquid;
		bool flag3 = !Grid.Element[num].IsLiquid;
		bool flag4 = scaldingTreshold < 0f || Grid.Temperature[cell] < scaldingTreshold;
		bool flag5 = Grid.Radiation[cell] < 250f;
		bool flag6 = false;
		if (brain.OxygenBreather != null)
		{
			for (int i = 0; i < GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length; i++)
			{
				int num2 = Grid.OffsetCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS[i]);
				if (Grid.IsValidCell(num2) && Grid.AreCellsInSameWorld(cell, num2) && Grid.Element[num2].HasTag(GameTags.Breathable))
				{
					breathableCellRatioInSample += 1f / (float)GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
				}
			}
			flag6 = GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, brain.OxygenBreather, out totalBreathableMassAroundCell).IsBreathable;
		}
		bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
		AbsorbCellQuery.AbsorbOxygenSafeCellFlags absorbOxygenSafeCellFlags = (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		if (flag4)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotScaldingTemperatures;
		}
		if (flag5)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotRadiated;
		}
		if (flag6)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsBreathable;
		}
		if (flag)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsClear;
		}
		if (flag7)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotTube;
		}
		if (flag2)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotLiquid;
		}
		if (flag3)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotLiquidOnMyFace;
		}
		return absorbOxygenSafeCellFlags;
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x0009A66C File Offset: 0x0009886C
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		float num = 0.1f * (float)GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
		float num2 = 2.5f * (float)GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
		float num3 = (float)(54 / GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length);
		float num4 = num3 / 2.8f;
		float num5 = (float)cost;
		bool flag;
		this.checker.GetSafetyConditions(cell, cost, this.context, out flag);
		if (flag)
		{
			float num6 = 0.03f;
			float num7 = num5 / 10f;
			float num8 = num6 * num7;
			float num9 = 0f;
			float num10 = 0f;
			AbsorbCellQuery.AbsorbOxygenSafeCellFlags absorbOxygenFlags = AbsorbCellQuery.GetAbsorbOxygenFlags(cell, this.brain, this.scaldingTreshold, out num9, out num10, this.allowCellEvenIfReserved);
			num9 = Mathf.Clamp(num9, 0f, num3);
			if (this.criticalMode)
			{
				if (!this.isRecoveringFromSuffocation && this.breathPercentage > DUPLICANTSTATS.BIONICS.Breath.SUFFOCATE_AMOUNT && num9 < num)
				{
					num9 = 0f;
				}
			}
			else if (num9 < num4)
			{
				num9 = 0f;
			}
			float num11 = (float)absorbOxygenFlags;
			float num12 = 10f * num10;
			float num13 = num9 * num12 - num8;
			bool flag2 = false;
			if (this.targetCell == Grid.InvalidCell)
			{
				flag2 = true;
			}
			bool flag3 = this.targetBreathableMassAvailable > 0f;
			bool flag4 = num5 < (float)this.targetCost;
			bool flag5 = this.targetOxygenScore >= num2;
			bool flag6 = num11 >= (float)this.targetCellSafetyFlags || !flag3;
			float num14 = this.targetOxygenScore;
			if (this.criticalMode)
			{
				num14 = Mathf.Min(num2, num14);
			}
			if (num13 >= num14 && flag6)
			{
				if (this.criticalMode)
				{
					if (flag4 || !flag5)
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = true;
				}
			}
			flag2 = (flag2 && num9 > DUPLICANTSTATS.BIONICS.BaseStats.NO_OXYGEN_THRESHOLD);
			if (flag2)
			{
				this.targetBreathableMassAvailable = num9;
				this.targetCellSafetyFlags = absorbOxygenFlags;
				this.targetCost = cost;
				this.targetCell = cell;
				this.targetOxygenScore = num13;
			}
		}
		return false;
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x0009A84A File Offset: 0x00098A4A
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001066 RID: 4198
	private MinionBrain brain;

	// Token: 0x04001067 RID: 4199
	private float scaldingTreshold = -1f;

	// Token: 0x04001068 RID: 4200
	private int targetCell;

	// Token: 0x04001069 RID: 4201
	private int targetCost;

	// Token: 0x0400106A RID: 4202
	private float targetOxygenScore;

	// Token: 0x0400106B RID: 4203
	private bool criticalMode;

	// Token: 0x0400106C RID: 4204
	private float bionicOxygenRemaining;

	// Token: 0x0400106D RID: 4205
	private float breathPercentage;

	// Token: 0x0400106E RID: 4206
	private float targetBreathableMassAvailable;

	// Token: 0x0400106F RID: 4207
	public AbsorbCellQuery.AbsorbOxygenSafeCellFlags targetCellSafetyFlags;

	// Token: 0x04001070 RID: 4208
	public float targetCellBreathabilityScore;

	// Token: 0x04001071 RID: 4209
	private int allowCellEvenIfReserved = -1;

	// Token: 0x04001072 RID: 4210
	private SafetyChecker checker;

	// Token: 0x04001073 RID: 4211
	private SafetyChecker.Context context;

	// Token: 0x04001074 RID: 4212
	private bool isRecoveringFromSuffocation;

	// Token: 0x0200139E RID: 5022
	public enum AbsorbOxygenSafeCellFlags
	{
		// Token: 0x04006BF8 RID: 27640
		IsNotTube = 1,
		// Token: 0x04006BF9 RID: 27641
		IsNotRadiated,
		// Token: 0x04006BFA RID: 27642
		IsBreathable = 4,
		// Token: 0x04006BFB RID: 27643
		IsNotScaldingTemperatures = 8,
		// Token: 0x04006BFC RID: 27644
		IsClear = 16,
		// Token: 0x04006BFD RID: 27645
		IsNotLiquidOnMyFace = 32,
		// Token: 0x04006BFE RID: 27646
		IsNotLiquid = 64
	}
}
