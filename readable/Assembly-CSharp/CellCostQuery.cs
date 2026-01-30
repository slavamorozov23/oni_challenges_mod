using System;

// Token: 0x02000501 RID: 1281
public class CellCostQuery : PathFinderQuery
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x0009957E File Offset: 0x0009777E
	// (set) Token: 0x06001BBA RID: 7098 RVA: 0x00099586 File Offset: 0x00097786
	public int resultCost { get; private set; }

	// Token: 0x06001BBB RID: 7099 RVA: 0x0009958F File Offset: 0x0009778F
	public void Reset(int target_cell, int max_cost)
	{
		this.targetCell = target_cell;
		this.maxCost = max_cost;
		this.resultCost = -1;
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x000995A6 File Offset: 0x000977A6
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (cost > this.maxCost)
		{
			return true;
		}
		if (cell == this.targetCell)
		{
			this.resultCost = cost;
			return true;
		}
		return false;
	}

	// Token: 0x0400102E RID: 4142
	private int targetCell;

	// Token: 0x0400102F RID: 4143
	private int maxCost;
}
