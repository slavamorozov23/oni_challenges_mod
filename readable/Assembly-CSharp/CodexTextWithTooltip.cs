using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CC9 RID: 3273
public class CodexTextWithTooltip : CodexWidget<CodexTextWithTooltip>
{
	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x06006507 RID: 25863 RVA: 0x00260FFE File Offset: 0x0025F1FE
	// (set) Token: 0x06006508 RID: 25864 RVA: 0x00261006 File Offset: 0x0025F206
	public string text { get; set; }

	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x06006509 RID: 25865 RVA: 0x0026100F File Offset: 0x0025F20F
	// (set) Token: 0x0600650A RID: 25866 RVA: 0x00261017 File Offset: 0x0025F217
	public string tooltip { get; set; }

	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x0600650B RID: 25867 RVA: 0x00261020 File Offset: 0x0025F220
	// (set) Token: 0x0600650C RID: 25868 RVA: 0x00261028 File Offset: 0x0025F228
	public CodexTextStyle style { get; set; }

	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x0600650E RID: 25870 RVA: 0x00261044 File Offset: 0x0025F244
	// (set) Token: 0x0600650D RID: 25869 RVA: 0x00261031 File Offset: 0x0025F231
	public string stringKey
	{
		get
		{
			return "--> " + (this.text ?? "NULL");
		}
		set
		{
			this.text = Strings.Get(value);
		}
	}

	// Token: 0x0600650F RID: 25871 RVA: 0x0026105F File Offset: 0x0025F25F
	public CodexTextWithTooltip()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x06006510 RID: 25872 RVA: 0x0026106E File Offset: 0x0025F26E
	public CodexTextWithTooltip(string text, string tooltip, CodexTextStyle style = CodexTextStyle.Body)
	{
		this.text = text;
		this.style = style;
		this.tooltip = tooltip;
	}

	// Token: 0x06006511 RID: 25873 RVA: 0x0026108C File Offset: 0x0025F28C
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x06006512 RID: 25874 RVA: 0x002610D8 File Offset: 0x0025F2D8
	public void ConfigureTooltip(ToolTip tooltip)
	{
		tooltip.SetSimpleTooltip(this.tooltip);
	}

	// Token: 0x06006513 RID: 25875 RVA: 0x002610E6 File Offset: 0x0025F2E6
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		this.ConfigureTooltip(contentGameObject.GetComponent<ToolTip>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
