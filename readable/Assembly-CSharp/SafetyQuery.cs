using System;

// Token: 0x0200050C RID: 1292
public class SafetyQuery : PathFinderQuery
{
	// Token: 0x06001BE0 RID: 7136 RVA: 0x0009A05B File Offset: 0x0009825B
	public SafetyQuery(SafetyChecker checker, KMonoBehaviour cmp, int max_cost)
	{
		this.checker = checker;
		this.cmp = cmp;
		this.maxCost = max_cost;
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x0009A078 File Offset: 0x00098278
	public void Reset()
	{
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetConditions = 0;
		this.context = new SafetyChecker.Context(this.cmp);
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x0009A0A8 File Offset: 0x000982A8
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		bool flag = false;
		int safetyConditions = this.checker.GetSafetyConditions(cell, cost, this.context, out flag);
		if (safetyConditions != 0 && (safetyConditions > this.targetConditions || (safetyConditions == this.targetConditions && cost < this.targetCost)))
		{
			this.targetCell = cell;
			this.targetConditions = safetyConditions;
			this.targetCost = cost;
			if (flag)
			{
				return true;
			}
		}
		return cost >= this.maxCost;
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x0009A111 File Offset: 0x00098311
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001059 RID: 4185
	private int targetCell;

	// Token: 0x0400105A RID: 4186
	private int targetCost;

	// Token: 0x0400105B RID: 4187
	private int targetConditions;

	// Token: 0x0400105C RID: 4188
	private int maxCost;

	// Token: 0x0400105D RID: 4189
	private SafetyChecker checker;

	// Token: 0x0400105E RID: 4190
	private KMonoBehaviour cmp;

	// Token: 0x0400105F RID: 4191
	private SafetyChecker.Context context;
}
