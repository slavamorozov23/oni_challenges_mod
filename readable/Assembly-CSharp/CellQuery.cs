using System;

// Token: 0x02000503 RID: 1283
public class CellQuery : PathFinderQuery
{
	// Token: 0x06001BC0 RID: 7104 RVA: 0x00099615 File Offset: 0x00097815
	public CellQuery Reset(int target_cell)
	{
		this.targetCell = target_cell;
		return this;
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x0009961F File Offset: 0x0009781F
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return cell == this.targetCell;
	}

	// Token: 0x04001031 RID: 4145
	private int targetCell;
}
