using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C8B RID: 3211
public class AllResourcesScreen : ShowOptimizedKScreen, ISim4000ms, ISim1000ms
{
	// Token: 0x0600627E RID: 25214 RVA: 0x00247FB7 File Offset: 0x002461B7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AllResourcesScreen.Instance = this;
	}

	// Token: 0x0600627F RID: 25215 RVA: 0x00247FC5 File Offset: 0x002461C5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.Init();
	}

	// Token: 0x06006280 RID: 25216 RVA: 0x00247FDA File Offset: 0x002461DA
	protected override void OnForcedCleanUp()
	{
		AllResourcesScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006281 RID: 25217 RVA: 0x00247FE8 File Offset: 0x002461E8
	public void SetFilter(string filter)
	{
		if (string.IsNullOrEmpty(filter))
		{
			filter = "";
		}
		this.searchInputField.text = filter;
	}

	// Token: 0x06006282 RID: 25218 RVA: 0x00248008 File Offset: 0x00246208
	public void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		DiscoveredResources.Instance.OnDiscover += delegate(Tag a, Tag b)
		{
			this.Populate(null);
		};
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		this.searchInputField.OnValueChangesPaused = delegate()
		{
			this.SearchFilter(this.searchInputField.text);
		};
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.Show(false);
	}

	// Token: 0x06006283 RID: 25219 RVA: 0x002480ED File Offset: 0x002462ED
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			ManagementMenu.Instance.CloseAll();
			AllDiagnosticsScreen.Instance.Show(false);
			this.RefreshRows();
			return;
		}
		this.SetFilter(null);
	}

	// Token: 0x06006284 RID: 25220 RVA: 0x0024811C File Offset: 0x0024631C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006285 RID: 25221 RVA: 0x0024815C File Offset: 0x0024635C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06006286 RID: 25222 RVA: 0x00248199 File Offset: 0x00246399
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06006287 RID: 25223 RVA: 0x002481A0 File Offset: 0x002463A0
	public void Populate(object data = null)
	{
		this.SpawnRows();
	}

	// Token: 0x06006288 RID: 25224 RVA: 0x002481A8 File Offset: 0x002463A8
	private void SpawnRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		this.allowDisplayCategories.Add(GameTags.MaterialCategories);
		this.allowDisplayCategories.Add(GameTags.CalorieCategories);
		this.allowDisplayCategories.Add(GameTags.UnitCategories);
		foreach (Tag categoryTag in GameTags.MaterialCategories)
		{
			this.SpawnCategoryRow(categoryTag, GameUtil.MeasureUnit.mass);
		}
		foreach (Tag categoryTag2 in GameTags.CalorieCategories)
		{
			this.SpawnCategoryRow(categoryTag2, GameUtil.MeasureUnit.kcal);
		}
		foreach (Tag categoryTag3 in GameTags.UnitCategories)
		{
			this.SpawnCategoryRow(categoryTag3, GameUtil.MeasureUnit.quantity);
		}
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort((Tag a, Tag b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink()));
		foreach (Tag key in list)
		{
			this.categoryRows[key].GameObject.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06006289 RID: 25225 RVA: 0x00248384 File Offset: 0x00246584
	private void SpawnCategoryRow(Tag categoryTag, GameUtil.MeasureUnit unit)
	{
		if (!this.categoryRows.ContainsKey(categoryTag))
		{
			GameObject gameObject = Util.KInstantiateUI(this.categoryLinePrefab, this.rootListContainer, true);
			AllResourcesScreen.CategoryRow categoryRow = new AllResourcesScreen.CategoryRow(categoryTag, gameObject);
			gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(categoryTag.ProperNameStripLink());
			this.categoryRows.Add(categoryTag, categoryRow);
			this.currentlyDisplayedRows.Add(categoryTag, true);
			this.units.Add(categoryTag, unit);
			GraphBase component = categoryRow.sparkLayer.GetComponent<GraphBase>();
			component.axis_x.min_value = 0f;
			component.axis_x.max_value = 600f;
			component.axis_x.guide_frequency = 120f;
			component.RefreshGuides();
		}
		GameObject container = this.categoryRows[categoryTag].FoldOutPanel.container;
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(categoryTag))
		{
			if (!this.resourceRows.ContainsKey(tag))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.resourceLinePrefab, container, true);
				HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
				component2.GetReference<Image>("Icon").sprite = uisprite.first;
				component2.GetReference<Image>("Icon").color = uisprite.second;
				component2.GetReference<LocText>("NameLabel").SetText(tag.ProperNameStripLink());
				Tag targetTag = tag;
				MultiToggle pinToggle = component2.GetReference<MultiToggle>("PinToggle");
				MultiToggle pinToggle2 = pinToggle;
				pinToggle2.onClick = (System.Action)Delegate.Combine(pinToggle2.onClick, new System.Action(delegate()
				{
					if (ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag))
					{
						ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Remove(targetTag);
					}
					else
					{
						ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Add(targetTag);
						if (DiscoveredResources.Instance.newDiscoveries.ContainsKey(targetTag))
						{
							DiscoveredResources.Instance.newDiscoveries.Remove(targetTag);
						}
					}
					this.RefreshPinnedState(targetTag);
					pinToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag) ? 1 : 0);
				}));
				gameObject2.GetComponent<MultiToggle>().onClick = pinToggle.onClick;
				MultiToggle notifyToggle = component2.GetReference<MultiToggle>("NotificationToggle");
				MultiToggle notifyToggle2 = notifyToggle;
				notifyToggle2.onClick = (System.Action)Delegate.Combine(notifyToggle2.onClick, new System.Action(delegate()
				{
					if (ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag))
					{
						ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Remove(targetTag);
					}
					else
					{
						ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Add(targetTag);
					}
					this.RefreshPinnedState(targetTag);
					notifyToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag) ? 1 : 0);
				}));
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.min_value = 0f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.max_value = 600f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.guide_frequency = 120f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().RefreshGuides();
				AllResourcesScreen.ResourceRow value = new AllResourcesScreen.ResourceRow(tag, gameObject2);
				this.resourceRows.Add(tag, value);
				this.currentlyDisplayedRows.Add(tag, true);
				if (this.units.ContainsKey(tag))
				{
					global::Debug.LogError(string.Concat(new string[]
					{
						"Trying to add ",
						tag.ToString(),
						":UnitType ",
						this.units[tag].ToString(),
						" but units dictionary already has key ",
						tag.ToString(),
						" with unit type:",
						unit.ToString()
					}));
				}
				else
				{
					this.units.Add(tag, unit);
				}
			}
		}
	}

	// Token: 0x0600628A RID: 25226 RVA: 0x0024871C File Offset: 0x0024691C
	private void FilterRowBySearch(Tag tag, string filter)
	{
		this.currentlyDisplayedRows[tag] = this.PassesSearchFilter(tag, filter);
	}

	// Token: 0x0600628B RID: 25227 RVA: 0x00248734 File Offset: 0x00246934
	private void SearchFilter(string search)
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair in this.resourceRows)
		{
			this.FilterRowBySearch(keyValuePair.Key, search);
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair2 in this.categoryRows)
		{
			if (this.PassesSearchFilter(keyValuePair2.Key, search))
			{
				this.currentlyDisplayedRows[keyValuePair2.Key] = true;
				using (HashSet<Tag>.Enumerator enumerator3 = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(keyValuePair2.Key).GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Tag key = enumerator3.Current;
						if (this.currentlyDisplayedRows.ContainsKey(key))
						{
							this.currentlyDisplayedRows[key] = true;
						}
					}
					continue;
				}
			}
			this.currentlyDisplayedRows[keyValuePair2.Key] = false;
		}
		this.EnableCategoriesByActiveChildren();
		this.SetRowsActive();
	}

	// Token: 0x0600628C RID: 25228 RVA: 0x00248878 File Offset: 0x00246A78
	private bool PassesSearchFilter(Tag tag, string filter)
	{
		filter = filter.ToUpper();
		string text = tag.ProperNameStripLink().ToUpper();
		return !(filter != "") || text.Contains(filter);
	}

	// Token: 0x0600628D RID: 25229 RVA: 0x002488B4 File Offset: 0x00246AB4
	private void EnableCategoriesByActiveChildren()
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(keyValuePair.Key).Count == 0)
			{
				this.currentlyDisplayedRows[keyValuePair.Key] = false;
			}
			else
			{
				GameObject container = keyValuePair.Value.GameObject.GetComponent<FoldOutPanel>().container;
				foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
				{
					if (!(keyValuePair2.Value.GameObject.transform.parent.gameObject != container))
					{
						this.currentlyDisplayedRows[keyValuePair.Key] = (this.currentlyDisplayedRows[keyValuePair.Key] || this.currentlyDisplayedRows[keyValuePair2.Key]);
					}
				}
			}
		}
	}

	// Token: 0x0600628E RID: 25230 RVA: 0x002489E8 File Offset: 0x00246BE8
	private void RefreshPinnedState(Tag tag)
	{
		this.resourceRows[tag].notificiationToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(tag) ? 1 : 0);
		this.resourceRows[tag].pinToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tag) ? 1 : 0);
	}

	// Token: 0x0600628F RID: 25231 RVA: 0x00248A64 File Offset: 0x00246C64
	public void RefreshRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		this.EnableCategoriesByActiveChildren();
		this.SetRowsActive();
		if (this.allowRefresh)
		{
			foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
			{
				if (keyValuePair.Value.GameObject.activeInHierarchy)
				{
					float amount = worldInventory.GetAmount(keyValuePair.Key, false);
					float totalAmount = worldInventory.GetTotalAmount(keyValuePair.Key, false);
					if (!worldInventory.HasValidCount)
					{
						keyValuePair.Value.availableLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair.Value.totalLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair.Value.reservedLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
					}
					else
					{
						switch (this.units[keyValuePair.Key])
						{
						case GameUtil.MeasureUnit.mass:
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedMass(totalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedMass(totalAmount - amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							break;
						case GameUtil.MeasureUnit.kcal:
						{
							float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.activeWorld.worldInventory, true);
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedCalories(totalAmount, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedCalories(totalAmount - amount, GameUtil.TimeSlice.None, true));
							}
							break;
						}
						case GameUtil.MeasureUnit.quantity:
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedUnits(amount, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedUnits(totalAmount, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedUnits(totalAmount - amount, GameUtil.TimeSlice.None, true, ""));
							}
							break;
						}
					}
				}
			}
			foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
			{
				if (keyValuePair2.Value.GameObject.activeInHierarchy)
				{
					float amount2 = worldInventory.GetAmount(keyValuePair2.Key, false);
					float totalAmount2 = worldInventory.GetTotalAmount(keyValuePair2.Key, false);
					if (!worldInventory.HasValidCount)
					{
						keyValuePair2.Value.availableLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair2.Value.totalLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair2.Value.reservedLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
					}
					else
					{
						switch (this.units[keyValuePair2.Key])
						{
						case GameUtil.MeasureUnit.mass:
							if (keyValuePair2.Value.CheckAvailableAmountChanged(amount2, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedMass(amount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedMass(totalAmount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedMass(totalAmount2 - amount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							break;
						case GameUtil.MeasureUnit.kcal:
						{
							float num = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(keyValuePair2.Key.Name, ClusterManager.Instance.activeWorld.worldInventory, true);
							if (keyValuePair2.Value.CheckAvailableAmountChanged(num, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedCalories(totalAmount2, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedCalories(totalAmount2 - amount2, GameUtil.TimeSlice.None, true));
							}
							break;
						}
						case GameUtil.MeasureUnit.quantity:
							if (keyValuePair2.Value.CheckAvailableAmountChanged(amount2, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedUnits(amount2, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedUnits(totalAmount2, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedUnits(totalAmount2 - amount2, GameUtil.TimeSlice.None, true, ""));
							}
							break;
						}
					}
					this.RefreshPinnedState(keyValuePair2.Key);
				}
			}
		}
	}

	// Token: 0x06006290 RID: 25232 RVA: 0x00249098 File Offset: 0x00247298
	public int UniqueResourceRowCount()
	{
		return this.resourceRows.Count;
	}

	// Token: 0x06006291 RID: 25233 RVA: 0x002490A8 File Offset: 0x002472A8
	private void RefreshCharts()
	{
		float time = GameClock.Instance.GetTime();
		float num = 3000f;
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, keyValuePair.Key);
			if (resourceStatistic != null)
			{
				SparkLayer sparkLayer = keyValuePair.Value.sparkLayer;
				global::Tuple<float, float>[] array = resourceStatistic.ChartableData(num);
				if (array.Length != 0)
				{
					sparkLayer.graph.axis_x.max_value = array[array.Length - 1].first;
				}
				else
				{
					sparkLayer.graph.axis_x.max_value = 0f;
				}
				sparkLayer.graph.axis_x.min_value = time - num;
				sparkLayer.RefreshLine(array, "resourceAmount");
			}
			else
			{
				DebugUtil.DevLogError("DevError: No tracker found for resource category " + keyValuePair.Key.ToString());
			}
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
		{
			if (keyValuePair2.Value.GameObject.activeInHierarchy)
			{
				ResourceTracker resourceStatistic2 = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, keyValuePair2.Key);
				if (resourceStatistic2 != null)
				{
					SparkLayer sparkLayer2 = keyValuePair2.Value.sparkLayer;
					global::Tuple<float, float>[] array2 = resourceStatistic2.ChartableData(num);
					if (array2.Length != 0)
					{
						sparkLayer2.graph.axis_x.max_value = array2[array2.Length - 1].first;
					}
					else
					{
						sparkLayer2.graph.axis_x.max_value = 0f;
					}
					sparkLayer2.graph.axis_x.min_value = time - num;
					sparkLayer2.RefreshLine(array2, "resourceAmount");
				}
				else
				{
					DebugUtil.DevLogError("DevError: No tracker found for resource " + keyValuePair2.Key.ToString());
				}
			}
		}
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x002492E0 File Offset: 0x002474E0
	private void SetRowsActive()
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			if (keyValuePair.Value.GameObject.activeSelf != this.currentlyDisplayedRows[keyValuePair.Key])
			{
				keyValuePair.Value.GameObject.SetActive(this.currentlyDisplayedRows[keyValuePair.Key]);
			}
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
		{
			if (keyValuePair2.Value.GameObject.activeSelf != this.currentlyDisplayedRows[keyValuePair2.Key])
			{
				keyValuePair2.Value.GameObject.SetActive(this.currentlyDisplayedRows[keyValuePair2.Key]);
				if (!this.currentlyDisplayedRows[keyValuePair2.Key] && keyValuePair2.Value.horizontalLayoutGroup.enabled)
				{
					keyValuePair2.Value.horizontalLayoutGroup.enabled = false;
				}
			}
		}
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x00249438 File Offset: 0x00247638
	public void Sim4000ms(float dt)
	{
		if (!this.IsScreenActive())
		{
			return;
		}
		this.RefreshCharts();
	}

	// Token: 0x06006294 RID: 25236 RVA: 0x00249449 File Offset: 0x00247649
	public void Sim1000ms(float dt)
	{
		if (!this.IsScreenActive())
		{
			return;
		}
		this.RefreshRows();
	}

	// Token: 0x040042F0 RID: 17136
	private Dictionary<Tag, AllResourcesScreen.ResourceRow> resourceRows = new Dictionary<Tag, AllResourcesScreen.ResourceRow>();

	// Token: 0x040042F1 RID: 17137
	private Dictionary<Tag, AllResourcesScreen.CategoryRow> categoryRows = new Dictionary<Tag, AllResourcesScreen.CategoryRow>();

	// Token: 0x040042F2 RID: 17138
	public Dictionary<Tag, GameUtil.MeasureUnit> units = new Dictionary<Tag, GameUtil.MeasureUnit>();

	// Token: 0x040042F3 RID: 17139
	public GameObject rootListContainer;

	// Token: 0x040042F4 RID: 17140
	public GameObject resourceLinePrefab;

	// Token: 0x040042F5 RID: 17141
	public GameObject categoryLinePrefab;

	// Token: 0x040042F6 RID: 17142
	public KButton closeButton;

	// Token: 0x040042F7 RID: 17143
	public bool allowRefresh = true;

	// Token: 0x040042F8 RID: 17144
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x040042F9 RID: 17145
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x040042FA RID: 17146
	public static AllResourcesScreen Instance;

	// Token: 0x040042FB RID: 17147
	public Dictionary<Tag, bool> currentlyDisplayedRows = new Dictionary<Tag, bool>();

	// Token: 0x040042FC RID: 17148
	public List<TagSet> allowDisplayCategories = new List<TagSet>();

	// Token: 0x040042FD RID: 17149
	private bool initialized;

	// Token: 0x02001EC0 RID: 7872
	private class ScreenRowBase
	{
		// Token: 0x0600B496 RID: 46230 RVA: 0x003EBDB8 File Offset: 0x003E9FB8
		public ScreenRowBase(Tag tag, GameObject gameObject)
		{
			this.Tag = tag;
			this.GameObject = gameObject;
			HierarchyReferences component = this.GameObject.GetComponent<HierarchyReferences>();
			this.availableLabel = component.GetReference<LocText>("AvailableLabel");
			this.totalLabel = component.GetReference<LocText>("TotalLabel");
			this.reservedLabel = component.GetReference<LocText>("ReservedLabel");
			this.sparkLayer = component.GetReference<SparkLayer>("Chart");
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x0600B497 RID: 46231 RVA: 0x003EBE4A File Offset: 0x003EA04A
		// (set) Token: 0x0600B498 RID: 46232 RVA: 0x003EBE52 File Offset: 0x003EA052
		public Tag Tag { get; private set; }

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x0600B499 RID: 46233 RVA: 0x003EBE5B File Offset: 0x003EA05B
		// (set) Token: 0x0600B49A RID: 46234 RVA: 0x003EBE63 File Offset: 0x003EA063
		public GameObject GameObject { get; private set; }

		// Token: 0x0600B49B RID: 46235 RVA: 0x003EBE6C File Offset: 0x003EA06C
		public bool CheckAvailableAmountChanged(float newAvailableResourceAmount, bool updateIfTrue)
		{
			bool flag = newAvailableResourceAmount != this.oldAvailableResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldAvailableResourceAmount = newAvailableResourceAmount;
			}
			return flag;
		}

		// Token: 0x0600B49C RID: 46236 RVA: 0x003EBE86 File Offset: 0x003EA086
		public bool CheckTotalResourceAmountChanged(float newTotalResourceAmount, bool updateIfTrue)
		{
			bool flag = newTotalResourceAmount != this.oldTotalResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldTotalResourceAmount = newTotalResourceAmount;
			}
			return flag;
		}

		// Token: 0x0600B49D RID: 46237 RVA: 0x003EBEA0 File Offset: 0x003EA0A0
		public bool CheckReservedResourceAmountChanged(float newReservedResourceAmount, bool updateIfTrue)
		{
			bool flag = newReservedResourceAmount != this.oldReserverResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldReserverResourceAmount = newReservedResourceAmount;
			}
			return flag;
		}

		// Token: 0x04009087 RID: 36999
		public LocText availableLabel;

		// Token: 0x04009088 RID: 37000
		public LocText totalLabel;

		// Token: 0x04009089 RID: 37001
		public LocText reservedLabel;

		// Token: 0x0400908A RID: 37002
		public SparkLayer sparkLayer;

		// Token: 0x0400908B RID: 37003
		private float oldAvailableResourceAmount = -1f;

		// Token: 0x0400908C RID: 37004
		private float oldTotalResourceAmount = -1f;

		// Token: 0x0400908D RID: 37005
		private float oldReserverResourceAmount = -1f;
	}

	// Token: 0x02001EC1 RID: 7873
	private class CategoryRow : AllResourcesScreen.ScreenRowBase
	{
		// Token: 0x0600B49E RID: 46238 RVA: 0x003EBEBA File Offset: 0x003EA0BA
		public CategoryRow(Tag tag, GameObject gameObject) : base(tag, gameObject)
		{
			this.FoldOutPanel = base.GameObject.GetComponent<FoldOutPanel>();
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x0600B49F RID: 46239 RVA: 0x003EBED5 File Offset: 0x003EA0D5
		// (set) Token: 0x0600B4A0 RID: 46240 RVA: 0x003EBEDD File Offset: 0x003EA0DD
		public FoldOutPanel FoldOutPanel { get; private set; }
	}

	// Token: 0x02001EC2 RID: 7874
	private class ResourceRow : AllResourcesScreen.ScreenRowBase
	{
		// Token: 0x0600B4A1 RID: 46241 RVA: 0x003EBEE8 File Offset: 0x003EA0E8
		public ResourceRow(Tag tag, GameObject gameObject) : base(tag, gameObject)
		{
			HierarchyReferences component = base.GameObject.GetComponent<HierarchyReferences>();
			this.notificiationToggle = component.GetReference<MultiToggle>("NotificationToggle");
			this.pinToggle = component.GetReference<MultiToggle>("PinToggle");
			this.horizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
		}

		// Token: 0x0400908F RID: 37007
		public MultiToggle notificiationToggle;

		// Token: 0x04009090 RID: 37008
		public MultiToggle pinToggle;

		// Token: 0x04009091 RID: 37009
		public HorizontalLayoutGroup horizontalLayoutGroup;
	}
}
