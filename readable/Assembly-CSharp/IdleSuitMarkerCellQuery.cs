using System;

// Token: 0x02000507 RID: 1287
public class IdleSuitMarkerCellQuery : PathFinderQuery
{
	// Token: 0x06001BCE RID: 7118 RVA: 0x00099832 File Offset: 0x00097A32
	public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x)
	{
		this.targetCell = Grid.InvalidCell;
		this.isRotated = is_rotated;
		this.markerX = marker_x;
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x00099854 File Offset: 0x00097A54
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < this.markerX != this.isRotated)
		{
			this.targetCell = cell;
		}
		return this.targetCell != Grid.InvalidCell;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000998A0 File Offset: 0x00097AA0
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001038 RID: 4152
	private int targetCell;

	// Token: 0x04001039 RID: 4153
	private bool isRotated;

	// Token: 0x0400103A RID: 4154
	private int markerX;
}
