using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CA4 RID: 3236
public class ExpandRevealUIContent : MonoBehaviour
{
	// Token: 0x06006303 RID: 25347 RVA: 0x0024B35C File Offset: 0x0024955C
	private void OnDisable()
	{
		if (this.BGChildFitter)
		{
			this.BGChildFitter.WidthScale = (this.BGChildFitter.HeightScale = 0f);
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = 0f;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = 0f;
			}
		}
		if (this.BGRectStretcher)
		{
			this.BGRectStretcher.XStretchFactor = (this.BGRectStretcher.YStretchFactor = 0f);
			this.BGRectStretcher.UpdateStretching();
		}
		if (this.MaskRectStretcher)
		{
			this.MaskRectStretcher.XStretchFactor = (this.MaskRectStretcher.YStretchFactor = 0f);
			this.MaskRectStretcher.UpdateStretching();
		}
	}

	// Token: 0x06006304 RID: 25348 RVA: 0x0024B448 File Offset: 0x00249648
	public void Expand(Action<object> completeCallback)
	{
		if (this.MaskChildFitter && this.MaskRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a MaskChildFitter and a MaskRectStretcher. It should have only one or the other. ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.BGChildFitter && this.BGRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a BGChildFitter and a BGRectStretcher . It should have only one or the other.  ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.activeRoutine != null)
		{
			base.StopCoroutine(this.activeRoutine);
		}
		this.CollapsedImmediate();
		this.activeRoutineCompleteCallback = completeCallback;
		this.activeRoutine = base.StartCoroutine(this.ExpandRoutine(null));
	}

	// Token: 0x06006305 RID: 25349 RVA: 0x0024B4D4 File Offset: 0x002496D4
	public void Collapse(Action<object> completeCallback)
	{
		if (this.activeRoutine != null)
		{
			if (this.activeRoutineCompleteCallback != null)
			{
				this.activeRoutineCompleteCallback(null);
			}
			base.StopCoroutine(this.activeRoutine);
		}
		this.activeRoutineCompleteCallback = completeCallback;
		if (base.gameObject.activeInHierarchy)
		{
			this.activeRoutine = base.StartCoroutine(this.CollapseRoutine(completeCallback));
			return;
		}
		this.activeRoutine = null;
		if (completeCallback != null)
		{
			completeCallback(null);
		}
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x0024B542 File Offset: 0x00249742
	private IEnumerator ExpandRoutine(Action<object> completeCallback)
	{
		this.Collapsing = false;
		this.Expanding = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.expandAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num / this.speedScale;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime * this.speedScale)
		{
			this.SetStretch(this.expandAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.expandAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Expanding = false;
		yield break;
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x0024B558 File Offset: 0x00249758
	private void SetStretch(float value)
	{
		if (this.BGRectStretcher)
		{
			if (this.BGRectStretcher.StretchX)
			{
				this.BGRectStretcher.XStretchFactor = value;
			}
			if (this.BGRectStretcher.StretchY)
			{
				this.BGRectStretcher.YStretchFactor = value;
			}
		}
		if (this.MaskRectStretcher)
		{
			if (this.MaskRectStretcher.StretchX)
			{
				this.MaskRectStretcher.XStretchFactor = value;
			}
			if (this.MaskRectStretcher.StretchY)
			{
				this.MaskRectStretcher.YStretchFactor = value;
			}
		}
		if (this.BGChildFitter)
		{
			if (this.BGChildFitter.fitWidth)
			{
				this.BGChildFitter.WidthScale = value;
			}
			if (this.BGChildFitter.fitHeight)
			{
				this.BGChildFitter.HeightScale = value;
			}
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = value;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = value;
			}
		}
	}

	// Token: 0x06006308 RID: 25352 RVA: 0x0024B661 File Offset: 0x00249861
	private IEnumerator CollapseRoutine(Action<object> completeCallback)
	{
		this.Expanding = false;
		this.Collapsing = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.collapseAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime)
		{
			this.SetStretch(this.collapseAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.collapseAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Collapsing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06006309 RID: 25353 RVA: 0x0024B678 File Offset: 0x00249878
	public void CollapsedImmediate()
	{
		float time = (float)this.collapseAnimation.length;
		this.SetStretch(this.collapseAnimation.Evaluate(time));
	}

	// Token: 0x0400430E RID: 17166
	private Coroutine activeRoutine;

	// Token: 0x0400430F RID: 17167
	private Action<object> activeRoutineCompleteCallback;

	// Token: 0x04004310 RID: 17168
	public AnimationCurve expandAnimation;

	// Token: 0x04004311 RID: 17169
	public AnimationCurve collapseAnimation;

	// Token: 0x04004312 RID: 17170
	public KRectStretcher MaskRectStretcher;

	// Token: 0x04004313 RID: 17171
	public KRectStretcher BGRectStretcher;

	// Token: 0x04004314 RID: 17172
	public KChildFitter MaskChildFitter;

	// Token: 0x04004315 RID: 17173
	public KChildFitter BGChildFitter;

	// Token: 0x04004316 RID: 17174
	public float speedScale = 1f;

	// Token: 0x04004317 RID: 17175
	public bool Collapsing;

	// Token: 0x04004318 RID: 17176
	public bool Expanding;
}
