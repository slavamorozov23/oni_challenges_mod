using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E0B RID: 3595
public class ShadowRect : MonoBehaviour
{
	// Token: 0x060071DE RID: 29150 RVA: 0x002B7F20 File Offset: 0x002B6120
	private void OnEnable()
	{
		if (this.RectShadow != null)
		{
			this.RectShadow.name = "Shadow_" + this.RectMain.name;
			this.MatchRect();
			return;
		}
		global::Debug.LogWarning("Shadowrect is missing rectshadow: " + base.gameObject.name);
	}

	// Token: 0x060071DF RID: 29151 RVA: 0x002B7F7C File Offset: 0x002B617C
	private void Update()
	{
		this.MatchRect();
	}

	// Token: 0x060071E0 RID: 29152 RVA: 0x002B7F84 File Offset: 0x002B6184
	protected virtual void MatchRect()
	{
		if (this.RectShadow == null || this.RectMain == null)
		{
			return;
		}
		if (this.shadowLayoutElement == null)
		{
			this.shadowLayoutElement = this.RectShadow.GetComponent<LayoutElement>();
		}
		if (this.shadowLayoutElement != null && !this.shadowLayoutElement.ignoreLayout)
		{
			this.shadowLayoutElement.ignoreLayout = true;
		}
		if (this.RectShadow.transform.parent != this.RectMain.transform.parent)
		{
			this.RectShadow.transform.SetParent(this.RectMain.transform.parent);
		}
		if (this.RectShadow.GetSiblingIndex() >= this.RectMain.GetSiblingIndex())
		{
			this.RectShadow.SetAsFirstSibling();
		}
		this.RectShadow.transform.localScale = Vector3.one;
		if (this.RectShadow.pivot != this.RectMain.pivot)
		{
			this.RectShadow.pivot = this.RectMain.pivot;
		}
		if (this.RectShadow.anchorMax != this.RectMain.anchorMax)
		{
			this.RectShadow.anchorMax = this.RectMain.anchorMax;
		}
		if (this.RectShadow.anchorMin != this.RectMain.anchorMin)
		{
			this.RectShadow.anchorMin = this.RectMain.anchorMin;
		}
		if (this.RectShadow.sizeDelta != this.RectMain.sizeDelta)
		{
			this.RectShadow.sizeDelta = this.RectMain.sizeDelta;
		}
		if (this.RectShadow.anchoredPosition != this.RectMain.anchoredPosition + this.ShadowOffset)
		{
			this.RectShadow.anchoredPosition = this.RectMain.anchoredPosition + this.ShadowOffset;
		}
		if (this.RectMain.gameObject.activeInHierarchy != this.RectShadow.gameObject.activeInHierarchy)
		{
			this.RectShadow.gameObject.SetActive(this.RectMain.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x04004EA0 RID: 20128
	public RectTransform RectMain;

	// Token: 0x04004EA1 RID: 20129
	public RectTransform RectShadow;

	// Token: 0x04004EA2 RID: 20130
	[SerializeField]
	protected Color shadowColor = new Color(0f, 0f, 0f, 0.6f);

	// Token: 0x04004EA3 RID: 20131
	[SerializeField]
	protected Vector2 ShadowOffset = new Vector2(1.5f, -1.5f);

	// Token: 0x04004EA4 RID: 20132
	private LayoutElement shadowLayoutElement;
}
