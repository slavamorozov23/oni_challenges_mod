using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E38 RID: 3640
public class DualSliderSideScreen : SideScreenContent
{
	// Token: 0x06007382 RID: 29570 RVA: 0x002C1C24 File Offset: 0x002BFE24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x06007383 RID: 29571 RVA: 0x002C1C5F File Offset: 0x002BFE5F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDualSliderControl>() != null;
	}

	// Token: 0x06007384 RID: 29572 RVA: 0x002C1C6C File Offset: 0x002BFE6C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IDualSliderControl>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a Manual Generator component");
			return;
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x04004FDE RID: 20446
	private IDualSliderControl target;

	// Token: 0x04004FDF RID: 20447
	public List<SliderSet> sliderSets;
}
