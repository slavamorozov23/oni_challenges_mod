using System;

// Token: 0x02000502 RID: 1282
public class CellOffsetQuery : CellArrayQuery
{
	// Token: 0x06001BBE RID: 7102 RVA: 0x000995D0 File Offset: 0x000977D0
	public CellArrayQuery Reset(int cell, CellOffset[] offsets)
	{
		int[] array = new int[offsets.Length];
		for (int i = 0; i < offsets.Length; i++)
		{
			array[i] = Grid.OffsetCell(cell, offsets[i]);
		}
		base.Reset(array);
		return this;
	}
}
