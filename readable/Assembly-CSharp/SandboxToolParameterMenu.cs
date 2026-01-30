using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DFA RID: 3578
public class SandboxToolParameterMenu : KScreen
{
	// Token: 0x06007136 RID: 28982 RVA: 0x002B2F61 File Offset: 0x002B1161
	public static void DestroyInstance()
	{
		SandboxToolParameterMenu.instance = null;
	}

	// Token: 0x06007137 RID: 28983 RVA: 0x002B2F69 File Offset: 0x002B1169
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06007138 RID: 28984 RVA: 0x002B2F70 File Offset: 0x002B1170
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ConfigureSettings();
		this.activateOnSpawn = true;
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06007139 RID: 28985 RVA: 0x002B2F8C File Offset: 0x002B118C
	private void ConfigureSettings()
	{
		this.massSlider.clampValueLow = 0.001f;
		this.massSlider.clampValueHigh = 10000f;
		this.temperatureAdditiveSlider.clampValueLow = -9999f;
		this.temperatureAdditiveSlider.clampValueHigh = 9999f;
		this.temperatureSlider.clampValueLow = -458f;
		this.temperatureSlider.clampValueHigh = 9999f;
		this.brushRadiusSlider.clampValueLow = 1f;
		this.brushRadiusSlider.clampValueHigh = 50f;
		this.diseaseCountSlider.clampValueHigh = 1000000f;
		this.diseaseCountSlider.slideMaxValue = 1000000f;
		this.settings = new SandboxSettings();
		SandboxSettings sandboxSettings = this.settings;
		sandboxSettings.OnChangeElement = (Action<bool>)Delegate.Combine(sandboxSettings.OnChangeElement, new Action<bool>(delegate(bool forceElementDefaults)
		{
			int num = this.settings.GetIntSetting("SandboxTools.SelectedElement");
			if (num >= ElementLoader.elements.Count)
			{
				num = 0;
			}
			Element element = ElementLoader.elements[num];
			this.elementSelector.button.GetComponentInChildren<LocText>().text = element.name + " (" + element.GetStateString() + ")";
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
			this.elementSelector.button.GetComponentsInChildren<Image>()[1].sprite = uisprite.first;
			this.elementSelector.button.GetComponentsInChildren<Image>()[1].color = uisprite.second;
			this.SetAbsoluteTemperatureSliderRange(element);
			this.massSlider.SetRange(0.1f, Mathf.Min(element.maxMass * 2f, this.massSlider.clampValueHigh), false);
			if (forceElementDefaults)
			{
				this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(element.defaultValues.temperature, true), true);
				this.massSlider.SetValue(element.defaultValues.mass, true);
			}
		}));
		SandboxSettings sandboxSettings2 = this.settings;
		sandboxSettings2.OnChangeMass = (System.Action)Delegate.Combine(sandboxSettings2.OnChangeMass, new System.Action(delegate()
		{
			this.massSlider.SetValue(this.settings.GetFloatSetting("SandboxTools.Mass"), false);
		}));
		SandboxSettings sandboxSettings3 = this.settings;
		sandboxSettings3.OnChangeDisease = (System.Action)Delegate.Combine(sandboxSettings3.OnChangeDisease, new System.Action(delegate()
		{
			Disease disease = Db.Get().Diseases.TryGet(SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedDisease"));
			if (disease == null)
			{
				disease = Db.Get().Diseases.Get("FoodPoisoning");
			}
			this.diseaseSelector.button.GetComponentInChildren<LocText>().text = disease.Name;
			this.diseaseSelector.button.GetComponentsInChildren<Image>()[1].sprite = Assets.GetSprite("germ");
			this.diseaseCountSlider.SetRange(0f, 1000000f, false);
		}));
		SandboxSettings sandboxSettings4 = this.settings;
		sandboxSettings4.OnChangeDiseaseCount = (System.Action)Delegate.Combine(sandboxSettings4.OnChangeDiseaseCount, new System.Action(delegate()
		{
			this.diseaseCountSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.DiseaseCount"), false);
		}));
		SandboxSettings sandboxSettings5 = this.settings;
		sandboxSettings5.OnChangeStory = (System.Action)Delegate.Combine(sandboxSettings5.OnChangeStory, new System.Action(delegate()
		{
			string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedStory");
			Story story = Db.Get().Stories.TryGet(stringSetting);
			if (story == null)
			{
				this.settings.ForceDefaultStringSetting("SandboxTools.SelectedStory");
				return;
			}
			this.storySelector.button.GetComponentInChildren<LocText>().text = Strings.Get(story.StoryTrait.name);
			this.storySelector.button.GetComponentsInChildren<Image>()[1].sprite = Assets.GetSprite(story.StoryTrait.icon);
		}));
		SandboxSettings sandboxSettings6 = this.settings;
		sandboxSettings6.OnChangeEntity = (System.Action)Delegate.Combine(sandboxSettings6.OnChangeEntity, new System.Action(delegate()
		{
			string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
			GameObject gameObject = Assets.TryGetPrefab(stringSetting);
			if (gameObject == null || !Game.IsCorrectDlcActiveForCurrentSave(gameObject.GetComponent<KPrefabID>()) || gameObject.HasTag(GameTags.HideFromSpawnTool))
			{
				this.settings.ForceDefaultStringSetting("SandboxTools.SelectedEntity");
				return;
			}
			this.entitySelector.button.GetComponentInChildren<LocText>().text = gameObject.GetProperName();
			global::Tuple<Sprite, Color> tuple;
			if (gameObject.HasTag(GameTags.BaseMinion))
			{
				tuple = new global::Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel(gameObject.PrefabID()), Color.white);
			}
			else
			{
				tuple = Def.GetUISprite(stringSetting, "ui", false);
			}
			if (tuple != null)
			{
				this.entitySelector.button.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
				this.entitySelector.button.GetComponentsInChildren<Image>()[1].color = tuple.second;
			}
		}));
		SandboxSettings sandboxSettings7 = this.settings;
		sandboxSettings7.OnChangeBrushSize = (System.Action)Delegate.Combine(sandboxSettings7.OnChangeBrushSize, new System.Action(delegate()
		{
			if (PlayerController.Instance.ActiveTool is BrushTool)
			{
				(PlayerController.Instance.ActiveTool as BrushTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
			}
		}));
		SandboxSettings sandboxSettings8 = this.settings;
		sandboxSettings8.OnChangeNoiseScale = (System.Action)Delegate.Combine(sandboxSettings8.OnChangeNoiseScale, new System.Action(delegate()
		{
			if (PlayerController.Instance.ActiveTool is SandboxSprinkleTool)
			{
				(PlayerController.Instance.ActiveTool as SandboxSprinkleTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
			}
		}));
		SandboxSettings sandboxSettings9 = this.settings;
		sandboxSettings9.OnChangeNoiseDensity = (System.Action)Delegate.Combine(sandboxSettings9.OnChangeNoiseDensity, new System.Action(delegate()
		{
			if (PlayerController.Instance.ActiveTool is SandboxSprinkleTool)
			{
				(PlayerController.Instance.ActiveTool as SandboxSprinkleTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
			}
		}));
		SandboxSettings sandboxSettings10 = this.settings;
		sandboxSettings10.OnChangeTemperature = (System.Action)Delegate.Combine(sandboxSettings10.OnChangeTemperature, new System.Action(delegate()
		{
			this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.Temperature"), false), false);
		}));
		SandboxSettings sandboxSettings11 = this.settings;
		sandboxSettings11.OnChangeAdditiveTemperature = (System.Action)Delegate.Combine(sandboxSettings11.OnChangeAdditiveTemperature, new System.Action(delegate()
		{
			this.temperatureAdditiveSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.TemperatureAdditive"), true), false);
		}));
		Game.Instance.Subscribe(999382396, new Action<object>(this.OnTemperatureUnitChanged));
		SandboxSettings sandboxSettings12 = this.settings;
		sandboxSettings12.OnChangeAdditiveStress = (System.Action)Delegate.Combine(sandboxSettings12.OnChangeAdditiveStress, new System.Action(delegate()
		{
			this.stressAdditiveSlider.SetValue(this.settings.GetFloatSetting("SandbosTools.StressAdditive"), false);
		}));
		SandboxSettings sandboxSettings13 = this.settings;
		sandboxSettings13.OnChangeMoraleAdjustment = (System.Action)Delegate.Combine(sandboxSettings13.OnChangeMoraleAdjustment, new System.Action(delegate()
		{
			this.moraleSlider.SetValue((float)this.settings.GetIntSetting("SandbosTools.MoraleAdjustment"), false);
		}));
	}

	// Token: 0x0600713A RID: 28986 RVA: 0x002B325C File Offset: 0x002B145C
	public void DisableParameters()
	{
		this.elementSelector.row.SetActive(false);
		this.entitySelector.row.SetActive(false);
		this.brushRadiusSlider.row.SetActive(false);
		this.noiseScaleSlider.row.SetActive(false);
		this.noiseDensitySlider.row.SetActive(false);
		this.massSlider.row.SetActive(false);
		this.temperatureAdditiveSlider.row.SetActive(false);
		this.temperatureSlider.row.SetActive(false);
		this.diseaseCountSlider.row.SetActive(false);
		this.diseaseSelector.row.SetActive(false);
		this.stressAdditiveSlider.row.SetActive(false);
		this.moraleSlider.row.SetActive(false);
		this.storySelector.row.SetActive(false);
	}

	// Token: 0x0600713B RID: 28987 RVA: 0x002B3348 File Offset: 0x002B1548
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ConfigureElementSelector();
		this.ConfigureDiseaseSelector();
		this.ConfigureEntitySelector();
		this.ConfigureStoryTraitSelector();
		this.SpawnSelector(this.entitySelector);
		this.SpawnSelector(this.elementSelector);
		this.SpawnSelector(this.storySelector);
		this.SpawnSlider(this.brushRadiusSlider);
		this.SpawnSlider(this.noiseScaleSlider);
		this.SpawnSlider(this.noiseDensitySlider);
		this.SpawnSlider(this.massSlider);
		this.SpawnSlider(this.temperatureSlider);
		this.SpawnSlider(this.temperatureAdditiveSlider);
		this.SpawnSlider(this.stressAdditiveSlider);
		this.SpawnSelector(this.diseaseSelector);
		this.SpawnSlider(this.diseaseCountSlider);
		this.SpawnSlider(this.moraleSlider);
		if (SandboxToolParameterMenu.instance == null)
		{
			SandboxToolParameterMenu.instance = this;
			base.gameObject.SetActive(false);
			this.settings.RestorePrefs();
		}
	}

	// Token: 0x0600713C RID: 28988 RVA: 0x002B3448 File Offset: 0x002B1648
	private void ConfigureElementSelector()
	{
		Func<object, bool> condition = (object element) => (element as Element).IsSolid;
		Func<object, bool> condition2 = (object element) => (element as Element).IsLiquid;
		Func<object, bool> condition3 = (object element) => (element as Element).IsGas;
		List<Element> commonElements = new List<Element>();
		Func<object, bool> condition4 = (object element) => commonElements.Contains(element as Element);
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Oxygen));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Water));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Vacuum));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Dirt));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SandStone));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Cuprite));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Steel));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Algae));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.CrudeOil));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.CarbonDioxide));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Sand));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SlimeMold));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Granite));
		List<Element> list = new List<Element>();
		foreach (Element element2 in ElementLoader.elements)
		{
			if (!element2.disabled)
			{
				bool flag = false;
				Tag[] oreTags = element2.oreTags;
				for (int i = 0; i < oreTags.Length; i++)
				{
					if (oreTags[i] == GameTags.HideFromSpawnTool)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(element2);
				}
			}
		}
		list.Sort((Element a, Element b) => a.name.CompareTo(b.name));
		object[] options = list.ToArray();
		this.elementSelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object element)
		{
			this.settings.SetIntSetting("SandboxTools.SelectedElement", (int)((Element)element).idx);
		}, (object element) => (element as Element).name + " (" + (element as Element).GetStateString() + ")", (string filterString, object option) => ((option as Element).name.ToUpper() + (option as Element).GetStateString().ToUpper()).Contains(filterString.ToUpper()), (object element) => Def.GetUISprite(element as Element, "ui", false), UI.SANDBOXTOOLS.SETTINGS.ELEMENT.NAME, new SandboxToolParameterMenu.SelectorValue.SearchFilter[]
		{
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.COMMON, condition4, null, null),
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.SOLID, condition, null, Def.GetUISprite(ElementLoader.FindElementByHash(SimHashes.SandStone), "ui", false)),
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.LIQUID, condition2, null, Def.GetUISprite(ElementLoader.FindElementByHash(SimHashes.Water), "ui", false)),
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.GAS, condition3, null, Def.GetUISprite(ElementLoader.FindElementByHash(SimHashes.Oxygen), "ui", false))
		});
	}

	// Token: 0x0600713D RID: 28989 RVA: 0x002B37D8 File Offset: 0x002B19D8
	private void ConfigureEntitySelector()
	{
		List<SandboxToolParameterMenu.SelectorValue.SearchFilter> list = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();
		SandboxToolParameterMenu.SelectorValue.SearchFilter item = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.FOOD, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			string idString = kprefabID2.PrefabID().ToString();
			return (!kprefabID2.HasTag(GameTags.Egg) && EdiblesManager.GetAllFoodTypes().Find((EdiblesManager.FoodInfo match) => match.Id == idString) != null) || kprefabID2.HasTag(GameTags.Dehydrated);
		}, null, Def.GetUISprite(Assets.GetPrefab("MushBar"), "ui", false));
		list.Add(item);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.COMETS, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			return kprefabID2.HasTag(GameTags.Comet) && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2);
		}, null, Def.GetUISprite(Assets.GetPrefab(CopperCometConfig.ID), "ui", false));
		list.Add(item2);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item3 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL, delegate(object entity)
		{
			KPrefabID restrictions = entity as KPrefabID;
			return (entity as KPrefabID).HasTag(GameTags.BaseMinion) && Game.IsCorrectDlcActiveForCurrentSave(restrictions);
		}, null, new global::Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel(GameTags.Minions.Models.Standard), Color.white));
		list.Add(item3);
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			SandboxToolParameterMenu.SelectorValue.SearchFilter item4 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.BIONICUPGRADES, delegate(object entity)
			{
				KPrefabID kprefabID2 = entity as KPrefabID;
				return kprefabID2.HasTag(GameTags.BionicUpgrade) && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2);
			}, null, new global::Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("upgrade_disc_kanim"), "ui", false, ""), Color.white));
			list.Add(item4);
		}
		SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter = null;
		searchFilter = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE, (object entity) => false, null, Def.GetUISprite(Assets.GetPrefab("Hatch"), "ui", false));
		list.Add(searchFilter);
		List<Tag> list2 = new List<Tag>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.CreatureBrain))
		{
			CreatureBrain brain = gameObject.GetComponent<CreatureBrain>();
			if (!list2.Contains(brain.species) && Game.IsCorrectDlcActiveForCurrentSave(brain.GetComponent<KPrefabID>()))
			{
				global::Tuple<Sprite, Color> icon = null;
				CodexEntry codexEntry;
				if (CodexCache.entries.TryGetValue(brain.species.ToString().ToUpper(), out codexEntry))
				{
					icon = new global::Tuple<Sprite, Color>(codexEntry.icon, codexEntry.iconColor);
				}
				list2.Add(brain.species);
				SandboxToolParameterMenu.SelectorValue.SearchFilter item5 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + brain.species.ToString().ToUpper()), delegate(object entity)
				{
					CreatureBrain component = Assets.GetPrefab((entity as KPrefabID).PrefabID()).GetComponent<CreatureBrain>();
					return (entity as KPrefabID).HasTag(GameTags.CreatureBrain) && component.species == brain.species;
				}, searchFilter, icon);
				list.Add(item5);
			}
		}
		SandboxToolParameterMenu.SelectorValue.SearchFilter item6 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE_EGG, (object entity) => (entity as KPrefabID).HasTag(GameTags.Egg), searchFilter, Def.GetUISprite(Assets.GetPrefab("HatchEgg"), "ui", false));
		list.Add(item6);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item7 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.EQUIPMENT, delegate(object entity)
		{
			if ((entity as KPrefabID).gameObject == null)
			{
				return false;
			}
			GameObject gameObject2 = (entity as KPrefabID).gameObject;
			return gameObject2 != null && gameObject2.GetComponent<Equippable>() != null;
		}, null, Def.GetUISprite(Assets.GetPrefab("Funky_Vest"), "ui", false));
		list.Add(item7);
		SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.PLANTS, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			return !(kprefabID2 == null) && !(kprefabID2.gameObject == null) && (kprefabID2 != null && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2)) && (kprefabID2.GetComponent<Harvestable>() != null || kprefabID2.GetComponent<WiltCondition>() != null);
		}, null, Def.GetUISprite(Assets.GetPrefab("PrickleFlower"), "ui", false));
		list.Add(searchFilter2);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item8 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.SEEDS, delegate(object entity)
		{
			if ((entity as KPrefabID).gameObject == null)
			{
				return false;
			}
			GameObject gameObject2 = (entity as KPrefabID).gameObject;
			return gameObject2 != null && gameObject2.GetComponent<PlantableSeed>() != null;
		}, searchFilter2, Def.GetUISprite(Assets.GetPrefab("PrickleFlowerSeed"), "ui", false));
		list.Add(item8);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item9 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.INDUSTRIAL_PRODUCTS, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			return !(kprefabID2 == null) && !(kprefabID2.gameObject == null) && !kprefabID2.HasTag(GameTags.DeprecatedContent) && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2) && (kprefabID2.HasTag(GameTags.IndustrialIngredient) || kprefabID2.HasTag(GameTags.TechComponents) || kprefabID2.HasTag(GameTags.IndustrialProduct) || kprefabID2.HasTag(GameTags.Medicine) || kprefabID2.HasTag(GameTags.MedicalSupplies) || kprefabID2.HasTag(GameTags.ChargedPortableBattery));
		}, null, Def.GetUISprite(Assets.GetPrefab("BasicCure"), "ui", false));
		list.Add(item9);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item10 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.ORE_CHUNKS, delegate(object entity)
		{
			Element element = ElementLoader.FindElementByTag((entity as KPrefabID).PrefabTag);
			return element != null && element.IsSolid;
		}, null, new global::Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-solid"), Color.white));
		list.Add(item10);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item11 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.BOTTLES, delegate(object entity)
		{
			Element element = ElementLoader.FindElementByTag((entity as KPrefabID).PrefabTag);
			return element != null && element.IsLiquid;
		}, null, new global::Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-liquids"), Color.white));
		list.Add(item11);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item12 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.CANISTERS, delegate(object entity)
		{
			Element element = ElementLoader.FindElementByTag((entity as KPrefabID).PrefabTag);
			return element != null && element.IsGas;
		}, null, new global::Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-gases"), Color.white));
		list.Add(item12);
		List<KPrefabID> list3 = new List<KPrefabID>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(kprefabID) && !kprefabID.HasTag(GameTags.HideFromSpawnTool))
			{
				using (List<SandboxToolParameterMenu.SelectorValue.SearchFilter>.Enumerator enumerator3 = list.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.condition(kprefabID))
						{
							list3.Add(kprefabID);
							break;
						}
					}
				}
			}
		}
		object[] options = list3.ToArray();
		this.entitySelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object entity)
		{
			this.settings.SetStringSetting("SandboxTools.SelectedEntity", (entity as KPrefabID).PrefabID().Name);
		}, (object entity) => (entity as KPrefabID).GetProperName(), null, delegate(object entity)
		{
			GameObject prefab = Assets.GetPrefab((entity as KPrefabID).PrefabTag);
			KPrefabID component = prefab.GetComponent<KPrefabID>();
			if (prefab != null)
			{
				if (component.HasTag(GameTags.BaseMinion))
				{
					return new global::Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel((entity as KPrefabID).PrefabID()), Color.white);
				}
				KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
				if (component2 != null && component2.AnimFiles.Length != 0 && component2.AnimFiles[0] != null)
				{
					return Def.GetUISprite(prefab, "ui", false);
				}
			}
			return null;
		}, UI.SANDBOXTOOLS.SETTINGS.SPAWN_ENTITY.NAME, list.ToArray());
	}

	// Token: 0x0600713E RID: 28990 RVA: 0x002B3EB0 File Offset: 0x002B20B0
	private void ConfigureStoryTraitSelector()
	{
		object[] options = Db.Get().Stories.resources.ToArray();
		this.storySelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object story)
		{
			this.settings.SetStringSetting("SandboxTools.SelectedStory", ((Story)story).Id);
		}, (object story) => Strings.Get((story as Story).StoryTrait.name), null, (object story) => new global::Tuple<Sprite, Color>(Assets.GetSprite(((Story)story).StoryTrait.icon), Color.white), UI.SANDBOXTOOLS.SETTINGS.SPAWN_STORY_TRAIT.NAME, null);
	}

	// Token: 0x0600713F RID: 28991 RVA: 0x002B3F34 File Offset: 0x002B2134
	private void ConfigureDiseaseSelector()
	{
		object[] options = Db.Get().Diseases.resources.ToArray();
		this.diseaseSelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object disease)
		{
			this.settings.SetStringSetting("SandboxTools.SelectedDisease", ((Disease)disease).Id);
		}, (object disease) => (disease as Disease).Name, null, (object disease) => new global::Tuple<Sprite, Color>(Assets.GetSprite("germ"), GlobalAssets.Instance.colorSet.GetColorByName((disease as Disease).overlayColourName)), UI.SANDBOXTOOLS.SETTINGS.DISEASE.NAME, null);
	}

	// Token: 0x06007140 RID: 28992 RVA: 0x002B3FB8 File Offset: 0x002B21B8
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (PlayerController.Instance.ActiveTool != null && SandboxToolParameterMenu.instance != null)
		{
			this.RefreshDisplay();
		}
	}

	// Token: 0x06007141 RID: 28993 RVA: 0x002B3FE8 File Offset: 0x002B21E8
	public void RefreshDisplay()
	{
		this.brushRadiusSlider.row.SetActive(PlayerController.Instance.ActiveTool is BrushTool);
		if (PlayerController.Instance.ActiveTool is BrushTool)
		{
			this.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
		}
		this.massSlider.SetValue(this.settings.GetFloatSetting("SandboxTools.Mass"), true);
		this.stressAdditiveSlider.SetValue(this.settings.GetFloatSetting("SandbosTools.StressAdditive"), true);
		this.RefreshTemperatureUnitDisplays();
		this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.Temperature"), true), true);
		this.temperatureAdditiveSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.TemperatureAdditive"), true), true);
		this.diseaseCountSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.DiseaseCount"), true);
		this.moraleSlider.SetValue((float)this.settings.GetIntSetting("SandbosTools.MoraleAdjustment"), true);
	}

	// Token: 0x06007142 RID: 28994 RVA: 0x002B4104 File Offset: 0x002B2304
	private void OnTemperatureUnitChanged(object _)
	{
		int num = this.settings.GetIntSetting("SandboxTools.SelectedElement");
		if (num >= ElementLoader.elements.Count)
		{
			num = 0;
		}
		Element absoluteTemperatureSliderRange = ElementLoader.elements[num];
		this.SetAbsoluteTemperatureSliderRange(absoluteTemperatureSliderRange);
		this.temperatureAdditiveSlider.SetValue(5f, true);
	}

	// Token: 0x06007143 RID: 28995 RVA: 0x002B4158 File Offset: 0x002B2358
	private void SetAbsoluteTemperatureSliderRange(Element element)
	{
		float num = Mathf.Max(element.lowTemp - 10f, 1f);
		float num2;
		if (element.IsGas)
		{
			num2 = Mathf.Min(new float[]
			{
				9999f,
				element.highTemp + 10f,
				element.defaultValues.temperature + 100f
			});
		}
		else
		{
			num2 = Mathf.Min(9999f, element.highTemp + 10f);
		}
		num = GameUtil.GetConvertedTemperature(num, true);
		num2 = GameUtil.GetConvertedTemperature(num2, true);
		this.temperatureSlider.SetRange(num, num2, false);
	}

	// Token: 0x06007144 RID: 28996 RVA: 0x002B41F4 File Offset: 0x002B23F4
	private void RefreshTemperatureUnitDisplays()
	{
		this.temperatureSlider.unitString = GameUtil.GetTemperatureUnitSuffix();
		this.temperatureSlider.row.GetComponent<HierarchyReferences>().GetReference<LocText>("UnitLabel").text = this.temperatureSlider.unitString;
		this.temperatureAdditiveSlider.unitString = GameUtil.GetTemperatureUnitSuffix();
		this.temperatureAdditiveSlider.row.GetComponent<HierarchyReferences>().GetReference<LocText>("UnitLabel").text = this.temperatureSlider.unitString;
	}

	// Token: 0x06007145 RID: 28997 RVA: 0x002B4278 File Offset: 0x002B2478
	private GameObject SpawnSelector(SandboxToolParameterMenu.SelectorValue selector)
	{
		GameObject gameObject = Util.KInstantiateUI(this.selectorPropertyPrefab, base.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		GameObject panel = component.GetReference("ScrollPanel").gameObject;
		GameObject gameObject2 = component.GetReference("Content").gameObject;
		InputField filterInputField = component.GetReference<InputField>("Filter");
		component.GetReference<LocText>("Label").SetText(selector.labelText);
		Game.Instance.Subscribe(1174281782, delegate(object data)
		{
			if (panel.activeSelf)
			{
				panel.SetActive(false);
			}
		});
		KButton reference = component.GetReference<KButton>("Button");
		reference.onClick += delegate()
		{
			panel.SetActive(!panel.activeSelf);
			if (panel.activeSelf)
			{
				panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
				filterInputField.ActivateInputField();
				filterInputField.onValueChanged.Invoke(filterInputField.text);
			}
		};
		GameObject gameObject3 = component.GetReference("optionPrefab").gameObject;
		selector.row = gameObject;
		selector.optionButtons = new List<KeyValuePair<object, GameObject>>();
		GameObject clearFilterButton = Util.KInstantiateUI(gameObject3, gameObject2, false);
		clearFilterButton.GetComponentInChildren<LocText>().text = UI.SANDBOXTOOLS.FILTERS.BACK;
		clearFilterButton.GetComponentsInChildren<Image>()[1].enabled = false;
		clearFilterButton.GetComponent<KButton>().onClick += delegate()
		{
			selector.currentFilter = null;
			selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
			{
				if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)
				{
					test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
					return;
				}
				test.Value.SetActive(false);
			});
			clearFilterButton.SetActive(false);
			panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
			filterInputField.text = "";
			filterInputField.onValueChanged.Invoke(filterInputField.text);
		};
		if (selector.filters != null)
		{
			SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters = selector.filters;
			for (int i = 0; i < filters.Length; i++)
			{
				SandboxToolParameterMenu.SelectorValue.SearchFilter filter = filters[i];
				GameObject gameObject4 = Util.KInstantiateUI(gameObject3, gameObject2, false);
				gameObject4.SetActive(filter.parentFilter == null);
				gameObject4.GetComponentInChildren<LocText>().text = filter.Name;
				if (filter.icon != null)
				{
					gameObject4.GetComponentsInChildren<Image>()[1].sprite = filter.icon.first;
					gameObject4.GetComponentsInChildren<Image>()[1].color = filter.icon.second;
				}
				Action<KeyValuePair<object, GameObject>> <>9__6;
				gameObject4.GetComponent<KButton>().onClick += delegate()
				{
					selector.currentFilter = filter;
					clearFilterButton.SetActive(true);
					List<KeyValuePair<object, GameObject>> optionButtons = selector.optionButtons;
					Action<KeyValuePair<object, GameObject>> action;
					if ((action = <>9__6) == null)
					{
						action = (<>9__6 = delegate(KeyValuePair<object, GameObject> test)
						{
							if (!(test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter))
							{
								test.Value.SetActive(selector.runCurrentFilter(test.Key));
								return;
							}
							if ((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null)
							{
								test.Value.SetActive(false);
								return;
							}
							test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == filter);
						});
					}
					optionButtons.ForEach(action);
					panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
				};
				selector.optionButtons.Add(new KeyValuePair<object, GameObject>(filter, gameObject4));
			}
		}
		object[] options = selector.options;
		for (int i = 0; i < options.Length; i++)
		{
			object option = options[i];
			GameObject gameObject5 = Util.KInstantiateUI(gameObject3, gameObject2, true);
			gameObject5.GetComponentInChildren<LocText>().text = selector.getOptionName(option);
			gameObject5.GetComponent<KButton>().onClick += delegate()
			{
				selector.onValueChanged(option);
				panel.SetActive(false);
			};
			global::Tuple<Sprite, Color> tuple = selector.getOptionSprite(option);
			gameObject5.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
			gameObject5.GetComponentsInChildren<Image>()[1].color = tuple.second;
			selector.optionButtons.Add(new KeyValuePair<object, GameObject>(option, gameObject5));
			if (option is SandboxToolParameterMenu.SelectorValue.SearchFilter)
			{
				gameObject5.SetActive((option as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
			}
			else
			{
				gameObject5.SetActive(false);
			}
		}
		selector.button = reference;
		filterInputField.onValueChanged.AddListener(delegate(string filterString)
		{
			if (!clearFilterButton.activeSelf && !string.IsNullOrEmpty(filterString))
			{
				clearFilterButton.SetActive(true);
			}
			new List<KeyValuePair<object, GameObject>>();
			bool flag = selector.optionButtons.Find((KeyValuePair<object, GameObject> match) => match.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter).Key != null;
			if (string.IsNullOrEmpty(filterString))
			{
				if (!flag)
				{
					selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
					{
						test.Value.SetActive(true);
					});
				}
				else
				{
					selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
					{
						if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter && ((SandboxToolParameterMenu.SelectorValue.SearchFilter)test.Key).parentFilter == null)
						{
							test.Value.SetActive(true);
							return;
						}
						test.Value.SetActive(false);
					});
				}
			}
			else
			{
				selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
				{
					if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)
					{
						test.Value.SetActive(((SandboxToolParameterMenu.SelectorValue.SearchFilter)test.Key).Name.ToUpper().Contains(filterString.ToUpper()));
						return;
					}
					test.Value.SetActive(selector.getOptionName(test.Key).ToUpper().Contains(filterString.ToUpper()));
				});
			}
			if (selector.filterOptionFunction != null)
			{
				object[] options2 = selector.options;
				for (int j = 0; j < options2.Length; j++)
				{
					object option = options2[j];
					foreach (KeyValuePair<object, GameObject> keyValuePair in selector.optionButtons.FindAll((KeyValuePair<object, GameObject> match) => match.Key == option))
					{
						if (string.IsNullOrEmpty(filterString))
						{
							keyValuePair.Value.SetActive(false);
						}
						else
						{
							keyValuePair.Value.SetActive(selector.filterOptionFunction(filterString, option));
						}
					}
				}
			}
			panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
		});
		this.inputFields.Add(filterInputField.gameObject);
		panel.SetActive(false);
		return gameObject;
	}

	// Token: 0x06007146 RID: 28998 RVA: 0x002B4650 File Offset: 0x002B2850
	private GameObject SpawnSlider(SandboxToolParameterMenu.SliderValue value)
	{
		GameObject gameObject = Util.KInstantiateUI(this.sliderPropertyPrefab, base.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("BottomIcon").sprite = Assets.GetSprite(value.bottomSprite);
		component.GetReference<Image>("TopIcon").sprite = Assets.GetSprite(value.topSprite);
		component.GetReference<LocText>("Label").SetText(value.labelText);
		KSlider slider = component.GetReference<KSlider>("Slider");
		KNumberInputField inputField = component.GetReference<KNumberInputField>("InputField");
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(value.tooltip);
		slider.minValue = value.slideMinValue;
		slider.maxValue = value.slideMaxValue;
		inputField.minValue = value.clampValueLow;
		inputField.maxValue = value.clampValueHigh;
		this.inputFields.Add(inputField.gameObject);
		value.slider = slider;
		inputField.decimalPlaces = value.roundToDecimalPlaces;
		value.inputField = inputField;
		value.row = gameObject;
		slider.onReleaseHandle += delegate()
		{
			float value2 = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			slider.value = value2;
			inputField.currentValue = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			inputField.SetDisplayValue(inputField.currentValue.ToString());
			if (value.onValueChanged != null)
			{
				value.onValueChanged(slider.value);
			}
		};
		slider.onDrag += delegate()
		{
			float num = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			slider.value = num;
			inputField.currentValue = num;
			inputField.SetDisplayValue(inputField.currentValue.ToString());
			if (value.onValueChanged != null)
			{
				value.onValueChanged(slider.value);
			}
		};
		slider.onMove += delegate()
		{
			float num = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			slider.value = num;
			inputField.currentValue = num;
			inputField.SetDisplayValue(inputField.currentValue.ToString());
			if (value.onValueChanged != null)
			{
				value.onValueChanged(slider.value);
			}
		};
		inputField.onEndEdit += delegate()
		{
			float num = inputField.currentValue;
			num = Mathf.Round(num * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			inputField.SetDisplayValue(num.ToString());
			slider.value = num;
			if (value.onValueChanged != null)
			{
				value.onValueChanged(num);
			}
		};
		component.GetReference<LocText>("UnitLabel").text = value.unitString;
		return gameObject;
	}

	// Token: 0x06007147 RID: 28999 RVA: 0x002B484B File Offset: 0x002B2A4B
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.CheckBlockedInput())
		{
			if (!e.Consumed)
			{
				e.Consumed = true;
				return;
			}
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06007148 RID: 29000 RVA: 0x002B486C File Offset: 0x002B2A6C
	private bool CheckBlockedInput()
	{
		bool result = false;
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				foreach (GameObject gameObject in this.inputFields)
				{
					if (currentSelectedGameObject == gameObject.gameObject)
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04004E2E RID: 20014
	public static SandboxToolParameterMenu instance;

	// Token: 0x04004E2F RID: 20015
	public SandboxSettings settings;

	// Token: 0x04004E30 RID: 20016
	[SerializeField]
	private GameObject sliderPropertyPrefab;

	// Token: 0x04004E31 RID: 20017
	[SerializeField]
	private GameObject selectorPropertyPrefab;

	// Token: 0x04004E32 RID: 20018
	private List<GameObject> inputFields = new List<GameObject>();

	// Token: 0x04004E33 RID: 20019
	public SandboxToolParameterMenu.SelectorValue elementSelector;

	// Token: 0x04004E34 RID: 20020
	public SandboxToolParameterMenu.SliderValue brushRadiusSlider = new SandboxToolParameterMenu.SliderValue(1f, 10f, "dash", "circle_hard", "", UI.SANDBOXTOOLS.SETTINGS.BRUSH_SIZE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.BRUSH_SIZE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.BrushSize", Mathf.Clamp(Mathf.RoundToInt(value), 1, 50));
	}, 0);

	// Token: 0x04004E35 RID: 20021
	public SandboxToolParameterMenu.SliderValue noiseScaleSlider = new SandboxToolParameterMenu.SliderValue(0f, 1f, "little", "lots", "", UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.NoiseScale", value);
	}, 2);

	// Token: 0x04004E36 RID: 20022
	public SandboxToolParameterMenu.SliderValue noiseDensitySlider = new SandboxToolParameterMenu.SliderValue(1f, 20f, "little", "lots", "", UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_DENSITY.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.NoiseDensity", value);
	}, 2);

	// Token: 0x04004E37 RID: 20023
	public SandboxToolParameterMenu.SliderValue massSlider = new SandboxToolParameterMenu.SliderValue(0.1f, 1000f, "action_pacify", "status_item_plant_solid", UI.UNITSUFFIXES.MASS.KILOGRAM, UI.SANDBOXTOOLS.SETTINGS.MASS.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.MASS.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.Mass", Mathf.Clamp(value, 0.001f, 9999f));
	}, 2);

	// Token: 0x04004E38 RID: 20024
	public SandboxToolParameterMenu.SliderValue temperatureSlider = new SandboxToolParameterMenu.SliderValue(150f, 500f, "cold", "hot", GameUtil.GetTemperatureUnitSuffix(), UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.Temperature", Mathf.Clamp(GameUtil.GetTemperatureConvertedToKelvin(value), 1f, 9999f));
	}, 0);

	// Token: 0x04004E39 RID: 20025
	public SandboxToolParameterMenu.SliderValue temperatureAdditiveSlider = new SandboxToolParameterMenu.SliderValue(-15f, 15f, "cold", "hot", GameUtil.GetTemperatureUnitSuffix(), UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE_ADDITIVE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE_ADDITIVE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.TemperatureAdditive", GameUtil.GetTemperatureConvertedToKelvin(value));
	}, 0);

	// Token: 0x04004E3A RID: 20026
	public SandboxToolParameterMenu.SliderValue stressAdditiveSlider = new SandboxToolParameterMenu.SliderValue(-10f, 10f, "little", "lots", UI.UNITSUFFIXES.PERCENT, UI.SANDBOXTOOLS.SETTINGS.STRESS_ADDITIVE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.STRESS_ADDITIVE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.StressAdditive", value);
	}, 0);

	// Token: 0x04004E3B RID: 20027
	public SandboxToolParameterMenu.SliderValue moraleSlider = new SandboxToolParameterMenu.SliderValue(-25f, 25f, "little", "lots", UI.UNITSUFFIXES.UNITS, UI.SANDBOXTOOLS.SETTINGS.MORALE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.MORALE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandbosTools.MoraleAdjustment", Mathf.RoundToInt(value));
	}, 0);

	// Token: 0x04004E3C RID: 20028
	public SandboxToolParameterMenu.SelectorValue diseaseSelector;

	// Token: 0x04004E3D RID: 20029
	public SandboxToolParameterMenu.SliderValue diseaseCountSlider = new SandboxToolParameterMenu.SliderValue(0f, 10000f, "status_item_barren", "germ", UI.UNITSUFFIXES.DISEASE.UNITS, UI.SANDBOXTOOLS.SETTINGS.DISEASE_COUNT.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.DISEASE_COUNT.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.DiseaseCount", Mathf.RoundToInt(value));
	}, 0);

	// Token: 0x04004E3E RID: 20030
	public SandboxToolParameterMenu.SelectorValue entitySelector;

	// Token: 0x04004E3F RID: 20031
	public SandboxToolParameterMenu.SelectorValue storySelector;

	// Token: 0x0200207A RID: 8314
	public class SelectorValue
	{
		// Token: 0x0600B946 RID: 47430 RVA: 0x003F7A88 File Offset: 0x003F5C88
		public SelectorValue(object[] options, Action<object> onValueChanged, Func<object, string> getOptionName, Func<string, object, bool> filterOptionFunction, Func<object, global::Tuple<Sprite, Color>> getOptionSprite, string labelText, SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters = null)
		{
			this.options = options;
			this.onValueChanged = onValueChanged;
			this.getOptionName = getOptionName;
			this.filterOptionFunction = filterOptionFunction;
			this.getOptionSprite = getOptionSprite;
			this.filters = filters;
			this.labelText = labelText;
		}

		// Token: 0x0600B947 RID: 47431 RVA: 0x003F7ADB File Offset: 0x003F5CDB
		public bool runCurrentFilter(object obj)
		{
			return this.currentFilter == null || this.currentFilter.condition(obj);
		}

		// Token: 0x0400961D RID: 38429
		public GameObject row;

		// Token: 0x0400961E RID: 38430
		public List<KeyValuePair<object, GameObject>> optionButtons;

		// Token: 0x0400961F RID: 38431
		public KButton button;

		// Token: 0x04009620 RID: 38432
		public object[] options;

		// Token: 0x04009621 RID: 38433
		public Action<object> onValueChanged;

		// Token: 0x04009622 RID: 38434
		public Func<object, string> getOptionName;

		// Token: 0x04009623 RID: 38435
		public Func<string, object, bool> filterOptionFunction;

		// Token: 0x04009624 RID: 38436
		public Func<object, global::Tuple<Sprite, Color>> getOptionSprite;

		// Token: 0x04009625 RID: 38437
		public SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters;

		// Token: 0x04009626 RID: 38438
		public List<SandboxToolParameterMenu.SelectorValue.SearchFilter> activeFilters = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();

		// Token: 0x04009627 RID: 38439
		public SandboxToolParameterMenu.SelectorValue.SearchFilter currentFilter;

		// Token: 0x04009628 RID: 38440
		public string labelText;

		// Token: 0x02002A82 RID: 10882
		public class SearchFilter
		{
			// Token: 0x0600D4F6 RID: 54518 RVA: 0x0043DB59 File Offset: 0x0043BD59
			public SearchFilter(string Name, Func<object, bool> condition, SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter = null, global::Tuple<Sprite, Color> icon = null)
			{
				this.Name = Name;
				this.condition = condition;
				this.parentFilter = parentFilter;
				this.icon = icon;
			}

			// Token: 0x0400BB8A RID: 48010
			public string Name;

			// Token: 0x0400BB8B RID: 48011
			public Func<object, bool> condition;

			// Token: 0x0400BB8C RID: 48012
			public SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter;

			// Token: 0x0400BB8D RID: 48013
			public global::Tuple<Sprite, Color> icon;
		}
	}

	// Token: 0x0200207B RID: 8315
	public class SliderValue
	{
		// Token: 0x0600B948 RID: 47432 RVA: 0x003F7B00 File Offset: 0x003F5D00
		public SliderValue(float slideMinValue, float slideMaxValue, string bottomSprite, string topSprite, string unitString, string tooltip, string labelText, Action<float> onValueChanged, int decimalPlaces = 0)
		{
			this.slideMinValue = slideMinValue;
			this.slideMaxValue = slideMaxValue;
			this.bottomSprite = bottomSprite;
			this.topSprite = topSprite;
			this.unitString = unitString;
			this.onValueChanged = onValueChanged;
			this.tooltip = tooltip;
			this.roundToDecimalPlaces = decimalPlaces;
			this.labelText = labelText;
			this.clampValueLow = slideMinValue;
			this.clampValueHigh = slideMaxValue;
		}

		// Token: 0x0600B949 RID: 47433 RVA: 0x003F7B68 File Offset: 0x003F5D68
		public void SetRange(float min, float max, bool resetCurrentValue = true)
		{
			this.slideMinValue = min;
			this.slideMaxValue = max;
			this.slider.minValue = this.slideMinValue;
			this.slider.maxValue = this.slideMaxValue;
			this.inputField.currentValue = this.slideMinValue + (this.slideMaxValue - this.slideMinValue) / 2f;
			this.inputField.SetDisplayValue(this.inputField.currentValue.ToString());
			if (resetCurrentValue)
			{
				this.slider.value = this.slideMinValue + (this.slideMaxValue - this.slideMinValue) / 2f;
				this.onValueChanged(this.slideMinValue + (this.slideMaxValue - this.slideMinValue) / 2f);
			}
		}

		// Token: 0x0600B94A RID: 47434 RVA: 0x003F7C34 File Offset: 0x003F5E34
		public void SetValue(float value, bool runOnValueChanged = true)
		{
			value = Mathf.Clamp(value, this.clampValueLow, this.clampValueHigh);
			this.slider.value = value;
			this.inputField.currentValue = value;
			if (runOnValueChanged)
			{
				this.onValueChanged(value);
			}
			this.RefreshDisplay();
		}

		// Token: 0x0600B94B RID: 47435 RVA: 0x003F7C84 File Offset: 0x003F5E84
		public void RefreshDisplay()
		{
			this.inputField.SetDisplayValue(((this.roundToDecimalPlaces == 0) ? ((float)Mathf.RoundToInt(this.inputField.currentValue)) : this.inputField.currentValue).ToString());
		}

		// Token: 0x04009629 RID: 38441
		public GameObject row;

		// Token: 0x0400962A RID: 38442
		public string bottomSprite;

		// Token: 0x0400962B RID: 38443
		public string topSprite;

		// Token: 0x0400962C RID: 38444
		public float slideMinValue;

		// Token: 0x0400962D RID: 38445
		public float slideMaxValue;

		// Token: 0x0400962E RID: 38446
		public float clampValueLow;

		// Token: 0x0400962F RID: 38447
		public float clampValueHigh;

		// Token: 0x04009630 RID: 38448
		public string unitString;

		// Token: 0x04009631 RID: 38449
		public Action<float> onValueChanged;

		// Token: 0x04009632 RID: 38450
		public string tooltip;

		// Token: 0x04009633 RID: 38451
		public int roundToDecimalPlaces;

		// Token: 0x04009634 RID: 38452
		public string labelText;

		// Token: 0x04009635 RID: 38453
		public KSlider slider;

		// Token: 0x04009636 RID: 38454
		public KNumberInputField inputField;
	}
}
