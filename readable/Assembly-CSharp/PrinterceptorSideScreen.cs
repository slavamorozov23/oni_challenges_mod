using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000005 RID: 5
public class PrinterceptorSideScreen : SideScreenContent
{
	// Token: 0x06000015 RID: 21 RVA: 0x0000247C File Offset: 0x0000067C
	public override bool IsValidForTarget(GameObject target)
	{
		HijackedHeadquarters.Instance smi = target.GetSMI<HijackedHeadquarters.Instance>();
		return smi != null && smi.IsInsideState(smi.sm.operational);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000024A6 File Offset: 0x000006A6
	public override int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000024A9 File Offset: 0x000006A9
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000024B1 File Offset: 0x000006B1
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.RefreshDisplay();
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000024C0 File Offset: 0x000006C0
	private void RefreshDisplay()
	{
		HijackedHeadquarters.Instance smi = this.target.GetSMI<HijackedHeadquarters.Instance>();
		this.interceptStateLabel.text = string.Format(UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_METER, smi.sm.interceptCharges.Get(smi), 3);
		bool flag = smi.sm.passcodeUnlocked.Get(smi) && Immigration.Instance.ImmigrantsAvailable && smi.sm.interceptCharges.Get(smi) < 3;
		bool flag2 = this.target.IsInsideState(this.target.sm.operational.readyToPrint.pre) || this.target.IsInsideState(this.target.sm.operational.readyToPrint.loop);
		this.interceptButton.isInteractable = flag;
		this.printButton.isInteractable = flag2;
		this.interceptButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_TOOLTIP : ((smi.sm.interceptCharges.Get(smi) >= 3) ? UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_TOOLTIP_DISABLED_TOO_FULL : UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_TOOLTIP_DISABLED));
		this.printButton.GetComponent<ToolTip>().SetSimpleTooltip(flag2 ? UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.PRINT_TOOLTIP : UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.PRINT_TOOLTIP_DISABLED);
		for (int i = 0; i < this.progressIndicators.Length; i++)
		{
			Image componentInChildren = this.progressIndicators[i].GetComponentInChildren<Image>();
			componentInChildren.sprite = Def.GetUISprite("Headquarters", "ui", false).first;
			componentInChildren.color = ((i < smi.sm.interceptCharges.Get(smi)) ? Color.white : Color.gray);
		}
		this.databankCountLabel.SetText(GameUtil.SafeStringFormat(UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.DATABANK_COUNT, new object[]
		{
			this.target.GetComponent<Storage>().GetAmountAvailable(DatabankHelper.ID).ToString()
		}));
		Image[] array = this.databankIcon;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].sprite = Def.GetUISprite(DatabankHelper.ID, "ui", false).first;
		}
		if (this.target.GetSMI<HijackedHeadquarters.Instance>().sm.passcodeUnlocked.Get(this.target))
		{
			this.lockedSection.SetActive(false);
			this.meterSection.SetActive(true);
			return;
		}
		this.lockedSection.SetActive(true);
		this.meterSection.SetActive(false);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002744 File Offset: 0x00000944
	public override void SetTarget(GameObject new_target)
	{
		this.target = new_target.GetSMI<HijackedHeadquarters.Instance>();
		this.printButton.ClearOnClick();
		this.interceptButton.ClearOnClick();
		this.printButton.onClick += delegate()
		{
			this.target.ActivatePrintInterface();
		};
		this.interceptButton.onClick += delegate()
		{
			this.target.Intercept();
		};
		this.RefreshDisplay();
	}

	// Token: 0x04000009 RID: 9
	private HijackedHeadquarters.Instance target;

	// Token: 0x0400000A RID: 10
	[SerializeField]
	private KButton printButton;

	// Token: 0x0400000B RID: 11
	[SerializeField]
	private KButton interceptButton;

	// Token: 0x0400000C RID: 12
	[SerializeField]
	private LocText interceptStateLabel;

	// Token: 0x0400000D RID: 13
	[SerializeField]
	private GameObject[] progressIndicators;

	// Token: 0x0400000E RID: 14
	[SerializeField]
	private Image[] databankIcon;

	// Token: 0x0400000F RID: 15
	[SerializeField]
	private LocText databankCountLabel;

	// Token: 0x04000010 RID: 16
	[SerializeField]
	private GameObject meterSection;

	// Token: 0x04000011 RID: 17
	[SerializeField]
	private GameObject lockedSection;
}
