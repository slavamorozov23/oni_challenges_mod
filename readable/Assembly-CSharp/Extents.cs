using System;
using UnityEngine;

// Token: 0x02000B2D RID: 2861
public struct Extents
{
	// Token: 0x06005417 RID: 21527 RVA: 0x001EB2B0 File Offset: 0x001E94B0
	public static Extents OneCell(int cell)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return new Extents(num, num2, 1, 1);
	}

	// Token: 0x06005418 RID: 21528 RVA: 0x001EB2D0 File Offset: 0x001E94D0
	public Extents(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x06005419 RID: 21529 RVA: 0x001EB2F0 File Offset: 0x001E94F0
	public Extents(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.x = num - radius;
		this.y = num2 - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x0600541A RID: 21530 RVA: 0x001EB333 File Offset: 0x001E9533
	public Extents(int center_x, int center_y, int radius)
	{
		this.x = center_x - radius;
		this.y = center_y - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x0600541B RID: 21531 RVA: 0x001EB360 File Offset: 0x001E9560
	public Extents(int cell, CellOffset[] offsets)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, offset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x0600541C RID: 21532 RVA: 0x001EB3FC File Offset: 0x001E95FC
	public Extents(int cell, CellOffset[] offsets, Extents.BoundExtendsToGridFlag _)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			int cell2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(cell2))
			{
				Grid.CellToXY(cell2, out val, out val2);
				num = Math.Min(num, val);
				num2 = Math.Min(num2, val2);
				num3 = Math.Max(num3, val);
				num4 = Math.Max(num4, val2);
			}
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x0600541D RID: 21533 RVA: 0x001EB4A4 File Offset: 0x001E96A4
	public Extents(int cell, CellOffset[] offsets, Orientation orientation)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		for (int i = 0; i < offsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offsets[i], orientation);
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x0600541E RID: 21534 RVA: 0x001EB544 File Offset: 0x001E9744
	public Extents(int cell, CellOffset[][] offset_table)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset[] array in offset_table)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, array[0]), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x001EB5E0 File Offset: 0x001E97E0
	public bool Contains(Vector2I pos)
	{
		return this.x <= pos.x && pos.x < this.x + this.width && this.y <= pos.y && pos.y < this.y + this.height;
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x001EB638 File Offset: 0x001E9838
	public bool Contains(Vector3 pos)
	{
		return (float)this.x <= pos.x && pos.x < (float)(this.x + this.width) && (float)this.y <= pos.y && pos.y < (float)(this.y + this.height);
	}

	// Token: 0x040038CE RID: 14542
	public int x;

	// Token: 0x040038CF RID: 14543
	public int y;

	// Token: 0x040038D0 RID: 14544
	public int width;

	// Token: 0x040038D1 RID: 14545
	public int height;

	// Token: 0x040038D2 RID: 14546
	public static Extents.BoundExtendsToGridFlag BoundsCheckCoords;

	// Token: 0x02001C97 RID: 7319
	public struct BoundExtendsToGridFlag
	{
	}
}
