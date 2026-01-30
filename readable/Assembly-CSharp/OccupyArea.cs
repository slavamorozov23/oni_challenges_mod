using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000A76 RID: 2678
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/OccupyArea")]
public class OccupyArea : KMonoBehaviour
{
	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06004DDB RID: 19931 RVA: 0x001C4075 File Offset: 0x001C2275
	public CellOffset[] OccupiedCellsOffsets
	{
		get
		{
			this.UpdateRotatedCells();
			return this._RotatedOccupiedCellsOffsets;
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x06004DDC RID: 19932 RVA: 0x001C4083 File Offset: 0x001C2283
	// (set) Token: 0x06004DDD RID: 19933 RVA: 0x001C408B File Offset: 0x001C228B
	public bool ApplyToCells
	{
		get
		{
			return this.applyToCells;
		}
		set
		{
			if (value != this.applyToCells)
			{
				if (value)
				{
					this.UpdateOccupiedArea();
				}
				else
				{
					this.ClearOccupiedArea();
				}
				this.applyToCells = value;
			}
		}
	}

	// Token: 0x06004DDE RID: 19934 RVA: 0x001C40AE File Offset: 0x001C22AE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.updateWithFacing && this.facing != null)
		{
			this.facingLeft = this.facing.facingLeft;
		}
		if (this.applyToCells)
		{
			this.UpdateOccupiedArea();
		}
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x001C40EB File Offset: 0x001C22EB
	private void ValidatePosition()
	{
		if (!Grid.IsValidCell(Grid.PosToCell(this)))
		{
			global::Debug.LogWarning(base.name + " is outside the grid! DELETING!");
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06004DE0 RID: 19936 RVA: 0x001C411A File Offset: 0x001C231A
	[OnSerializing]
	private void OnSerializing()
	{
		this.ValidatePosition();
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001C4122 File Offset: 0x001C2322
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ValidatePosition();
	}

	// Token: 0x06004DE2 RID: 19938 RVA: 0x001C412C File Offset: 0x001C232C
	public int GetOffsetCellWithRotation(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x06004DE3 RID: 19939 RVA: 0x001C4167 File Offset: 0x001C2367
	public void SetCellOffsets(CellOffset[] cells)
	{
		this._UnrotatedOccupiedCellsOffsets = cells;
		this._RotatedOccupiedCellsOffsets = cells;
		this.UpdateRotatedCells();
	}

	// Token: 0x06004DE4 RID: 19940 RVA: 0x001C4180 File Offset: 0x001C2380
	private void UpdateRotatedCells()
	{
		if (this.rotatable != null && this.appliedOrientation != this.rotatable.Orientation)
		{
			this._RotatedOccupiedCellsOffsets = new CellOffset[this._UnrotatedOccupiedCellsOffsets.Length];
			for (int i = 0; i < this._UnrotatedOccupiedCellsOffsets.Length; i++)
			{
				CellOffset offset = this._UnrotatedOccupiedCellsOffsets[i];
				this._RotatedOccupiedCellsOffsets[i] = this.rotatable.GetRotatedCellOffset(offset);
			}
			this.appliedOrientation = this.rotatable.Orientation;
			return;
		}
		if (this.updateWithFacing && this.facing != null && this.facingLeft != this.facing.facingLeft)
		{
			this.facingLeft = this.facing.facingLeft;
			this._RotatedOccupiedCellsOffsets = new CellOffset[this._UnrotatedOccupiedCellsOffsets.Length];
			for (int j = 0; j < this._UnrotatedOccupiedCellsOffsets.Length; j++)
			{
				CellOffset cellOffset = this._UnrotatedOccupiedCellsOffsets[j];
				cellOffset.x *= ((!this.facingLeft) ? -1 : 1);
				this._RotatedOccupiedCellsOffsets[j] = cellOffset;
			}
		}
	}

	// Token: 0x06004DE5 RID: 19941 RVA: 0x001C42A0 File Offset: 0x001C24A0
	public bool CheckIsOccupying(int checkCell)
	{
		int num = Grid.PosToCell(base.gameObject);
		if (checkCell == num)
		{
			return true;
		}
		foreach (CellOffset offset in this.OccupiedCellsOffsets)
		{
			if (Grid.OffsetCell(num, offset) == checkCell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004DE6 RID: 19942 RVA: 0x001C42E9 File Offset: 0x001C24E9
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearOccupiedArea();
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x001C42F8 File Offset: 0x001C24F8
	private void ClearOccupiedArea()
	{
		if (this.occupiedGridCells == null)
		{
			return;
		}
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				foreach (int cell in this.occupiedGridCells)
				{
					if (Grid.Objects[cell, (int)objectLayer] == base.gameObject)
					{
						Grid.Objects[cell, (int)objectLayer] = null;
					}
				}
			}
		}
	}

	// Token: 0x06004DE8 RID: 19944 RVA: 0x001C4374 File Offset: 0x001C2574
	public void UpdateOccupiedArea()
	{
		if (this.objectLayers.Length == 0)
		{
			return;
		}
		if (this.occupiedGridCells == null)
		{
			this.occupiedGridCells = new int[this.OccupiedCellsOffsets.Length];
		}
		this.ClearOccupiedArea();
		int cell = Grid.PosToCell(base.gameObject);
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				for (int j = 0; j < this.OccupiedCellsOffsets.Length; j++)
				{
					CellOffset offset = this.OccupiedCellsOffsets[j];
					int num = Grid.OffsetCell(cell, offset);
					Grid.Objects[num, (int)objectLayer] = base.gameObject;
					this.occupiedGridCells[j] = num;
				}
			}
		}
	}

	// Token: 0x06004DE9 RID: 19945 RVA: 0x001C4424 File Offset: 0x001C2624
	public int GetWidthInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.x);
			num2 = Math.Max(num2, cellOffset.x);
		}
		return num2 - num + 1;
	}

	// Token: 0x06004DEA RID: 19946 RVA: 0x001C447C File Offset: 0x001C267C
	public int GetHeightInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.y);
			num2 = Math.Max(num2, cellOffset.y);
		}
		return num2 - num + 1;
	}

	// Token: 0x06004DEB RID: 19947 RVA: 0x001C44D4 File Offset: 0x001C26D4
	public Extents GetExtents()
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets);
	}

	// Token: 0x06004DEC RID: 19948 RVA: 0x001C44EC File Offset: 0x001C26EC
	public Extents GetExtents(Orientation orientation)
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets, orientation);
	}

	// Token: 0x06004DED RID: 19949 RVA: 0x001C4508 File Offset: 0x001C2708
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.PosToCell(base.gameObject);
		if (this.OccupiedCellsOffsets != null)
		{
			foreach (CellOffset offset in this.OccupiedCellsOffsets)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one);
			}
		}
		if (this.AboveOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset2 in this.AboveOccupiedCellOffsets)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset2)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
		if (this.BelowOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset3 in this.BelowOccupiedCellOffsets)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset3)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
	}

	// Token: 0x06004DEE RID: 19950 RVA: 0x001C4680 File Offset: 0x001C2880
	public bool CanOccupyArea(int rootCell, ObjectLayer layer)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int cell = Grid.OffsetCell(rootCell, offset);
			if (Grid.Objects[cell, (int)layer] != null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004DEF RID: 19951 RVA: 0x001C46CC File Offset: 0x001C28CC
	public bool TestArea(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int arg = Grid.OffsetCell(rootCell, offset);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x001C4710 File Offset: 0x001C2910
	public bool TestAreaAbove(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.AboveOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y + 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.AboveOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.AboveOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.AboveOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004DF1 RID: 19953 RVA: 0x001C47C0 File Offset: 0x001C29C0
	public bool TestAreaBelow(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.BelowOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y - 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.BelowOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.BelowOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.BelowOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040033DE RID: 13278
	private CellOffset[] AboveOccupiedCellOffsets;

	// Token: 0x040033DF RID: 13279
	private CellOffset[] BelowOccupiedCellOffsets;

	// Token: 0x040033E0 RID: 13280
	private int[] occupiedGridCells;

	// Token: 0x040033E1 RID: 13281
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x040033E2 RID: 13282
	private Orientation appliedOrientation;

	// Token: 0x040033E3 RID: 13283
	[MyCmpGet]
	private Facing facing;

	// Token: 0x040033E4 RID: 13284
	private bool facingLeft;

	// Token: 0x040033E5 RID: 13285
	public bool updateWithFacing;

	// Token: 0x040033E6 RID: 13286
	public CellOffset[] _UnrotatedOccupiedCellsOffsets;

	// Token: 0x040033E7 RID: 13287
	public CellOffset[] _RotatedOccupiedCellsOffsets;

	// Token: 0x040033E8 RID: 13288
	public ObjectLayer[] objectLayers = new ObjectLayer[0];

	// Token: 0x040033E9 RID: 13289
	[SerializeField]
	private bool applyToCells = true;
}
