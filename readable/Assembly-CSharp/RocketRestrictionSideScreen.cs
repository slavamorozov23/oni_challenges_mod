using System;
using UnityEngine;

// Token: 0x02000E6F RID: 3695
public class RocketRestrictionSideScreen : SideScreenContent
{
	// Token: 0x0600756F RID: 30063 RVA: 0x002CD2AF File Offset: 0x002CB4AF
	protected override void OnSpawn()
	{
		this.unrestrictedButton.onClick += this.ClickNone;
		this.spaceRestrictedButton.onClick += this.ClickSpace;
	}

	// Token: 0x06007570 RID: 30064 RVA: 0x002CD2DF File Offset: 0x002CB4DF
	public override int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x06007571 RID: 30065 RVA: 0x002CD2E2 File Offset: 0x002CB4E2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<RocketControlStation.StatesInstance>() != null;
	}

	// Token: 0x06007572 RID: 30066 RVA: 0x002CD2F0 File Offset: 0x002CB4F0
	public override void SetTarget(GameObject new_target)
	{
		if (this.controlStation != null || this.controlStationLogicSubHandle != -1)
		{
			this.ClearTarget();
		}
		this.controlStation = new_target.GetComponent<RocketControlStation>();
		this.controlStationLogicSubHandle = this.controlStation.Subscribe(1861523068, new Action<object>(this.UpdateButtonStates));
		this.UpdateButtonStates(null);
	}

	// Token: 0x06007573 RID: 30067 RVA: 0x002CD34F File Offset: 0x002CB54F
	public override void ClearTarget()
	{
		if (this.controlStationLogicSubHandle != -1 && this.controlStation != null)
		{
			this.controlStation.Unsubscribe(this.controlStationLogicSubHandle);
			this.controlStationLogicSubHandle = -1;
		}
		this.controlStation = null;
	}

	// Token: 0x06007574 RID: 30068 RVA: 0x002CD388 File Offset: 0x002CB588
	private void UpdateButtonStates(object data = null)
	{
		bool flag = this.controlStation.IsLogicInputConnected();
		if (!flag)
		{
			this.unrestrictedButton.isOn = !this.controlStation.RestrictWhenGrounded;
			this.spaceRestrictedButton.isOn = this.controlStation.RestrictWhenGrounded;
		}
		this.unrestrictedButton.gameObject.SetActive(!flag);
		this.spaceRestrictedButton.gameObject.SetActive(!flag);
		this.automationControlled.gameObject.SetActive(flag);
	}

	// Token: 0x06007575 RID: 30069 RVA: 0x002CD40C File Offset: 0x002CB60C
	private void ClickNone()
	{
		this.controlStation.RestrictWhenGrounded = false;
		this.UpdateButtonStates(null);
	}

	// Token: 0x06007576 RID: 30070 RVA: 0x002CD421 File Offset: 0x002CB621
	private void ClickSpace()
	{
		this.controlStation.RestrictWhenGrounded = true;
		this.UpdateButtonStates(null);
	}

	// Token: 0x04005145 RID: 20805
	private RocketControlStation controlStation;

	// Token: 0x04005146 RID: 20806
	[Header("Buttons")]
	public KToggle unrestrictedButton;

	// Token: 0x04005147 RID: 20807
	public KToggle spaceRestrictedButton;

	// Token: 0x04005148 RID: 20808
	public GameObject automationControlled;

	// Token: 0x04005149 RID: 20809
	private int controlStationLogicSubHandle = -1;
}
