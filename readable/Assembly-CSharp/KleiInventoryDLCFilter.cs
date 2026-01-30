using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D3F RID: 3391
public class KleiInventoryDLCFilter : KMonoBehaviour
{
	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x060068EF RID: 26863 RVA: 0x0027B6F7 File Offset: 0x002798F7
	// (set) Token: 0x060068F0 RID: 26864 RVA: 0x0027B6FF File Offset: 0x002798FF
	[HideInInspector]
	public string SelectedDLCID { get; set; }

	// Token: 0x060068F1 RID: 26865 RVA: 0x0027B708 File Offset: 0x00279908
	private void ShowDropdown(bool show)
	{
		this.dlcFilterButtonContainer.gameObject.SetActive(show);
	}

	// Token: 0x060068F2 RID: 26866 RVA: 0x0027B71B File Offset: 0x0027991B
	public void ResetToDefault()
	{
		this.SetDLCFilter(null);
	}

	// Token: 0x060068F3 RID: 26867 RVA: 0x0027B724 File Offset: 0x00279924
	public void ConfigButtons()
	{
		this.dropdownButton.ClearOnClick();
		this.dropdownButton.onClick += delegate()
		{
			this.ShowDropdown(!this.dlcFilterButtonContainer.gameObject.activeSelf);
		};
		this.MakeButton(null);
		List<string> list = new List<string>(DlcManager.GetActiveDLCIds());
		for (int i = list.Count - 1; i >= 0; i--)
		{
			this.MakeButton(list[i]);
		}
		this.SetDLCFilter(null);
	}

	// Token: 0x060068F4 RID: 26868 RVA: 0x0027B78C File Offset: 0x0027998C
	private void MakeButton(string dlcID)
	{
		HierarchyReferences component = Util.KInstantiateUI(this.dlcFilterButtonPrefab, this.dlcFilterButtonContainer.gameObject, true).GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Logo").sprite = ((dlcID == null) ? Assets.GetSprite("ONI_mini_logo") : Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcID)));
		component.GetReference<Image>("Stripe").sprite = Assets.GetSprite(DlcManager.GetDlcBannerSprite(dlcID));
		component.GetReference<Image>("Stripe").color = ((dlcID == null) ? Color.white : DlcManager.GetDlcBannerColor(dlcID));
		component.GetReference<KButton>("Button").ClearOnClick();
		component.GetReference<KButton>("Button").onClick += delegate()
		{
			this.SetDLCFilter(dlcID);
			this.ShowDropdown(false);
		};
		this.ShowDropdown(false);
	}

	// Token: 0x060068F5 RID: 26869 RVA: 0x0027B888 File Offset: 0x00279A88
	private void SetDLCFilter(string DLCID)
	{
		this.SelectedDLCID = DLCID;
		System.Action action = this.onDLCFilterChanged;
		if (action != null)
		{
			action();
		}
		this.selectedDLCIcon.sprite = ((DLCID == null) ? Assets.GetSprite("ONI_mini_logo") : Assets.GetSprite(DlcManager.GetDlcSmallLogo(DLCID)));
		this.selectedDLCStripe.color = ((DLCID == null) ? Color.white : DlcManager.GetDlcBannerColor(DLCID));
		this.dropdownButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.SafeStringFormat(UI.KLEI_INVENTORY_SCREEN.TOOLTIP_DLC_FILTER, new object[]
		{
			(DLCID == null) ? UI.KLEI_INVENTORY_SCREEN.TOOLTIP_DLC_FILTER_ALL : DlcManager.GetDlcTitle(DLCID)
		}));
	}

	// Token: 0x060068F6 RID: 26870 RVA: 0x0027B92F File Offset: 0x00279B2F
	public void HideDropdown()
	{
		this.ShowDropdown(false);
	}

	// Token: 0x060068F7 RID: 26871 RVA: 0x0027B938 File Offset: 0x00279B38
	public bool IsDropdownVisible()
	{
		return this.dlcFilterButtonContainer.gameObject.activeSelf;
	}

	// Token: 0x04004812 RID: 18450
	[SerializeField]
	private Transform dlcFilterButtonContainer;

	// Token: 0x04004813 RID: 18451
	[SerializeField]
	private GameObject dlcFilterButtonPrefab;

	// Token: 0x04004814 RID: 18452
	[SerializeField]
	private Image selectedDLCIcon;

	// Token: 0x04004815 RID: 18453
	[SerializeField]
	private Image selectedDLCStripe;

	// Token: 0x04004816 RID: 18454
	[SerializeField]
	private KButton dropdownButton;

	// Token: 0x04004817 RID: 18455
	public System.Action onDLCFilterChanged;
}
