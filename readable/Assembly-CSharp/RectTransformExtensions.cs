using System;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public static class RectTransformExtensions
{
	// Token: 0x06001855 RID: 6229 RVA: 0x00088180 File Offset: 0x00086380
	public static RectTransform Fill(this RectTransform rectTransform)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		return rectTransform;
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000881E4 File Offset: 0x000863E4
	public static RectTransform Fill(this RectTransform rectTransform, Padding padding)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(padding.left, padding.bottom);
		rectTransform.sizeDelta = new Vector2(-padding.right, -padding.top);
		return rectTransform;
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x0008824C File Offset: 0x0008644C
	public static RectTransform Pivot(this RectTransform rectTransform, float x, float y)
	{
		rectTransform.pivot = new Vector2(x, y);
		return rectTransform;
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x0008825C File Offset: 0x0008645C
	public static RectTransform Pivot(this RectTransform rectTransform, Vector2 pivot)
	{
		rectTransform.pivot = pivot;
		return rectTransform;
	}
}
