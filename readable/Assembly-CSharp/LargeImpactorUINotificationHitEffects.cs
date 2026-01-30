using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B8B RID: 2955
public class LargeImpactorUINotificationHitEffects : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06005832 RID: 22578 RVA: 0x002008F6 File Offset: 0x001FEAF6
	private float Intensity
	{
		get
		{
			return this.timer / this.duration;
		}
	}

	// Token: 0x06005833 RID: 22579 RVA: 0x00200905 File Offset: 0x001FEB05
	public void PlayHitEffect()
	{
		this.timer = this.duration;
	}

	// Token: 0x06005834 RID: 22580 RVA: 0x00200914 File Offset: 0x001FEB14
	public void RenderEveryTick(float dt)
	{
		if (this.lastTimerValue != this.timer)
		{
			this.lastTimerValue = this.timer;
			this.hitBackgorund.Opacity(this.Intensity);
			this.heartIcon.color = Color.Lerp(this.heartIconOriginalColor, this.HighlightedColor, this.Intensity);
			this.healthBarFill.color = Color.Lerp(this.healthBarOriginalColor, this.HighlightedColor, this.Intensity);
			this.shake.SetIntensity(this.Intensity);
		}
		this.timer = Mathf.Clamp(this.timer - dt, 0f, this.duration);
	}

	// Token: 0x06005835 RID: 22581 RVA: 0x002009BF File Offset: 0x001FEBBF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.heartIconOriginalColor = this.heartIcon.color;
		this.healthBarOriginalColor = this.healthBarFill.color;
	}

	// Token: 0x04003B3D RID: 15165
	public Image hitBackgorund;

	// Token: 0x04003B3E RID: 15166
	public Image heartIcon;

	// Token: 0x04003B3F RID: 15167
	public Image healthBarFill;

	// Token: 0x04003B40 RID: 15168
	public UIShake shake;

	// Token: 0x04003B41 RID: 15169
	public Color HighlightedColor = Color.yellow;

	// Token: 0x04003B42 RID: 15170
	private Color heartIconOriginalColor;

	// Token: 0x04003B43 RID: 15171
	private Color healthBarOriginalColor;

	// Token: 0x04003B44 RID: 15172
	private float duration = 0.4f;

	// Token: 0x04003B45 RID: 15173
	private float lastTimerValue;

	// Token: 0x04003B46 RID: 15174
	private float timer;
}
