using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public static class PresUtil
{
	// Token: 0x06001835 RID: 6197 RVA: 0x00087DA8 File Offset: 0x00085FA8
	public static Promise MoveAndFade(RectTransform rect, Vector2 targetAnchoredPosition, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		CanvasGroup canvasGroup = rect.FindOrAddComponent<CanvasGroup>();
		return rect.FindOrAddComponent<CoroutineRunner>().Run(Updater.Parallel(new Updater[]
		{
			Updater.Ease(delegate(float f)
			{
				canvasGroup.alpha = f;
			}, canvasGroup.alpha, targetAlpha, duration, easing, -1f),
			Updater.Ease(delegate(Vector2 v2)
			{
				rect.anchoredPosition = v2;
			}, rect.anchoredPosition, targetAnchoredPosition, duration, easing, -1f)
		}));
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x00087E4C File Offset: 0x0008604C
	public static Promise OffsetFromAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		return PresUtil.MoveAndFade(rect, offset + anchoredPosition, targetAlpha, duration, easing);
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x00087E74 File Offset: 0x00086074
	public static Promise OffsetToAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		rect.anchoredPosition = offset + anchoredPosition;
		return PresUtil.MoveAndFade(rect, anchoredPosition, targetAlpha, duration, easing);
	}
}
