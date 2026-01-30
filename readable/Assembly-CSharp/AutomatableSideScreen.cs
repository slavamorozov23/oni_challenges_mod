using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E1A RID: 3610
public class AutomatableSideScreen : SideScreenContent
{
	// Token: 0x0600727A RID: 29306 RVA: 0x002BBD5D File Offset: 0x002B9F5D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600727B RID: 29307 RVA: 0x002BBD68 File Offset: 0x002B9F68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allowManualToggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.AUTOMATABLE_SIDE_SCREEN.ALLOWMANUALBUTTONTOOLTIP);
		this.allowManualToggle.onValueChanged += this.OnAllowManualChanged;
	}

	// Token: 0x0600727C RID: 29308 RVA: 0x002BBDB6 File Offset: 0x002B9FB6
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Automatable>() != null;
	}

	// Token: 0x0600727D RID: 29309 RVA: 0x002BBDC4 File Offset: 0x002B9FC4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetAutomatable = target.GetComponent<Automatable>();
		if (this.targetAutomatable == null)
		{
			global::Debug.LogError("The target provided does not have an Automatable component");
			return;
		}
		this.allowManualToggle.isOn = !this.targetAutomatable.GetAutomationOnly();
		this.allowManualToggleCheckMark.enabled = this.allowManualToggle.isOn;
	}

	// Token: 0x0600727E RID: 29310 RVA: 0x002BBE40 File Offset: 0x002BA040
	private void OnAllowManualChanged(bool value)
	{
		this.targetAutomatable.SetAutomationOnly(!value);
		this.allowManualToggleCheckMark.enabled = value;
	}

	// Token: 0x04004F16 RID: 20246
	public KToggle allowManualToggle;

	// Token: 0x04004F17 RID: 20247
	public KImage allowManualToggleCheckMark;

	// Token: 0x04004F18 RID: 20248
	public GameObject content;

	// Token: 0x04004F19 RID: 20249
	private GameObject target;

	// Token: 0x04004F1A RID: 20250
	public LocText DescriptionText;

	// Token: 0x04004F1B RID: 20251
	private Automatable targetAutomatable;
}
