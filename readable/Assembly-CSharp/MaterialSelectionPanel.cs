using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000D88 RID: 3464
public class MaterialSelectionPanel : KScreen, IRender200ms
{
	// Token: 0x06006BC7 RID: 27591 RVA: 0x0028EE5F File Offset: 0x0028D05F
	public static void ClearStatics()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06006BC8 RID: 27592 RVA: 0x0028EE6B File Offset: 0x0028D06B
	public Tag CurrentSelectedElement
	{
		get
		{
			if (this.materialSelectors.Count == 0)
			{
				return null;
			}
			return this.materialSelectors[0].CurrentSelectedElement;
		}
	}

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06006BC9 RID: 27593 RVA: 0x0028EE94 File Offset: 0x0028D094
	public IList<Tag> GetSelectedElementAsList
	{
		get
		{
			this.currentSelectedElements.Clear();
			foreach (MaterialSelector materialSelector in this.materialSelectors)
			{
				if (materialSelector.gameObject.activeSelf)
				{
					global::Debug.Assert(materialSelector.CurrentSelectedElement != null);
					this.currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
				}
			}
			return this.currentSelectedElements;
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06006BCA RID: 27594 RVA: 0x0028EF28 File Offset: 0x0028D128
	public PriorityScreen PriorityScreen
	{
		get
		{
			return this.priorityScreen;
		}
	}

	// Token: 0x06006BCB RID: 27595 RVA: 0x0028EF30 File Offset: 0x0028D130
	protected override void OnPrefabInit()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		for (int i = 0; i < 3; i++)
		{
			MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(this.MaterialSelectorTemplate, base.gameObject, false);
			materialSelector.selectorIndex = i;
			this.materialSelectors.Add(materialSelector);
		}
		this.materialSelectors[0].gameObject.SetActive(true);
		this.MaterialSelectorTemplate.SetActive(false);
		this.ToggleResearchRequired(false);
		if (this.priorityScreenParent != null)
		{
			this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
			this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
			this.priorityScreenParent.transform.SetAsLastSibling();
		}
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, delegate(object d)
		{
			this.RefreshSelectors();
		}));
	}

	// Token: 0x06006BCC RID: 27596 RVA: 0x0028F029 File Offset: 0x0028D229
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateOnSpawn = true;
	}

	// Token: 0x06006BCD RID: 27597 RVA: 0x0028F038 File Offset: 0x0028D238
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
	}

	// Token: 0x06006BCE RID: 27598 RVA: 0x0028F0A0 File Offset: 0x0028D2A0
	public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions)Delegate.Combine(selector.selectMaterialActions, action);
		});
	}

	// Token: 0x06006BCF RID: 27599 RVA: 0x0028F0D1 File Offset: 0x0028D2D1
	public void ClearSelectActions()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = null;
		});
	}

	// Token: 0x06006BD0 RID: 27600 RVA: 0x0028F0FD File Offset: 0x0028D2FD
	public void ClearMaterialToggles()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.ClearMaterialToggles();
		});
	}

	// Token: 0x06006BD1 RID: 27601 RVA: 0x0028F129 File Offset: 0x0028D329
	public void ConfigureScreen(Recipe recipe, MaterialSelectionPanel.GetBuildableStateDelegate buildableStateCB, MaterialSelectionPanel.GetBuildableTooltipDelegate buildableTooltipCB)
	{
		this.activeRecipe = recipe;
		this.GetBuildableState = buildableStateCB;
		this.GetBuildableTooltip = buildableTooltipCB;
		this.RefreshSelectors();
	}

	// Token: 0x06006BD2 RID: 27602 RVA: 0x0028F148 File Offset: 0x0028D348
	public bool AllSelectorsSelected()
	{
		bool flag = false;
		foreach (MaterialSelector materialSelector in this.materialSelectors)
		{
			flag = (flag || materialSelector.gameObject.activeInHierarchy);
			if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == null)
			{
				return false;
			}
		}
		return flag;
	}

	// Token: 0x06006BD3 RID: 27603 RVA: 0x0028F1D0 File Offset: 0x0028D3D0
	public void RefreshSelectors()
	{
		if (this.activeRecipe == null)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.gameObject.SetActive(false);
		});
		BuildingDef buildingDef = this.activeRecipe.GetBuildingDef();
		bool flag = this.GetBuildableState(buildingDef);
		string text = this.GetBuildableTooltip(buildingDef);
		if (!flag)
		{
			this.ToggleResearchRequired(true);
			LocText[] componentsInChildren = this.ResearchRequired.GetComponentsInChildren<LocText>();
			componentsInChildren[0].text = "";
			componentsInChildren[1].text = text;
			componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(false);
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.ToggleResearchRequired(false);
			for (int i = 0; i < this.activeRecipe.Ingredients.Count; i++)
			{
				this.materialSelectors[i].gameObject.SetActive(true);
				this.materialSelectors[i].ConfigureScreen(this.activeRecipe.Ingredients[i], this.activeRecipe);
			}
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.transform.SetAsLastSibling();
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(true);
				this.buildToolRotateButton.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x06006BD4 RID: 27604 RVA: 0x0028F37A File Offset: 0x0028D57A
	private void UpdateResourceToggleValues()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			if (selector.gameObject.activeSelf)
			{
				selector.RefreshToggleContents();
			}
		});
	}

	// Token: 0x06006BD5 RID: 27605 RVA: 0x0028F3B4 File Offset: 0x0028D5B4
	private void ToggleResearchRequired(bool state)
	{
		if (this.ResearchRequired == null)
		{
			return;
		}
		this.ResearchRequired.SetActive(state);
	}

	// Token: 0x06006BD6 RID: 27606 RVA: 0x0028F3D4 File Offset: 0x0028D5D4
	public bool AutoSelectAvailableMaterial()
	{
		bool result = true;
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (!this.materialSelectors[i].AutoSelectAvailableMaterial())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06006BD7 RID: 27607 RVA: 0x0028F410 File Offset: 0x0028D610
	public void SelectSourcesMaterials(Building building)
	{
		Tag[] array = null;
		Deconstructable component = building.gameObject.GetComponent<Deconstructable>();
		if (component != null)
		{
			array = component.constructionElements;
		}
		Constructable component2 = building.GetComponent<Constructable>();
		if (component2 != null)
		{
			array = component2.SelectedElementsTags.ToArray<Tag>();
		}
		if (array != null)
		{
			for (int i = 0; i < Mathf.Min(array.Length, this.materialSelectors.Count); i++)
			{
				if (this.materialSelectors[i].ElementToggles.ContainsKey(array[i]))
				{
					this.materialSelectors[i].OnSelectMaterial(array[i], this.activeRecipe, false);
				}
			}
		}
	}

	// Token: 0x06006BD8 RID: 27608 RVA: 0x0028F4B6 File Offset: 0x0028D6B6
	public void ForceSelectPrimaryTag(Tag tag)
	{
		this.materialSelectors[0].OnSelectMaterial(tag, this.activeRecipe, false);
	}

	// Token: 0x06006BD9 RID: 27609 RVA: 0x0028F4D4 File Offset: 0x0028D6D4
	public static MaterialSelectionPanel.SelectedElemInfo Filter(Tag _materialCategoryTag)
	{
		MaterialSelectionPanel.SelectedElemInfo selectedElemInfo = default(MaterialSelectionPanel.SelectedElemInfo);
		selectedElemInfo.element = null;
		selectedElemInfo.kgAvailable = 0f;
		if (DiscoveredResources.Instance == null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
		{
			return selectedElemInfo;
		}
		string[] array = _materialCategoryTag.ToString().Split('&', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			Tag tag = array[i];
			List<Tag> list = null;
			if (!MaterialSelectionPanel.elementsWithTag.TryGetValue(tag, out list))
			{
				list = new List<Tag>();
				foreach (Element element in ElementLoader.elements)
				{
					if (element.tag == tag || element.HasTag(tag))
					{
						list.Add(element.tag);
					}
				}
				foreach (Tag tag2 in GameTags.MaterialBuildingElements)
				{
					if (tag2 == tag)
					{
						foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag2))
						{
							KPrefabID component = gameObject.GetComponent<KPrefabID>();
							if (component != null && !list.Contains(component.PrefabTag))
							{
								list.Add(component.PrefabTag);
							}
						}
					}
				}
				MaterialSelectionPanel.elementsWithTag[tag] = list;
			}
			foreach (Tag tag3 in list)
			{
				float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag3, true);
				if (amount > selectedElemInfo.kgAvailable)
				{
					selectedElemInfo.kgAvailable = amount;
					selectedElemInfo.element = tag3;
				}
			}
		}
		return selectedElemInfo;
	}

	// Token: 0x06006BDA RID: 27610 RVA: 0x0028F708 File Offset: 0x0028D908
	public void ToggleShowDescriptorPanels(bool show)
	{
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (this.materialSelectors[i] != null)
			{
				this.materialSelectors[i].ToggleShowDescriptorsPanel(show);
			}
		}
	}

	// Token: 0x06006BDB RID: 27611 RVA: 0x0028F751 File Offset: 0x0028D951
	private void OnPriorityClicked(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x06006BDC RID: 27612 RVA: 0x0028F760 File Offset: 0x0028D960
	public void Render200ms(float dt)
	{
		this.UpdateResourceToggleValues();
	}

	// Token: 0x040049F7 RID: 18935
	public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();

	// Token: 0x040049F8 RID: 18936
	private List<MaterialSelector> materialSelectors = new List<MaterialSelector>();

	// Token: 0x040049F9 RID: 18937
	private List<Tag> currentSelectedElements = new List<Tag>();

	// Token: 0x040049FA RID: 18938
	[SerializeField]
	protected PriorityScreen priorityScreenPrefab;

	// Token: 0x040049FB RID: 18939
	[SerializeField]
	protected GameObject priorityScreenParent;

	// Token: 0x040049FC RID: 18940
	[SerializeField]
	protected BuildToolRotateButtonUI buildToolRotateButton;

	// Token: 0x040049FD RID: 18941
	private PriorityScreen priorityScreen;

	// Token: 0x040049FE RID: 18942
	public GameObject MaterialSelectorTemplate;

	// Token: 0x040049FF RID: 18943
	public GameObject ResearchRequired;

	// Token: 0x04004A00 RID: 18944
	private Recipe activeRecipe;

	// Token: 0x04004A01 RID: 18945
	private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();

	// Token: 0x04004A02 RID: 18946
	private MaterialSelectionPanel.GetBuildableStateDelegate GetBuildableState;

	// Token: 0x04004A03 RID: 18947
	private MaterialSelectionPanel.GetBuildableTooltipDelegate GetBuildableTooltip;

	// Token: 0x04004A04 RID: 18948
	private List<int> gameSubscriptionHandles = new List<int>();

	// Token: 0x02001FD9 RID: 8153
	// (Invoke) Token: 0x0600B77F RID: 46975
	public delegate bool GetBuildableStateDelegate(BuildingDef def);

	// Token: 0x02001FDA RID: 8154
	// (Invoke) Token: 0x0600B783 RID: 46979
	public delegate string GetBuildableTooltipDelegate(BuildingDef def);

	// Token: 0x02001FDB RID: 8155
	// (Invoke) Token: 0x0600B787 RID: 46983
	public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

	// Token: 0x02001FDC RID: 8156
	public struct SelectedElemInfo
	{
		// Token: 0x0400940A RID: 37898
		public Tag element;

		// Token: 0x0400940B RID: 37899
		public float kgAvailable;
	}
}
