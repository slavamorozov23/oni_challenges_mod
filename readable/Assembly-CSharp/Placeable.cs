using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200061F RID: 1567
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Placeable")]
public class Placeable : KMonoBehaviour
{
	// Token: 0x06002546 RID: 9542 RVA: 0x000D5EEC File Offset: 0x000D40EC
	public bool IsValidPlaceLocation(int cell, out string reason)
	{
		if (this.placementRules.Contains(Placeable.PlacementRules.RestrictToWorld) && (int)Grid.WorldIdx[cell] != this.restrictWorldId)
		{
			reason = UI.TOOLS.PLACE.REASONS.RESTRICT_TO_WORLD;
			return false;
		}
		if (!this.occupyArea.CanOccupyArea(cell, this.occupyArea.objectLayers[0]))
		{
			reason = UI.TOOLS.PLACE.REASONS.CAN_OCCUPY_AREA;
			return false;
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.OnFoundation))
		{
			bool flag = this.occupyArea.TestAreaBelow(cell, null, new Func<int, object, bool>(this.FoundationTest));
			if (this.checkRootCellOnly)
			{
				flag = this.FoundationTest(Grid.CellBelow(cell), null);
			}
			if (!flag)
			{
				reason = UI.TOOLS.PLACE.REASONS.ON_FOUNDATION;
				return false;
			}
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.VisibleToSpace))
		{
			bool flag2 = this.occupyArea.TestArea(cell, null, new Func<int, object, bool>(this.SunnySpaceTest));
			if (this.checkRootCellOnly)
			{
				flag2 = this.SunnySpaceTest(cell, null);
			}
			if (!flag2)
			{
				reason = UI.TOOLS.PLACE.REASONS.VISIBLE_TO_SPACE;
				return false;
			}
		}
		reason = "ok!";
		return true;
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x000D5FF0 File Offset: 0x000D41F0
	private bool SunnySpaceTest(int cell, object data)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int x;
		int startY;
		Grid.CellToXY(cell, out x, out startY);
		int num = (int)Grid.WorldIdx[cell];
		if (num == 255)
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(num);
		int top = world.WorldOffset.y + world.WorldSize.y;
		return !Grid.Solid[cell] && !Grid.Foundation[cell] && (Grid.ExposedToSunlight[cell] >= 253 || this.ClearPathToSky(x, startY, top));
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x000D6084 File Offset: 0x000D4284
	private bool ClearPathToSky(int x, int startY, int top)
	{
		for (int i = startY; i < top; i++)
		{
			int i2 = Grid.XYToCell(x, i);
			if (Grid.Solid[i2] || Grid.Foundation[i2])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x000D60C2 File Offset: 0x000D42C2
	private bool FoundationTest(int cell, object data)
	{
		return Grid.IsValidBuildingCell(cell) && (Grid.Solid[cell] || Grid.Foundation[cell]);
	}

	// Token: 0x040015CA RID: 5578
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x040015CB RID: 5579
	public string kAnimName;

	// Token: 0x040015CC RID: 5580
	public string animName;

	// Token: 0x040015CD RID: 5581
	public List<Placeable.PlacementRules> placementRules = new List<Placeable.PlacementRules>();

	// Token: 0x040015CE RID: 5582
	[NonSerialized]
	public int restrictWorldId;

	// Token: 0x040015CF RID: 5583
	public bool checkRootCellOnly;

	// Token: 0x020014F4 RID: 5364
	public enum PlacementRules
	{
		// Token: 0x04007011 RID: 28689
		OnFoundation,
		// Token: 0x04007012 RID: 28690
		VisibleToSpace,
		// Token: 0x04007013 RID: 28691
		RestrictToWorld
	}
}
