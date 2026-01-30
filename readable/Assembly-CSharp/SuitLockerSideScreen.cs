using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E83 RID: 3715
public class SuitLockerSideScreen : SideScreenContent
{
	// Token: 0x0600763B RID: 30267 RVA: 0x002D19D0 File Offset: 0x002CFBD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600763C RID: 30268 RVA: 0x002D19D8 File Offset: 0x002CFBD8
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SuitLocker>() != null;
	}

	// Token: 0x0600763D RID: 30269 RVA: 0x002D19E8 File Offset: 0x002CFBE8
	public override void SetTarget(GameObject target)
	{
		this.suitLocker = target.GetComponent<SuitLocker>();
		this.initialConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
		this.initialConfigNoSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_NO_SUIT_TOOLTIP);
		this.initialConfigRequestSuitButton.ClearOnClick();
		this.initialConfigRequestSuitButton.onClick += delegate()
		{
			this.suitLocker.ConfigRequestSuit();
		};
		this.initialConfigNoSuitButton.ClearOnClick();
		this.initialConfigNoSuitButton.onClick += delegate()
		{
			this.suitLocker.ConfigNoSuit();
		};
		this.regularConfigRequestSuitButton.ClearOnClick();
		this.regularConfigRequestSuitButton.onClick += delegate()
		{
			if (this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi))
			{
				this.suitLocker.ConfigNoSuit();
				return;
			}
			this.suitLocker.ConfigRequestSuit();
		};
		this.regularConfigDropSuitButton.ClearOnClick();
		this.regularConfigDropSuitButton.onClick += delegate()
		{
			this.suitLocker.DropSuit();
		};
	}

	// Token: 0x0600763E RID: 30270 RVA: 0x002D1AC0 File Offset: 0x002CFCC0
	private void Update()
	{
		bool flag = this.suitLocker.smi.sm.isConfigured.Get(this.suitLocker.smi);
		this.initialConfigScreen.gameObject.SetActive(!flag);
		this.regularConfigScreen.gameObject.SetActive(flag);
		bool flag2 = this.suitLocker.GetStoredOutfit() != null;
		bool flag3 = this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi);
		this.regularConfigRequestSuitButton.isInteractable = !flag2;
		if (!flag3)
		{
			this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT;
			this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
		}
		else
		{
			this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST;
			this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST_TOOLTIP);
		}
		if (flag2)
		{
			this.regularConfigDropSuitButton.isInteractable = true;
			this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_TOOLTIP);
		}
		else
		{
			this.regularConfigDropSuitButton.isInteractable = false;
			this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP);
		}
		KSelectable component = this.suitLocker.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup.Entry statusItem = component.GetStatusItem(Db.Get().StatusItemCategories.Main);
			if (statusItem.item != null)
			{
				this.regularConfigLabel.text = statusItem.item.GetName(statusItem.data);
				this.regularConfigLabel.GetComponentInChildren<ToolTip>().SetSimpleTooltip(statusItem.item.GetTooltip(statusItem.data));
			}
		}
	}

	// Token: 0x040051C5 RID: 20933
	[SerializeField]
	private GameObject initialConfigScreen;

	// Token: 0x040051C6 RID: 20934
	[SerializeField]
	private GameObject regularConfigScreen;

	// Token: 0x040051C7 RID: 20935
	[SerializeField]
	private LocText initialConfigLabel;

	// Token: 0x040051C8 RID: 20936
	[SerializeField]
	private KButton initialConfigRequestSuitButton;

	// Token: 0x040051C9 RID: 20937
	[SerializeField]
	private KButton initialConfigNoSuitButton;

	// Token: 0x040051CA RID: 20938
	[SerializeField]
	private LocText regularConfigLabel;

	// Token: 0x040051CB RID: 20939
	[SerializeField]
	private KButton regularConfigRequestSuitButton;

	// Token: 0x040051CC RID: 20940
	[SerializeField]
	private KButton regularConfigDropSuitButton;

	// Token: 0x040051CD RID: 20941
	private SuitLocker suitLocker;
}
