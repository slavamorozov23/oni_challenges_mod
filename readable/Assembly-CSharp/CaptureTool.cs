using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009A2 RID: 2466
public class CaptureTool : DragTool
{
	// Token: 0x0600471A RID: 18202 RVA: 0x0019BCF4 File Offset: 0x00199EF4
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, true);
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x0019BD3C File Offset: 0x00199F3C
	public static void MarkForCapture(Vector2 min, Vector2 max, bool mark)
	{
		foreach (Capturable capturable in Components.Capturables.Items)
		{
			Vector2 vector = Grid.PosToXY(capturable.transform.GetPosition());
			if (vector.x >= min.x && vector.x < max.x && vector.y >= min.y && vector.y < max.y)
			{
				if (capturable.allowCapture)
				{
					PrioritySetting lastSelectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
					capturable.MarkForCapture(mark, lastSelectedPriority, true);
				}
				else if (mark)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.TOOLS.CAPTURE.NOT_CAPTURABLE, null, capturable.transform.GetPosition(), 1.5f, false, false);
				}
			}
		}
	}

	// Token: 0x0600471C RID: 18204 RVA: 0x0019BE3C File Offset: 0x0019A03C
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x0019BE54 File Offset: 0x0019A054
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}
}
