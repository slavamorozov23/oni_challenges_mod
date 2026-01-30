using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000663 RID: 1635
public abstract class Tracker
{
	// Token: 0x06002797 RID: 10135 RVA: 0x000E31BC File Offset: 0x000E13BC
	public global::Tuple<float, float>[] ChartableData(float periodLength)
	{
		float time = GameClock.Instance.GetTime();
		List<global::Tuple<float, float>> list = new List<global::Tuple<float, float>>();
		int num = this.dataPoints.Count - 1;
		while (num >= 0 && this.dataPoints[num].periodStart >= time - periodLength)
		{
			list.Add(new global::Tuple<float, float>(this.dataPoints[num].periodStart, this.dataPoints[num].periodValue));
			num--;
		}
		if (list.Count == 0)
		{
			if (this.dataPoints.Count > 0)
			{
				list.Add(new global::Tuple<float, float>(this.dataPoints[this.dataPoints.Count - 1].periodStart, this.dataPoints[this.dataPoints.Count - 1].periodValue));
			}
			else
			{
				list.Add(new global::Tuple<float, float>(0f, 0f));
			}
		}
		list.Reverse();
		return list.ToArray();
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x000E32B4 File Offset: 0x000E14B4
	public float GetDataTimeLength()
	{
		float num = 0f;
		for (int i = this.dataPoints.Count - 1; i >= 0; i--)
		{
			num += this.dataPoints[i].periodEnd - this.dataPoints[i].periodStart;
		}
		return num;
	}

	// Token: 0x06002799 RID: 10137
	public abstract void UpdateData();

	// Token: 0x0600279A RID: 10138
	public abstract string FormatValueString(float value);

	// Token: 0x0600279B RID: 10139 RVA: 0x000E3306 File Offset: 0x000E1506
	public float GetCurrentValue()
	{
		if (this.dataPoints.Count == 0)
		{
			return 0f;
		}
		return this.dataPoints[this.dataPoints.Count - 1].periodValue;
	}

	// Token: 0x0600279C RID: 10140 RVA: 0x000E3338 File Offset: 0x000E1538
	public float GetMinValue(float sampleHistoryLengthSeconds)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData(sampleHistoryLengthSeconds);
		if (array.Length == 0)
		{
			return 0f;
		}
		if (array.Length == 1)
		{
			return array[0].second;
		}
		float num = array[array.Length - 1].second;
		int num2 = array.Length - 1;
		while (num2 >= 0 && time - array[num2].first <= sampleHistoryLengthSeconds)
		{
			num = Mathf.Min(num, array[num2].second);
			num2--;
		}
		return num;
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x000E33AC File Offset: 0x000E15AC
	public float GetMaxValue(int sampleHistoryLengthSeconds)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData((float)sampleHistoryLengthSeconds);
		if (array.Length == 0)
		{
			return 0f;
		}
		if (array.Length == 1)
		{
			return array[0].second;
		}
		float num = array[array.Length - 1].second;
		int num2 = array.Length - 1;
		while (num2 >= 0 && time - array[num2].first <= (float)sampleHistoryLengthSeconds)
		{
			num = Mathf.Max(num, array[num2].second);
			num2--;
		}
		return num;
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000E3424 File Offset: 0x000E1624
	public float GetAverageValue(float sampleHistoryLengthSeconds)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData(sampleHistoryLengthSeconds);
		float num = 0f;
		float num2 = 0f;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (array[i].first >= time - sampleHistoryLengthSeconds)
			{
				float num3 = (i == array.Length - 1) ? (time - array[i].first) : (array[i + 1].first - array[i].first);
				num2 += num3;
				if (!float.IsNaN(array[i].second))
				{
					num += num3 * array[i].second;
				}
			}
		}
		float result;
		if (num2 == 0f)
		{
			if (array.Length == 0)
			{
				result = 0f;
			}
			else
			{
				result = array[array.Length - 1].second;
			}
		}
		else
		{
			result = num / num2;
		}
		return result;
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000E34F8 File Offset: 0x000E16F8
	public float GetDelta(float secondsAgo)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData(secondsAgo);
		if (array.Length < 2)
		{
			return 0f;
		}
		float num = -1f;
		float second = array[array.Length - 1].second;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (time - array[i].first >= secondsAgo)
			{
				num = array[i].second;
			}
		}
		return second - num;
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000E3568 File Offset: 0x000E1768
	protected void AddPoint(float value)
	{
		if (float.IsNaN(value))
		{
			value = 0f;
		}
		this.dataPoints.Add(new DataPoint((this.dataPoints.Count == 0) ? GameClock.Instance.GetTime() : this.dataPoints[this.dataPoints.Count - 1].periodEnd, GameClock.Instance.GetTime(), value));
		int count = Math.Max(0, this.dataPoints.Count - this.maxPoints);
		this.dataPoints.RemoveRange(0, count);
	}

	// Token: 0x060027A1 RID: 10145 RVA: 0x000E35FC File Offset: 0x000E17FC
	public List<DataPoint> GetCompressedData()
	{
		int num = 10;
		List<DataPoint> list = new List<DataPoint>();
		float num2 = (this.dataPoints[this.dataPoints.Count - 1].periodEnd - this.dataPoints[0].periodStart) / (float)num;
		for (int i = 0; i < num; i++)
		{
			float num3 = num2 * (float)i;
			float num4 = num3 + num2;
			float num5 = 0f;
			for (int j = 0; j < this.dataPoints.Count; j++)
			{
				DataPoint dataPoint = this.dataPoints[j];
				num5 += dataPoint.periodValue * Mathf.Max(0f, Mathf.Min(num4, dataPoint.periodEnd) - Mathf.Max(dataPoint.periodStart, num3));
			}
			list.Add(new DataPoint(num3, num4, num5 / (num4 - num3)));
		}
		return list;
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x000E36DF File Offset: 0x000E18DF
	public void OverwriteData(List<DataPoint> newData)
	{
		this.dataPoints = newData;
	}

	// Token: 0x04001743 RID: 5955
	private const int standardSampleRate = 4;

	// Token: 0x04001744 RID: 5956
	private const int defaultCyclesTracked = 5;

	// Token: 0x04001745 RID: 5957
	public List<GameObject> objectsOfInterest = new List<GameObject>();

	// Token: 0x04001746 RID: 5958
	protected List<DataPoint> dataPoints = new List<DataPoint>();

	// Token: 0x04001747 RID: 5959
	private int maxPoints = Mathf.CeilToInt(750f);
}
