using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE1 RID: 3553
public class PrinterceptorScreen : KModalScreen
{
	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x06006FAD RID: 28589 RVA: 0x002A6878 File Offset: 0x002A4A78
	// (set) Token: 0x06006FAE RID: 28590 RVA: 0x002A6880 File Offset: 0x002A4A80
	public Tag selectedEntityTag { get; private set; }

	// Token: 0x06006FAF RID: 28591 RVA: 0x002A6889 File Offset: 0x002A4A89
	protected override void OnActivate()
	{
		PrinterceptorScreen.Instance = this;
		this.Show(false);
		this.closeButton.ClearOnClick();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x06006FB0 RID: 28592 RVA: 0x002A68BC File Offset: 0x002A4ABC
	public void SetTarget(HijackedHeadquarters.Instance target)
	{
		this.target = target;
		this.printButton.ClearOnClick();
		this.printButton.onClick += delegate()
		{
			target.Trigger(1816718186, null);
			this.Show(false);
		};
	}

	// Token: 0x06006FB1 RID: 28593 RVA: 0x002A690B File Offset: 0x002A4B0B
	public override float GetSortKey()
	{
		return 40f;
	}

	// Token: 0x06006FB2 RID: 28594 RVA: 0x002A6912 File Offset: 0x002A4B12
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006FB3 RID: 28595 RVA: 0x002A691C File Offset: 0x002A4B1C
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(true, 0.5f);
			MusicManager.instance.PlaySong("Music_SupplyCloset", false);
			Image[] array = this.dataWalletIcon;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sprite = Def.GetUISprite(DatabankHelper.ID, "ui", false).first;
			}
			this.dataWalletLabel.SetText(GameUtil.SafeStringFormat(UI.PRINTERCEPTORSCREEN.DATABANKS_AVAILABLE, new object[]
			{
				this.target.GetComponent<Storage>().GetAmountAvailable(DatabankHelper.ID).ToString()
			}));
			this.SelectEntity(this.selectedEntityTag);
			using (Dictionary<Tag, MultiToggle>.Enumerator enumerator = this.optionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Tag, MultiToggle> keyValuePair = enumerator.Current;
					this.RefreshOptionButton(keyValuePair.Key);
				}
				return;
			}
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.OnSupplyClosetMenu(false, 1f);
		if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
		{
			MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06006FB4 RID: 28596 RVA: 0x002A6A84 File Offset: 0x002A4C84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SpawnOptionButtons();
	}

	// Token: 0x06006FB5 RID: 28597 RVA: 0x002A6A92 File Offset: 0x002A4C92
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006FB6 RID: 28598 RVA: 0x002A6AB5 File Offset: 0x002A4CB5
	public override void Deactivate()
	{
		this.Show(false);
	}

	// Token: 0x06006FB7 RID: 28599 RVA: 0x002A6AC0 File Offset: 0x002A4CC0
	private void SpawnOptionButtons()
	{
		foreach (KeyValuePair<Tag, List<EggCrackerConfig.EggData>> keyValuePair in EggCrackerConfig.EggsBySpecies)
		{
			foreach (EggCrackerConfig.EggData eggData in keyValuePair.Value)
			{
				if (eggData.isBaseMorph)
				{
					this.SpawnOptionButton(eggData.id);
				}
			}
		}
		List<Tag> list = new List<Tag>();
		list.AddRange(from x in Assets.GetPrefabsWithTag(GameTags.Seed)
		select x.GetComponent<KPrefabID>().PrefabTag);
		list.AddRange(from x in Assets.GetPrefabsWithTag(GameTags.CropSeed)
		select x.GetComponent<KPrefabID>().PrefabTag);
		foreach (Tag id in list)
		{
			this.SpawnOptionButton(id);
		}
		this.SelectEntity("SquirrelEgg");
		this.SpawnOptionButton("BeeBaby");
	}

	// Token: 0x06006FB8 RID: 28600 RVA: 0x002A6C28 File Offset: 0x002A4E28
	private void SpawnOptionButton(Tag id)
	{
		if (this.optionButtons.ContainsKey(id))
		{
			return;
		}
		GameObject prefab = Assets.GetPrefab(id);
		if (prefab == null)
		{
			return;
		}
		if (!Game.IsCorrectDlcActiveForCurrentSave(prefab.GetComponent<KPrefabID>()))
		{
			return;
		}
		if (prefab.HasTag(GameTags.DeprecatedContent))
		{
			return;
		}
		PlantableSeed component = prefab.GetComponent<PlantableSeed>();
		if (component != null)
		{
			GameObject prefab2 = Assets.GetPrefab(component.PlantID);
			if (prefab2 != null && prefab2.HasTag(GameTags.DeprecatedContent))
			{
				return;
			}
		}
		GameObject gameObject = global::Util.KInstantiateUI(this.optionButtonPrefab, this.optionGridContainer.gameObject, true);
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		this.optionButtons.Add(id, component2);
		HierarchyReferences component3 = gameObject.GetComponent<HierarchyReferences>();
		MultiToggle multiToggle = component2;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectEntity(id);
		}));
		component3.GetReference<Image>("FGIcon").sprite = Def.GetUISprite(id, "ui", false).first;
		component3.GetReference<LocText>("NameLabel").text = id.ProperName();
		component3.GetReference<Image>("ProgressOverlay").fillAmount = 0f;
		component3.GetReference<LocText>("CostLabel").text = HijackedHeadquartersConfig.GetDataBankCost(id, this.GetPrintCount(id)).ToString();
		component3.GetReference<Image>("CostIcon").sprite = Def.GetUISprite(DatabankHelper.ID, "ui", false).first;
	}

	// Token: 0x06006FB9 RID: 28601 RVA: 0x002A6DCC File Offset: 0x002A4FCC
	private void RefreshOptionButton(Tag id)
	{
		this.optionButtons[id].GetComponent<HierarchyReferences>().GetReference<LocText>("CostLabel").text = HijackedHeadquartersConfig.GetDataBankCost(id, this.GetPrintCount(id)).ToString();
	}

	// Token: 0x06006FBA RID: 28602 RVA: 0x002A6E10 File Offset: 0x002A5010
	private void SelectEntity(Tag id)
	{
		this.selectedEntityTag = id;
		GameObject prefab = Assets.GetPrefab(this.selectedEntityTag);
		this.selectedEffectsText.text = prefab.GetComponent<InfoDescription>().description;
		this.selectedTitleText.text = prefab.GetProperName();
		this.selectedIcon.sprite = Def.GetUISprite(this.selectedEntityTag, "ui", false).first;
		if (prefab.HasTag(GameTags.Egg))
		{
			Tag spawnedCreature = prefab.GetDef<IncubationMonitor.Def>().spawnedCreature;
			this.selectedIconAlt.sprite = Def.GetUISprite(spawnedCreature, "ui", false).first;
		}
		else if (prefab.HasTag(GameTags.Seed))
		{
			this.selectedIconAlt.sprite = Def.GetUISprite(prefab.GetComponent<PlantableSeed>().PlantID, "ui", false).first;
		}
		else if (prefab.HasTag(GameTags.CropSeed))
		{
			this.selectedIconAlt.sprite = Def.GetUISprite(prefab.GetComponent<PlantableSeed>().PlantID, "ui", false).first;
		}
		else if (prefab.HasTag(GameTags.Creature))
		{
			CreatureBrain component = prefab.GetComponent<CreatureBrain>();
			this.selectedIconAlt.sprite = Def.GetUISprite(component.species, "ui", false).first;
		}
		else
		{
			this.selectedIconAlt.sprite = null;
		}
		foreach (KeyValuePair<Tag, MultiToggle> keyValuePair in this.optionButtons)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((this.selectedEntityTag == keyValuePair.Key) ? 1 : 0);
		}
		this.selectedCostIcon.sprite = Def.GetUISprite(DatabankHelper.ID, "ui", false).first;
		this.selectedCostLabel.SetText(GameUtil.SafeStringFormat(UI.PRINTERCEPTORSCREEN.DATABANKS_COST, new object[]
		{
			HijackedHeadquartersConfig.GetDataBankCost(this.selectedEntityTag, this.GetPrintCount(this.selectedEntityTag)).ToString()
		}));
		this.printButton.isInteractable = (this.target != null && this.target.GetComponent<Storage>().GetAmountAvailable(DatabankHelper.ID) >= (float)HijackedHeadquartersConfig.GetDataBankCost(this.selectedEntityTag, this.GetPrintCount(this.selectedEntityTag)));
		this.printButton.GetComponent<ToolTip>().SetSimpleTooltip(this.printButton.isInteractable ? GameUtil.SafeStringFormat(UI.PRINTERCEPTORSCREEN.PRINT_TOOLTIP, new object[]
		{
			25
		}) : UI.PRINTERCEPTORSCREEN.PRINT_TOOLTIP_DISABLED);
	}

	// Token: 0x06006FBB RID: 28603 RVA: 0x002A70DC File Offset: 0x002A52DC
	private int GetPrintCount(Tag id)
	{
		if (this.target == null || !this.target.printCounts.ContainsKey(id))
		{
			return 0;
		}
		return this.target.printCounts[id];
	}

	// Token: 0x04004C83 RID: 19587
	public static PrinterceptorScreen Instance;

	// Token: 0x04004C84 RID: 19588
	[SerializeField]
	private RectTransform optionGridContainer;

	// Token: 0x04004C85 RID: 19589
	[SerializeField]
	private GameObject optionButtonPrefab;

	// Token: 0x04004C86 RID: 19590
	[SerializeField]
	private LocText selectedTitleText;

	// Token: 0x04004C87 RID: 19591
	[SerializeField]
	private Image selectedIcon;

	// Token: 0x04004C88 RID: 19592
	[SerializeField]
	private Image selectedIconAlt;

	// Token: 0x04004C89 RID: 19593
	[SerializeField]
	private LocText selectedEffectsText;

	// Token: 0x04004C8A RID: 19594
	[SerializeField]
	private LocText selectedFlavourText;

	// Token: 0x04004C8B RID: 19595
	[SerializeField]
	private KButton printButton;

	// Token: 0x04004C8C RID: 19596
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004C8D RID: 19597
	[SerializeField]
	private LocText dataWalletLabel;

	// Token: 0x04004C8E RID: 19598
	[SerializeField]
	private Image[] dataWalletIcon;

	// Token: 0x04004C8F RID: 19599
	[SerializeField]
	private LocText selectedCostLabel;

	// Token: 0x04004C90 RID: 19600
	[SerializeField]
	private Image selectedCostIcon;

	// Token: 0x04004C91 RID: 19601
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04004C92 RID: 19602
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04004C93 RID: 19603
	[SerializeField]
	private Material desatUIMaterial;

	// Token: 0x04004C95 RID: 19605
	private HijackedHeadquarters.Instance target;

	// Token: 0x04004C96 RID: 19606
	private Dictionary<Tag, MultiToggle> optionButtons = new Dictionary<Tag, MultiToggle>();
}
