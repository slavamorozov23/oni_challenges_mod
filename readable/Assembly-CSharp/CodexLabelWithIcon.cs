using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CD0 RID: 3280
public class CodexLabelWithIcon : CodexWidget<CodexLabelWithIcon>
{
	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x0600653B RID: 25915 RVA: 0x00261592 File Offset: 0x0025F792
	// (set) Token: 0x0600653C RID: 25916 RVA: 0x0026159A File Offset: 0x0025F79A
	public CodexImage icon { get; set; }

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x0600653D RID: 25917 RVA: 0x002615A3 File Offset: 0x0025F7A3
	// (set) Token: 0x0600653E RID: 25918 RVA: 0x002615AB File Offset: 0x0025F7AB
	public CodexText label { get; set; }

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x0600653F RID: 25919 RVA: 0x002615B4 File Offset: 0x0025F7B4
	// (set) Token: 0x06006540 RID: 25920 RVA: 0x002615BC File Offset: 0x0025F7BC
	public string stringKey { get; set; } = "";

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x06006541 RID: 25921 RVA: 0x002615C5 File Offset: 0x0025F7C5
	// (set) Token: 0x06006542 RID: 25922 RVA: 0x002615CD File Offset: 0x0025F7CD
	public string batchedAnimPrefabSourceID { get; set; } = "";

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x06006543 RID: 25923 RVA: 0x002615D6 File Offset: 0x0025F7D6
	// (set) Token: 0x06006544 RID: 25924 RVA: 0x002615DE File Offset: 0x0025F7DE
	public string spriteName { get; set; } = "";

	// Token: 0x06006545 RID: 25925 RVA: 0x002615E7 File Offset: 0x0025F7E7
	public CodexLabelWithIcon()
	{
		this.icon = new CodexImage();
		this.label = new CodexText();
	}

	// Token: 0x06006546 RID: 25926 RVA: 0x00261628 File Offset: 0x0025F828
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06006547 RID: 25927 RVA: 0x00261678 File Offset: 0x0025F878
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06006548 RID: 25928 RVA: 0x002616CC File Offset: 0x0025F8CC
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
		this.icon.ConfigureImage(contentGameObject.GetComponentInChildren<Image>());
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentInChildren<Image>().GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
	}
}
