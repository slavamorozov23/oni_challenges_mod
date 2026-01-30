using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E0A RID: 3594
public class ShadowImage : ShadowRect
{
	// Token: 0x060071DC RID: 29148 RVA: 0x002B7DEC File Offset: 0x002B5FEC
	protected override void MatchRect()
	{
		base.MatchRect();
		if (this.RectMain == null || this.RectShadow == null)
		{
			return;
		}
		if (this.shadowImage == null)
		{
			this.shadowImage = this.RectShadow.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			this.mainImage = this.RectMain.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			if (this.shadowImage != null)
			{
				this.shadowImage.color = Color.clear;
			}
			return;
		}
		if (this.shadowImage == null)
		{
			return;
		}
		if (this.shadowImage.sprite != this.mainImage.sprite)
		{
			this.shadowImage.sprite = this.mainImage.sprite;
		}
		if (this.shadowImage.color != this.shadowColor)
		{
			if (this.shadowImage.sprite != null)
			{
				this.shadowImage.color = this.shadowColor;
				return;
			}
			this.shadowImage.color = Color.clear;
		}
	}

	// Token: 0x04004E9E RID: 20126
	private Image shadowImage;

	// Token: 0x04004E9F RID: 20127
	private Image mainImage;
}
