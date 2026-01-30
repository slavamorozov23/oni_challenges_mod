using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009B8 RID: 2488
public class SandboxCritterTool : BrushTool
{
	// Token: 0x06004826 RID: 18470 RVA: 0x001A0AC9 File Offset: 0x0019ECC9
	public static void DestroyInstance()
	{
		SandboxCritterTool.instance = null;
	}

	// Token: 0x06004827 RID: 18471 RVA: 0x001A0AD1 File Offset: 0x0019ECD1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxCritterTool.instance = this;
	}

	// Token: 0x06004828 RID: 18472 RVA: 0x001A0ADF File Offset: 0x0019ECDF
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06004829 RID: 18473 RVA: 0x001A0AE6 File Offset: 0x0019ECE6
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600482A RID: 18474 RVA: 0x001A0AF3 File Offset: 0x0019ECF3
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(6f, true);
	}

	// Token: 0x0600482B RID: 18475 RVA: 0x001A0B2A File Offset: 0x0019ED2A
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x0600482C RID: 18476 RVA: 0x001A0B44 File Offset: 0x0019ED44
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x0600482D RID: 18477 RVA: 0x001A0BAC File Offset: 0x0019EDAC
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x0600482E RID: 18478 RVA: 0x001A0BB5 File Offset: 0x0019EDB5
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x0600482F RID: 18479 RVA: 0x001A0BD0 File Offset: 0x0019EDD0
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		HashSetPool<GameObject, SandboxCritterTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxCritterTool>.Allocate();
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell && health.GetComponent<KPrefabID>().HasTag(GameTags.Creature))
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (GameObject gameObject in pooledHashSet)
		{
			KFMOD.PlayOneShot(this.soundPath, gameObject.gameObject.transform.GetPosition(), 1f);
			Util.KDestroyGameObject(gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04003028 RID: 12328
	public static SandboxCritterTool instance;

	// Token: 0x04003029 RID: 12329
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
