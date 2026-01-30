using System;

// Token: 0x020004F5 RID: 1269
public class NavTableValidator
{
	// Token: 0x06001B72 RID: 7026 RVA: 0x00097F80 File Offset: 0x00096180
	protected bool IsClear(int cell, CellOffset[] bounding_offsets, bool is_dupe)
	{
		foreach (CellOffset offset in bounding_offsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			if (!Grid.IsWorldValidCell(cell2) || !NavTableValidator.IsCellPassable(cell2, is_dupe))
			{
				return false;
			}
			int num = Grid.CellAbove(cell2);
			if (Grid.IsValidCell(num) && Grid.Element[num].IsUnstable)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x00097FE0 File Offset: 0x000961E0
	protected static bool IsCellPassable(int cell, bool is_dupe)
	{
		Grid.BuildFlags buildFlags = Grid.BuildMasks[cell] & ~(Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.FakeFloor);
		if (buildFlags == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
		{
			return true;
		}
		if (is_dupe)
		{
			return (buildFlags & Grid.BuildFlags.DupeImpassable) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) && ((buildFlags & Grid.BuildFlags.Solid) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || (buildFlags & Grid.BuildFlags.DupePassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor));
		}
		return (buildFlags & (Grid.BuildFlags.Solid | Grid.BuildFlags.CritterImpassable)) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0009801D File Offset: 0x0009621D
	public virtual void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
	{
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x0009801F File Offset: 0x0009621F
	public virtual void Clear()
	{
	}

	// Token: 0x04000FF0 RID: 4080
	public Action<int> onDirty;
}
