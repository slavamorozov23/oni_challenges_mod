using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CC3 RID: 3267
public class CodexScreen : KScreen
{
	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x060064B9 RID: 25785 RVA: 0x0025EDEE File Offset: 0x0025CFEE
	// (set) Token: 0x060064BA RID: 25786 RVA: 0x0025EDF6 File Offset: 0x0025CFF6
	public string activeEntryID
	{
		get
		{
			return this._activeEntryID;
		}
		private set
		{
			this._activeEntryID = value;
		}
	}

	// Token: 0x060064BB RID: 25787 RVA: 0x0025EE00 File Offset: 0x0025D000
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = true;
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		if (string.IsNullOrEmpty(this.activeEntryID))
		{
			this.ChangeArticle("HOME", false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		}
		this.searchInputField.OnValueChangesPaused = delegate()
		{
			this.FilterSearch(this.searchInputField.text);
		};
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			this.editingSearch = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			this.editingSearch = false;
		});
	}

	// Token: 0x060064BC RID: 25788 RVA: 0x0025EED8 File Offset: 0x0025D0D8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.editingSearch)
		{
			e.Consumed = true;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060064BD RID: 25789 RVA: 0x0025EEF0 File Offset: 0x0025D0F0
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x060064BE RID: 25790 RVA: 0x0025EEF8 File Offset: 0x0025D0F8
	public void RefreshTutorialMessages()
	{
		if (!this.HasFocus)
		{
			return;
		}
		string key = CodexCache.FormatLinkID("MISCELLANEOUSTIPS");
		CodexEntry codexEntry;
		if (CodexCache.entries.TryGetValue(key, out codexEntry))
		{
			for (int i = 0; i < codexEntry.subEntries.Count; i++)
			{
				for (int j = 0; j < codexEntry.subEntries[i].contentContainers.Count; j++)
				{
					for (int k = 0; k < codexEntry.subEntries[i].contentContainers[j].content.Count; k++)
					{
						CodexText codexText = codexEntry.subEntries[i].contentContainers[j].content[k] as CodexText;
						if (codexText != null && codexText.messageID == MISC.NOTIFICATIONS.BASICCONTROLS.NAME)
						{
							if (KInputManager.currentControllerIsGamepad)
							{
								codexText.text = MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODYALT;
							}
							else
							{
								codexText.text = MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODY;
							}
							if (!string.IsNullOrEmpty(this.activeEntryID))
							{
								this.ChangeArticle("MISCELLANEOUSTIPS0", false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060064BF RID: 25791 RVA: 0x0025F03C File Offset: 0x0025D23C
	private void CodexScreenInit()
	{
		this.textStyles[CodexTextStyle.Title] = this.textStyleTitle;
		this.textStyles[CodexTextStyle.Subtitle] = this.textStyleSubtitle;
		this.textStyles[CodexTextStyle.Body] = this.textStyleBody;
		this.textStyles[CodexTextStyle.BodyWhite] = this.textStyleBodyWhite;
		this.SetupPrefabs();
		this.PopulatePools();
		this.CategorizeEntries();
		this.FilterSearch("");
		this.backButtonButton.onClick += this.HistoryStepBack;
		this.backButtonButton.soundPlayer.AcceptClickCondition = (() => this.currentHistoryIdx > 0);
		this.fwdButtonButton.onClick += this.HistoryStepForward;
		this.fwdButtonButton.soundPlayer.AcceptClickCondition = (() => this.currentHistoryIdx < this.history.Count - 1);
		Game.Instance.Subscribe(1594320620, delegate(object val)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this.FilterSearch(this.searchInputField.text);
			if (!string.IsNullOrEmpty(this.activeEntryID))
			{
				this.ChangeArticle(this.activeEntryID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		});
		KInputManager.InputChange.AddListener(new UnityAction(this.RefreshTutorialMessages));
	}

	// Token: 0x060064C0 RID: 25792 RVA: 0x0025F148 File Offset: 0x0025D348
	private void SetupPrefabs()
	{
		this.contentContainerPool = new UIGameObjectPool(this.prefabContentContainer);
		this.contentContainerPool.disabledElementParent = this.widgetPool;
		this.ContentPrefabs[typeof(CodexText)] = this.prefabTextWidget;
		this.ContentPrefabs[typeof(CodexTextWithTooltip)] = this.prefabTextWithTooltipWidget;
		this.ContentPrefabs[typeof(CodexImage)] = this.prefabImageWidget;
		this.ContentPrefabs[typeof(CodexDividerLine)] = this.prefabDividerLineWidget;
		this.ContentPrefabs[typeof(CodexSpacer)] = this.prefabSpacer;
		this.ContentPrefabs[typeof(CodexLabelWithIcon)] = this.prefabLabelWithIcon;
		this.ContentPrefabs[typeof(CodexLabelWithLargeIcon)] = this.prefabLabelWithLargeIcon;
		this.ContentPrefabs[typeof(CodexContentLockedIndicator)] = this.prefabContentLocked;
		this.ContentPrefabs[typeof(CodexLargeSpacer)] = this.prefabLargeSpacer;
		this.ContentPrefabs[typeof(CodexVideo)] = this.prefabVideoWidget;
		this.ContentPrefabs[typeof(CodexIndentedLabelWithIcon)] = this.prefabIndentedLabelWithIcon;
		this.ContentPrefabs[typeof(CodexRecipePanel)] = this.prefabRecipePanel;
		this.ContentPrefabs[typeof(CodexConfigurableConsumerRecipePanel)] = this.PrefabConfigurableConsumerRecipePanel;
		this.ContentPrefabs[typeof(CodexTemperatureTransitionPanel)] = this.PrefabTemperatureTransitionPanel;
		this.ContentPrefabs[typeof(CodexConversionPanel)] = this.prefabConversionPanel;
		this.ContentPrefabs[typeof(CodexCollapsibleHeader)] = this.prefabCollapsibleHeader;
		this.ContentPrefabs[typeof(CodexCritterLifecycleWidget)] = this.prefabCritterLifecycleWidget;
		this.ContentPrefabs[typeof(CodexElementCategoryList)] = this.prefabElementCategoryList;
	}

	// Token: 0x060064C1 RID: 25793 RVA: 0x0025F360 File Offset: 0x0025D560
	private HashSet<CodexEntry> FilterSearch(string input)
	{
		this.searchResults.Clear();
		this.subEntrySearchResults.Clear();
		input = SearchUtil.Canonicalize(input).Trim();
		if (string.IsNullOrEmpty(input))
		{
			foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
			{
				if (Game.IsCorrectDlcActiveForCurrentSave(keyValuePair.Value) && !keyValuePair.Value.searchOnly)
				{
					this.searchResults.Add(keyValuePair.Value);
				}
			}
			this.FilterEntries(false);
		}
		else
		{
			foreach (KeyValuePair<string, CodexEntry> keyValuePair2 in CodexCache.entries)
			{
				if (Game.IsCorrectDlcActiveForCurrentSave(keyValuePair2.Value))
				{
					try
					{
						if (SearchUtil.IsPassingScore(FuzzySearch.CanonicalizeAndScore(input, keyValuePair2.Value.name).score))
						{
							this.searchResults.Add(keyValuePair2.Value);
						}
						bool flag = false;
						if (!flag && keyValuePair2.Value.title != null && SearchUtil.IsPassingScore(FuzzySearch.CanonicalizeAndScore(input, Strings.Get(keyValuePair2.Value.title)).score))
						{
							this.subEntrySearchResults.UnionWith(keyValuePair2.Value.subEntries);
							flag = true;
						}
						if (!flag && keyValuePair2.Value.category != null && SearchUtil.IsPassingScore(FuzzySearch.CanonicalizeAndScore(input, keyValuePair2.Value.category).score))
						{
							this.subEntrySearchResults.UnionWith(keyValuePair2.Value.subEntries);
							flag = true;
						}
						if (!flag)
						{
							foreach (SubEntry subEntry in keyValuePair2.Value.subEntries)
							{
								if (SearchUtil.IsPassingScore(FuzzySearch.CanonicalizeAndScore(input, subEntry.name).score))
								{
									this.subEntrySearchResults.Add(subEntry);
								}
							}
						}
					}
					catch (Exception ex)
					{
						KCrashReporter.ReportDevNotification("Fuzzy score bind failed", Environment.StackTrace, ex.Message, false, null);
					}
				}
			}
			this.FilterEntries(true);
		}
		return this.searchResults;
	}

	// Token: 0x060064C2 RID: 25794 RVA: 0x0025F604 File Offset: 0x0025D804
	private bool HasUnlockedCategoryEntries(string entryID)
	{
		foreach (ContentContainer contentContainer in CodexCache.entries[entryID].contentContainers)
		{
			if (string.IsNullOrEmpty(contentContainer.lockID) || Game.Instance.unlocks.IsUnlocked(contentContainer.lockID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060064C3 RID: 25795 RVA: 0x0025F688 File Offset: 0x0025D888
	private void FilterEntries(bool allowOpenCategories = true)
	{
		foreach (KeyValuePair<CodexEntry, GameObject> keyValuePair in this.entryButtons)
		{
			keyValuePair.Value.SetActive(this.searchResults.Contains(keyValuePair.Key) && this.HasUnlockedCategoryEntries(keyValuePair.Key.id));
		}
		foreach (KeyValuePair<SubEntry, GameObject> keyValuePair2 in this.subEntryButtons)
		{
			keyValuePair2.Value.SetActive(this.subEntrySearchResults.Contains(keyValuePair2.Key));
		}
		foreach (GameObject gameObject in this.categoryHeaders)
		{
			bool flag = false;
			Transform transform = gameObject.transform.Find("Content");
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).gameObject.activeSelf)
				{
					flag = true;
					break;
				}
			}
			gameObject.SetActive(flag);
			if (allowOpenCategories)
			{
				if (flag)
				{
					this.ToggleCategoryOpen(gameObject, true);
				}
			}
			else
			{
				this.ToggleCategoryOpen(gameObject, false);
			}
		}
	}

	// Token: 0x060064C4 RID: 25796 RVA: 0x0025F80C File Offset: 0x0025DA0C
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		header.GetComponent<HierarchyReferences>().GetReference("Content").gameObject.SetActive(open);
	}

	// Token: 0x060064C5 RID: 25797 RVA: 0x0025F848 File Offset: 0x0025DA48
	private void PopulatePools()
	{
		foreach (KeyValuePair<Type, GameObject> keyValuePair in this.ContentPrefabs)
		{
			UIGameObjectPool uigameObjectPool = new UIGameObjectPool(keyValuePair.Value);
			uigameObjectPool.disabledElementParent = this.widgetPool;
			this.ContentUIPools[keyValuePair.Key] = uigameObjectPool;
		}
	}

	// Token: 0x060064C6 RID: 25798 RVA: 0x0025F8C0 File Offset: 0x0025DAC0
	private GameObject NewCategoryHeader(KeyValuePair<string, CodexEntry> entryKVP, Dictionary<string, GameObject> categories)
	{
		if (entryKVP.Value.category == "")
		{
			entryKVP.Value.category = "Root";
		}
		GameObject categoryHeader = Util.KInstantiateUI(this.prefabCategoryHeader, this.navigatorContent.gameObject, true);
		GameObject categoryContent = categoryHeader.GetComponent<HierarchyReferences>().GetReference("Content").gameObject;
		categories.Add(entryKVP.Value.category, categoryContent);
		LocText reference = categoryHeader.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
		if (CodexCache.entries.ContainsKey(entryKVP.Value.category))
		{
			reference.text = CodexCache.entries[entryKVP.Value.category].name;
		}
		else
		{
			reference.text = Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES." + entryKVP.Value.category.ToUpper());
		}
		this.categoryHeaders.Add(categoryHeader);
		categoryContent.SetActive(false);
		categoryHeader.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").onClick = delegate()
		{
			this.ToggleCategoryOpen(categoryHeader, !categoryContent.activeSelf);
		};
		return categoryHeader;
	}

	// Token: 0x060064C7 RID: 25799 RVA: 0x0025FA20 File Offset: 0x0025DC20
	private void CategorizeEntries()
	{
		GameObject gameObject = this.navigatorContent.gameObject;
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		List<global::Tuple<string, CodexEntry>> list = new List<global::Tuple<string, CodexEntry>>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
		{
			if (string.IsNullOrEmpty(keyValuePair.Value.sortString))
			{
				keyValuePair.Value.sortString = UI.StripLinkFormatting(Strings.Get(keyValuePair.Value.title));
			}
			list.Add(new global::Tuple<string, CodexEntry>(keyValuePair.Key, keyValuePair.Value));
		}
		list.Sort((global::Tuple<string, CodexEntry> a, global::Tuple<string, CodexEntry> b) => string.Compare(a.second.sortString, b.second.sortString));
		for (int i = 0; i < list.Count; i++)
		{
			global::Tuple<string, CodexEntry> tuple = list[i];
			string text = tuple.second.category;
			if (text == "")
			{
				text = "Root";
			}
			if (!dictionary.ContainsKey(text))
			{
				this.NewCategoryHeader(new KeyValuePair<string, CodexEntry>(tuple.first, tuple.second), dictionary);
			}
			GameObject gameObject2 = Util.KInstantiateUI(this.prefabNavigatorEntry, dictionary[text], true);
			string id = tuple.second.id;
			gameObject2.GetComponent<KButton>().onClick += delegate()
			{
				this.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
			if (string.IsNullOrEmpty(tuple.second.name))
			{
				tuple.second.name = Strings.Get(tuple.second.title);
			}
			gameObject2.GetComponentInChildren<LocText>().text = tuple.second.name;
			this.entryButtons.Add(tuple.second, gameObject2);
			foreach (SubEntry subEntry in tuple.second.subEntries)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.prefabNavigatorEntry, dictionary[text], true);
				string subEntryId = subEntry.id;
				gameObject3.GetComponent<KButton>().onClick += delegate()
				{
					this.ChangeArticle(subEntryId, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
				};
				if (string.IsNullOrEmpty(subEntry.name))
				{
					subEntry.name = Strings.Get(subEntry.title);
				}
				gameObject3.GetComponentInChildren<LocText>().text = subEntry.name;
				this.subEntryButtons.Add(subEntry, gameObject3);
				CodexCache.subEntries.Add(subEntry.id, subEntry);
			}
		}
		foreach (KeyValuePair<string, CodexEntry> keyValuePair2 in CodexCache.entries)
		{
			if (CodexCache.entries.ContainsKey(keyValuePair2.Value.category) && CodexCache.entries.ContainsKey(CodexCache.entries[keyValuePair2.Value.category].category))
			{
				keyValuePair2.Value.searchOnly = true;
			}
		}
		List<KeyValuePair<string, GameObject>> list2 = new List<KeyValuePair<string, GameObject>>();
		foreach (KeyValuePair<string, GameObject> item in dictionary)
		{
			list2.Add(item);
		}
		list2.Sort((KeyValuePair<string, GameObject> a, KeyValuePair<string, GameObject> b) => string.Compare(a.Value.name, b.Value.name));
		for (int j = 0; j < list2.Count; j++)
		{
			list2[j].Value.transform.parent.SetSiblingIndex(j);
		}
		CodexScreen.SetupCategory(dictionary, "PLANTS");
		CodexScreen.SetupCategory(dictionary, "CREATURES");
		CodexScreen.SetupCategory(dictionary, "NOTICES");
		CodexScreen.SetupCategory(dictionary, "RESEARCHNOTES");
		CodexScreen.SetupCategory(dictionary, "JOURNALS");
		CodexScreen.SetupCategory(dictionary, "EMAILS");
		CodexScreen.SetupCategory(dictionary, "INVESTIGATIONS");
		CodexScreen.SetupCategory(dictionary, "MYLOG");
		CodexScreen.SetupCategory(dictionary, "CREATURES::GeneralInfo");
		CodexScreen.SetupCategory(dictionary, "LESSONS");
		CodexScreen.SetupCategory(dictionary, "Root");
	}

	// Token: 0x060064C8 RID: 25800 RVA: 0x0025FEB0 File Offset: 0x0025E0B0
	private static void SetupCategory(Dictionary<string, GameObject> categories, string category_name)
	{
		if (!categories.ContainsKey(category_name))
		{
			return;
		}
		categories[category_name].transform.parent.SetAsFirstSibling();
	}

	// Token: 0x060064C9 RID: 25801 RVA: 0x0025FED4 File Offset: 0x0025E0D4
	public void ChangeArticle(string id, bool playClickSound = false, Vector3 targetPosition = default(Vector3), CodexScreen.HistoryDirection historyMovement = CodexScreen.HistoryDirection.NewArticle)
	{
		global::Debug.Assert(id != null);
		if (playClickSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		}
		if (this.contentContainerPool == null)
		{
			this.CodexScreenInit();
		}
		string text = "";
		SubEntry subEntry = null;
		if (!CodexCache.entries.ContainsKey(id))
		{
			subEntry = CodexCache.FindSubEntry(id);
			if (subEntry != null && !subEntry.disabled)
			{
				id = subEntry.parentEntryID.ToUpper();
				text = UI.StripLinkFormatting(subEntry.name);
			}
			else
			{
				global::Debug.LogWarning("Codex SubEntry not found for target id: " + id);
				id = "PAGENOTFOUND";
			}
		}
		if (CodexCache.entries[id].disabled)
		{
			global::Debug.LogWarning("Codex Entry disabled for target id: " + id);
			id = "PAGENOTFOUND";
		}
		if (string.IsNullOrEmpty(text))
		{
			text = UI.StripLinkFormatting(CodexCache.entries[id].name);
		}
		ICodexWidget codexWidget = null;
		CodexCache.entries[id].GetFirstWidget();
		RectTransform rectTransform = null;
		if (subEntry != null)
		{
			foreach (ContentContainer contentContainer in CodexCache.entries[id].contentContainers)
			{
				if (contentContainer == subEntry.contentContainers[0])
				{
					codexWidget = contentContainer.content[0];
					break;
				}
			}
		}
		int num = 0;
		string text2 = "";
		while (this.contentContainers.transform.childCount > 0)
		{
			while (!string.IsNullOrEmpty(text2) && CodexCache.entries[this.activeEntryID].contentContainers[num].lockID == text2)
			{
				num++;
			}
			GameObject gameObject = this.contentContainers.transform.GetChild(0).gameObject;
			int num2 = 0;
			while (gameObject.transform.childCount > 0)
			{
				if (DlcManager.IsCorrectDlcSubscribed(CodexCache.entries[this.activeEntryID].contentContainers[num].content[num2] as IHasDlcRestrictions))
				{
					GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
					Type key;
					if (gameObject2.name == "PrefabContentLocked")
					{
						text2 = CodexCache.entries[this.activeEntryID].contentContainers[num].lockID;
						key = typeof(CodexContentLockedIndicator);
					}
					else
					{
						key = CodexCache.entries[this.activeEntryID].contentContainers[num].content[num2].GetType();
					}
					this.ContentUIPools[key].ClearElement(gameObject2);
				}
				num2++;
			}
			this.contentContainerPool.ClearElement(this.contentContainers.transform.GetChild(0).gameObject);
			num++;
		}
		bool flag = CodexCache.entries[id] is CategoryEntry;
		this.activeEntryID = id;
		if (CodexCache.entries[id].contentContainers == null)
		{
			CodexCache.entries[id].CreateContentContainerCollection();
		}
		bool flag2 = false;
		string a = "";
		for (int i = 0; i < CodexCache.entries[id].contentContainers.Count; i++)
		{
			ContentContainer contentContainer2 = CodexCache.entries[id].contentContainers[i];
			if (Game.IsCorrectDlcActiveForCurrentSave(contentContainer2))
			{
				if (!string.IsNullOrEmpty(contentContainer2.lockID) && !Game.Instance.unlocks.IsUnlocked(contentContainer2.lockID))
				{
					if (a != contentContainer2.lockID)
					{
						GameObject gameObject3 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
						this.ConfigureContentContainer(contentContainer2, gameObject3, flag && flag2);
						a = contentContainer2.lockID;
						GameObject gameObject4 = this.ContentUIPools[typeof(CodexContentLockedIndicator)].GetFreeElement(gameObject3, true).gameObject;
					}
				}
				else
				{
					GameObject gameObject3 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
					this.ConfigureContentContainer(contentContainer2, gameObject3, flag && flag2);
					flag2 = !flag2;
					if (contentContainer2.content != null)
					{
						foreach (ICodexWidget codexWidget2 in contentContainer2.content)
						{
							if (Game.IsCorrectDlcActiveForCurrentSave(codexWidget2 as IHasDlcRestrictions))
							{
								GameObject gameObject5 = this.ContentUIPools[codexWidget2.GetType()].GetFreeElement(gameObject3, true).gameObject;
								codexWidget2.Configure(gameObject5, this.displayPane, this.textStyles);
								if (codexWidget2 == codexWidget)
								{
									rectTransform = gameObject5.rectTransform();
								}
							}
						}
					}
				}
			}
		}
		string text3 = "";
		string text4 = id;
		int num3 = 0;
		while (text4 != CodexCache.FormatLinkID("HOME") && num3 < 10)
		{
			num3++;
			if (text4 != null)
			{
				if (text4 != id)
				{
					text3 = text3.Insert(0, CodexCache.entries[text4].name + " > ");
				}
				else
				{
					text3 = text3.Insert(0, CodexCache.entries[text4].name);
				}
				text4 = CodexCache.entries[text4].parentId;
			}
			else
			{
				text4 = CodexCache.entries[CodexCache.FormatLinkID("HOME")].id;
				text3 = text3.Insert(0, CodexCache.entries[text4].name + " > ");
			}
		}
		this.currentLocationText.text = ((text3 == "") ? ("<b>" + UI.StripLinkFormatting(CodexCache.entries["HOME"].name) + "</b>") : text3);
		if (this.history.Count == 0)
		{
			this.history.Add(new CodexScreen.HistoryEntry(id, Vector3.zero, text));
			this.currentHistoryIdx = 0;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.Back)
		{
			this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
			this.currentHistoryIdx--;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.Forward)
		{
			this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
			this.currentHistoryIdx++;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.NewArticle || historyMovement == CodexScreen.HistoryDirection.Up)
		{
			if (this.currentHistoryIdx == this.history.Count - 1)
			{
				this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, text));
				this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
				this.currentHistoryIdx++;
			}
			else
			{
				for (int j = this.history.Count - 1; j > this.currentHistoryIdx; j--)
				{
					this.history.RemoveAt(j);
				}
				this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, text));
				this.history[this.history.Count - 2].position = this.displayPane.transform.localPosition;
				this.currentHistoryIdx++;
			}
		}
		if (this.currentHistoryIdx > 0)
		{
			this.backButtonButton.GetComponent<Image>().color = Color.black;
			this.backButton.text = UI.FormatAsLink(string.Format(UI.CODEX.BACK_BUTTON, UI.StripLinkFormatting(CodexCache.entries[this.history[this.history.Count - 2].id].name)), CodexCache.entries[this.history[this.history.Count - 2].id].id);
			this.backButtonButton.GetComponent<ToolTip>().toolTip = string.Format(UI.CODEX.BACK_BUTTON_TOOLTIP, this.history[this.currentHistoryIdx - 1].name);
		}
		else
		{
			this.backButtonButton.GetComponent<Image>().color = Color.grey;
			this.backButton.text = UI.StripLinkFormatting(GameUtil.ColourizeString(Color.grey, string.Format(UI.CODEX.BACK_BUTTON, CodexCache.entries["HOME"].name)));
			this.backButtonButton.GetComponent<ToolTip>().toolTip = UI.CODEX.BACK_BUTTON_NO_HISTORY_TOOLTIP;
		}
		if (this.currentHistoryIdx < this.history.Count - 1)
		{
			this.fwdButtonButton.GetComponent<Image>().color = Color.black;
			this.fwdButtonButton.GetComponent<ToolTip>().toolTip = string.Format(UI.CODEX.FORWARD_BUTTON_TOOLTIP, this.history[this.currentHistoryIdx + 1].name);
		}
		else
		{
			this.fwdButtonButton.GetComponent<Image>().color = Color.grey;
			this.fwdButtonButton.GetComponent<ToolTip>().toolTip = UI.CODEX.FORWARD_BUTTON_NO_HISTORY_TOOLTIP;
		}
		if (targetPosition != Vector3.zero)
		{
			if (this.scrollToTargetRoutine != null)
			{
				base.StopCoroutine(this.scrollToTargetRoutine);
			}
			this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(targetPosition));
			return;
		}
		if (rectTransform != null)
		{
			if (this.scrollToTargetRoutine != null)
			{
				base.StopCoroutine(this.scrollToTargetRoutine);
			}
			this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(rectTransform));
			return;
		}
		this.displayScrollRect.content.SetLocalPosition(Vector3.zero);
	}

	// Token: 0x060064CA RID: 25802 RVA: 0x002608DC File Offset: 0x0025EADC
	private void HistoryStepBack()
	{
		if (this.currentHistoryIdx == 0)
		{
			return;
		}
		this.ChangeArticle(this.history[this.currentHistoryIdx - 1].id, false, this.history[this.currentHistoryIdx - 1].position, CodexScreen.HistoryDirection.Back);
	}

	// Token: 0x060064CB RID: 25803 RVA: 0x0026092C File Offset: 0x0025EB2C
	private void HistoryStepForward()
	{
		if (this.currentHistoryIdx == this.history.Count - 1)
		{
			return;
		}
		this.ChangeArticle(this.history[this.currentHistoryIdx + 1].id, false, this.history[this.currentHistoryIdx + 1].position, CodexScreen.HistoryDirection.Forward);
	}

	// Token: 0x060064CC RID: 25804 RVA: 0x00260988 File Offset: 0x0025EB88
	private void HistoryStepUp()
	{
		if (string.IsNullOrEmpty(CodexCache.entries[this.activeEntryID].parentId))
		{
			return;
		}
		this.ChangeArticle(CodexCache.entries[this.activeEntryID].parentId, false, default(Vector3), CodexScreen.HistoryDirection.Up);
	}

	// Token: 0x060064CD RID: 25805 RVA: 0x002609D8 File Offset: 0x0025EBD8
	private IEnumerator ScrollToTarget(RectTransform targetWidgetTransform)
	{
		yield return 0;
		this.displayScrollRect.content.SetLocalPosition(Vector3.down * (this.displayScrollRect.content.InverseTransformPoint(targetWidgetTransform.GetPosition()).y + 12f));
		this.scrollToTargetRoutine = null;
		yield break;
	}

	// Token: 0x060064CE RID: 25806 RVA: 0x002609EE File Offset: 0x0025EBEE
	private IEnumerator ScrollToTarget(Vector3 position)
	{
		yield return 0;
		this.displayScrollRect.content.SetLocalPosition(position);
		this.scrollToTargetRoutine = null;
		yield break;
	}

	// Token: 0x060064CF RID: 25807 RVA: 0x00260A04 File Offset: 0x0025EC04
	public void FocusContainer(ContentContainer target)
	{
		if (target == null || target.go == null)
		{
			return;
		}
		RectTransform rectTransform = target.go.transform.GetChild(0) as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		if (this.scrollToTargetRoutine != null)
		{
			base.StopCoroutine(this.scrollToTargetRoutine);
		}
		this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(rectTransform));
	}

	// Token: 0x060064D0 RID: 25808 RVA: 0x00260A6C File Offset: 0x0025EC6C
	private void ConfigureContentContainer(ContentContainer container, GameObject containerGameObject, bool bgColor = false)
	{
		container.go = containerGameObject;
		LayoutGroup layoutGroup = containerGameObject.GetComponent<LayoutGroup>();
		if (layoutGroup != null)
		{
			UnityEngine.Object.DestroyImmediate(layoutGroup);
		}
		if (!Game.Instance.unlocks.IsUnlocked(container.lockID) && !string.IsNullOrEmpty(container.lockID))
		{
			layoutGroup = containerGameObject.AddComponent<VerticalLayoutGroup>();
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		}
		switch (container.contentLayout)
		{
		case ContentContainer.ContentLayout.Vertical:
			layoutGroup = containerGameObject.AddComponent<VerticalLayoutGroup>();
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		case ContentContainer.ContentLayout.Horizontal:
			layoutGroup = containerGameObject.AddComponent<HorizontalLayoutGroup>();
			layoutGroup.childAlignment = TextAnchor.MiddleLeft;
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		case ContentContainer.ContentLayout.Grid:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 4;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(128f, 180f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(6f, 6f);
			return;
		case ContentContainer.ContentLayout.GridTwoColumn:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 2;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(264f, 32f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(0f, 12f);
			return;
		case ContentContainer.ContentLayout.GridTwoColumnTall:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 2;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(264f, 64f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(0f, 12f);
			return;
		default:
			return;
		}
	}

	// Token: 0x04004447 RID: 17479
	private string _activeEntryID;

	// Token: 0x04004448 RID: 17480
	private Dictionary<Type, UIGameObjectPool> ContentUIPools = new Dictionary<Type, UIGameObjectPool>();

	// Token: 0x04004449 RID: 17481
	private Dictionary<Type, GameObject> ContentPrefabs = new Dictionary<Type, GameObject>();

	// Token: 0x0400444A RID: 17482
	private List<GameObject> categoryHeaders = new List<GameObject>();

	// Token: 0x0400444B RID: 17483
	private Dictionary<CodexEntry, GameObject> entryButtons = new Dictionary<CodexEntry, GameObject>();

	// Token: 0x0400444C RID: 17484
	private Dictionary<SubEntry, GameObject> subEntryButtons = new Dictionary<SubEntry, GameObject>();

	// Token: 0x0400444D RID: 17485
	private UIGameObjectPool contentContainerPool;

	// Token: 0x0400444E RID: 17486
	[SerializeField]
	private KScrollRect displayScrollRect;

	// Token: 0x0400444F RID: 17487
	[SerializeField]
	private RectTransform scrollContentPane;

	// Token: 0x04004450 RID: 17488
	private bool editingSearch;

	// Token: 0x04004451 RID: 17489
	private List<CodexScreen.HistoryEntry> history = new List<CodexScreen.HistoryEntry>();

	// Token: 0x04004452 RID: 17490
	private int currentHistoryIdx;

	// Token: 0x04004453 RID: 17491
	[Header("Hierarchy")]
	[SerializeField]
	private Transform navigatorContent;

	// Token: 0x04004454 RID: 17492
	[SerializeField]
	private Transform displayPane;

	// Token: 0x04004455 RID: 17493
	[SerializeField]
	private Transform contentContainers;

	// Token: 0x04004456 RID: 17494
	[SerializeField]
	private Transform widgetPool;

	// Token: 0x04004457 RID: 17495
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004458 RID: 17496
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x04004459 RID: 17497
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x0400445A RID: 17498
	[SerializeField]
	private LocText backButton;

	// Token: 0x0400445B RID: 17499
	[SerializeField]
	private KButton backButtonButton;

	// Token: 0x0400445C RID: 17500
	[SerializeField]
	private KButton fwdButtonButton;

	// Token: 0x0400445D RID: 17501
	[SerializeField]
	private LocText currentLocationText;

	// Token: 0x0400445E RID: 17502
	[Header("Prefabs")]
	[SerializeField]
	private GameObject prefabNavigatorEntry;

	// Token: 0x0400445F RID: 17503
	[SerializeField]
	private GameObject prefabCategoryHeader;

	// Token: 0x04004460 RID: 17504
	[SerializeField]
	private GameObject prefabContentContainer;

	// Token: 0x04004461 RID: 17505
	[SerializeField]
	private GameObject prefabTextWidget;

	// Token: 0x04004462 RID: 17506
	[SerializeField]
	private GameObject prefabTextWithTooltipWidget;

	// Token: 0x04004463 RID: 17507
	[SerializeField]
	private GameObject prefabImageWidget;

	// Token: 0x04004464 RID: 17508
	[SerializeField]
	private GameObject prefabDividerLineWidget;

	// Token: 0x04004465 RID: 17509
	[SerializeField]
	private GameObject prefabSpacer;

	// Token: 0x04004466 RID: 17510
	[SerializeField]
	private GameObject prefabLargeSpacer;

	// Token: 0x04004467 RID: 17511
	[SerializeField]
	private GameObject prefabLabelWithIcon;

	// Token: 0x04004468 RID: 17512
	[SerializeField]
	private GameObject prefabLabelWithLargeIcon;

	// Token: 0x04004469 RID: 17513
	[SerializeField]
	private GameObject prefabContentLocked;

	// Token: 0x0400446A RID: 17514
	[SerializeField]
	private GameObject prefabVideoWidget;

	// Token: 0x0400446B RID: 17515
	[SerializeField]
	private GameObject prefabIndentedLabelWithIcon;

	// Token: 0x0400446C RID: 17516
	[SerializeField]
	private GameObject prefabRecipePanel;

	// Token: 0x0400446D RID: 17517
	[SerializeField]
	private GameObject PrefabConfigurableConsumerRecipePanel;

	// Token: 0x0400446E RID: 17518
	[SerializeField]
	private GameObject PrefabTemperatureTransitionPanel;

	// Token: 0x0400446F RID: 17519
	[SerializeField]
	private GameObject prefabConversionPanel;

	// Token: 0x04004470 RID: 17520
	[SerializeField]
	private GameObject prefabCollapsibleHeader;

	// Token: 0x04004471 RID: 17521
	[SerializeField]
	private GameObject prefabCritterLifecycleWidget;

	// Token: 0x04004472 RID: 17522
	[SerializeField]
	private GameObject prefabElementCategoryList;

	// Token: 0x04004473 RID: 17523
	[Header("Text Styles")]
	[SerializeField]
	private TextStyleSetting textStyleTitle;

	// Token: 0x04004474 RID: 17524
	[SerializeField]
	private TextStyleSetting textStyleSubtitle;

	// Token: 0x04004475 RID: 17525
	[SerializeField]
	private TextStyleSetting textStyleBody;

	// Token: 0x04004476 RID: 17526
	[SerializeField]
	private TextStyleSetting textStyleBodyWhite;

	// Token: 0x04004477 RID: 17527
	private Dictionary<CodexTextStyle, TextStyleSetting> textStyles = new Dictionary<CodexTextStyle, TextStyleSetting>();

	// Token: 0x04004478 RID: 17528
	private readonly HashSet<CodexEntry> searchResults = new HashSet<CodexEntry>();

	// Token: 0x04004479 RID: 17529
	private readonly HashSet<SubEntry> subEntrySearchResults = new HashSet<SubEntry>();

	// Token: 0x0400447A RID: 17530
	private Coroutine scrollToTargetRoutine;

	// Token: 0x02001EFB RID: 7931
	public enum PlanCategory
	{
		// Token: 0x0400912D RID: 37165
		Home,
		// Token: 0x0400912E RID: 37166
		Tips,
		// Token: 0x0400912F RID: 37167
		MyLog,
		// Token: 0x04009130 RID: 37168
		Investigations,
		// Token: 0x04009131 RID: 37169
		Emails,
		// Token: 0x04009132 RID: 37170
		Journals,
		// Token: 0x04009133 RID: 37171
		ResearchNotes,
		// Token: 0x04009134 RID: 37172
		Creatures,
		// Token: 0x04009135 RID: 37173
		Plants,
		// Token: 0x04009136 RID: 37174
		Food,
		// Token: 0x04009137 RID: 37175
		Tech,
		// Token: 0x04009138 RID: 37176
		Diseases,
		// Token: 0x04009139 RID: 37177
		Roles,
		// Token: 0x0400913A RID: 37178
		Buildings,
		// Token: 0x0400913B RID: 37179
		Elements
	}

	// Token: 0x02001EFC RID: 7932
	public enum HistoryDirection
	{
		// Token: 0x0400913D RID: 37181
		Back,
		// Token: 0x0400913E RID: 37182
		Forward,
		// Token: 0x0400913F RID: 37183
		Up,
		// Token: 0x04009140 RID: 37184
		NewArticle
	}

	// Token: 0x02001EFD RID: 7933
	public class HistoryEntry
	{
		// Token: 0x0600B51C RID: 46364 RVA: 0x003ED295 File Offset: 0x003EB495
		public HistoryEntry(string entry, Vector3 pos, string articleName)
		{
			this.id = entry;
			this.position = pos;
			this.name = articleName;
		}

		// Token: 0x04009141 RID: 37185
		public string id;

		// Token: 0x04009142 RID: 37186
		public Vector3 position;

		// Token: 0x04009143 RID: 37187
		public string name;
	}
}
