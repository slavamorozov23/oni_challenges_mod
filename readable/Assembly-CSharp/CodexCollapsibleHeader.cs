using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CD7 RID: 3287
public class CodexCollapsibleHeader : CodexWidget<CodexCollapsibleHeader>
{
	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x0600656F RID: 25967 RVA: 0x002635CF File Offset: 0x002617CF
	// (set) Token: 0x06006570 RID: 25968 RVA: 0x002635F6 File Offset: 0x002617F6
	protected GameObject ContentsGameObject
	{
		get
		{
			if (this.contentsGameObject == null)
			{
				this.contentsGameObject = this.contents.go;
			}
			return this.contentsGameObject;
		}
		set
		{
			this.contentsGameObject = value;
		}
	}

	// Token: 0x06006571 RID: 25969 RVA: 0x002635FF File Offset: 0x002617FF
	public CodexCollapsibleHeader(string label, ContentContainer contents)
	{
		this.label = label;
		this.contents = contents;
	}

	// Token: 0x06006572 RID: 25970 RVA: 0x00263618 File Offset: 0x00261818
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("Label");
		reference.text = this.label;
		reference.textStyleSetting = textStyles[CodexTextStyle.Subtitle];
		reference.ApplySettings();
		MultiToggle reference2 = component.GetReference<MultiToggle>("ExpandToggle");
		reference2.ChangeState(1);
		reference2.onClick = delegate()
		{
			this.ToggleCategoryOpen(contentGameObject, !this.ContentsGameObject.activeSelf);
		};
	}

	// Token: 0x06006573 RID: 25971 RVA: 0x0026368F File Offset: 0x0026188F
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		this.ContentsGameObject.SetActive(open);
	}

	// Token: 0x040044C9 RID: 17609
	protected ContentContainer contents;

	// Token: 0x040044CA RID: 17610
	private string label;

	// Token: 0x040044CB RID: 17611
	private GameObject contentsGameObject;
}
