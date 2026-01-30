using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D85 RID: 3461
public class TableScreen : ShowOptimizedKScreen
{
	// Token: 0x06006B99 RID: 27545 RVA: 0x0028D439 File Offset: 0x0028B639
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.removeWorldHandle = ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorldDivider));
	}

	// Token: 0x06006B9A RID: 27546 RVA: 0x0028D464 File Offset: 0x0028B664
	protected override void OnActivate()
	{
		base.OnActivate();
		this.title_bar.text = this.title;
		base.ConsumeMouseScroll = true;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.incubating = true;
		base.transform.rectTransform().localScale = Vector3.zero;
		Components.LiveMinionIdentities.OnAdd += delegate(MinionIdentity param)
		{
			this.MarkRowsDirty();
		};
		Components.LiveMinionIdentities.OnRemove += delegate(MinionIdentity param)
		{
			this.MarkRowsDirty();
		};
	}

	// Token: 0x06006B9B RID: 27547 RVA: 0x0028D501 File Offset: 0x0028B701
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.removeWorldHandle != -1)
		{
			ClusterManager.Instance.Unsubscribe(this.removeWorldHandle);
		}
	}

	// Token: 0x06006B9C RID: 27548 RVA: 0x0028D522 File Offset: 0x0028B722
	protected override void OnShow(bool show)
	{
		if (!show)
		{
			this.active_cascade_coroutine_count = 0;
			base.StopAllCoroutines();
			this.StopLoopingCascadeSound();
		}
		this.ZeroScrollers();
		base.OnShow(show);
		if (show)
		{
			this.MarkRowsDirty();
		}
	}

	// Token: 0x06006B9D RID: 27549 RVA: 0x0028D550 File Offset: 0x0028B750
	private void ZeroScrollers()
	{
		if (this.rows.Count > 0)
		{
			foreach (string scrollerID in this.column_scrollers)
			{
				foreach (TableRow tableRow in this.rows)
				{
					ScrollRect component = tableRow.GetScroller(scrollerID).transform.parent.GetComponent<ScrollRect>();
					if (component.horizontalNormalizedPosition > 0.001f)
					{
						component.horizontalNormalizedPosition = 0f;
					}
				}
			}
			foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
			{
				ScrollRect reference = keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<ScrollRect>("ScrollerScrollRect");
				if (reference != null && reference.horizontalNormalizedPosition > 0.001f)
				{
					reference.horizontalNormalizedPosition = 0f;
				}
			}
		}
	}

	// Token: 0x06006B9E RID: 27550 RVA: 0x0028D690 File Offset: 0x0028B890
	public bool CheckScrollersDirty()
	{
		return this.scrollersDirty;
	}

	// Token: 0x06006B9F RID: 27551 RVA: 0x0028D698 File Offset: 0x0028B898
	public void SetScrollersDirty(float position)
	{
		this.targetScrollerPosition = position;
		this.scrollersDirty = true;
		this.PositionScrollers();
	}

	// Token: 0x06006BA0 RID: 27552 RVA: 0x0028D6B0 File Offset: 0x0028B8B0
	public void PositionScrollers()
	{
		foreach (TableRow tableRow in this.rows)
		{
			ScrollRect scroll_rect = tableRow.scroll_rect;
			if (scroll_rect != null)
			{
				scroll_rect.horizontalNormalizedPosition = this.targetScrollerPosition;
			}
		}
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
		{
			if (keyValuePair.Value.activeInHierarchy)
			{
				ScrollRect reference = keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<ScrollRect>("ScrollerScrollRect");
				if (reference != null)
				{
					reference.horizontalNormalizedPosition = this.targetScrollerPosition;
				}
			}
		}
		this.scrollersDirty = false;
	}

	// Token: 0x06006BA1 RID: 27553 RVA: 0x0028D798 File Offset: 0x0028B998
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.incubating)
		{
			this.ZeroScrollers();
			base.transform.rectTransform().localScale = Vector3.one;
			this.incubating = false;
		}
		if (this.rows_dirty)
		{
			this.RefreshRows();
		}
		foreach (TableRow tableRow in this.rows)
		{
			tableRow.RefreshScrollers();
		}
		foreach (TableColumn tableColumn in this.columns.Values)
		{
			if (tableColumn.isDirty)
			{
				foreach (KeyValuePair<TableRow, GameObject> keyValuePair in tableColumn.widgets_by_row)
				{
					tableColumn.on_load_action(keyValuePair.Key.GetIdentity(), keyValuePair.Value);
					tableColumn.MarkClean();
				}
			}
		}
	}

	// Token: 0x06006BA2 RID: 27554 RVA: 0x0028D8D0 File Offset: 0x0028BAD0
	protected void MarkRowsDirty()
	{
		this.rows_dirty = true;
	}

	// Token: 0x06006BA3 RID: 27555 RVA: 0x0028D8DC File Offset: 0x0028BADC
	protected virtual void RefreshRows()
	{
		this.ObsoleteRows();
		this.AddRow(null);
		if (this.has_default_duplicant_row)
		{
			this.AddDefaultRow();
		}
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			if (Components.LiveMinionIdentities[i] != null)
			{
				this.AddRow(Components.LiveMinionIdentities[i]);
			}
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity minion = info.serializedMinion.Get<StoredMinionIdentity>();
					this.AddRow(minion);
				}
			}
		}
		foreach (int worldId in ClusterManager.Instance.GetWorldIDsSorted())
		{
			this.AddWorldDivider(worldId);
		}
		this.AddWorldDivider(255);
		foreach (KeyValuePair<int, bool> keyValuePair in this.obsoleteWorldDividerStatus)
		{
			if (keyValuePair.Value)
			{
				Boxed<int> boxed = Boxed<int>.Get(keyValuePair.Key);
				this.RemoveWorldDivider(boxed);
				boxed.Release();
			}
		}
		this.obsoleteWorldDividerStatus.Clear();
		foreach (KeyValuePair<int, GameObject> keyValuePair2 in this.worldDividers)
		{
			Component reference = keyValuePair2.Value.GetComponent<HierarchyReferences>().GetReference("NobodyRow");
			bool flag = true;
			foreach (object obj in Components.MinionAssignablesProxy)
			{
				MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)obj;
				if (minionAssignablesProxy != null && minionAssignablesProxy.GetTargetGameObject() != null)
				{
					WorldContainer myWorld = minionAssignablesProxy.GetTargetGameObject().GetMyWorld();
					if (myWorld != null && myWorld.id == keyValuePair2.Key)
					{
						flag = false;
						break;
					}
					if (myWorld == null && keyValuePair2.Key == 255)
					{
						flag = false;
						break;
					}
				}
			}
			reference.gameObject.SetActive(flag);
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair2.Key);
			bool flag2 = DlcManager.FeatureClusterSpaceEnabled() && (world == null || ClusterManager.Instance.GetWorld(keyValuePair2.Key).IsDiscovered);
			if (world == null && flag)
			{
				flag2 = false;
			}
			if (keyValuePair2.Value.activeSelf != flag2)
			{
				keyValuePair2.Value.SetActive(flag2);
			}
		}
		using (Dictionary<IAssignableIdentity, bool>.Enumerator enumerator7 = this.obsoleteMinionRowStatus.GetEnumerator())
		{
			while (enumerator7.MoveNext())
			{
				KeyValuePair<IAssignableIdentity, bool> kvp = enumerator7.Current;
				if (kvp.Value)
				{
					int index = this.rows.FindIndex((TableRow match) => match.GetIdentity() == kvp.Key);
					TableRow item = this.rows[index];
					this.rows[index].Clear();
					this.rows.RemoveAt(index);
					this.all_sortable_rows.Remove(item);
				}
			}
		}
		this.obsoleteMinionRowStatus.Clear();
		this.SortRows();
		this.rows_dirty = false;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		foreach (TableRow tableRow in this.rows)
		{
			tableRow.PositionScrollerBorders();
		}
	}

	// Token: 0x06006BA4 RID: 27556 RVA: 0x0028DD9C File Offset: 0x0028BF9C
	public virtual void SetSortComparison(Comparison<IAssignableIdentity> comparison, TableColumn sort_column)
	{
		if (comparison == null)
		{
			return;
		}
		if (this.active_sort_column != sort_column)
		{
			this.active_sort_column = sort_column;
			this.active_sort_method = comparison;
			this.sort_is_reversed = false;
			return;
		}
		if (this.sort_is_reversed)
		{
			this.sort_is_reversed = false;
			this.active_sort_method = null;
			this.active_sort_column = null;
			return;
		}
		this.sort_is_reversed = true;
	}

	// Token: 0x06006BA5 RID: 27557 RVA: 0x0028DDF4 File Offset: 0x0028BFF4
	public void SortRows()
	{
		foreach (TableColumn tableColumn in this.columns.Values)
		{
			if (!(tableColumn.column_sort_toggle == null))
			{
				if (tableColumn == this.active_sort_column)
				{
					if (this.sort_is_reversed)
					{
						tableColumn.column_sort_toggle.ChangeState(2);
					}
					else
					{
						tableColumn.column_sort_toggle.ChangeState(1);
					}
				}
				else
				{
					tableColumn.column_sort_toggle.ChangeState(0);
				}
			}
		}
		Dictionary<IAssignableIdentity, TableRow> dictionary = new Dictionary<IAssignableIdentity, TableRow>();
		foreach (TableRow tableRow in this.all_sortable_rows)
		{
			dictionary.Add(tableRow.GetIdentity(), tableRow);
		}
		Dictionary<int, List<IAssignableIdentity>> dictionary2 = new Dictionary<int, List<IAssignableIdentity>>();
		foreach (KeyValuePair<IAssignableIdentity, TableRow> keyValuePair in dictionary)
		{
			WorldContainer myWorld = keyValuePair.Key.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld();
			int key = 255;
			if (myWorld != null)
			{
				key = myWorld.id;
			}
			if (!dictionary2.ContainsKey(key))
			{
				dictionary2.Add(key, new List<IAssignableIdentity>());
			}
			dictionary2[key].Add(keyValuePair.Key);
		}
		this.all_sortable_rows.Clear();
		Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<int, List<IAssignableIdentity>> keyValuePair2 in dictionary2)
		{
			dictionary3.Add(keyValuePair2.Key, num);
			num++;
			List<IAssignableIdentity> list = new List<IAssignableIdentity>();
			foreach (IAssignableIdentity item in keyValuePair2.Value)
			{
				list.Add(item);
			}
			if (this.active_sort_method != null)
			{
				list.Sort(this.active_sort_method);
				if (this.sort_is_reversed)
				{
					list.Reverse();
				}
			}
			num += list.Count;
			num2 += list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				this.all_sortable_rows.Add(dictionary[list[i]]);
			}
		}
		for (int j = 0; j < this.all_sortable_rows.Count; j++)
		{
			this.all_sortable_rows[j].gameObject.transform.SetSiblingIndex(j);
		}
		foreach (KeyValuePair<int, int> keyValuePair3 in dictionary3)
		{
			this.worldDividers[keyValuePair3.Key].transform.SetSiblingIndex(keyValuePair3.Value);
		}
		if (this.has_default_duplicant_row)
		{
			this.default_row.transform.SetAsFirstSibling();
		}
	}

	// Token: 0x06006BA6 RID: 27558 RVA: 0x0028E154 File Offset: 0x0028C354
	protected int compare_rows_alphabetical(IAssignableIdentity a, IAssignableIdentity b)
	{
		if (a == null && b == null)
		{
			return 0;
		}
		if (a == null)
		{
			return -1;
		}
		if (b == null)
		{
			return 1;
		}
		return a.GetProperName().CompareTo(b.GetProperName());
	}

	// Token: 0x06006BA7 RID: 27559 RVA: 0x0028E179 File Offset: 0x0028C379
	protected int default_sort(TableRow a, TableRow b)
	{
		return 0;
	}

	// Token: 0x06006BA8 RID: 27560 RVA: 0x0028E17C File Offset: 0x0028C37C
	protected void ObsoleteRows()
	{
		for (int i = this.rows.Count - 1; i >= 0; i--)
		{
			IAssignableIdentity identity = this.rows[i].GetIdentity();
			if (identity != null)
			{
				this.obsoleteMinionRowStatus.Add(identity, true);
			}
		}
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
		{
			this.obsoleteWorldDividerStatus.Add(keyValuePair.Key, true);
		}
	}

	// Token: 0x06006BA9 RID: 27561 RVA: 0x0028E218 File Offset: 0x0028C418
	protected void AddRow(IAssignableIdentity minion)
	{
		bool flag = minion == null;
		if (!flag && this.obsoleteMinionRowStatus.ContainsKey(minion))
		{
			this.obsoleteMinionRowStatus[minion] = false;
			this.rows.Find((TableRow match) => match.GetIdentity() == minion).RefreshColumns(this.columns);
			return;
		}
		if (flag && this.header_row != null)
		{
			this.header_row.GetComponent<TableRow>().RefreshColumns(this.columns);
			return;
		}
		GameObject gameObject = Util.KInstantiateUI(flag ? this.prefab_row_header : this.prefab_row_empty, (minion == null) ? this.header_content_transform.gameObject : this.scroll_content_transform.gameObject, true);
		TableRow component = gameObject.GetComponent<TableRow>();
		component.rowType = (flag ? TableRow.RowType.Header : ((minion as MinionIdentity != null) ? TableRow.RowType.Minion : TableRow.RowType.StoredMinon));
		this.rows.Add(component);
		component.ConfigureContent(minion, this.columns, this);
		if (!flag)
		{
			this.all_sortable_rows.Add(component);
			return;
		}
		this.header_row = gameObject;
	}

	// Token: 0x06006BAA RID: 27562 RVA: 0x0028E348 File Offset: 0x0028C548
	protected void AddDefaultRow()
	{
		if (this.default_row != null)
		{
			this.default_row.GetComponent<TableRow>().RefreshColumns(this.columns);
			return;
		}
		GameObject gameObject = Util.KInstantiateUI(this.prefab_row_empty, this.scroll_content_transform.gameObject, true);
		this.default_row = gameObject;
		TableRow component = gameObject.GetComponent<TableRow>();
		component.rowType = TableRow.RowType.Default;
		component.isDefault = true;
		this.rows.Add(component);
		component.ConfigureContent(null, this.columns, this);
	}

	// Token: 0x06006BAB RID: 27563 RVA: 0x0028E3C8 File Offset: 0x0028C5C8
	protected void AddWorldDivider(int worldId)
	{
		if (this.obsoleteWorldDividerStatus.ContainsKey(worldId) && this.obsoleteWorldDividerStatus[worldId])
		{
			this.obsoleteWorldDividerStatus[worldId] = false;
			return;
		}
		GameObject gameObject = Util.KInstantiateUI(this.prefab_world_divider, this.scroll_content_transform.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("RowBG").color = ClusterManager.worldColors[worldId % ClusterManager.worldColors.Length];
		LocText reference = component.GetReference<LocText>("NameLabel");
		RectTransform rectTransform = reference.rectTransform();
		rectTransform.sizeDelta = new Vector2(150f, rectTransform.sizeDelta.y);
		WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
		if (world != null)
		{
			ClusterGridEntity component2 = world.GetComponent<ClusterGridEntity>();
			string str = (component2 is Clustercraft) ? NAMEGEN.WORLD.SPACECRAFT_PREFIX : NAMEGEN.WORLD.PLANETOID_PREFIX;
			reference.SetText(str + component2.Name);
			reference.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(NAMEGEN.WORLD.WORLDDIVIDER_TOOLTIP, component2.Name));
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = component2.GetUISprite();
		}
		else
		{
			reference.SetText(NAMEGEN.WORLD.UNKNOWN_WORLD);
			reference.GetComponent<ToolTip>().SetSimpleTooltip("");
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Assets.GetSprite("hex_unknown");
		}
		this.worldDividers.Add(worldId, gameObject);
		gameObject.GetComponent<TableRow>().ConfigureAsWorldDivider(this.columns, this);
	}

	// Token: 0x06006BAC RID: 27564 RVA: 0x0028E55C File Offset: 0x0028C75C
	protected void RemoveWorldDivider(object worldId)
	{
		int value = ((Boxed<int>)worldId).value;
		if (this.worldDividers.ContainsKey(value))
		{
			this.rows.Remove(this.worldDividers[value].GetComponent<TableRow>());
			Util.KDestroyGameObject(this.worldDividers[value]);
			this.worldDividers.Remove(value);
		}
	}

	// Token: 0x06006BAD RID: 27565 RVA: 0x0028E5C0 File Offset: 0x0028C7C0
	protected TableRow GetWidgetRow(GameObject widget_go)
	{
		if (widget_go == null)
		{
			global::Debug.LogWarning("Widget is null");
			return null;
		}
		if (this.known_widget_rows.ContainsKey(widget_go))
		{
			return this.known_widget_rows[widget_go];
		}
		foreach (TableRow tableRow in this.rows)
		{
			if (tableRow.rowType != TableRow.RowType.WorldDivider && tableRow.ContainsWidget(widget_go))
			{
				this.known_widget_rows.Add(widget_go, tableRow);
				return tableRow;
			}
		}
		global::Debug.LogWarning("Row is null for widget: " + widget_go.name + " parent is " + widget_go.transform.parent.name);
		return null;
	}

	// Token: 0x06006BAE RID: 27566 RVA: 0x0028E68C File Offset: 0x0028C88C
	protected void StartScrollableContent(string scrollablePanelID)
	{
		if (!this.column_scrollers.Contains(scrollablePanelID))
		{
			DividerColumn new_column = new DividerColumn(() => true, "");
			this.RegisterColumn("scroller_spacer_" + scrollablePanelID, new_column);
			this.column_scrollers.Add(scrollablePanelID);
		}
	}

	// Token: 0x06006BAF RID: 27567 RVA: 0x0028E6F0 File Offset: 0x0028C8F0
	protected PortraitTableColumn AddPortraitColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, bool double_click_to_target = true)
	{
		PortraitTableColumn portraitTableColumn = new PortraitTableColumn(on_load_action, sort_comparison, double_click_to_target);
		if (this.RegisterColumn(id, portraitTableColumn))
		{
			return portraitTableColumn;
		}
		return null;
	}

	// Token: 0x06006BB0 RID: 27568 RVA: 0x0028E714 File Offset: 0x0028C914
	protected ButtonLabelColumn AddButtonLabelColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Action<GameObject> on_click_action, Action<GameObject> on_double_click_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, bool whiteText = false)
	{
		ButtonLabelColumn buttonLabelColumn = new ButtonLabelColumn(on_load_action, get_value_action, on_click_action, on_double_click_action, sort_comparison, on_tooltip, on_sort_tooltip, whiteText);
		if (this.RegisterColumn(id, buttonLabelColumn))
		{
			return buttonLabelColumn;
		}
		return null;
	}

	// Token: 0x06006BB1 RID: 27569 RVA: 0x0028E744 File Offset: 0x0028C944
	protected LabelTableColumn AddLabelColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, int widget_width = 128, bool should_refresh_columns = false)
	{
		LabelTableColumn labelTableColumn = new LabelTableColumn(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, widget_width, should_refresh_columns);
		if (this.RegisterColumn(id, labelTableColumn))
		{
			return labelTableColumn;
		}
		return null;
	}

	// Token: 0x06006BB2 RID: 27570 RVA: 0x0028E770 File Offset: 0x0028C970
	protected CheckboxTableColumn AddCheckboxColumn(string id, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_function, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip)
	{
		CheckboxTableColumn checkboxTableColumn = new CheckboxTableColumn(on_load_action, get_value_action, on_press_action, set_value_function, sort_comparison, on_tooltip, on_sort_tooltip, null);
		if (this.RegisterColumn(id, checkboxTableColumn))
		{
			return checkboxTableColumn;
		}
		return null;
	}

	// Token: 0x06006BB3 RID: 27571 RVA: 0x0028E7A0 File Offset: 0x0028C9A0
	protected SuperCheckboxTableColumn AddSuperCheckboxColumn(string id, CheckboxTableColumn[] columns_affected, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = new SuperCheckboxTableColumn(columns_affected, on_load_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip);
		if (this.RegisterColumn(id, superCheckboxTableColumn))
		{
			foreach (CheckboxTableColumn checkboxTableColumn in columns_affected)
			{
				checkboxTableColumn.on_set_action = (Action<GameObject, TableScreen.ResultValues>)Delegate.Combine(checkboxTableColumn.on_set_action, new Action<GameObject, TableScreen.ResultValues>(superCheckboxTableColumn.MarkDirty));
			}
			superCheckboxTableColumn.MarkDirty(null, TableScreen.ResultValues.False);
			return superCheckboxTableColumn;
		}
		global::Debug.LogWarning("SuperCheckbox column registration failed");
		return null;
	}

	// Token: 0x06006BB4 RID: 27572 RVA: 0x0028E814 File Offset: 0x0028CA14
	protected NumericDropDownTableColumn AddNumericDropDownColumn(string id, object user_data, List<TMP_Dropdown.OptionData> options, Action<IAssignableIdentity, GameObject> on_load_action, Action<GameObject, int> set_value_action, Comparison<IAssignableIdentity> sort_comparison, NumericDropDownTableColumn.ToolTipCallbacks tooltip_callbacks)
	{
		NumericDropDownTableColumn numericDropDownTableColumn = new NumericDropDownTableColumn(user_data, options, on_load_action, set_value_action, sort_comparison, tooltip_callbacks, null);
		if (this.RegisterColumn(id, numericDropDownTableColumn))
		{
			return numericDropDownTableColumn;
		}
		return null;
	}

	// Token: 0x06006BB5 RID: 27573 RVA: 0x0028E83F File Offset: 0x0028CA3F
	protected bool RegisterColumn(string id, TableColumn new_column)
	{
		if (this.columns.ContainsKey(id))
		{
			global::Debug.LogWarning(string.Format("Column with id {0} already in dictionary", id));
			return false;
		}
		new_column.screen = this;
		this.columns.Add(id, new_column);
		this.MarkRowsDirty();
		return true;
	}

	// Token: 0x06006BB6 RID: 27574 RVA: 0x0028E87C File Offset: 0x0028CA7C
	protected TableColumn GetWidgetColumn(GameObject widget_go)
	{
		if (this.known_widget_columns.ContainsKey(widget_go))
		{
			return this.known_widget_columns[widget_go];
		}
		foreach (KeyValuePair<string, TableColumn> keyValuePair in this.columns)
		{
			if (keyValuePair.Value.ContainsWidget(widget_go))
			{
				this.known_widget_columns.Add(widget_go, keyValuePair.Value);
				return keyValuePair.Value;
			}
		}
		global::Debug.LogWarning("No column found for widget gameobject " + widget_go.name);
		return null;
	}

	// Token: 0x06006BB7 RID: 27575 RVA: 0x0028E928 File Offset: 0x0028CB28
	protected void on_load_portrait(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		CrewPortrait component = widget_go.GetComponent<CrewPortrait>();
		if (minion != null)
		{
			component.SetIdentityObject(minion, false);
			component.ForceRefresh();
			return;
		}
		component.targetImage.enabled = (widgetRow.rowType == TableRow.RowType.Default);
	}

	// Token: 0x06006BB8 RID: 27576 RVA: 0x0028E96C File Offset: 0x0028CB6C
	protected void on_load_name_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		LocText locText = null;
		HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
		LocText locText2 = component.GetReference("Label") as LocText;
		if (component.HasReference("SubLabel"))
		{
			locText = (component.GetReference("SubLabel") as LocText);
		}
		if (minion != null)
		{
			locText2.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			if (locText != null)
			{
				MinionIdentity minionIdentity = minion as MinionIdentity;
				if (minionIdentity != null)
				{
					locText.text = minionIdentity.gameObject.GetComponent<MinionResume>().GetSkillsSubtitle();
				}
				else
				{
					locText.text = "";
				}
				locText.enableWordWrapping = false;
				return;
			}
		}
		else
		{
			if (widgetRow.isDefault)
			{
				locText2.text = UI.JOBSCREEN_DEFAULT;
				if (locText != null && locText.gameObject.activeSelf)
				{
					locText.gameObject.SetActive(false);
				}
			}
			else
			{
				locText2.text = UI.JOBSCREEN_EVERYONE;
			}
			if (locText != null)
			{
				locText.text = "";
			}
		}
	}

	// Token: 0x06006BB9 RID: 27577 RVA: 0x0028EA86 File Offset: 0x0028CC86
	protected string get_value_name_label(IAssignableIdentity minion, GameObject widget_go)
	{
		return minion.GetProperName();
	}

	// Token: 0x06006BBA RID: 27578 RVA: 0x0028EA90 File Offset: 0x0028CC90
	protected void on_load_value_checkbox_column_super(IAssignableIdentity minion, GameObject widget_go)
	{
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		TableRow.RowType rowType = this.GetWidgetRow(widget_go).rowType;
		if (rowType <= TableRow.RowType.Minion)
		{
			component.ChangeState((int)this.get_value_checkbox_column_super(minion, widget_go));
		}
	}

	// Token: 0x06006BBB RID: 27579 RVA: 0x0028EAC8 File Offset: 0x0028CCC8
	public virtual TableScreen.ResultValues get_value_checkbox_column_super(IAssignableIdentity minion, GameObject widget_go)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		bool flag4 = false;
		foreach (CheckboxTableColumn checkboxTableColumn in superCheckboxTableColumn.columns_affected)
		{
			if (checkboxTableColumn.isRevealed)
			{
				switch (checkboxTableColumn.get_value_action(widgetRow.GetIdentity(), widgetRow.GetWidget(checkboxTableColumn)))
				{
				case TableScreen.ResultValues.False:
					flag2 = false;
					if (!flag)
					{
					}
					break;
				case TableScreen.ResultValues.Partial:
					flag4 = true;
					break;
				case TableScreen.ResultValues.True:
					flag4 = true;
					flag = false;
					if (!flag2)
					{
					}
					break;
				case TableScreen.ResultValues.ConditionalGroup:
					flag3 = true;
					flag2 = false;
					flag = false;
					break;
				}
			}
		}
		TableScreen.ResultValues result = TableScreen.ResultValues.Partial;
		if (flag3 && !flag4 && !flag2 && !flag)
		{
			result = TableScreen.ResultValues.ConditionalGroup;
		}
		else if (flag2)
		{
			result = TableScreen.ResultValues.True;
		}
		else if (flag)
		{
			result = TableScreen.ResultValues.False;
		}
		else if (flag4)
		{
			result = TableScreen.ResultValues.Partial;
		}
		return result;
	}

	// Token: 0x06006BBC RID: 27580 RVA: 0x0028EBAC File Offset: 0x0028CDAC
	protected void set_value_checkbox_column_super(GameObject widget_go, TableScreen.ResultValues new_value)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
			base.StartCoroutine(this.CascadeSetRowCheckBoxes(superCheckboxTableColumn.columns_affected, this.default_row.GetComponent<TableRow>(), new_value, widget_go));
			base.StartCoroutine(this.CascadeSetColumnCheckBoxes(this.all_sortable_rows, superCheckboxTableColumn, new_value, widget_go));
			return;
		case TableRow.RowType.Default:
			base.StartCoroutine(this.CascadeSetRowCheckBoxes(superCheckboxTableColumn.columns_affected, widgetRow, new_value, widget_go));
			return;
		case TableRow.RowType.Minion:
			base.StartCoroutine(this.CascadeSetRowCheckBoxes(superCheckboxTableColumn.columns_affected, widgetRow, new_value, widget_go));
			return;
		default:
			return;
		}
	}

	// Token: 0x06006BBD RID: 27581 RVA: 0x0028EC4C File Offset: 0x0028CE4C
	protected IEnumerator CascadeSetRowCheckBoxes(CheckboxTableColumn[] checkBoxToggleColumns, TableRow row, TableScreen.ResultValues state, GameObject ignore_widget = null)
	{
		if (this.active_cascade_coroutine_count == 0)
		{
			this.current_looping_sound = LoopingSoundManager.StartSound(this.cascade_sound_path, Vector3.zero, false, false);
		}
		this.active_cascade_coroutine_count++;
		int num;
		for (int i = 0; i < checkBoxToggleColumns.Length; i = num + 1)
		{
			if (checkBoxToggleColumns[i].widgets_by_row.ContainsKey(row))
			{
				GameObject gameObject = checkBoxToggleColumns[i].widgets_by_row[row];
				if (!(gameObject == ignore_widget) && checkBoxToggleColumns[i].isRevealed)
				{
					bool flag = false;
					switch ((this.GetWidgetColumn(gameObject) as CheckboxTableColumn).get_value_action(row.GetIdentity(), gameObject))
					{
					case TableScreen.ResultValues.False:
						flag = (state != TableScreen.ResultValues.False);
						break;
					case TableScreen.ResultValues.Partial:
					case TableScreen.ResultValues.ConditionalGroup:
						flag = true;
						break;
					case TableScreen.ResultValues.True:
						flag = (state != TableScreen.ResultValues.True);
						break;
					}
					if (flag)
					{
						(this.GetWidgetColumn(gameObject) as CheckboxTableColumn).on_set_action(gameObject, state);
						yield return null;
					}
				}
			}
			num = i;
		}
		this.active_cascade_coroutine_count--;
		if (this.active_cascade_coroutine_count <= 0)
		{
			this.StopLoopingCascadeSound();
		}
		yield break;
	}

	// Token: 0x06006BBE RID: 27582 RVA: 0x0028EC78 File Offset: 0x0028CE78
	protected IEnumerator CascadeSetColumnCheckBoxes(List<TableRow> rows, CheckboxTableColumn checkBoxToggleColumn, TableScreen.ResultValues state, GameObject header_widget_go = null)
	{
		if (this.active_cascade_coroutine_count == 0)
		{
			this.current_looping_sound = LoopingSoundManager.StartSound(this.cascade_sound_path, Vector3.zero, false, true);
		}
		this.active_cascade_coroutine_count++;
		int num;
		for (int i = 0; i < rows.Count; i = num + 1)
		{
			GameObject widget = rows[i].GetWidget(checkBoxToggleColumn);
			if (!(widget == header_widget_go))
			{
				bool flag = false;
				switch ((this.GetWidgetColumn(widget) as CheckboxTableColumn).get_value_action(rows[i].GetIdentity(), widget))
				{
				case TableScreen.ResultValues.False:
					flag = (state != TableScreen.ResultValues.False);
					break;
				case TableScreen.ResultValues.Partial:
				case TableScreen.ResultValues.ConditionalGroup:
					flag = true;
					break;
				case TableScreen.ResultValues.True:
					flag = (state != TableScreen.ResultValues.True);
					break;
				}
				if (flag)
				{
					(this.GetWidgetColumn(widget) as CheckboxTableColumn).on_set_action(widget, state);
					yield return null;
				}
			}
			num = i;
		}
		if (header_widget_go != null)
		{
			(this.GetWidgetColumn(header_widget_go) as CheckboxTableColumn).on_load_action(null, header_widget_go);
		}
		this.active_cascade_coroutine_count--;
		if (this.active_cascade_coroutine_count <= 0)
		{
			this.StopLoopingCascadeSound();
		}
		yield break;
	}

	// Token: 0x06006BBF RID: 27583 RVA: 0x0028ECA4 File Offset: 0x0028CEA4
	private void StopLoopingCascadeSound()
	{
		if (this.current_looping_sound.IsValid())
		{
			LoopingSoundManager.StopSound(this.current_looping_sound);
			this.current_looping_sound.Clear();
		}
	}

	// Token: 0x06006BC0 RID: 27584 RVA: 0x0028ECCC File Offset: 0x0028CECC
	protected void on_press_checkbox_column_super(GameObject widget_go)
	{
		SuperCheckboxTableColumn superCheckboxTableColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
		TableRow widgetRow = this.GetWidgetRow(widget_go);
		switch (this.get_value_checkbox_column_super(widgetRow.GetIdentity(), widget_go))
		{
		case TableScreen.ResultValues.False:
			superCheckboxTableColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
			break;
		case TableScreen.ResultValues.Partial:
		case TableScreen.ResultValues.ConditionalGroup:
			superCheckboxTableColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
			break;
		case TableScreen.ResultValues.True:
			superCheckboxTableColumn.on_set_action(widget_go, TableScreen.ResultValues.False);
			break;
		}
		superCheckboxTableColumn.on_load_action(widgetRow.GetIdentity(), widget_go);
	}

	// Token: 0x06006BC1 RID: 27585 RVA: 0x0028ED54 File Offset: 0x0028CF54
	protected void on_tooltip_sort_alphabetically(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (this.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_NAME, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
			break;
		default:
			return;
		}
	}

	// Token: 0x040049D1 RID: 18897
	protected string title;

	// Token: 0x040049D2 RID: 18898
	protected bool has_default_duplicant_row = true;

	// Token: 0x040049D3 RID: 18899
	protected bool useWorldDividers = true;

	// Token: 0x040049D4 RID: 18900
	private bool rows_dirty;

	// Token: 0x040049D5 RID: 18901
	protected Comparison<IAssignableIdentity> active_sort_method;

	// Token: 0x040049D6 RID: 18902
	protected TableColumn active_sort_column;

	// Token: 0x040049D7 RID: 18903
	protected bool sort_is_reversed;

	// Token: 0x040049D8 RID: 18904
	private int active_cascade_coroutine_count;

	// Token: 0x040049D9 RID: 18905
	private HandleVector<int>.Handle current_looping_sound = HandleVector<int>.InvalidHandle;

	// Token: 0x040049DA RID: 18906
	private bool incubating;

	// Token: 0x040049DB RID: 18907
	private int removeWorldHandle = -1;

	// Token: 0x040049DC RID: 18908
	protected Dictionary<string, TableColumn> columns = new Dictionary<string, TableColumn>();

	// Token: 0x040049DD RID: 18909
	public List<TableRow> rows = new List<TableRow>();

	// Token: 0x040049DE RID: 18910
	public List<TableRow> all_sortable_rows = new List<TableRow>();

	// Token: 0x040049DF RID: 18911
	public List<string> column_scrollers = new List<string>();

	// Token: 0x040049E0 RID: 18912
	private Dictionary<GameObject, TableRow> known_widget_rows = new Dictionary<GameObject, TableRow>();

	// Token: 0x040049E1 RID: 18913
	private Dictionary<GameObject, TableColumn> known_widget_columns = new Dictionary<GameObject, TableColumn>();

	// Token: 0x040049E2 RID: 18914
	public GameObject prefab_row_empty;

	// Token: 0x040049E3 RID: 18915
	public GameObject prefab_row_header;

	// Token: 0x040049E4 RID: 18916
	public GameObject prefab_world_divider;

	// Token: 0x040049E5 RID: 18917
	public GameObject prefab_scroller_border;

	// Token: 0x040049E6 RID: 18918
	private string cascade_sound_path = GlobalAssets.GetSound("Placers_Unfurl_LP", false);

	// Token: 0x040049E7 RID: 18919
	public KButton CloseButton;

	// Token: 0x040049E8 RID: 18920
	[MyCmpGet]
	private VerticalLayoutGroup VLG;

	// Token: 0x040049E9 RID: 18921
	protected GameObject header_row;

	// Token: 0x040049EA RID: 18922
	protected GameObject default_row;

	// Token: 0x040049EB RID: 18923
	public LocText title_bar;

	// Token: 0x040049EC RID: 18924
	public Transform header_content_transform;

	// Token: 0x040049ED RID: 18925
	public Transform scroll_content_transform;

	// Token: 0x040049EE RID: 18926
	public Transform scroller_borders_transform;

	// Token: 0x040049EF RID: 18927
	public Dictionary<int, GameObject> worldDividers = new Dictionary<int, GameObject>();

	// Token: 0x040049F0 RID: 18928
	private bool scrollersDirty;

	// Token: 0x040049F1 RID: 18929
	private float targetScrollerPosition;

	// Token: 0x040049F2 RID: 18930
	private Dictionary<IAssignableIdentity, bool> obsoleteMinionRowStatus = new Dictionary<IAssignableIdentity, bool>();

	// Token: 0x040049F3 RID: 18931
	private Dictionary<int, bool> obsoleteWorldDividerStatus = new Dictionary<int, bool>();

	// Token: 0x02001FD3 RID: 8147
	public enum ResultValues
	{
		// Token: 0x040093F0 RID: 37872
		False,
		// Token: 0x040093F1 RID: 37873
		Partial,
		// Token: 0x040093F2 RID: 37874
		True,
		// Token: 0x040093F3 RID: 37875
		ConditionalGroup,
		// Token: 0x040093F4 RID: 37876
		NotApplicable
	}
}
