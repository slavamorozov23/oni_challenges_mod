using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C59 RID: 3161
[AddComponentMenu("KMonoBehaviour/scripts/HoverTextConfiguration")]
public class HoverTextConfiguration : KMonoBehaviour
{
	// Token: 0x06006002 RID: 24578 RVA: 0x00233775 File Offset: 0x00231975
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ConfigureHoverScreen();
	}

	// Token: 0x06006003 RID: 24579 RVA: 0x00233783 File Offset: 0x00231983
	protected virtual void ConfigureTitle(HoverTextScreen screen)
	{
		if (string.IsNullOrEmpty(this.ToolName))
		{
			this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
		}
	}

	// Token: 0x06006004 RID: 24580 RVA: 0x002337AD File Offset: 0x002319AD
	protected void DrawTitle(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		drawer.DrawText(this.ToolName, this.ToolTitleTextStyle);
	}

	// Token: 0x06006005 RID: 24581 RVA: 0x002337C4 File Offset: 0x002319C4
	protected void DrawInstructions(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		TextStyleSetting standard = this.Styles_Instruction.Standard;
		drawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseLeft, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_left"), 20);
		}
		drawer.DrawText(this.ActionName, standard);
		drawer.AddIndent(8);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_right"), 20);
		}
		drawer.DrawText(this.backStr, standard);
	}

	// Token: 0x06006006 RID: 24582 RVA: 0x00233868 File Offset: 0x00231A68
	public virtual void ConfigureHoverScreen()
	{
		if (!string.IsNullOrEmpty(this.ActionStringKey))
		{
			this.ActionName = Strings.Get(this.ActionStringKey);
		}
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.ConfigureTitle(instance);
		this.backStr = UI.TOOLS.GENERIC.BACK.ToString().ToUpper();
	}

	// Token: 0x06006007 RID: 24583 RVA: 0x002338BC File Offset: 0x00231ABC
	public virtual void UpdateHoverElements(List<KSelectable> hover_objects)
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
		this.DrawTitle(instance, hoverTextDrawer);
		this.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04004021 RID: 16417
	public TextStyleSetting[] HoverTextStyleSettings;

	// Token: 0x04004022 RID: 16418
	public string ToolNameStringKey = "";

	// Token: 0x04004023 RID: 16419
	public string ActionStringKey = "";

	// Token: 0x04004024 RID: 16420
	[HideInInspector]
	public string ActionName = "";

	// Token: 0x04004025 RID: 16421
	[HideInInspector]
	public string ToolName;

	// Token: 0x04004026 RID: 16422
	protected string backStr;

	// Token: 0x04004027 RID: 16423
	public TextStyleSetting ToolTitleTextStyle;

	// Token: 0x04004028 RID: 16424
	public HoverTextConfiguration.TextStylePair Styles_Title;

	// Token: 0x04004029 RID: 16425
	public HoverTextConfiguration.TextStylePair Styles_BodyText;

	// Token: 0x0400402A RID: 16426
	public HoverTextConfiguration.TextStylePair Styles_Instruction;

	// Token: 0x0400402B RID: 16427
	public HoverTextConfiguration.TextStylePair Styles_Warning;

	// Token: 0x0400402C RID: 16428
	public HoverTextConfiguration.ValuePropertyTextStyles Styles_Values;

	// Token: 0x02001E03 RID: 7683
	[Serializable]
	public struct TextStylePair
	{
		// Token: 0x04008CFB RID: 36091
		public TextStyleSetting Standard;

		// Token: 0x04008CFC RID: 36092
		public TextStyleSetting Selected;
	}

	// Token: 0x02001E04 RID: 7684
	[Serializable]
	public struct ValuePropertyTextStyles
	{
		// Token: 0x04008CFD RID: 36093
		public HoverTextConfiguration.TextStylePair Property;

		// Token: 0x04008CFE RID: 36094
		public HoverTextConfiguration.TextStylePair Property_Decimal;

		// Token: 0x04008CFF RID: 36095
		public HoverTextConfiguration.TextStylePair Property_Unit;
	}
}
