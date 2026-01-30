using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C56 RID: 3158
public class EmptyPipeToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005FF0 RID: 24560 RVA: 0x002332E0 File Offset: 0x002314E0
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

	// Token: 0x06005FF1 RID: 24561 RVA: 0x00233380 File Offset: 0x00231580
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
