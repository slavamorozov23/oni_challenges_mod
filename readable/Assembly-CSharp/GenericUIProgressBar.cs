using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D14 RID: 3348
[AddComponentMenu("KMonoBehaviour/scripts/GenericUIProgressBar")]
public class GenericUIProgressBar : KMonoBehaviour
{
	// Token: 0x060067A0 RID: 26528 RVA: 0x00270F7D File Offset: 0x0026F17D
	public void SetMaxValue(float max)
	{
		this.maxValue = max;
	}

	// Token: 0x060067A1 RID: 26529 RVA: 0x00270F88 File Offset: 0x0026F188
	public void SetFillPercentage(float value)
	{
		this.fill.fillAmount = value;
		this.label.text = Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value)) + "/" + this.maxValue.ToString();
	}

	// Token: 0x040046F9 RID: 18169
	public Image fill;

	// Token: 0x040046FA RID: 18170
	public LocText label;

	// Token: 0x040046FB RID: 18171
	private float maxValue;
}
