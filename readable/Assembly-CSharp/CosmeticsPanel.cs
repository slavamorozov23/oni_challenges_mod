using System;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE3 RID: 3299
public class CosmeticsPanel : TargetPanel
{
	// Token: 0x060065DF RID: 26079 RVA: 0x00265BA7 File Offset: 0x00263DA7
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x060065E0 RID: 26080 RVA: 0x00265BAC File Offset: 0x00263DAC
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		BuildingFacade buildingFacade = this.selectedTarget.GetComponent<BuildingFacade>();
		MinionIdentity component = this.selectedTarget.GetComponent<MinionIdentity>();
		this.selectionPanel.OnFacadeSelectionChanged = null;
		this.outfitCategoryButtonContainer.SetActive(component != null);
		if (component != null)
		{
			ClothingOutfitTarget outfitTarget = ClothingOutfitTarget.FromMinion(this.selectedOutfitCategory, component.gameObject);
			this.selectionPanel.SetOutfitTarget(outfitTarget, this.selectedOutfitCategory);
			FacadeSelectionPanel facadeSelectionPanel = this.selectionPanel;
			facadeSelectionPanel.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel.OnFacadeSelectionChanged, new System.Action(delegate()
			{
				if (this.selectionPanel.SelectedFacade == null || this.selectionPanel.SelectedFacade == "DEFAULT_FACADE")
				{
					outfitTarget.WriteItems(this.selectedOutfitCategory, new string[0]);
				}
				else
				{
					outfitTarget.WriteItems(this.selectedOutfitCategory, ClothingOutfitTarget.FromTemplateId(this.selectionPanel.SelectedFacade).ReadItems());
				}
				this.Refresh();
			}));
		}
		else if (buildingFacade != null)
		{
			this.selectionPanel.SetBuildingDef(this.selectedTarget.GetComponent<Building>().Def.PrefabID, this.selectedTarget.GetComponent<BuildingFacade>().CurrentFacade);
			this.selectionPanel.OnFacadeSelectionChanged = null;
			FacadeSelectionPanel facadeSelectionPanel2 = this.selectionPanel;
			facadeSelectionPanel2.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel2.OnFacadeSelectionChanged, new System.Action(delegate()
			{
				if (this.selectionPanel.SelectedFacade == null || this.selectionPanel.SelectedFacade == "DEFAULT_FACADE" || Db.GetBuildingFacades().TryGet(this.selectionPanel.SelectedFacade).IsNullOrDestroyed())
				{
					buildingFacade.ApplyDefaultFacade(true);
				}
				else
				{
					buildingFacade.ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.selectionPanel.SelectedFacade), true);
				}
				this.Refresh();
			}));
		}
		this.Refresh();
	}

	// Token: 0x060065E1 RID: 26081 RVA: 0x00265CEC File Offset: 0x00263EEC
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
	}

	// Token: 0x060065E2 RID: 26082 RVA: 0x00265CF8 File Offset: 0x00263EF8
	public void Refresh()
	{
		UnityEngine.Object component = this.selectedTarget.GetComponent<MinionIdentity>();
		BuildingFacade component2 = this.selectedTarget.GetComponent<BuildingFacade>();
		if (component != null)
		{
			ClothingOutfitTarget outfit = ClothingOutfitTarget.FromMinion(this.selectedOutfitCategory, this.selectedTarget);
			this.editButton.gameObject.SetActive(true);
			this.mannequin.gameObject.SetActive(true);
			this.mannequin.SetOutfit(outfit);
			Vector2 sizeDelta = new Vector2(0f, 0f);
			if (outfit.OutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				sizeDelta = new Vector2(-8f, -8f);
			}
			else if (outfit.OutfitType == ClothingOutfitUtility.OutfitType.JetSuit)
			{
				sizeDelta = new Vector2(-12f, -12f);
			}
			this.mannequin.rectTransform().sizeDelta = sizeDelta;
			this.buildingIcon.gameObject.SetActive(false);
			this.editButton.ClearOnClick();
			this.editButton.onClick += this.OnClickEditOutfit;
			this.nameLabel.SetText(outfit.ReadName());
			this.descriptionLabel.SetText("");
		}
		else if (component2 != null)
		{
			this.editButton.gameObject.SetActive(false);
			this.mannequin.gameObject.SetActive(false);
			this.buildingIcon.gameObject.SetActive(true);
			if (component2.CurrentFacade != null && component2.CurrentFacade != "DEFAULT_FACADE" && !Db.GetBuildingFacades().TryGet(component2.CurrentFacade).IsNullOrDestroyed())
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(component2.CurrentFacade);
				this.nameLabel.SetText(buildingFacadeResource.Name);
				this.descriptionLabel.SetText(buildingFacadeResource.Description);
				this.buildingIcon.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile), "ui", false, "");
			}
			else
			{
				string prefabID = component2.GetComponent<Building>().Def.PrefabID;
				StringEntry stringEntry;
				Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					prefabID.ToString().ToUpperInvariant(),
					".FACADES.DEFAULT_",
					prefabID.ToString().ToUpperInvariant(),
					".NAME"
				}), out stringEntry);
				if (stringEntry == null)
				{
					Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".NAME", out stringEntry);
				}
				StringEntry stringEntry2;
				Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					prefabID.ToString().ToUpperInvariant(),
					".FACADES.DEFAULT_",
					prefabID.ToString().ToUpperInvariant(),
					".DESC"
				}), out stringEntry2);
				if (stringEntry2 == null)
				{
					Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".DESC", out stringEntry2);
				}
				this.nameLabel.SetText((stringEntry != null) ? stringEntry : "");
				this.descriptionLabel.SetText((stringEntry2 != null) ? stringEntry2 : "");
				this.buildingIcon.sprite = Def.GetUISprite(prefabID, "ui", false).first;
			}
		}
		this.RefreshOutfitCategories();
		this.selectionPanel.Refresh();
	}

	// Token: 0x060065E3 RID: 26083 RVA: 0x0026604C File Offset: 0x0026424C
	public void OnClickEditOutfit()
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
		MinionBrowserScreenConfig.MinionInstances(this.selectedTarget).ApplyAndOpenScreen(delegate
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
		}, this.selectedOutfitCategory);
	}

	// Token: 0x060065E4 RID: 26084 RVA: 0x002660AC File Offset: 0x002642AC
	private void RefreshOutfitCategories()
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, GameObject> keyValuePair in this.outfitCategories)
		{
			global::Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.outfitCategories.Clear();
		string[] array = new string[]
		{
			"outfit",
			"atmosuit"
		};
		Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, string>();
		dictionary.Add(ClothingOutfitUtility.OutfitType.Clothing, UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_OUTFIT);
		dictionary.Add(ClothingOutfitUtility.OutfitType.AtmoSuit, UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_ATMOSUIT);
		dictionary.Add(ClothingOutfitUtility.OutfitType.JetSuit, UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_JETSUIT);
		for (int i = 0; i < 4; i++)
		{
			if (i != 1)
			{
				int idx = i;
				GameObject gameObject = global::Util.KInstantiateUI(this.outfitCategoryButtonPrefab, this.outfitCategoryButtonContainer, true);
				this.outfitCategories.Add((ClothingOutfitUtility.OutfitType)idx, gameObject);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(dictionary[(ClothingOutfitUtility.OutfitType)i]);
				component.GetReference<Image>("FG").sprite = Assets.GetSprite(CosmeticsPanel.categoryIcons[(ClothingOutfitUtility.OutfitType)i]);
				MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
				component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
				{
					this.selectedOutfitCategory = (ClothingOutfitUtility.OutfitType)idx;
					this.Refresh();
					this.selectionPanel.SelectedOutfitCategory = this.selectedOutfitCategory;
				}));
				component2.ChangeState((this.selectedOutfitCategory == (ClothingOutfitUtility.OutfitType)idx) ? 1 : 0);
			}
		}
	}

	// Token: 0x0400456D RID: 17773
	[SerializeField]
	private GameObject cosmeticSlotContainer;

	// Token: 0x0400456E RID: 17774
	[SerializeField]
	private FacadeSelectionPanel selectionPanel;

	// Token: 0x0400456F RID: 17775
	[SerializeField]
	private LocText nameLabel;

	// Token: 0x04004570 RID: 17776
	[SerializeField]
	private LocText descriptionLabel;

	// Token: 0x04004571 RID: 17777
	[SerializeField]
	private KButton editButton;

	// Token: 0x04004572 RID: 17778
	[SerializeField]
	private UIMannequin mannequin;

	// Token: 0x04004573 RID: 17779
	[SerializeField]
	private Image buildingIcon;

	// Token: 0x04004574 RID: 17780
	[SerializeField]
	private Dictionary<ClothingOutfitUtility.OutfitType, GameObject> outfitCategories = new Dictionary<ClothingOutfitUtility.OutfitType, GameObject>();

	// Token: 0x04004575 RID: 17781
	[SerializeField]
	private GameObject outfitCategoryButtonPrefab;

	// Token: 0x04004576 RID: 17782
	[SerializeField]
	private GameObject outfitCategoryButtonContainer;

	// Token: 0x04004577 RID: 17783
	private ClothingOutfitUtility.OutfitType selectedOutfitCategory;

	// Token: 0x04004578 RID: 17784
	private static Dictionary<ClothingOutfitUtility.OutfitType, string> categoryIcons = new Dictionary<ClothingOutfitUtility.OutfitType, string>
	{
		{
			ClothingOutfitUtility.OutfitType.Clothing,
			"icon_inventory_equipment"
		},
		{
			ClothingOutfitUtility.OutfitType.AtmoSuit,
			"icon_inventory_atmosuits"
		},
		{
			ClothingOutfitUtility.OutfitType.JetSuit,
			"icon_inventory_jetsuits"
		}
	};
}
