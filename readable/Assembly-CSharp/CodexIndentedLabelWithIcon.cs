using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CCD RID: 3277
public class CodexIndentedLabelWithIcon : CodexWidget<CodexIndentedLabelWithIcon>
{
	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x06006529 RID: 25897 RVA: 0x002612EF File Offset: 0x0025F4EF
	// (set) Token: 0x0600652A RID: 25898 RVA: 0x002612F7 File Offset: 0x0025F4F7
	public CodexImage icon { get; set; }

	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x0600652B RID: 25899 RVA: 0x00261300 File Offset: 0x0025F500
	// (set) Token: 0x0600652C RID: 25900 RVA: 0x00261308 File Offset: 0x0025F508
	public CodexText label { get; set; }

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x0600652D RID: 25901 RVA: 0x00261311 File Offset: 0x0025F511
	// (set) Token: 0x0600652E RID: 25902 RVA: 0x00261319 File Offset: 0x0025F519
	public string stringKey { get; set; } = "";

	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x0600652F RID: 25903 RVA: 0x00261322 File Offset: 0x0025F522
	// (set) Token: 0x06006530 RID: 25904 RVA: 0x0026132A File Offset: 0x0025F52A
	public string batchedAnimPrefabSourceID { get; set; } = "";

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x06006531 RID: 25905 RVA: 0x00261333 File Offset: 0x0025F533
	// (set) Token: 0x06006532 RID: 25906 RVA: 0x0026133B File Offset: 0x0025F53B
	public string spriteName { get; set; } = "";

	// Token: 0x06006533 RID: 25907 RVA: 0x00261344 File Offset: 0x0025F544
	public CodexIndentedLabelWithIcon()
	{
		this.icon = new CodexImage();
		this.label = new CodexText();
	}

	// Token: 0x06006534 RID: 25908 RVA: 0x00261384 File Offset: 0x0025F584
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x002613D4 File Offset: 0x0025F5D4
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x00261428 File Offset: 0x0025F628
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		if (!string.IsNullOrEmpty(this.stringKey))
		{
			this.label.stringKey = this.stringKey;
		}
		if (!string.IsNullOrEmpty(this.batchedAnimPrefabSourceID))
		{
			GameObject gameObject = Assets.TryGetPrefab(this.batchedAnimPrefabSourceID);
			KBatchedAnimController kbatchedAnimController = (gameObject != null) ? gameObject.GetComponent<KBatchedAnimController>() : null;
			KAnimFile kanimFile = (kbatchedAnimController != null) ? kbatchedAnimController.AnimFiles[0] : null;
			this.icon.sprite = ((kanimFile != null) ? Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "") : null);
		}
		if (!string.IsNullOrEmpty(this.spriteName))
		{
			this.icon.sprite = Assets.GetSprite(this.spriteName);
		}
		Image componentInChildren = contentGameObject.GetComponentInChildren<Image>();
		this.icon.ConfigureImage(componentInChildren);
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
	}
}
