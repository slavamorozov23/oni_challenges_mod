using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CD8 RID: 3288
public class CodexVideo : CodexWidget<CodexVideo>
{
	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x06006574 RID: 25972 RVA: 0x002636B9 File Offset: 0x002618B9
	// (set) Token: 0x06006575 RID: 25973 RVA: 0x002636C1 File Offset: 0x002618C1
	public string name { get; set; }

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x06006577 RID: 25975 RVA: 0x002636D3 File Offset: 0x002618D3
	// (set) Token: 0x06006576 RID: 25974 RVA: 0x002636CA File Offset: 0x002618CA
	public string videoName
	{
		get
		{
			return "--> " + (this.name ?? "NULL");
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x06006578 RID: 25976 RVA: 0x002636EE File Offset: 0x002618EE
	// (set) Token: 0x06006579 RID: 25977 RVA: 0x002636F6 File Offset: 0x002618F6
	public string overlayName { get; set; }

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x0600657A RID: 25978 RVA: 0x002636FF File Offset: 0x002618FF
	// (set) Token: 0x0600657B RID: 25979 RVA: 0x00263707 File Offset: 0x00261907
	public List<string> overlayTexts { get; set; }

	// Token: 0x0600657C RID: 25980 RVA: 0x00263710 File Offset: 0x00261910
	public void ConfigureVideo(VideoWidget videoWidget, string clipName, string overlayName = null, List<string> overlayTexts = null)
	{
		videoWidget.SetClip(Assets.GetVideo(clipName), overlayName, overlayTexts);
	}

	// Token: 0x0600657D RID: 25981 RVA: 0x00263721 File Offset: 0x00261921
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.preferredHeight = 180;
		base.preferredWidth = 320;
		this.ConfigureVideo(contentGameObject.GetComponent<VideoWidget>(), this.name, this.overlayName, this.overlayTexts);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
