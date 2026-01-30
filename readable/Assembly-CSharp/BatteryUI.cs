using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CAC RID: 3244
[AddComponentMenu("KMonoBehaviour/scripts/BatteryUI")]
public class BatteryUI : KMonoBehaviour
{
	// Token: 0x06006347 RID: 25415 RVA: 0x0024D498 File Offset: 0x0024B698
	private void Initialize()
	{
		if (this.unitLabel == null)
		{
			this.unitLabel = this.currentKJLabel.gameObject.GetComponentInChildrenOnly<LocText>();
		}
		if (this.sizeMap == null || this.sizeMap.Count == 0)
		{
			this.sizeMap = new Dictionary<float, float>();
			this.sizeMap.Add(20000f, 10f);
			this.sizeMap.Add(40000f, 25f);
			this.sizeMap.Add(60000f, 40f);
		}
	}

	// Token: 0x06006348 RID: 25416 RVA: 0x0024D528 File Offset: 0x0024B728
	public void SetContent(Battery bat)
	{
		if (bat == null || bat.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		base.gameObject.SetActive(true);
		this.Initialize();
		RectTransform component = this.batteryBG.GetComponent<RectTransform>();
		float num = 0f;
		foreach (KeyValuePair<float, float> keyValuePair in this.sizeMap)
		{
			if (bat.Capacity <= keyValuePair.Key)
			{
				num = keyValuePair.Value;
				break;
			}
		}
		this.batteryBG.sprite = ((bat.Capacity >= 40000f) ? this.bigBatteryBG : this.regularBatteryBG);
		float y = 25f;
		component.sizeDelta = new Vector2(num, y);
		BuildingEnabledButton component2 = bat.GetComponent<BuildingEnabledButton>();
		Color color;
		if (component2 != null && !component2.IsEnabled)
		{
			color = Color.gray;
		}
		else
		{
			color = ((bat.PercentFull >= bat.PreviousPercentFull) ? this.energyIncreaseColor : this.energyDecreaseColor);
		}
		this.batteryMeter.color = color;
		this.batteryBG.color = color;
		float num2 = this.batteryBG.GetComponent<RectTransform>().rect.height * bat.PercentFull;
		this.batteryMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(num - 5.5f, num2 - 5.5f);
		color.a = 1f;
		if (this.currentKJLabel.color != color)
		{
			this.currentKJLabel.color = color;
			this.unitLabel.color = color;
		}
		this.currentKJLabel.text = bat.JoulesAvailable.ToString("F0");
	}

	// Token: 0x0400436A RID: 17258
	[SerializeField]
	private LocText currentKJLabel;

	// Token: 0x0400436B RID: 17259
	[SerializeField]
	private Image batteryBG;

	// Token: 0x0400436C RID: 17260
	[SerializeField]
	private Image batteryMeter;

	// Token: 0x0400436D RID: 17261
	[SerializeField]
	private Sprite regularBatteryBG;

	// Token: 0x0400436E RID: 17262
	[SerializeField]
	private Sprite bigBatteryBG;

	// Token: 0x0400436F RID: 17263
	[SerializeField]
	private Color energyIncreaseColor = Color.green;

	// Token: 0x04004370 RID: 17264
	[SerializeField]
	private Color energyDecreaseColor = Color.red;

	// Token: 0x04004371 RID: 17265
	private LocText unitLabel;

	// Token: 0x04004372 RID: 17266
	private const float UIUnit = 10f;

	// Token: 0x04004373 RID: 17267
	private Dictionary<float, float> sizeMap;
}
