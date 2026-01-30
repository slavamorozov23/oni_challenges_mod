using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200085E RID: 2142
public static class SequenceTools
{
	// Token: 0x06003AD8 RID: 15064 RVA: 0x00148830 File Offset: 0x00146A30
	public static WaitUntil Interpolate(this MonoBehaviour owner, Action<float> action, float duration, System.Action then = null)
	{
		Coroutine coroutine;
		return owner.Interpolate(action, duration, out coroutine, then);
	}

	// Token: 0x06003AD9 RID: 15065 RVA: 0x00148848 File Offset: 0x00146A48
	public static WaitUntil Interpolate(this MonoBehaviour owner, Action<float> action, float duration, out Coroutine coroutineOut, System.Action then = null)
	{
		bool completed = false;
		System.Action then2 = delegate()
		{
			if (then != null)
			{
				then();
			}
			completed = true;
		};
		coroutineOut = owner.StartCoroutine(SequenceTools.InterpolateCoroutineLogic(action, duration, then2));
		return new WaitUntil(() => completed);
	}

	// Token: 0x06003ADA RID: 15066 RVA: 0x00148896 File Offset: 0x00146A96
	private static IEnumerator InterpolateCoroutineLogic(Action<float> action, float duration, System.Action then)
	{
		float timer = 0f;
		while (timer < duration)
		{
			float obj = timer / duration;
			action(obj);
			timer += Time.unscaledDeltaTime;
			yield return null;
		}
		action(1f);
		yield return null;
		if (then != null)
		{
			then();
		}
		yield break;
	}

	// Token: 0x06003ADB RID: 15067 RVA: 0x001488B4 File Offset: 0x00146AB4
	public static void TextEraser(LocText label, string text, float progress)
	{
		string text2 = text.Substring(0, Mathf.CeilToInt((float)text.Length * (1f - progress)));
		label.SetText(text2);
		label.ForceMeshUpdate();
	}

	// Token: 0x06003ADC RID: 15068 RVA: 0x001488EC File Offset: 0x00146AEC
	public static void TextWriter(LocText label, string text, float progress)
	{
		string text2 = (progress == 1f) ? text : text.Substring(0, Mathf.CeilToInt((float)text.Length * progress));
		label.SetText(text2);
		label.ForceMeshUpdate();
	}
}
