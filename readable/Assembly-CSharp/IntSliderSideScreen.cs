using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E46 RID: 3654
public class IntSliderSideScreen : SideScreenContent
{
	// Token: 0x060073D3 RID: 29651 RVA: 0x002C3760 File Offset: 0x002C1960
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
			this.sliderSets[i].valueSlider.wholeNumbers = true;
		}
	}

	// Token: 0x060073D4 RID: 29652 RVA: 0x002C37B2 File Offset: 0x002C19B2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IIntSliderControl>() != null || target.GetSMI<IIntSliderControl>() != null;
	}

	// Token: 0x060073D5 RID: 29653 RVA: 0x002C37C8 File Offset: 0x002C19C8
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IIntSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IIntSliderControl>();
		}
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

	// Token: 0x04005016 RID: 20502
	private IIntSliderControl target;

	// Token: 0x04005017 RID: 20503
	public List<SliderSet> sliderSets;
}
