using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C4B RID: 3147
public class CancelToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005F4C RID: 24396 RVA: 0x0022E4C0 File Offset: 0x0022C6C0
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
		if (lastEnabledFilter != null && lastEnabledFilter != this.lastUpdatedFilter)
		{
			this.ConfigureTitle(instance);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x06005F4D RID: 24397 RVA: 0x0022E560 File Offset: 0x0022C760
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
		this.lastUpdatedFilter = lastEnabledFilter;
	}

	// Token: 0x04003F95 RID: 16277
	private string lastUpdatedFilter;
}
