using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D63 RID: 3427
public class LockerMenuScreen : KModalScreen
{
	// Token: 0x06006A22 RID: 27170 RVA: 0x00282027 File Offset: 0x00280227
	protected override void OnActivate()
	{
		LockerMenuScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06006A23 RID: 27171 RVA: 0x00282036 File Offset: 0x00280236
	public override float GetSortKey()
	{
		return 40f;
	}

	// Token: 0x06006A24 RID: 27172 RVA: 0x0028203D File Offset: 0x0028023D
	public void ShowInventoryScreen()
	{
		if (!base.isActiveAndEnabled)
		{
			this.Show(true);
		}
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen, null);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "inventory", true);
	}

	// Token: 0x06006A25 RID: 27173 RVA: 0x00282080 File Offset: 0x00280280
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.buttonInventory;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.ShowInventoryScreen();
		}));
		MultiToggle multiToggle2 = this.buttonDuplicants;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			MinionBrowserScreenConfig.Personalities(default(Option<Personality>)).ApplyAndOpenScreen(null, ClothingOutfitUtility.OutfitType.Clothing);
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "dupe", true);
		}));
		MultiToggle multiToggle3 = this.buttonOutfitBroswer;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			OutfitBrowserScreenConfig.Mannequin().ApplyAndOpenScreen();
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "wardrobe", true);
		}));
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.ConfigureHoverForButton(this.buttonInventory, UI.LOCKER_MENU.BUTTON_INVENTORY_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonDuplicants, UI.LOCKER_MENU.BUTTON_DUPLICANTS_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonOutfitBroswer, UI.LOCKER_MENU.BUTTON_OUTFITS_DESCRIPTION, true);
		this.descriptionArea.text = UI.LOCKER_MENU.DEFAULT_DESCRIPTION;
		this.CreateDLCLogos();
	}

	// Token: 0x06006A26 RID: 27174 RVA: 0x002821A8 File Offset: 0x002803A8
	private void ConfigureHoverForButton(MultiToggle toggle, string desc, bool useHoverColor = true)
	{
		LockerMenuScreen.<>c__DisplayClass19_0 CS$<>8__locals1 = new LockerMenuScreen.<>c__DisplayClass19_0();
		CS$<>8__locals1.useHoverColor = useHoverColor;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.defaultColor = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		CS$<>8__locals1.hoverColor = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		toggle.onEnter = null;
		toggle.onExit = null;
		toggle.onEnter = (System.Action)Delegate.Combine(toggle.onEnter, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverEnterFn|0(toggle, desc));
		toggle.onExit = (System.Action)Delegate.Combine(toggle.onExit, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverExitFn|1(toggle));
	}

	// Token: 0x06006A27 RID: 27175 RVA: 0x00282250 File Offset: 0x00280450
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(true, 0.5f);
			MusicManager.instance.PlaySong("Music_SupplyCloset", false);
			ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(new KleiAccount.GetUserIDdelegate(this.TriggerShouldRefreshClaimItems), false);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		this.RefreshClaimItemsButton();
	}

	// Token: 0x06006A28 RID: 27176 RVA: 0x00282303 File Offset: 0x00280503
	private void TriggerShouldRefreshClaimItems()
	{
		this.refreshRequested = true;
	}

	// Token: 0x06006A29 RID: 27177 RVA: 0x0028230C File Offset: 0x0028050C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (KPrivacyPrefs.instance.disableDataCollection)
		{
			this.noConnectionIcon.GetComponent<ToolTip>().SetSimpleTooltip(UI.LOCKER_MENU.OFFLINE_ICON_TOOLTIP_DATA_COLLECTIONS);
		}
	}

	// Token: 0x06006A2A RID: 27178 RVA: 0x0028233A File Offset: 0x0028053A
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x06006A2B RID: 27179 RVA: 0x00282344 File Offset: 0x00280544
	private void RefreshClaimItemsButton()
	{
		this.noConnectionIcon.SetActive(!ThreadedHttps<KleiAccount>.Instance.HasValidTicket());
		this.refreshRequested = false;
		bool hasClaimable = PermitItems.HasUnopenedItem();
		this.dropsAvailableNotification.SetActive(hasClaimable);
		this.buttonClaimItems.ChangeState(hasClaimable ? 0 : 1);
		this.buttonClaimItems.GetComponent<HierarchyReferences>().GetReference<Image>("FGIcon").material = (hasClaimable ? null : this.desatUIMaterial);
		this.buttonClaimItems.onClick = null;
		MultiToggle multiToggle = this.buttonClaimItems;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (!hasClaimable)
			{
				return;
			}
			UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
			this.Show(false);
		}));
		this.ConfigureHoverForButton(this.buttonClaimItems, hasClaimable ? UI.LOCKER_MENU.BUTTON_CLAIM_DESCRIPTION : UI.LOCKER_MENU.BUTTON_CLAIM_NONE_DESCRIPTION, hasClaimable);
	}

	// Token: 0x06006A2C RID: 27180 RVA: 0x0028243C File Offset: 0x0028063C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006A2D RID: 27181 RVA: 0x002824B1 File Offset: 0x002806B1
	private void Update()
	{
		if (this.refreshRequested)
		{
			this.RefreshClaimItemsButton();
		}
	}

	// Token: 0x06006A2E RID: 27182 RVA: 0x002824C4 File Offset: 0x002806C4
	private void CreateDLCLogos()
	{
		using (Dictionary<string, DlcManager.DlcInfo>.Enumerator enumerator = DlcManager.DLC_PACKS.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, DlcManager.DlcInfo> dlc = enumerator.Current;
				if (dlc.Value.isCosmetic)
				{
					GameObject gameObject = global::Util.KInstantiateUI(this.DLCLogoPrefab, this.DLCLogoContainer, true);
					Image component = gameObject.GetComponent<Image>();
					component.sprite = Assets.GetSprite(DlcManager.GetDlcLargeLogo(dlc.Key));
					component.material = (DlcManager.IsContentSubscribed(dlc.Key) ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
					gameObject.GetComponent<MultiToggle>().states[0].sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlc.Key));
					string text = DlcManager.GetDlcTitle(dlc.Key);
					if (!DlcManager.IsContentSubscribed(dlc.Key))
					{
						text = string.Concat(new string[]
						{
							text,
							"\n\n",
							UI.FRONTEND.MAINMENU.WISHLIST_AD,
							"\n\n",
							UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP
						});
					}
					else
					{
						text = string.Concat(new string[]
						{
							text,
							"\n\n",
							UI.FRONTEND.MAINMENU.DLC.CONTENT_INSTALLED_LABEL,
							"\n\n",
							UI.FRONTEND.MAINMENU.DLC.COSMETIC_CONTENT_ACTIVE_TOOLTIP,
							"\n\n",
							UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP
						});
					}
					gameObject.GetComponent<ToolTip>().SetSimpleTooltip(text);
					MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
					component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
					{
						App.OpenWebURL(this.GetCosmeticDLCStoreURL(dlc.Key));
					}));
					gameObject.gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x06006A2F RID: 27183 RVA: 0x002826CC File Offset: 0x002808CC
	private string GetCosmeticDLCStoreURL(string dlcId)
	{
		if (DistributionPlatform.Initialized || Application.isEditor)
		{
			if (DistributionPlatform.Inst.Name == "Steam")
			{
				if (dlcId == "COSMETIC1_ID")
				{
					return "https://store.steampowered.com/app/4157740/Oxygen_Not_Included_Neutronium_Cosmetics_Pack/";
				}
				return "";
			}
			else if (DistributionPlatform.Inst.Name == "Epic")
			{
				if (dlcId == "COSMETIC1_ID")
				{
					return "https://store.epicgames.com/p/oxygen-not-included-oxygen-not-included-neutronium-cosmetics-pack-d9e8af";
				}
				return "";
			}
			else if (DistributionPlatform.Inst.Name == "Rail")
			{
				if (dlcId == "COSMETIC1_ID")
				{
					return "https://www.wegame.com.cn/store/2002628";
				}
				return "";
			}
		}
		return "";
	}

	// Token: 0x040048EB RID: 18667
	public static LockerMenuScreen Instance;

	// Token: 0x040048EC RID: 18668
	[SerializeField]
	private MultiToggle buttonInventory;

	// Token: 0x040048ED RID: 18669
	[SerializeField]
	private MultiToggle buttonDuplicants;

	// Token: 0x040048EE RID: 18670
	[SerializeField]
	private MultiToggle buttonOutfitBroswer;

	// Token: 0x040048EF RID: 18671
	[SerializeField]
	private MultiToggle buttonClaimItems;

	// Token: 0x040048F0 RID: 18672
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x040048F1 RID: 18673
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040048F2 RID: 18674
	[SerializeField]
	private GameObject dropsAvailableNotification;

	// Token: 0x040048F3 RID: 18675
	[SerializeField]
	private GameObject noConnectionIcon;

	// Token: 0x040048F4 RID: 18676
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x040048F5 RID: 18677
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x040048F6 RID: 18678
	[SerializeField]
	private Material desatUIMaterial;

	// Token: 0x040048F7 RID: 18679
	private bool refreshRequested;

	// Token: 0x040048F8 RID: 18680
	[SerializeField]
	private GameObject DLCLogoContainer;

	// Token: 0x040048F9 RID: 18681
	[SerializeField]
	private GameObject DLCLogoPrefab;
}
