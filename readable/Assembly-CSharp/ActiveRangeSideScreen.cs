using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000E11 RID: 3601
public class ActiveRangeSideScreen : SideScreenContent
{
	// Token: 0x06007213 RID: 29203 RVA: 0x002B9540 File Offset: 0x002B7740
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06007214 RID: 29204 RVA: 0x002B9548 File Offset: 0x002B7748
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateValueLabel.maxValue = this.target.MaxValue;
		this.activateValueLabel.minValue = this.target.MinValue;
		this.deactivateValueLabel.maxValue = this.target.MaxValue;
		this.deactivateValueLabel.minValue = this.target.MinValue;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
	}

	// Token: 0x06007215 RID: 29205 RVA: 0x002B95EC File Offset: 0x002B77EC
	private void OnActivateValueChanged(float new_value)
	{
		this.target.ActivateValue = new_value;
		if (this.target.ActivateValue < this.target.DeactivateValue)
		{
			this.target.ActivateValue = this.target.DeactivateValue;
			this.activateValueSlider.value = this.target.ActivateValue;
		}
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x06007216 RID: 29206 RVA: 0x002B9670 File Offset: 0x002B7870
	private void OnDeactivateValueChanged(float new_value)
	{
		this.target.DeactivateValue = new_value;
		if (this.target.DeactivateValue > this.target.ActivateValue)
		{
			this.target.DeactivateValue = this.activateValueSlider.value;
			this.deactivateValueSlider.value = this.target.DeactivateValue;
		}
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x06007217 RID: 29207 RVA: 0x002B96F4 File Offset: 0x002B78F4
	private void RefreshTooltips()
	{
		this.activateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.ActivateTooltip, this.activateValueSlider.value, this.deactivateValueSlider.value));
		this.deactivateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.DeactivateTooltip, this.deactivateValueSlider.value, this.activateValueSlider.value));
	}

	// Token: 0x06007218 RID: 29208 RVA: 0x002B9781 File Offset: 0x002B7981
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IActivationRangeTarget>() != null;
	}

	// Token: 0x06007219 RID: 29209 RVA: 0x002B978C File Offset: 0x002B798C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IActivationRangeTarget>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IActivationRangeTarget component");
			return;
		}
		this.activateLabel.text = this.target.ActivateSliderLabelText;
		this.deactivateLabel.text = this.target.DeactivateSliderLabelText;
		this.activateValueLabel.Activate();
		this.deactivateValueLabel.Activate();
		this.activateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueSlider.minValue = this.target.MinValue;
		this.activateValueSlider.maxValue = this.target.MaxValue;
		this.activateValueSlider.value = this.target.ActivateValue;
		this.activateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.activateValueLabel.onEndEdit += delegate()
		{
			float activateValue = this.target.ActivateValue;
			float.TryParse(this.activateValueLabel.field.text, out activateValue);
			this.OnActivateValueChanged(activateValue);
			this.activateValueSlider.value = activateValue;
		};
		this.deactivateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueSlider.minValue = this.target.MinValue;
		this.deactivateValueSlider.maxValue = this.target.MaxValue;
		this.deactivateValueSlider.value = this.target.DeactivateValue;
		this.deactivateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.deactivateValueLabel.onEndEdit += delegate()
		{
			float deactivateValue = this.target.DeactivateValue;
			float.TryParse(this.deactivateValueLabel.field.text, out deactivateValue);
			this.OnDeactivateValueChanged(deactivateValue);
			this.deactivateValueSlider.value = deactivateValue;
		};
		this.RefreshTooltips();
	}

	// Token: 0x0600721A RID: 29210 RVA: 0x002B999E File Offset: 0x002B7B9E
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return this.target.ActivationRangeTitleText;
		}
		return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
	}

	// Token: 0x04004ECA RID: 20170
	private IActivationRangeTarget target;

	// Token: 0x04004ECB RID: 20171
	[SerializeField]
	private KSlider activateValueSlider;

	// Token: 0x04004ECC RID: 20172
	[SerializeField]
	private KSlider deactivateValueSlider;

	// Token: 0x04004ECD RID: 20173
	[SerializeField]
	private LocText activateLabel;

	// Token: 0x04004ECE RID: 20174
	[SerializeField]
	private LocText deactivateLabel;

	// Token: 0x04004ECF RID: 20175
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField activateValueLabel;

	// Token: 0x04004ED0 RID: 20176
	[SerializeField]
	private KNumberInputField deactivateValueLabel;
}
