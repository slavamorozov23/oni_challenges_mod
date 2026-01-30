using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000CBA RID: 3258
public static class CodexCache
{
	// Token: 0x060063EB RID: 25579 RVA: 0x0025383F File Offset: 0x00251A3F
	public static string FormatLinkID(string linkID)
	{
		linkID = linkID.ToUpper();
		linkID = linkID.Replace("_", "");
		return linkID;
	}

	// Token: 0x060063EC RID: 25580 RVA: 0x0025385C File Offset: 0x00251A5C
	public static void CodexCacheInit()
	{
		CodexCache.entries = new Dictionary<string, CodexEntry>();
		CodexCache.subEntries = new Dictionary<string, SubEntry>();
		CodexCache.unlockedEntryLookup = new Dictionary<string, List<string>>();
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		if (CodexCache.widgetTagMappings == null)
		{
			CodexCache.widgetTagMappings = new List<global::Tuple<string, Type>>
			{
				new global::Tuple<string, Type>("!CodexText", typeof(CodexText)),
				new global::Tuple<string, Type>("!CodexImage", typeof(CodexImage)),
				new global::Tuple<string, Type>("!CodexDividerLine", typeof(CodexDividerLine)),
				new global::Tuple<string, Type>("!CodexSpacer", typeof(CodexSpacer)),
				new global::Tuple<string, Type>("!CodexLabelWithIcon", typeof(CodexLabelWithIcon)),
				new global::Tuple<string, Type>("!CodexLabelWithLargeIcon", typeof(CodexLabelWithLargeIcon)),
				new global::Tuple<string, Type>("!CodexContentLockedIndicator", typeof(CodexContentLockedIndicator)),
				new global::Tuple<string, Type>("!CodexLargeSpacer", typeof(CodexLargeSpacer)),
				new global::Tuple<string, Type>("!CodexVideo", typeof(CodexVideo)),
				new global::Tuple<string, Type>("!CodexElementCategoryList", typeof(CodexElementCategoryList))
			};
		}
		string text = CodexCache.FormatLinkID("DUPLICANTSCATEGORY");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.DUPLICANTS, CodexEntryGenerator.GenerateDuplicantEntries(), Assets.GetSprite("codexIconDupes"), true, false, UI.CODEX.CATEGORYNAMES.DUPLICANTS));
		text = CodexCache.FormatLinkID("LESSONS");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.TUTORIALS, CodexEntryGenerator.GenerateTutorialNotificationEntries(), Assets.GetSprite("codexIconLessons"), true, true, UI.CODEX.CATEGORYNAMES.TUTORIALS));
		text = CodexCache.FormatLinkID("creatures");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.CREATURES, CodexEntryGenerator_Creatures.GenerateEntries(), Assets.GetSprite("codexIconCritters"), true, false, null));
		DebugUtil.DevAssert(text == "CREATURES", string.Empty, null);
		text = CodexCache.FormatLinkID("plants");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.PLANTS, CodexEntryGenerator.GeneratePlantEntries(), Assets.GetSprite("codexIconPlants"), true, true, null));
		text = CodexCache.FormatLinkID("food");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.FOOD, CodexEntryGenerator.GenerateFoodEntries(), Assets.GetSprite("codexIconFood"), true, true, null));
		text = CodexCache.FormatLinkID("buildings");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.BUILDINGS, CodexEntryGenerator.GenerateBuildingEntries(), Assets.GetSprite("codexIconBuildings"), true, true, null));
		text = CodexCache.FormatLinkID("tech");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.TECH, CodexEntryGenerator.GenerateTechEntries(), Assets.GetSprite("codexIconResearch"), true, true, null));
		text = CodexCache.FormatLinkID("disease");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.DISEASE, CodexEntryGenerator.GenerateDiseaseEntries(), Assets.GetSprite("codexIconDisease"), false, true, null));
		text = CodexCache.FormatLinkID("elements");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.ELEMENTS, CodexEntryGenerator_Elements.GenerateEntries(), Assets.GetSprite("codexIconElements"), true, false, null));
		text = CodexCache.FormatLinkID("BUILDINGMATERIALCLASSES");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.BUILDINGMATERIALCLASSES, CodexEntryGenerator.GenerateConstructionMaterialEntries(), Assets.GetSprite("ui_elements_classes"), true, false, null));
		text = CodexCache.FormatLinkID("geysers");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.GEYSERS, CodexEntryGenerator.GenerateGeyserEntries(), Assets.GetSprite("codexIconGeysers"), true, true, null));
		text = CodexCache.FormatLinkID("equipment");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.EQUIPMENT, CodexEntryGenerator.GenerateEquipmentEntries(), Assets.GetSprite("codexIconEquipment"), true, true, null));
		text = CodexCache.FormatLinkID("biomes");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.BIOMES, CodexEntryGenerator.GenerateBiomeEntries(), Assets.GetSprite("codexIconGeysers"), true, true, null));
		text = CodexCache.FormatLinkID("rooms");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.ROOMS, CodexEntryGenerator.GenerateRoomsEntries(), Assets.GetSprite("codexIconRooms"), true, true, null));
		text = CodexCache.FormatLinkID("STORYTRAITS");
		dictionary.Add(text, CodexEntryGenerator.GenerateCategoryEntry(text, UI.CODEX.CATEGORYNAMES.STORYTRAITS, new Dictionary<string, CodexEntry>(), Assets.GetSprite("codexIconStoryTraits"), true, true, null));
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			CodexEntryGenerator.GenerateBionicUpgradeEntries();
			CodexEntryGenerator.GenerateElectrobankEntries();
		}
		CategoryEntry item = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID("HOME"), UI.CODEX.CATEGORYNAMES.ROOT, dictionary, null, true, true, null);
		CodexEntryGenerator.GeneratePageNotFound();
		List<CategoryEntry> list = new List<CategoryEntry>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in dictionary)
		{
			list.Add(keyValuePair.Value as CategoryEntry);
		}
		CodexCache.CollectYAMLEntries(list);
		CodexCache.CollectYAMLSubEntries(list);
		CodexCache.CheckUnlockableContent();
		list.Add(item);
		foreach (KeyValuePair<string, CodexEntry> keyValuePair2 in CodexCache.entries)
		{
			if (keyValuePair2.Value.contentMadeAndUsed.Count > 0)
			{
				foreach (CodexEntry_MadeAndUsed codexEntry_MadeAndUsed in keyValuePair2.Value.contentMadeAndUsed)
				{
					List<ContentContainer> list2 = new List<ContentContainer>();
					Element element = ElementLoader.GetElement(codexEntry_MadeAndUsed.tag);
					if (element != null)
					{
						CodexEntryGenerator_Elements.GenerateElementDescriptionContainers(element, list2);
					}
					else
					{
						CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(codexEntry_MadeAndUsed.tag, list2);
					}
					keyValuePair2.Value.contentContainers.InsertRange(keyValuePair2.Value.contentContainers.Count, list2);
				}
			}
			if (keyValuePair2.Value.subEntries.Count > 0)
			{
				keyValuePair2.Value.subEntries.Sort((SubEntry a, SubEntry b) => a.layoutPriority.CompareTo(b.layoutPriority));
				if (keyValuePair2.Value.icon == null)
				{
					keyValuePair2.Value.icon = keyValuePair2.Value.subEntries[0].icon;
					keyValuePair2.Value.iconColor = keyValuePair2.Value.subEntries[0].iconColor;
				}
				int num = 0;
				foreach (SubEntry subEntry in keyValuePair2.Value.subEntries)
				{
					if (subEntry.lockID != null && !Game.Instance.unlocks.IsUnlocked(subEntry.lockID))
					{
						num++;
					}
				}
				if (keyValuePair2.Value.subEntries.Count > 1)
				{
					List<ICodexWidget> list3 = new List<ICodexWidget>();
					list3.Add(new CodexSpacer());
					list3.Add(new CodexText(string.Format(CODEX.HEADERS.SUBENTRIES, keyValuePair2.Value.subEntries.Count - num, keyValuePair2.Value.subEntries.Count), CodexTextStyle.Subtitle, null));
					foreach (SubEntry subEntry2 in keyValuePair2.Value.subEntries)
					{
						if (subEntry2.lockID != null && !Game.Instance.unlocks.IsUnlocked(subEntry2.lockID))
						{
							list3.Add(new CodexText(UI.FormatAsLink(CODEX.HEADERS.CONTENTLOCKED, UI.ExtractLinkID(subEntry2.name)), CodexTextStyle.Body, null));
						}
						else
						{
							string text2 = UI.StripLinkFormatting((subEntry2.name == null) ? Strings.Get(subEntry2.title) : subEntry2.name);
							text2 = UI.FormatAsLink(text2, subEntry2.id);
							list3.Add(new CodexText(text2, CodexTextStyle.Body, null));
						}
					}
					list3.Add(new CodexSpacer());
					keyValuePair2.Value.contentContainers.Insert(keyValuePair2.Value.customContentLength, new ContentContainer(list3, ContentContainer.ContentLayout.Vertical));
				}
			}
			for (int i = 0; i < keyValuePair2.Value.subEntries.Count; i++)
			{
				keyValuePair2.Value.AddContentContainerRange(keyValuePair2.Value.subEntries[i].contentContainers);
			}
		}
		CodexEntryGenerator.PopulateCategoryEntries(list, delegate(CodexEntry a, CodexEntry b)
		{
			if (a.name == UI.CODEX.CATEGORYNAMES.TIPS)
			{
				return -1;
			}
			if (b.name == UI.CODEX.CATEGORYNAMES.TIPS)
			{
				return 1;
			}
			return UI.StripLinkFormatting(a.name).CompareTo(UI.StripLinkFormatting(b.name));
		});
		CodexEntryGenerator.PopulateCategoryEntries(CodexCache.categoriesForPostYAMLPopulation, null);
		CodexCache.categoriesForPostYAMLPopulation.Clear();
	}

	// Token: 0x060063ED RID: 25581 RVA: 0x0025421C File Offset: 0x0025241C
	public static CodexEntry FindEntry(string id)
	{
		if (CodexCache.entries == null)
		{
			global::Debug.LogWarning("Can't search Codex cache while it's stil null");
			return null;
		}
		if (CodexCache.entries.ContainsKey(id))
		{
			return CodexCache.entries[id];
		}
		global::Debug.LogWarning("Could not find codex entry with id: " + id);
		return null;
	}

	// Token: 0x060063EE RID: 25582 RVA: 0x0025425C File Offset: 0x0025245C
	public static SubEntry FindSubEntry(string id)
	{
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
		{
			foreach (SubEntry subEntry in keyValuePair.Value.subEntries)
			{
				if (subEntry.id.ToUpper() == id.ToUpper())
				{
					return subEntry;
				}
			}
		}
		return null;
	}

	// Token: 0x060063EF RID: 25583 RVA: 0x0025430C File Offset: 0x0025250C
	private static void CheckUnlockableContent()
	{
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
		{
			foreach (SubEntry subEntry in keyValuePair.Value.subEntries)
			{
				if (subEntry.lockedContentContainer != null)
				{
					subEntry.lockedContentContainer.content.Clear();
					subEntry.contentContainers.Remove(subEntry.lockedContentContainer);
				}
			}
		}
	}

	// Token: 0x060063F0 RID: 25584 RVA: 0x002543C4 File Offset: 0x002525C4
	private static void CollectYAMLEntries(List<CategoryEntry> categories)
	{
		CodexCache.baseEntryPath = Application.streamingAssetsPath + "/codex";
		foreach (CodexEntry codexEntry in CodexCache.CollectEntries(""))
		{
			if (codexEntry != null && codexEntry.id != null && codexEntry.contentContainers != null && Game.IsCorrectDlcActiveForCurrentSave(codexEntry))
			{
				if (CodexCache.entries.ContainsKey(CodexCache.FormatLinkID(codexEntry.id)))
				{
					CodexCache.MergeEntry(codexEntry.id, codexEntry);
				}
				else
				{
					CodexCache.AddEntry(codexEntry.id, codexEntry, categories);
					codexEntry.customContentLength = codexEntry.contentContainers.Count;
				}
			}
		}
		string[] directories = Directory.GetDirectories(CodexCache.baseEntryPath);
		for (int i = 0; i < directories.Length; i++)
		{
			foreach (CodexEntry codexEntry2 in CodexCache.CollectEntries(Path.GetFileNameWithoutExtension(directories[i])))
			{
				if (codexEntry2 != null && codexEntry2.id != null && codexEntry2.contentContainers != null && Game.IsCorrectDlcActiveForCurrentSave(codexEntry2))
				{
					if (CodexCache.entries.ContainsKey(CodexCache.FormatLinkID(codexEntry2.id)))
					{
						CodexCache.MergeEntry(codexEntry2.id, codexEntry2);
					}
					else
					{
						CodexCache.AddEntry(codexEntry2.id, codexEntry2, categories);
						codexEntry2.customContentLength = codexEntry2.contentContainers.Count;
					}
				}
			}
		}
	}

	// Token: 0x060063F1 RID: 25585 RVA: 0x00254554 File Offset: 0x00252754
	private static void CollectYAMLSubEntries(List<CategoryEntry> categories)
	{
		CodexCache.baseEntryPath = Application.streamingAssetsPath + "/codex";
		using (List<SubEntry>.Enumerator enumerator = CodexCache.CollectSubEntries("").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SubEntry v = enumerator.Current;
				if (v.parentEntryID != null && v.id != null && Game.IsCorrectDlcActiveForCurrentSave(v))
				{
					if (CodexCache.entries.ContainsKey(v.parentEntryID.ToUpper()))
					{
						SubEntry subEntry = CodexCache.entries[v.parentEntryID.ToUpper()].subEntries.Find((SubEntry match) => match.id == v.id);
						if (!string.IsNullOrEmpty(v.lockID))
						{
							foreach (ContentContainer contentContainer in v.contentContainers)
							{
								contentContainer.lockID = v.lockID;
							}
						}
						if (subEntry != null)
						{
							if (!string.IsNullOrEmpty(v.lockID))
							{
								foreach (ContentContainer contentContainer2 in subEntry.contentContainers)
								{
									contentContainer2.lockID = v.lockID;
								}
								subEntry.lockID = v.lockID;
							}
							for (int i = 0; i < v.contentContainers.Count; i++)
							{
								if (Game.IsCorrectDlcActiveForCurrentSave(v.contentContainers[i]))
								{
									if (!string.IsNullOrEmpty(v.contentContainers[i].lockID))
									{
										int num = subEntry.contentContainers.IndexOf(subEntry.lockedContentContainer);
										subEntry.contentContainers.Insert(num + 1, v.contentContainers[i]);
									}
									else if (v.contentContainers[i].showBeforeGeneratedContent)
									{
										subEntry.contentContainers.Insert(0, v.contentContainers[i]);
									}
									else
									{
										subEntry.contentContainers.Add(v.contentContainers[i]);
									}
								}
							}
							subEntry.contentContainers.Add(new ContentContainer(new List<ICodexWidget>
							{
								new CodexLargeSpacer()
							}, ContentContainer.ContentLayout.Vertical));
							subEntry.layoutPriority = v.layoutPriority;
						}
						else
						{
							CodexCache.entries[v.parentEntryID.ToUpper()].subEntries.Add(v);
						}
					}
					else
					{
						global::Debug.LogWarningFormat("Codex SubEntry {0} cannot find parent codex entry with id {1}", new object[]
						{
							v.name,
							v.parentEntryID
						});
					}
				}
			}
		}
	}

	// Token: 0x060063F2 RID: 25586 RVA: 0x002548B8 File Offset: 0x00252AB8
	private static void AddLockLookup(string lockId, string articleId)
	{
		if (!CodexCache.unlockedEntryLookup.ContainsKey(lockId))
		{
			CodexCache.unlockedEntryLookup[lockId] = new List<string>();
		}
		CodexCache.unlockedEntryLookup[lockId].Add(articleId);
	}

	// Token: 0x060063F3 RID: 25587 RVA: 0x002548E8 File Offset: 0x00252AE8
	public static string GetEntryForLock(string lockId)
	{
		if (CodexCache.unlockedEntryLookup == null)
		{
			global::Debug.LogWarningFormat("Trying to get lock entry {0} before codex cache has been initialized.", new object[]
			{
				lockId
			});
			return null;
		}
		if (string.IsNullOrEmpty(lockId))
		{
			return null;
		}
		if (CodexCache.unlockedEntryLookup.ContainsKey(lockId) && CodexCache.unlockedEntryLookup[lockId] != null && CodexCache.unlockedEntryLookup[lockId].Count > 0)
		{
			return CodexCache.unlockedEntryLookup[lockId][0];
		}
		return null;
	}

	// Token: 0x060063F4 RID: 25588 RVA: 0x0025495C File Offset: 0x00252B5C
	public static void AddEntry(string id, CodexEntry entry, List<CategoryEntry> categoryEntries = null)
	{
		id = CodexCache.FormatLinkID(id);
		if (CodexCache.entries.ContainsKey(id))
		{
			global::Debug.LogError("Tried to add " + id + " to the Codex screen multiple times");
		}
		CodexCache.entries.Add(id, entry);
		entry.id = id;
		if (entry.name == null)
		{
			entry.name = Strings.Get(entry.title);
		}
		if (!string.IsNullOrEmpty(entry.iconAssetName))
		{
			try
			{
				entry.icon = Assets.GetSprite(entry.iconAssetName);
				if (!entry.iconLockID.IsNullOrWhiteSpace())
				{
					entry.iconColor = (Game.Instance.unlocks.IsUnlocked(entry.iconLockID) ? Color.white : Color.black);
				}
				goto IL_16E;
			}
			catch
			{
				global::Debug.LogWarningFormat("Unable to get icon for asset name {0}", new object[]
				{
					entry.iconAssetName
				});
				goto IL_16E;
			}
		}
		if (!string.IsNullOrEmpty(entry.iconPrefabID))
		{
			try
			{
				entry.icon = Def.GetUISpriteFromMultiObjectAnim(Assets.GetPrefab(entry.iconPrefabID).GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "");
				if (!entry.iconLockID.IsNullOrWhiteSpace())
				{
					entry.iconColor = (Game.Instance.unlocks.IsUnlocked(entry.iconLockID) ? Color.white : Color.black);
				}
			}
			catch
			{
				global::Debug.LogWarningFormat("Unable to get icon for prefabID {0}", new object[]
				{
					entry.iconPrefabID
				});
			}
		}
		IL_16E:
		if (!entry.parentId.IsNullOrWhiteSpace() && CodexCache.entries.ContainsKey(entry.parentId))
		{
			(CodexCache.entries[entry.parentId] as CategoryEntry).entriesInCategory.Add(entry);
		}
		foreach (ContentContainer contentContainer in entry.contentContainers)
		{
			if (contentContainer.lockID != null)
			{
				CodexCache.AddLockLookup(contentContainer.lockID, entry.id);
			}
		}
		entry.contentContainers.RemoveAll((ContentContainer x) => !Game.IsCorrectDlcActiveForCurrentSave(x));
	}

	// Token: 0x060063F5 RID: 25589 RVA: 0x00254BB4 File Offset: 0x00252DB4
	public static void AddSubEntry(string id, SubEntry entry)
	{
	}

	// Token: 0x060063F6 RID: 25590 RVA: 0x00254BB6 File Offset: 0x00252DB6
	public static void MergeSubEntry(string id, SubEntry entry)
	{
	}

	// Token: 0x060063F7 RID: 25591 RVA: 0x00254BB8 File Offset: 0x00252DB8
	public static void MergeEntry(string id, CodexEntry entry)
	{
		id = CodexCache.FormatLinkID(entry.id);
		entry.id = id;
		CodexEntry codexEntry = CodexCache.entries[id];
		if (codexEntry.GetRequiredDlcIds() != null && entry.GetForbiddenDlcIds() != null)
		{
			DebugUtil.DevLogError("Codex Entry with id=" + id + " defines requiredDlcIds but the existing entry also specifies requiredDlcIds. This is currently not handled, please investigate.");
		}
		if (codexEntry.GetRequiredDlcIds() != null && entry.GetForbiddenDlcIds() != null)
		{
			DebugUtil.DevLogError("Codex Entry with id=" + id + " defines forbiddenDlcIds but the existing entry also specifies forbiddenDlcIds. This is currently not handled, please investigate.");
		}
		if (entry.requiredDlcIds != null)
		{
			codexEntry.requiredDlcIds = entry.requiredDlcIds;
		}
		if (entry.forbiddenDlcIds != null)
		{
			codexEntry.forbiddenDlcIds = entry.forbiddenDlcIds;
		}
		for (int i = 0; i < entry.log.modificationRecords.Count; i++)
		{
		}
		codexEntry.customContentLength = entry.contentContainers.Count;
		for (int j = entry.contentContainers.Count - 1; j >= 0; j--)
		{
			codexEntry.InsertContentContainer(entry.insertMergeContentAtBottom ? (codexEntry.contentContainers.Count - 1) : 0, entry.contentContainers[j]);
		}
		if (entry.disabled)
		{
			codexEntry.disabled = entry.disabled;
		}
		codexEntry.showBeforeGeneratedCategoryLinks = entry.showBeforeGeneratedCategoryLinks;
		if (!string.IsNullOrEmpty(entry.category))
		{
			codexEntry.category = entry.category;
		}
		if (!string.IsNullOrEmpty(entry.parentId))
		{
			codexEntry.parentId = entry.parentId;
		}
		foreach (ContentContainer contentContainer in entry.contentContainers)
		{
			if (contentContainer.lockID != null)
			{
				CodexCache.AddLockLookup(contentContainer.lockID, entry.id);
			}
		}
	}

	// Token: 0x060063F8 RID: 25592 RVA: 0x00254D74 File Offset: 0x00252F74
	public static void Clear()
	{
		CodexCache.entries = null;
		CodexCache.baseEntryPath = null;
	}

	// Token: 0x060063F9 RID: 25593 RVA: 0x00254D82 File Offset: 0x00252F82
	public static string GetEntryPath()
	{
		return CodexCache.baseEntryPath;
	}

	// Token: 0x060063FA RID: 25594 RVA: 0x00254D8C File Offset: 0x00252F8C
	public static CodexEntry GetTemplate(string templatePath)
	{
		if (!CodexCache.entries.ContainsKey(templatePath))
		{
			CodexCache.entries.Add(templatePath, null);
		}
		if (CodexCache.entries[templatePath] == null)
		{
			string text = Path.Combine(CodexCache.baseEntryPath, templatePath);
			CodexEntry codexEntry = YamlIO.LoadFile<CodexEntry>(text + ".yaml", null, CodexCache.widgetTagMappings);
			if (codexEntry == null)
			{
				global::Debug.LogWarning("Missing template [" + text + ".yaml]");
			}
			CodexCache.entries[templatePath] = codexEntry;
		}
		return CodexCache.entries[templatePath];
	}

	// Token: 0x060063FB RID: 25595 RVA: 0x00254E11 File Offset: 0x00253011
	private static void YamlParseErrorCB(YamlIO.Error error, bool force_log_as_warning)
	{
		throw new Exception(string.Format("{0} parse error in {1}\n{2}", error.severity, error.file.full_path, error.message), error.inner_exception);
	}

	// Token: 0x060063FC RID: 25596 RVA: 0x00254E44 File Offset: 0x00253044
	public static List<CodexEntry> CollectEntries(string folder)
	{
		List<CodexEntry> list = new List<CodexEntry>();
		string path = (folder == "") ? CodexCache.baseEntryPath : Path.Combine(CodexCache.baseEntryPath, folder);
		string[] array = new string[0];
		try
		{
			array = Directory.GetFiles(path, "*.yaml");
		}
		catch (UnauthorizedAccessException obj)
		{
			global::Debug.LogWarning(obj);
		}
		string category = folder.ToUpper();
		foreach (string text in array)
		{
			if (!CodexCache.IsSubEntryAtPath(text))
			{
				try
				{
					CodexEntry codexEntry = YamlIO.LoadFile<CodexEntry>(text, new YamlIO.ErrorHandler(CodexCache.YamlParseErrorCB), CodexCache.widgetTagMappings);
					if (codexEntry != null)
					{
						codexEntry.category = category;
						list.Add(codexEntry);
					}
				}
				catch (Exception ex)
				{
					DebugUtil.DevLogErrorFormat("CodexCache.CollectEntries failed to load [{0}]: {1}", new object[]
					{
						text,
						ex.ToString()
					});
				}
			}
		}
		foreach (CodexEntry codexEntry2 in list)
		{
			if (string.IsNullOrEmpty(codexEntry2.sortString))
			{
				codexEntry2.sortString = Strings.Get(codexEntry2.title);
			}
		}
		list.Sort((CodexEntry x, CodexEntry y) => x.sortString.CompareTo(y.sortString));
		return list;
	}

	// Token: 0x060063FD RID: 25597 RVA: 0x00254FB8 File Offset: 0x002531B8
	public static List<SubEntry> CollectSubEntries(string folder)
	{
		List<SubEntry> list = new List<SubEntry>();
		string path = (folder == "") ? CodexCache.baseEntryPath : Path.Combine(CodexCache.baseEntryPath, folder);
		string[] array = new string[0];
		try
		{
			array = Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories);
		}
		catch (UnauthorizedAccessException obj)
		{
			global::Debug.LogWarning(obj);
		}
		foreach (string text in array)
		{
			if (CodexCache.IsSubEntryAtPath(text))
			{
				try
				{
					SubEntry subEntry = YamlIO.LoadFile<SubEntry>(text, new YamlIO.ErrorHandler(CodexCache.YamlParseErrorCB), CodexCache.widgetTagMappings);
					if (subEntry != null)
					{
						list.Add(subEntry);
					}
				}
				catch (Exception ex)
				{
					DebugUtil.DevLogErrorFormat("CodexCache.CollectSubEntries failed to load [{0}]: {1}", new object[]
					{
						text,
						ex.ToString()
					});
				}
			}
		}
		list.Sort((SubEntry x, SubEntry y) => x.title.CompareTo(y.title));
		return list;
	}

	// Token: 0x060063FE RID: 25598 RVA: 0x002550BC File Offset: 0x002532BC
	public static bool IsSubEntryAtPath(string path)
	{
		return Path.GetFileName(path).Contains("SubEntry");
	}

	// Token: 0x040043FD RID: 17405
	private static string baseEntryPath = null;

	// Token: 0x040043FE RID: 17406
	public static Dictionary<string, CodexEntry> entries = null;

	// Token: 0x040043FF RID: 17407
	public static Dictionary<string, SubEntry> subEntries = null;

	// Token: 0x04004400 RID: 17408
	private static Dictionary<string, List<string>> unlockedEntryLookup;

	// Token: 0x04004401 RID: 17409
	private static List<global::Tuple<string, Type>> widgetTagMappings;

	// Token: 0x04004402 RID: 17410
	public static List<CategoryEntry> categoriesForPostYAMLPopulation = new List<CategoryEntry>();
}
