using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CC6 RID: 3270
public abstract class CodexWidget<SubClass> : ICodexWidget, IHasDlcRestrictions
{
	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x060064DA RID: 25818 RVA: 0x00260D99 File Offset: 0x0025EF99
	// (set) Token: 0x060064DB RID: 25819 RVA: 0x00260DA1 File Offset: 0x0025EFA1
	public int preferredWidth { get; set; }

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x060064DC RID: 25820 RVA: 0x00260DAA File Offset: 0x0025EFAA
	// (set) Token: 0x060064DD RID: 25821 RVA: 0x00260DB2 File Offset: 0x0025EFB2
	public int preferredHeight { get; set; }

	// Token: 0x060064DE RID: 25822 RVA: 0x00260DBB File Offset: 0x0025EFBB
	protected CodexWidget()
	{
		this.preferredWidth = -1;
		this.preferredHeight = -1;
	}

	// Token: 0x060064DF RID: 25823 RVA: 0x00260DD1 File Offset: 0x0025EFD1
	protected CodexWidget(int preferredWidth, int preferredHeight)
	{
		this.preferredWidth = preferredWidth;
		this.preferredHeight = preferredHeight;
	}

	// Token: 0x060064E0 RID: 25824
	public abstract void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

	// Token: 0x060064E1 RID: 25825 RVA: 0x00260DE7 File Offset: 0x0025EFE7
	protected void ConfigurePreferredLayout(GameObject contentGameObject)
	{
		LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
		componentInChildren.minWidth = (float)this.preferredWidth;
		componentInChildren.minHeight = (float)this.preferredHeight;
		componentInChildren.preferredHeight = (float)this.preferredHeight;
		componentInChildren.preferredWidth = (float)this.preferredWidth;
	}

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x060064E2 RID: 25826 RVA: 0x00260E22 File Offset: 0x0025F022
	// (set) Token: 0x060064E3 RID: 25827 RVA: 0x00260E2A File Offset: 0x0025F02A
	public string[] requiredAtLeastOneDlcIds { get; set; }

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x060064E4 RID: 25828 RVA: 0x00260E33 File Offset: 0x0025F033
	// (set) Token: 0x060064E5 RID: 25829 RVA: 0x00260E3B File Offset: 0x0025F03B
	public string[] requiredDlcIds { get; set; }

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x060064E6 RID: 25830 RVA: 0x00260E44 File Offset: 0x0025F044
	// (set) Token: 0x060064E7 RID: 25831 RVA: 0x00260E4C File Offset: 0x0025F04C
	public string[] forbiddenDlcIds { get; set; }

	// Token: 0x060064E8 RID: 25832 RVA: 0x00260E55 File Offset: 0x0025F055
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x060064E9 RID: 25833 RVA: 0x00260E5D File Offset: 0x0025F05D
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x060064EA RID: 25834 RVA: 0x00260E65 File Offset: 0x0025F065
	public string[] GetAnyRequiredDlcIds()
	{
		return this.requiredAtLeastOneDlcIds;
	}
}
