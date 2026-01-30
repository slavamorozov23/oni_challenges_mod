using System;
using UnityEngine;

// Token: 0x0200097D RID: 2429
public struct GridArea
{
	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x060045A0 RID: 17824 RVA: 0x00192D62 File Offset: 0x00190F62
	public Vector2I Min
	{
		get
		{
			return this.min;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x060045A1 RID: 17825 RVA: 0x00192D6A File Offset: 0x00190F6A
	public Vector2I Max
	{
		get
		{
			return this.max;
		}
	}

	// Token: 0x060045A2 RID: 17826 RVA: 0x00192D74 File Offset: 0x00190F74
	public void SetArea(int cell, int width, int height)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x + width, vector2I.y + height);
		this.SetExtents(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y);
	}

	// Token: 0x060045A3 RID: 17827 RVA: 0x00192DC0 File Offset: 0x00190FC0
	public void SetExtents(int min_x, int min_y, int max_x, int max_y)
	{
		this.min.x = Math.Max(min_x, 0);
		this.min.y = Math.Max(min_y, 0);
		this.max.x = Math.Min(max_x, Grid.WidthInCells);
		this.max.y = Math.Min(max_y, Grid.HeightInCells);
		this.MinCell = Grid.XYToCell(this.min.x, this.min.y);
		this.MaxCell = Grid.XYToCell(this.max.x, this.max.y);
	}

	// Token: 0x060045A4 RID: 17828 RVA: 0x00192E60 File Offset: 0x00191060
	public bool Contains(int cell)
	{
		if (cell >= this.MinCell && cell < this.MaxCell)
		{
			int num = cell % Grid.WidthInCells;
			return num >= this.Min.x && num < this.Max.x;
		}
		return false;
	}

	// Token: 0x060045A5 RID: 17829 RVA: 0x00192EA7 File Offset: 0x001910A7
	public bool Contains(int x, int y)
	{
		return x >= this.min.x && x < this.max.x && y >= this.min.y && y < this.max.y;
	}

	// Token: 0x060045A6 RID: 17830 RVA: 0x00192EE4 File Offset: 0x001910E4
	public bool Contains(Vector3 pos)
	{
		return (float)this.min.x <= pos.x && pos.x < (float)this.max.x && (float)this.min.y <= pos.y && pos.y <= (float)this.max.y;
	}

	// Token: 0x060045A7 RID: 17831 RVA: 0x00192F46 File Offset: 0x00191146
	public void RunIfInside(int cell, Action<int> action)
	{
		if (this.Contains(cell))
		{
			action(cell);
		}
	}

	// Token: 0x060045A8 RID: 17832 RVA: 0x00192F58 File Offset: 0x00191158
	public void Run(Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				int obj = Grid.XYToCell(j, i);
				action(obj);
			}
		}
	}

	// Token: 0x060045A9 RID: 17833 RVA: 0x00192FB4 File Offset: 0x001911B4
	public void RunOnDifference(GridArea subtract_area, Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				if (!subtract_area.Contains(j, i))
				{
					int obj = Grid.XYToCell(j, i);
					action(obj);
				}
			}
		}
	}

	// Token: 0x060045AA RID: 17834 RVA: 0x0019301B File Offset: 0x0019121B
	public int GetCellCount()
	{
		return (this.max.x - this.min.x) * (this.max.y - this.min.y);
	}

	// Token: 0x04002EF1 RID: 12017
	private Vector2I min;

	// Token: 0x04002EF2 RID: 12018
	private Vector2I max;

	// Token: 0x04002EF3 RID: 12019
	private int MinCell;

	// Token: 0x04002EF4 RID: 12020
	private int MaxCell;
}
