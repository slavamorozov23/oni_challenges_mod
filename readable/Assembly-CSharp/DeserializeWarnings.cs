using System;
using UnityEngine;

// Token: 0x020008D7 RID: 2263
[AddComponentMenu("KMonoBehaviour/scripts/DeserializeWarnings")]
public class DeserializeWarnings : KMonoBehaviour
{
	// Token: 0x06003F18 RID: 16152 RVA: 0x00162317 File Offset: 0x00160517
	public static void DestroyInstance()
	{
		DeserializeWarnings.Instance = null;
	}

	// Token: 0x06003F19 RID: 16153 RVA: 0x0016231F File Offset: 0x0016051F
	protected override void OnPrefabInit()
	{
		DeserializeWarnings.Instance = this;
	}

	// Token: 0x0400273E RID: 10046
	public DeserializeWarnings.Warning BuildingTemeperatureIsZeroKelvin;

	// Token: 0x0400273F RID: 10047
	public DeserializeWarnings.Warning PipeContentsTemperatureIsNan;

	// Token: 0x04002740 RID: 10048
	public DeserializeWarnings.Warning PrimaryElementTemperatureIsNan;

	// Token: 0x04002741 RID: 10049
	public DeserializeWarnings.Warning PrimaryElementHasNoElement;

	// Token: 0x04002742 RID: 10050
	public static DeserializeWarnings Instance;

	// Token: 0x020018F0 RID: 6384
	public struct Warning
	{
		// Token: 0x0600A0CF RID: 41167 RVA: 0x003AA820 File Offset: 0x003A8A20
		public void Warn(string message, GameObject obj = null)
		{
			if (!this.isSet)
			{
				global::Debug.LogWarning(message, obj);
				this.isSet = true;
			}
		}

		// Token: 0x04007C7D RID: 31869
		private bool isSet;
	}
}
