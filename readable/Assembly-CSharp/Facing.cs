using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005DE RID: 1502
[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
	// Token: 0x060022CB RID: 8907 RVA: 0x000CAC02 File Offset: 0x000C8E02
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("Facing", 35);
	}

	// Token: 0x060022CC RID: 8908 RVA: 0x000CAC1C File Offset: 0x000C8E1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateMirror();
	}

	// Token: 0x060022CD RID: 8909 RVA: 0x000CAC2C File Offset: 0x000C8E2C
	public void Face(float target_x)
	{
		float x = base.transform.GetLocalPosition().x;
		if (target_x < x)
		{
			this.SetFacing(true);
			return;
		}
		if (target_x > x)
		{
			this.SetFacing(false);
		}
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x000CAC64 File Offset: 0x000C8E64
	public void Face(Vector3 target_pos)
	{
		int num = Grid.CellColumn(Grid.PosToCell(base.transform.GetLocalPosition()));
		int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
		if (num > num2)
		{
			this.SetFacing(true);
			return;
		}
		if (num2 > num)
		{
			this.SetFacing(false);
		}
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000CACAA File Offset: 0x000C8EAA
	[ContextMenu("Flip")]
	public void SwapFacing()
	{
		this.SetFacing(!this.facingLeft);
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x000CACBB File Offset: 0x000C8EBB
	private void UpdateMirror()
	{
		if (this.kanimController != null && this.kanimController.FlipX != this.facingLeft)
		{
			this.kanimController.FlipX = this.facingLeft;
			bool flag = this.facingLeft;
		}
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x000CACF6 File Offset: 0x000C8EF6
	public bool GetFacing()
	{
		return this.facingLeft;
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000CACFE File Offset: 0x000C8EFE
	public void SetFacing(bool mirror_x)
	{
		this.facingLeft = mirror_x;
		this.UpdateMirror();
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000CAD10 File Offset: 0x000C8F10
	public int GetFrontCell()
	{
		int cell = Grid.PosToCell(this);
		if (this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000CAD3C File Offset: 0x000C8F3C
	public int GetBackCell()
	{
		int cell = Grid.PosToCell(this);
		if (!this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x04001459 RID: 5209
	[MyCmpGet]
	private KAnimControllerBase kanimController;

	// Token: 0x0400145A RID: 5210
	private LoggerFS log;

	// Token: 0x0400145B RID: 5211
	[Serialize]
	public bool facingLeft;
}
