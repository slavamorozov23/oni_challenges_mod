using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CD1 RID: 3281
public class CodexLabelWithLargeIcon : CodexLabelWithIcon
{
	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x06006549 RID: 25929 RVA: 0x0026181E File Offset: 0x0025FA1E
	// (set) Token: 0x0600654A RID: 25930 RVA: 0x00261826 File Offset: 0x0025FA26
	public string linkID { get; set; }

	// Token: 0x0600654B RID: 25931 RVA: 0x0026182F File Offset: 0x0025FA2F
	public CodexLabelWithLargeIcon()
	{
	}

	// Token: 0x0600654C RID: 25932 RVA: 0x00261838 File Offset: 0x0025FA38
	public CodexLabelWithLargeIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, string targetEntrylinkID) : base(text, style, coloredSprite, 128, 128)
	{
		base.icon = new CodexImage(128, 128, coloredSprite);
		base.label = new CodexText(text, style, null);
		this.linkID = targetEntrylinkID;
	}

	// Token: 0x0600654D RID: 25933 RVA: 0x00261884 File Offset: 0x0025FA84
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.icon.ConfigureImage(contentGameObject.GetComponentsInChildren<Image>()[1]);
		if (base.icon.preferredWidth != -1 && base.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentsInChildren<Image>()[1].GetComponent<LayoutElement>();
			component.minWidth = (float)base.icon.preferredHeight;
			component.minHeight = (float)base.icon.preferredWidth;
			component.preferredHeight = (float)base.icon.preferredHeight;
			component.preferredWidth = (float)base.icon.preferredWidth;
		}
		base.label.text = UI.StripLinkFormatting(base.label.text);
		base.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		contentGameObject.GetComponent<KButton>().ClearOnClick();
		contentGameObject.GetComponent<KButton>().onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(this.linkID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
	}
}
