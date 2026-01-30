using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CA3 RID: 3235
public class EasingAnimations : MonoBehaviour
{
	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x060062F9 RID: 25337 RVA: 0x0024B0C0 File Offset: 0x002492C0
	public bool IsPlaying
	{
		get
		{
			return this.animationCoroutine != null;
		}
	}

	// Token: 0x060062FA RID: 25338 RVA: 0x0024B0CB File Offset: 0x002492CB
	private void Start()
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
	}

	// Token: 0x060062FB RID: 25339 RVA: 0x0024B0E8 File Offset: 0x002492E8
	private void Initialize()
	{
		this.animationMap = new Dictionary<string, EasingAnimations.AnimationScales>();
		foreach (EasingAnimations.AnimationScales animationScales in this.scales)
		{
			this.animationMap.Add(animationScales.name, animationScales);
		}
	}

	// Token: 0x060062FC RID: 25340 RVA: 0x0024B130 File Offset: 0x00249330
	public void PlayAnimation(string animationName, float delay = 0f)
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
		if (!this.animationMap.ContainsKey(animationName))
		{
			return;
		}
		if (this.animationCoroutine != null)
		{
			base.StopCoroutine(this.animationCoroutine);
		}
		this.currentAnimation = this.animationMap[animationName];
		this.currentAnimation.currentScale = this.currentAnimation.startScale;
		base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
		this.animationCoroutine = base.StartCoroutine(this.ExecuteAnimation(delay));
	}

	// Token: 0x060062FD RID: 25341 RVA: 0x0024B1D6 File Offset: 0x002493D6
	private IEnumerator ExecuteAnimation(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startTime + delay)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		startTime = Time.realtimeSinceStartup;
		bool keepAnimating = true;
		while (keepAnimating)
		{
			float num = Time.realtimeSinceStartup - startTime;
			this.currentAnimation.currentScale = this.GetEasing(num * this.currentAnimation.easingMultiplier);
			if (this.currentAnimation.endScale > this.currentAnimation.startScale)
			{
				keepAnimating = (this.currentAnimation.currentScale < this.currentAnimation.endScale - 0.025f);
			}
			else
			{
				keepAnimating = (this.currentAnimation.currentScale > this.currentAnimation.endScale + 0.025f);
			}
			if (!keepAnimating)
			{
				this.currentAnimation.currentScale = this.currentAnimation.endScale;
			}
			base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.animationCoroutine = null;
		if (this.OnAnimationDone != null)
		{
			this.OnAnimationDone(this.currentAnimation.name);
		}
		yield break;
	}

	// Token: 0x060062FE RID: 25342 RVA: 0x0024B1EC File Offset: 0x002493EC
	private float GetEasing(float t)
	{
		EasingAnimations.AnimationScales.AnimationType type = this.currentAnimation.type;
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseOutBack)
		{
			return this.EaseOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseInBack)
		{
			return this.EaseInBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		return this.EaseInOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
	}

	// Token: 0x060062FF RID: 25343 RVA: 0x0024B268 File Offset: 0x00249468
	public float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x06006300 RID: 25344 RVA: 0x0024B2E4 File Offset: 0x002494E4
	public float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x06006301 RID: 25345 RVA: 0x0024B318 File Offset: 0x00249518
	public float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x04004309 RID: 17161
	public EasingAnimations.AnimationScales[] scales;

	// Token: 0x0400430A RID: 17162
	private EasingAnimations.AnimationScales currentAnimation;

	// Token: 0x0400430B RID: 17163
	private Coroutine animationCoroutine;

	// Token: 0x0400430C RID: 17164
	private Dictionary<string, EasingAnimations.AnimationScales> animationMap;

	// Token: 0x0400430D RID: 17165
	public Action<string> OnAnimationDone;

	// Token: 0x02001ECC RID: 7884
	[Serializable]
	public struct AnimationScales
	{
		// Token: 0x0400909F RID: 37023
		public string name;

		// Token: 0x040090A0 RID: 37024
		public float startScale;

		// Token: 0x040090A1 RID: 37025
		public float endScale;

		// Token: 0x040090A2 RID: 37026
		public EasingAnimations.AnimationScales.AnimationType type;

		// Token: 0x040090A3 RID: 37027
		public float easingMultiplier;

		// Token: 0x040090A4 RID: 37028
		[HideInInspector]
		public float currentScale;

		// Token: 0x02002A70 RID: 10864
		public enum AnimationType
		{
			// Token: 0x0400BB64 RID: 47972
			EaseInOutBack,
			// Token: 0x0400BB65 RID: 47973
			EaseOutBack,
			// Token: 0x0400BB66 RID: 47974
			EaseInBack
		}
	}
}
