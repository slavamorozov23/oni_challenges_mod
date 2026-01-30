using System;

// Token: 0x02000B23 RID: 2851
public class RunningAverage
{
	// Token: 0x06005354 RID: 21332 RVA: 0x001E5B1E File Offset: 0x001E3D1E
	public RunningAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 15, bool allowZero = true)
	{
		this.samples = new RingBuffer<float>(sampleCount, float.NaN);
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06005355 RID: 21333 RVA: 0x001E5B50 File Offset: 0x001E3D50
	public float AverageValue
	{
		get
		{
			return this.GetAverage();
		}
	}

	// Token: 0x06005356 RID: 21334 RVA: 0x001E5B58 File Offset: 0x001E3D58
	public void AddSample(float value)
	{
		if (value < this.min || value > this.max || (this.ignoreZero && value == 0f))
		{
			return;
		}
		this.samples.Add(value);
	}

	// Token: 0x06005357 RID: 21335 RVA: 0x001E5B8C File Offset: 0x001E3D8C
	private float GetAverage()
	{
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < this.samples.Count; i++)
		{
			float num3 = this.samples[i];
			if (num3 != float.NaN)
			{
				num += num3;
				num2++;
			}
		}
		if (num2 == 0)
		{
			return float.NaN;
		}
		return num / (float)num2;
	}

	// Token: 0x04003866 RID: 14438
	private RingBuffer<float> samples;

	// Token: 0x04003867 RID: 14439
	private float min;

	// Token: 0x04003868 RID: 14440
	private float max;

	// Token: 0x04003869 RID: 14441
	private bool ignoreZero;
}
