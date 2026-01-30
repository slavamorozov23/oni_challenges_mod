using System;
using UnityEngine;

// Token: 0x02000DBE RID: 3518
public class NonLinearSlider : KSlider
{
	// Token: 0x06006DDD RID: 28125 RVA: 0x00299E69 File Offset: 0x00298069
	public static NonLinearSlider.Range[] GetDefaultRange(float maxValue)
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(100f, maxValue)
		};
	}

	// Token: 0x06006DDE RID: 28126 RVA: 0x00299E83 File Offset: 0x00298083
	protected override void Start()
	{
		base.Start();
		base.minValue = 0f;
		base.maxValue = 100f;
	}

	// Token: 0x06006DDF RID: 28127 RVA: 0x00299EA1 File Offset: 0x002980A1
	public void SetRanges(NonLinearSlider.Range[] ranges)
	{
		this.ranges = ranges;
	}

	// Token: 0x06006DE0 RID: 28128 RVA: 0x00299EAC File Offset: 0x002980AC
	public float GetPercentageFromValue(float value)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (value >= num2 && value <= this.ranges[i].peakValue)
			{
				float t = (value - num2) / (this.ranges[i].peakValue - num2);
				return Mathf.Lerp(num, num + this.ranges[i].width, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return 100f;
	}

	// Token: 0x06006DE1 RID: 28129 RVA: 0x00299F50 File Offset: 0x00298150
	public float GetValueForPercentage(float percentage)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (percentage >= num && num + this.ranges[i].width >= percentage)
			{
				float t = (percentage - num) / this.ranges[i].width;
				return Mathf.Lerp(num2, this.ranges[i].peakValue, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return num2;
	}

	// Token: 0x06006DE2 RID: 28130 RVA: 0x00299FEC File Offset: 0x002981EC
	protected override void Set(float input, bool sendCallback)
	{
		base.Set(input, sendCallback);
	}

	// Token: 0x04004B06 RID: 19206
	public NonLinearSlider.Range[] ranges;

	// Token: 0x0200200E RID: 8206
	[Serializable]
	public struct Range
	{
		// Token: 0x0600B821 RID: 47137 RVA: 0x003F4642 File Offset: 0x003F2842
		public Range(float width, float peakValue)
		{
			this.width = width;
			this.peakValue = peakValue;
		}

		// Token: 0x040094B4 RID: 38068
		public float width;

		// Token: 0x040094B5 RID: 38069
		public float peakValue;
	}
}
