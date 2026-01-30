using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000E98 RID: 3736
[AddComponentMenu("KMonoBehaviour/scripts/SliderContainer")]
public class SliderContainer : KMonoBehaviour
{
	// Token: 0x06007783 RID: 30595 RVA: 0x002DD3C4 File Offset: 0x002DB5C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateSliderLabel));
	}

	// Token: 0x06007784 RID: 30596 RVA: 0x002DD3E8 File Offset: 0x002DB5E8
	public void UpdateSliderLabel(float newValue)
	{
		if (this.isPercentValue)
		{
			this.valueLabel.text = (newValue * 100f).ToString("F0") + "%";
			return;
		}
		this.valueLabel.text = newValue.ToString();
	}

	// Token: 0x04005308 RID: 21256
	public bool isPercentValue = true;

	// Token: 0x04005309 RID: 21257
	public KSlider slider;

	// Token: 0x0400530A RID: 21258
	public LocText nameLabel;

	// Token: 0x0400530B RID: 21259
	public LocText valueLabel;
}
