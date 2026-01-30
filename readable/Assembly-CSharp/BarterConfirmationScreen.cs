using System;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CAB RID: 3243
public class BarterConfirmationScreen : KModalScreen
{
	// Token: 0x0600633D RID: 25405 RVA: 0x0024D073 File Offset: 0x0024B273
	protected override void OnActivate()
	{
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.cancelButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x0600633E RID: 25406 RVA: 0x0024D0AC File Offset: 0x0024B2AC
	public void Present(PermitResource permit, bool isPurchase)
	{
		this.Show(true);
		this.ShowContentContainer(true);
		this.ShowLoadingPanel(false);
		this.HideResultPanel();
		if (isPurchase)
		{
			this.itemIcon.transform.SetAsLastSibling();
			this.filamentIcon.transform.SetAsFirstSibling();
		}
		else
		{
			this.itemIcon.transform.SetAsFirstSibling();
			this.filamentIcon.transform.SetAsLastSibling();
		}
		KleiItems.ResponseCallback <>9__1;
		KleiItems.ResponseCallback <>9__2;
		this.confirmButton.onClick += delegate()
		{
			string serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
			if (serverTypeFromPermit == null)
			{
				return;
			}
			this.ShowContentContainer(false);
			this.HideResultPanel();
			this.ShowLoadingPanel(true);
			if (isPurchase)
			{
				string itemType = serverTypeFromPermit;
				KleiItems.ResponseCallback cb;
				if ((cb = <>9__1) == null)
				{
					cb = (<>9__1 = delegate(KleiItems.Result result)
					{
						if (this.IsNullOrDestroyed())
						{
							return;
						}
						this.ShowContentContainer(false);
						this.ShowLoadingPanel(false);
						if (!result.Success)
						{
							this.ShowResultPanel(permit, true, false);
							return;
						}
						this.ShowResultPanel(permit, true, true);
					});
				}
				KleiItems.AddRequestBarterGainItem(itemType, cb);
				return;
			}
			ulong itemInstanceID = KleiItems.GetItemInstanceID(serverTypeFromPermit);
			KleiItems.ResponseCallback cb2;
			if ((cb2 = <>9__2) == null)
			{
				cb2 = (<>9__2 = delegate(KleiItems.Result result)
				{
					if (this.IsNullOrDestroyed())
					{
						return;
					}
					this.ShowContentContainer(false);
					this.ShowLoadingPanel(false);
					if (!result.Success)
					{
						this.ShowResultPanel(permit, false, false);
						return;
					}
					this.ShowResultPanel(permit, false, true);
				});
			}
			KleiItems.AddRequestBarterLoseItem(itemInstanceID, cb2);
		};
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemIcon.GetComponent<Image>().sprite = permitPresentationInfo.sprite;
		this.itemLabel.SetText(permit.Name);
		this.transactionDescriptionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT : UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE);
		this.panelHeaderLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER : UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER);
		this.confirmButtonActionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY : UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL);
		this.confirmButtonFilamentLabel.SetText(isPurchase ? num.ToString() : (UIConstants.ColorPrefixGreen + "+" + num2.ToString() + UIConstants.ColorSuffix));
		this.largeCostLabel.SetText(isPurchase ? ("x" + num.ToString()) : ("x" + num2.ToString()));
	}

	// Token: 0x0600633F RID: 25407 RVA: 0x0024D277 File Offset: 0x0024B477
	private void Update()
	{
		if (this.shouldCloseScreen)
		{
			this.ShowContentContainer(false);
			this.ShowLoadingPanel(false);
			this.HideResultPanel();
			this.Show(false);
		}
	}

	// Token: 0x06006340 RID: 25408 RVA: 0x0024D29C File Offset: 0x0024B49C
	private void ShowContentContainer(bool show)
	{
		this.contentContainer.SetActive(show);
	}

	// Token: 0x06006341 RID: 25409 RVA: 0x0024D2AC File Offset: 0x0024B4AC
	private void ShowLoadingPanel(bool show)
	{
		this.loadingContainer.SetActive(show);
		this.resultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
		if (show)
		{
			this.loadingAnimation.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			this.loadingAnimation.Stop();
		}
		if (!show)
		{
			this.shouldCloseScreen = false;
		}
	}

	// Token: 0x06006342 RID: 25410 RVA: 0x0024D314 File Offset: 0x0024B514
	private void HideResultPanel()
	{
		this.resultContainer.SetActive(false);
	}

	// Token: 0x06006343 RID: 25411 RVA: 0x0024D324 File Offset: 0x0024B524
	private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult)
	{
		this.resultContainer.SetActive(true);
		if (!transationResult)
		{
			this.resultIcon.sprite = Assets.GetSprite("error_message");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
			this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
			this.resultFilamentLabel.SetText("");
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed", false));
			return;
		}
		this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
		if (isPurchase)
		{
			PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
			this.resultIcon.sprite = permitPresentationInfo.sprite;
			this.resultFilamentLabel.SetText("");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed", false));
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		this.resultIcon.sprite = Assets.GetSprite("filament");
		this.resultFilamentLabel.GetComponent<LocText>().SetText("x" + num2.ToString());
		this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed", false));
	}

	// Token: 0x04004355 RID: 17237
	[SerializeField]
	private GameObject itemIcon;

	// Token: 0x04004356 RID: 17238
	[SerializeField]
	private GameObject filamentIcon;

	// Token: 0x04004357 RID: 17239
	[SerializeField]
	private LocText largeCostLabel;

	// Token: 0x04004358 RID: 17240
	[SerializeField]
	private LocText largeQuantityLabel;

	// Token: 0x04004359 RID: 17241
	[SerializeField]
	private LocText itemLabel;

	// Token: 0x0400435A RID: 17242
	[SerializeField]
	private LocText transactionDescriptionLabel;

	// Token: 0x0400435B RID: 17243
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x0400435C RID: 17244
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x0400435D RID: 17245
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400435E RID: 17246
	[SerializeField]
	private LocText panelHeaderLabel;

	// Token: 0x0400435F RID: 17247
	[SerializeField]
	private LocText confirmButtonActionLabel;

	// Token: 0x04004360 RID: 17248
	[SerializeField]
	private LocText confirmButtonFilamentLabel;

	// Token: 0x04004361 RID: 17249
	[SerializeField]
	private LocText resultLabel;

	// Token: 0x04004362 RID: 17250
	[SerializeField]
	private KBatchedAnimController loadingAnimation;

	// Token: 0x04004363 RID: 17251
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x04004364 RID: 17252
	[SerializeField]
	private GameObject loadingContainer;

	// Token: 0x04004365 RID: 17253
	[SerializeField]
	private GameObject resultContainer;

	// Token: 0x04004366 RID: 17254
	[SerializeField]
	private Image resultIcon;

	// Token: 0x04004367 RID: 17255
	[SerializeField]
	private LocText mainResultLabel;

	// Token: 0x04004368 RID: 17256
	[SerializeField]
	private LocText resultFilamentLabel;

	// Token: 0x04004369 RID: 17257
	private bool shouldCloseScreen;
}
