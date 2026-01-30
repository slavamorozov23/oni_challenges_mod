using System;
using UnityEngine;

// Token: 0x02000B24 RID: 2852
public class RunningWeightedAverage
{
	// Token: 0x06005358 RID: 21336 RVA: 0x001E5BE4 File Offset: 0x001E3DE4
	public RunningWeightedAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 20, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new RingBuffer<RunningWeightedAverage.Entry>(sampleCount, new RunningWeightedAverage.Entry
		{
			time = float.NaN,
			value = float.NaN
		});
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06005359 RID: 21337 RVA: 0x001E5C3D File Offset: 0x001E3E3D
	public float GetUnweightedAverage
	{
		get
		{
			return this.GetAverageOfLastSeconds(4f);
		}
	}

	// Token: 0x0600535A RID: 21338 RVA: 0x001E5C4C File Offset: 0x001E3E4C
	public void AddSample(float value, float timeOfRecord)
	{
		if (this.ignoreZero && value == 0f)
		{
			return;
		}
		if (value > this.max)
		{
			value = this.max;
		}
		if (value < this.min)
		{
			value = this.min;
		}
		this.samples.Add(new RunningWeightedAverage.Entry
		{
			time = timeOfRecord,
			value = value
		});
	}

	// Token: 0x0600535B RID: 21339 RVA: 0x001E5CB0 File Offset: 0x001E3EB0
	public int ValidRecordsInLastSeconds(float seconds)
	{
		int num = 0;
		float time = Time.time;
		for (int i = 0; i < this.samples.Count; i++)
		{
			RunningWeightedAverage.Entry entry = this.samples[i];
			if (entry.time != float.NaN && time - entry.time <= seconds)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600535C RID: 21340 RVA: 0x001E5D08 File Offset: 0x001E3F08
	private float GetAverageOfLastSeconds(float seconds)
	{
		float num = 0f;
		int num2 = 0;
		float time = Time.time;
		for (int i = 0; i < this.samples.Count; i++)
		{
			RunningWeightedAverage.Entry entry = this.samples[i];
			if (entry.time != float.NaN && time - entry.time <= seconds)
			{
				num += entry.value;
				num2++;
			}
		}
		if (num2 == 0)
		{
			return 0f;
		}
		return num / (float)num2;
	}

	// Token: 0x0400386A RID: 14442
	private RingBuffer<RunningWeightedAverage.Entry> samples;

	// Token: 0x0400386B RID: 14443
	private float min;

	// Token: 0x0400386C RID: 14444
	private float max;

	// Token: 0x0400386D RID: 14445
	private bool ignoreZero;

	// Token: 0x02001C72 RID: 7282
	private struct Entry
	{
		// Token: 0x0600ADB2 RID: 44466 RVA: 0x003D0FD6 File Offset: 0x003CF1D6
		public bool IsValid()
		{
			return this.time != float.NaN;
		}

		// Token: 0x04008824 RID: 34852
		public float time;

		// Token: 0x04008825 RID: 34853
		public float value;
	}
}
