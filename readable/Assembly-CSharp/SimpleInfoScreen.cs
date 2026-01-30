using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using ProcGen;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E92 RID: 3730
public class SimpleInfoScreen : DetailScreenTab, ISim4000ms, ISim1000ms
{
	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x0600770E RID: 30478 RVA: 0x002D5F84 File Offset: 0x002D4184
	// (set) Token: 0x0600770F RID: 30479 RVA: 0x002D5F8C File Offset: 0x002D418C
	public CollapsibleDetailContentPanel StoragePanel { get; private set; }

	// Token: 0x06007710 RID: 30480 RVA: 0x002D5F95 File Offset: 0x002D4195
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x06007711 RID: 30481 RVA: 0x002D5F98 File Offset: 0x002D4198
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.processConditionContainer = base.CreateCollapsableSection(UI.DETAILTABS.PROCESS_CONDITIONS.NAME);
		this.statusItemPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_STATUS);
		this.statusItemPanel.Content.GetComponent<VerticalLayoutGroup>().padding.bottom = 10;
		this.statusItemPanel.scalerMask.hoverLock = true;
		this.statusItemsFolder = this.statusItemPanel.Content.gameObject;
		this.spaceSimpleInfoPOIPanel = new SpacePOISimpleInfoPanel(this);
		this.spacePOIPanel = base.CreateCollapsableSection(null);
		this.starmapHexCellStorageInfoPanel = new StarmapHexCellInventoryInfoPanel(this);
		this.spaceHexCellStoragePanel = base.CreateCollapsableSection(null);
		this.rocketSimpleInfoPanel = new RocketSimpleInfoPanel(this);
		this.rocketStatusContainer = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ROCKET);
		this.vitalsPanel = global::Util.KInstantiateUI(this.VitalsPanelTemplate, base.gameObject, false).GetComponent<MinionVitalsPanel>();
		this.fertilityPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_FERTILITY);
		this.mooFertilityPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_MOO_FERTILITY);
		this.infoPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_DESCRIPTION);
		this.requirementsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
		this.requirementContent = global::Util.KInstantiateUI<DescriptorPanel>(this.DescriptorContentPrefab.gameObject, this.requirementsPanel.Content.gameObject, false);
		this.effectsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_EFFECTS);
		this.effectsContent = global::Util.KInstantiateUI<DescriptorPanel>(this.DescriptorContentPrefab.gameObject, this.effectsPanel.Content.gameObject, false);
		this.worldMeteorShowersPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_METEORSHOWERS);
		this.worldElementsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ELEMENTS);
		this.worldGeysersPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_GEYSERS);
		this.worldTraitsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_WORLDTRAITS);
		this.worldBiomesPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_BIOMES);
		this.worldLifePanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_LIFE);
		this.StoragePanel = base.CreateCollapsableSection(null);
		this.stressPanel = base.CreateCollapsableSection(null);
		this.stressDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.stressPanel.Content.gameObject);
		this.movePanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_MOVABLE);
		base.Subscribe<SimpleInfoScreen>(-1514841199, SimpleInfoScreen.OnRefreshDataDelegate);
	}

	// Token: 0x06007712 RID: 30482 RVA: 0x002D6230 File Offset: 0x002D4430
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		base.Subscribe(target, -1697596308, new Action<object>(this.TriggerRefreshStorage));
		base.Subscribe(target, -1197125120, new Action<object>(this.TriggerRefreshStorage));
		base.Subscribe(target, 1059811075, new Action<object>(this.OnBreedingChanceChanged));
		base.Subscribe(target, 1105317911, new Action<object>(this.OnMooSongChanceChanged));
		KSelectable component = target.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
			if (statusItemGroup != null)
			{
				StatusItemGroup statusItemGroup2 = statusItemGroup;
				statusItemGroup2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(statusItemGroup2.OnAddStatusItem, new Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem));
				StatusItemGroup statusItemGroup3 = statusItemGroup;
				statusItemGroup3.OnRemoveStatusItem = (Action<StatusItemGroup.Entry, bool>)Delegate.Combine(statusItemGroup3.OnRemoveStatusItem, new Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem));
				foreach (StatusItemGroup.Entry entry in statusItemGroup)
				{
					if (entry.category != null && entry.category.Id == "Main")
					{
						this.DoAddStatusItem(entry, entry.category, false);
					}
				}
				foreach (StatusItemGroup.Entry entry2 in statusItemGroup)
				{
					if (entry2.category == null || entry2.category.Id != "Main")
					{
						this.DoAddStatusItem(entry2, entry2.category, false);
					}
				}
			}
		}
		this.statusItemPanel.gameObject.SetActive(true);
		this.statusItemPanel.scalerMask.UpdateSize();
		this.Refresh(true);
		this.RefreshWorldPanel();
		this.RefreshProcessConditionsPanel();
		this.spaceSimpleInfoPOIPanel.Refresh(this.spacePOIPanel, this.selectedTarget);
		this.starmapHexCellStorageInfoPanel.Refresh(this.spaceHexCellStoragePanel, this.selectedTarget);
	}

	// Token: 0x06007713 RID: 30483 RVA: 0x002D6434 File Offset: 0x002D4634
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		if (target != null)
		{
			base.Unsubscribe(target, -1697596308, new Action<object>(this.TriggerRefreshStorage));
			base.Unsubscribe(target, -1197125120, new Action<object>(this.TriggerRefreshStorage));
			base.Unsubscribe(target, 1059811075, new Action<object>(this.OnBreedingChanceChanged));
			base.Unsubscribe(target, 1105317911, new Action<object>(this.OnMooSongChanceChanged));
		}
		KSelectable component = target.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
			if (statusItemGroup != null)
			{
				StatusItemGroup statusItemGroup2 = statusItemGroup;
				statusItemGroup2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(statusItemGroup2.OnAddStatusItem, new Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem));
				StatusItemGroup statusItemGroup3 = statusItemGroup;
				statusItemGroup3.OnRemoveStatusItem = (Action<StatusItemGroup.Entry, bool>)Delegate.Remove(statusItemGroup3.OnRemoveStatusItem, new Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem));
				foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry in this.statusItems)
				{
					statusItemEntry.Destroy(true);
				}
				this.statusItems.Clear();
				foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry2 in this.oldStatusItems)
				{
					statusItemEntry2.onDestroy = null;
					statusItemEntry2.Destroy(true);
				}
				this.oldStatusItems.Clear();
			}
		}
	}

	// Token: 0x06007714 RID: 30484 RVA: 0x002D65B8 File Offset: 0x002D47B8
	private void OnStorageChange(object data)
	{
		SimpleInfoScreen.RefreshStoragePanel(this.StoragePanel, this.selectedTarget);
	}

	// Token: 0x06007715 RID: 30485 RVA: 0x002D65CB File Offset: 0x002D47CB
	private void OnBreedingChanceChanged(object data)
	{
		SimpleInfoScreen.RefreshFertilityPanel(this.fertilityPanel, this.selectedTarget);
	}

	// Token: 0x06007716 RID: 30486 RVA: 0x002D65DE File Offset: 0x002D47DE
	private void OnMooSongChanceChanged(object data)
	{
		SimpleInfoScreen.RefreshMooSongPanel(this.fertilityPanel, this.selectedTarget);
	}

	// Token: 0x06007717 RID: 30487 RVA: 0x002D65F1 File Offset: 0x002D47F1
	private void OnAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category)
	{
		this.DoAddStatusItem(status_item, category, false);
	}

	// Token: 0x06007718 RID: 30488 RVA: 0x002D65FC File Offset: 0x002D47FC
	private void DoAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category, bool show_immediate = false)
	{
		if (status_item.item.showInHoverCardOnly)
		{
			return;
		}
		GameObject gameObject = this.statusItemsFolder;
		Color color;
		if (status_item.item.notificationType == NotificationType.BadMinor || status_item.item.notificationType == NotificationType.Bad || status_item.item.notificationType == NotificationType.DuplicantThreatening)
		{
			color = GlobalAssets.Instance.colorSet.statusItemBad;
		}
		else if (status_item.item.notificationType == NotificationType.Event)
		{
			color = GlobalAssets.Instance.colorSet.statusItemEvent;
		}
		else if (status_item.item.notificationType == NotificationType.MessageImportant)
		{
			color = GlobalAssets.Instance.colorSet.statusItemMessageImportant;
		}
		else
		{
			color = this.statusItemTextColor_regular;
		}
		TextStyleSetting style = (category == Db.Get().StatusItemCategories.Main) ? this.StatusItemStyle_Main : this.StatusItemStyle_Other;
		SimpleInfoScreen.StatusItemEntry statusItemEntry = new SimpleInfoScreen.StatusItemEntry(status_item, category, this.StatusItemPrefab, gameObject.transform, this.ToolTipStyle_Property, color, style, show_immediate, new Action<SimpleInfoScreen.StatusItemEntry>(this.OnStatusItemDestroy));
		statusItemEntry.SetSprite(status_item.item.sprite);
		if (category != null)
		{
			int num = -1;
			foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry2 in this.oldStatusItems.FindAll((SimpleInfoScreen.StatusItemEntry e) => e.category == category))
			{
				num = statusItemEntry2.GetIndex();
				statusItemEntry2.Destroy(true);
				this.oldStatusItems.Remove(statusItemEntry2);
			}
			if (category == Db.Get().StatusItemCategories.Main)
			{
				num = 0;
			}
			if (num != -1)
			{
				statusItemEntry.SetIndex(num);
			}
		}
		this.statusItems.Add(statusItemEntry);
	}

	// Token: 0x06007719 RID: 30489 RVA: 0x002D67DC File Offset: 0x002D49DC
	private void OnRemoveStatusItem(StatusItemGroup.Entry status_item, bool immediate = false)
	{
		this.DoRemoveStatusItem(status_item, immediate);
	}

	// Token: 0x0600771A RID: 30490 RVA: 0x002D67E8 File Offset: 0x002D49E8
	private void DoRemoveStatusItem(StatusItemGroup.Entry status_item, bool destroy_immediate = false)
	{
		for (int i = 0; i < this.statusItems.Count; i++)
		{
			if (this.statusItems[i].item.item == status_item.item)
			{
				SimpleInfoScreen.StatusItemEntry statusItemEntry = this.statusItems[i];
				this.statusItems.RemoveAt(i);
				this.oldStatusItems.Add(statusItemEntry);
				statusItemEntry.Destroy(destroy_immediate);
				return;
			}
		}
	}

	// Token: 0x0600771B RID: 30491 RVA: 0x002D6856 File Offset: 0x002D4A56
	private void OnStatusItemDestroy(SimpleInfoScreen.StatusItemEntry item)
	{
		this.oldStatusItems.Remove(item);
	}

	// Token: 0x0600771C RID: 30492 RVA: 0x002D6865 File Offset: 0x002D4A65
	private void OnRefreshData(object obj)
	{
		this.Refresh(false);
	}

	// Token: 0x0600771D RID: 30493 RVA: 0x002D6870 File Offset: 0x002D4A70
	protected override void Refresh(bool force = false)
	{
		if (this.selectedTarget != this.lastTarget || force)
		{
			this.lastTarget = this.selectedTarget;
		}
		int count = this.statusItems.Count;
		this.statusItemPanel.gameObject.SetActive(count > 0);
		for (int i = 0; i < count; i++)
		{
			this.statusItems[i].Refresh();
		}
		SimpleInfoScreen.RefreshStressPanel(this.stressPanel, this.selectedTarget);
		SimpleInfoScreen.RefreshStoragePanel(this.StoragePanel, this.selectedTarget);
		SimpleInfoScreen.RefreshMovePanel(this.movePanel, this.selectedTarget);
		SimpleInfoScreen.RefreshFertilityPanel(this.fertilityPanel, this.selectedTarget);
		SimpleInfoScreen.RefreshMooSongPanel(this.mooFertilityPanel, this.selectedTarget);
		SimpleInfoScreen.RefreshEffectsPanel(this.effectsPanel, this.selectedTarget, this.effectsContent);
		SimpleInfoScreen.RefreshRequirementsPanel(this.requirementsPanel, this.selectedTarget, this.requirementContent);
		SimpleInfoScreen.RefreshInfoPanel(this.infoPanel, this.selectedTarget);
		this.vitalsPanel.Refresh(this.selectedTarget);
		this.rocketSimpleInfoPanel.Refresh(this.rocketStatusContainer, this.selectedTarget);
	}

	// Token: 0x0600771E RID: 30494 RVA: 0x002D6998 File Offset: 0x002D4B98
	public void Sim1000ms(float dt)
	{
		if (this.selectedTarget != null && this.selectedTarget.GetComponent<IProcessConditionSet>() != null)
		{
			this.RefreshProcessConditionsPanel();
		}
	}

	// Token: 0x0600771F RID: 30495 RVA: 0x002D69BB File Offset: 0x002D4BBB
	public void Sim4000ms(float dt)
	{
		this.RefreshWorldPanel();
		this.spaceSimpleInfoPOIPanel.Refresh(this.spacePOIPanel, this.selectedTarget);
		this.starmapHexCellStorageInfoPanel.Refresh(this.spaceHexCellStoragePanel, this.selectedTarget);
	}

	// Token: 0x06007720 RID: 30496 RVA: 0x002D69F4 File Offset: 0x002D4BF4
	private static void RefreshInfoPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		string text = "";
		string text2 = "";
		UnityEngine.Object component = targetEntity.GetComponent<MinionIdentity>();
		InfoDescription component2 = targetEntity.GetComponent<InfoDescription>();
		BuildingComplete component3 = targetEntity.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component4 = targetEntity.GetComponent<BuildingUnderConstruction>();
		Edible component5 = targetEntity.GetComponent<Edible>();
		PrimaryElement component6 = targetEntity.GetComponent<PrimaryElement>();
		CellSelectionObject component7 = targetEntity.GetComponent<CellSelectionObject>();
		if (!component)
		{
			if (component2)
			{
				text = component2.description;
				text2 = component2.effect;
			}
			else if (component3 != null)
			{
				text = component3.DescFlavour + "\n\n" + component3.Desc;
			}
			else if (component4 != null)
			{
				text = component4.DescFlavour + "\n\n" + component4.Desc;
			}
			else if (component5 != null)
			{
				EdiblesManager.FoodInfo foodInfo = component5.FoodInfo;
				text += string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true));
			}
			else if (component7 != null)
			{
				text = component7.element.FullDescription(false);
			}
			else if (component6 != null)
			{
				Element element = ElementLoader.FindElementByHash(component6.ElementID);
				text = ((element != null) ? element.FullDescription(false) : "");
			}
			if (!string.IsNullOrEmpty(text))
			{
				targetPanel.SetLabel("Description", text, "");
			}
			bool flag = !string.IsNullOrEmpty(text2) && text2 != "\n";
			string text3 = "\n" + text2;
			if (flag)
			{
				targetPanel.SetLabel("Flavour", text3, "");
			}
			string[] roomClassForObject = CodexEntryGenerator.GetRoomClassForObject(targetEntity);
			if (roomClassForObject != null)
			{
				string text4 = "\n" + CODEX.HEADERS.BUILDINGTYPE + ":";
				foreach (string str in roomClassForObject)
				{
					text4 = text4 + "\n    • " + str;
				}
				targetPanel.SetLabel("RoomClass", text4, "");
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06007721 RID: 30497 RVA: 0x002D6BF4 File Offset: 0x002D4DF4
	private static void RefreshEffectsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity, DescriptorPanel effectsContent)
	{
		if (targetEntity.GetComponent<MinionIdentity>() != null)
		{
			targetPanel.SetActive(false);
			return;
		}
		targetEntity.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component = targetEntity.GetComponent<BuildingUnderConstruction>();
		List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(component ? component.Def.BuildingComplete : targetEntity, true);
		bool flag = gameObjectEffects.Count > 0;
		effectsContent.gameObject.SetActive(flag);
		if (flag)
		{
			effectsContent.SetDescriptors(gameObjectEffects);
		}
		targetPanel.SetActive(targetEntity != null && flag);
	}

	// Token: 0x06007722 RID: 30498 RVA: 0x002D6C74 File Offset: 0x002D4E74
	private static void RefreshRequirementsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity, DescriptorPanel requirementContent)
	{
		MinionIdentity component = targetEntity.GetComponent<MinionIdentity>();
		UnityEngine.Object component2 = targetEntity.GetComponent<WiltCondition>();
		CreatureBrain component3 = targetEntity.GetComponent<CreatureBrain>();
		if (component2 != null || component != null || component3 != null)
		{
			targetPanel.SetActive(false);
			return;
		}
		targetPanel.SetActive(true);
		BuildingUnderConstruction component4 = targetEntity.GetComponent<BuildingUnderConstruction>();
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(component4 ? component4.Def.BuildingComplete : targetEntity, true), false);
		bool flag = requirementDescriptors.Count > 0;
		requirementContent.gameObject.SetActive(flag);
		if (flag)
		{
			requirementContent.SetDescriptors(requirementDescriptors);
		}
		targetPanel.SetActive(flag);
	}

	// Token: 0x06007723 RID: 30499 RVA: 0x002D6D14 File Offset: 0x002D4F14
	private static void RefreshFertilityPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		FertilityMonitor.Instance smi = targetEntity.GetSMI<FertilityMonitor.Instance>();
		if (smi != null)
		{
			int num = 0;
			foreach (FertilityMonitor.BreedingChance breedingChance in smi.breedingChances)
			{
				List<FertilityModifier> forTag = Db.Get().FertilityModifiers.GetForTag(breedingChance.egg);
				if (forTag.Count > 0)
				{
					string text = "";
					foreach (FertilityModifier fertilityModifier in forTag)
					{
						text += string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_MOD_FORMAT, fertilityModifier.GetTooltip());
					}
					targetPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None), text));
				}
				else
				{
					targetPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP_NOMOD, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)));
				}
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06007724 RID: 30500 RVA: 0x002D6F08 File Offset: 0x002D5108
	private static void RefreshMooSongPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		BeckoningMonitor.Instance smi = targetEntity.GetSMI<BeckoningMonitor.Instance>();
		if (smi != null)
		{
			int num = 0;
			foreach (BeckoningMonitor.SongChance songChance in smi.songChances)
			{
				List<MooSongModifier> forTag = Db.Get().MooSongModifiers.GetForTag(songChance.meteorID);
				if (forTag.Count > 0)
				{
					string text = "";
					foreach (MooSongModifier mooSongModifier in forTag)
					{
						text += string.Format(UI.DETAILTABS.MOO_SONG_CHANCES.CHANCE_MOD_FORMAT, mooSongModifier.GetTooltip());
					}
					targetPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.MOO_SONG_CHANCES.CHANCE_FORMAT, songChance.meteorID.ProperName(), GameUtil.GetFormattedPercent(songChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.MOO_SONG_CHANCES.CHANCE_FORMAT_TOOLTIP, songChance.meteorID.ProperName(), GameUtil.GetFormattedPercent(songChance.weight * 100f, GameUtil.TimeSlice.None), text));
				}
				else
				{
					targetPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.MOO_SONG_CHANCES.CHANCE_FORMAT, songChance.meteorID.ProperName(), GameUtil.GetFormattedPercent(songChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.MOO_SONG_CHANCES.CHANCE_FORMAT_TOOLTIP_NOMOD, songChance.meteorID.ProperName(), GameUtil.GetFormattedPercent(songChance.weight * 100f, GameUtil.TimeSlice.None)));
				}
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06007725 RID: 30501 RVA: 0x002D70FC File Offset: 0x002D52FC
	private void TriggerRefreshStorage(object data = null)
	{
		SimpleInfoScreen.RefreshStoragePanel(this.StoragePanel, this.selectedTarget);
	}

	// Token: 0x06007726 RID: 30502 RVA: 0x002D7110 File Offset: 0x002D5310
	private static void RefreshStoragePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			targetPanel.gameObject.SetActive(false);
			targetPanel.Commit();
			return;
		}
		IStorage[] array = targetEntity.GetComponentsInChildren<IStorage>();
		if (array == null)
		{
			targetPanel.gameObject.SetActive(false);
			targetPanel.Commit();
			return;
		}
		array = Array.FindAll<IStorage>(array, (IStorage n) => n.ShouldShowInUI());
		if (array.Length == 0)
		{
			targetPanel.gameObject.SetActive(false);
			targetPanel.Commit();
			return;
		}
		string title = (targetEntity.GetComponent<MinionIdentity>() != null) ? UI.DETAILTABS.DETAILS.GROUPNAME_MINION_CONTENTS : UI.DETAILTABS.DETAILS.GROUPNAME_CONTENTS;
		targetPanel.gameObject.SetActive(true);
		targetPanel.SetTitle(title);
		DictionaryPool<int, SimpleInfoScreen.StoredItemCategoryData, SimpleInfoScreen>.PooledDictionary pooledDictionary = DictionaryPool<int, SimpleInfoScreen.StoredItemCategoryData, SimpleInfoScreen>.Allocate();
		SimpleInfoScreen.storageItemPrefabDataIndexesFound.Clear();
		IStorage[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			foreach (GameObject gameObject in array2[i].GetItems())
			{
				if (!(gameObject == null))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
					if (!(component2 != null) || component2.Mass != 0f)
					{
						int hashCode = component.GetHashCode();
						float mass = component2.Mass;
						SimpleInfoScreen.StoredItemCategoryData storedItemCategoryData;
						if (!pooledDictionary.TryGetValue(hashCode, out storedItemCategoryData))
						{
							storedItemCategoryData = new SimpleInfoScreen.StoredItemCategoryData(component.GetProperName(), 0f, component2.MassPerUnit);
							pooledDictionary[hashCode] = storedItemCategoryData;
							SimpleInfoScreen.storageItemPrefabDataIndexesFound.Add(hashCode);
						}
						storedItemCategoryData.mass += mass;
						storedItemCategoryData.temperatureRanges.x = Mathf.Min(storedItemCategoryData.temperatureRanges.x, component2.Temperature);
						storedItemCategoryData.temperatureRanges.y = Mathf.Max(storedItemCategoryData.temperatureRanges.y, component2.Temperature);
						storedItemCategoryData.instancesFound++;
						storedItemCategoryData.lastInstance = component;
						storedItemCategoryData.lastPEInstance = component2;
					}
				}
			}
		}
		int num = 0;
		foreach (int num2 in SimpleInfoScreen.storageItemPrefabDataIndexesFound)
		{
			SimpleInfoScreen.StoredItemCategoryData storedItemCategoryData2 = pooledDictionary[num2];
			int prefabHashCode = num2;
			string text = "";
			string tooltip = "";
			string secondaryText = "";
			string text2 = "";
			text = storedItemCategoryData2.name;
			if (storedItemCategoryData2.instancesFound == 1)
			{
				SimpleInfoScreen.ForgeNameAndTooltipForStoredItem(storedItemCategoryData2.lastInstance, storedItemCategoryData2.lastPEInstance, out text, out secondaryText, out text2, out tooltip, true);
				text = "• " + text;
				KSelectable itemSelectable = storedItemCategoryData2.lastInstance.GetComponent<KSelectable>();
				targetPanel.SetLabelWithButton("storage_" + num.ToString(), text, secondaryText, text2, tooltip, delegate()
				{
					SelectTool.Instance.Select(itemSelectable, false);
				}).transform.SetAsFirstSibling();
			}
			else
			{
				text2 = (storedItemCategoryData2.usingUnits ? GameUtil.GetFormattedUnits(storedItemCategoryData2.mass / storedItemCategoryData2.massPerUnit, GameUtil.TimeSlice.None, true, "") : GameUtil.GetFormattedMass(storedItemCategoryData2.mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				targetPanel.SetCollapsableLabel("storage_group_" + prefabHashCode.ToString(), text, text2, tooltip, new SimpleInfoScreen.StorageCollapsibleRowData
				{
					prefabHashCode = prefabHashCode,
					storages = array
				}, new Action<DetailCollapsableLabel>(SimpleInfoScreen.OnStorageCollapsibleRowExpanded), new Action<DetailCollapsableLabel>(SimpleInfoScreen.OnStorageCollapsibleRowCollapsed));
			}
			storedItemCategoryData2.ClearData();
			num++;
		}
		if (num == 0)
		{
			targetPanel.SetLabel("storage_empty", UI.DETAILTABS.DETAILS.STORAGE_EMPTY, "");
		}
		pooledDictionary.Recycle();
		targetPanel.Commit();
	}

	// Token: 0x06007727 RID: 30503 RVA: 0x002D7510 File Offset: 0x002D5710
	private static string GetIconsForItemName(string itemName, KPrefabID item, PrimaryElement pe, int maxIconsAllowed)
	{
		string text = itemName;
		bool flag = maxIconsAllowed <= 0;
		char c = ' ';
		if (!flag && item.HasTag(GameTags.RotModifierTags.Refrigerated))
		{
			text = text + c.ToString() + UI.DETAILTABS.TEXTICONDATA.REFRIGERATED.ICON;
			flag = (--maxIconsAllowed <= 0);
		}
		else if (!flag && item.HasTag(GameTags.RotModifierTags.DeepFrozen))
		{
			text = text + c.ToString() + UI.DETAILTABS.TEXTICONDATA.DEEPFROZEN.ICON;
			flag = (--maxIconsAllowed <= 0);
		}
		if (!flag && item.HasTag(GameTags.SpicedFood))
		{
			text = text + c.ToString() + UI.DETAILTABS.TEXTICONDATA.SPICEDFOOD.ICON;
			flag = (--maxIconsAllowed <= 0);
		}
		if (!flag && item.HasTag(GameTags.RotModifierTags.Fresh))
		{
			text = text + c.ToString() + UI.DETAILTABS.TEXTICONDATA.FRESH.ICON;
			flag = (--maxIconsAllowed <= 0);
		}
		else if (!flag && item.HasTag(GameTags.RotModifierTags.Stale))
		{
			text = text + c.ToString() + UI.DETAILTABS.TEXTICONDATA.STALE.ICON;
			flag = (--maxIconsAllowed <= 0);
		}
		if (!flag && pe.DiseaseIdx != 255)
		{
			Disease disease = Db.Get().Diseases[(int)pe.DiseaseIdx];
			text = text + c.ToString() + UI.DETAILTABS.TEXTICONDATA.GERMS.ICON;
			flag = (--maxIconsAllowed <= 0);
		}
		if (flag)
		{
			text += "…";
		}
		return text;
	}

	// Token: 0x06007728 RID: 30504 RVA: 0x002D7674 File Offset: 0x002D5874
	private static string GetIconsLegendForItem(KPrefabID item, PrimaryElement pe, Rottable.Instance rottable, bool addTabs = false)
	{
		string text = "";
		string text2 = addTabs ? "\n  " : "\n";
		char c = ' ';
		if (item.HasTag(GameTags.SpicedFood))
		{
			text = string.Concat(new string[]
			{
				text,
				text2,
				UI.DETAILTABS.TEXTICONDATA.SPICEDFOOD.ICON,
				c.ToString(),
				UI.DETAILTABS.TEXTICONDATA.SPICEDFOOD.NAME
			});
		}
		if (item.HasTag(GameTags.RotModifierTags.Refrigerated))
		{
			text = string.Concat(new string[]
			{
				text,
				text2,
				UI.DETAILTABS.TEXTICONDATA.REFRIGERATED.ICON,
				c.ToString(),
				UI.DETAILTABS.TEXTICONDATA.REFRIGERATED.NAME
			});
		}
		else if (item.HasTag(GameTags.RotModifierTags.DeepFrozen))
		{
			text = string.Concat(new string[]
			{
				text,
				text2,
				UI.DETAILTABS.TEXTICONDATA.DEEPFROZEN.ICON,
				c.ToString(),
				UI.DETAILTABS.TEXTICONDATA.DEEPFROZEN.NAME
			});
		}
		if (item.HasTag(GameTags.RotModifierTags.Fresh))
		{
			string text3 = (rottable == null) ? UI.DETAILTABS.TEXTICONDATA.FRESH.NAME : rottable.StateString();
			text = string.Concat(new string[]
			{
				text,
				text2,
				UI.DETAILTABS.TEXTICONDATA.FRESH.ICON,
				c.ToString(),
				text3
			});
		}
		else if (item.HasTag(GameTags.RotModifierTags.Stale))
		{
			string text4 = (rottable == null) ? UI.DETAILTABS.TEXTICONDATA.STALE.NAME : rottable.StateString();
			text = string.Concat(new string[]
			{
				text,
				text2,
				UI.DETAILTABS.TEXTICONDATA.STALE.ICON,
				c.ToString(),
				text4
			});
		}
		if (pe.DiseaseIdx != 255)
		{
			Disease disease = Db.Get().Diseases[(int)pe.DiseaseIdx];
			text = string.Concat(new string[]
			{
				text,
				text2,
				UI.DETAILTABS.TEXTICONDATA.GERMS.ICON,
				c.ToString(),
				GameUtil.SafeStringFormat(UI.OVERLAYS.DISEASE.DISEASE_FORMAT, new object[]
				{
					disease.Name,
					GameUtil.GetFormattedDiseaseAmount(pe.DiseaseCount, GameUtil.TimeSlice.None)
				})
			});
		}
		return text.TrimStart('\n');
	}

	// Token: 0x06007729 RID: 30505 RVA: 0x002D787D File Offset: 0x002D5A7D
	private static string GetTrimmedString(string value, int maxCharacterCount)
	{
		if (value.Length <= maxCharacterCount)
		{
			return value;
		}
		return value.Substring(0, maxCharacterCount) + "…";
	}

	// Token: 0x0600772A RID: 30506 RVA: 0x002D789C File Offset: 0x002D5A9C
	private static void ForgeNameAndTooltipForStoredItem(KPrefabID itemPrefabID, PrimaryElement pe, out string nameText, out string temperatureText, out string massText, out string tooltip, bool trim)
	{
		GameObject gameObject = itemPrefabID.gameObject;
		Rottable.Instance smi = gameObject.GetSMI<Rottable.Instance>();
		HighEnergyParticleStorage component = gameObject.GetComponent<HighEnergyParticleStorage>();
		pe = ((pe == null) ? gameObject.GetComponent<PrimaryElement>() : pe);
		string text = UI.StripLinkFormatting(gameObject.GetProperName());
		nameText = (trim ? SimpleInfoScreen.GetTrimmedString(text, 15) : text);
		tooltip = "";
		massText = "";
		temperatureText = "";
		if (pe != null && component == null)
		{
			massText = ((pe.MassPerUnit > 1f) ? GameUtil.GetFormattedUnits(pe.Mass / pe.MassPerUnit, GameUtil.TimeSlice.None, true, "") : GameUtil.GetFormattedMass(pe.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			temperatureText = GameUtil.GetFormattedTemperature(pe.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			nameText = SimpleInfoScreen.GetIconsForItemName(nameText, itemPrefabID, pe, trim ? (15 - nameText.Length) : 99999);
			string iconsLegendForItem = SimpleInfoScreen.GetIconsLegendForItem(itemPrefabID, pe, smi, true);
			tooltip = text + "\n";
			tooltip += iconsLegendForItem;
			tooltip += (string.IsNullOrEmpty(iconsLegendForItem) ? "" : "\n\n");
		}
		if (component != null)
		{
			nameText = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
			temperatureText = GameUtil.GetFormattedHighEnergyParticles(component.Particles, GameUtil.TimeSlice.None, true);
		}
		if (smi != null)
		{
			tooltip += smi.GetToolTip();
		}
		tooltip.TrimEnd('\n');
	}

	// Token: 0x0600772B RID: 30507 RVA: 0x002D7A18 File Offset: 0x002D5C18
	private static void OnStorageCollapsibleRowExpanded(DetailCollapsableLabel collapsableLabel)
	{
		collapsableLabel.MarkAllRowsUnused();
		SimpleInfoScreen.StorageCollapsibleRowData storageCollapsibleRowData = collapsableLabel.Data as SimpleInfoScreen.StorageCollapsibleRowData;
		IStorage[] storages = storageCollapsibleRowData.storages;
		for (int i = 0; i < storages.Length; i++)
		{
			using (List<GameObject>.Enumerator enumerator = storages[i].GetItems().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject go = enumerator.Current;
					if (!(go == null))
					{
						KPrefabID component = go.GetComponent<KPrefabID>();
						if (component.GetHashCode() == storageCollapsibleRowData.prefabHashCode)
						{
							string text = "";
							string text2 = "";
							string text3 = "";
							string text4 = "";
							SimpleInfoScreen.ForgeNameAndTooltipForStoredItem(component, null, out text, out text3, out text2, out text4, true);
							DetailLabelWithButton detailLabelWithButton = collapsableLabel.AddOrGetAvailableContentRow();
							detailLabelWithButton.label.SetText(text);
							detailLabelWithButton.label2.SetText(text3);
							detailLabelWithButton.label3.SetText(text2);
							detailLabelWithButton.RefreshLabelsVisibility();
							detailLabelWithButton.toolTip.SetSimpleTooltip(text4.TrimEnd('\n'));
							detailLabelWithButton.button.ClearOnClick();
							detailLabelWithButton.button.onClick += delegate()
							{
								SelectTool.Instance.Select(go.GetComponent<KSelectable>(), false);
							};
						}
					}
				}
			}
		}
		collapsableLabel.RefreshRowVisibilityState();
	}

	// Token: 0x0600772C RID: 30508 RVA: 0x002D7B74 File Offset: 0x002D5D74
	private static void OnStorageCollapsibleRowCollapsed(DetailCollapsableLabel collapsableLabel)
	{
		collapsableLabel.MarkAllRowsUnused();
		collapsableLabel.RefreshRowVisibilityState();
	}

	// Token: 0x0600772D RID: 30509 RVA: 0x002D7B84 File Offset: 0x002D5D84
	private void CreateWorldTraitRow()
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		this.worldTraitRows.Add(gameObject);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").gameObject.SetActive(false);
		component.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
	}

	// Token: 0x0600772E RID: 30510 RVA: 0x002D7BEC File Offset: 0x002D5DEC
	private static void RefreshMovePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		CancellableMove component = targetEntity.GetComponent<CancellableMove>();
		Movable moving = targetEntity.GetComponent<Movable>();
		if (component != null)
		{
			List<Ref<Movable>> movingObjects = component.movingObjects;
			int num = 0;
			using (List<Ref<Movable>>.Enumerator enumerator = movingObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<Movable> @ref = enumerator.Current;
					Movable movable = @ref.Get();
					GameObject go = (movable != null) ? movable.gameObject : null;
					if (!(go == null))
					{
						PrimaryElement component2 = go.GetComponent<PrimaryElement>();
						if (!(component2 != null) || component2.Mass != 0f)
						{
							Rottable.Instance smi = go.GetSMI<Rottable.Instance>();
							HighEnergyParticleStorage component3 = go.GetComponent<HighEnergyParticleStorage>();
							string text = "";
							string text2 = "";
							if (component2 != null && component3 == null)
							{
								text = GameUtil.GetUnitFormattedName(go, false);
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedMass(component2.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE, text, GameUtil.GetFormattedTemperature(component2.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
							}
							if (component3 != null)
							{
								text = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedHighEnergyParticles(component3.Particles, GameUtil.TimeSlice.None, true));
							}
							if (smi != null)
							{
								string text3 = smi.StateString();
								if (!string.IsNullOrEmpty(text3))
								{
									text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, text3);
								}
								text2 += smi.GetToolTip();
							}
							if (component2.DiseaseIdx != 255)
							{
								text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_DISEASED, GameUtil.GetFormattedDisease(component2.DiseaseIdx, component2.DiseaseCount, false));
								string formattedDisease = GameUtil.GetFormattedDisease(component2.DiseaseIdx, component2.DiseaseCount, true);
								text2 += formattedDisease;
							}
							targetPanel.SetLabelWithButton("move_" + num.ToString(), text, text2, delegate()
							{
								SelectTool.Instance.SelectAndFocus(go.transform.GetPosition(), go.GetComponent<KSelectable>(), new Vector3(5f, 0f, 0f));
							});
							num++;
						}
					}
				}
				goto IL_29C;
			}
		}
		if (moving != null && moving.IsMarkedForMove)
		{
			targetPanel.SetLabelWithButton("moveplacer", MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS, MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS_TOOLTIP, delegate()
			{
				SelectTool.Instance.SelectAndFocus(moving.StorageProxy.transform.GetPosition(), moving.StorageProxy.GetComponent<KSelectable>(), new Vector3(5f, 0f, 0f));
			});
		}
		IL_29C:
		targetPanel.Commit();
	}

	// Token: 0x0600772F RID: 30511 RVA: 0x002D7EB8 File Offset: 0x002D60B8
	private void RefreshWorldPanel()
	{
		WorldContainer worldContainer = (this.selectedTarget == null) ? null : this.selectedTarget.GetComponent<WorldContainer>();
		AsteroidGridEntity x = (this.selectedTarget == null) ? null : this.selectedTarget.GetComponent<AsteroidGridEntity>();
		bool flag = ManagementMenu.Instance.IsScreenOpen(ClusterMapScreen.Instance) && worldContainer != null && x != null;
		this.worldBiomesPanel.gameObject.SetActive(flag);
		this.worldGeysersPanel.gameObject.SetActive(flag);
		this.worldMeteorShowersPanel.gameObject.SetActive(flag);
		this.worldTraitsPanel.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.biomeRows)
		{
			keyValuePair.Value.SetActive(false);
		}
		if (worldContainer.Biomes != null)
		{
			using (List<string>.Enumerator enumerator2 = worldContainer.Biomes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string text = enumerator2.Current;
					Sprite biomeSprite = GameUtil.GetBiomeSprite(text);
					if (!this.biomeRows.ContainsKey(text))
					{
						this.biomeRows.Add(text, global::Util.KInstantiateUI(this.bigIconLabelRow, this.worldBiomesPanel.Content.gameObject, true));
						HierarchyReferences component = this.biomeRows[text].GetComponent<HierarchyReferences>();
						component.GetReference<Image>("Icon").sprite = biomeSprite;
						component.GetReference<LocText>("NameLabel").SetText(UI.FormatAsLink(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".NAME"), "BIOME" + text.ToUpper()));
						component.GetReference<LocText>("DescriptionLabel").SetText(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".DESC"));
					}
					this.biomeRows[text].SetActive(true);
				}
				goto IL_23C;
			}
		}
		this.worldBiomesPanel.gameObject.SetActive(false);
		IL_23C:
		List<Tag> list = new List<Tag>();
		foreach (Geyser cmp in Components.Geysers.GetItems(worldContainer.id))
		{
			list.Add(cmp.PrefabID());
		}
		list.AddRange(SaveGame.Instance.worldGenSpawner.GetUnspawnedWithType<Geyser>(worldContainer.id));
		list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("OilWell", worldContainer.id, true));
		foreach (KeyValuePair<Tag, GameObject> keyValuePair2 in this.geyserRows)
		{
			keyValuePair2.Value.SetActive(false);
		}
		foreach (Tag tag in list)
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			if (!this.geyserRows.ContainsKey(tag))
			{
				this.geyserRows.Add(tag, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
				HierarchyReferences component2 = this.geyserRows[tag].GetComponent<HierarchyReferences>();
				component2.GetReference<Image>("Icon").sprite = uisprite.first;
				component2.GetReference<Image>("Icon").color = uisprite.second;
				component2.GetReference<LocText>("NameLabel").SetText(Assets.GetPrefab(tag).GetProperName());
				component2.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
			}
			this.geyserRows[tag].SetActive(true);
		}
		int count = SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("GeyserGeneric", worldContainer.id, false).Count;
		if (count > 0)
		{
			global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite("GeyserGeneric", "ui", false);
			Tag key = "GeyserGeneric";
			if (!this.geyserRows.ContainsKey(key))
			{
				this.geyserRows.Add(key, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
			}
			HierarchyReferences component3 = this.geyserRows[key].GetComponent<HierarchyReferences>();
			component3.GetReference<Image>("Icon").sprite = uisprite2.first;
			component3.GetReference<Image>("Icon").color = uisprite2.second;
			component3.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.UNKNOWN_GEYSERS.Replace("{num}", count.ToString()));
			component3.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
			this.geyserRows[key].SetActive(true);
		}
		Tag key2 = "NoGeysers";
		if (!this.geyserRows.ContainsKey(key2))
		{
			this.geyserRows.Add(key2, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
			HierarchyReferences component4 = this.geyserRows[key2].GetComponent<HierarchyReferences>();
			component4.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
			component4.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_GEYSERS);
			component4.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
		}
		this.geyserRows[key2].gameObject.SetActive(list.Count == 0 && count == 0);
		foreach (KeyValuePair<Tag, GameObject> keyValuePair3 in this.meteorShowerRows)
		{
			keyValuePair3.Value.SetActive(false);
		}
		bool flag2 = false;
		foreach (string id in worldContainer.GetSeasonIds())
		{
			GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(id);
			if (gameplaySeason != null)
			{
				foreach (GameplayEvent gameplayEvent in gameplaySeason.events)
				{
					if (gameplayEvent.tags.Contains(GameTags.SpaceDanger) && gameplayEvent is MeteorShowerEvent)
					{
						flag2 = true;
						MeteorShowerEvent meteorShowerEvent = gameplayEvent as MeteorShowerEvent;
						string id2 = meteorShowerEvent.Id;
						global::Tuple<Sprite, Color> uisprite3 = Def.GetUISprite(meteorShowerEvent.GetClusterMapMeteorShowerID(), "ui", false);
						if (!this.meteorShowerRows.ContainsKey(id2))
						{
							this.meteorShowerRows.Add(id2, global::Util.KInstantiateUI(this.iconLabelRow, this.worldMeteorShowersPanel.Content.gameObject, true));
							HierarchyReferences component5 = this.meteorShowerRows[id2].GetComponent<HierarchyReferences>();
							component5.GetReference<Image>("Icon").sprite = uisprite3.first;
							component5.GetReference<Image>("Icon").color = uisprite3.second;
							component5.GetReference<LocText>("NameLabel").SetText(Assets.GetPrefab(meteorShowerEvent.GetClusterMapMeteorShowerID()).GetProperName());
							component5.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
						}
						this.meteorShowerRows[id2].SetActive(true);
					}
				}
			}
		}
		Tag key3 = "NoMeteorShowers";
		if (!this.meteorShowerRows.ContainsKey(key3))
		{
			this.meteorShowerRows.Add(key3, global::Util.KInstantiateUI(this.iconLabelRow, this.worldMeteorShowersPanel.Content.gameObject, true));
			HierarchyReferences component6 = this.meteorShowerRows[key3].GetComponent<HierarchyReferences>();
			component6.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
			component6.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_METEORSHOWERS);
			component6.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
		}
		this.meteorShowerRows[key3].gameObject.SetActive(!flag2);
		List<string> worldTraitIds = worldContainer.WorldTraitIds;
		if (worldTraitIds != null)
		{
			for (int i = 0; i < worldTraitIds.Count; i++)
			{
				if (i > this.worldTraitRows.Count - 1)
				{
					this.CreateWorldTraitRow();
				}
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraitIds[i], false);
				Image reference = this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
				if (cachedWorldTrait != null)
				{
					Sprite sprite = Assets.GetSprite(cachedWorldTrait.filePath.Substring(cachedWorldTrait.filePath.LastIndexOf("/") + 1));
					reference.gameObject.SetActive(true);
					reference.sprite = ((sprite == null) ? Assets.GetSprite("unknown") : sprite);
					reference.color = global::Util.ColorFromHex(cachedWorldTrait.colorHex);
					this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(Strings.Get(cachedWorldTrait.name));
					this.worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip(Strings.Get(cachedWorldTrait.description));
				}
				else
				{
					Sprite sprite2 = Assets.GetSprite("NoTraits");
					reference.gameObject.SetActive(true);
					reference.sprite = sprite2;
					reference.color = Color.white;
					this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(WORLD_TRAITS.MISSING_TRAIT);
					this.worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip("");
				}
			}
			for (int j = 0; j < this.worldTraitRows.Count; j++)
			{
				this.worldTraitRows[j].SetActive(j < worldTraitIds.Count);
			}
			if (worldTraitIds.Count == 0)
			{
				if (this.worldTraitRows.Count < 1)
				{
					this.CreateWorldTraitRow();
				}
				Image reference2 = this.worldTraitRows[0].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
				Sprite sprite3 = Assets.GetSprite("NoTraits");
				reference2.gameObject.SetActive(true);
				reference2.sprite = sprite3;
				reference2.color = Color.black;
				this.worldTraitRows[0].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND);
				this.worldTraitRows[0].AddOrGet<ToolTip>().SetSimpleTooltip(WORLD_TRAITS.NO_TRAITS.DESCRIPTION);
				this.worldTraitRows[0].SetActive(true);
			}
		}
		for (int k = this.surfaceConditionRows.Count - 1; k >= 0; k--)
		{
			global::Util.KDestroyGameObject(this.surfaceConditionRows[k]);
		}
		this.surfaceConditionRows.Clear();
		GameObject gameObject = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		HierarchyReferences component7 = gameObject.GetComponent<HierarchyReferences>();
		component7.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_lights");
		component7.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.LIGHT);
		component7.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedLux(worldContainer.SunlightFixedTraits[worldContainer.sunlightFixedTrait]));
		component7.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		this.surfaceConditionRows.Add(gameObject);
		GameObject gameObject2 = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		HierarchyReferences component8 = gameObject2.GetComponent<HierarchyReferences>();
		component8.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_radiation");
		component8.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.RADIATION);
		component8.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedRads((float)worldContainer.CosmicRadiationFixedTraits[worldContainer.cosmicRadiationFixedTrait], GameUtil.TimeSlice.None));
		component8.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		this.surfaceConditionRows.Add(gameObject2);
	}

	// Token: 0x06007730 RID: 30512 RVA: 0x002D8C6C File Offset: 0x002D6E6C
	private void RefreshProcessConditionsPanel()
	{
		foreach (GameObject original in this.processConditionRows)
		{
			global::Util.KDestroyGameObject(original);
		}
		this.processConditionRows.Clear();
		this.processConditionContainer.SetActive(this.selectedTarget.GetComponent<IProcessConditionSet>() != null);
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			if (this.selectedTarget.GetComponent<LaunchableRocket>() != null)
			{
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketPrep);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketStorage);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketBoard);
				return;
			}
			this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.All);
			return;
		}
		else
		{
			if (this.selectedTarget.GetComponent<LaunchPad>() != null || this.selectedTarget.GetComponent<RocketProcessConditionDisplayTarget>() != null)
			{
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketFlight);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketPrep);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketStorage);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketBoard);
				return;
			}
			this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.All);
			return;
		}
	}

	// Token: 0x06007731 RID: 30513 RVA: 0x002D8D98 File Offset: 0x002D6F98
	private static void RefreshStressPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		MinionIdentity identity = (targetEntity != null) ? targetEntity.GetComponent<MinionIdentity>() : null;
		if (identity != null)
		{
			List<ReportManager.ReportEntry.Note> stressNotes = new List<ReportManager.ReportEntry.Note>();
			targetPanel.gameObject.SetActive(true);
			targetPanel.SetTitle(UI.DETAILTABS.STATS.GROUPNAME_STRESS);
			ReportManager.ReportEntry reportEntry = ReportManager.Instance.TodaysReport.reportEntries.Find((ReportManager.ReportEntry entry) => entry.reportType == ReportManager.ReportType.StressDelta);
			float num = 0f;
			stressNotes.Clear();
			int num2 = reportEntry.contextEntries.FindIndex((ReportManager.ReportEntry entry) => entry.context == identity.GetProperName());
			ReportManager.ReportEntry reportEntry2 = (num2 != -1) ? reportEntry.contextEntries[num2] : null;
			if (reportEntry2 != null)
			{
				reportEntry2.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
				{
					stressNotes.Add(note);
				});
				stressNotes.Sort((ReportManager.ReportEntry.Note a, ReportManager.ReportEntry.Note b) => a.value.CompareTo(b.value));
				for (int i = 0; i < stressNotes.Count; i++)
				{
					string text = float.IsNegativeInfinity(stressNotes[i].value) ? UI.NEG_INFINITY.ToString() : global::Util.FormatTwoDecimalPlace(stressNotes[i].value);
					targetPanel.SetLabel("stressNotes_" + i.ToString(), string.Concat(new string[]
					{
						(stressNotes[i].value > 0f) ? UIConstants.ColorPrefixRed : "",
						stressNotes[i].note,
						": ",
						text,
						"%",
						(stressNotes[i].value > 0f) ? UIConstants.ColorSuffix : ""
					}), "");
					num += stressNotes[i].value;
				}
			}
			string arg = float.IsNegativeInfinity(num) ? UI.NEG_INFINITY.ToString() : global::Util.FormatTwoDecimalPlace(num);
			targetPanel.SetLabel("net_stress", ((num > 0f) ? UIConstants.ColorPrefixRed : "") + string.Format(UI.DETAILTABS.DETAILS.NET_STRESS, arg) + ((num > 0f) ? UIConstants.ColorSuffix : ""), "");
		}
		targetPanel.Commit();
	}

	// Token: 0x06007732 RID: 30514 RVA: 0x002D9040 File Offset: 0x002D7240
	private void RefreshProcessConditionsForType(GameObject target, ProcessCondition.ProcessConditionType conditionType)
	{
		IProcessConditionSet component = target.GetComponent<IProcessConditionSet>();
		if (component == null)
		{
			return;
		}
		List<ProcessCondition> list;
		using (ProcessCondition.ListPool.Get(out list))
		{
			if (component.PopulateConditionSet(conditionType, list) != 0)
			{
				HierarchyReferences hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.processConditionHeader.gameObject, this.processConditionContainer.Content.gameObject, true);
				hierarchyReferences.GetReference<LocText>("Label").text = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." + conditionType.ToString().ToUpper());
				hierarchyReferences.GetComponent<ToolTip>().toolTip = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." + conditionType.ToString().ToUpper() + "_TOOLTIP");
				this.processConditionRows.Add(hierarchyReferences.gameObject);
				List<ProcessCondition> list2 = new List<ProcessCondition>();
				using (List<ProcessCondition>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ProcessCondition condition = enumerator.Current;
						if (condition.ShowInUI() && (condition.GetType() == typeof(RequireAttachedComponent) || list2.Find((ProcessCondition match) => match.GetType() == condition.GetType()) == null))
						{
							list2.Add(condition);
							GameObject gameObject = global::Util.KInstantiateUI(this.processConditionRow, this.processConditionContainer.Content.gameObject, true);
							this.processConditionRows.Add(gameObject);
							ConditionListSideScreen.SetRowState(gameObject, condition);
						}
					}
				}
			}
		}
	}

	// Token: 0x04005262 RID: 21090
	public GameObject iconLabelRow;

	// Token: 0x04005263 RID: 21091
	public GameObject spacerRow;

	// Token: 0x04005264 RID: 21092
	[SerializeField]
	private GameObject attributesLabelTemplate;

	// Token: 0x04005265 RID: 21093
	[SerializeField]
	private GameObject attributesLabelButtonTemplate;

	// Token: 0x04005266 RID: 21094
	[SerializeField]
	private DescriptorPanel DescriptorContentPrefab;

	// Token: 0x04005267 RID: 21095
	[SerializeField]
	private GameObject VitalsPanelTemplate;

	// Token: 0x04005268 RID: 21096
	[SerializeField]
	private GameObject StatusItemPrefab;

	// Token: 0x04005269 RID: 21097
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x0400526A RID: 21098
	[SerializeField]
	private HierarchyReferences processConditionHeader;

	// Token: 0x0400526B RID: 21099
	[SerializeField]
	private GameObject processConditionRow;

	// Token: 0x0400526C RID: 21100
	[SerializeField]
	private Text StatusPanelCurrentActionLabel;

	// Token: 0x0400526D RID: 21101
	[SerializeField]
	private GameObject bigIconLabelRow;

	// Token: 0x0400526E RID: 21102
	[SerializeField]
	private TextStyleSetting ToolTipStyle_Property;

	// Token: 0x0400526F RID: 21103
	[SerializeField]
	private TextStyleSetting StatusItemStyle_Main;

	// Token: 0x04005270 RID: 21104
	[SerializeField]
	private TextStyleSetting StatusItemStyle_Other;

	// Token: 0x04005271 RID: 21105
	[SerializeField]
	private Color statusItemTextColor_regular = Color.black;

	// Token: 0x04005272 RID: 21106
	[SerializeField]
	private Color statusItemTextColor_old = new Color(0.8235294f, 0.8235294f, 0.8235294f);

	// Token: 0x04005274 RID: 21108
	private CollapsibleDetailContentPanel statusItemPanel;

	// Token: 0x04005275 RID: 21109
	private MinionVitalsPanel vitalsPanel;

	// Token: 0x04005276 RID: 21110
	private CollapsibleDetailContentPanel fertilityPanel;

	// Token: 0x04005277 RID: 21111
	private CollapsibleDetailContentPanel mooFertilityPanel;

	// Token: 0x04005278 RID: 21112
	private CollapsibleDetailContentPanel rocketStatusContainer;

	// Token: 0x04005279 RID: 21113
	private CollapsibleDetailContentPanel worldLifePanel;

	// Token: 0x0400527A RID: 21114
	private CollapsibleDetailContentPanel worldElementsPanel;

	// Token: 0x0400527B RID: 21115
	private CollapsibleDetailContentPanel worldBiomesPanel;

	// Token: 0x0400527C RID: 21116
	private CollapsibleDetailContentPanel worldGeysersPanel;

	// Token: 0x0400527D RID: 21117
	private CollapsibleDetailContentPanel worldMeteorShowersPanel;

	// Token: 0x0400527E RID: 21118
	private CollapsibleDetailContentPanel spacePOIPanel;

	// Token: 0x0400527F RID: 21119
	private CollapsibleDetailContentPanel spaceHexCellStoragePanel;

	// Token: 0x04005280 RID: 21120
	private CollapsibleDetailContentPanel worldTraitsPanel;

	// Token: 0x04005281 RID: 21121
	private CollapsibleDetailContentPanel processConditionContainer;

	// Token: 0x04005282 RID: 21122
	private CollapsibleDetailContentPanel requirementsPanel;

	// Token: 0x04005283 RID: 21123
	private CollapsibleDetailContentPanel effectsPanel;

	// Token: 0x04005284 RID: 21124
	private CollapsibleDetailContentPanel stressPanel;

	// Token: 0x04005285 RID: 21125
	private CollapsibleDetailContentPanel infoPanel;

	// Token: 0x04005286 RID: 21126
	private CollapsibleDetailContentPanel movePanel;

	// Token: 0x04005287 RID: 21127
	private DescriptorPanel effectsContent;

	// Token: 0x04005288 RID: 21128
	private DescriptorPanel requirementContent;

	// Token: 0x04005289 RID: 21129
	private RocketSimpleInfoPanel rocketSimpleInfoPanel;

	// Token: 0x0400528A RID: 21130
	private SpacePOISimpleInfoPanel spaceSimpleInfoPOIPanel;

	// Token: 0x0400528B RID: 21131
	private StarmapHexCellInventoryInfoPanel starmapHexCellStorageInfoPanel;

	// Token: 0x0400528C RID: 21132
	private DetailsPanelDrawer stressDrawer;

	// Token: 0x0400528D RID: 21133
	private bool TargetIsMinion;

	// Token: 0x0400528E RID: 21134
	private GameObject lastTarget;

	// Token: 0x0400528F RID: 21135
	private GameObject statusItemsFolder;

	// Token: 0x04005290 RID: 21136
	private Dictionary<Tag, GameObject> lifeformRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04005291 RID: 21137
	private Dictionary<Tag, GameObject> biomeRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04005292 RID: 21138
	private Dictionary<Tag, GameObject> geyserRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04005293 RID: 21139
	private Dictionary<Tag, GameObject> meteorShowerRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04005294 RID: 21140
	private List<GameObject> worldTraitRows = new List<GameObject>();

	// Token: 0x04005295 RID: 21141
	private List<GameObject> surfaceConditionRows = new List<GameObject>();

	// Token: 0x04005296 RID: 21142
	private List<SimpleInfoScreen.StatusItemEntry> statusItems = new List<SimpleInfoScreen.StatusItemEntry>();

	// Token: 0x04005297 RID: 21143
	private List<SimpleInfoScreen.StatusItemEntry> oldStatusItems = new List<SimpleInfoScreen.StatusItemEntry>();

	// Token: 0x04005298 RID: 21144
	private List<GameObject> processConditionRows = new List<GameObject>();

	// Token: 0x04005299 RID: 21145
	private static readonly EventSystem.IntraObjectHandler<SimpleInfoScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<SimpleInfoScreen>(delegate(SimpleInfoScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x0400529A RID: 21146
	private const string STORAGE_ROW_ID_PREFIX = "storage_";

	// Token: 0x0400529B RID: 21147
	private const string STORAGE_GROUP_ROW_ID_PREFIX = "storage_group_";

	// Token: 0x0400529C RID: 21148
	private const int MAXStoreItemNameCharacterCount = 15;

	// Token: 0x0400529D RID: 21149
	private const string TRIMMED_STRING = "…";

	// Token: 0x0400529E RID: 21150
	private static List<int> storageItemPrefabDataIndexesFound = new List<int>();

	// Token: 0x020020F4 RID: 8436
	[DebuggerDisplay("{item.item.Name}")]
	public class StatusItemEntry : IRenderEveryTick
	{
		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x0600BAE3 RID: 47843 RVA: 0x003FC120 File Offset: 0x003FA320
		public Image GetImage
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x0600BAE4 RID: 47844 RVA: 0x003FC128 File Offset: 0x003FA328
		public StatusItemEntry(StatusItemGroup.Entry item, StatusItemCategory category, GameObject status_item_prefab, Transform parent, TextStyleSetting tooltip_style, Color color, TextStyleSetting style, bool skip_fade, Action<SimpleInfoScreen.StatusItemEntry> onDestroy)
		{
			this.item = item;
			this.category = category;
			this.tooltipStyle = tooltip_style;
			this.onDestroy = onDestroy;
			this.color = color;
			this.style = style;
			this.widget = global::Util.KInstantiateUI(status_item_prefab, parent.gameObject, false);
			this.text = this.widget.GetComponentInChildren<LocText>(true);
			SetTextStyleSetting.ApplyStyle(this.text, style);
			this.toolTip = this.widget.GetComponentInChildren<ToolTip>(true);
			this.image = this.widget.GetComponentInChildren<Image>(true);
			item.SetIcon(this.image);
			this.widget.SetActive(true);
			this.toolTip.OnToolTip = new Func<string>(this.OnToolTip);
			this.button = this.widget.GetComponentInChildren<KButton>();
			if (item.item.statusItemClickCallback != null)
			{
				this.button.onClick += this.OnClick;
			}
			else
			{
				this.button.enabled = false;
			}
			this.fadeStage = (skip_fade ? SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT : SimpleInfoScreen.StatusItemEntry.FadeStage.IN);
			SimAndRenderScheduler.instance.Add(this, false);
			this.Refresh();
			this.SetColor(1f);
		}

		// Token: 0x0600BAE5 RID: 47845 RVA: 0x003FC269 File Offset: 0x003FA469
		internal void SetSprite(TintedSprite sprite)
		{
			if (sprite != null)
			{
				this.image.sprite = sprite.sprite;
			}
		}

		// Token: 0x0600BAE6 RID: 47846 RVA: 0x003FC27F File Offset: 0x003FA47F
		public int GetIndex()
		{
			return this.widget.transform.GetSiblingIndex();
		}

		// Token: 0x0600BAE7 RID: 47847 RVA: 0x003FC291 File Offset: 0x003FA491
		public void SetIndex(int index)
		{
			this.widget.transform.SetSiblingIndex(index);
		}

		// Token: 0x0600BAE8 RID: 47848 RVA: 0x003FC2A4 File Offset: 0x003FA4A4
		public void RenderEveryTick(float dt)
		{
			switch (this.fadeStage)
			{
			case SimpleInfoScreen.StatusItemEntry.FadeStage.IN:
			{
				this.fade = Mathf.Min(this.fade + Time.deltaTime / this.fadeInTime, 1f);
				float num = this.fade;
				this.SetColor(num);
				if (this.fade >= 1f)
				{
					this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT;
					return;
				}
				break;
			}
			case SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT:
				break;
			case SimpleInfoScreen.StatusItemEntry.FadeStage.OUT:
			{
				float num2 = this.fade;
				this.SetColor(num2);
				this.fade = Mathf.Max(this.fade - Time.deltaTime / this.fadeOutTime, 0f);
				if (this.fade <= 0f)
				{
					this.Destroy(true);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600BAE9 RID: 47849 RVA: 0x003FC356 File Offset: 0x003FA556
		private string OnToolTip()
		{
			this.item.ShowToolTip(this.toolTip, this.tooltipStyle);
			return "";
		}

		// Token: 0x0600BAEA RID: 47850 RVA: 0x003FC374 File Offset: 0x003FA574
		private void OnClick()
		{
			this.item.OnClick();
		}

		// Token: 0x0600BAEB RID: 47851 RVA: 0x003FC384 File Offset: 0x003FA584
		public void Refresh()
		{
			string name = this.item.GetName();
			if (name != this.text.text)
			{
				this.text.text = name;
				this.SetColor(1f);
			}
		}

		// Token: 0x0600BAEC RID: 47852 RVA: 0x003FC3C8 File Offset: 0x003FA5C8
		private void SetColor(float alpha = 1f)
		{
			Color color = new Color(this.color.r, this.color.g, this.color.b, alpha);
			this.image.color = color;
			this.text.color = color;
		}

		// Token: 0x0600BAED RID: 47853 RVA: 0x003FC418 File Offset: 0x003FA618
		public void Destroy(bool immediate)
		{
			if (this.toolTip != null)
			{
				this.toolTip.OnToolTip = null;
			}
			if (this.button != null && this.button.enabled)
			{
				this.button.onClick -= this.OnClick;
			}
			if (immediate)
			{
				if (this.onDestroy != null)
				{
					this.onDestroy(this);
				}
				SimAndRenderScheduler.instance.Remove(this);
				UnityEngine.Object.Destroy(this.widget);
				return;
			}
			this.fade = 0.5f;
			this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.OUT;
		}

		// Token: 0x040097A2 RID: 38818
		public StatusItemGroup.Entry item;

		// Token: 0x040097A3 RID: 38819
		public StatusItemCategory category;

		// Token: 0x040097A4 RID: 38820
		public Color color;

		// Token: 0x040097A5 RID: 38821
		public TextStyleSetting style;

		// Token: 0x040097A6 RID: 38822
		public Action<SimpleInfoScreen.StatusItemEntry> onDestroy;

		// Token: 0x040097A7 RID: 38823
		private LayoutElement spacerLayout;

		// Token: 0x040097A8 RID: 38824
		private GameObject widget;

		// Token: 0x040097A9 RID: 38825
		private ToolTip toolTip;

		// Token: 0x040097AA RID: 38826
		private TextStyleSetting tooltipStyle;

		// Token: 0x040097AB RID: 38827
		private Image image;

		// Token: 0x040097AC RID: 38828
		private LocText text;

		// Token: 0x040097AD RID: 38829
		private KButton button;

		// Token: 0x040097AE RID: 38830
		private SimpleInfoScreen.StatusItemEntry.FadeStage fadeStage;

		// Token: 0x040097AF RID: 38831
		private float fade;

		// Token: 0x040097B0 RID: 38832
		private float fadeInTime;

		// Token: 0x040097B1 RID: 38833
		private float fadeOutTime = 1.8f;

		// Token: 0x02002A87 RID: 10887
		private enum FadeStage
		{
			// Token: 0x0400BB98 RID: 48024
			IN,
			// Token: 0x0400BB99 RID: 48025
			WAIT,
			// Token: 0x0400BB9A RID: 48026
			OUT
		}
	}

	// Token: 0x020020F5 RID: 8437
	private class StorageCollapsibleRowData
	{
		// Token: 0x040097B2 RID: 38834
		public int prefabHashCode;

		// Token: 0x040097B3 RID: 38835
		public IStorage[] storages;
	}

	// Token: 0x020020F6 RID: 8438
	private class StoredItemCategoryData
	{
		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x0600BAEF RID: 47855 RVA: 0x003FC4B9 File Offset: 0x003FA6B9
		public bool usingUnits
		{
			get
			{
				return this.massPerUnit > 1f;
			}
		}

		// Token: 0x0600BAF0 RID: 47856 RVA: 0x003FC4C8 File Offset: 0x003FA6C8
		public StoredItemCategoryData(string name, float m, float massPerUnit)
		{
			this.temperatureRanges = new Vector2(float.MaxValue, float.MinValue);
			this.name = name;
			this.mass = m;
			this.massPerUnit = massPerUnit;
		}

		// Token: 0x0600BAF1 RID: 47857 RVA: 0x003FC4FC File Offset: 0x003FA6FC
		public void ClearData()
		{
			this.name = null;
			this.mass = 0f;
			this.massPerUnit = 1f;
			this.temperatureRanges = new Vector2(float.MaxValue, float.MinValue);
			this.instancesFound = 0;
			this.lastInstance = null;
			this.lastPEInstance = null;
		}

		// Token: 0x040097B4 RID: 38836
		public float mass;

		// Token: 0x040097B5 RID: 38837
		public float massPerUnit;

		// Token: 0x040097B6 RID: 38838
		public string name;

		// Token: 0x040097B7 RID: 38839
		public Vector2 temperatureRanges;

		// Token: 0x040097B8 RID: 38840
		public int instancesFound;

		// Token: 0x040097B9 RID: 38841
		public KPrefabID lastInstance;

		// Token: 0x040097BA RID: 38842
		public PrimaryElement lastPEInstance;
	}
}
