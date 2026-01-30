using System;

// Token: 0x020009E0 RID: 2528
public class LiquidFetchMask
{
	// Token: 0x06004985 RID: 18821 RVA: 0x001A9C68 File Offset: 0x001A7E68
	public LiquidFetchMask(CellOffset[][] offset_table)
	{
		for (int i = 0; i < offset_table.Length; i++)
		{
			for (int j = 0; j < offset_table[i].Length; j++)
			{
				this.maxOffset.x = Math.Max(this.maxOffset.x, Math.Abs(offset_table[i][j].x));
				this.maxOffset.y = Math.Max(this.maxOffset.y, Math.Abs(offset_table[i][j].y));
			}
		}
		this.isLiquidAvailable = new bool[Grid.CellCount];
		for (int k = 0; k < Grid.CellCount; k++)
		{
			this.RefreshCell(k);
		}
	}

	// Token: 0x06004986 RID: 18822 RVA: 0x001A9D1C File Offset: 0x001A7F1C
	private void RefreshCell(int cell)
	{
		CellOffset offset = Grid.GetOffset(cell);
		int num = Math.Max(0, offset.y - this.maxOffset.y);
		while (num < Grid.HeightInCells && num < offset.y + this.maxOffset.y)
		{
			int num2 = Math.Max(0, offset.x - this.maxOffset.x);
			while (num2 < Grid.WidthInCells && num2 < offset.x + this.maxOffset.x)
			{
				if (Grid.Element[Grid.XYToCell(num2, num)].IsLiquid)
				{
					this.isLiquidAvailable[cell] = true;
					return;
				}
				num2++;
			}
			num++;
		}
		this.isLiquidAvailable[cell] = false;
	}

	// Token: 0x06004987 RID: 18823 RVA: 0x001A9DCF File Offset: 0x001A7FCF
	public void MarkDirty(int cell)
	{
		this.RefreshCell(cell);
	}

	// Token: 0x06004988 RID: 18824 RVA: 0x001A9DD8 File Offset: 0x001A7FD8
	public bool IsLiquidAvailable(int cell)
	{
		return this.isLiquidAvailable[cell];
	}

	// Token: 0x06004989 RID: 18825 RVA: 0x001A9DE2 File Offset: 0x001A7FE2
	public void Destroy()
	{
		this.isLiquidAvailable = null;
	}

	// Token: 0x040030F9 RID: 12537
	private bool[] isLiquidAvailable;

	// Token: 0x040030FA RID: 12538
	private CellOffset maxOffset;
}
