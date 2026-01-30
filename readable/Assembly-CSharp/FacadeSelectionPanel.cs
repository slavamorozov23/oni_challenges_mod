using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D0B RID: 3339
public class FacadeSelectionPanel : KMonoBehaviour
{
	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x0600674F RID: 26447 RVA: 0x0026F40A File Offset: 0x0026D60A
	private int GridLayoutConstraintCount
	{
		get
		{
			if (this.gridLayout != null)
			{
				return this.gridLayout.constraintCount;
			}
			return 3;
		}
	}

	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x06006751 RID: 26449 RVA: 0x0026F436 File Offset: 0x0026D636
	// (set) Token: 0x06006750 RID: 26448 RVA: 0x0026F427 File Offset: 0x0026D627
	public ClothingOutfitUtility.OutfitType SelectedOutfitCategory
	{
		get
		{
			return this.selectedOutfitCategory;
		}
		set
		{
			this.selectedOutfitCategory = value;
			this.Refresh();
		}
	}

	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x06006752 RID: 26450 RVA: 0x0026F43E File Offset: 0x0026D63E
	public string SelectedBuildingDefID
	{
		get
		{
			return this.selectedBuildingDefID;
		}
	}

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06006753 RID: 26451 RVA: 0x0026F446 File Offset: 0x0026D646
	// (set) Token: 0x06006754 RID: 26452 RVA: 0x0026F450 File Offset: 0x0026D650
	public string SelectedFacade
	{
		get
		{
			return this._selectedFacade;
		}
		set
		{
			if (this._selectedFacade != value)
			{
				this._selectedFacade = value;
				FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
				if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
				{
					if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
					{
						this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
					}
				}
				else
				{
					this.RefreshTogglesForBuilding();
				}
				if (this.OnFacadeSelectionChanged != null)
				{
					this.OnFacadeSelectionChanged();
				}
			}
		}
	}

	// Token: 0x06006755 RID: 26453 RVA: 0x0026F4A9 File Offset: 0x0026D6A9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayout = this.toggleContainer.GetComponent<GridLayoutGroup>();
	}

	// Token: 0x06006756 RID: 26454 RVA: 0x0026F4C2 File Offset: 0x0026D6C2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.getMoreButton.ClearOnClick();
		this.getMoreButton.onClick += LockerMenuScreen.Instance.ShowInventoryScreen;
	}

	// Token: 0x06006757 RID: 26455 RVA: 0x0026F4F0 File Offset: 0x0026D6F0
	public void SetBuildingDef(string defID, string currentFacadeID = null)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.BuildingFacade;
		this.ClearToggles();
		this.selectedBuildingDefID = defID;
		this.SelectedFacade = ((currentFacadeID == null) ? "DEFAULT_FACADE" : currentFacadeID);
		this.RefreshTogglesForBuilding();
		if (this.hideWhenEmpty)
		{
			base.gameObject.SetActive(Assets.GetBuildingDef(defID).AvailableFacades.Count != 0);
		}
	}

	// Token: 0x06006758 RID: 26456 RVA: 0x0026F54E File Offset: 0x0026D74E
	public void SetOutfitTarget(ClothingOutfitTarget outfitTarget, ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.MinionOutfit;
		this.ClearToggles();
		this.SelectedFacade = outfitTarget.OutfitId;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06006759 RID: 26457 RVA: 0x0026F578 File Offset: 0x0026D778
	private void ClearToggles()
	{
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			this.pooledFacadeToggles.Add(keyValuePair.Value.gameObject);
			keyValuePair.Value.gameObject.SetActive(false);
		}
		this.activeFacadeToggles.Clear();
	}

	// Token: 0x0600675A RID: 26458 RVA: 0x0026F600 File Offset: 0x0026D800
	public void Refresh()
	{
		FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
		if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
		{
			if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
			{
				this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
			}
		}
		else
		{
			this.RefreshTogglesForBuilding();
		}
		this.getMoreButton.gameObject.SetActive(this.showGetMoreButton);
		if (this.useDummyPlaceholder)
		{
			for (int i = 0; i < this.dummyGridPlaceholders.Count; i++)
			{
				this.dummyGridPlaceholders[i].SetActive(false);
			}
			int num = 0;
			for (int j = 0; j < this.toggleContainer.transform.childCount; j++)
			{
				if (this.toggleContainer.GetChild(j).gameObject.activeInHierarchy)
				{
					num++;
				}
			}
			this.getMoreButton.transform.SetAsLastSibling();
			if (num % this.GridLayoutConstraintCount != 0)
			{
				for (int k = 0; k < this.GridLayoutConstraintCount - 1; k++)
				{
					this.dummyGridPlaceholders[k].SetActive(k < this.GridLayoutConstraintCount - num % this.GridLayoutConstraintCount);
					this.dummyGridPlaceholders[k].transform.SetAsLastSibling();
				}
				return;
			}
		}
		else
		{
			this.getMoreButton.transform.SetAsLastSibling();
		}
	}

	// Token: 0x0600675B RID: 26459 RVA: 0x0026F730 File Offset: 0x0026D930
	private void RefreshTogglesForOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == outfitType
		select outfit;
		List<string> list = new List<string>();
		using (Dictionary<string, FacadeSelectionPanel.FacadeToggle>.Enumerator enumerator = this.activeFacadeToggles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> toggle = enumerator.Current;
				if (!enumerable.Any((ClothingOutfitTarget match) => match.OutfitId == toggle.Key))
				{
					list.Add(toggle.Key);
				}
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultOutfitToggle();
		enumerable = enumerable.StableSort((ClothingOutfitTarget a, ClothingOutfitTarget b) => a.OutfitId.CompareTo(b.OutfitId));
		foreach (ClothingOutfitTarget clothingOutfitTarget in enumerable)
		{
			if (!clothingOutfitTarget.DoesContainLockedItems())
			{
				this.AddNewOutfitToggle(clothingOutfitTarget.OutfitId, false);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			keyValuePair.Value.multiToggle.ChangeState((this.SelectedFacade != null && this.SelectedFacade == keyValuePair.Key) ? 1 : 0);
		}
		this.RefreshHeight();
	}

	// Token: 0x0600675C RID: 26460 RVA: 0x0026F954 File Offset: 0x0026DB54
	private void RefreshTogglesForBuilding()
	{
		BuildingDef buildingDef = Assets.GetBuildingDef(this.selectedBuildingDefID);
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			if (!buildingDef.AvailableFacades.Contains(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultBuildingFacadeToggle();
		foreach (string text in buildingDef.AvailableFacades)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(text);
			if (permitResource != null && permitResource.IsUnlocked())
			{
				this.AddNewBuildingToggle(text);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair2 in this.activeFacadeToggles)
		{
			keyValuePair2.Value.multiToggle.ChangeState((this.SelectedFacade == keyValuePair2.Key) ? 1 : 0);
		}
		this.activeFacadeToggles["DEFAULT_FACADE"].gameObject.transform.SetAsFirstSibling();
		this.RefreshHeight();
	}

	// Token: 0x0600675D RID: 26461 RVA: 0x0026FB5C File Offset: 0x0026DD5C
	private void RefreshHeight()
	{
		if (this.usesScrollRect)
		{
			LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
			component.minHeight = (float)(58 * Math.Clamp(Mathf.CeilToInt((float)this.activeFacadeToggles.Count / 5f), 1, 6));
			component.preferredHeight = component.minHeight;
		}
	}

	// Token: 0x0600675E RID: 26462 RVA: 0x0026FBAF File Offset: 0x0026DDAF
	private void AddDefaultBuildingFacadeToggle()
	{
		this.AddNewBuildingToggle("DEFAULT_FACADE");
	}

	// Token: 0x0600675F RID: 26463 RVA: 0x0026FBBC File Offset: 0x0026DDBC
	private void AddDefaultOutfitToggle()
	{
		this.AddNewOutfitToggle("DEFAULT_FACADE", true);
	}

	// Token: 0x06006760 RID: 26464 RVA: 0x0026FBCC File Offset: 0x0026DDCC
	private void AddNewBuildingToggle(string facadeID)
	{
		if (this.activeFacadeToggles.ContainsKey(facadeID))
		{
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(facadeID, this.selectedBuildingDefID, gameObject);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
	}

	// Token: 0x06006761 RID: 26465 RVA: 0x0026FC94 File Offset: 0x0026DE94
	private void AddNewOutfitToggle(string outfitID, bool setAsFirstSibling = false)
	{
		if (this.activeFacadeToggles.ContainsKey(outfitID))
		{
			if (setAsFirstSibling)
			{
				this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
			}
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(outfitID, gameObject, this.selectedOutfitCategory);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
		if (setAsFirstSibling)
		{
			this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
		}
	}

	// Token: 0x06006762 RID: 26466 RVA: 0x0026FD9D File Offset: 0x0026DF9D
	private void SelectFacade(string id)
	{
		this.SelectedFacade = id;
	}

	// Token: 0x040046C1 RID: 18113
	[SerializeField]
	private GameObject togglePrefab;

	// Token: 0x040046C2 RID: 18114
	[SerializeField]
	private RectTransform toggleContainer;

	// Token: 0x040046C3 RID: 18115
	[SerializeField]
	private bool usesScrollRect;

	// Token: 0x040046C4 RID: 18116
	[SerializeField]
	private LayoutElement scrollRect;

	// Token: 0x040046C5 RID: 18117
	private Dictionary<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeSelectionPanel.FacadeToggle>();

	// Token: 0x040046C6 RID: 18118
	private List<GameObject> pooledFacadeToggles = new List<GameObject>();

	// Token: 0x040046C7 RID: 18119
	[SerializeField]
	private KButton getMoreButton;

	// Token: 0x040046C8 RID: 18120
	[SerializeField]
	private bool showGetMoreButton;

	// Token: 0x040046C9 RID: 18121
	[SerializeField]
	private bool hideWhenEmpty = true;

	// Token: 0x040046CA RID: 18122
	[SerializeField]
	private bool useDummyPlaceholder;

	// Token: 0x040046CB RID: 18123
	private GridLayoutGroup gridLayout;

	// Token: 0x040046CC RID: 18124
	[SerializeField]
	private List<GameObject> dummyGridPlaceholders;

	// Token: 0x040046CD RID: 18125
	public System.Action OnFacadeSelectionChanged;

	// Token: 0x040046CE RID: 18126
	private ClothingOutfitUtility.OutfitType selectedOutfitCategory;

	// Token: 0x040046CF RID: 18127
	private string selectedBuildingDefID;

	// Token: 0x040046D0 RID: 18128
	private FacadeSelectionPanel.ConfigType currentConfigType;

	// Token: 0x040046D1 RID: 18129
	private string _selectedFacade;

	// Token: 0x040046D2 RID: 18130
	public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

	// Token: 0x02001F3B RID: 7995
	private struct FacadeToggle
	{
		// Token: 0x0600B5CA RID: 46538 RVA: 0x003EEA94 File Offset: 0x003ECC94
		public FacadeToggle(string buildingFacadeID, string buildingPrefabID, GameObject gameObject)
		{
			this.id = buildingFacadeID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<UIMannequin>("Mannequin").gameObject.SetActive(false);
			component.GetReference<Image>("FGImage").SetAlpha(1f);
			Sprite sprite;
			string simpleTooltip;
			string dlcId;
			if (buildingFacadeID != "DEFAULT_FACADE")
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(buildingFacadeID);
				sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile), "ui", false, "");
				simpleTooltip = KleiItemsUI.GetTooltipStringFor(buildingFacadeResource);
				dlcId = buildingFacadeResource.GetDlcIdFrom();
			}
			else
			{
				GameObject prefab = Assets.GetPrefab(buildingPrefabID);
				Building component2 = prefab.GetComponent<Building>();
				StringEntry entry;
				string text;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".NAME"
				}), out entry))
				{
					text = entry;
				}
				else if (component2 != null)
				{
					text = component2.Def.Name;
				}
				else
				{
					text = prefab.GetProperName();
				}
				StringEntry entry2;
				string str;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".DESC"
				}), out entry2))
				{
					str = entry2;
				}
				else if (component2 != null)
				{
					str = component2.Def.Desc;
				}
				else
				{
					str = "";
				}
				sprite = Def.GetUISprite(buildingPrefabID, "ui", false).first;
				simpleTooltip = KleiItemsUI.WrapAsToolTipTitle(text) + "\n" + str;
				dlcId = null;
			}
			component.GetReference<Image>("FGImage").sprite = sprite;
			this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
			Image reference = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference.gameObject.SetActive(true);
				reference.sprite = Assets.GetSprite(DlcManager.GetDlcBannerSprite(dlcId));
				reference.color = DlcManager.GetDlcBannerColor(dlcId);
				return;
			}
			reference.gameObject.SetActive(false);
		}

		// Token: 0x0600B5CB RID: 46539 RVA: 0x003EECD8 File Offset: 0x003ECED8
		public FacadeToggle(string outfitID, GameObject gameObject, ClothingOutfitUtility.OutfitType outfitType)
		{
			this.id = outfitID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			UIMannequin reference = component.GetReference<UIMannequin>("Mannequin");
			reference.gameObject.SetActive(true);
			component.GetReference<Image>("FGImage").SetAlpha(0f);
			ToolTip component2 = this.gameObject.GetComponent<ToolTip>();
			component2.SetSimpleTooltip("");
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget outfit = ClothingOutfitTarget.FromTemplateId(outfitID);
				component.GetReference<UIMannequin>("Mannequin").SetOutfit(outfit);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(outfit.ReadName()));
			}
			else
			{
				component.GetReference<UIMannequin>("Mannequin").ClearOutfit(outfitType);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(UI.OUTFIT_NAME.NONE));
			}
			string dlcId = null;
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget.Implementation impl = ClothingOutfitTarget.FromTemplateId(outfitID).impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					dlcId = databaseAuthoredTemplate.resource.GetDlcIdFrom();
				}
			}
			Image reference2 = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference2.gameObject.SetActive(true);
				reference2.color = DlcManager.GetDlcBannerColor(dlcId);
			}
			else
			{
				reference2.gameObject.SetActive(false);
			}
			Vector2 sizeDelta = new Vector2(0f, 0f);
			if (outfitType == ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				sizeDelta = new Vector2(-16f, -16f);
			}
			else if (outfitType == ClothingOutfitUtility.OutfitType.JetSuit)
			{
				sizeDelta = new Vector2(-32f, -24f);
			}
			reference.rectTransform().sizeDelta = sizeDelta;
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x0600B5CC RID: 46540 RVA: 0x003EEE81 File Offset: 0x003ED081
		// (set) Token: 0x0600B5CD RID: 46541 RVA: 0x003EEE89 File Offset: 0x003ED089
		public string id { readonly get; set; }

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x0600B5CE RID: 46542 RVA: 0x003EEE92 File Offset: 0x003ED092
		// (set) Token: 0x0600B5CF RID: 46543 RVA: 0x003EEE9A File Offset: 0x003ED09A
		public GameObject gameObject { readonly get; set; }

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x0600B5D0 RID: 46544 RVA: 0x003EEEA3 File Offset: 0x003ED0A3
		// (set) Token: 0x0600B5D1 RID: 46545 RVA: 0x003EEEAB File Offset: 0x003ED0AB
		public MultiToggle multiToggle { readonly get; set; }
	}

	// Token: 0x02001F3C RID: 7996
	private enum ConfigType
	{
		// Token: 0x040091FD RID: 37373
		BuildingFacade,
		// Token: 0x040091FE RID: 37374
		MinionOutfit
	}
}
