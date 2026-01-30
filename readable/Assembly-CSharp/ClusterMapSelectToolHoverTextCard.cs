using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C4F RID: 3151
public class ClusterMapSelectToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005FC1 RID: 24513 RVA: 0x00231DB8 File Offset: 0x0022FFB8
	public override void ConfigureHoverScreen()
	{
		base.ConfigureHoverScreen();
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.m_iconWarning = instance.GetSprite("iconWarning");
		this.m_iconDash = instance.GetSprite("dash");
		this.m_iconHighlighted = instance.GetSprite("dash_arrow");
	}

	// Token: 0x06005FC2 RID: 24514 RVA: 0x00231E04 File Offset: 0x00230004
	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
	{
		if (this.m_iconWarning == null)
		{
			this.ConfigureHoverScreen();
		}
		HoverTextDrawer hoverTextDrawer = HoverTextScreen.Instance.BeginDrawing();
		foreach (KSelectable kselectable in hoverObjects)
		{
			hoverTextDrawer.BeginShadowBar(ClusterMapSelectTool.Instance.GetSelected() == kselectable);
			string unitFormattedName = GameUtil.GetUnitFormattedName(kselectable.gameObject, true);
			hoverTextDrawer.DrawText(unitFormattedName, this.Styles_Title.Standard);
			foreach (StatusItemGroup.Entry entry in kselectable.GetStatusItemGroup())
			{
				if (entry.category != null && entry.category.Id == "Main")
				{
					TextStyleSetting style = this.IsStatusItemWarning(entry) ? this.Styles_Warning.Standard : this.Styles_BodyText.Standard;
					Sprite icon = (entry.item.sprite != null) ? entry.item.sprite.sprite : this.m_iconWarning;
					Color color = this.IsStatusItemWarning(entry) ? this.Styles_Warning.Standard.textColor : this.Styles_BodyText.Standard.textColor;
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawIcon(icon, color, 18, 2);
					hoverTextDrawer.DrawText(entry.GetName(), style);
				}
			}
			foreach (StatusItemGroup.Entry entry2 in kselectable.GetStatusItemGroup())
			{
				if (entry2.category == null || entry2.category.Id != "Main")
				{
					TextStyleSetting style2 = this.IsStatusItemWarning(entry2) ? this.Styles_Warning.Standard : this.Styles_BodyText.Standard;
					Sprite icon2 = (entry2.item.sprite != null) ? entry2.item.sprite.sprite : this.m_iconWarning;
					Color color2 = this.IsStatusItemWarning(entry2) ? this.Styles_Warning.Standard.textColor : this.Styles_BodyText.Standard.textColor;
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawIcon(icon2, color2, 18, 2);
					hoverTextDrawer.DrawText(entry2.GetName(), style2);
				}
			}
			hoverTextDrawer.EndShadowBar();
		}
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x06005FC3 RID: 24515 RVA: 0x002320E0 File Offset: 0x002302E0
	private bool IsStatusItemWarning(StatusItemGroup.Entry item)
	{
		return item.item.notificationType == NotificationType.Bad || item.item.notificationType == NotificationType.BadMinor || item.item.notificationType == NotificationType.DuplicantThreatening;
	}

	// Token: 0x04003FF5 RID: 16373
	private Sprite m_iconWarning;

	// Token: 0x04003FF6 RID: 16374
	private Sprite m_iconDash;

	// Token: 0x04003FF7 RID: 16375
	private Sprite m_iconHighlighted;
}
