using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E65 RID: 3685
public class RailGunSideScreen : SideScreenContent
{
	// Token: 0x06007500 RID: 29952 RVA: 0x002CA514 File Offset: 0x002C8714
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
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

	// Token: 0x06007501 RID: 29953 RVA: 0x002CA5A5 File Offset: 0x002C87A5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x06007502 RID: 29954 RVA: 0x002CA5C1 File Offset: 0x002C87C1
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RailGun>() != null;
	}

	// Token: 0x06007503 RID: 29955 RVA: 0x002CA5D0 File Offset: 0x002C87D0
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.selectedGun = new_target.GetComponent<RailGun>();
		if (this.selectedGun == null)
		{
			global::Debug.LogError("The gameObject received does not contain a RailGun component");
			return;
		}
		this.targetRailgunHEPStorageSubHandle = this.selectedGun.Subscribe(-1837862626, new Action<object>(this.UpdateHEPLabels));
		this.slider.minValue = this.selectedGun.MinLaunchMass;
		this.slider.maxValue = this.selectedGun.MaxLaunchMass;
		this.slider.value = this.selectedGun.launchMass;
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
		this.numberInput.minValue = this.selectedGun.MinLaunchMass;
		this.numberInput.maxValue = this.selectedGun.MaxLaunchMass;
		this.numberInput.currentValue = Mathf.Max(this.selectedGun.MinLaunchMass, Mathf.Min(this.selectedGun.MaxLaunchMass, this.selectedGun.launchMass));
		this.UpdateMaxCapacityLabel();
		this.numberInput.Activate();
		this.UpdateHEPLabels(null);
	}

	// Token: 0x06007504 RID: 29956 RVA: 0x002CA70A File Offset: 0x002C890A
	public override void ClearTarget()
	{
		if (this.targetRailgunHEPStorageSubHandle != -1 && this.selectedGun != null)
		{
			this.selectedGun.Unsubscribe(this.targetRailgunHEPStorageSubHandle);
			this.targetRailgunHEPStorageSubHandle = -1;
		}
		this.selectedGun = null;
	}

	// Token: 0x06007505 RID: 29957 RVA: 0x002CA744 File Offset: 0x002C8944
	public void UpdateHEPLabels(object data = null)
	{
		if (this.selectedGun == null)
		{
			return;
		}
		string text = BUILDINGS.PREFABS.RAILGUN.SIDESCREEN_HEP_REQUIRED;
		text = text.Replace("{current}", this.selectedGun.CurrentEnergy.ToString());
		text = text.Replace("{required}", this.selectedGun.EnergyCost.ToString());
		this.hepStorageInfo.text = text;
	}

	// Token: 0x06007506 RID: 29958 RVA: 0x002CA7B5 File Offset: 0x002C89B5
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06007507 RID: 29959 RVA: 0x002CA7BE File Offset: 0x002C89BE
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x06007508 RID: 29960 RVA: 0x002CA7C7 File Offset: 0x002C89C7
	private void UpdateMaxCapacity(float newValue)
	{
		this.selectedGun.launchMass = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
		this.selectedGun.Trigger(161772031, null);
	}

	// Token: 0x06007509 RID: 29961 RVA: 0x002CA7F8 File Offset: 0x002C89F8
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.selectedGun.launchMass.ToString());
	}

	// Token: 0x040050E6 RID: 20710
	public GameObject content;

	// Token: 0x040050E7 RID: 20711
	private RailGun selectedGun;

	// Token: 0x040050E8 RID: 20712
	public LocText DescriptionText;

	// Token: 0x040050E9 RID: 20713
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x040050EA RID: 20714
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040050EB RID: 20715
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x040050EC RID: 20716
	[SerializeField]
	private LocText hepStorageInfo;

	// Token: 0x040050ED RID: 20717
	private int targetRailgunHEPStorageSubHandle = -1;
}
