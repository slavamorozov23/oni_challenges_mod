using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF1 RID: 3569
public class ResearchScreenSideBar : KScreen
{
	// Token: 0x0600707D RID: 28797 RVA: 0x002ACB41 File Offset: 0x002AAD41
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.raycaster = this.projectsContainer.GetComponent<GraphicRaycaster>();
	}

	// Token: 0x0600707E RID: 28798 RVA: 0x002ACB5C File Offset: 0x002AAD5C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.PopulateProjects();
		this.PopulateFilterButtons();
		this.RefreshCategoriesContentExpanded();
		this.RefreshWidgets();
		this.searchBox.OnValueChangesPaused = delegate()
		{
			this.SetTextFilter(this.searchBox.text, false);
		};
		KInputTextField kinputTextField = this.searchBox;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchBox.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.clearSearchButton.onClick += delegate()
		{
			this.ResetFilter();
		};
		this.ConfigCompletionFilters();
		base.ConsumeMouseScroll = true;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.UpdateProjectFilter));
	}

	// Token: 0x0600707F RID: 28799 RVA: 0x002ACC24 File Offset: 0x002AAE24
	private void Update()
	{
		for (int i = 0; i < Math.Min(this.QueuedActivations.Count, this.activationPerFrame); i++)
		{
			this.QueuedActivations[i].SetActive(true);
		}
		this.QueuedActivations.RemoveRange(0, Math.Min(this.QueuedActivations.Count, this.activationPerFrame));
		for (int j = 0; j < Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame); j++)
		{
			this.QueuedDeactivations[j].SetActive(false);
		}
		this.QueuedDeactivations.RemoveRange(0, Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame));
	}

	// Token: 0x06007080 RID: 28800 RVA: 0x002ACCDB File Offset: 0x002AAEDB
	public override bool IsScreenActive()
	{
		return this.researchScreen.IsScreenActive();
	}

	// Token: 0x06007081 RID: 28801 RVA: 0x002ACCE8 File Offset: 0x002AAEE8
	private void ConfigCompletionFilters()
	{
		MultiToggle multiToggle = this.allFilter;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All, false);
		}));
		MultiToggle multiToggle2 = this.completedFilter;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Completed, false);
		}));
		MultiToggle multiToggle3 = this.availableFilter;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Available, false);
		}));
		this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All, false);
	}

	// Token: 0x06007082 RID: 28802 RVA: 0x002ACD74 File Offset: 0x002AAF74
	private void SetCompletionFilter(ResearchScreenSideBar.CompletionState state, bool suppressUpdate)
	{
		this.completionFilter = state;
		this.allFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.All) ? 1 : 0);
		this.completedFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.Completed) ? 1 : 0);
		this.availableFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.Available) ? 1 : 0);
		if (!suppressUpdate)
		{
			this.UpdateProjectFilter(null);
		}
	}

	// Token: 0x06007083 RID: 28803 RVA: 0x002ACDE8 File Offset: 0x002AAFE8
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 21f;
	}

	// Token: 0x06007084 RID: 28804 RVA: 0x002ACE00 File Offset: 0x002AB000
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.researchScreen != null && this.researchScreen.canvas && !this.researchScreen.canvas.enabled)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		if (!e.Consumed)
		{
			Vector2 vector = base.transform.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (vector.x >= 0f && vector.x <= base.transform.rectTransform().rect.width)
			{
				if (e.TryConsume(global::Action.MouseRight))
				{
					return;
				}
				if (e.TryConsume(global::Action.MouseLeft))
				{
					return;
				}
				if (!KInputManager.currentControllerIsGamepad)
				{
					if (e.TryConsume(global::Action.ZoomIn))
					{
						return;
					}
					e.TryConsume(global::Action.ZoomOut);
					return;
				}
			}
		}
	}

	// Token: 0x06007085 RID: 28805 RVA: 0x002ACECE File Offset: 0x002AB0CE
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.raycaster.enabled = show;
		this.RefreshWidgets();
	}

	// Token: 0x06007086 RID: 28806 RVA: 0x002ACEE9 File Offset: 0x002AB0E9
	public override void Show(bool show = true)
	{
		this.mouseOver = false;
		this.OnShow(show);
	}

	// Token: 0x06007087 RID: 28807 RVA: 0x002ACEFC File Offset: 0x002AB0FC
	private void SetTextFilter(string newValue, bool suppressUpdate)
	{
		if (base.isEditing)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
			{
				this.filterStates[keyValuePair.Key] = false;
				keyValuePair.Value.GetComponent<MultiToggle>().ChangeState(0);
			}
		}
		bool flag = this.IsTextFilterActive();
		this.currentSearchString = newValue;
		this.currentSearchStringUpper = this.currentSearchString.ToUpper().Trim();
		if (this.IsTextFilterActive())
		{
			Transform reference = this.projectCategories["SearchResults"].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
			using (Dictionary<string, GameObject>.Enumerator enumerator = this.projectTechs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, GameObject> keyValuePair2 = enumerator.Current;
					this.projectTechs[keyValuePair2.Key].transform.SetParent(reference);
				}
				goto IL_183;
			}
		}
		if (flag)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.projectTechs)
			{
				Transform reference2 = this.projectCategories[Db.Get().Techs.Get(keyValuePair3.Key).category].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
				this.projectTechs[keyValuePair3.Key].transform.SetParent(reference2);
			}
		}
		IL_183:
		if (!suppressUpdate)
		{
			this.UpdateProjectFilter(null);
		}
	}

	// Token: 0x06007088 RID: 28808 RVA: 0x002AD0C0 File Offset: 0x002AB2C0
	private void UpdateProjectFilter(object data = null)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectCategories)
		{
			dictionary.Add(keyValuePair.Key, false);
		}
		bool flag = this.IsTextFilterActive();
		if (flag)
		{
			dictionary["SearchResults"] = true;
			this.categoryExpanded["SearchResults"] = true;
		}
		this.RefreshProjectsActive();
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.projectTechs)
		{
			if ((keyValuePair2.Value.activeSelf || this.QueuedActivations.Contains(keyValuePair2.Value)) && !this.QueuedDeactivations.Contains(keyValuePair2.Value))
			{
				dictionary[Db.Get().Techs.Get(keyValuePair2.Key).category] = !flag;
				this.categoryExpanded[Db.Get().Techs.Get(keyValuePair2.Key).category] = true;
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair3 in dictionary)
		{
			this.ChangeGameObjectActive(this.projectCategories[keyValuePair3.Key], keyValuePair3.Value);
		}
		this.RefreshCategoriesContentExpanded();
		foreach (GameObject gameObject in this.orderedTechs)
		{
			gameObject.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06007089 RID: 28809 RVA: 0x002AD2B4 File Offset: 0x002AB4B4
	private int CompareTechScores(global::Tuple<GameObject, string> a, global::Tuple<GameObject, string> b)
	{
		int techMatchScore = this.GetTechMatchScore(a.second);
		int techMatchScore2 = this.GetTechMatchScore(b.second);
		int num = -techMatchScore.CompareTo(techMatchScore2);
		if (num != 0)
		{
			return num;
		}
		if (!this.IsTextFilterActive())
		{
			return num;
		}
		return this.techCaches[a.second].CompareTo(this.techCaches[b.second]);
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x0600708A RID: 28810 RVA: 0x002AD31B File Offset: 0x002AB51B
	private Comparer<global::Tuple<GameObject, string>> TechWidgetComparer
	{
		get
		{
			if (this.techWidgetComparer == null)
			{
				this.techWidgetComparer = Comparer<global::Tuple<GameObject, string>>.Create(new Comparison<global::Tuple<GameObject, string>>(this.CompareTechScores));
			}
			return this.techWidgetComparer;
		}
	}

	// Token: 0x0600708B RID: 28811 RVA: 0x002AD344 File Offset: 0x002AB544
	private void RefreshProjectsActive()
	{
		if (this.projectTechItems.Count == 0)
		{
			return;
		}
		Techs techs = Db.Get().Techs;
		if (this.techCaches == null)
		{
			this.techCaches = SearchUtil.CacheTechs();
		}
		if (this.IsTextFilterActive())
		{
			using (Dictionary<string, SearchUtil.TechCache>.Enumerator enumerator = this.techCaches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, SearchUtil.TechCache> keyValuePair = enumerator.Current;
					try
					{
						keyValuePair.Value.Bind(this.currentSearchStringUpper);
					}
					catch (Exception ex)
					{
						KCrashReporter.ReportDevNotification("Fuzzy score bind failed", Environment.StackTrace, ex.Message, false, null);
						keyValuePair.Value.Reset();
					}
				}
				goto IL_DC;
			}
		}
		foreach (KeyValuePair<string, SearchUtil.TechCache> keyValuePair2 in this.techCaches)
		{
			keyValuePair2.Value.Reset();
		}
		IL_DC:
		for (int num = 0; num != techs.Count; num++)
		{
			Tech tech = (Tech)techs.GetResource(num);
			SearchUtil.TechCache techCache = this.techCaches[tech.Id];
			foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.projectTechItems[tech.Id])
			{
				bool flag = SearchUtil.IsPassingScore(this.GetTechItemMatchScore(techCache, keyValuePair3.Key));
				HierarchyReferences component = keyValuePair3.Value.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").color = (flag ? Color.white : Color.grey);
				component.GetReference<Image>("Icon").color = (flag ? Color.white : new Color(1f, 1f, 1f, 0.5f));
			}
		}
		ListPool<global::Tuple<int, int>, ResearchScreen>.PooledList pooledList = ListPool<global::Tuple<int, int>, ResearchScreen>.Allocate();
		for (int num2 = 0; num2 != techs.Count; num2++)
		{
			Tech tech2 = (Tech)techs.GetResource(num2);
			pooledList.Add(new global::Tuple<int, int>(num2, tech2.tier));
		}
		pooledList.Sort((global::Tuple<int, int> a, global::Tuple<int, int> b) => a.second.CompareTo(b.second));
		ListPool<global::Tuple<GameObject, string>, ResearchScreenSideBar>.PooledList pooledList2 = ListPool<global::Tuple<GameObject, string>, ResearchScreenSideBar>.Allocate();
		foreach (global::Tuple<int, int> tuple in pooledList)
		{
			Tech tech3 = (Tech)techs.GetResource(tuple.first);
			GameObject gameObject = this.projectTechs[tech3.Id];
			bool flag2 = SearchUtil.IsPassingScore(this.GetTechMatchScore(tech3.Id));
			this.ChangeGameObjectActive(gameObject, flag2);
			this.researchScreen.GetEntry(tech3).UpdateFilterState(flag2);
			if (flag2)
			{
				global::Tuple<GameObject, string> tuple2 = new global::Tuple<GameObject, string>(gameObject, tech3.Id);
				int num3 = pooledList2.BinarySearch(tuple2, this.TechWidgetComparer);
				if (num3 < 0)
				{
					num3 = ~num3;
				}
				while (num3 < pooledList2.Count && this.CompareTechScores(tuple2, pooledList2[num3]) == 0)
				{
					num3++;
				}
				pooledList2.Insert(num3, tuple2);
			}
		}
		pooledList.Recycle();
		this.orderedTechs.Clear();
		foreach (global::Tuple<GameObject, string> tuple3 in pooledList2)
		{
			this.orderedTechs.Add(tuple3.first);
		}
		pooledList2.Recycle();
	}

	// Token: 0x0600708C RID: 28812 RVA: 0x002AD714 File Offset: 0x002AB914
	private void RefreshCategoriesContentExpanded()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectCategories)
		{
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject.SetActive(this.categoryExpanded[keyValuePair.Key]);
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.categoryExpanded[keyValuePair.Key] ? 1 : 0);
		}
	}

	// Token: 0x0600708D RID: 28813 RVA: 0x002AD7C8 File Offset: 0x002AB9C8
	private void CreateCategory(string categoryID, string title = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.techCategoryPrefabAlt, this.projectsContainer, true);
		gameObject.name = categoryID;
		if (title == null)
		{
			title = Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + categoryID.ToUpper());
		}
		gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(title);
		this.categoryExpanded.Add(categoryID, false);
		this.projectCategories.Add(categoryID, gameObject);
		gameObject.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
		{
			this.categoryExpanded[categoryID] = !this.categoryExpanded[categoryID];
			this.RefreshCategoriesContentExpanded();
		};
	}

	// Token: 0x0600708E RID: 28814 RVA: 0x002AD888 File Offset: 0x002ABA88
	private void PopulateProjects()
	{
		ListPool<global::Tuple<global::Tuple<string, GameObject>, int>, ResearchScreen>.PooledList pooledList = ListPool<global::Tuple<global::Tuple<string, GameObject>, int>, ResearchScreen>.Allocate();
		for (int i = 0; i < Db.Get().Techs.Count; i++)
		{
			Tech tech = (Tech)Db.Get().Techs.GetResource(i);
			if (!this.projectCategories.ContainsKey(tech.category))
			{
				this.CreateCategory(tech.category, null);
			}
			GameObject gameObject = this.SpawnTechWidget(tech.Id, this.projectCategories[tech.category]);
			pooledList.Add(new global::Tuple<global::Tuple<string, GameObject>, int>(new global::Tuple<string, GameObject>(tech.Id, gameObject), tech.tier));
			this.projectTechs.Add(tech.Id, gameObject);
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(tech.desc);
			MultiToggle component = gameObject.GetComponent<MultiToggle>();
			component.onEnter = (System.Action)Delegate.Combine(component.onEnter, new System.Action(delegate()
			{
				this.researchScreen.TurnEverythingOff();
				this.researchScreen.GetEntry(tech).OnHover(true, tech);
				this.soundPlayer.Play(1);
			}));
			MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
			component2.onExit = (System.Action)Delegate.Combine(component2.onExit, new System.Action(delegate()
			{
				this.researchScreen.TurnEverythingOff();
			}));
		}
		this.CreateCategory("SearchResults", UI.RESEARCHSCREEN.SEARCH_RESULTS_CATEGORY);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectTechs)
		{
			Transform reference = this.projectCategories[Db.Get().Techs.Get(keyValuePair.Key).category].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
			this.projectTechs[keyValuePair.Key].transform.SetParent(reference);
		}
		pooledList.Sort((global::Tuple<global::Tuple<string, GameObject>, int> a, global::Tuple<global::Tuple<string, GameObject>, int> b) => a.second.CompareTo(b.second));
		foreach (global::Tuple<global::Tuple<string, GameObject>, int> tuple in pooledList)
		{
			tuple.first.second.transform.SetAsLastSibling();
		}
		pooledList.Recycle();
	}

	// Token: 0x0600708F RID: 28815 RVA: 0x002ADAF8 File Offset: 0x002ABCF8
	private void PopulateFilterButtons()
	{
		using (Dictionary<string, List<Tag>>.Enumerator enumerator = this.filterPresets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, List<Tag>> kvp = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.filterButtonPrefab, this.searchFiltersContainer, true);
				this.filterButtons.Add(kvp.Key, gameObject);
				this.filterStates.Add(kvp.Key, false);
				MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<LocText>();
				StringEntry text = Strings.Get("STRINGS.UI.RESEARCHSCREEN.FILTER_BUTTONS." + kvp.Key.ToUpper());
				componentInChildren.SetText(text);
				MultiToggle toggle2 = toggle;
				toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
				{
					foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
					{
						if (keyValuePair.Key != kvp.Key)
						{
							this.filterStates[keyValuePair.Key] = false;
							this.filterButtons[keyValuePair.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[keyValuePair.Key] ? 1 : 0);
						}
					}
					this.filterStates[kvp.Key] = !this.filterStates[kvp.Key];
					toggle.ChangeState(this.filterStates[kvp.Key] ? 1 : 0);
					this.searchBox.text = (this.filterStates[kvp.Key] ? text.String : "");
				}));
			}
		}
	}

	// Token: 0x06007090 RID: 28816 RVA: 0x002ADC0C File Offset: 0x002ABE0C
	public void RefreshQueue()
	{
	}

	// Token: 0x06007091 RID: 28817 RVA: 0x002ADC10 File Offset: 0x002ABE10
	private void RefreshWidgets()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		using (Dictionary<string, GameObject>.Enumerator enumerator = this.projectTechs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, GameObject> kvp = enumerator.Current;
				if (Db.Get().Techs.Get(kvp.Key).IsComplete())
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(2);
				}
				else if (researchQueue.Find((TechInstance match) => match.tech.Id == kvp.Key) != null)
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(1);
				}
				else
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(0);
				}
			}
		}
	}

	// Token: 0x06007092 RID: 28818 RVA: 0x002ADCF4 File Offset: 0x002ABEF4
	private void RefreshWidgetProgressBars(string techID, GameObject widget)
	{
		HierarchyReferences component = widget.GetComponent<HierarchyReferences>();
		ResearchPointInventory progressInventory = Research.Instance.GetTechInstance(techID).progressInventory;
		int num = 0;
		for (int i = 0; i < Research.Instance.researchTypes.Types.Count; i++)
		{
			if (Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID.ContainsKey(Research.Instance.researchTypes.Types[i].id) && Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id] > 0f)
			{
				HierarchyReferences component2 = component.GetReference<RectTransform>("BarRows").GetChild(1 + num).GetComponent<HierarchyReferences>();
				float num2 = progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[i].id] / Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id];
				RectTransform rectTransform = component2.GetReference<Image>("Bar").rectTransform;
				rectTransform.sizeDelta = new Vector2(rectTransform.parent.rectTransform().rect.width * num2, rectTransform.sizeDelta.y);
				component2.GetReference<LocText>("Label").SetText(progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[i].id].ToString() + "/" + Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id].ToString());
				num++;
			}
		}
	}

	// Token: 0x06007093 RID: 28819 RVA: 0x002ADEFC File Offset: 0x002AC0FC
	private GameObject SpawnTechWidget(string techID, GameObject parentContainer)
	{
		GameObject gameObject = Util.KInstantiateUI(this.techWidgetRootAltPrefab, parentContainer, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		gameObject.name = Db.Get().Techs.Get(techID).Name;
		component.GetReference<LocText>("Label").SetText(Db.Get().Techs.Get(techID).Name);
		if (!this.projectTechItems.ContainsKey(techID))
		{
			this.projectTechItems.Add(techID, new Dictionary<string, GameObject>());
		}
		RectTransform reference = component.GetReference<RectTransform>("UnlockContainer");
		System.Action <>9__0;
		foreach (TechItem techItem in Db.Get().Techs.Get(techID).unlockedItems)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(techItem))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.techItemPrefab, reference.gameObject, true);
				gameObject2.GetComponentsInChildren<Image>()[1].sprite = techItem.UISprite();
				gameObject2.GetComponentsInChildren<LocText>()[0].SetText(techItem.Name);
				MultiToggle component2 = gameObject2.GetComponent<MultiToggle>();
				Delegate onClick = component2.onClick;
				System.Action b;
				if ((b = <>9__0) == null)
				{
					b = (<>9__0 = delegate()
					{
						this.researchScreen.ZoomToTech(techID, false);
					});
				}
				component2.onClick = (System.Action)Delegate.Combine(onClick, b);
				gameObject2.GetComponentsInChildren<Image>()[0].color = (this.evenRow ? this.evenRowColor : this.oddRowColor);
				this.evenRow = !this.evenRow;
				if (!this.projectTechItems[techID].ContainsKey(techItem.Id))
				{
					this.projectTechItems[techID].Add(techItem.Id, gameObject2);
				}
			}
		}
		MultiToggle component3 = gameObject.GetComponent<MultiToggle>();
		component3.onClick = (System.Action)Delegate.Combine(component3.onClick, new System.Action(delegate()
		{
			this.researchScreen.ZoomToTech(techID, false);
		}));
		return gameObject;
	}

	// Token: 0x06007094 RID: 28820 RVA: 0x002AE138 File Offset: 0x002AC338
	private void ChangeGameObjectActive(GameObject target, bool targetActiveState)
	{
		if (target.activeSelf != targetActiveState)
		{
			if (targetActiveState)
			{
				this.QueuedActivations.Add(target);
				if (this.QueuedDeactivations.Contains(target))
				{
					this.QueuedDeactivations.Remove(target);
					return;
				}
			}
			else
			{
				this.QueuedDeactivations.Add(target);
				if (this.QueuedActivations.Contains(target))
				{
					this.QueuedActivations.Remove(target);
				}
			}
		}
	}

	// Token: 0x06007095 RID: 28821 RVA: 0x002AE1A0 File Offset: 0x002AC3A0
	private bool IsTextFilterActive()
	{
		return !string.IsNullOrEmpty(this.currentSearchString);
	}

	// Token: 0x06007096 RID: 28822 RVA: 0x002AE1B0 File Offset: 0x002AC3B0
	private bool AnyFilterActive()
	{
		return this.completionFilter != ResearchScreenSideBar.CompletionState.All || this.IsTextFilterActive();
	}

	// Token: 0x06007097 RID: 28823 RVA: 0x002AE1C4 File Offset: 0x002AC3C4
	private int GetTechItemMatchScore(SearchUtil.TechCache techCache, string techItemID)
	{
		TechItem techItem = Db.Get().TechItems.Get(techItemID);
		if (!Game.IsCorrectDlcActiveForCurrentSave(techItem))
		{
			return 0;
		}
		switch (this.completionFilter)
		{
		case ResearchScreenSideBar.CompletionState.Available:
			if (techItem.IsComplete())
			{
				return 0;
			}
			if (!techItem.ParentTech.ArePrerequisitesComplete())
			{
				return 0;
			}
			break;
		case ResearchScreenSideBar.CompletionState.Completed:
			if (!techItem.IsComplete())
			{
				return 0;
			}
			break;
		}
		if (!this.IsTextFilterActive())
		{
			return 100;
		}
		return techCache.techItems[techItemID].Score;
	}

	// Token: 0x06007098 RID: 28824 RVA: 0x002AE248 File Offset: 0x002AC448
	private int GetTechMatchScore(string techID)
	{
		Tech tech = Db.Get().Techs.Get(techID);
		switch (this.completionFilter)
		{
		case ResearchScreenSideBar.CompletionState.Available:
			if (tech.IsComplete())
			{
				return 0;
			}
			if (!tech.ArePrerequisitesComplete())
			{
				return 0;
			}
			break;
		case ResearchScreenSideBar.CompletionState.Completed:
			if (!tech.IsComplete())
			{
				return 0;
			}
			break;
		}
		if (!this.IsTextFilterActive())
		{
			return 100;
		}
		return this.techCaches[techID].Score;
	}

	// Token: 0x06007099 RID: 28825 RVA: 0x002AE2BC File Offset: 0x002AC4BC
	public void ResetFilter()
	{
		this.SetTextFilter("", true);
		this.searchBox.text = "";
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
		{
			this.filterStates[keyValuePair.Key] = false;
			this.filterButtons[keyValuePair.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[keyValuePair.Key] ? 1 : 0);
		}
		this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All, true);
		this.UpdateProjectFilter(null);
	}

	// Token: 0x0600709A RID: 28826 RVA: 0x002AE37C File Offset: 0x002AC57C
	public void SetSearch(string newSearch)
	{
		newSearch = UI.StripLinkFormatting(newSearch);
		this.searchBox.text = newSearch;
		this.SetTextFilter(newSearch, false);
	}

	// Token: 0x04004D6A RID: 19818
	[Header("Containers")]
	[SerializeField]
	private GameObject queueContainer;

	// Token: 0x04004D6B RID: 19819
	[SerializeField]
	private GameObject projectsContainer;

	// Token: 0x04004D6C RID: 19820
	[SerializeField]
	private GameObject searchFiltersContainer;

	// Token: 0x04004D6D RID: 19821
	[Header("Prefabs")]
	[SerializeField]
	private GameObject headerTechTypePrefab;

	// Token: 0x04004D6E RID: 19822
	[SerializeField]
	private GameObject filterButtonPrefab;

	// Token: 0x04004D6F RID: 19823
	[SerializeField]
	private GameObject techWidgetRootPrefab;

	// Token: 0x04004D70 RID: 19824
	[SerializeField]
	private GameObject techWidgetRootAltPrefab;

	// Token: 0x04004D71 RID: 19825
	[SerializeField]
	private GameObject techItemPrefab;

	// Token: 0x04004D72 RID: 19826
	[SerializeField]
	private GameObject techWidgetUnlockedItemPrefab;

	// Token: 0x04004D73 RID: 19827
	[SerializeField]
	private GameObject techWidgetRowPrefab;

	// Token: 0x04004D74 RID: 19828
	[SerializeField]
	private GameObject techCategoryPrefab;

	// Token: 0x04004D75 RID: 19829
	[SerializeField]
	private GameObject techCategoryPrefabAlt;

	// Token: 0x04004D76 RID: 19830
	[Header("Other references")]
	[SerializeField]
	private KInputTextField searchBox;

	// Token: 0x04004D77 RID: 19831
	[SerializeField]
	private MultiToggle allFilter;

	// Token: 0x04004D78 RID: 19832
	[SerializeField]
	private MultiToggle availableFilter;

	// Token: 0x04004D79 RID: 19833
	[SerializeField]
	private MultiToggle completedFilter;

	// Token: 0x04004D7A RID: 19834
	[SerializeField]
	private ResearchScreen researchScreen;

	// Token: 0x04004D7B RID: 19835
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04004D7C RID: 19836
	[SerializeField]
	private Color evenRowColor;

	// Token: 0x04004D7D RID: 19837
	[SerializeField]
	private Color oddRowColor;

	// Token: 0x04004D7E RID: 19838
	private GraphicRaycaster raycaster;

	// Token: 0x04004D7F RID: 19839
	private ResearchScreenSideBar.CompletionState completionFilter;

	// Token: 0x04004D80 RID: 19840
	private Dictionary<string, bool> filterStates = new Dictionary<string, bool>();

	// Token: 0x04004D81 RID: 19841
	private Dictionary<string, bool> categoryExpanded = new Dictionary<string, bool>();

	// Token: 0x04004D82 RID: 19842
	private string currentSearchString = "";

	// Token: 0x04004D83 RID: 19843
	private string currentSearchStringUpper = "";

	// Token: 0x04004D84 RID: 19844
	private const string SEARCH_RESULTS_CATEGORY_ID = "SearchResults";

	// Token: 0x04004D85 RID: 19845
	private Dictionary<string, SearchUtil.TechCache> techCaches;

	// Token: 0x04004D86 RID: 19846
	private readonly Dictionary<string, SearchUtil.TechItemCache> techItemCaches = new Dictionary<string, SearchUtil.TechItemCache>();

	// Token: 0x04004D87 RID: 19847
	private readonly List<GameObject> orderedTechs = new List<GameObject>();

	// Token: 0x04004D88 RID: 19848
	private Dictionary<string, GameObject> queueTechs = new Dictionary<string, GameObject>();

	// Token: 0x04004D89 RID: 19849
	private Dictionary<string, GameObject> projectTechs = new Dictionary<string, GameObject>();

	// Token: 0x04004D8A RID: 19850
	private Dictionary<string, GameObject> projectCategories = new Dictionary<string, GameObject>();

	// Token: 0x04004D8B RID: 19851
	private Dictionary<string, GameObject> filterButtons = new Dictionary<string, GameObject>();

	// Token: 0x04004D8C RID: 19852
	private Dictionary<string, Dictionary<string, GameObject>> projectTechItems = new Dictionary<string, Dictionary<string, GameObject>>();

	// Token: 0x04004D8D RID: 19853
	private Dictionary<string, List<Tag>> filterPresets = new Dictionary<string, List<Tag>>
	{
		{
			"Oxygen",
			new List<Tag>()
		},
		{
			"Food",
			new List<Tag>()
		},
		{
			"Water",
			new List<Tag>()
		},
		{
			"Power",
			new List<Tag>()
		},
		{
			"Morale",
			new List<Tag>()
		},
		{
			"Ranching",
			new List<Tag>()
		},
		{
			"Filter",
			new List<Tag>()
		},
		{
			"Tile",
			new List<Tag>()
		},
		{
			"Transport",
			new List<Tag>()
		},
		{
			"Automation",
			new List<Tag>()
		},
		{
			"Medicine",
			new List<Tag>()
		},
		{
			"Rocket",
			new List<Tag>()
		}
	};

	// Token: 0x04004D8E RID: 19854
	private List<GameObject> QueuedActivations = new List<GameObject>();

	// Token: 0x04004D8F RID: 19855
	private List<GameObject> QueuedDeactivations = new List<GameObject>();

	// Token: 0x04004D90 RID: 19856
	public ButtonSoundPlayer soundPlayer;

	// Token: 0x04004D91 RID: 19857
	[SerializeField]
	private int activationPerFrame = 5;

	// Token: 0x04004D92 RID: 19858
	private Comparer<global::Tuple<GameObject, string>> techWidgetComparer;

	// Token: 0x04004D93 RID: 19859
	private bool evenRow;

	// Token: 0x0200205E RID: 8286
	private enum CompletionState
	{
		// Token: 0x040095D8 RID: 38360
		All,
		// Token: 0x040095D9 RID: 38361
		Available,
		// Token: 0x040095DA RID: 38362
		Completed
	}
}
