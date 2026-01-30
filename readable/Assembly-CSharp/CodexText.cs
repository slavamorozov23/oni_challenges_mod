using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CC8 RID: 3272
public class CodexText : CodexWidget<CodexText>
{
	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x060064FB RID: 25851 RVA: 0x00260F0C File Offset: 0x0025F10C
	// (set) Token: 0x060064FC RID: 25852 RVA: 0x00260F14 File Offset: 0x0025F114
	public string text { get; set; }

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x060064FD RID: 25853 RVA: 0x00260F1D File Offset: 0x0025F11D
	// (set) Token: 0x060064FE RID: 25854 RVA: 0x00260F25 File Offset: 0x0025F125
	public string messageID { get; set; }

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x060064FF RID: 25855 RVA: 0x00260F2E File Offset: 0x0025F12E
	// (set) Token: 0x06006500 RID: 25856 RVA: 0x00260F36 File Offset: 0x0025F136
	public CodexTextStyle style { get; set; }

	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x06006502 RID: 25858 RVA: 0x00260F52 File Offset: 0x0025F152
	// (set) Token: 0x06006501 RID: 25857 RVA: 0x00260F3F File Offset: 0x0025F13F
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

	// Token: 0x06006503 RID: 25859 RVA: 0x00260F6D File Offset: 0x0025F16D
	public CodexText()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x06006504 RID: 25860 RVA: 0x00260F7C File Offset: 0x0025F17C
	public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body, string id = null)
	{
		this.text = text;
		this.style = style;
		if (id != null)
		{
			this.messageID = id;
		}
	}

	// Token: 0x06006505 RID: 25861 RVA: 0x00260F9C File Offset: 0x0025F19C
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x06006506 RID: 25862 RVA: 0x00260FE8 File Offset: 0x0025F1E8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
