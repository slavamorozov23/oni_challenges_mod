using System;

// Token: 0x0200083C RID: 2108
public class CellVisibility
{
	// Token: 0x0600398A RID: 14730 RVA: 0x001416F4 File Offset: 0x0013F8F4
	public CellVisibility()
	{
		Grid.GetVisibleExtents(out this.MinX, out this.MinY, out this.MaxX, out this.MaxY);
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x0014171C File Offset: 0x0013F91C
	public bool IsVisible(int cell)
	{
		int num = Grid.CellColumn(cell);
		if (num < this.MinX || num > this.MaxX)
		{
			return false;
		}
		int num2 = Grid.CellRow(cell);
		return num2 >= this.MinY && num2 <= this.MaxY;
	}

	// Token: 0x04002337 RID: 9015
	private int MinX;

	// Token: 0x04002338 RID: 9016
	private int MinY;

	// Token: 0x04002339 RID: 9017
	private int MaxX;

	// Token: 0x0400233A RID: 9018
	private int MaxY;
}
