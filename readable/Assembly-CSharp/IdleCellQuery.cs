using System;

// Token: 0x02000506 RID: 1286
public class IdleCellQuery : PathFinderQuery
{
	// Token: 0x06001BCA RID: 7114 RVA: 0x000997BB File Offset: 0x000979BB
	public IdleCellQuery Reset(MinionBrain brain, int max_cost)
	{
		this.brain = brain;
		this.maxCost = max_cost;
		this.targetCell = Grid.InvalidCell;
		return this;
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x000997D8 File Offset: 0x000979D8
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, false, (SafeCellQuery.SafeFlags)0);
		if ((flags & SafeCellQuery.SafeFlags.IsClear) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLadder) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotTube) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags)0)
		{
			this.targetCell = cell;
		}
		return cost > this.maxCost;
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x00099822 File Offset: 0x00097A22
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001035 RID: 4149
	private MinionBrain brain;

	// Token: 0x04001036 RID: 4150
	private int targetCell;

	// Token: 0x04001037 RID: 4151
	private int maxCost;
}
