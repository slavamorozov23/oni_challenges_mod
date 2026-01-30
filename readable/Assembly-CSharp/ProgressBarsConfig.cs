using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DE6 RID: 3558
public class ProgressBarsConfig : ScriptableObject
{
	// Token: 0x06006FF8 RID: 28664 RVA: 0x002A8975 File Offset: 0x002A6B75
	public static void DestroyInstance()
	{
		ProgressBarsConfig.instance = null;
	}

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x002A897D File Offset: 0x002A6B7D
	public static ProgressBarsConfig Instance
	{
		get
		{
			if (ProgressBarsConfig.instance == null)
			{
				ProgressBarsConfig.instance = Resources.Load<ProgressBarsConfig>("ProgressBarsConfig");
				ProgressBarsConfig.instance.Initialize();
			}
			return ProgressBarsConfig.instance;
		}
	}

	// Token: 0x06006FFA RID: 28666 RVA: 0x002A89AC File Offset: 0x002A6BAC
	public void Initialize()
	{
		foreach (ProgressBarsConfig.BarData barData in this.barColorDataList)
		{
			this.barColorMap.Add(barData.barName, barData);
		}
	}

	// Token: 0x06006FFB RID: 28667 RVA: 0x002A8A0C File Offset: 0x002A6C0C
	public string GetBarDescription(string barName)
	{
		string result = "";
		if (this.IsBarNameValid(barName))
		{
			result = Strings.Get(this.barColorMap[barName].barDescriptionKey);
		}
		return result;
	}

	// Token: 0x06006FFC RID: 28668 RVA: 0x002A8A48 File Offset: 0x002A6C48
	public Color GetBarColor(string barName)
	{
		Color result = Color.clear;
		if (this.IsBarNameValid(barName))
		{
			result = this.barColorMap[barName].barColor;
		}
		return result;
	}

	// Token: 0x06006FFD RID: 28669 RVA: 0x002A8A77 File Offset: 0x002A6C77
	public bool IsBarNameValid(string barName)
	{
		if (string.IsNullOrEmpty(barName))
		{
			global::Debug.LogError("The barName provided was null or empty. Don't do that.");
			return false;
		}
		if (!this.barColorMap.ContainsKey(barName))
		{
			global::Debug.LogError(string.Format("No BarData found for the entry [ {0} ]", barName));
			return false;
		}
		return true;
	}

	// Token: 0x04004CC7 RID: 19655
	public GameObject progressBarPrefab;

	// Token: 0x04004CC8 RID: 19656
	public GameObject progressBarUIPrefab;

	// Token: 0x04004CC9 RID: 19657
	public GameObject healthBarPrefab;

	// Token: 0x04004CCA RID: 19658
	public List<ProgressBarsConfig.BarData> barColorDataList = new List<ProgressBarsConfig.BarData>();

	// Token: 0x04004CCB RID: 19659
	public Dictionary<string, ProgressBarsConfig.BarData> barColorMap = new Dictionary<string, ProgressBarsConfig.BarData>();

	// Token: 0x04004CCC RID: 19660
	private static ProgressBarsConfig instance;

	// Token: 0x02002050 RID: 8272
	[Serializable]
	public struct BarData
	{
		// Token: 0x040095AB RID: 38315
		public string barName;

		// Token: 0x040095AC RID: 38316
		public Color barColor;

		// Token: 0x040095AD RID: 38317
		public string barDescriptionKey;
	}
}
