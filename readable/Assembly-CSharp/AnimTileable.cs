using System;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/AnimTileable")]
public class AnimTileable : KMonoBehaviour
{
	// Token: 0x06002AAF RID: 10927 RVA: 0x000F9D43 File Offset: 0x000F7F43
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				base.GetComponent<KPrefabID>().PrefabTag
			};
		}
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x000F9D7C File Offset: 0x000F7F7C
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		else
		{
			Building component2 = base.GetComponent<Building>();
			this.extents = component2.GetExtents();
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		this.UpdateEndCaps();
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x000F9E34 File Offset: 0x000F8034
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x000F9E4C File Offset: 0x000F804C
	private void UpdateEndCaps()
	{
		int cell = Grid.PosToCell(this);
		bool is_visible = true;
		bool is_visible2 = true;
		bool is_visible3 = true;
		bool is_visible4 = true;
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset rotatedCellOffset = new CellOffset(this.extents.x - num - 1, 0);
		CellOffset rotatedCellOffset2 = new CellOffset(this.extents.x - num + this.extents.width, 0);
		CellOffset rotatedCellOffset3 = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		CellOffset rotatedCellOffset4 = new CellOffset(0, this.extents.y - num2 - 1);
		Rotatable component = base.GetComponent<Rotatable>();
		if (component)
		{
			rotatedCellOffset = component.GetRotatedCellOffset(rotatedCellOffset);
			rotatedCellOffset2 = component.GetRotatedCellOffset(rotatedCellOffset2);
			rotatedCellOffset3 = component.GetRotatedCellOffset(rotatedCellOffset3);
			rotatedCellOffset4 = component.GetRotatedCellOffset(rotatedCellOffset4);
		}
		int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num4 = Grid.OffsetCell(cell, rotatedCellOffset2);
		int num5 = Grid.OffsetCell(cell, rotatedCellOffset3);
		int num6 = Grid.OffsetCell(cell, rotatedCellOffset4);
		if (Grid.IsValidCell(num3))
		{
			is_visible = !this.HasTileableNeighbour(num3);
		}
		if (Grid.IsValidCell(num4))
		{
			is_visible2 = !this.HasTileableNeighbour(num4);
		}
		if (Grid.IsValidCell(num5))
		{
			is_visible3 = !this.HasTileableNeighbour(num5);
		}
		if (Grid.IsValidCell(num6))
		{
			is_visible4 = !this.HasTileableNeighbour(num6);
		}
		foreach (KBatchedAnimController kbatchedAnimController in base.GetComponentsInChildren<KBatchedAnimController>())
		{
			foreach (KAnimHashedString symbol in AnimTileable.leftSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol, is_visible);
			}
			foreach (KAnimHashedString symbol2 in AnimTileable.rightSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol2, is_visible2);
			}
			foreach (KAnimHashedString symbol3 in AnimTileable.topSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol3, is_visible3);
			}
			foreach (KAnimHashedString symbol4 in AnimTileable.bottomSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol4, is_visible4);
			}
		}
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000FA084 File Offset: 0x000F8284
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000FA0CF File Offset: 0x000F82CF
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x0400196C RID: 6508
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400196D RID: 6509
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x0400196E RID: 6510
	public Tag[] tags;

	// Token: 0x0400196F RID: 6511
	private Extents extents;

	// Token: 0x04001970 RID: 6512
	private static readonly KAnimHashedString[] leftSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_left"),
		new KAnimHashedString("cap_left_fg"),
		new KAnimHashedString("cap_left_place")
	};

	// Token: 0x04001971 RID: 6513
	private static readonly KAnimHashedString[] rightSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_right"),
		new KAnimHashedString("cap_right_fg"),
		new KAnimHashedString("cap_right_place")
	};

	// Token: 0x04001972 RID: 6514
	private static readonly KAnimHashedString[] topSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_top"),
		new KAnimHashedString("cap_top_fg"),
		new KAnimHashedString("cap_top_place")
	};

	// Token: 0x04001973 RID: 6515
	private static readonly KAnimHashedString[] bottomSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_bottom"),
		new KAnimHashedString("cap_bottom_fg"),
		new KAnimHashedString("cap_bottom_place")
	};
}
