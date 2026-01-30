using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C58 RID: 3160
public class HarvestToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005FFF RID: 24575 RVA: 0x00233618 File Offset: 0x00231818
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		base.DrawTitle(instance, hoverTextDrawer);
		base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		if (lastEnabledFilter != null && lastEnabledFilter != "ALL")
		{
			this.ConfigureTitle(instance);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x06006000 RID: 24576 RVA: 0x002336B8 File Offset: 0x002318B8
	protected override void ConfigureTitle(HoverTextScreen screen)
	{
		string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
		if (string.IsNullOrEmpty(this.ToolName) || lastEnabledFilter == "ALL")
		{
			this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
		}
		if (lastEnabledFilter != null && lastEnabledFilter != "ALL")
		{
			this.ToolName = string.Format(UI.TOOLS.CAPITALS, Strings.Get(this.ToolNameStringKey).String + string.Format(UI.TOOLS.FILTER_HOVERCARD_HEADER, Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + lastEnabledFilter + ".TOOLTIP").String));
		}
	}
}
