using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02000971 RID: 2417
public class GeothermalPlantComponent : KMonoBehaviour, ICheckboxListGroupControl, IRelatedEntities
{
	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x060044E0 RID: 17632 RVA: 0x0018D4D0 File Offset: 0x0018B6D0
	string ICheckboxListGroupControl.Title
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_TITLE;
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x060044E1 RID: 17633 RVA: 0x0018D4DC File Offset: 0x0018B6DC
	string ICheckboxListGroupControl.Description
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_DESC;
		}
	}

	// Token: 0x060044E2 RID: 17634 RVA: 0x0018D4E8 File Offset: 0x0018B6E8
	public ICheckboxListGroupControl.ListGroup[] GetData()
	{
		ColonyAchievement activateGeothermalPlant = Db.Get().ColonyAchievements.ActivateGeothermalPlant;
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[activateGeothermalPlant.requirementChecklist.Count];
		for (int i = 0; i < array.Length; i++)
		{
			ICheckboxListGroupControl.CheckboxItem checkboxItem = default(ICheckboxListGroupControl.CheckboxItem);
			bool flag = activateGeothermalPlant.requirementChecklist[i].Success();
			checkboxItem.isOn = flag;
			checkboxItem.text = (activateGeothermalPlant.requirementChecklist[i] as VictoryColonyAchievementRequirement).Name();
			checkboxItem.tooltip = activateGeothermalPlant.requirementChecklist[i].GetProgress(flag);
			array[i] = checkboxItem;
		}
		return new ICheckboxListGroupControl.ListGroup[]
		{
			new ICheckboxListGroupControl.ListGroup(activateGeothermalPlant.Name, array, null, null)
		};
	}

	// Token: 0x060044E3 RID: 17635 RVA: 0x0018D5A2 File Offset: 0x0018B7A2
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060044E4 RID: 17636 RVA: 0x0018D5A5 File Offset: 0x0018B7A5
	public int CheckboxSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x060044E5 RID: 17637 RVA: 0x0018D5A9 File Offset: 0x0018B7A9
	public static bool GeothermalControllerRepaired()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x0018D5BA File Offset: 0x0018B7BA
	public static bool GeothermalFacilityDiscovered()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered;
	}

	// Token: 0x060044E7 RID: 17639 RVA: 0x0018D5CB File Offset: 0x0018B7CB
	protected override void OnSpawn()
	{
		base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
	}

	// Token: 0x060044E8 RID: 17640 RVA: 0x0018D5E5 File Offset: 0x0018B7E5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060044E9 RID: 17641 RVA: 0x0018D5F0 File Offset: 0x0018B7F0
	public static void DisplayPopup(string title, string desc, HashedString anim, System.Action onDismissCallback, Transform clickFocus = null)
	{
		EventInfoData eventInfoData = new EventInfoData(title, desc, anim);
		if (Components.LiveMinionIdentities.Count >= 2)
		{
			int num = UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count);
			int num2 = UnityEngine.Random.Range(1, Components.LiveMinionIdentities.Count);
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[num].gameObject,
				Components.LiveMinionIdentities[(num + num2) % Components.LiveMinionIdentities.Count].gameObject
			};
		}
		else if (Components.LiveMinionIdentities.Count == 1)
		{
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[0].gameObject
			};
		}
		eventInfoData.AddDefaultOption(onDismissCallback);
		eventInfoData.clickFocus = clickFocus;
		EventInfoScreen.ShowPopup(eventInfoData);
	}

	// Token: 0x060044EA RID: 17642 RVA: 0x0018D6BC File Offset: 0x0018B8BC
	protected void RevealAllVentsAndController()
	{
		foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalVentEntity"
		}))
		{
			int baseX;
			int num;
			Grid.CellToXY(spawnable.cell, out baseX, out num);
			GridVisibility.Reveal(baseX, num + 2, 5, 5f);
		}
		foreach (WorldGenSpawner.Spawnable spawnable2 in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalControllerEntity"
		}))
		{
			int baseX2;
			int num2;
			Grid.CellToXY(spawnable2.cell, out baseX2, out num2);
			GridVisibility.Reveal(baseX2, num2 + 3, 7, 7f);
		}
		SelectTool.Instance.Select(null, true);
	}

	// Token: 0x060044EB RID: 17643 RVA: 0x0018D7CC File Offset: 0x0018B9CC
	protected void OnObjectSelect(object _)
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
		if (SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered)
		{
			return;
		}
		SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered = true;
		GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISCOVERED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISOCVERED_DESC, "geothermalplantintro_kanim", new System.Action(this.RevealAllVentsAndController), null);
	}

	// Token: 0x060044EC RID: 17644 RVA: 0x0018D844 File Offset: 0x0018BA44
	public static void OnVentingHotMaterial(int worldid)
	{
		foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(worldid))
		{
			if (geothermalVent.IsQuestEntombed())
			{
				geothermalVent.SetQuestComplete();
				if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent)
				{
					GeothermalVictorySequence.VictoryVent = geothermalVent;
					GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_DESC, "geothermalplantachievement_kanim", delegate
					{
						SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent = true;
					}, null);
					break;
				}
			}
		}
	}

	// Token: 0x060044ED RID: 17645 RVA: 0x0018D900 File Offset: 0x0018BB00
	public List<KSelectable> GetRelatedEntities()
	{
		List<KSelectable> list = new List<KSelectable>();
		int myWorldId = this.GetMyWorldId();
		foreach (GeothermalController geothermalController in Components.GeothermalControllers.GetItems(myWorldId))
		{
			list.Add(geothermalController.GetComponent<KSelectable>());
		}
		foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(myWorldId))
		{
			list.Add(geothermalVent.GetComponent<KSelectable>());
		}
		return list;
	}

	// Token: 0x04002E37 RID: 11831
	public const string POPUP_DISCOVERED_KANIM = "geothermalplantintro_kanim";

	// Token: 0x04002E38 RID: 11832
	public const string POPUP_PROGRESS_KANIM = "geothermalplantonline_kanim";

	// Token: 0x04002E39 RID: 11833
	public const string POPUP_COMPLETE_KANIM = "geothermalplantachievement_kanim";
}
