using System;
using UnityEngine;

// Token: 0x020009B2 RID: 2482
public class MoveToLocationTool : InterfaceTool
{
	// Token: 0x060047DF RID: 18399 RVA: 0x0019F926 File Offset: 0x0019DB26
	public static void DestroyInstance()
	{
		MoveToLocationTool.Instance = null;
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x0019F92E File Offset: 0x0019DB2E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MoveToLocationTool.Instance = this;
		this.visualizer = Util.KInstantiate(this.visualizer, null, null);
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x0019F94F File Offset: 0x0019DB4F
	public void Activate(Navigator navigator)
	{
		this.targetNavigator = navigator;
		this.targetMovable = null;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x0019F96A File Offset: 0x0019DB6A
	public void Activate(Movable movable)
	{
		this.targetNavigator = null;
		this.targetMovable = movable;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x0019F988 File Offset: 0x0019DB88
	public bool CanMoveTo(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			return this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>() != null && this.targetNavigator.CanReach(target_cell);
		}
		return this.targetMovable != null && this.targetMovable.CanMoveTo(target_cell);
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x0019F9DC File Offset: 0x0019DBDC
	private void SetMoveToLocation(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			MoveToLocationMonitor.Instance smi = this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>();
			if (smi != null)
			{
				smi.MoveToLocation(target_cell);
				return;
			}
		}
		else if (this.targetMovable != null)
		{
			this.targetMovable.MoveToLocation(target_cell);
		}
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x0019FA28 File Offset: 0x0019DC28
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.visualizer.gameObject.SetActive(true);
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x0019FA44 File Offset: 0x0019DC44
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (this.targetNavigator != null && new_tool == SelectTool.Instance)
		{
			SelectTool.Instance.SelectNextFrame(this.targetNavigator.GetComponent<KSelectable>(), true);
		}
		this.visualizer.gameObject.SetActive(false);
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x0019FA9C File Offset: 0x0019DC9C
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (this.targetNavigator != null || this.targetMovable != null)
		{
			int mouseCell = DebugHandler.GetMouseCell();
			if (this.CanMoveTo(mouseCell))
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
				this.SetMoveToLocation(mouseCell);
				SelectTool.Instance.Activate();
				return;
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x0019FB10 File Offset: 0x0019DD10
	private void RefreshColor()
	{
		Color white = new Color(0.91f, 0.21f, 0.2f);
		if (this.CanMoveTo(DebugHandler.GetMouseCell()))
		{
			white = Color.white;
		}
		this.SetColor(this.visualizer, white);
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x0019FB53 File Offset: 0x0019DD53
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.RefreshColor();
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x0019FB62 File Offset: 0x0019DD62
	private void SetColor(GameObject root, Color c)
	{
		root.GetComponentInChildren<MeshRenderer>().material.color = c;
	}

	// Token: 0x04003014 RID: 12308
	public static MoveToLocationTool Instance;

	// Token: 0x04003015 RID: 12309
	private Navigator targetNavigator;

	// Token: 0x04003016 RID: 12310
	private Movable targetMovable;
}
