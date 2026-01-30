using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C46 RID: 3142
public class AttackToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005F1B RID: 24347 RVA: 0x0022C420 File Offset: 0x0022A620
	public override void UpdateHoverElements(List<KSelectable> hover_objects)
	{
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
		hoverTextDrawer.EndShadowBar();
		if (hover_objects != null)
		{
			foreach (KSelectable kselectable in hover_objects)
			{
				if (kselectable.GetComponent<AttackableBase>() != null)
				{
					hoverTextDrawer.BeginShadowBar(false);
					hoverTextDrawer.DrawText(kselectable.GetProperName().ToUpper(), this.Styles_Title.Standard);
					hoverTextDrawer.EndShadowBar();
					break;
				}
			}
		}
		hoverTextDrawer.EndDrawing();
	}
}
