using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C72 RID: 3186
public static class ResearchButtonImageToggleStateUtilityFunctions
{
	// Token: 0x0600611E RID: 24862 RVA: 0x0023B444 File Offset: 0x00239644
	public static void Opacity(this Graphic graphic, float opacity)
	{
		Color color = graphic.color;
		color.a = opacity;
		graphic.color = color;
	}

	// Token: 0x0600611F RID: 24863 RVA: 0x0023B468 File Offset: 0x00239668
	public static WaitUntil FadeAway(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(0f);
				return true;
			}
			float num = timer / duration;
			num = 1f - num;
			graphic.Opacity(startingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}

	// Token: 0x06006120 RID: 24864 RVA: 0x0023B4C0 File Offset: 0x002396C0
	public static WaitUntil FadeToVisible(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		float remainingOpacity = 1f - graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(1f);
				return true;
			}
			float num = timer / duration;
			graphic.Opacity(startingOpacity + remainingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}
}
