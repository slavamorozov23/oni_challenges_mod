using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E97 RID: 3735
public class SkillsScreen : KModalScreen
{
	// Token: 0x0600775B RID: 30555 RVA: 0x002DA918 File Offset: 0x002D8B18
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x1700083E RID: 2110
	// (get) Token: 0x0600775C RID: 30556 RVA: 0x002DA92D File Offset: 0x002D8B2D
	// (set) Token: 0x0600775D RID: 30557 RVA: 0x002DA94C File Offset: 0x002D8B4C
	public IAssignableIdentity CurrentlySelectedMinion
	{
		get
		{
			if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
			{
				return null;
			}
			return this.currentlySelectedMinion;
		}
		set
		{
			this.currentlySelectedMinion = value;
			if (base.IsActive())
			{
				this.RefreshSelectedMinion();
				this.RefreshSkillWidgets();
				this.RefreshBoosters();
			}
		}
	}

	// Token: 0x0600775E RID: 30558 RVA: 0x002DA96F File Offset: 0x002D8B6F
	protected override void OnSpawn()
	{
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.WorldRemoved));
	}

	// Token: 0x0600775F RID: 30559 RVA: 0x002DA990 File Offset: 0x002D8B90
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = true;
		base.OnActivate();
		this.BuildMinions();
		this.RefreshAll();
		this.SortRows((this.active_sort_method == null) ? this.compareByMinion : this.active_sort_method);
		Components.LiveMinionIdentities.OnAdd += this.OnAddMinionIdentity;
		Components.LiveMinionIdentities.OnRemove += this.OnRemoveMinionIdentity;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		MultiToggle multiToggle = this.dupeSortingToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SortRows(this.compareByMinion);
		}));
		MultiToggle multiToggle2 = this.moraleSortingToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SortRows(this.compareByMorale);
		}));
		MultiToggle multiToggle3 = this.experienceSortingToggle;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			this.SortRows(this.compareByExperience);
		}));
	}

	// Token: 0x06007760 RID: 30560 RVA: 0x002DAAA0 File Offset: 0x002D8CA0
	protected override void OnShow(bool show)
	{
		if (show)
		{
			if (this.CurrentlySelectedMinion == null && Components.LiveMinionIdentities.Count > 0)
			{
				this.CurrentlySelectedMinion = Components.LiveMinionIdentities.Items[0];
			}
			this.BuildMinions();
			if (this.boosterWidgets.Count == 0)
			{
				this.PopulateBoosters();
			}
			this.RefreshAll();
			this.SortRows((this.active_sort_method == null) ? this.compareByMinion : this.active_sort_method);
		}
		base.OnShow(show);
	}

	// Token: 0x06007761 RID: 30561 RVA: 0x002DAB1D File Offset: 0x002D8D1D
	public void RefreshAll()
	{
		this.dirty = false;
		this.RefreshSkillWidgets();
		this.RefreshSelectedMinion();
		this.RefreshBoosters();
		this.linesPending = true;
	}

	// Token: 0x06007762 RID: 30562 RVA: 0x002DAB3F File Offset: 0x002D8D3F
	private void RefreshSelectedMinion()
	{
		this.minionAnimWidget.SetPortraitAnimator(this.currentlySelectedMinion);
		this.RefreshProgressBars();
		this.RefreshHat();
	}

	// Token: 0x06007763 RID: 30563 RVA: 0x002DAB5E File Offset: 0x002D8D5E
	public void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
	{
		if (assignableIdentity is MinionAssignablesProxy)
		{
			minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
			storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			return;
		}
		minionIdentity = (assignableIdentity as MinionIdentity);
		storedMinionIdentity = (assignableIdentity as StoredMinionIdentity);
	}

	// Token: 0x06007764 RID: 30564 RVA: 0x002DABA0 File Offset: 0x002D8DA0
	private void RefreshProgressBars()
	{
		if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
		{
			return;
		}
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		HierarchyReferences component = this.expectationsTooltip.GetComponent<HierarchyReferences>();
		component.GetReference("Labels").gameObject.SetActive(minionIdentity != null);
		component.GetReference("MoraleBar").gameObject.SetActive(minionIdentity != null);
		component.GetReference("ExpectationBar").gameObject.SetActive(minionIdentity != null);
		component.GetReference("StoredMinion").gameObject.SetActive(minionIdentity == null);
		this.experienceProgressFill.gameObject.SetActive(minionIdentity != null);
		if (minionIdentity == null)
		{
			this.expectationsTooltip.SetSimpleTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), this.currentlySelectedMinion.GetProperName()));
			this.experienceBarTooltip.SetSimpleTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), this.currentlySelectedMinion.GetProperName()));
			this.EXPCount.text = "";
			this.duplicantLevelIndicator.text = UI.TABLESCREENS.NA;
			return;
		}
		MinionResume component2 = minionIdentity.GetComponent<MinionResume>();
		float num = MinionResume.CalculatePreviousExperienceBar(component2.TotalSkillPointsGained);
		float num2 = MinionResume.CalculateNextExperienceBar(component2.TotalSkillPointsGained);
		float fillAmount = (component2.TotalExperienceGained - num) / (num2 - num);
		this.EXPCount.text = Mathf.RoundToInt(component2.TotalExperienceGained - num).ToString() + " / " + Mathf.RoundToInt(num2 - num).ToString();
		this.duplicantLevelIndicator.text = component2.AvailableSkillpoints.ToString();
		this.experienceProgressFill.fillAmount = fillAmount;
		this.experienceBarTooltip.SetSimpleTooltip(string.Format(UI.SKILLS_SCREEN.EXPERIENCE_TOOLTIP, Mathf.RoundToInt(num2 - num) - Mathf.RoundToInt(component2.TotalExperienceGained - num)));
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component2);
		AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component2);
		float num3 = 0f;
		float num4 = 0f;
		if (!string.IsNullOrEmpty(this.hoveredSkillID) && !component2.HasMasteredSkill(this.hoveredSkillID))
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			list.Add(this.hoveredSkillID);
			while (list.Count > 0)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (!component2.HasMasteredSkill(list[i]))
					{
						num3 += (float)(Db.Get().Skills.Get(list[i]).tier + 1);
						if (component2.AptitudeBySkillGroup.ContainsKey(Db.Get().Skills.Get(list[i]).skillGroup) && component2.AptitudeBySkillGroup[Db.Get().Skills.Get(list[i]).skillGroup] > 0f)
						{
							num4 += 1f;
						}
						foreach (string item in Db.Get().Skills.Get(list[i]).priorSkills)
						{
							list2.Add(item);
						}
					}
				}
				list.Clear();
				list.AddRange(list2);
				list2.Clear();
			}
		}
		float num5 = attributeInstance.GetTotalValue() + num4 / (attributeInstance2.GetTotalValue() + num3);
		float f = Mathf.Max(attributeInstance.GetTotalValue() + num4, attributeInstance2.GetTotalValue() + num3);
		while (this.moraleNotches.Count < Mathf.RoundToInt(f))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.moraleNotch, this.moraleNotch.transform.parent);
			gameObject.SetActive(true);
			this.moraleNotches.Add(gameObject);
		}
		while (this.moraleNotches.Count > Mathf.RoundToInt(f))
		{
			GameObject gameObject2 = this.moraleNotches[this.moraleNotches.Count - 1];
			this.moraleNotches.Remove(gameObject2);
			UnityEngine.Object.Destroy(gameObject2);
		}
		for (int j = 0; j < this.moraleNotches.Count; j++)
		{
			if ((float)j < attributeInstance.GetTotalValue() + num4)
			{
				this.moraleNotches[j].GetComponentsInChildren<Image>()[1].color = this.moraleNotchColor;
			}
			else
			{
				this.moraleNotches[j].GetComponentsInChildren<Image>()[1].color = Color.clear;
			}
		}
		this.moraleProgressLabel.text = UI.SKILLS_SCREEN.MORALE + ": " + attributeInstance.GetTotalValue().ToString();
		if (num4 > 0f)
		{
			LocText locText = this.moraleProgressLabel;
			locText.text = locText.text + " + " + GameUtil.ApplyBoldString(GameUtil.ColourizeString(this.moraleNotchColor, num4.ToString()));
		}
		while (this.expectationNotches.Count < Mathf.RoundToInt(f))
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.expectationNotch, this.expectationNotch.transform.parent);
			gameObject3.SetActive(true);
			this.expectationNotches.Add(gameObject3);
		}
		while (this.expectationNotches.Count > Mathf.RoundToInt(f))
		{
			GameObject gameObject4 = this.expectationNotches[this.expectationNotches.Count - 1];
			this.expectationNotches.Remove(gameObject4);
			UnityEngine.Object.Destroy(gameObject4);
		}
		for (int k = 0; k < this.expectationNotches.Count; k++)
		{
			if ((float)k < attributeInstance2.GetTotalValue() + num3)
			{
				if ((float)k < attributeInstance2.GetTotalValue())
				{
					this.expectationNotches[k].GetComponentsInChildren<Image>()[1].color = this.expectationNotchColor;
				}
				else
				{
					this.expectationNotches[k].GetComponentsInChildren<Image>()[1].color = this.expectationNotchProspectColor;
				}
			}
			else
			{
				this.expectationNotches[k].GetComponentsInChildren<Image>()[1].color = Color.clear;
			}
		}
		this.expectationsProgressLabel.text = UI.SKILLS_SCREEN.MORALE_EXPECTATION + ": " + attributeInstance2.GetTotalValue().ToString();
		if (num3 > 0f)
		{
			LocText locText2 = this.expectationsProgressLabel;
			locText2.text = locText2.text + " + " + GameUtil.ApplyBoldString(GameUtil.ColourizeString(this.expectationNotchColor, num3.ToString()));
		}
		if (num5 < 1f)
		{
			this.expectationWarning.SetActive(true);
			this.moraleWarning.SetActive(false);
		}
		else
		{
			this.expectationWarning.SetActive(false);
			this.moraleWarning.SetActive(true);
		}
		string text = "";
		List<global::Tuple<string, float>> list3 = new List<global::Tuple<string, float>>();
		text = string.Concat(new string[]
		{
			text,
			GameUtil.ApplyBoldString(UI.SKILLS_SCREEN.MORALE),
			": ",
			attributeInstance.GetTotalValue().ToString(),
			"\n"
		});
		for (int l = 0; l < attributeInstance.Modifiers.Count; l++)
		{
			list3.Add(new global::Tuple<string, float>(attributeInstance.Modifiers[l].GetDescription(), attributeInstance.Modifiers[l].Value));
		}
		List<global::Tuple<string, float>> list4 = list3.ToList<global::Tuple<string, float>>();
		list4.Sort((global::Tuple<string, float> pair1, global::Tuple<string, float> pair2) => pair2.second.CompareTo(pair1.second));
		foreach (global::Tuple<string, float> tuple in list4)
		{
			text = string.Concat(new string[]
			{
				text,
				"    • ",
				tuple.first,
				": ",
				(tuple.second > 0f) ? UIConstants.ColorPrefixGreen : UIConstants.ColorPrefixRed,
				tuple.second.ToString(),
				UIConstants.ColorSuffix,
				"\n"
			});
		}
		text += "\n";
		text = string.Concat(new string[]
		{
			text,
			GameUtil.ApplyBoldString(UI.SKILLS_SCREEN.MORALE_EXPECTATION),
			": ",
			attributeInstance2.GetTotalValue().ToString(),
			"\n"
		});
		for (int m = 0; m < attributeInstance2.Modifiers.Count; m++)
		{
			text = string.Concat(new string[]
			{
				text,
				"    • ",
				attributeInstance2.Modifiers[m].GetDescription(),
				": ",
				(attributeInstance2.Modifiers[m].Value > 0f) ? UIConstants.ColorPrefixRed : UIConstants.ColorPrefixGreen,
				attributeInstance2.Modifiers[m].GetFormattedString(),
				UIConstants.ColorSuffix,
				"\n"
			});
		}
		this.expectationsTooltip.SetSimpleTooltip(text);
	}

	// Token: 0x06007765 RID: 30565 RVA: 0x002DB558 File Offset: 0x002D9758
	private Tag SelectedMinionModel()
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			return Db.Get().Personalities.Get(minionIdentity.personalityResourceId).model;
		}
		if (storedMinionIdentity != null)
		{
			return Db.Get().Personalities.Get(storedMinionIdentity.personalityResourceId).model;
		}
		return null;
	}

	// Token: 0x06007766 RID: 30566 RVA: 0x002DB5C4 File Offset: 0x002D97C4
	private void RefreshHat()
	{
		if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
		{
			return;
		}
		List<IListableOption> list = new List<IListableOption>();
		string text = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			text = (string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat);
			foreach (MinionResume.HatInfo hatInfo in component.GetAllHats())
			{
				list.Add(new HatListable(hatInfo.Source, hatInfo.Hat));
			}
			this.hatDropDown.Initialize(list, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, this.currentlySelectedMinion);
		}
		else
		{
			text = (string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat);
		}
		this.hatDropDown.openButton.enabled = (minionIdentity != null);
		this.selectedHat.transform.Find("Arrow").gameObject.SetActive(minionIdentity != null);
		this.selectedHat.sprite = Assets.GetSprite(string.IsNullOrEmpty(text) ? "hat_role_none" : text);
	}

	// Token: 0x06007767 RID: 30567 RVA: 0x002DB748 File Offset: 0x002D9948
	private void OnHatDropEntryClick(IListableOption skill, object data)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity == null)
		{
			return;
		}
		MinionResume component = minionIdentity.GetComponent<MinionResume>();
		string text = "hat_role_none";
		if (skill != null)
		{
			this.selectedHat.sprite = Assets.GetSprite((skill as HatListable).hat);
			if (component != null)
			{
				text = (skill as HatListable).hat;
				component.SetHats(component.CurrentHat, text);
				if (component.OwnsHat(text))
				{
					new PutOnHatChore(component, Db.Get().ChoreTypes.SwitchHat);
				}
			}
		}
		else
		{
			this.selectedHat.sprite = Assets.GetSprite(text);
			if (component != null)
			{
				component.SetHats(component.CurrentHat, null);
				component.ApplyTargetHat();
			}
		}
		IAssignableIdentity assignableIdentity = minionIdentity.assignableProxy.Get();
		foreach (SkillMinionWidget skillMinionWidget in this.sortableRows)
		{
			if (skillMinionWidget.assignableIdentity == assignableIdentity)
			{
				skillMinionWidget.RefreshHat(component.TargetHat);
			}
		}
	}

	// Token: 0x06007768 RID: 30568 RVA: 0x002DB87C File Offset: 0x002D9A7C
	private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			HatListable hatListable = entry.entryData as HatListable;
			entry.image.sprite = Assets.GetSprite(hatListable.hat);
		}
	}

	// Token: 0x06007769 RID: 30569 RVA: 0x002DB8B8 File Offset: 0x002D9AB8
	private int hatDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x0600776A RID: 30570 RVA: 0x002DB8BC File Offset: 0x002D9ABC
	private void Update()
	{
		if (this.dirty)
		{
			this.RefreshAll();
		}
		if (this.linesPending)
		{
			foreach (GameObject gameObject in this.skillWidgets.Values)
			{
				gameObject.GetComponent<SkillWidget>().RefreshLines();
			}
			this.linesPending = false;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			this.scrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
		}
	}

	// Token: 0x0600776B RID: 30571 RVA: 0x002DB95C File Offset: 0x002D9B5C
	private void PopulateBoosters()
	{
		foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.BionicUpgrade))
		{
			Tag id = gameObject.GetComponent<KPrefabID>().PrefabID();
			GameObject gameObject2 = Util.KInstantiate(this.boosterPrefab, this.boosterContentGrid, gameObject.name);
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.SetActive(true);
			HierarchyReferences component = gameObject2.GetComponent<HierarchyReferences>();
			this.boosterWidgets.Add(gameObject.PrefabID(), component);
			component.GetReference<Image>("Icon").sprite = Def.GetUISprite(gameObject, "ui", false).first;
			gameObject2.GetComponentInChildren<LocText>().SetText(gameObject.GetProperName());
			KButton reference = component.GetReference<KButton>("AssignmentIncrementButton");
			reference.ClearOnClick();
			reference.onClick += delegate()
			{
				this.IncrementBoosterAssignment(id);
			};
			KButton reference2 = component.GetReference<KButton>("AssignmentDecrementButton");
			reference2.ClearOnClick();
			reference2.onClick += delegate()
			{
				this.DecrementBoosterAssignment(id);
			};
			foreach (GameObject original in this.boosterSlotIcons)
			{
				Util.KDestroyGameObject(original);
			}
			this.boosterSlotIcons.Clear();
			for (int i = 0; i < 8; i++)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.boosterSlotIconPrefab, this.boosterSlotIconPrefab.transform.parent.gameObject, false);
				this.boosterSlotIcons.Add(gameObject3);
				int slotIdx = i;
				gameObject3.transform.GetChild(0).GetComponent<MultiToggle>().onClick = delegate()
				{
					MinionIdentity minionIdentity;
					StoredMinionIdentity storedMinionIdentity;
					this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
					if (minionIdentity == null)
					{
						return;
					}
					minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots[slotIdx].GetAssignableSlotInstance().Unassign(true);
					this.RefreshBoosters();
				};
			}
		}
	}

	// Token: 0x0600776C RID: 30572 RVA: 0x002DBB6C File Offset: 0x002D9D6C
	private void IncrementBoosterAssignment(Tag boosterType)
	{
		BionicUpgradeComponent bionicUpgradeComponent = this.FindAvailableBoosterOfType(boosterType);
		if (bionicUpgradeComponent != null)
		{
			bionicUpgradeComponent.Assign(this.CurrentlySelectedMinion);
		}
		this.RefreshBoosters();
	}

	// Token: 0x0600776D RID: 30573 RVA: 0x002DBB9C File Offset: 0x002D9D9C
	private void DecrementBoosterAssignment(Tag boosterType)
	{
		MinionIdentity cmp;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out cmp, out storedMinionIdentity);
		BionicUpgradesMonitor.Instance smi = cmp.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			bool flag = false;
			for (int i = smi.upgradeComponentSlots.Length - 1; i >= 0; i--)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = smi.upgradeComponentSlots[i];
				if (upgradeComponentSlot.assignedUpgradeComponent != null && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == boosterType && upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.AssignedUpgradeMatchesInstalledUpgrade)
				{
					upgradeComponentSlot.GetAssignableSlotInstance().Unassign(true);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = smi.upgradeComponentSlots.Length - 1; j >= 0; j--)
				{
					BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot2 = smi.upgradeComponentSlots[j];
					if (upgradeComponentSlot2.assignedUpgradeComponent != null && upgradeComponentSlot2.assignedUpgradeComponent.PrefabID() == boosterType)
					{
						upgradeComponentSlot2.GetAssignableSlotInstance().Unassign(true);
						break;
					}
				}
			}
		}
		this.RefreshBoosters();
	}

	// Token: 0x0600776E RID: 30574 RVA: 0x002DBC94 File Offset: 0x002D9E94
	private BionicUpgradeComponent FindAvailableBoosterOfType(Tag boosterType)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity == null)
		{
			return null;
		}
		List<Pickupable> list = ClusterManager.Instance.GetWorld(minionIdentity.GetMyWorldId()).worldInventory.CreatePickupablesList(boosterType);
		if (list == null || list.Count == 0)
		{
			return null;
		}
		list = list.FindAll((Pickupable match) => match.GetComponent<BionicUpgradeComponent>().assignee == null);
		if (list == null || list.Count == 0)
		{
			return null;
		}
		using (List<Pickupable>.Enumerator enumerator = list.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current.GetComponent<BionicUpgradeComponent>();
			}
		}
		return null;
	}

	// Token: 0x0600776F RID: 30575 RVA: 0x002DBD60 File Offset: 0x002D9F60
	private void RefreshBoosters()
	{
		BionicUpgradesMonitor.Instance instance = null;
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		bool flag = this.SelectedMinionModel() == GameTags.Minions.Models.Bionic && minionIdentity != null;
		if (flag)
		{
			instance = minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>();
			if (instance == null)
			{
				flag = false;
			}
		}
		if (flag)
		{
			this.equippedBoostersHeaderLabel.SetText(GameUtil.SafeStringFormat(UI.SKILLS_SCREEN.ASSIGNED_BOOSTERS_HEADER, new object[]
			{
				this.CurrentlySelectedMinion.GetProperName()
			}));
			this.assignedBoostersCountLabel.SetText(GameUtil.SafeStringFormat(UI.SKILLS_SCREEN.ASSIGNED_BOOSTERS_COUNT_LABEL, new object[]
			{
				instance.AssignedSlotCount,
				instance.UnlockedSlotCount
			}));
			this.boosterPanel.SetActive(true);
			this.boosterHeader.SetActive(true);
			float canvasScale = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale();
			float num = (float)Screen.height / canvasScale * 0.4f;
			float num2 = 96f;
			this.skillsContainer.rectTransform().sizeDelta = new Vector2(0f, -1f * (num + num2));
			this.boosterPanel.rectTransform().sizeDelta = new Vector2(0f, num);
			this.boosterHeader.rectTransform().anchoredPosition = new Vector2(0f, num);
			for (int i = 0; i < this.boosterSlotIcons.Count; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = instance.upgradeComponentSlots[7 - i];
				this.boosterSlotIcons[i].SetActive(true);
				if (i >= instance.upgradeComponentSlots.Length || instance.upgradeComponentSlots[i].IsLocked)
				{
					this.boosterSlotIcons[i].GetComponent<Image>().sprite = Assets.GetSprite("bionicUpgradeSlotLocked");
					this.boosterSlotIcons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
					this.boosterSlotIcons[i].GetComponent<ToolTip>().SetSimpleTooltip(UI.SKILLS_SCREEN.BIONIC_UPGRADE_SLOT_LOCKED);
					this.boosterSlotIcons[i].transform.GetChild(0).gameObject.SetActive(false);
				}
				else if (instance.upgradeComponentSlots[i].assignedUpgradeComponent != null)
				{
					this.boosterSlotIcons[i].GetComponent<Image>().sprite = Def.GetUISprite(instance.upgradeComponentSlots[i].assignedUpgradeComponent.PrefabID(), "ui", false).first;
					this.boosterSlotIcons[i].GetComponent<ToolTip>().SetSimpleTooltip(instance.upgradeComponentSlots[i].assignedUpgradeComponent.GetProperName() + "\n\n" + UI.SKILLS_SCREEN.BIONIC_UPGRADE_SLOT_UNASSIGN);
					this.boosterSlotIcons[i].GetComponent<Image>().color = Color.white;
					this.boosterSlotIcons[i].transform.GetChild(0).gameObject.SetActive(true);
				}
				else
				{
					this.boosterSlotIcons[i].GetComponent<Image>().sprite = Assets.GetSprite("bionicUpgradeSlot");
					this.boosterSlotIcons[i].GetComponent<ToolTip>().SetSimpleTooltip(UI.SKILLS_SCREEN.BIONIC_UPGRADE_SLOT_AVAILABLE);
					this.boosterSlotIcons[i].GetComponent<Image>().color = Color.white;
					this.boosterSlotIcons[i].transform.GetChild(0).gameObject.SetActive(false);
				}
			}
			using (Dictionary<Tag, HierarchyReferences>.Enumerator enumerator = this.boosterWidgets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Tag, HierarchyReferences> widget = enumerator.Current;
					int num3 = 0;
					if (instance != null && instance.upgradeComponentSlots != null)
					{
						foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot2 in instance.upgradeComponentSlots)
						{
							if (upgradeComponentSlot2.assignedUpgradeComponent != null && upgradeComponentSlot2.assignedUpgradeComponent.PrefabID() == widget.Key)
							{
								num3++;
							}
						}
					}
					GameObject prefab = Assets.GetPrefab(widget.Key);
					TMP_Text reference = widget.Value.GetReference<LocText>("Label");
					string properName = prefab.GetProperName();
					reference.SetText(properName);
					float num4 = 0f;
					List<Pickupable> list = ClusterManager.Instance.GetWorld(minionIdentity.GetMyWorldId()).worldInventory.CreatePickupablesList(widget.Key);
					if (list != null && list.Count > 0)
					{
						list = list.FindAll((Pickupable match) => match.GetComponent<Assignable>().assignee == null);
						num4 = (float)list.Count;
					}
					if (num4 > 0f)
					{
						widget.Value.GetReference<Image>("Icon").material = GlobalResources.Instance().AnimUIMaterial;
						widget.Value.GetReference<Image>("Icon").color = new Color(1f, 1f, 1f, 1f);
					}
					else
					{
						widget.Value.GetReference<Image>("Icon").material = GlobalResources.Instance().AnimMaterialUIDesaturated;
						widget.Value.GetReference<Image>("Icon").color = new Color(1f, 1f, 1f, 0.5f);
					}
					string text = GameUtil.SafeStringFormat(UI.SKILLS_SCREEN.AVAILABLE_BOOSTERS_LABEL, new object[]
					{
						num4.ToString()
					});
					LocText reference2 = widget.Value.GetReference<LocText>("AvailableLabel");
					reference2.SetText(text);
					reference2.color = ((num4 > 0f) ? new Color(0.53f, 0.83f, 0.53f) : new Color(0.65f, 0.65f, 0.65f));
					string text2 = GameUtil.SafeStringFormat(UI.SKILLS_SCREEN.ASSIGNED_BOOSTERS_LABEL, new object[]
					{
						num3
					});
					widget.Value.GetReference<LocText>("EquipCountLabel").SetText(text2);
					widget.Value.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(string.Concat(new string[]
					{
						"<b>",
						prefab.GetProperName(),
						"</b>\n\n",
						BionicUpgradeComponentConfig.UpgradesData[widget.Key].stateMachineDescription,
						"\n\n",
						BionicUpgradeComponentConfig.GetColonyBoosterAssignmentString(widget.Key.Name)
					}));
					bool flag2 = instance.AssignedSlotCount < instance.UnlockedSlotCount;
					bool flag3 = num4 > 0f;
					MultiToggle component = widget.Value.gameObject.GetComponent<MultiToggle>();
					component.onClick = null;
					if (flag3 && flag2)
					{
						MultiToggle multiToggle = component;
						multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
						{
							this.IncrementBoosterAssignment(widget.Key);
						}));
					}
				}
				return;
			}
		}
		this.boosterPanel.SetActive(false);
		this.boosterHeader.SetActive(false);
		this.skillsContainer.rectTransform().sizeDelta = new Vector2(0f, 0f);
	}

	// Token: 0x06007770 RID: 30576 RVA: 0x002DC518 File Offset: 0x002DA718
	private void RefreshSkillWidgets()
	{
		int num = 1;
		foreach (SkillGroup skillGroup in Db.Get().SkillGroups.resources)
		{
			List<Skill> skillsBySkillGroup = this.GetSkillsBySkillGroup(skillGroup.Id);
			if (skillsBySkillGroup.Count > 0)
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				for (int i = 0; i < skillsBySkillGroup.Count; i++)
				{
					Skill skill = skillsBySkillGroup[i];
					if (!skill.deprecated && Game.IsCorrectDlcActiveForCurrentSave(skill))
					{
						if (!this.skillWidgets.ContainsKey(skill.Id))
						{
							while (skill.tier >= this.skillColumns.Count)
							{
								GameObject gameObject = Util.KInstantiateUI(this.Prefab_skillColumn, this.Prefab_tableLayout, true);
								this.skillColumns.Add(gameObject);
								HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
								if (this.skillColumns.Count % 2 == 0)
								{
									component.GetReference("BG").gameObject.SetActive(false);
								}
							}
							int num2 = 0;
							dictionary.TryGetValue(skill.tier, out num2);
							dictionary[skill.tier] = num2 + 1;
							GameObject value = Util.KInstantiateUI(this.Prefab_skillWidget, this.skillColumns[skill.tier], true);
							this.skillWidgets.Add(skill.Id, value);
						}
						this.skillWidgets[skill.Id].GetComponent<SkillWidget>().Refresh(skill.Id);
					}
				}
				if (!this.skillGroupRow.ContainsKey(skillGroup.Id))
				{
					int num3 = 1;
					foreach (KeyValuePair<int, int> keyValuePair in dictionary)
					{
						num3 = Mathf.Max(num3, keyValuePair.Value);
					}
					this.skillGroupRow.Add(skillGroup.Id, num);
					num += num3;
				}
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.skillWidgets)
		{
			if (Db.Get().Skills.Get(keyValuePair2.Key).requiredDuplicantModel != null)
			{
				keyValuePair2.Value.SetActive(Db.Get().Skills.Get(keyValuePair2.Key).requiredDuplicantModel == this.SelectedMinionModel());
			}
		}
		foreach (SkillMinionWidget skillMinionWidget in this.sortableRows)
		{
			skillMinionWidget.Refresh();
		}
		this.RefreshWidgetPositions();
	}

	// Token: 0x06007771 RID: 30577 RVA: 0x002DC84C File Offset: 0x002DAA4C
	public void HoverSkill(string skillID)
	{
		this.hoveredSkillID = skillID;
		if (this.delayRefreshRoutine != null)
		{
			base.StopCoroutine(this.delayRefreshRoutine);
			this.delayRefreshRoutine = null;
		}
		if (string.IsNullOrEmpty(this.hoveredSkillID))
		{
			this.delayRefreshRoutine = base.StartCoroutine(this.DelayRefreshProgressBars());
			return;
		}
		this.RefreshProgressBars();
	}

	// Token: 0x06007772 RID: 30578 RVA: 0x002DC8A1 File Offset: 0x002DAAA1
	private IEnumerator DelayRefreshProgressBars()
	{
		yield return SequenceUtil.WaitForSecondsRealtime(0.1f);
		this.RefreshProgressBars();
		yield break;
	}

	// Token: 0x06007773 RID: 30579 RVA: 0x002DC8B0 File Offset: 0x002DAAB0
	public void RefreshWidgetPositions()
	{
		float num = 0f;
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.skillWidgets)
		{
			if (!(Db.Get().Skills.Get(keyValuePair.Key).requiredDuplicantModel != this.SelectedMinionModel()))
			{
				float rowPosition = this.GetRowPosition(keyValuePair.Key);
				num = Mathf.Max(rowPosition, num);
				keyValuePair.Value.rectTransform().anchoredPosition = Vector2.down * rowPosition;
			}
		}
		num = Mathf.Max(num, (float)this.layoutRowHeight);
		float num2 = (float)this.layoutRowHeight;
		foreach (GameObject gameObject in this.skillColumns)
		{
			gameObject.GetComponent<LayoutElement>().minHeight = num + num2;
		}
		this.linesPending = true;
	}

	// Token: 0x06007774 RID: 30580 RVA: 0x002DC9CC File Offset: 0x002DABCC
	public float GetRowPosition(string skillID)
	{
		Skill skill = Db.Get().Skills.Get(skillID);
		int num = this.skillGroupRow[skill.skillGroup];
		int num2 = num;
		foreach (KeyValuePair<string, int> keyValuePair in this.skillGroupRow)
		{
			if (keyValuePair.Value <= num && this.SelectedMinionModel() != this.GetSkillsBySkillGroup(keyValuePair.Key)[0].requiredDuplicantModel)
			{
				num2--;
			}
		}
		num = num2;
		List<Skill> skillsBySkillGroup = this.GetSkillsBySkillGroup(skill.skillGroup);
		int num3 = 0;
		foreach (Skill skill2 in skillsBySkillGroup)
		{
			if (skill2 == skill)
			{
				break;
			}
			if (skill2.tier == skill.tier)
			{
				num3++;
			}
		}
		return (float)(this.layoutRowHeight * (num3 + num - 1));
	}

	// Token: 0x06007775 RID: 30581 RVA: 0x002DCAE8 File Offset: 0x002DACE8
	private void OnAddMinionIdentity(MinionIdentity add)
	{
		this.BuildMinions();
		this.RefreshAll();
	}

	// Token: 0x06007776 RID: 30582 RVA: 0x002DCAF8 File Offset: 0x002DACF8
	private void OnRemoveMinionIdentity(MinionIdentity remove)
	{
		if (remove != null)
		{
			if (this.CurrentlySelectedMinion == remove)
			{
				this.CurrentlySelectedMinion = null;
			}
			if (remove.assignableProxy.Get() == this.CurrentlySelectedMinion)
			{
				this.CurrentlySelectedMinion = null;
			}
		}
		this.BuildMinions();
		this.RefreshAll();
	}

	// Token: 0x06007777 RID: 30583 RVA: 0x002DCB48 File Offset: 0x002DAD48
	private void BuildMinions()
	{
		for (int i = this.sortableRows.Count - 1; i >= 0; i--)
		{
			this.sortableRows[i].DeleteObject();
		}
		this.sortableRows.Clear();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			GameObject gameObject = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
			gameObject.GetComponent<SkillMinionWidget>().SetMinon(minionIdentity.assignableProxy.Get());
			this.sortableRows.Add(gameObject.GetComponent<SkillMinionWidget>());
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity storedMinionIdentity = info.serializedMinion.Get<StoredMinionIdentity>();
					GameObject gameObject2 = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
					gameObject2.GetComponent<SkillMinionWidget>().SetMinon(storedMinionIdentity.assignableProxy.Get());
					this.sortableRows.Add(gameObject2.GetComponent<SkillMinionWidget>());
				}
			}
		}
		foreach (int num in ClusterManager.Instance.GetWorldIDsSorted())
		{
			if (ClusterManager.Instance.GetWorld(num).IsDiscovered)
			{
				this.AddWorldDivider(num);
			}
		}
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
		{
			keyValuePair.Value.SetActive(ClusterManager.Instance.GetWorld(keyValuePair.Key).IsDiscovered && DlcManager.FeatureClusterSpaceEnabled());
			Component reference = keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference("NobodyRow");
			reference.gameObject.SetActive(true);
			using (IEnumerator enumerator6 = Components.MinionAssignablesProxy.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					if (((MinionAssignablesProxy)enumerator6.Current).GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld().id == keyValuePair.Key)
					{
						reference.gameObject.SetActive(false);
						break;
					}
				}
			}
		}
		if (this.CurrentlySelectedMinion == null && Components.LiveMinionIdentities.Count > 0)
		{
			this.CurrentlySelectedMinion = Components.LiveMinionIdentities.Items[0];
		}
	}

	// Token: 0x06007778 RID: 30584 RVA: 0x002DCE6C File Offset: 0x002DB06C
	protected void AddWorldDivider(int worldId)
	{
		if (!this.worldDividers.ContainsKey(worldId))
		{
			GameObject gameObject = Util.KInstantiateUI(this.Prefab_worldDivider, this.Prefab_minionLayout, true);
			gameObject.GetComponentInChildren<Image>().color = ClusterManager.worldColors[worldId % ClusterManager.worldColors.Length];
			ClusterGridEntity component = ClusterManager.Instance.GetWorld(worldId).GetComponent<ClusterGridEntity>();
			gameObject.GetComponentInChildren<LocText>().SetText(component.Name);
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = component.GetUISprite();
			this.worldDividers.Add(worldId, gameObject);
		}
	}

	// Token: 0x06007779 RID: 30585 RVA: 0x002DCF04 File Offset: 0x002DB104
	private void WorldRemoved(object worldId)
	{
		int value = ((Boxed<int>)worldId).value;
		GameObject obj;
		if (this.worldDividers.TryGetValue(value, out obj))
		{
			UnityEngine.Object.Destroy(obj);
			this.worldDividers.Remove(value);
		}
	}

	// Token: 0x0600777A RID: 30586 RVA: 0x002DCF40 File Offset: 0x002DB140
	public Vector2 GetSkillWidgetLineTargetPosition(string skillID)
	{
		return this.skillWidgets[skillID].GetComponent<SkillWidget>().lines_right.GetPosition();
	}

	// Token: 0x0600777B RID: 30587 RVA: 0x002DCF62 File Offset: 0x002DB162
	public SkillWidget GetSkillWidget(string skill)
	{
		return this.skillWidgets[skill].GetComponent<SkillWidget>();
	}

	// Token: 0x0600777C RID: 30588 RVA: 0x002DCF78 File Offset: 0x002DB178
	public List<Skill> GetSkillsBySkillGroup(string skillGrp)
	{
		List<Skill> list = new List<Skill>();
		foreach (Skill skill in Db.Get().Skills.resources)
		{
			if (skill.skillGroup == skillGrp && !skill.deprecated)
			{
				list.Add(skill);
			}
		}
		return list;
	}

	// Token: 0x0600777D RID: 30589 RVA: 0x002DCFF4 File Offset: 0x002DB1F4
	private void SelectSortToggle(MultiToggle toggle)
	{
		this.dupeSortingToggle.ChangeState(0);
		this.experienceSortingToggle.ChangeState(0);
		this.moraleSortingToggle.ChangeState(0);
		if (toggle != null)
		{
			if (this.activeSortToggle == toggle)
			{
				this.sortReversed = !this.sortReversed;
			}
			this.activeSortToggle = toggle;
		}
		this.activeSortToggle.ChangeState(this.sortReversed ? 2 : 1);
	}

	// Token: 0x0600777E RID: 30590 RVA: 0x002DD06C File Offset: 0x002DB26C
	private void SortRows(Comparison<IAssignableIdentity> comparison)
	{
		this.active_sort_method = comparison;
		Dictionary<IAssignableIdentity, SkillMinionWidget> dictionary = new Dictionary<IAssignableIdentity, SkillMinionWidget>();
		foreach (SkillMinionWidget skillMinionWidget in this.sortableRows)
		{
			dictionary.Add(skillMinionWidget.assignableIdentity, skillMinionWidget);
		}
		Dictionary<int, List<IAssignableIdentity>> minionsByWorld = ClusterManager.Instance.MinionsByWorld;
		this.sortableRows.Clear();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<int, List<IAssignableIdentity>> keyValuePair in minionsByWorld)
		{
			dictionary2.Add(keyValuePair.Key, num);
			num++;
			List<IAssignableIdentity> list = new List<IAssignableIdentity>();
			foreach (IAssignableIdentity item in keyValuePair.Value)
			{
				list.Add(item);
			}
			if (comparison != null)
			{
				list.Sort(comparison);
				if (this.sortReversed)
				{
					list.Reverse();
				}
			}
			num += list.Count;
			num2 += list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				IAssignableIdentity key = list[i];
				SkillMinionWidget item2 = dictionary[key];
				this.sortableRows.Add(item2);
			}
		}
		for (int j = 0; j < this.sortableRows.Count; j++)
		{
			this.sortableRows[j].gameObject.transform.SetSiblingIndex(j);
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in dictionary2)
		{
			this.worldDividers[keyValuePair2.Key].transform.SetSiblingIndex(keyValuePair2.Value);
		}
	}

	// Token: 0x040052CD RID: 21197
	[SerializeField]
	private KButton CloseButton;

	// Token: 0x040052CE RID: 21198
	[Header("Prefabs")]
	[SerializeField]
	private GameObject Prefab_skillWidget;

	// Token: 0x040052CF RID: 21199
	[SerializeField]
	private GameObject Prefab_skillColumn;

	// Token: 0x040052D0 RID: 21200
	[SerializeField]
	private GameObject Prefab_minion;

	// Token: 0x040052D1 RID: 21201
	[SerializeField]
	private GameObject Prefab_minionLayout;

	// Token: 0x040052D2 RID: 21202
	[SerializeField]
	private GameObject Prefab_tableLayout;

	// Token: 0x040052D3 RID: 21203
	[SerializeField]
	private GameObject Prefab_worldDivider;

	// Token: 0x040052D4 RID: 21204
	[Header("Sort Toggles")]
	[SerializeField]
	private MultiToggle dupeSortingToggle;

	// Token: 0x040052D5 RID: 21205
	[SerializeField]
	private MultiToggle experienceSortingToggle;

	// Token: 0x040052D6 RID: 21206
	[SerializeField]
	private MultiToggle moraleSortingToggle;

	// Token: 0x040052D7 RID: 21207
	private MultiToggle activeSortToggle;

	// Token: 0x040052D8 RID: 21208
	private bool sortReversed;

	// Token: 0x040052D9 RID: 21209
	private Comparison<IAssignableIdentity> active_sort_method;

	// Token: 0x040052DA RID: 21210
	[Header("Duplicant Animation")]
	[SerializeField]
	private FullBodyUIMinionWidget minionAnimWidget;

	// Token: 0x040052DB RID: 21211
	[Header("Progress Bars")]
	[SerializeField]
	private ToolTip expectationsTooltip;

	// Token: 0x040052DC RID: 21212
	[SerializeField]
	private LocText moraleProgressLabel;

	// Token: 0x040052DD RID: 21213
	[SerializeField]
	private GameObject moraleWarning;

	// Token: 0x040052DE RID: 21214
	[SerializeField]
	private GameObject moraleNotch;

	// Token: 0x040052DF RID: 21215
	[SerializeField]
	private Color moraleNotchColor;

	// Token: 0x040052E0 RID: 21216
	private List<GameObject> moraleNotches = new List<GameObject>();

	// Token: 0x040052E1 RID: 21217
	[SerializeField]
	private LocText expectationsProgressLabel;

	// Token: 0x040052E2 RID: 21218
	[SerializeField]
	private GameObject expectationWarning;

	// Token: 0x040052E3 RID: 21219
	[SerializeField]
	private GameObject expectationNotch;

	// Token: 0x040052E4 RID: 21220
	[SerializeField]
	private Color expectationNotchColor;

	// Token: 0x040052E5 RID: 21221
	[SerializeField]
	private Color expectationNotchProspectColor;

	// Token: 0x040052E6 RID: 21222
	private List<GameObject> expectationNotches = new List<GameObject>();

	// Token: 0x040052E7 RID: 21223
	[SerializeField]
	private ToolTip experienceBarTooltip;

	// Token: 0x040052E8 RID: 21224
	[SerializeField]
	private Image experienceProgressFill;

	// Token: 0x040052E9 RID: 21225
	[SerializeField]
	private LocText EXPCount;

	// Token: 0x040052EA RID: 21226
	[SerializeField]
	private LocText duplicantLevelIndicator;

	// Token: 0x040052EB RID: 21227
	[SerializeField]
	private KScrollRect scrollRect;

	// Token: 0x040052EC RID: 21228
	[SerializeField]
	private float scrollSpeed = 7f;

	// Token: 0x040052ED RID: 21229
	[SerializeField]
	private DropDown hatDropDown;

	// Token: 0x040052EE RID: 21230
	[SerializeField]
	public Image selectedHat;

	// Token: 0x040052EF RID: 21231
	[SerializeField]
	private GameObject skillsContainer;

	// Token: 0x040052F0 RID: 21232
	[SerializeField]
	private GameObject boosterPanel;

	// Token: 0x040052F1 RID: 21233
	[SerializeField]
	private GameObject boosterHeader;

	// Token: 0x040052F2 RID: 21234
	[SerializeField]
	private GameObject boosterContentGrid;

	// Token: 0x040052F3 RID: 21235
	[SerializeField]
	private GameObject boosterPrefab;

	// Token: 0x040052F4 RID: 21236
	private Dictionary<Tag, HierarchyReferences> boosterWidgets = new Dictionary<Tag, HierarchyReferences>();

	// Token: 0x040052F5 RID: 21237
	[SerializeField]
	private LocText equippedBoostersHeaderLabel;

	// Token: 0x040052F6 RID: 21238
	[SerializeField]
	private LocText assignedBoostersCountLabel;

	// Token: 0x040052F7 RID: 21239
	[SerializeField]
	private GameObject boosterSlotIconPrefab;

	// Token: 0x040052F8 RID: 21240
	private List<GameObject> boosterSlotIcons = new List<GameObject>();

	// Token: 0x040052F9 RID: 21241
	private IAssignableIdentity currentlySelectedMinion;

	// Token: 0x040052FA RID: 21242
	private List<GameObject> rows = new List<GameObject>();

	// Token: 0x040052FB RID: 21243
	private List<SkillMinionWidget> sortableRows = new List<SkillMinionWidget>();

	// Token: 0x040052FC RID: 21244
	private Dictionary<int, GameObject> worldDividers = new Dictionary<int, GameObject>();

	// Token: 0x040052FD RID: 21245
	private string hoveredSkillID = "";

	// Token: 0x040052FE RID: 21246
	private Dictionary<string, GameObject> skillWidgets = new Dictionary<string, GameObject>();

	// Token: 0x040052FF RID: 21247
	private Dictionary<string, int> skillGroupRow = new Dictionary<string, int>();

	// Token: 0x04005300 RID: 21248
	private List<GameObject> skillColumns = new List<GameObject>();

	// Token: 0x04005301 RID: 21249
	private bool dirty;

	// Token: 0x04005302 RID: 21250
	private bool linesPending;

	// Token: 0x04005303 RID: 21251
	private int layoutRowHeight = 80;

	// Token: 0x04005304 RID: 21252
	private Coroutine delayRefreshRoutine;

	// Token: 0x04005305 RID: 21253
	protected Comparison<IAssignableIdentity> compareByExperience = delegate(IAssignableIdentity a, IAssignableIdentity b)
	{
		GameObject targetGameObject = ((MinionAssignablesProxy)a).GetTargetGameObject();
		GameObject targetGameObject2 = ((MinionAssignablesProxy)b).GetTargetGameObject();
		if (targetGameObject == null && targetGameObject2 == null)
		{
			return 0;
		}
		if (targetGameObject == null)
		{
			return -1;
		}
		if (targetGameObject2 == null)
		{
			return 1;
		}
		MinionResume component = targetGameObject.GetComponent<MinionResume>();
		MinionResume component2 = targetGameObject2.GetComponent<MinionResume>();
		if (component == null && component2 == null)
		{
			return 0;
		}
		if (component == null)
		{
			return -1;
		}
		if (component2 == null)
		{
			return 1;
		}
		float num = (float)component.AvailableSkillpoints;
		float value = (float)component2.AvailableSkillpoints;
		return num.CompareTo(value);
	};

	// Token: 0x04005306 RID: 21254
	protected Comparison<IAssignableIdentity> compareByMinion = (IAssignableIdentity a, IAssignableIdentity b) => a.GetProperName().CompareTo(b.GetProperName());

	// Token: 0x04005307 RID: 21255
	protected Comparison<IAssignableIdentity> compareByMorale = delegate(IAssignableIdentity a, IAssignableIdentity b)
	{
		GameObject targetGameObject = ((MinionAssignablesProxy)a).GetTargetGameObject();
		GameObject targetGameObject2 = ((MinionAssignablesProxy)b).GetTargetGameObject();
		if (targetGameObject == null && targetGameObject2 == null)
		{
			return 0;
		}
		if (targetGameObject == null)
		{
			return -1;
		}
		if (targetGameObject2 == null)
		{
			return 1;
		}
		MinionResume component = targetGameObject.GetComponent<MinionResume>();
		MinionResume component2 = targetGameObject2.GetComponent<MinionResume>();
		if (component == null && component2 == null)
		{
			return 0;
		}
		if (component == null)
		{
			return -1;
		}
		if (component2 == null)
		{
			return 1;
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component);
		Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component);
		AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLife.Lookup(component2);
		Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component2);
		float totalValue = attributeInstance.GetTotalValue();
		float totalValue2 = attributeInstance2.GetTotalValue();
		return totalValue.CompareTo(totalValue2);
	};
}
