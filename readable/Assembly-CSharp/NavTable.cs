using System;

// Token: 0x020004F4 RID: 1268
public class NavTable
{
	// Token: 0x06001B6F RID: 7023 RVA: 0x00097EB8 File Offset: 0x000960B8
	public NavTable(int cell_count)
	{
		this.ValidCells = new short[cell_count];
		this.NavTypeMasks = new short[11];
		for (short num = 0; num < 11; num += 1)
		{
			this.NavTypeMasks[(int)num] = (short)(1 << (int)num);
		}
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x00097F01 File Offset: 0x00096101
	public bool IsValid(int cell, NavType nav_type = NavType.Floor)
	{
		return Grid.IsValidCell(cell) && (this.NavTypeMasks[(int)nav_type] & this.ValidCells[cell]) != 0;
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x00097F24 File Offset: 0x00096124
	public void SetValid(int cell, NavType nav_type, bool is_valid)
	{
		short num = this.NavTypeMasks[(int)nav_type];
		short num2 = this.ValidCells[cell];
		if ((num2 & num) != 0 != is_valid)
		{
			if (is_valid)
			{
				this.ValidCells[cell] = (num | num2);
			}
			else
			{
				this.ValidCells[cell] = (~num & num2);
			}
			if (this.OnValidCellChanged != null)
			{
				this.OnValidCellChanged(cell, nav_type);
			}
		}
	}

	// Token: 0x04000FED RID: 4077
	public Action<int, NavType> OnValidCellChanged;

	// Token: 0x04000FEE RID: 4078
	private short[] NavTypeMasks;

	// Token: 0x04000FEF RID: 4079
	private short[] ValidCells;
}
