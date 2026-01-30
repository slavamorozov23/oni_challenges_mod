using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E81 RID: 3713
public class SingleSliderSideScreen : SideScreenContent
{
	// Token: 0x06007630 RID: 30256 RVA: 0x002D1790 File Offset: 0x002CF990
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x06007631 RID: 30257 RVA: 0x002D17CC File Offset: 0x002CF9CC
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		ISingleSliderControl singleSliderControl = target.GetComponent<ISingleSliderControl>();
		singleSliderControl = ((singleSliderControl != null) ? singleSliderControl : target.GetSMI<ISingleSliderControl>());
		return singleSliderControl != null && !component.IsPrefabID("HydrogenGenerator".ToTag()) && !component.IsPrefabID("MethaneGenerator".ToTag()) && !component.IsPrefabID("PetroleumGenerator".ToTag()) && !component.IsPrefabID("DevGenerator".ToTag()) && !component.HasTag(GameTags.DeadReactor) && singleSliderControl.GetSliderMin(0) != singleSliderControl.GetSliderMax(0);
	}

	// Token: 0x06007632 RID: 30258 RVA: 0x002D1864 File Offset: 0x002CFA64
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ISingleSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<ISingleSliderControl>();
			if (this.target == null)
			{
				global::Debug.LogError("The gameObject received does not contain a ISingleSliderControl implementation");
				return;
			}
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x040051C1 RID: 20929
	private ISingleSliderControl target;

	// Token: 0x040051C2 RID: 20930
	public List<SliderSet> sliderSets;
}
