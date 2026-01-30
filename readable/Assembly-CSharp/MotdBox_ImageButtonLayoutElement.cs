using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB4 RID: 3508
public class MotdBox_ImageButtonLayoutElement : LayoutElement
{
	// Token: 0x06006D7A RID: 28026 RVA: 0x00297A40 File Offset: 0x00295C40
	private void UpdateState()
	{
		MotdBox_ImageButtonLayoutElement.Style style = this.style;
		if (style == MotdBox_ImageButtonLayoutElement.Style.WidthExpandsBasedOnHeight)
		{
			this.flexibleHeight = 1f;
			this.preferredHeight = -1f;
			this.minHeight = -1f;
			this.flexibleWidth = 0f;
			this.preferredWidth = this.rectTransform().sizeDelta.y * this.heightToWidthRatio;
			this.minWidth = this.preferredWidth;
			this.ignoreLayout = false;
			return;
		}
		if (style != MotdBox_ImageButtonLayoutElement.Style.HeightExpandsBasedOnWidth)
		{
			return;
		}
		this.flexibleWidth = 1f;
		this.preferredWidth = -1f;
		this.minWidth = -1f;
		this.flexibleHeight = 0f;
		this.preferredHeight = this.rectTransform().sizeDelta.x / this.heightToWidthRatio;
		this.minHeight = this.preferredHeight;
		this.ignoreLayout = false;
	}

	// Token: 0x06006D7B RID: 28027 RVA: 0x00297B15 File Offset: 0x00295D15
	protected override void OnTransformParentChanged()
	{
		this.UpdateState();
		base.OnTransformParentChanged();
	}

	// Token: 0x06006D7C RID: 28028 RVA: 0x00297B23 File Offset: 0x00295D23
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateState();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04004ACE RID: 19150
	[SerializeField]
	private float heightToWidthRatio;

	// Token: 0x04004ACF RID: 19151
	[SerializeField]
	private MotdBox_ImageButtonLayoutElement.Style style;

	// Token: 0x02002004 RID: 8196
	private enum Style
	{
		// Token: 0x0400948C RID: 38028
		WidthExpandsBasedOnHeight,
		// Token: 0x0400948D RID: 38029
		HeightExpandsBasedOnWidth
	}
}
