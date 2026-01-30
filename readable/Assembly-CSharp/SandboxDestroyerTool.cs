using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009B9 RID: 2489
public class SandboxDestroyerTool : BrushTool
{
	// Token: 0x06004831 RID: 18481 RVA: 0x001A0CD5 File Offset: 0x0019EED5
	public static void DestroyInstance()
	{
		SandboxDestroyerTool.instance = null;
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06004832 RID: 18482 RVA: 0x001A0CDD File Offset: 0x0019EEDD
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x001A0CE9 File Offset: 0x0019EEE9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxDestroyerTool.instance = this;
		this.affectFoundation = true;
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x001A0CFE File Offset: 0x0019EEFE
	protected override string GetDragSound()
	{
		return "SandboxTool_Delete_Add";
	}

	// Token: 0x06004835 RID: 18485 RVA: 0x001A0D05 File Offset: 0x0019EF05
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x001A0D12 File Offset: 0x0019EF12
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x001A0D49 File Offset: 0x0019EF49
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x001A0D64 File Offset: 0x0019EF64
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x001A0DCC File Offset: 0x0019EFCC
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Delete", false));
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x001A0DE5 File Offset: 0x0019EFE5
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		GameUtil.DestroyCell(cell, CellEventLogger.Instance.SandBoxTool, true);
	}

	// Token: 0x0400302A RID: 12330
	public static SandboxDestroyerTool instance;
}
