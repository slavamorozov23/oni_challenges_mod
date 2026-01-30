using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D2A RID: 3370
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class ImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x0600681D RID: 26653 RVA: 0x00274C2C File Offset: 0x00272E2C
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.sprite != null)
		{
			base.aspectRatio = this.targetImage.sprite.rect.width / this.targetImage.sprite.rect.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x0600681E RID: 26654 RVA: 0x00274C9D File Offset: 0x00272E9D
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x0600681F RID: 26655 RVA: 0x00274CAB File Offset: 0x00272EAB
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04004783 RID: 18307
	[SerializeField]
	private Image targetImage;
}
