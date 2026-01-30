using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DE4 RID: 3556
public class ProductInfoScreen : KScreen
{
	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x06006FCD RID: 28621 RVA: 0x002A75B8 File Offset: 0x002A57B8
	public FacadeSelectionPanel FacadeSelectionPanel
	{
		get
		{
			return this.facadeSelectionPanel;
		}
	}

	// Token: 0x06006FCE RID: 28622 RVA: 0x002A75C0 File Offset: 0x002A57C0
	private void RefreshScreen()
	{
		if (this.currentDef != null)
		{
			this.SetTitle(this.currentDef);
			return;
		}
		this.ClearProduct(true);
	}

	// Token: 0x06006FCF RID: 28623 RVA: 0x002A75E4 File Offset: 0x002A57E4
	public void ClearProduct(bool deactivateTool = true)
	{
		if (this.materialSelectionPanel == null)
		{
			return;
		}
		this.currentDef = null;
		this.materialSelectionPanel.ClearMaterialToggles();
		if (PlayerController.Instance.ActiveTool == BuildTool.Instance && deactivateTool)
		{
			BuildTool.Instance.Deactivate();
		}
		if (PlayerController.Instance.ActiveTool == UtilityBuildTool.Instance || PlayerController.Instance.ActiveTool == WireBuildTool.Instance)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.ClearLabels();
		this.Show(false);
	}

	// Token: 0x06006FD0 RID: 28624 RVA: 0x002A7678 File Offset: 0x002A5878
	public new void Awake()
	{
		base.Awake();
		this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab.gameObject, base.gameObject, false);
		FacadeSelectionPanel facadeSelectionPanel = this.facadeSelectionPanel;
		facadeSelectionPanel.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel.OnFacadeSelectionChanged, new System.Action(this.OnFacadeSelectionChanged));
		this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x06006FD1 RID: 28625 RVA: 0x002A76EC File Offset: 0x002A58EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen instance = BuildingGroupScreen.Instance;
			instance.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildingGroupScreen instance2 = BuildingGroupScreen.Instance;
			instance2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance2.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (PlanScreen.Instance != null)
		{
			PlanScreen instance3 = PlanScreen.Instance;
			instance3.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance3.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			PlanScreen instance4 = PlanScreen.Instance;
			instance4.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance4.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu instance5 = BuildMenu.Instance;
			instance5.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance5.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildMenu instance6 = BuildMenu.Instance;
			instance6.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance6.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		base.ConsumeMouseScroll = true;
		this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		MultiToggle multiToggle = this.sandboxInstantBuildToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SandboxToolParameterMenu.instance.settings.InstantBuild = !SandboxToolParameterMenu.instance.settings.InstantBuild;
			this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		}));
		this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		});
	}

	// Token: 0x06006FD2 RID: 28626 RVA: 0x002A78D2 File Offset: 0x002A5AD2
	public void ConfigureScreen(BuildingDef def)
	{
		this.ConfigureScreen(def, this.FacadeSelectionPanel.SelectedFacade);
	}

	// Token: 0x06006FD3 RID: 28627 RVA: 0x002A78E8 File Offset: 0x002A5AE8
	public void ConfigureScreen(BuildingDef def, string facadeID)
	{
		this.configuring = true;
		this.currentDef = def;
		this.SetTitle(def);
		this.SetDescription(def);
		this.SetEffects(def);
		this.facadeSelectionPanel.SetBuildingDef(def.PrefabID, null);
		BuildingFacadeResource buildingFacadeResource = null;
		if ("DEFAULT_FACADE" != facadeID)
		{
			buildingFacadeResource = Db.GetBuildingFacades().TryGet(facadeID);
		}
		if (buildingFacadeResource != null && buildingFacadeResource.PrefabID == def.PrefabID && buildingFacadeResource.IsUnlocked())
		{
			this.facadeSelectionPanel.SelectedFacade = facadeID;
		}
		else
		{
			this.facadeSelectionPanel.SelectedFacade = "DEFAULT_FACADE";
		}
		this.SetMaterials(def);
		this.configuring = false;
	}

	// Token: 0x06006FD4 RID: 28628 RVA: 0x002A798F File Offset: 0x002A5B8F
	private void ExpandInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(true);
	}

	// Token: 0x06006FD5 RID: 28629 RVA: 0x002A7998 File Offset: 0x002A5B98
	private void CollapseInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(false);
	}

	// Token: 0x06006FD6 RID: 28630 RVA: 0x002A79A4 File Offset: 0x002A5BA4
	public void ToggleExpandedInfo(bool state)
	{
		this.expandedInfo = state;
		if (this.ProductDescriptionPane != null)
		{
			this.ProductDescriptionPane.SetActive(this.expandedInfo);
		}
		if (this.ProductRequirementsPane != null)
		{
			this.ProductRequirementsPane.gameObject.SetActive(this.expandedInfo && this.ProductRequirementsPane.HasDescriptors());
		}
		if (this.RoomConstrainsPanel != null)
		{
			this.RoomConstrainsPanel.gameObject.SetActive(this.expandedInfo && this.RoomConstrainsPanel.HasDescriptors());
		}
		if (this.ProductEffectsPane != null)
		{
			this.ProductEffectsPane.gameObject.SetActive(this.expandedInfo && this.ProductEffectsPane.HasDescriptors());
		}
		if (this.ProductFlavourPane != null)
		{
			this.ProductFlavourPane.SetActive(this.expandedInfo);
		}
		if (this.materialSelectionPanel != null && this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			this.materialSelectionPanel.ToggleShowDescriptorPanels(this.expandedInfo);
		}
	}

	// Token: 0x06006FD7 RID: 28631 RVA: 0x002A7ACC File Offset: 0x002A5CCC
	private void CheckMouseOver(PointerEventData data)
	{
		bool state = base.GetMouseOver || (PlanScreen.Instance != null && ((PlanScreen.Instance.IsScreenActive() && PlanScreen.Instance.GetMouseOver) || BuildingGroupScreen.Instance.GetMouseOver)) || (BuildMenu.Instance != null && BuildMenu.Instance.IsScreenActive() && BuildMenu.Instance.GetMouseOver);
		this.ToggleExpandedInfo(state);
	}

	// Token: 0x06006FD8 RID: 28632 RVA: 0x002A7B44 File Offset: 0x002A5D44
	private void Update()
	{
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && this.currentDef != null && this.materialSelectionPanel.CurrentSelectedElement != null && !MaterialSelector.AllowInsufficientMaterialBuild() && this.currentDef.Mass[0] > ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.materialSelectionPanel.CurrentSelectedElement, true))
		{
			this.materialSelectionPanel.AutoSelectAvailableMaterial();
		}
	}

	// Token: 0x06006FD9 RID: 28633 RVA: 0x002A7BCC File Offset: 0x002A5DCC
	private void SetTitle(BuildingDef def)
	{
		this.titleBar.SetTitle(def.Name);
		bool flag = (PlanScreen.Instance != null && PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.IsDefBuildable(def)) || (BuildMenu.Instance != null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete);
		this.titleBar.GetComponentInChildren<KImage>().ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Disabled);
	}

	// Token: 0x06006FDA RID: 28634 RVA: 0x002A7C58 File Offset: 0x002A5E58
	private void SetDescription(BuildingDef def)
	{
		if (def == null)
		{
			return;
		}
		if (this.productFlavourText == null)
		{
			return;
		}
		string text = "";
		text += def.Desc;
		Dictionary<Klei.AI.Attribute, float> dictionary = new Dictionary<Klei.AI.Attribute, float>();
		Dictionary<Klei.AI.Attribute, float> dictionary2 = new Dictionary<Klei.AI.Attribute, float>();
		foreach (Klei.AI.Attribute key in def.attributes)
		{
			if (!dictionary.ContainsKey(key))
			{
				dictionary[key] = 0f;
			}
		}
		foreach (AttributeModifier attributeModifier in def.attributeModifiers)
		{
			float num = 0f;
			Klei.AI.Attribute key2 = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
			dictionary.TryGetValue(key2, out num);
			num += attributeModifier.Value;
			dictionary[key2] = num;
		}
		if (this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			Element element = ElementLoader.GetElement(this.materialSelectionPanel.CurrentSelectedElement);
			if (element != null)
			{
				using (List<AttributeModifier>.Enumerator enumerator2 = element.attributeModifiers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AttributeModifier attributeModifier2 = enumerator2.Current;
						float num2 = 0f;
						Klei.AI.Attribute key3 = Db.Get().BuildingAttributes.Get(attributeModifier2.AttributeId);
						dictionary2.TryGetValue(key3, out num2);
						num2 += attributeModifier2.Value;
						dictionary2[key3] = num2;
					}
					goto IL_229;
				}
			}
			PrefabAttributeModifiers component = Assets.TryGetPrefab(this.materialSelectionPanel.CurrentSelectedElement).GetComponent<PrefabAttributeModifiers>();
			if (component != null)
			{
				foreach (AttributeModifier attributeModifier3 in component.descriptors)
				{
					float num3 = 0f;
					Klei.AI.Attribute key4 = Db.Get().BuildingAttributes.Get(attributeModifier3.AttributeId);
					dictionary2.TryGetValue(key4, out num3);
					num3 += attributeModifier3.Value;
					dictionary2[key4] = num3;
				}
			}
		}
		IL_229:
		if (dictionary.Count > 0)
		{
			text += "\n\n";
			foreach (KeyValuePair<Klei.AI.Attribute, float> keyValuePair in dictionary)
			{
				float num4 = 0f;
				dictionary.TryGetValue(keyValuePair.Key, out num4);
				float num5 = 0f;
				string text2 = "";
				if (dictionary2.TryGetValue(keyValuePair.Key, out num5))
				{
					num5 = Mathf.Abs(num4 * num5);
					text2 = "(+" + num5.ToString() + ")";
				}
				text = string.Concat(new string[]
				{
					text,
					"\n",
					keyValuePair.Key.Name,
					": ",
					(num4 + num5).ToString(),
					text2
				});
			}
		}
		this.productFlavourText.text = text;
	}

	// Token: 0x06006FDB RID: 28635 RVA: 0x002A7FC4 File Offset: 0x002A61C4
	private void SetEffects(BuildingDef def)
	{
		if (this.productDescriptionText.text != null)
		{
			this.productDescriptionText.text = string.Format("{0}", def.Effect);
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def.BuildingComplete, false);
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
		List<Descriptor> list = new List<Descriptor>();
		if (requirementDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONREQUIREMENTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONREQUIREMENTS, Descriptor.DescriptorType.Effect);
			requirementDescriptors.Insert(0, item);
			this.ProductRequirementsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductRequirementsPane.gameObject.SetActive(false);
		}
		this.ProductRequirementsPane.SetDescriptors(requirementDescriptors);
		List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
		if (effectDescriptors.Count > 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONEFFECTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONEFFECTS, Descriptor.DescriptorType.Effect);
			effectDescriptors.Insert(0, item2);
			this.ProductEffectsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductEffectsPane.gameObject.SetActive(false);
		}
		this.ProductEffectsPane.SetDescriptors(effectDescriptors);
		foreach (Tag tag in def.BuildingComplete.GetComponent<KPrefabID>().Tags)
		{
			if (RoomConstraints.ConstraintTags.AllTags.Contains(tag) && !this.HiddenRoomConstrainTags.Contains(tag))
			{
				Descriptor item3 = default(Descriptor);
				item3.SetupDescriptor(RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(tag), null, Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		if (list.Count > 0)
		{
			list = GameUtil.GetEffectDescriptors(list);
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(CODEX.HEADERS.BUILDINGTYPE, UI.BUILDINGEFFECTS.TOOLTIPS.BUILDINGROOMREQUIREMENTCLASS, Descriptor.DescriptorType.Effect);
			list.Insert(0, item4);
			this.RoomConstrainsPanel.gameObject.SetActive(true);
		}
		else
		{
			this.RoomConstrainsPanel.gameObject.SetActive(false);
		}
		this.RoomConstrainsPanel.SetDescriptors(list);
	}

	// Token: 0x06006FDC RID: 28636 RVA: 0x002A81D8 File Offset: 0x002A63D8
	public void ClearLabels()
	{
		List<string> list = new List<string>(this.descLabels.Keys);
		if (list.Count > 0)
		{
			foreach (string key in list)
			{
				GameObject gameObject = this.descLabels[key];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				this.descLabels.Remove(key);
			}
		}
	}

	// Token: 0x06006FDD RID: 28637 RVA: 0x002A8264 File Offset: 0x002A6464
	public void SetMaterials(BuildingDef def)
	{
		this.materialSelectionPanel.gameObject.SetActive(true);
		Recipe craftRecipe = def.CraftRecipe;
		this.materialSelectionPanel.ClearSelectActions();
		this.materialSelectionPanel.ConfigureScreen(craftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(PlanScreen.Instance.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(PlanScreen.Instance.GetTooltipForBuildable));
		this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshScreen));
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.onMenuMaterialChanged));
		this.materialSelectionPanel.AutoSelectAvailableMaterial();
		this.ActivateAppropriateTool(def);
	}

	// Token: 0x06006FDE RID: 28638 RVA: 0x002A830D File Offset: 0x002A650D
	private void OnFacadeSelectionChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
	}

	// Token: 0x06006FDF RID: 28639 RVA: 0x002A832A File Offset: 0x002A652A
	private void onMenuMaterialChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
		this.SetDescription(this.currentDef);
	}

	// Token: 0x06006FE0 RID: 28640 RVA: 0x002A8354 File Offset: 0x002A6554
	private void ActivateAppropriateTool(BuildingDef def)
	{
		global::Debug.Assert(def != null, "def was null");
		if (((PlanScreen.Instance != null) ? PlanScreen.Instance.IsDefBuildable(def) : (BuildMenu.Instance != null && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete)) && this.materialSelectionPanel.AllSelectorsSelected() && this.facadeSelectionPanel.SelectedFacade != null)
		{
			this.onElementsFullySelected.Signal();
			return;
		}
		if (!MaterialSelector.AllowInsufficientMaterialBuild() && !DebugHandler.InstantBuildMode)
		{
			if (PlayerController.Instance.ActiveTool == BuildTool.Instance)
			{
				BuildTool.Instance.Deactivate();
			}
			PrebuildTool.Instance.Activate(def, PlanScreen.Instance.GetTooltipForBuildable(def));
		}
	}

	// Token: 0x06006FE1 RID: 28641 RVA: 0x002A8418 File Offset: 0x002A6618
	public static bool MaterialsMet(Recipe recipe)
	{
		if (recipe == null)
		{
			global::Debug.LogError("Trying to verify the materials on a null recipe!");
			return false;
		}
		if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
		{
			global::Debug.LogError("Trying to verify the materials on a recipe with no MaterialCategoryTags!");
			return false;
		}
		bool result = true;
		for (int i = 0; i < recipe.Ingredients.Count; i++)
		{
			if (MaterialSelectionPanel.Filter(recipe.Ingredients[i].tag).kgAvailable < recipe.Ingredients[i].amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006FE2 RID: 28642 RVA: 0x002A84A0 File Offset: 0x002A66A0
	public void Close()
	{
		if (this.configuring)
		{
			return;
		}
		this.ClearProduct(true);
		this.Show(false);
	}

	// Token: 0x04004CAC RID: 19628
	public TitleBar titleBar;

	// Token: 0x04004CAD RID: 19629
	public GameObject ProductDescriptionPane;

	// Token: 0x04004CAE RID: 19630
	public LocText productDescriptionText;

	// Token: 0x04004CAF RID: 19631
	public DescriptorPanel ProductRequirementsPane;

	// Token: 0x04004CB0 RID: 19632
	public DescriptorPanel ProductEffectsPane;

	// Token: 0x04004CB1 RID: 19633
	public DescriptorPanel RoomConstrainsPanel;

	// Token: 0x04004CB2 RID: 19634
	public GameObject ProductFlavourPane;

	// Token: 0x04004CB3 RID: 19635
	public LocText productFlavourText;

	// Token: 0x04004CB4 RID: 19636
	public RectTransform BGPanel;

	// Token: 0x04004CB5 RID: 19637
	public MaterialSelectionPanel materialSelectionPanelPrefab;

	// Token: 0x04004CB6 RID: 19638
	public FacadeSelectionPanel facadeSelectionPanelPrefab;

	// Token: 0x04004CB7 RID: 19639
	private Dictionary<string, GameObject> descLabels = new Dictionary<string, GameObject>();

	// Token: 0x04004CB8 RID: 19640
	public MultiToggle sandboxInstantBuildToggle;

	// Token: 0x04004CB9 RID: 19641
	private List<Tag> HiddenRoomConstrainTags = new List<Tag>
	{
		RoomConstraints.ConstraintTags.Refrigerator,
		RoomConstraints.ConstraintTags.FarmStationType,
		RoomConstraints.ConstraintTags.LuxuryBedType,
		RoomConstraints.ConstraintTags.MassageTable,
		RoomConstraints.ConstraintTags.MessTable,
		RoomConstraints.ConstraintTags.NatureReserve,
		RoomConstraints.ConstraintTags.Park,
		RoomConstraints.ConstraintTags.SpiceStation,
		RoomConstraints.ConstraintTags.DeStressingBuilding,
		RoomConstraints.ConstraintTags.MachineShopType
	};

	// Token: 0x04004CBA RID: 19642
	[NonSerialized]
	public MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x04004CBB RID: 19643
	[SerializeField]
	private FacadeSelectionPanel facadeSelectionPanel;

	// Token: 0x04004CBC RID: 19644
	[NonSerialized]
	public BuildingDef currentDef;

	// Token: 0x04004CBD RID: 19645
	public System.Action onElementsFullySelected;

	// Token: 0x04004CBE RID: 19646
	private bool expandedInfo = true;

	// Token: 0x04004CBF RID: 19647
	private bool configuring;
}
