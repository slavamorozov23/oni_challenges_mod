using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E56 RID: 3670
public class MultiSliderSideScreen : SideScreenContent
{
	// Token: 0x0600745D RID: 29789 RVA: 0x002C6BF0 File Offset: 0x002C4DF0
	public override bool IsValidForTarget(GameObject target)
	{
		IMultiSliderControl component = target.GetComponent<IMultiSliderControl>();
		return component != null && component.SidescreenEnabled();
	}

	// Token: 0x0600745E RID: 29790 RVA: 0x002C6C0F File Offset: 0x002C4E0F
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IMultiSliderControl>();
		this.titleKey = this.target.SidescreenTitleKey;
		this.Refresh();
	}

	// Token: 0x0600745F RID: 29791 RVA: 0x002C6C48 File Offset: 0x002C4E48
	private void Refresh()
	{
		while (this.liveSliders.Count < this.target.sliderControls.Length)
		{
			GameObject gameObject = Util.KInstantiateUI(this.sliderPrefab.gameObject, this.sliderContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			SliderSet sliderSet = new SliderSet();
			sliderSet.valueSlider = component.GetReference<KSlider>("Slider");
			sliderSet.numberInput = component.GetReference<KNumberInputField>("NumberInputField");
			if (sliderSet.numberInput != null)
			{
				sliderSet.numberInput.Activate();
			}
			sliderSet.targetLabel = component.GetReference<LocText>("TargetLabel");
			sliderSet.unitsLabel = component.GetReference<LocText>("UnitsLabel");
			sliderSet.minLabel = component.GetReference<LocText>("MinLabel");
			sliderSet.maxLabel = component.GetReference<LocText>("MaxLabel");
			sliderSet.SetupSlider(this.liveSliders.Count);
			this.liveSliders.Add(gameObject);
			this.sliderSets.Add(sliderSet);
		}
		for (int i = 0; i < this.liveSliders.Count; i++)
		{
			if (i >= this.target.sliderControls.Length)
			{
				this.liveSliders[i].SetActive(false);
			}
			else
			{
				if (!this.liveSliders[i].activeSelf)
				{
					this.liveSliders[i].SetActive(true);
					this.liveSliders[i].gameObject.SetActive(true);
				}
				this.sliderSets[i].SetTarget(this.target.sliderControls[i], i);
			}
		}
	}

	// Token: 0x04005076 RID: 20598
	public LayoutElement sliderPrefab;

	// Token: 0x04005077 RID: 20599
	public RectTransform sliderContainer;

	// Token: 0x04005078 RID: 20600
	private IMultiSliderControl target;

	// Token: 0x04005079 RID: 20601
	private List<GameObject> liveSliders = new List<GameObject>();

	// Token: 0x0400507A RID: 20602
	private List<SliderSet> sliderSets = new List<SliderSet>();
}
