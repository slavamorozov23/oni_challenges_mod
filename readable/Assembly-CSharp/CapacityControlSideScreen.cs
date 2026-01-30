using System;
using UnityEngine;

// Token: 0x02000E22 RID: 3618
public class CapacityControlSideScreen : SideScreenContent
{
	// Token: 0x060072BC RID: 29372 RVA: 0x002BCE40 File Offset: 0x002BB040
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = this.target.CapacityUnits;
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x060072BD RID: 29373 RVA: 0x002BCED8 File Offset: 0x002BB0D8
	public override bool IsValidForTarget(GameObject target)
	{
		IUserControlledCapacity userControlledCapacity = target.GetComponent<IUserControlledCapacity>();
		if (userControlledCapacity == null)
		{
			userControlledCapacity = target.GetSMI<IUserControlledCapacity>();
		}
		return userControlledCapacity != null && userControlledCapacity.ControlEnabled();
	}

	// Token: 0x060072BE RID: 29374 RVA: 0x002BCF04 File Offset: 0x002BB104
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IUserControlledCapacity>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IUserControlledCapacity>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.slider.minValue = this.target.MinCapacity;
		this.slider.maxValue = this.target.MaxCapacity;
		this.slider.value = this.target.UserMaxCapacity;
		this.slider.GetComponentInChildren<ToolTip>();
		this.unitsLabel.text = this.target.CapacityUnits;
		this.numberInput.minValue = this.target.MinCapacity;
		this.numberInput.maxValue = this.target.MaxCapacity;
		this.numberInput.currentValue = Mathf.Max(this.target.MinCapacity, Mathf.Min(this.target.MaxCapacity, this.target.UserMaxCapacity));
		this.numberInput.Activate();
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x060072BF RID: 29375 RVA: 0x002BD034 File Offset: 0x002BB234
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x060072C0 RID: 29376 RVA: 0x002BD03D File Offset: 0x002BB23D
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x060072C1 RID: 29377 RVA: 0x002BD046 File Offset: 0x002BB246
	private void UpdateMaxCapacity(float newValue)
	{
		this.target.UserMaxCapacity = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x060072C2 RID: 29378 RVA: 0x002BD068 File Offset: 0x002BB268
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.target.UserMaxCapacity.ToString());
	}

	// Token: 0x04004F51 RID: 20305
	private IUserControlledCapacity target;

	// Token: 0x04004F52 RID: 20306
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x04004F53 RID: 20307
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04004F54 RID: 20308
	[SerializeField]
	private LocText unitsLabel;
}
