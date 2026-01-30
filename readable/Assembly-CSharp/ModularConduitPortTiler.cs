using System;
using UnityEngine;

// Token: 0x020007C7 RID: 1991
public class ModularConduitPortTiler : KMonoBehaviour
{
	// Token: 0x060034AD RID: 13485 RVA: 0x0012ACB4 File Offset: 0x00128EB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.ModularConduitPort, true);
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				GameTags.ModularConduitPort
			};
		}
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x0012AD04 File Offset: 0x00128F04
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.leftCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.leftCapDefaultSceneLayerAdjust), ModularConduitPortTiler.leftCapDefaultStr);
		if (this.manageLeftCap)
		{
			this.leftCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.leftCapLaunchpadStr);
			this.leftCapConduit = new KAnimSynchronizedController(component2, component2.GetLayer() + Grid.SceneLayer.Backwall, ModularConduitPortTiler.leftCapConduitStr);
		}
		this.rightCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.rightCapDefaultSceneLayerAdjust), ModularConduitPortTiler.rightCapDefaultStr);
		if (this.manageRightCap)
		{
			this.rightCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapLaunchpadStr);
			this.rightCapConduit = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapConduitStr);
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y, this.extents.width + 2, this.extents.height);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ModularConduitPort.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		this.UpdateEndCaps();
		this.CorrectAdjacentLaunchPads();
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x0012AE5A File Offset: 0x0012905A
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0012AE74 File Offset: 0x00129074
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellLeft = this.GetCellLeft();
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellLeft))
		{
			if (this.HasTileableNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (Grid.IsValidCell(cellRight))
		{
			if (this.HasTileableNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (this.manageLeftCap)
		{
			this.leftCapDefault.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.leftCapConduit.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.leftCapLaunchpad.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
		if (this.manageRightCap)
		{
			this.rightCapDefault.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.rightCapConduit.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.rightCapLaunchpad.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x0012AF8C File Offset: 0x0012918C
	private int GetCellLeft()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num - 1, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x0012AFC8 File Offset: 0x001291C8
	private int GetCellRight()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num + this.extents.width, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x0012B00C File Offset: 0x0012920C
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

	// Token: 0x060034B4 RID: 13492 RVA: 0x0012B058 File Offset: 0x00129258
	private bool HasLaunchpadNeighbour(int neighbour_cell)
	{
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x0012B091 File Offset: 0x00129291
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

	// Token: 0x060034B6 RID: 13494 RVA: 0x0012B0C0 File Offset: 0x001292C0
	private void CorrectAdjacentLaunchPads()
	{
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellRight) && this.HasLaunchpadNeighbour(cellRight))
		{
			Grid.Objects[cellRight, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
		int cellLeft = this.GetCellLeft();
		if (Grid.IsValidCell(cellLeft) && this.HasLaunchpadNeighbour(cellLeft))
		{
			Grid.Objects[cellLeft, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
	}

	// Token: 0x04001FD3 RID: 8147
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001FD4 RID: 8148
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04001FD5 RID: 8149
	public Tag[] tags;

	// Token: 0x04001FD6 RID: 8150
	public bool manageLeftCap = true;

	// Token: 0x04001FD7 RID: 8151
	public bool manageRightCap = true;

	// Token: 0x04001FD8 RID: 8152
	public int leftCapDefaultSceneLayerAdjust;

	// Token: 0x04001FD9 RID: 8153
	public int rightCapDefaultSceneLayerAdjust;

	// Token: 0x04001FDA RID: 8154
	private Extents extents;

	// Token: 0x04001FDB RID: 8155
	private ModularConduitPortTiler.AnimCapType leftCapSetting;

	// Token: 0x04001FDC RID: 8156
	private ModularConduitPortTiler.AnimCapType rightCapSetting;

	// Token: 0x04001FDD RID: 8157
	private static readonly string leftCapDefaultStr = "#cap_left_default";

	// Token: 0x04001FDE RID: 8158
	private static readonly string leftCapLaunchpadStr = "#cap_left_launchpad";

	// Token: 0x04001FDF RID: 8159
	private static readonly string leftCapConduitStr = "#cap_left_conduit";

	// Token: 0x04001FE0 RID: 8160
	private static readonly string rightCapDefaultStr = "#cap_right_default";

	// Token: 0x04001FE1 RID: 8161
	private static readonly string rightCapLaunchpadStr = "#cap_right_launchpad";

	// Token: 0x04001FE2 RID: 8162
	private static readonly string rightCapConduitStr = "#cap_right_conduit";

	// Token: 0x04001FE3 RID: 8163
	private KAnimSynchronizedController leftCapDefault;

	// Token: 0x04001FE4 RID: 8164
	private KAnimSynchronizedController leftCapLaunchpad;

	// Token: 0x04001FE5 RID: 8165
	private KAnimSynchronizedController leftCapConduit;

	// Token: 0x04001FE6 RID: 8166
	private KAnimSynchronizedController rightCapDefault;

	// Token: 0x04001FE7 RID: 8167
	private KAnimSynchronizedController rightCapLaunchpad;

	// Token: 0x04001FE8 RID: 8168
	private KAnimSynchronizedController rightCapConduit;

	// Token: 0x02001713 RID: 5907
	private enum AnimCapType
	{
		// Token: 0x040076D8 RID: 30424
		Default,
		// Token: 0x040076D9 RID: 30425
		Conduit,
		// Token: 0x040076DA RID: 30426
		Launchpad
	}
}
