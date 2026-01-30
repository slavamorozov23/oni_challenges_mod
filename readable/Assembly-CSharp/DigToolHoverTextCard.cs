using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C54 RID: 3156
public class DigToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06005FE2 RID: 24546 RVA: 0x00232D4C File Offset: 0x00230F4C
	public override void UpdateHoverElements(List<KSelectable> selected)
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
		if (Grid.IsVisible(num))
		{
			base.DrawTitle(instance, hoverTextDrawer);
			base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
			Element element = Grid.Element[num];
			bool flag = false;
			if (Grid.Solid[num] && Diggable.IsDiggable(num))
			{
				flag = true;
			}
			if (flag)
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
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				hoverTextDrawer.DrawText(GameUtil.GetHardnessString(Grid.Element[num], true), this.Styles_BodyText.Standard);
			}
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("iconWarning"), 18);
			hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.UNKNOWN, this.Styles_BodyText.Standard);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x0400400F RID: 16399
	private DigToolHoverTextCard.HoverScreenFields hoverScreenElements;

	// Token: 0x02001E02 RID: 7682
	private struct HoverScreenFields
	{
		// Token: 0x04008CF4 RID: 36084
		public GameObject UnknownAreaLine;

		// Token: 0x04008CF5 RID: 36085
		public Image ElementStateIcon;

		// Token: 0x04008CF6 RID: 36086
		public LocText ElementCategory;

		// Token: 0x04008CF7 RID: 36087
		public LocText ElementName;

		// Token: 0x04008CF8 RID: 36088
		public LocText[] ElementMass;

		// Token: 0x04008CF9 RID: 36089
		public LocText ElementHardness;

		// Token: 0x04008CFA RID: 36090
		public LocText ElementHardnessDescription;
	}
}
