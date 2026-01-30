using System;
using UnityEngine;

// Token: 0x02000E7F RID: 3711
[Serializable]
public class SliderSet
{
	// Token: 0x06007624 RID: 30244 RVA: 0x002D1354 File Offset: 0x002CF554
	public void SetupSlider(int index)
	{
		this.index = index;
		this.valueSlider.onReleaseHandle += delegate()
		{
			this.valueSlider.value = Mathf.Round(this.valueSlider.value * 10f) / 10f;
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput();
		};
	}

	// Token: 0x06007625 RID: 30245 RVA: 0x002D13DC File Offset: 0x002CF5DC
	public void SetTarget(ISliderControl target, int index)
	{
		this.index = index;
		this.target = target;
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(target.GetSliderTooltip(index));
		}
		if (this.targetLabel != null)
		{
			this.targetLabel.text = ((target.SliderTitleKey != null) ? Strings.Get(target.SliderTitleKey) : "");
		}
		this.unitsLabel.text = target.SliderUnits;
		this.minLabel.text = target.GetSliderMin(index).ToString() + target.SliderUnits;
		this.maxLabel.text = target.GetSliderMax(index).ToString() + target.SliderUnits;
		this.numberInput.minValue = target.GetSliderMin(index);
		this.numberInput.maxValue = target.GetSliderMax(index);
		this.numberInput.decimalPlaces = target.SliderDecimalPlaces(index);
		this.numberInput.field.characterLimit = Mathf.FloorToInt(1f + Mathf.Log10(this.numberInput.maxValue + (float)this.numberInput.decimalPlaces));
		Vector2 sizeDelta = this.numberInput.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = (float)((this.numberInput.field.characterLimit + 1) * 10);
		this.numberInput.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		this.valueSlider.minValue = target.GetSliderMin(index);
		this.valueSlider.maxValue = target.GetSliderMax(index);
		this.valueSlider.value = target.GetSliderValue(index);
		this.SetValue(target.GetSliderValue(index));
		if (index == 0)
		{
			this.numberInput.Activate();
		}
	}

	// Token: 0x06007626 RID: 30246 RVA: 0x002D15B0 File Offset: 0x002CF7B0
	private void ReceiveValueFromSlider()
	{
		float num = this.valueSlider.value;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.SetValue(num);
	}

	// Token: 0x06007627 RID: 30247 RVA: 0x002D1600 File Offset: 0x002CF800
	private void ReceiveValueFromInput()
	{
		float num = this.numberInput.currentValue;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.valueSlider.value = num;
		this.SetValue(num);
	}

	// Token: 0x06007628 RID: 30248 RVA: 0x002D165C File Offset: 0x002CF85C
	private void SetValue(float value)
	{
		float num = value;
		if (num > this.target.GetSliderMax(this.index))
		{
			num = this.target.GetSliderMax(this.index);
		}
		else if (num < this.target.GetSliderMin(this.index))
		{
			num = this.target.GetSliderMin(this.index);
		}
		this.UpdateLabel(num);
		this.target.SetSliderValue(num, this.index);
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(this.target.GetSliderTooltip(this.index));
		}
	}

	// Token: 0x06007629 RID: 30249 RVA: 0x002D1704 File Offset: 0x002CF904
	private void UpdateLabel(float value)
	{
		float num = Mathf.Round(value * 10f) / 10f;
		this.numberInput.SetDisplayValue(num.ToString());
	}

	// Token: 0x040051B9 RID: 20921
	public KSlider valueSlider;

	// Token: 0x040051BA RID: 20922
	public KNumberInputField numberInput;

	// Token: 0x040051BB RID: 20923
	public LocText targetLabel;

	// Token: 0x040051BC RID: 20924
	public LocText unitsLabel;

	// Token: 0x040051BD RID: 20925
	public LocText minLabel;

	// Token: 0x040051BE RID: 20926
	public LocText maxLabel;

	// Token: 0x040051BF RID: 20927
	[NonSerialized]
	public int index;

	// Token: 0x040051C0 RID: 20928
	private ISliderControl target;
}
