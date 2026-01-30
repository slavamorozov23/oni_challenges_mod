using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000CC2 RID: 3266
public class CodexImageLayoutMB : UIBehaviour
{
	// Token: 0x060064B7 RID: 25783 RVA: 0x0025ECCC File Offset: 0x0025CECC
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (this.image.preserveAspect && this.image.sprite != null && this.image.sprite)
		{
			float num = this.image.sprite.rect.height / this.image.sprite.rect.width;
			this.layoutElement.preferredHeight = num * this.rectTransform.sizeDelta.x;
			this.layoutElement.minHeight = this.layoutElement.preferredHeight;
			return;
		}
		this.layoutElement.preferredHeight = -1f;
		this.layoutElement.preferredWidth = -1f;
		this.layoutElement.minHeight = -1f;
		this.layoutElement.minWidth = -1f;
		this.layoutElement.flexibleHeight = -1f;
		this.layoutElement.flexibleWidth = -1f;
		this.layoutElement.ignoreLayout = false;
	}

	// Token: 0x04004444 RID: 17476
	public RectTransform rectTransform;

	// Token: 0x04004445 RID: 17477
	public LayoutElement layoutElement;

	// Token: 0x04004446 RID: 17478
	public Image image;
}
