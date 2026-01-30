using System;
using System.Collections.Generic;

// Token: 0x020004FC RID: 1276
public class PathGrid
{
	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06001B98 RID: 7064 RVA: 0x00098ABC File Offset: 0x00096CBC
	public ulong AllocatedClassification
	{
		get
		{
			DebugUtil.Assert(this.widthInCells < 65535);
			DebugUtil.Assert(this.heightInCells < 65535);
			DebugUtil.Assert(this.ValidNavTypes.Length < 256);
			return (ulong)((((long)this.widthInCells << 16) + (long)this.heightInCells << 8) + (long)this.ValidNavTypes.Length);
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06001B99 RID: 7065 RVA: 0x00098B1F File Offset: 0x00096D1F
	public ushort SerialNo
	{
		get
		{
			return this.serialNo;
		}
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x00098B27 File Offset: 0x00096D27
	public PathGrid(PathGrid other) : this(other.widthInCells, other.heightInCells, other.applyOffset, other.ValidNavTypes)
	{
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x00098B48 File Offset: 0x00096D48
	public PathGrid(int width_in_cells, int height_in_cells, bool apply_offset, NavType[] valid_nav_types)
	{
		this.applyOffset = apply_offset;
		this.widthInCells = width_in_cells;
		this.heightInCells = height_in_cells;
		this.ValidNavTypes = valid_nav_types;
		int num = 0;
		this.NavTypeTable = new int[11];
		for (int i = 0; i < this.NavTypeTable.Length; i++)
		{
			this.NavTypeTable[i] = -1;
			for (int j = 0; j < this.ValidNavTypes.Length; j++)
			{
				if (this.ValidNavTypes[j] == (NavType)i)
				{
					this.NavTypeTable[i] = num++;
					break;
				}
			}
		}
		DebugUtil.DevAssert(true, "Cell packs nav type into 4 bits!", null);
		this.Cells = new PathFinder.Cell[width_in_cells * height_in_cells * this.ValidNavTypes.Length];
		this.ProberCells = new PathGrid.ProberCell[width_in_cells * height_in_cells];
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x00098C04 File Offset: 0x00096E04
	public void CloneNavTypes(PathGrid other)
	{
		DebugUtil.Assert(other.ValidNavTypes.Length == this.ValidNavTypes.Length);
		other.ValidNavTypes.CopyTo(this.ValidNavTypes, 0);
		int num = 0;
		for (int i = 0; i < this.NavTypeTable.Length; i++)
		{
			this.NavTypeTable[i] = -1;
			for (int j = 0; j < this.ValidNavTypes.Length; j++)
			{
				if (this.ValidNavTypes[j] == (NavType)i)
				{
					this.NavTypeTable[i] = num++;
					break;
				}
			}
		}
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x00098C88 File Offset: 0x00096E88
	public void ResetProberCells()
	{
		for (int i = 0; i < this.ProberCells.Length; i++)
		{
			this.ProberCells[i] = default(PathGrid.ProberCell);
		}
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x00098CBA File Offset: 0x00096EBA
	public void OnCleanUp()
	{
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x00098CBC File Offset: 0x00096EBC
	public void BeginUpdate(ushort new_serial_no, int root_cell, List<int> found_cells_list = null)
	{
		this.freshlyOccupiedCells = found_cells_list;
		if (this.applyOffset)
		{
			Grid.CellToXY(root_cell, out this.rootX, out this.rootY);
			this.rootX -= this.widthInCells / 2;
			this.rootY -= this.heightInCells / 2;
		}
		this.serialNo = new_serial_no;
	}

	// Token: 0x06001BA0 RID: 7072 RVA: 0x00098D1B File Offset: 0x00096F1B
	public void EndUpdate()
	{
		this.freshlyOccupiedCells = null;
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x00098D24 File Offset: 0x00096F24
	private bool IsValidSerialNo(ushort serialNo)
	{
		return serialNo == this.serialNo && serialNo > 0;
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x00098D35 File Offset: 0x00096F35
	public PathFinder.Cell GetCell(PathFinder.PotentialPath potential_path, out bool is_cell_in_range)
	{
		return this.GetCell(potential_path.cell, potential_path.navType, out is_cell_in_range);
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00098D4C File Offset: 0x00096F4C
	public PathFinder.Cell GetCell(int cell, NavType nav_type, out bool is_cell_in_range)
	{
		int num = this.OffsetCell(cell);
		is_cell_in_range = (-1 != num);
		if (!is_cell_in_range)
		{
			return PathGrid.InvalidCell;
		}
		if ((int)nav_type >= this.NavTypeTable.Length)
		{
			return PathGrid.InvalidCell;
		}
		if (num * this.ValidNavTypes.Length + this.NavTypeTable[(int)nav_type] >= this.Cells.Length)
		{
			return PathGrid.InvalidCell;
		}
		PathFinder.Cell cell2 = this.Cells[num * this.ValidNavTypes.Length + this.NavTypeTable[(int)nav_type]];
		if (!this.IsValidSerialNo(cell2.queryId))
		{
			return PathGrid.InvalidCell;
		}
		return cell2;
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x00098DDC File Offset: 0x00096FDC
	private PathGrid.ProberCell GetProberCell(int cell)
	{
		int num = this.OffsetCell(cell);
		if (num == -1)
		{
			return PathGrid.InvalidProberCell;
		}
		return this.ProberCells[num];
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x00098E08 File Offset: 0x00097008
	public void SetCell(PathFinder.PotentialPath potential_path, ref PathFinder.Cell cell_data)
	{
		int num = this.OffsetCell(potential_path.cell);
		if (-1 == num)
		{
			return;
		}
		cell_data.queryId = this.serialNo;
		int num2 = this.NavTypeTable[(int)potential_path.navType];
		int num3 = num * this.ValidNavTypes.Length + num2;
		this.Cells[num3] = cell_data;
		if (potential_path.navType != NavType.Tube)
		{
			PathGrid.ProberCell proberCell = this.ProberCells[num];
			if (cell_data.queryId != proberCell.queryId && this.freshlyOccupiedCells != null)
			{
				this.freshlyOccupiedCells.Add(potential_path.cell);
			}
			if (cell_data.queryId != proberCell.queryId || cell_data.cost < proberCell.cost)
			{
				proberCell.queryId = cell_data.queryId;
				proberCell.cost = cell_data.cost;
				proberCell.navType = potential_path.navType;
				this.ProberCells[num] = proberCell;
			}
		}
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x00098EF0 File Offset: 0x000970F0
	public int GetCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		int num = -1;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(num2))
			{
				PathGrid.ProberCell proberCell = this.ProberCells[num2];
				if (this.IsValidSerialNo(proberCell.queryId) && (num == -1 || proberCell.cost < num))
				{
					num = proberCell.cost;
				}
			}
		}
		return num;
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x00098F60 File Offset: 0x00097160
	public int GetCost(int cell)
	{
		int num = this.OffsetCell(cell);
		if (-1 == num)
		{
			return -1;
		}
		PathGrid.ProberCell proberCell = this.ProberCells[num];
		if (!this.IsValidSerialNo(proberCell.queryId))
		{
			return -1;
		}
		return proberCell.cost;
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x00098FA0 File Offset: 0x000971A0
	private int OffsetCell(int cell)
	{
		if (!this.applyOffset)
		{
			return cell;
		}
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		if (num < this.rootX || num >= this.rootX + this.widthInCells || num2 < this.rootY || num2 >= this.rootY + this.heightInCells)
		{
			return -1;
		}
		int num3 = num - this.rootX;
		return (num2 - this.rootY) * this.widthInCells + num3;
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x00099010 File Offset: 0x00097210
	public bool BuildPath(int source_cell, int target_cell, NavType current_nav_type, ref PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			path.nodes.Clear();
		}
		path.cost = -1;
		if (target_cell == PathFinder.InvalidCell || this.GetCost(target_cell) == -1 || this.GetCost(source_cell) == -1)
		{
			return false;
		}
		bool flag = false;
		PathGrid.ProberCell proberCell = this.GetProberCell(target_cell);
		PathFinder.Cell cell = this.GetCell(target_cell, proberCell.navType, out flag);
		path.Clear();
		path.cost = cell.cost;
		int cost = path.cost;
		while (target_cell != PathFinder.InvalidCell)
		{
			path.AddNode(new PathFinder.Path.Node
			{
				cell = target_cell,
				navType = cell.navType,
				transitionId = cell.transitionId
			});
			if (target_cell == source_cell && cell.navType == current_nav_type)
			{
				path.nodes.Reverse();
				return true;
			}
			if (target_cell != PathFinder.InvalidCell)
			{
				target_cell = cell.parent;
				cell = this.GetCell(target_cell, cell.parentNavType, out flag);
			}
			if (cell.cost >= cost && target_cell != PathFinder.InvalidCell)
			{
				KCrashReporter.ReportDevNotification("Invalid Cost Progression", Environment.StackTrace, string.Format("{0}x{1} -> {2} via path of length {3} cell_data.cost: {4} previousCost: {5} cell_data.navType: {6}", new object[]
				{
					source_cell,
					current_nav_type,
					target_cell,
					path.nodes.Count,
					cell.cost,
					cost,
					cell.navType
				}), false, null);
				break;
			}
			cost = cell.cost;
		}
		path.Clear();
		return false;
	}

	// Token: 0x04001013 RID: 4115
	private PathFinder.Cell[] Cells;

	// Token: 0x04001014 RID: 4116
	private PathGrid.ProberCell[] ProberCells;

	// Token: 0x04001015 RID: 4117
	private List<int> freshlyOccupiedCells;

	// Token: 0x04001016 RID: 4118
	public NavType[] ValidNavTypes;

	// Token: 0x04001017 RID: 4119
	public int[] NavTypeTable;

	// Token: 0x04001018 RID: 4120
	public int widthInCells;

	// Token: 0x04001019 RID: 4121
	public int heightInCells;

	// Token: 0x0400101A RID: 4122
	public bool applyOffset;

	// Token: 0x0400101B RID: 4123
	private int rootX;

	// Token: 0x0400101C RID: 4124
	private int rootY;

	// Token: 0x0400101D RID: 4125
	private ushort serialNo;

	// Token: 0x0400101E RID: 4126
	public static readonly PathFinder.Cell InvalidCell = new PathFinder.Cell
	{
		cost = -1,
		parent = -1
	};

	// Token: 0x0400101F RID: 4127
	private static readonly PathGrid.ProberCell InvalidProberCell = new PathGrid.ProberCell
	{
		cost = -1,
		queryId = 0,
		navType = NavType.Floor
	};

	// Token: 0x02001394 RID: 5012
	private struct ProberCell
	{
		// Token: 0x04006BB5 RID: 27573
		public int cost;

		// Token: 0x04006BB6 RID: 27574
		public ushort queryId;

		// Token: 0x04006BB7 RID: 27575
		public NavType navType;
	}
}
