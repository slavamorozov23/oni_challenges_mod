using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C2D RID: 3117
public class OniMetrics : MonoBehaviour
{
	// Token: 0x06005E39 RID: 24121 RVA: 0x00225FFC File Offset: 0x002241FC
	private static void EnsureMetrics()
	{
		if (OniMetrics.Metrics != null)
		{
			return;
		}
		OniMetrics.Metrics = new List<Dictionary<string, object>>(2);
		for (int i = 0; i < 2; i++)
		{
			OniMetrics.Metrics.Add(null);
		}
	}

	// Token: 0x06005E3A RID: 24122 RVA: 0x00226033 File Offset: 0x00224233
	public static void LogEvent(OniMetrics.Event eventType, string key, object data)
	{
		OniMetrics.EnsureMetrics();
		if (OniMetrics.Metrics[(int)eventType] == null)
		{
			OniMetrics.Metrics[(int)eventType] = new Dictionary<string, object>();
		}
		OniMetrics.Metrics[(int)eventType][key] = data;
	}

	// Token: 0x06005E3B RID: 24123 RVA: 0x0022606C File Offset: 0x0022426C
	public static void SendEvent(OniMetrics.Event eventType, string debugName)
	{
		if (OniMetrics.Metrics[(int)eventType] == null || OniMetrics.Metrics[(int)eventType].Count == 0)
		{
			return;
		}
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(OniMetrics.Metrics[(int)eventType], debugName);
		OniMetrics.Metrics[(int)eventType].Clear();
	}

	// Token: 0x04003EA1 RID: 16033
	private static List<Dictionary<string, object>> Metrics;

	// Token: 0x02001DCE RID: 7630
	public enum Event : short
	{
		// Token: 0x04008C50 RID: 35920
		NewSave,
		// Token: 0x04008C51 RID: 35921
		EndOfCycle,
		// Token: 0x04008C52 RID: 35922
		NumEvents
	}
}
