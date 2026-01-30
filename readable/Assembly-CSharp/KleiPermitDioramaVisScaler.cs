using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D4D RID: 3405
[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour
{
	// Token: 0x06006981 RID: 27009 RVA: 0x0027F8B0 File Offset: 0x0027DAB0
	protected override void OnRectTransformDimensionsChange()
	{
		this.Layout();
	}

	// Token: 0x06006982 RID: 27010 RVA: 0x0027F8B8 File Offset: 0x0027DAB8
	public void Layout()
	{
		KleiPermitDioramaVisScaler.Layout(this.root, this.scaleTarget, this.slot);
	}

	// Token: 0x06006983 RID: 27011 RVA: 0x0027F8D4 File Offset: 0x0027DAD4
	public static void Layout(RectTransform root, RectTransform scaleTarget, RectTransform slot)
	{
		float aspectRatio = 2.125f;
		AspectRatioFitter aspectRatioFitter = slot.FindOrAddComponent<AspectRatioFitter>();
		aspectRatioFitter.aspectRatio = aspectRatio;
		aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
		float num = 128f;
		float num2 = 128f;
		float num3 = 1700f;
		float a = Mathf.Max(0.1f, root.rect.width - num) / num3;
		float num4 = 800f;
		float b = Mathf.Max(0.1f, root.rect.height - num2) / num4;
		float d = Mathf.Max(a, b);
		scaleTarget.localScale = Vector3.one * d;
		scaleTarget.sizeDelta = new Vector2(1700f, 800f);
		scaleTarget.anchorMin = Vector2.one * 0.5f;
		scaleTarget.anchorMax = Vector2.one * 0.5f;
		scaleTarget.pivot = Vector2.one * 0.5f;
		scaleTarget.anchoredPosition = Vector2.zero;
	}

	// Token: 0x0400488A RID: 18570
	public const float REFERENCE_WIDTH = 1700f;

	// Token: 0x0400488B RID: 18571
	public const float REFERENCE_HEIGHT = 800f;

	// Token: 0x0400488C RID: 18572
	[SerializeField]
	private RectTransform root;

	// Token: 0x0400488D RID: 18573
	[SerializeField]
	private RectTransform scaleTarget;

	// Token: 0x0400488E RID: 18574
	[SerializeField]
	private RectTransform slot;
}
