using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009B7 RID: 2487
public class SandboxClearFloorTool : BrushTool
{
	// Token: 0x0600481A RID: 18458 RVA: 0x001A0851 File Offset: 0x0019EA51
	public static void DestroyInstance()
	{
		SandboxClearFloorTool.instance = null;
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x0600481B RID: 18459 RVA: 0x001A0859 File Offset: 0x0019EA59
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x001A0865 File Offset: 0x0019EA65
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxClearFloorTool.instance = this;
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x001A0873 File Offset: 0x0019EA73
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x001A087A File Offset: 0x0019EA7A
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600481F RID: 18463 RVA: 0x001A0888 File Offset: 0x0019EA88
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x001A08EB File Offset: 0x0019EAEB
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x001A0904 File Offset: 0x0019EB04
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x001A096C File Offset: 0x0019EB6C
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x001A0975 File Offset: 0x0019EB75
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x001A0990 File Offset: 0x0019EB90
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		bool flag = false;
		using (List<Pickupable>.Enumerator enumerator = Components.Pickupables.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Pickupable pickup = enumerator.Current;
				if (!(pickup.storage != null) && Grid.PosToCell(pickup) == cell && Components.LiveMinionIdentities.Items.Find((MinionIdentity match) => match.gameObject == pickup.gameObject) == null)
				{
					if (!flag)
					{
						KFMOD.PlayOneShot(this.soundPath, pickup.gameObject.transform.GetPosition(), 1f);
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickup.transform, 1.5f, false);
						flag = true;
					}
					Util.KDestroyGameObject(pickup.gameObject);
				}
			}
		}
	}

	// Token: 0x04003026 RID: 12326
	public static SandboxClearFloorTool instance;

	// Token: 0x04003027 RID: 12327
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
