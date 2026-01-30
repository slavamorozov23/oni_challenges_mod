using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C67 RID: 3175
public class MoveToLocationToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x060060CD RID: 24781 RVA: 0x00239B70 File Offset: 0x00237D70
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		HoverTextDrawer hoverTextDrawer = HoverTextScreen.Instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		base.DrawTitle(HoverTextScreen.Instance, hoverTextDrawer);
		base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		if (!MoveToLocationTool.Instance.CanMoveTo(num))
		{
			hoverTextDrawer.NewLine(26);
			hoverTextDrawer.DrawText(UI.TOOLS.MOVETOLOCATION.UNREACHABLE, this.HoverTextStyleSettings[1]);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}
}
