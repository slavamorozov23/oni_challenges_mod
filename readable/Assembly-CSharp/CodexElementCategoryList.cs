using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CDA RID: 3290
public class CodexElementCategoryList : CodexCollapsibleHeader
{
	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x06006581 RID: 25985 RVA: 0x0026381E File Offset: 0x00261A1E
	// (set) Token: 0x06006582 RID: 25986 RVA: 0x00263826 File Offset: 0x00261A26
	public Tag categoryTag { get; set; }

	// Token: 0x06006583 RID: 25987 RVA: 0x0026382F File Offset: 0x00261A2F
	public CodexElementCategoryList() : base(UI.CODEX.CATEGORYNAMES.ELEMENTS, null)
	{
	}

	// Token: 0x06006584 RID: 25988 RVA: 0x00263850 File Offset: 0x00261A50
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		base.ContentsGameObject = component.GetReference<RectTransform>("ContentContainer").gameObject;
		base.Configure(contentGameObject, displayPane, textStyles);
		Component reference = component.GetReference<RectTransform>("HeaderLabel");
		RectTransform reference2 = component.GetReference<RectTransform>("PrefabLabelWithIcon");
		this.ClearPanel(reference2.transform.parent, reference2);
		reference.GetComponent<LocText>().SetText(UI.CODEX.CATEGORYNAMES.ELEMENTS);
		foreach (GameObject gameObject in Assets.GetPrefabsWithTag(this.categoryTag))
		{
			GameObject gameObject2 = Util.KInstantiateUI(reference2.gameObject, reference2.parent.gameObject, true);
			Image componentInChildren = gameObject2.GetComponentInChildren<Image>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(gameObject, "ui", false);
			componentInChildren.sprite = uisprite.first;
			componentInChildren.color = uisprite.second;
			gameObject2.GetComponentInChildren<LocText>().SetText(gameObject.GetProperName());
			this.rows.Add(gameObject2);
		}
	}

	// Token: 0x06006585 RID: 25989 RVA: 0x0026396C File Offset: 0x00261B6C
	private void ClearPanel(Transform containerToClear, Transform skipDestroyingPrefab)
	{
		skipDestroyingPrefab.SetAsFirstSibling();
		for (int i = containerToClear.childCount - 1; i >= 1; i--)
		{
			UnityEngine.Object.Destroy(containerToClear.GetChild(i).gameObject);
		}
		for (int j = this.rows.Count - 1; j >= 0; j--)
		{
			UnityEngine.Object.Destroy(this.rows[j].gameObject);
		}
		this.rows.Clear();
	}

	// Token: 0x040044D0 RID: 17616
	private List<GameObject> rows = new List<GameObject>();
}
