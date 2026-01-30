using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009A1 RID: 2465
public class CancelTool : FilteredDragTool
{
	// Token: 0x06004712 RID: 18194 RVA: 0x0019BC07 File Offset: 0x00199E07
	public static void DestroyInstance()
	{
		CancelTool.Instance = null;
	}

	// Token: 0x06004713 RID: 18195 RVA: 0x0019BC0F File Offset: 0x00199E0F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CancelTool.Instance = this;
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x0019BC1D File Offset: 0x00199E1D
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		base.GetDefaultFilters(filters);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CLEANANDCLEAR, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.DIGPLACER, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x0019BC3E File Offset: 0x00199E3E
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06004716 RID: 18198 RVA: 0x0019BC45 File Offset: 0x00199E45
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x0019BC4C File Offset: 0x00199E4C
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(2127324410, null);
				}
			}
		}
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x0019BC9C File Offset: 0x00199E9C
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, false);
	}

	// Token: 0x04002FC0 RID: 12224
	public static CancelTool Instance;
}
