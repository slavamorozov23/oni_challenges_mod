using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D15 RID: 3349
[AddComponentMenu("KMonoBehaviour/scripts/GoodnessSlider")]
public class GoodnessSlider : KMonoBehaviour
{
	// Token: 0x060067A3 RID: 26531 RVA: 0x00270FE1 File Offset: 0x0026F1E1
	protected override void OnSpawn()
	{
		base.Spawn();
		this.UpdateValues();
	}

	// Token: 0x060067A4 RID: 26532 RVA: 0x00270FF0 File Offset: 0x0026F1F0
	public void UpdateValues()
	{
		this.text.color = (this.fill.color = this.gradient.Evaluate(this.slider.value));
		for (int i = 0; i < this.gradient.colorKeys.Length; i++)
		{
			if (this.gradient.colorKeys[i].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
			if (i == this.gradient.colorKeys.Length - 1 && this.gradient.colorKeys[i - 1].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
		}
	}

	// Token: 0x040046FC RID: 18172
	public Image icon;

	// Token: 0x040046FD RID: 18173
	public Text text;

	// Token: 0x040046FE RID: 18174
	public Slider slider;

	// Token: 0x040046FF RID: 18175
	public Image fill;

	// Token: 0x04004700 RID: 18176
	public Gradient gradient;

	// Token: 0x04004701 RID: 18177
	public string[] names;
}
