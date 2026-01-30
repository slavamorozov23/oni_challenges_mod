using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D42 RID: 3394
public class KleiItemDropScreen : KModalScreen
{
	// Token: 0x06006930 RID: 26928 RVA: 0x0027DE8D File Offset: 0x0027C08D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		KleiItemDropScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		if (string.IsNullOrEmpty(KleiAccount.KleiToken))
		{
			base.Show(false);
		}
	}

	// Token: 0x06006931 RID: 26929 RVA: 0x0027DEC5 File Offset: 0x0027C0C5
	protected override void OnActivate()
	{
		KleiItemDropScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06006932 RID: 26930 RVA: 0x0027DED4 File Offset: 0x0027C0D4
	public override void Show(bool show = true)
	{
		this.serverRequestState.Reset();
		if (!show)
		{
			this.animatedLoadingIcon.gameObject.SetActive(false);
			if (this.activePresentationRoutine != null)
			{
				base.StopCoroutine(this.activePresentationRoutine);
			}
			if (this.shouldDoCloseRoutine)
			{
				this.closeButton.gameObject.SetActive(false);
				Updater.RunRoutine(this, this.AnimateScreenOutRoutine()).Then(delegate
				{
					base.Show(false);
				});
				this.shouldDoCloseRoutine = false;
			}
			else
			{
				base.Show(false);
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
		base.Show(true);
	}

	// Token: 0x06006933 RID: 26931 RVA: 0x0027DF91 File Offset: 0x0027C191
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006934 RID: 26932 RVA: 0x0027DFB4 File Offset: 0x0027C1B4
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			return;
		}
		if (PermitItems.HasUnopenedItem())
		{
			this.PresentNextUnopenedItem(true);
			this.shouldDoCloseRoutine = true;
			return;
		}
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.PresentNoItemAvailablePrompt(true);
		this.shouldDoCloseRoutine = true;
	}

	// Token: 0x06006935 RID: 26933 RVA: 0x0027E008 File Offset: 0x0027C208
	public void PresentNextUnopenedItem(bool firstItemPresentation = true)
	{
		int num = 0;
		using (IEnumerator<KleiItems.ItemData> enumerator = PermitItems.IterateInventory().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsOpened)
				{
					num++;
				}
			}
		}
		this.RefreshUnopenedItemsLabel();
		foreach (KleiItems.ItemData itemData in PermitItems.IterateInventory())
		{
			if (!itemData.IsOpened)
			{
				this.PresentItem(itemData, firstItemPresentation, num == 1);
				return;
			}
		}
		this.PresentNoItemAvailablePrompt(false);
	}

	// Token: 0x06006936 RID: 26934 RVA: 0x0027E0B4 File Offset: 0x0027C2B4
	private void RefreshUnopenedItemsLabel()
	{
		int num = 0;
		using (IEnumerator<KleiItems.ItemData> enumerator = PermitItems.IterateInventory().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsOpened)
				{
					num++;
				}
			}
		}
		if (num > 1)
		{
			this.unopenedItemCountLabel.gameObject.SetActive(true);
			this.unopenedItemCountLabel.SetText(UI.ITEM_DROP_SCREEN.UNOPENED_ITEM_COUNT, (float)num);
			return;
		}
		if (num == 1)
		{
			this.unopenedItemCountLabel.gameObject.SetActive(true);
			this.unopenedItemCountLabel.SetText(UI.ITEM_DROP_SCREEN.UNOPENED_ITEM, (float)num);
			return;
		}
		this.unopenedItemCountLabel.gameObject.SetActive(false);
	}

	// Token: 0x06006937 RID: 26935 RVA: 0x0027E170 File Offset: 0x0027C370
	public void PresentItem(KleiItems.ItemData item, bool firstItemPresentation, bool lastItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.THANKS_FOR_PLAYING);
		this.giftAcknowledged = false;
		this.serverRequestState.revealConfirmedByServer = false;
		this.serverRequestState.revealRejectedByServer = false;
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentItemRoutine(item, firstItemPresentation, lastItemPresentation));
		this.acceptButton.ClearOnClick();
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.PRINT_ITEM_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.RequestReveal(item);
		};
		this.acknowledgeButton.onClick += delegate()
		{
			if (this.serverRequestState.revealConfirmedByServer)
			{
				this.giftAcknowledged = true;
			}
		};
	}

	// Token: 0x06006938 RID: 26936 RVA: 0x0027E251 File Offset: 0x0027C451
	private void RequestReveal(KleiItems.ItemData item)
	{
		this.serverRequestState.revealRequested = true;
		PermitItems.QueueRequestOpenOrUnboxItem(item, new KleiItems.ResponseCallback(this.OnOpenItemRequestResponse));
	}

	// Token: 0x06006939 RID: 26937 RVA: 0x0027E274 File Offset: 0x0027C474
	public void OnOpenItemRequestResponse(KleiItems.Result result)
	{
		if (!this.serverRequestState.revealRequested)
		{
			return;
		}
		this.serverRequestState.revealRequested = false;
		if (result.Success)
		{
			this.serverRequestState.revealRejectedByServer = false;
			this.serverRequestState.revealConfirmedByServer = true;
			return;
		}
		this.serverRequestState.revealRejectedByServer = true;
		this.serverRequestState.revealConfirmedByServer = false;
	}

	// Token: 0x0600693A RID: 26938 RVA: 0x0027E2D4 File Offset: 0x0027C4D4
	public void PresentNoItemAvailablePrompt(bool firstItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.noItemAvailableAcknowledged = false;
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.DISMISS_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.noItemAvailableAcknowledged = true;
		};
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentNoItemAvailableRoutine(firstItemPresentation));
	}

	// Token: 0x0600693B RID: 26939 RVA: 0x0027E36B File Offset: 0x0027C56B
	private IEnumerator AnimateScreenInRoutine()
	{
		float scaleFactor = base.transform.parent.GetComponent<CanvasScaler>().scaleFactor;
		float OPEN_WIDTH = (float)Screen.width / scaleFactor;
		float y = Mathf.Clamp((float)Screen.height / scaleFactor, 720f, 900f);
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Open", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, y), 0.5f, Easing.CircInOut, -1f);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(OPEN_WIDTH, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut, -1f);
		this.userMessageLabel.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x0600693C RID: 26940 RVA: 0x0027E37A File Offset: 0x0027C57A
	private IEnumerator AnimateScreenOutRoutine()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Close", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(8f, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut, -1f);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, 0f), 0.25f, Easing.CircInOut, -1f);
		yield break;
	}

	// Token: 0x0600693D RID: 26941 RVA: 0x0027E389 File Offset: 0x0027C589
	private IEnumerator PresentNoItemAvailableRoutine(bool firstItem)
	{
		yield return null;
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		else
		{
			yield return Updater.WaitForSeconds(0.25f);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
		this.acceptButtonRect.gameObject.SetActive(true);
		this.acceptButtonPosition.SetOn(this.acceptButtonRect);
		yield return Updater.WaitForSeconds(0.75f);
		yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
		yield return Updater.Until(() => this.noItemAvailableAcknowledged);
		yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		this.Show(false);
		yield break;
	}

	// Token: 0x0600693E RID: 26942 RVA: 0x0027E39F File Offset: 0x0027C59F
	private IEnumerator PresentItemRoutine(KleiItems.ItemData item, bool firstItem, bool lastItem)
	{
		yield return null;
		if (item.ItemId == 0UL)
		{
			global::Debug.LogError("Could not find dropped item inventory.");
			yield break;
		}
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		this.permitVisualizer.ResetState();
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		if (firstItem)
		{
			this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
			this.acceptButtonRect.gameObject.SetActive(true);
			this.acceptButtonPosition.SetOn(this.acceptButtonRect);
			this.animatedPod.Play("powerup", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.WaitForSeconds(1.25f);
			yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
			yield return Updater.Until(() => this.serverRequestState.revealRequested);
			yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		}
		else
		{
			this.RequestReveal(item);
		}
		this.animatedLoadingIcon.gameObject.rectTransform().anchoredPosition = new Vector2(0f, -352f);
		if (this.animatedLoadingIcon.GetComponent<CanvasGroup>() != null)
		{
			this.animatedLoadingIcon.GetComponent<CanvasGroup>().alpha = 1f;
		}
		yield return new WaitForSecondsRealtime(0.3f);
		if (!this.serverRequestState.revealConfirmedByServer && !this.serverRequestState.revealRejectedByServer)
		{
			this.animatedLoadingIcon.gameObject.SetActive(true);
			this.animatedLoadingIcon.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.Until(() => this.serverRequestState.revealConfirmedByServer || this.serverRequestState.revealRejectedByServer);
			yield return new WaitForSecondsRealtime(2f);
			yield return PresUtil.OffsetFromAndFade(this.animatedLoadingIcon.gameObject.rectTransform(), new Vector2(0f, -512f), 0f, 0.25f, Easing.SmoothStep);
			this.animatedLoadingIcon.gameObject.SetActive(false);
		}
		if (this.serverRequestState.revealRejectedByServer)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.errorMessage.gameObject.SetActive(true);
			yield return Updater.WaitForSeconds(3f);
			this.errorMessage.gameObject.SetActive(false);
		}
		else if (this.serverRequestState.revealConfirmedByServer)
		{
			float num = 1f;
			this.animatedPod.PlaySpeedMultiplier = (firstItem ? 1f : (1f * num));
			this.animatedPod.Play("additional_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.WaitForSeconds(firstItem ? 1f : (1f / num));
			this.animatedPod.PlaySpeedMultiplier = 1f;
			this.RefreshUnopenedItemsLabel();
			DropScreenPresentationInfo info;
			info.UseEquipmentVis = false;
			info.BuildOverride = null;
			info.Sprite = null;
			string name = "";
			string desc = "";
			PermitRarity rarity = PermitRarity.Unknown;
			string categoryString = "";
			string s;
			if (PermitItems.TryGetBoxInfo(item, out name, out desc, out s))
			{
				info.UseEquipmentVis = false;
				info.BuildOverride = null;
				info.Sprite = Assets.GetSprite(s);
				rarity = PermitRarity.Loyalty;
			}
			else
			{
				PermitResource permitResource = Db.Get().Permits.Get(item.Id);
				info.Sprite = permitResource.GetPermitPresentationInfo().sprite;
				info.UseEquipmentVis = (permitResource.Category == PermitCategory.Equipment);
				if (permitResource is EquippableFacadeResource)
				{
					info.BuildOverride = (permitResource as EquippableFacadeResource).BuildOverride;
				}
				name = permitResource.Name;
				desc = permitResource.Description;
				rarity = permitResource.Rarity;
				PermitCategory category = permitResource.Category;
				if (category != PermitCategory.Building)
				{
					if (category != PermitCategory.Artwork)
					{
						if (category != PermitCategory.JoyResponse)
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
						}
						else
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
							if (permitResource is BalloonArtistFacadeResource)
							{
								categoryString = PermitCategories.GetDisplayName(permitResource.Category) + ": " + UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
							}
						}
					}
					else
					{
						categoryString = PermitCategories.GetDisplayName(permitResource.Category);
						if (permitResource is ArtableStage)
						{
							categoryString = Assets.GetPrefab((permitResource as ArtableStage).prefabId).GetProperName();
						}
					}
				}
				else
				{
					categoryString = Assets.GetPrefab((permitResource as BuildingFacadeResource).PrefabID).GetProperName();
				}
			}
			this.permitVisualizer.ConfigureWith(info);
			yield return this.permitVisualizer.AnimateIn();
			KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("GiftItemDrop_Rarity", false), "GiftItemRarity", string.Format("{0}", rarity));
			this.itemNameLabel.SetText(name);
			this.itemDescriptionLabel.SetText(desc);
			this.itemRarityLabel.SetText(rarity.GetLocStringName());
			this.itemCategoryLabel.SetText(categoryString);
			this.itemTextContainerPosition.SetOn(this.itemTextContainer);
			yield return Updater.Parallel(new Updater[]
			{
				PresUtil.OffsetToAndFade(this.itemTextContainer.rectTransform(), animate_offset, 1f, 0.125f, Easing.CircInOut)
			});
			yield return Updater.Until(() => this.giftAcknowledged);
			if (lastItem)
			{
				this.animatedPod.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
				this.animatedPod.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				yield return Updater.Parallel(new Updater[]
				{
					PresUtil.OffsetFromAndFade(this.itemTextContainer.rectTransform(), animate_offset, 0f, 0.125f, Easing.CircInOut)
				});
				this.itemNameLabel.SetText("");
				this.itemDescriptionLabel.SetText("");
				this.itemRarityLabel.SetText("");
				this.itemCategoryLabel.SetText("");
				yield return this.permitVisualizer.AnimateOut();
			}
			else
			{
				this.itemNameLabel.SetText("");
				this.itemDescriptionLabel.SetText("");
				this.itemRarityLabel.SetText("");
				this.itemCategoryLabel.SetText("");
			}
			name = null;
			desc = null;
			categoryString = null;
		}
		this.PresentNextUnopenedItem(false);
		yield break;
	}

	// Token: 0x0600693F RID: 26943 RVA: 0x0027E3C3 File Offset: 0x0027C5C3
	public static bool HasItemsToShow()
	{
		return PermitItems.HasUnopenedItem();
	}

	// Token: 0x0400484B RID: 18507
	[SerializeField]
	private RectTransform shieldMaskRect;

	// Token: 0x0400484C RID: 18508
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400484D RID: 18509
	[Header("Animated Item")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis permitVisualizer;

	// Token: 0x0400484E RID: 18510
	[SerializeField]
	private KBatchedAnimController animatedPod;

	// Token: 0x0400484F RID: 18511
	[SerializeField]
	private LocText userMessageLabel;

	// Token: 0x04004850 RID: 18512
	[SerializeField]
	private LocText unopenedItemCountLabel;

	// Token: 0x04004851 RID: 18513
	[Header("Item Info")]
	[SerializeField]
	private RectTransform itemTextContainer;

	// Token: 0x04004852 RID: 18514
	[SerializeField]
	private LocText itemNameLabel;

	// Token: 0x04004853 RID: 18515
	[SerializeField]
	private LocText itemDescriptionLabel;

	// Token: 0x04004854 RID: 18516
	[SerializeField]
	private LocText itemRarityLabel;

	// Token: 0x04004855 RID: 18517
	[SerializeField]
	private LocText itemCategoryLabel;

	// Token: 0x04004856 RID: 18518
	[Header("Accept Button")]
	[SerializeField]
	private RectTransform acceptButtonRect;

	// Token: 0x04004857 RID: 18519
	[SerializeField]
	private KButton acceptButton;

	// Token: 0x04004858 RID: 18520
	[SerializeField]
	private KBatchedAnimController animatedLoadingIcon;

	// Token: 0x04004859 RID: 18521
	[SerializeField]
	private KButton acknowledgeButton;

	// Token: 0x0400485A RID: 18522
	[SerializeField]
	private LocText errorMessage;

	// Token: 0x0400485B RID: 18523
	private Coroutine activePresentationRoutine;

	// Token: 0x0400485C RID: 18524
	private KleiItemDropScreen.ServerRequestState serverRequestState;

	// Token: 0x0400485D RID: 18525
	private bool giftAcknowledged;

	// Token: 0x0400485E RID: 18526
	private bool noItemAvailableAcknowledged;

	// Token: 0x0400485F RID: 18527
	public static KleiItemDropScreen Instance;

	// Token: 0x04004860 RID: 18528
	private bool shouldDoCloseRoutine;

	// Token: 0x04004861 RID: 18529
	private const float TEXT_AND_BUTTON_ANIMATE_OFFSET_Y = -30f;

	// Token: 0x04004862 RID: 18530
	private PrefabDefinedUIPosition acceptButtonPosition = new PrefabDefinedUIPosition();

	// Token: 0x04004863 RID: 18531
	private PrefabDefinedUIPosition itemTextContainerPosition = new PrefabDefinedUIPosition();

	// Token: 0x02001F79 RID: 8057
	private struct ServerRequestState
	{
		// Token: 0x0600B660 RID: 46688 RVA: 0x003EFE3A File Offset: 0x003EE03A
		public void Reset()
		{
			this.revealRequested = false;
			this.revealConfirmedByServer = false;
			this.revealRejectedByServer = false;
		}

		// Token: 0x040092F8 RID: 37624
		public bool revealRequested;

		// Token: 0x040092F9 RID: 37625
		public bool revealConfirmedByServer;

		// Token: 0x040092FA RID: 37626
		public bool revealRejectedByServer;
	}
}
