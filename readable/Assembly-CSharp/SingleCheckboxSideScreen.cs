using System;
using UnityEngine;

// Token: 0x02000E79 RID: 3705
public class SingleCheckboxSideScreen : SideScreenContent
{
	// Token: 0x060075DC RID: 30172 RVA: 0x002D058B File Offset: 0x002CE78B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060075DD RID: 30173 RVA: 0x002D0593 File Offset: 0x002CE793
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggle.onValueChanged += this.OnValueChanged;
	}

	// Token: 0x060075DE RID: 30174 RVA: 0x002D05B2 File Offset: 0x002CE7B2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ICheckboxControl>() != null || target.GetSMI<ICheckboxControl>() != null;
	}

	// Token: 0x060075DF RID: 30175 RVA: 0x002D05C8 File Offset: 0x002CE7C8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.target = target.GetComponent<ICheckboxControl>();
		if (this.target == null)
		{
			this.target = target.GetSMI<ICheckboxControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The target provided does not have an ICheckboxControl component");
			return;
		}
		this.label.text = this.target.CheckboxLabel;
		this.toggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(this.target.CheckboxTooltip);
		this.titleKey = this.target.CheckboxTitleKey;
		this.toggle.isOn = this.target.GetCheckboxValue();
		this.toggleCheckMark.enabled = this.toggle.isOn;
	}

	// Token: 0x060075E0 RID: 30176 RVA: 0x002D069B File Offset: 0x002CE89B
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.target = null;
	}

	// Token: 0x060075E1 RID: 30177 RVA: 0x002D06AA File Offset: 0x002CE8AA
	private void OnValueChanged(bool value)
	{
		this.target.SetCheckboxValue(value);
		this.toggleCheckMark.enabled = value;
	}

	// Token: 0x04005198 RID: 20888
	public KToggle toggle;

	// Token: 0x04005199 RID: 20889
	public KImage toggleCheckMark;

	// Token: 0x0400519A RID: 20890
	public LocText label;

	// Token: 0x0400519B RID: 20891
	private ICheckboxControl target;
}
