using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using ProcGen;
using UnityEngine;

// Token: 0x0200097C RID: 2428
public class Grid
{
	// Token: 0x0600452B RID: 17707 RVA: 0x00191035 File Offset: 0x0018F235
	private static void UpdateBuildMask(int i, Grid.BuildFlags flag, bool state)
	{
		if (state)
		{
			Grid.BuildMasks[i] |= flag;
			return;
		}
		Grid.BuildMasks[i] &= ~flag;
	}

	// Token: 0x0600452C RID: 17708 RVA: 0x0019105D File Offset: 0x0018F25D
	public static void SetSolid(int cell, bool solid, CellSolidEvent ev)
	{
		Grid.UpdateBuildMask(cell, Grid.BuildFlags.Solid, solid);
	}

	// Token: 0x0600452D RID: 17709 RVA: 0x00191069 File Offset: 0x0018F269
	private static void UpdateVisMask(int i, Grid.VisFlags flag, bool state)
	{
		if (state)
		{
			Grid.VisMasks[i] |= flag;
			return;
		}
		Grid.VisMasks[i] &= ~flag;
	}

	// Token: 0x0600452E RID: 17710 RVA: 0x00191091 File Offset: 0x0018F291
	private static void UpdateNavValidatorMask(int i, Grid.NavValidatorFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavValidatorMasks[i] |= flag;
			return;
		}
		Grid.NavValidatorMasks[i] &= ~flag;
	}

	// Token: 0x0600452F RID: 17711 RVA: 0x001910B9 File Offset: 0x0018F2B9
	private static void UpdateNavMask(int i, Grid.NavFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavMasks[i] |= flag;
			return;
		}
		Grid.NavMasks[i] &= ~flag;
	}

	// Token: 0x06004530 RID: 17712 RVA: 0x001910E1 File Offset: 0x0018F2E1
	public static void ResetNavMasksAndDetails()
	{
		Grid.NavMasks = null;
		Grid.tubeEntrances.Clear();
		Grid.restrictions.Clear();
		Grid.suitMarkers.Clear();
	}

	// Token: 0x06004531 RID: 17713 RVA: 0x00191107 File Offset: 0x0018F307
	public static bool DEBUG_GetRestrictions(int cell, out Grid.Restriction restriction)
	{
		return Grid.restrictions.TryGetValue(cell, out restriction);
	}

	// Token: 0x06004532 RID: 17714 RVA: 0x00191118 File Offset: 0x0018F318
	public static void RegisterRestriction(int cell, Grid.Restriction.Orientation orientation)
	{
		Grid.HasAccessDoor[cell] = true;
		Grid.restrictions[cell] = new Grid.Restriction
		{
			DirectionMasksForMinionInstanceID = new Dictionary<int, Grid.Restriction.Directions>(),
			orientation = orientation
		};
	}

	// Token: 0x06004533 RID: 17715 RVA: 0x0019115C File Offset: 0x0018F35C
	public static void UnregisterRestriction(int cell)
	{
		Grid.Restriction restriction;
		Grid.restrictions.TryRemove(cell, out restriction);
		Grid.HasAccessDoor[cell] = false;
	}

	// Token: 0x06004534 RID: 17716 RVA: 0x00191183 File Offset: 0x0018F383
	public static void SetRestriction(int cell, int minionInstanceID, Grid.Restriction.Directions directions)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID[minionInstanceID] = directions;
	}

	// Token: 0x06004535 RID: 17717 RVA: 0x0019119C File Offset: 0x0018F39C
	public static void ClearRestriction(int cell, int minionInstanceID)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID.Remove(minionInstanceID);
	}

	// Token: 0x06004536 RID: 17718 RVA: 0x001911B8 File Offset: 0x0018F3B8
	public static bool HasPermission(int cell, int minionInstanceID, int tagID, int fromCell, NavType fromNavType)
	{
		Grid.Restriction restriction;
		if (!Grid.HasAccessDoor[cell] || !Grid.restrictions.TryGetValue(cell, out restriction))
		{
			return true;
		}
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = Grid.CellToXY(fromCell);
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		int num = vector2I.x - vector2I2.x;
		int num2 = vector2I.y - vector2I2.y;
		switch (restriction.orientation)
		{
		case Grid.Restriction.Orientation.Vertical:
			if (num < 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num > 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.Horizontal:
			if (num2 > 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num2 < 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.SingleCell:
			if (Math.Abs(num) != 1 && Math.Abs(num2) != 1 && fromNavType != NavType.Teleport)
			{
				directions |= Grid.Restriction.Directions.Teleport;
			}
			break;
		}
		Grid.Restriction.Directions directions2 = (Grid.Restriction.Directions)0;
		return (!restriction.DirectionMasksForMinionInstanceID.TryGetValue(minionInstanceID, out directions2) && !restriction.DirectionMasksForMinionInstanceID.TryGetValue(tagID, out directions2)) || (directions2 & directions) == (Grid.Restriction.Directions)0;
	}

	// Token: 0x06004537 RID: 17719 RVA: 0x0019129C File Offset: 0x0018F49C
	public static void RegisterTubeEntrance(int cell, int reservationCapacity)
	{
		DebugUtil.Assert(!Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = true;
		Grid.tubeEntrances[cell] = new Grid.TubeEntrance
		{
			reservationCapacity = reservationCapacity,
			reservedInstanceIDs = new HashSet<int>()
		};
	}

	// Token: 0x06004538 RID: 17720 RVA: 0x001912F0 File Offset: 0x0018F4F0
	public static void UnregisterTubeEntrance(int cell)
	{
		DebugUtil.Assert(Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = false;
		Grid.TubeEntrance tubeEntrance;
		Grid.tubeEntrances.TryRemove(cell, out tubeEntrance);
	}

	// Token: 0x06004539 RID: 17721 RVA: 0x00191328 File Offset: 0x0018F528
	public static bool ReserveTubeEntrance(int cell, int minionInstanceID, bool reserve)
	{
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		if (!reserve)
		{
			return reservedInstanceIDs.Remove(minionInstanceID);
		}
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		if (reservedInstanceIDs.Count == tubeEntrance.reservationCapacity)
		{
			return false;
		}
		DebugUtil.Assert(reservedInstanceIDs.Add(minionInstanceID));
		return true;
	}

	// Token: 0x0600453A RID: 17722 RVA: 0x00191380 File Offset: 0x0018F580
	public static void SetTubeEntranceReservationCapacity(int cell, int newReservationCapacity)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.reservationCapacity = newReservationCapacity;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x0600453B RID: 17723 RVA: 0x001913C0 File Offset: 0x0018F5C0
	public static bool HasUsableTubeEntrance(int cell, int minionInstanceID)
	{
		Grid.TubeEntrance tubeEntrance;
		if (!Grid.HasTubeEntrance[cell] || !Grid.tubeEntrances.TryGetValue(cell, out tubeEntrance))
		{
			return false;
		}
		if (!tubeEntrance.operational)
		{
			return false;
		}
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		return reservedInstanceIDs.Count < tubeEntrance.reservationCapacity || reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x0600453C RID: 17724 RVA: 0x00191413 File Offset: 0x0018F613
	public static bool HasReservedTubeEntrance(int cell, int minionInstanceID)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		return Grid.tubeEntrances[cell].reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x0600453D RID: 17725 RVA: 0x0019143C File Offset: 0x0018F63C
	public static void SetTubeEntranceOperational(int cell, bool operational)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.operational = operational;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x0600453E RID: 17726 RVA: 0x0019147C File Offset: 0x0018F67C
	public static void RegisterSuitMarker(int cell)
	{
		DebugUtil.Assert(!Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = true;
		Grid.suitMarkers[cell] = new Grid.SuitMarker
		{
			suitCount = 0,
			lockerCount = 0,
			flags = Grid.SuitMarker.Flags.Operational,
			minionIDsWithSuitReservations = new HashSet<int>(),
			minionIDsWithEmptyLockerReservations = new HashSet<int>()
		};
	}

	// Token: 0x0600453F RID: 17727 RVA: 0x001914EC File Offset: 0x0018F6EC
	public static void UnregisterSuitMarker(int cell)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = false;
		Grid.SuitMarker suitMarker;
		Grid.suitMarkers.TryRemove(cell, out suitMarker);
	}

	// Token: 0x06004540 RID: 17728 RVA: 0x00191524 File Offset: 0x0018F724
	public static bool ReserveSuit(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithSuitReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag);
			return flag;
		}
		if (minionIDsWithSuitReservations.Count >= suitMarker.suitCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithSuitReservations.Add(minionInstanceID));
		return true;
	}

	// Token: 0x06004541 RID: 17729 RVA: 0x00191584 File Offset: 0x0018F784
	public static bool ReserveEmptyLocker(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell], "No suit marker");
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithEmptyLockerReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag, "Reservation not removed");
			return flag;
		}
		if (minionIDsWithEmptyLockerReservations.Count >= suitMarker.emptyLockerCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithEmptyLockerReservations.Add(minionInstanceID), "Reservation not made");
		return true;
	}

	// Token: 0x06004542 RID: 17730 RVA: 0x001915F4 File Offset: 0x0018F7F4
	public static void UpdateSuitMarker(int cell, int fullLockerCount, int emptyLockerCount, Grid.SuitMarker.Flags flags, PathFinder.PotentialPath.Flags pathFlags)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker value = Grid.suitMarkers[cell];
		value.suitCount = fullLockerCount;
		value.lockerCount = fullLockerCount + emptyLockerCount;
		value.flags = flags;
		value.pathFlags = pathFlags;
		Grid.suitMarkers[cell] = value;
	}

	// Token: 0x06004543 RID: 17731 RVA: 0x0019164C File Offset: 0x0018F84C
	public static bool TryGetSuitMarkerFlags(int cell, out Grid.SuitMarker.Flags flags, out PathFinder.PotentialPath.Flags pathFlags)
	{
		Grid.SuitMarker suitMarker;
		if (Grid.HasSuitMarker[cell] && Grid.suitMarkers.TryGetValue(cell, out suitMarker))
		{
			flags = suitMarker.flags;
			pathFlags = suitMarker.pathFlags;
			return true;
		}
		flags = (Grid.SuitMarker.Flags)0;
		pathFlags = PathFinder.PotentialPath.Flags.None;
		return false;
	}

	// Token: 0x06004544 RID: 17732 RVA: 0x00191690 File Offset: 0x0018F890
	public static bool HasSuit(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		return minionIDsWithSuitReservations.Count < suitMarker.suitCount || minionIDsWithSuitReservations.Contains(minionInstanceID);
	}

	// Token: 0x06004545 RID: 17733 RVA: 0x001916D8 File Offset: 0x0018F8D8
	public static bool HasEmptyLocker(int cell, int minionInstanceID)
	{
		Grid.SuitMarker suitMarker;
		if (!Grid.HasSuitMarker[cell] || !Grid.suitMarkers.TryGetValue(cell, out suitMarker))
		{
			return false;
		}
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		return minionIDsWithEmptyLockerReservations.Count < suitMarker.emptyLockerCount || minionIDsWithEmptyLockerReservations.Contains(minionInstanceID);
	}

	// Token: 0x06004546 RID: 17734 RVA: 0x00191724 File Offset: 0x0018F924
	public unsafe static void InitializeCells()
	{
		for (int num = 0; num != Grid.WidthInCells * Grid.HeightInCells; num++)
		{
			ushort index = Grid.elementIdx[num];
			Element element = ElementLoader.elements[(int)index];
			Grid.Element[num] = element;
			if (element.IsSolid)
			{
				Grid.BuildMasks[num] |= Grid.BuildFlags.Solid;
			}
			else
			{
				Grid.BuildMasks[num] &= ~Grid.BuildFlags.Solid;
			}
			Grid.RenderedByWorld[num] = (element.substance != null && element.substance.renderedByWorld && Grid.Objects[num, 9] == null);
		}
	}

	// Token: 0x06004547 RID: 17735 RVA: 0x001917D1 File Offset: 0x0018F9D1
	public static bool IsInitialized()
	{
		return Grid.mass != null;
	}

	// Token: 0x06004548 RID: 17736 RVA: 0x001917E0 File Offset: 0x0018F9E0
	public static int GetCellInDirection(int cell, Direction d)
	{
		switch (d)
		{
		case Direction.Up:
			return Grid.CellAbove(cell);
		case Direction.Right:
			return Grid.CellRight(cell);
		case Direction.Down:
			return Grid.CellBelow(cell);
		case Direction.Left:
			return Grid.CellLeft(cell);
		case Direction.None:
			return cell;
		}
		return -1;
	}

	// Token: 0x06004549 RID: 17737 RVA: 0x0019182C File Offset: 0x0018FA2C
	public static bool Raycast(int cell, Vector2I direction, out int hitDistance, int maxDistance = 100, Grid.BuildFlags layerMask = Grid.BuildFlags.Any)
	{
		bool flag = false;
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = vector2I + direction * maxDistance;
		int num = cell;
		int num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
		int num3 = 0;
		int num4 = 0;
		float num5 = (float)maxDistance * 0.5f;
		while ((float)num3 < num5)
		{
			if (!Grid.IsValidCell(num) || (Grid.BuildMasks[num] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				flag = true;
				break;
			}
			if (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				num4 = maxDistance - num3;
			}
			vector2I += direction;
			vector2I2 -= direction;
			num = Grid.XYToCell(vector2I.x, vector2I.y);
			num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
			num3++;
		}
		if (!flag && maxDistance % 2 == 0)
		{
			flag = (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor));
		}
		hitDistance = (flag ? num3 : ((num4 > 0) ? num4 : maxDistance));
		return flag | hitDistance == num4;
	}

	// Token: 0x0600454A RID: 17738 RVA: 0x0019192D File Offset: 0x0018FB2D
	public static int CellAbove(int cell)
	{
		return cell + Grid.WidthInCells;
	}

	// Token: 0x0600454B RID: 17739 RVA: 0x00191936 File Offset: 0x0018FB36
	public static int CellBelow(int cell)
	{
		return cell - Grid.WidthInCells;
	}

	// Token: 0x0600454C RID: 17740 RVA: 0x0019193F File Offset: 0x0018FB3F
	public static int CellLeft(int cell)
	{
		if (cell % Grid.WidthInCells <= 0)
		{
			return Grid.InvalidCell;
		}
		return cell - 1;
	}

	// Token: 0x0600454D RID: 17741 RVA: 0x00191954 File Offset: 0x0018FB54
	public static int CellRight(int cell)
	{
		if (cell % Grid.WidthInCells >= Grid.WidthInCells - 1)
		{
			return Grid.InvalidCell;
		}
		return cell + 1;
	}

	// Token: 0x0600454E RID: 17742 RVA: 0x00191970 File Offset: 0x0018FB70
	public static CellOffset GetOffset(int cell)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return new CellOffset(x, y);
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x00191994 File Offset: 0x0018FB94
	public static int CellUpLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x001919CC File Offset: 0x0018FBCC
	public static int CellUpRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06004551 RID: 17745 RVA: 0x00191A0C File Offset: 0x0018FC0C
	public static int CellDownLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell > Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06004552 RID: 17746 RVA: 0x00191A3C File Offset: 0x0018FC3C
	public static int CellDownRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell >= Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x06004553 RID: 17747 RVA: 0x00191A72 File Offset: 0x0018FC72
	public static bool IsCellLeftOf(int cell, int other_cell)
	{
		return Grid.CellColumn(cell) < Grid.CellColumn(other_cell);
	}

	// Token: 0x06004554 RID: 17748 RVA: 0x00191A84 File Offset: 0x0018FC84
	public static bool IsCellOffsetOf(int cell, int target_cell, CellOffset[] target_offsets)
	{
		int num = target_offsets.Length;
		for (int i = 0; i < num; i++)
		{
			if (cell == Grid.OffsetCell(target_cell, target_offsets[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004555 RID: 17749 RVA: 0x00191AB4 File Offset: 0x0018FCB4
	public static int GetCellDistance(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Abs(num - num3) + Math.Abs(num2 - num4);
	}

	// Token: 0x06004556 RID: 17750 RVA: 0x00191AE8 File Offset: 0x0018FCE8
	public static int GetCellRange(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Max(Math.Abs(num - num3), Math.Abs(num2 - num4));
	}

	// Token: 0x06004557 RID: 17751 RVA: 0x00191B20 File Offset: 0x0018FD20
	public static CellOffset GetOffset(int base_cell, int offset_cell)
	{
		int num;
		int num2;
		Grid.CellToXY(base_cell, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(offset_cell, out num3, out num4);
		return new CellOffset(num3 - num, num4 - num2);
	}

	// Token: 0x06004558 RID: 17752 RVA: 0x00191B4C File Offset: 0x0018FD4C
	public static CellOffset GetCellOffsetDirection(int base_cell, int offset_cell)
	{
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		offset.x = Mathf.Clamp(offset.x, -1, 1);
		offset.y = Mathf.Clamp(offset.y, -1, 1);
		return offset;
	}

	// Token: 0x06004559 RID: 17753 RVA: 0x00191B8A File Offset: 0x0018FD8A
	public static int OffsetCell(int cell, CellOffset offset)
	{
		return cell + offset.x + offset.y * Grid.WidthInCells;
	}

	// Token: 0x0600455A RID: 17754 RVA: 0x00191BA1 File Offset: 0x0018FDA1
	public static int OffsetCell(int cell, int x, int y)
	{
		return cell + x + y * Grid.WidthInCells;
	}

	// Token: 0x0600455B RID: 17755 RVA: 0x00191BB0 File Offset: 0x0018FDB0
	public static bool IsCellOffsetValid(int cell, int x, int y)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return num + x >= 0 && num + x < Grid.WidthInCells && num2 + y >= 0 && num2 + y < Grid.HeightInCells;
	}

	// Token: 0x0600455C RID: 17756 RVA: 0x00191BEB File Offset: 0x0018FDEB
	public static bool IsCellOffsetValid(int cell, CellOffset offset)
	{
		return Grid.IsCellOffsetValid(cell, offset.x, offset.y);
	}

	// Token: 0x0600455D RID: 17757 RVA: 0x00191BFF File Offset: 0x0018FDFF
	public static int PosToCell(StateMachine.Instance smi)
	{
		return Grid.PosToCell(smi.transform.GetPosition());
	}

	// Token: 0x0600455E RID: 17758 RVA: 0x00191C11 File Offset: 0x0018FE11
	public static int PosToCell(GameObject go)
	{
		return Grid.PosToCell(go.transform.GetPosition());
	}

	// Token: 0x0600455F RID: 17759 RVA: 0x00191C23 File Offset: 0x0018FE23
	public static int PosToCell(KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x06004560 RID: 17760 RVA: 0x00191C38 File Offset: 0x0018FE38
	public static bool IsValidBuildingCell(int cell)
	{
		if (!Grid.IsWorldValidCell(cell))
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
		if (world == null)
		{
			return false;
		}
		Vector2I vector2I = Grid.CellToXY(cell);
		return (float)vector2I.x >= world.minimumBounds.x && (float)vector2I.x <= world.maximumBounds.x && (float)vector2I.y >= world.minimumBounds.y && (float)vector2I.y <= world.maximumBounds.y - (float)Grid.TopBorderHeight;
	}

	// Token: 0x06004561 RID: 17761 RVA: 0x00191CCF File Offset: 0x0018FECF
	public static bool IsWorldValidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue;
	}

	// Token: 0x06004562 RID: 17762 RVA: 0x00191CEC File Offset: 0x0018FEEC
	public static bool IsValidCell(int cell)
	{
		return cell >= 0 && cell < Grid.CellCount;
	}

	// Token: 0x06004563 RID: 17763 RVA: 0x00191CFC File Offset: 0x0018FEFC
	public static bool IsValidCellInWorld(int cell, int world)
	{
		return cell >= 0 && cell < Grid.CellCount && (int)Grid.WorldIdx[cell] == world;
	}

	// Token: 0x06004564 RID: 17764 RVA: 0x00191D16 File Offset: 0x0018FF16
	public static bool IsActiveWorld(int cell)
	{
		return ClusterManager.Instance != null && ClusterManager.Instance.activeWorldId == (int)Grid.WorldIdx[cell];
	}

	// Token: 0x06004565 RID: 17765 RVA: 0x00191D3A File Offset: 0x0018FF3A
	public static bool AreCellsInSameWorld(int cell, int world_cell)
	{
		return Grid.IsValidCell(cell) && Grid.IsValidCell(world_cell) && Grid.WorldIdx[cell] == Grid.WorldIdx[world_cell];
	}

	// Token: 0x06004566 RID: 17766 RVA: 0x00191D5E File Offset: 0x0018FF5E
	public static bool IsCellOpenToSpace(int cell)
	{
		return !Grid.IsSolidCell(cell) && !(Grid.Objects[cell, 2] != null) && Grid.IsCellBiomeSpaceBiome(cell);
	}

	// Token: 0x06004567 RID: 17767 RVA: 0x00191D86 File Offset: 0x0018FF86
	public static bool IsCellBiomeSpaceBiome(int cell)
	{
		return global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
	}

	// Token: 0x06004568 RID: 17768 RVA: 0x00191D9C File Offset: 0x0018FF9C
	public static int PosToCell(Vector2 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x06004569 RID: 17769 RVA: 0x00191DC8 File Offset: 0x0018FFC8
	public static int PosToCell(Vector3 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x0600456A RID: 17770 RVA: 0x00191DF4 File Offset: 0x0018FFF4
	public static void PosToXY(Vector3 pos, out int x, out int y)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out x, out y);
	}

	// Token: 0x0600456B RID: 17771 RVA: 0x00191E03 File Offset: 0x00190003
	public static void PosToXY(Vector3 pos, out Vector2I xy)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out xy.x, out xy.y);
	}

	// Token: 0x0600456C RID: 17772 RVA: 0x00191E1C File Offset: 0x0019001C
	public static Vector2I PosToXY(Vector3 pos)
	{
		Vector2I result;
		Grid.CellToXY(Grid.PosToCell(pos), out result.x, out result.y);
		return result;
	}

	// Token: 0x0600456D RID: 17773 RVA: 0x00191E43 File Offset: 0x00190043
	public static int XYToCell(int x, int y)
	{
		return x + y * Grid.WidthInCells;
	}

	// Token: 0x0600456E RID: 17774 RVA: 0x00191E4E File Offset: 0x0019004E
	public static void CellToXY(int cell, out int x, out int y)
	{
		x = Grid.CellColumn(cell);
		y = Grid.CellRow(cell);
	}

	// Token: 0x0600456F RID: 17775 RVA: 0x00191E60 File Offset: 0x00190060
	public static Vector2I CellToXY(int cell)
	{
		return new Vector2I(Grid.CellColumn(cell), Grid.CellRow(cell));
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x00191E74 File Offset: 0x00190074
	public static Vector3 CellToPos(int cell, float x_offset, float y_offset, float z_offset)
	{
		int widthInCells = Grid.WidthInCells;
		float num = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float num2 = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(num + x_offset, num2 + y_offset, z_offset);
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x00191EA8 File Offset: 0x001900A8
	public static Vector3 CellToPos(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(x, y, 0f);
	}

	// Token: 0x06004572 RID: 17778 RVA: 0x00191EDC File Offset: 0x001900DC
	public static Vector3 CellToPos2D(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector2(x, y);
	}

	// Token: 0x06004573 RID: 17779 RVA: 0x00191F0F File Offset: 0x0019010F
	public static int CellRow(int cell)
	{
		return cell / Grid.WidthInCells;
	}

	// Token: 0x06004574 RID: 17780 RVA: 0x00191F18 File Offset: 0x00190118
	public static int CellColumn(int cell)
	{
		return cell % Grid.WidthInCells;
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x00191F21 File Offset: 0x00190121
	public static int ClampX(int x)
	{
		return Math.Min(Math.Max(x, 0), Grid.WidthInCells - 1);
	}

	// Token: 0x06004576 RID: 17782 RVA: 0x00191F36 File Offset: 0x00190136
	public static int ClampY(int y)
	{
		return Math.Min(Math.Max(y, 0), Grid.HeightInCells - 1);
	}

	// Token: 0x06004577 RID: 17783 RVA: 0x00191F4C File Offset: 0x0019014C
	public static Vector2I Constrain(Vector2I val)
	{
		val.x = Mathf.Max(0, Mathf.Min(val.x, Grid.WidthInCells - 1));
		val.y = Mathf.Max(0, Mathf.Min(val.y, Grid.HeightInCells - 1));
		return val;
	}

	// Token: 0x06004578 RID: 17784 RVA: 0x00191F98 File Offset: 0x00190198
	public static void Reveal(int cell, byte visibility = 255, bool forceReveal = false)
	{
		bool flag = Grid.Spawnable[cell] == 0 && visibility > 0;
		Grid.Spawnable[cell] = Math.Max(visibility, Grid.Visible[cell]);
		if (forceReveal || !Grid.PreventFogOfWarReveal[cell])
		{
			Grid.Visible[cell] = Math.Max(visibility, Grid.Visible[cell]);
		}
		if (flag && Grid.OnReveal != null)
		{
			Grid.OnReveal(cell);
		}
	}

	// Token: 0x06004579 RID: 17785 RVA: 0x00192001 File Offset: 0x00190201
	public static ObjectLayer GetObjectLayerForConduitType(ConduitType conduit_type)
	{
		switch (conduit_type)
		{
		case ConduitType.Gas:
			return ObjectLayer.GasConduitConnection;
		case ConduitType.Liquid:
			return ObjectLayer.LiquidConduitConnection;
		case ConduitType.Solid:
			return ObjectLayer.SolidConduitConnection;
		default:
			throw new ArgumentException("Invalid value.", "conduit_type");
		}
	}

	// Token: 0x0600457A RID: 17786 RVA: 0x00192034 File Offset: 0x00190234
	public static Vector3 CellToPos(int cell, CellAlignment alignment, Grid.SceneLayer layer)
	{
		switch (alignment)
		{
		case CellAlignment.Bottom:
			return Grid.CellToPosCBC(cell, layer);
		case CellAlignment.Top:
			return Grid.CellToPosCTC(cell, layer);
		case CellAlignment.Left:
			return Grid.CellToPosLCC(cell, layer);
		case CellAlignment.Right:
			return Grid.CellToPosRCC(cell, layer);
		case CellAlignment.RandomInternal:
		{
			Vector3 b = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0f, 0f);
			return Grid.CellToPosCCC(cell, layer) + b;
		}
		}
		return Grid.CellToPosCCC(cell, layer);
	}

	// Token: 0x0600457B RID: 17787 RVA: 0x001920B6 File Offset: 0x001902B6
	public static float GetLayerZ(Grid.SceneLayer layer)
	{
		return -Grid.HalfCellSizeInMeters - Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier;
	}

	// Token: 0x0600457C RID: 17788 RVA: 0x001920CD File Offset: 0x001902CD
	public static Vector3 CellToPosCCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600457D RID: 17789 RVA: 0x001920E5 File Offset: 0x001902E5
	public static Vector3 CellToPosCBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600457E RID: 17790 RVA: 0x001920FD File Offset: 0x001902FD
	public static Vector3 CellToPosCCF(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, -Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier);
	}

	// Token: 0x0600457F RID: 17791 RVA: 0x0019211E File Offset: 0x0019031E
	public static Vector3 CellToPosLCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x06004580 RID: 17792 RVA: 0x00192136 File Offset: 0x00190336
	public static Vector3 CellToPosRCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x06004581 RID: 17793 RVA: 0x00192154 File Offset: 0x00190354
	public static Vector3 CellToPosRBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x06004582 RID: 17794 RVA: 0x00192172 File Offset: 0x00190372
	public static Vector3 CellToPosLBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, 0.01f, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x06004583 RID: 17795 RVA: 0x0019218A File Offset: 0x0019038A
	public static Vector3 CellToPosCTC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.CellSizeInMeters - 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x06004584 RID: 17796 RVA: 0x001921A8 File Offset: 0x001903A8
	public static bool IsSolidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	// Token: 0x06004585 RID: 17797 RVA: 0x001921C0 File Offset: 0x001903C0
	public unsafe static bool IsSubstantialLiquid(int cell, float threshold = 0.35f)
	{
		if (Grid.IsValidCell(cell))
		{
			ushort num = Grid.elementIdx[cell];
			if ((int)num < ElementLoader.elements.Count)
			{
				Element element = ElementLoader.elements[(int)num];
				if (element.IsLiquid && Grid.mass[cell] >= element.defaultValues.mass * threshold)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004586 RID: 17798 RVA: 0x00192220 File Offset: 0x00190420
	public static bool IsVisiblyInLiquid(Vector2 pos)
	{
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && Grid.IsLiquid(num))
		{
			int cell = Grid.CellAbove(num);
			if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
			{
				return true;
			}
			float num2 = Grid.Mass[num];
			float num3 = (float)((int)pos.y) - pos.y;
			if (num2 / 1000f <= num3)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x00192284 File Offset: 0x00190484
	public static bool IsNavigatableLiquid(int cell)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.IsSubstantialLiquid(cell, 0.35f))
		{
			return true;
		}
		if (Grid.IsLiquid(cell))
		{
			if (Grid.Element[num].IsLiquid)
			{
				return true;
			}
			if (Grid.Element[num].IsSolid)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004588 RID: 17800 RVA: 0x001922E2 File Offset: 0x001904E2
	public static bool IsLiquid(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsLiquid;
	}

	// Token: 0x06004589 RID: 17801 RVA: 0x00192303 File Offset: 0x00190503
	public static bool IsGas(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsGas;
	}

	// Token: 0x0600458A RID: 17802 RVA: 0x00192324 File Offset: 0x00190524
	public static void GetVisibleExtents(out int min_x, out int min_y, out int max_x, out int max_y)
	{
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		min_y = (int)vector2.y;
		max_y = (int)(vector.y + 0.5f);
		min_x = (int)vector2.x;
		max_x = (int)(vector.x + 0.5f);
	}

	// Token: 0x0600458B RID: 17803 RVA: 0x001923BD File Offset: 0x001905BD
	public static void GetVisibleExtents(out Vector2I min, out Vector2I max)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
	}

	// Token: 0x0600458C RID: 17804 RVA: 0x001923DC File Offset: 0x001905DC
	public static void GetVisibleCellRangeInActiveWorld(out Vector2I min, out Vector2I max, int padding = 4, float rangeScale = 1.5f)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
		min.x -= padding;
		min.y -= padding;
		if (CameraController.Instance != null && DlcManager.IsExpansion1Active())
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
			min.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, min.x));
			min.y = Math.Min(vector2I.y + vector2I2.y - 1, Math.Max(vector2I.y, min.y));
			max.x += padding;
			max.y += padding;
			max.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, max.x));
			max.y = Math.Min(vector2I.y + vector2I2.y - 1 + 20, Math.Max(vector2I.y, max.y));
			return;
		}
		min.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, min.x));
		min.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, min.y));
		max.x += padding;
		max.y += padding;
		max.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, max.x));
		max.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, max.y));
	}

	// Token: 0x0600458D RID: 17805 RVA: 0x001925A8 File Offset: 0x001907A8
	public static Extents GetVisibleExtentsInActiveWorld(int padding = 4, float rangeScale = 1.5f)
	{
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		return new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
	}

	// Token: 0x0600458E RID: 17806 RVA: 0x001925EF File Offset: 0x001907EF
	public static bool IsVisible(int cell)
	{
		return Grid.Visible[cell] > 0 || !PropertyTextures.IsFogOfWarEnabled;
	}

	// Token: 0x0600458F RID: 17807 RVA: 0x00192605 File Offset: 0x00190805
	public static bool VisibleBlockingCB(int cell)
	{
		return !Grid.Transparent[cell] && Grid.IsSolidCell(cell);
	}

	// Token: 0x06004590 RID: 17808 RVA: 0x0019261C File Offset: 0x0019081C
	public static bool VisibilityTest(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.TestLineOfSight(x, y, x2, y2, Grid.VisibleBlockingDelegate, blocking_tile_visible, false);
	}

	// Token: 0x06004591 RID: 17809 RVA: 0x00192630 File Offset: 0x00190830
	public static bool VisibilityTest(int cell, int target_cell, bool blocking_tile_visible = false)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		int x2 = 0;
		int y2 = 0;
		Grid.CellToXY(target_cell, out x2, out y2);
		return Grid.VisibilityTest(x, y, x2, y2, blocking_tile_visible);
	}

	// Token: 0x06004592 RID: 17810 RVA: 0x00192663 File Offset: 0x00190863
	public static bool PhysicalBlockingCB(int cell)
	{
		return Grid.Solid[cell];
	}

	// Token: 0x06004593 RID: 17811 RVA: 0x00192670 File Offset: 0x00190870
	public static bool IsPhysicallyAccessible(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.FastTestLineOfSightSolid(x, y, x2, y2);
	}

	// Token: 0x06004594 RID: 17812 RVA: 0x0019267C File Offset: 0x0019087C
	public static void CollectCellsInLine(int startCell, int endCell, HashSet<int> outputCells)
	{
		int num = 2;
		int cellDistance = Grid.GetCellDistance(startCell, endCell);
		Vector2 a = (Grid.CellToPos(endCell) - Grid.CellToPos(startCell)).normalized;
		for (float num2 = 0f; num2 < (float)cellDistance; num2 = Mathf.Min(num2 + 1f / (float)num, (float)cellDistance))
		{
			int num3 = Grid.PosToCell(Grid.CellToPos(startCell) + a * num2);
			if (Grid.GetCellDistance(startCell, num3) <= cellDistance)
			{
				outputCells.Add(num3);
			}
		}
	}

	// Token: 0x06004595 RID: 17813 RVA: 0x00192708 File Offset: 0x00190908
	public static bool IsRangeExposedToSunlight(int cell, int scanRadius, CellOffset scanShape, out int cellsClear, int clearThreshold = 1)
	{
		cellsClear = 0;
		if (Grid.IsValidCell(cell) && (int)Grid.ExposedToSunlight[cell] >= clearThreshold)
		{
			cellsClear++;
		}
		bool flag = true;
		bool flag2 = true;
		int num = 1;
		while (num <= scanRadius && (flag || flag2))
		{
			int num2 = Grid.OffsetCell(cell, scanShape.x * num, scanShape.y * num);
			int num3 = Grid.OffsetCell(cell, -scanShape.x * num, scanShape.y * num);
			if (Grid.IsValidCell(num2) && (int)Grid.ExposedToSunlight[num2] >= clearThreshold)
			{
				cellsClear++;
			}
			if (Grid.IsValidCell(num3) && (int)Grid.ExposedToSunlight[num3] >= clearThreshold)
			{
				cellsClear++;
			}
			num++;
		}
		return cellsClear > 0;
	}

	// Token: 0x06004596 RID: 17814 RVA: 0x001927BC File Offset: 0x001909BC
	public static int FindMidSkyCellAlignedWithCellInWorld(int cellToAlignWith, int worldID)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
		int cell = Grid.XYToCell(Grid.CellToXY(cellToAlignWith).x, world.WorldOffset.y + world.Height);
		int num = cellToAlignWith;
		int invalidCell = Grid.InvalidCell;
		int num2 = Grid.InvalidCell;
		while (num2 == Grid.InvalidCell && Grid.CellToXY(num).y < world.WorldOffset.y + world.Height)
		{
			if (Grid.IsCellBiomeSpaceBiome(num))
			{
				num2 = num;
				break;
			}
			num = Grid.CellAbove(num);
		}
		return Grid.XYToCell(Grid.CellToXY(cellToAlignWith).x, (int)((float)(Grid.CellToXY(cell).y + Grid.CellToXY(num2).y) * 0.5f));
	}

	// Token: 0x06004597 RID: 17815 RVA: 0x00192878 File Offset: 0x00190A78
	public static bool FastTestLineOfSightSolid(int x, int y, int x2, int y2)
	{
		int value = x2 - x;
		int num = y2 - y;
		int num2 = 0;
		int num4;
		int num3 = num4 = Math.Sign(value);
		int num5 = Math.Sign(num);
		int num6 = Math.Abs(value);
		int num7 = Math.Abs(num);
		if (num6 <= num7)
		{
			num6 = Math.Abs(num);
			num7 = Math.Abs(value);
			if (num < 0)
			{
				num2 = -1;
			}
			else if (num > 0)
			{
				num2 = 1;
			}
			num4 = 0;
		}
		int num8 = num6 >> 1;
		int num9 = num3 + num5 * Grid.WidthInCells;
		int num10 = num4 + num2 * Grid.WidthInCells;
		int num11 = Grid.XYToCell(x, y);
		for (int i = 1; i < num6; i++)
		{
			num8 += num7;
			if (num8 < num6)
			{
				num11 += num10;
			}
			else
			{
				num8 -= num6;
				num11 += num9;
			}
			if (Grid.Solid[num11])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004598 RID: 17816 RVA: 0x00192948 File Offset: 0x00190B48
	public static bool TestLineOfSightFixedBlockingVisible(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x06004599 RID: 17817 RVA: 0x00192A64 File Offset: 0x00190C64
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, Func<int, bool> blocking_tile_visible_cb, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible_cb(num12) && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x0600459A RID: 17818 RVA: 0x00192B8B File Offset: 0x00190D8B
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible = false, bool allow_invalid_cells = false)
	{
		return Grid.TestLineOfSightFixedBlockingVisible(x, y, x2, y2, blocking_cb, blocking_tile_visible, allow_invalid_cells);
	}

	// Token: 0x0600459B RID: 17819 RVA: 0x00192B9C File Offset: 0x00190D9C
	public static bool GetFreeGridSpace(Vector2I size, out Vector2I offset)
	{
		Vector2I gridOffset = BestFit.GetGridOffset(ClusterManager.Instance.WorldContainers, size, out offset);
		if (gridOffset.X <= Grid.WidthInCells && gridOffset.Y <= Grid.HeightInCells)
		{
			SimMessages.SimDataResizeGridAndInitializeVacuumCells(gridOffset, size.x, size.y, offset.x, offset.y);
			Game.Instance.roomProber.Refresh();
			return true;
		}
		return false;
	}

	// Token: 0x0600459C RID: 17820 RVA: 0x00192C08 File Offset: 0x00190E08
	public static void FreeGridSpace(Vector2I size, Vector2I offset)
	{
		SimMessages.SimDataFreeCells(size.x, size.y, offset.x, offset.y);
		for (int i = offset.y; i < size.y + offset.y + 1; i++)
		{
			for (int j = offset.x - 1; j < size.x + offset.x + 1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num))
				{
					Grid.Element[num] = ElementLoader.FindElementByHash(SimHashes.Vacuum);
				}
			}
		}
		Game.Instance.roomProber.Refresh();
	}

	// Token: 0x0600459D RID: 17821 RVA: 0x00192CA2 File Offset: 0x00190EA2
	[Conditional("UNITY_EDITOR")]
	public static void DrawBoxOnCell(int cell, Color color, float offset = 0f)
	{
		Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, 0f);
	}

	// Token: 0x04002EA0 RID: 11936
	public static readonly CellOffset[] DefaultOffset = new CellOffset[1];

	// Token: 0x04002EA1 RID: 11937
	public static float WidthInMeters;

	// Token: 0x04002EA2 RID: 11938
	public static float HeightInMeters;

	// Token: 0x04002EA3 RID: 11939
	public static int WidthInCells;

	// Token: 0x04002EA4 RID: 11940
	public static int HeightInCells;

	// Token: 0x04002EA5 RID: 11941
	public static float CellSizeInMeters;

	// Token: 0x04002EA6 RID: 11942
	public static float InverseCellSizeInMeters;

	// Token: 0x04002EA7 RID: 11943
	public static float HalfCellSizeInMeters;

	// Token: 0x04002EA8 RID: 11944
	public static int CellCount;

	// Token: 0x04002EA9 RID: 11945
	public static int InvalidCell = -1;

	// Token: 0x04002EAA RID: 11946
	public static int TopBorderHeight = 2;

	// Token: 0x04002EAB RID: 11947
	public static Dictionary<int, GameObject>[] ObjectLayers;

	// Token: 0x04002EAC RID: 11948
	public static Action<int> OnReveal;

	// Token: 0x04002EAD RID: 11949
	public static Vector3 OffWorldPosition = new Vector3(-1f, -1f, 0f);

	// Token: 0x04002EAE RID: 11950
	public static Grid.BuildFlags[] BuildMasks;

	// Token: 0x04002EAF RID: 11951
	public static Grid.BuildFlagsFoundationIndexer Foundation;

	// Token: 0x04002EB0 RID: 11952
	public static Grid.BuildFlagsSolidIndexer Solid;

	// Token: 0x04002EB1 RID: 11953
	public static Grid.BuildFlagsDupeImpassableIndexer DupeImpassable;

	// Token: 0x04002EB2 RID: 11954
	public static Grid.BuildFlagsFakeFloorIndexer FakeFloor;

	// Token: 0x04002EB3 RID: 11955
	public static Grid.BuildFlagsDupePassableIndexer DupePassable;

	// Token: 0x04002EB4 RID: 11956
	public static Grid.BuildFlagsImpassableIndexer CritterImpassable;

	// Token: 0x04002EB5 RID: 11957
	public static Grid.BuildFlagsDoorIndexer HasDoor;

	// Token: 0x04002EB6 RID: 11958
	public static Grid.VisFlags[] VisMasks;

	// Token: 0x04002EB7 RID: 11959
	public static Grid.VisFlagsRevealedIndexer Revealed;

	// Token: 0x04002EB8 RID: 11960
	public static Grid.VisFlagsPreventFogOfWarRevealIndexer PreventFogOfWarReveal;

	// Token: 0x04002EB9 RID: 11961
	public static Grid.VisFlagsRenderedByWorldIndexer RenderedByWorld;

	// Token: 0x04002EBA RID: 11962
	public static Grid.VisFlagsAllowPathfindingIndexer AllowPathfinding;

	// Token: 0x04002EBB RID: 11963
	public static Grid.NavValidatorFlags[] NavValidatorMasks;

	// Token: 0x04002EBC RID: 11964
	public static Grid.NavValidatorFlagsLadderIndexer HasLadder;

	// Token: 0x04002EBD RID: 11965
	public static Grid.NavValidatorFlagsPoleIndexer HasPole;

	// Token: 0x04002EBE RID: 11966
	public static Grid.NavValidatorFlagsTubeIndexer HasTube;

	// Token: 0x04002EBF RID: 11967
	public static Grid.NavValidatorFlagsNavTeleporterIndexer HasNavTeleporter;

	// Token: 0x04002EC0 RID: 11968
	public static Grid.NavValidatorFlagsUnderConstructionIndexer IsTileUnderConstruction;

	// Token: 0x04002EC1 RID: 11969
	public static Grid.NavFlags[] NavMasks;

	// Token: 0x04002EC2 RID: 11970
	private static Grid.NavFlagsAccessDoorIndexer HasAccessDoor;

	// Token: 0x04002EC3 RID: 11971
	public static Grid.NavFlagsTubeEntranceIndexer HasTubeEntrance;

	// Token: 0x04002EC4 RID: 11972
	public static Grid.NavFlagsPreventIdleTraversalIndexer PreventIdleTraversal;

	// Token: 0x04002EC5 RID: 11973
	public static Grid.NavFlagsReservedIndexer Reserved;

	// Token: 0x04002EC6 RID: 11974
	public static Grid.NavFlagsSuitMarkerIndexer HasSuitMarker;

	// Token: 0x04002EC7 RID: 11975
	private static ConcurrentDictionary<int, Grid.Restriction> restrictions = new ConcurrentDictionary<int, Grid.Restriction>();

	// Token: 0x04002EC8 RID: 11976
	private static ConcurrentDictionary<int, Grid.TubeEntrance> tubeEntrances = new ConcurrentDictionary<int, Grid.TubeEntrance>();

	// Token: 0x04002EC9 RID: 11977
	private static ConcurrentDictionary<int, Grid.SuitMarker> suitMarkers = new ConcurrentDictionary<int, Grid.SuitMarker>();

	// Token: 0x04002ECA RID: 11978
	public unsafe static ushort* elementIdx;

	// Token: 0x04002ECB RID: 11979
	public unsafe static float* temperature;

	// Token: 0x04002ECC RID: 11980
	public unsafe static float* radiation;

	// Token: 0x04002ECD RID: 11981
	public unsafe static float* mass;

	// Token: 0x04002ECE RID: 11982
	public unsafe static byte* properties;

	// Token: 0x04002ECF RID: 11983
	public unsafe static byte* strengthInfo;

	// Token: 0x04002ED0 RID: 11984
	public unsafe static byte* insulation;

	// Token: 0x04002ED1 RID: 11985
	public unsafe static byte* diseaseIdx;

	// Token: 0x04002ED2 RID: 11986
	public unsafe static int* diseaseCount;

	// Token: 0x04002ED3 RID: 11987
	public unsafe static byte* exposedToSunlight;

	// Token: 0x04002ED4 RID: 11988
	public unsafe static float* AccumulatedFlowValues = null;

	// Token: 0x04002ED5 RID: 11989
	public static byte[] Visible;

	// Token: 0x04002ED6 RID: 11990
	public static byte[] Spawnable;

	// Token: 0x04002ED7 RID: 11991
	public static float[] Damage;

	// Token: 0x04002ED8 RID: 11992
	public static float[] Decor;

	// Token: 0x04002ED9 RID: 11993
	public static bool[] GravitasFacility;

	// Token: 0x04002EDA RID: 11994
	public static byte[] WorldIdx;

	// Token: 0x04002EDB RID: 11995
	public static float[] Loudness;

	// Token: 0x04002EDC RID: 11996
	public static Element[] Element;

	// Token: 0x04002EDD RID: 11997
	public static int[] LightCount;

	// Token: 0x04002EDE RID: 11998
	public static Grid.PressureIndexer Pressure;

	// Token: 0x04002EDF RID: 11999
	public static Grid.LiquidImpermeableIndexer LiquidImpermeable;

	// Token: 0x04002EE0 RID: 12000
	public static Grid.TransparentIndexer Transparent;

	// Token: 0x04002EE1 RID: 12001
	public static Grid.ElementIdxIndexer ElementIdx;

	// Token: 0x04002EE2 RID: 12002
	public static Grid.TemperatureIndexer Temperature;

	// Token: 0x04002EE3 RID: 12003
	public static Grid.RadiationIndexer Radiation;

	// Token: 0x04002EE4 RID: 12004
	public static Grid.MassIndexer Mass;

	// Token: 0x04002EE5 RID: 12005
	public static Grid.PropertiesIndexer Properties;

	// Token: 0x04002EE6 RID: 12006
	public static Grid.ExposedToSunlightIndexer ExposedToSunlight;

	// Token: 0x04002EE7 RID: 12007
	public static Grid.StrengthInfoIndexer StrengthInfo;

	// Token: 0x04002EE8 RID: 12008
	public static Grid.Insulationndexer Insulation;

	// Token: 0x04002EE9 RID: 12009
	public static Grid.DiseaseIdxIndexer DiseaseIdx;

	// Token: 0x04002EEA RID: 12010
	public static Grid.DiseaseCountIndexer DiseaseCount;

	// Token: 0x04002EEB RID: 12011
	public static Grid.LightIntensityIndexer LightIntensity;

	// Token: 0x04002EEC RID: 12012
	public static Grid.AccumulatedFlowIndexer AccumulatedFlow;

	// Token: 0x04002EED RID: 12013
	public static Grid.ObjectLayerIndexer Objects;

	// Token: 0x04002EEE RID: 12014
	public static float LayerMultiplier = 1f;

	// Token: 0x04002EEF RID: 12015
	private static readonly Func<int, bool> VisibleBlockingDelegate = (int cell) => Grid.VisibleBlockingCB(cell);

	// Token: 0x04002EF0 RID: 12016
	private static readonly Func<int, bool> PhysicalBlockingDelegate = (int cell) => Grid.PhysicalBlockingCB(cell);

	// Token: 0x020019BB RID: 6587
	[Flags]
	public enum BuildFlags : byte
	{
		// Token: 0x04007F3D RID: 32573
		Solid = 1,
		// Token: 0x04007F3E RID: 32574
		Foundation = 2,
		// Token: 0x04007F3F RID: 32575
		Door = 4,
		// Token: 0x04007F40 RID: 32576
		DupePassable = 8,
		// Token: 0x04007F41 RID: 32577
		DupeImpassable = 16,
		// Token: 0x04007F42 RID: 32578
		CritterImpassable = 32,
		// Token: 0x04007F43 RID: 32579
		FakeFloor = 192,
		// Token: 0x04007F44 RID: 32580
		Any = 255
	}

	// Token: 0x020019BC RID: 6588
	public struct BuildFlagsFoundationIndexer
	{
		// Token: 0x17000B32 RID: 2866
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Foundation) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Foundation, value);
			}
		}
	}

	// Token: 0x020019BD RID: 6589
	public struct BuildFlagsSolidIndexer
	{
		// Token: 0x17000B33 RID: 2867
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Solid) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}
	}

	// Token: 0x020019BE RID: 6590
	public struct BuildFlagsDupeImpassableIndexer
	{
		// Token: 0x17000B34 RID: 2868
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupeImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupeImpassable, value);
			}
		}
	}

	// Token: 0x020019BF RID: 6591
	public struct BuildFlagsFakeFloorIndexer
	{
		// Token: 0x17000B35 RID: 2869
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.FakeFloor) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}

		// Token: 0x0600A30F RID: 41743 RVA: 0x003B12A0 File Offset: 0x003AF4A0
		public void Add(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) + 1);
			num = Math.Min(num, 3);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}

		// Token: 0x0600A310 RID: 41744 RVA: 0x003B12E0 File Offset: 0x003AF4E0
		public void Remove(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) - Grid.BuildFlags.Solid);
			num = Math.Max(num, 0);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}
	}

	// Token: 0x020019C0 RID: 6592
	public struct BuildFlagsDupePassableIndexer
	{
		// Token: 0x17000B36 RID: 2870
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupePassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupePassable, value);
			}
		}
	}

	// Token: 0x020019C1 RID: 6593
	public struct BuildFlagsImpassableIndexer
	{
		// Token: 0x17000B37 RID: 2871
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.CritterImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.CritterImpassable, value);
			}
		}
	}

	// Token: 0x020019C2 RID: 6594
	public struct BuildFlagsDoorIndexer
	{
		// Token: 0x17000B38 RID: 2872
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Door) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Door, value);
			}
		}
	}

	// Token: 0x020019C3 RID: 6595
	[Flags]
	public enum VisFlags : byte
	{
		// Token: 0x04007F46 RID: 32582
		Revealed = 1,
		// Token: 0x04007F47 RID: 32583
		PreventFogOfWarReveal = 2,
		// Token: 0x04007F48 RID: 32584
		RenderedByWorld = 4,
		// Token: 0x04007F49 RID: 32585
		AllowPathfinding = 8
	}

	// Token: 0x020019C4 RID: 6596
	public struct VisFlagsRevealedIndexer
	{
		// Token: 0x17000B39 RID: 2873
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.Revealed) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.Revealed, value);
			}
		}
	}

	// Token: 0x020019C5 RID: 6597
	public struct VisFlagsPreventFogOfWarRevealIndexer
	{
		// Token: 0x17000B3A RID: 2874
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.PreventFogOfWarReveal) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.PreventFogOfWarReveal, value);
			}
		}
	}

	// Token: 0x020019C6 RID: 6598
	public struct VisFlagsRenderedByWorldIndexer
	{
		// Token: 0x17000B3B RID: 2875
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.RenderedByWorld) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.RenderedByWorld, value);
			}
		}
	}

	// Token: 0x020019C7 RID: 6599
	public struct VisFlagsAllowPathfindingIndexer
	{
		// Token: 0x17000B3C RID: 2876
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.AllowPathfinding) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.AllowPathfinding, value);
			}
		}
	}

	// Token: 0x020019C8 RID: 6600
	[Flags]
	public enum NavValidatorFlags : byte
	{
		// Token: 0x04007F4B RID: 32587
		Ladder = 1,
		// Token: 0x04007F4C RID: 32588
		Pole = 2,
		// Token: 0x04007F4D RID: 32589
		Tube = 4,
		// Token: 0x04007F4E RID: 32590
		NavTeleporter = 8,
		// Token: 0x04007F4F RID: 32591
		UnderConstruction = 16
	}

	// Token: 0x020019C9 RID: 6601
	public struct NavValidatorFlagsLadderIndexer
	{
		// Token: 0x17000B3D RID: 2877
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Ladder) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Ladder, value);
			}
		}
	}

	// Token: 0x020019CA RID: 6602
	public struct NavValidatorFlagsPoleIndexer
	{
		// Token: 0x17000B3E RID: 2878
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Pole) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Pole, value);
			}
		}
	}

	// Token: 0x020019CB RID: 6603
	public struct NavValidatorFlagsTubeIndexer
	{
		// Token: 0x17000B3F RID: 2879
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Tube) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Tube, value);
			}
		}
	}

	// Token: 0x020019CC RID: 6604
	public struct NavValidatorFlagsNavTeleporterIndexer
	{
		// Token: 0x17000B40 RID: 2880
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.NavTeleporter) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.NavTeleporter, value);
			}
		}
	}

	// Token: 0x020019CD RID: 6605
	public struct NavValidatorFlagsUnderConstructionIndexer
	{
		// Token: 0x17000B41 RID: 2881
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.UnderConstruction) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.UnderConstruction, value);
			}
		}
	}

	// Token: 0x020019CE RID: 6606
	[Flags]
	public enum NavFlags : byte
	{
		// Token: 0x04007F51 RID: 32593
		AccessDoor = 1,
		// Token: 0x04007F52 RID: 32594
		TubeEntrance = 2,
		// Token: 0x04007F53 RID: 32595
		PreventIdleTraversal = 4,
		// Token: 0x04007F54 RID: 32596
		Reserved = 8,
		// Token: 0x04007F55 RID: 32597
		SuitMarker = 16
	}

	// Token: 0x020019CF RID: 6607
	public struct NavFlagsAccessDoorIndexer
	{
		// Token: 0x17000B42 RID: 2882
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.AccessDoor) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.AccessDoor, value);
			}
		}
	}

	// Token: 0x020019D0 RID: 6608
	public struct NavFlagsTubeEntranceIndexer
	{
		// Token: 0x17000B43 RID: 2883
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.TubeEntrance) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.TubeEntrance, value);
			}
		}
	}

	// Token: 0x020019D1 RID: 6609
	public struct NavFlagsPreventIdleTraversalIndexer
	{
		// Token: 0x17000B44 RID: 2884
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.PreventIdleTraversal) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.PreventIdleTraversal, value);
			}
		}
	}

	// Token: 0x020019D2 RID: 6610
	public struct NavFlagsReservedIndexer
	{
		// Token: 0x17000B45 RID: 2885
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.Reserved) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.Reserved, value);
			}
		}
	}

	// Token: 0x020019D3 RID: 6611
	public struct NavFlagsSuitMarkerIndexer
	{
		// Token: 0x17000B46 RID: 2886
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.SuitMarker) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.SuitMarker, value);
			}
		}
	}

	// Token: 0x020019D4 RID: 6612
	public struct Restriction
	{
		// Token: 0x04007F56 RID: 32598
		public Dictionary<int, Grid.Restriction.Directions> DirectionMasksForMinionInstanceID;

		// Token: 0x04007F57 RID: 32599
		public Grid.Restriction.Orientation orientation;

		// Token: 0x020029BA RID: 10682
		[Flags]
		public enum Directions : byte
		{
			// Token: 0x0400B888 RID: 47240
			Left = 1,
			// Token: 0x0400B889 RID: 47241
			Right = 2,
			// Token: 0x0400B88A RID: 47242
			Teleport = 4
		}

		// Token: 0x020029BB RID: 10683
		public enum Orientation : byte
		{
			// Token: 0x0400B88C RID: 47244
			Vertical,
			// Token: 0x0400B88D RID: 47245
			Horizontal,
			// Token: 0x0400B88E RID: 47246
			SingleCell
		}
	}

	// Token: 0x020019D5 RID: 6613
	private struct TubeEntrance
	{
		// Token: 0x04007F58 RID: 32600
		public bool operational;

		// Token: 0x04007F59 RID: 32601
		public int reservationCapacity;

		// Token: 0x04007F5A RID: 32602
		public HashSet<int> reservedInstanceIDs;
	}

	// Token: 0x020019D6 RID: 6614
	public struct SuitMarker
	{
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x0600A333 RID: 41779 RVA: 0x003B14BD File Offset: 0x003AF6BD
		public int emptyLockerCount
		{
			get
			{
				return this.lockerCount - this.suitCount;
			}
		}

		// Token: 0x04007F5B RID: 32603
		public int suitCount;

		// Token: 0x04007F5C RID: 32604
		public int lockerCount;

		// Token: 0x04007F5D RID: 32605
		public Grid.SuitMarker.Flags flags;

		// Token: 0x04007F5E RID: 32606
		public PathFinder.PotentialPath.Flags pathFlags;

		// Token: 0x04007F5F RID: 32607
		public HashSet<int> minionIDsWithSuitReservations;

		// Token: 0x04007F60 RID: 32608
		public HashSet<int> minionIDsWithEmptyLockerReservations;

		// Token: 0x020029BC RID: 10684
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x0400B890 RID: 47248
			OnlyTraverseIfUnequipAvailable = 1,
			// Token: 0x0400B891 RID: 47249
			Operational = 2,
			// Token: 0x0400B892 RID: 47250
			Rotated = 4
		}
	}

	// Token: 0x020019D7 RID: 6615
	public struct ObjectLayerIndexer
	{
		// Token: 0x17000B48 RID: 2888
		public GameObject this[int cell, int layer]
		{
			get
			{
				GameObject result = null;
				Grid.ObjectLayers[layer].TryGetValue(cell, out result);
				return result;
			}
			set
			{
				if (value == null)
				{
					Grid.ObjectLayers[layer].Remove(cell);
				}
				else
				{
					Grid.ObjectLayers[layer][cell] = value;
				}
				GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.objectLayers[layer], value);
			}
		}
	}

	// Token: 0x020019D8 RID: 6616
	public struct PressureIndexer
	{
		// Token: 0x17000B49 RID: 2889
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i] * 101.3f;
			}
		}
	}

	// Token: 0x020019D9 RID: 6617
	public struct LiquidImpermeableIndexer
	{
		// Token: 0x17000B4A RID: 2890
		public unsafe bool this[int i]
		{
			get
			{
				return (Grid.properties[i] & 2) > 0;
			}
		}
	}

	// Token: 0x020019DA RID: 6618
	public struct TransparentIndexer
	{
		// Token: 0x17000B4B RID: 2891
		public unsafe bool this[int i]
		{
			get
			{
				return (Grid.properties[i] & 16) > 0;
			}
		}
	}

	// Token: 0x020019DB RID: 6619
	public struct ElementIdxIndexer
	{
		// Token: 0x17000B4C RID: 2892
		public unsafe ushort this[int i]
		{
			get
			{
				return Grid.elementIdx[i];
			}
		}
	}

	// Token: 0x020019DC RID: 6620
	public struct TemperatureIndexer
	{
		// Token: 0x17000B4D RID: 2893
		public unsafe float this[int i]
		{
			get
			{
				return Grid.temperature[i];
			}
		}
	}

	// Token: 0x020019DD RID: 6621
	public struct RadiationIndexer
	{
		// Token: 0x17000B4E RID: 2894
		public unsafe float this[int i]
		{
			get
			{
				return Grid.radiation[i];
			}
		}
	}

	// Token: 0x020019DE RID: 6622
	public struct MassIndexer
	{
		// Token: 0x17000B4F RID: 2895
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i];
			}
		}
	}

	// Token: 0x020019DF RID: 6623
	public struct PropertiesIndexer
	{
		// Token: 0x17000B50 RID: 2896
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.properties[i];
			}
		}
	}

	// Token: 0x020019E0 RID: 6624
	public struct ExposedToSunlightIndexer
	{
		// Token: 0x17000B51 RID: 2897
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.exposedToSunlight[i];
			}
		}
	}

	// Token: 0x020019E1 RID: 6625
	public struct StrengthInfoIndexer
	{
		// Token: 0x17000B52 RID: 2898
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.strengthInfo[i];
			}
		}
	}

	// Token: 0x020019E2 RID: 6626
	public struct Insulationndexer
	{
		// Token: 0x17000B53 RID: 2899
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.insulation[i];
			}
		}
	}

	// Token: 0x020019E3 RID: 6627
	public struct DiseaseIdxIndexer
	{
		// Token: 0x17000B54 RID: 2900
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.diseaseIdx[i];
			}
		}
	}

	// Token: 0x020019E4 RID: 6628
	public struct DiseaseCountIndexer
	{
		// Token: 0x17000B55 RID: 2901
		public unsafe int this[int i]
		{
			get
			{
				return Grid.diseaseCount[i];
			}
		}
	}

	// Token: 0x020019E5 RID: 6629
	public struct AccumulatedFlowIndexer
	{
		// Token: 0x17000B56 RID: 2902
		public unsafe float this[int i]
		{
			get
			{
				return Grid.AccumulatedFlowValues[i];
			}
		}
	}

	// Token: 0x020019E6 RID: 6630
	public struct LightIntensityIndexer
	{
		// Token: 0x17000B57 RID: 2903
		public unsafe int this[int i]
		{
			get
			{
				float num = Game.Instance.currentFallbackSunlightIntensity;
				WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[i]);
				if (world != null)
				{
					num = world.currentSunlightIntensity;
				}
				int num2 = (int)((float)Grid.exposedToSunlight[i] / 255f * num);
				int num3 = Grid.LightCount[i];
				return num2 + num3;
			}
		}
	}

	// Token: 0x020019E7 RID: 6631
	public enum SceneLayer
	{
		// Token: 0x04007F62 RID: 32610
		WorldSelection = -3,
		// Token: 0x04007F63 RID: 32611
		NoLayer,
		// Token: 0x04007F64 RID: 32612
		Background,
		// Token: 0x04007F65 RID: 32613
		Backwall = 1,
		// Token: 0x04007F66 RID: 32614
		Gas,
		// Token: 0x04007F67 RID: 32615
		GasConduits,
		// Token: 0x04007F68 RID: 32616
		GasConduitBridges,
		// Token: 0x04007F69 RID: 32617
		LiquidConduits,
		// Token: 0x04007F6A RID: 32618
		LiquidConduitBridges,
		// Token: 0x04007F6B RID: 32619
		SolidConduits,
		// Token: 0x04007F6C RID: 32620
		SolidConduitContents,
		// Token: 0x04007F6D RID: 32621
		SolidConduitBridges,
		// Token: 0x04007F6E RID: 32622
		Wires,
		// Token: 0x04007F6F RID: 32623
		WireBridges,
		// Token: 0x04007F70 RID: 32624
		WireBridgesFront,
		// Token: 0x04007F71 RID: 32625
		LogicWires,
		// Token: 0x04007F72 RID: 32626
		LogicGates,
		// Token: 0x04007F73 RID: 32627
		LogicGatesFront,
		// Token: 0x04007F74 RID: 32628
		InteriorWall,
		// Token: 0x04007F75 RID: 32629
		GasFront,
		// Token: 0x04007F76 RID: 32630
		BuildingBack,
		// Token: 0x04007F77 RID: 32631
		Building,
		// Token: 0x04007F78 RID: 32632
		BuildingUse,
		// Token: 0x04007F79 RID: 32633
		BuildingFront,
		// Token: 0x04007F7A RID: 32634
		TransferArm,
		// Token: 0x04007F7B RID: 32635
		Ore,
		// Token: 0x04007F7C RID: 32636
		Creatures,
		// Token: 0x04007F7D RID: 32637
		Move,
		// Token: 0x04007F7E RID: 32638
		Front,
		// Token: 0x04007F7F RID: 32639
		GlassTile,
		// Token: 0x04007F80 RID: 32640
		Liquid,
		// Token: 0x04007F81 RID: 32641
		Ground,
		// Token: 0x04007F82 RID: 32642
		TileMain,
		// Token: 0x04007F83 RID: 32643
		TileFront,
		// Token: 0x04007F84 RID: 32644
		FXFront,
		// Token: 0x04007F85 RID: 32645
		FXFront2,
		// Token: 0x04007F86 RID: 32646
		SceneMAX
	}
}
