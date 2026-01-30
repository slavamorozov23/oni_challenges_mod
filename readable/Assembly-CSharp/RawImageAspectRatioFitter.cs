using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE8 RID: 3560
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class RawImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x06007007 RID: 28679 RVA: 0x002A8DE4 File Offset: 0x002A6FE4
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.texture != null)
		{
			base.aspectRatio = (float)this.targetImage.texture.width / (float)this.targetImage.texture.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x06007008 RID: 28680 RVA: 0x002A8E47 File Offset: 0x002A7047
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x06007009 RID: 28681 RVA: 0x002A8E55 File Offset: 0x002A7055
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04004CD7 RID: 19671
	[SerializeField]
	private RawImage targetImage;
}
