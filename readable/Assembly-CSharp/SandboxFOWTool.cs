using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020009BA RID: 2490
public class SandboxFOWTool : BrushTool
{
	// Token: 0x0600483C RID: 18492 RVA: 0x001A0E08 File Offset: 0x0019F008
	public static void DestroyInstance()
	{
		SandboxFOWTool.instance = null;
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x0600483D RID: 18493 RVA: 0x001A0E10 File Offset: 0x0019F010
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x001A0E1C File Offset: 0x0019F01C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFOWTool.instance = this;
	}

	// Token: 0x0600483F RID: 18495 RVA: 0x001A0E2A File Offset: 0x0019F02A
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06004840 RID: 18496 RVA: 0x001A0E31 File Offset: 0x0019F031
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x001A0E3E File Offset: 0x0019F03E
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x001A0E75 File Offset: 0x0019F075
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x06004843 RID: 18499 RVA: 0x001A0E9C File Offset: 0x0019F09C
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06004844 RID: 18500 RVA: 0x001A0F54 File Offset: 0x0019F154
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x001A0F5D File Offset: 0x0019F15D
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		Grid.Reveal(cell, byte.MaxValue, true);
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x001A0F74 File Offset: 0x0019F174
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		int intSetting = this.settings.GetIntSetting("SandboxTools.BrushSize");
		this.ev = KFMOD.CreateInstance(GlobalAssets.GetSound("SandboxTool_Reveal", false));
		this.ev.setParameterByName("BrushSize", (float)intSetting, false);
		this.ev.start();
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x001A0FCF File Offset: 0x0019F1CF
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.ev.stop(STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x0400302B RID: 12331
	public static SandboxFOWTool instance;

	// Token: 0x0400302C RID: 12332
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x0400302D RID: 12333
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x0400302E RID: 12334
	private EventInstance ev;
}
