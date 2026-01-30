using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D82 RID: 3458
public class TableColumn : IRender1000ms
{
	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x06006B7A RID: 27514 RVA: 0x0028C85E File Offset: 0x0028AA5E
	public bool isRevealed
	{
		get
		{
			return this.revealed == null || this.revealed();
		}
	}

	// Token: 0x06006B7B RID: 27515 RVA: 0x0028C878 File Offset: 0x0028AA78
	public TableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip = null, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip = null, Func<bool> revealed = null, bool should_refresh_columns = false, string scrollerID = "")
	{
		this.on_load_action = on_load_action;
		this.sort_comparer = sort_comparison;
		this.on_tooltip = on_tooltip;
		this.on_sort_tooltip = on_sort_tooltip;
		this.revealed = revealed;
		this.scrollerID = scrollerID;
		if (should_refresh_columns)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
	}

	// Token: 0x06006B7C RID: 27516 RVA: 0x0028C8D4 File Offset: 0x0028AAD4
	protected string GetTooltip(ToolTip tool_tip_instance)
	{
		GameObject gameObject = tool_tip_instance.gameObject;
		HierarchyReferences component = tool_tip_instance.GetComponent<HierarchyReferences>();
		if (component != null && component.HasReference("Widget"))
		{
			gameObject = component.GetReference("Widget").gameObject;
		}
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_tooltip != null)
		{
			this.on_tooltip(tableRow.GetIdentity(), gameObject, tool_tip_instance);
		}
		return "";
	}

	// Token: 0x06006B7D RID: 27517 RVA: 0x0028C99C File Offset: 0x0028AB9C
	protected string GetSortTooltip(ToolTip sort_tooltip_instance)
	{
		GameObject gameObject = sort_tooltip_instance.transform.parent.gameObject;
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_sort_tooltip != null)
		{
			this.on_sort_tooltip(tableRow.GetIdentity(), gameObject, sort_tooltip_instance);
		}
		return "";
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06006B7E RID: 27518 RVA: 0x0028CA40 File Offset: 0x0028AC40
	public bool isDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x06006B7F RID: 27519 RVA: 0x0028CA48 File Offset: 0x0028AC48
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets_by_row.ContainsValue(widget);
	}

	// Token: 0x06006B80 RID: 27520 RVA: 0x0028CA56 File Offset: 0x0028AC56
	public virtual GameObject GetMinionWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06006B81 RID: 27521 RVA: 0x0028CA63 File Offset: 0x0028AC63
	public virtual GameObject GetHeaderWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06006B82 RID: 27522 RVA: 0x0028CA70 File Offset: 0x0028AC70
	public virtual GameObject GetDefaultWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06006B83 RID: 27523 RVA: 0x0028CA7D File Offset: 0x0028AC7D
	public void Render1000ms(float dt)
	{
		this.MarkDirty(null, TableScreen.ResultValues.False);
	}

	// Token: 0x06006B84 RID: 27524 RVA: 0x0028CA87 File Offset: 0x0028AC87
	public void MarkDirty(GameObject triggering_obj = null, TableScreen.ResultValues triggering_object_state = TableScreen.ResultValues.False)
	{
		this.dirty = true;
	}

	// Token: 0x06006B85 RID: 27525 RVA: 0x0028CA90 File Offset: 0x0028AC90
	public void MarkClean()
	{
		this.dirty = false;
	}

	// Token: 0x040049BB RID: 18875
	public Action<IAssignableIdentity, GameObject> on_load_action;

	// Token: 0x040049BC RID: 18876
	public Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip;

	// Token: 0x040049BD RID: 18877
	public Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip;

	// Token: 0x040049BE RID: 18878
	public Comparison<IAssignableIdentity> sort_comparer;

	// Token: 0x040049BF RID: 18879
	public Dictionary<TableRow, GameObject> widgets_by_row = new Dictionary<TableRow, GameObject>();

	// Token: 0x040049C0 RID: 18880
	public string scrollerID;

	// Token: 0x040049C1 RID: 18881
	public TableScreen screen;

	// Token: 0x040049C2 RID: 18882
	public MultiToggle column_sort_toggle;

	// Token: 0x040049C3 RID: 18883
	private Func<bool> revealed;

	// Token: 0x040049C4 RID: 18884
	protected bool dirty;
}
