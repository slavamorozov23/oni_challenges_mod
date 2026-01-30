using System;
using System.Collections.Generic;
using KSerialization.Converters;
using UnityEngine;

// Token: 0x02000CC7 RID: 3271
public class ContentContainer : IHasDlcRestrictions
{
	// Token: 0x060064EB RID: 25835 RVA: 0x00260E6D File Offset: 0x0025F06D
	public ContentContainer()
	{
		this.content = new List<ICodexWidget>();
	}

	// Token: 0x060064EC RID: 25836 RVA: 0x00260E80 File Offset: 0x0025F080
	public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
	{
		this.content = content;
		this.contentLayout = contentLayout;
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x060064ED RID: 25837 RVA: 0x00260E96 File Offset: 0x0025F096
	// (set) Token: 0x060064EE RID: 25838 RVA: 0x00260E9E File Offset: 0x0025F09E
	public List<ICodexWidget> content { get; set; }

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x060064EF RID: 25839 RVA: 0x00260EA7 File Offset: 0x0025F0A7
	// (set) Token: 0x060064F0 RID: 25840 RVA: 0x00260EAF File Offset: 0x0025F0AF
	public string lockID { get; set; }

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x060064F1 RID: 25841 RVA: 0x00260EB8 File Offset: 0x0025F0B8
	// (set) Token: 0x060064F2 RID: 25842 RVA: 0x00260EC0 File Offset: 0x0025F0C0
	public string[] requiredDlcIds { get; set; }

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x060064F3 RID: 25843 RVA: 0x00260EC9 File Offset: 0x0025F0C9
	// (set) Token: 0x060064F4 RID: 25844 RVA: 0x00260ED1 File Offset: 0x0025F0D1
	public string[] forbiddenDlcIds { get; set; }

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x060064F5 RID: 25845 RVA: 0x00260EDA File Offset: 0x0025F0DA
	// (set) Token: 0x060064F6 RID: 25846 RVA: 0x00260EE2 File Offset: 0x0025F0E2
	[StringEnumConverter]
	public ContentContainer.ContentLayout contentLayout { get; set; }

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x060064F7 RID: 25847 RVA: 0x00260EEB File Offset: 0x0025F0EB
	// (set) Token: 0x060064F8 RID: 25848 RVA: 0x00260EF3 File Offset: 0x0025F0F3
	public bool showBeforeGeneratedContent { get; set; }

	// Token: 0x060064F9 RID: 25849 RVA: 0x00260EFC File Offset: 0x0025F0FC
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x060064FA RID: 25850 RVA: 0x00260F04 File Offset: 0x0025F104
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04004485 RID: 17541
	public GameObject go;

	// Token: 0x02001F04 RID: 7940
	public enum ContentLayout
	{
		// Token: 0x04009158 RID: 37208
		Vertical,
		// Token: 0x04009159 RID: 37209
		Horizontal,
		// Token: 0x0400915A RID: 37210
		Grid,
		// Token: 0x0400915B RID: 37211
		GridTwoColumn,
		// Token: 0x0400915C RID: 37212
		GridTwoColumnTall
	}
}
