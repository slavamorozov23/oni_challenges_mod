using System;
using UnityEngine;

// Token: 0x020009CE RID: 2510
public class UIShake : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x060048E4 RID: 18660 RVA: 0x001A616B File Offset: 0x001A436B
	public float Intensity
	{
		get
		{
			return this.intensity;
		}
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x001A6174 File Offset: 0x001A4374
	public void RenderEveryTick(float dt)
	{
		if (this.intensity != 0f || this.lastIntensity != 0f)
		{
			this.lastIntensity = this.intensity;
			Vector2 b = new Vector2(UnityEngine.Random.Range(-1f, 1f) * this.MaxOffsets.x * this.intensity, UnityEngine.Random.Range(-1f, 1f) * this.MaxOffsets.y * this.intensity);
			Vector2 anchoredPosition = this.initialLocalPosition + b;
			this.transform.anchoredPosition = anchoredPosition;
		}
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x001A620B File Offset: 0x001A440B
	public void SetIntensity(float intensity)
	{
		this.intensity = intensity;
	}

	// Token: 0x060048E7 RID: 18663 RVA: 0x001A6214 File Offset: 0x001A4414
	protected override void OnPrefabInit()
	{
		this.transform = (base.transform as RectTransform);
		this.initialLocalPosition = this.transform.anchoredPosition;
	}

	// Token: 0x04003077 RID: 12407
	public Vector2 MaxOffsets = Vector2.one;

	// Token: 0x04003078 RID: 12408
	private float lastIntensity;

	// Token: 0x04003079 RID: 12409
	private float intensity;

	// Token: 0x0400307A RID: 12410
	private Vector2 initialLocalPosition;

	// Token: 0x0400307B RID: 12411
	private new RectTransform transform;
}
