using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C66 RID: 3174
public class MopToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x060060CB RID: 24779 RVA: 0x002399BC File Offset: 0x00237BBC
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		if (Grid.IsVisible(num))
		{
			base.DrawTitle(instance, hoverTextDrawer);
			base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
			Element element = Grid.Element[num];
			if (element.IsLiquid)
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawText(element.nameUpperCase, this.Styles_Title.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				hoverTextDrawer.DrawText(element.GetMaterialCategoryTag().ProperName(), this.Styles_BodyText.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				string[] array = HoverTextHelper.MassStringsReadOnly(num);
				hoverTextDrawer.DrawText(array[0], this.Styles_Values.Property.Standard);
				hoverTextDrawer.DrawText(array[1], this.Styles_Values.Property_Decimal.Standard);
				hoverTextDrawer.DrawText(array[2], this.Styles_Values.Property.Standard);
				hoverTextDrawer.DrawText(array[3], this.Styles_Values.Property.Standard);
			}
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("iconWarning"), 18);
			hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.UNKNOWN.ToString().ToUpper(), this.Styles_BodyText.Standard);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x040040AB RID: 16555
	private MopToolHoverTextCard.HoverScreenFields hoverScreenElements;

	// Token: 0x02001E21 RID: 7713
	private struct HoverScreenFields
	{
		// Token: 0x04008D74 RID: 36212
		public GameObject UnknownAreaLine;

		// Token: 0x04008D75 RID: 36213
		public Image ElementStateIcon;

		// Token: 0x04008D76 RID: 36214
		public LocText ElementCategory;

		// Token: 0x04008D77 RID: 36215
		public LocText ElementName;

		// Token: 0x04008D78 RID: 36216
		public LocText[] ElementMass;

		// Token: 0x04008D79 RID: 36217
		public LocText ElementHardness;

		// Token: 0x04008D7A RID: 36218
		public LocText ElementHardnessDescription;
	}
}
