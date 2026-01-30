using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D77 RID: 3447
public class NumericDropDownTableColumn : TableColumn
{
	// Token: 0x06006AE6 RID: 27366 RVA: 0x00287915 File Offset: 0x00285B15
	public NumericDropDownTableColumn(object user_data, List<TMP_Dropdown.OptionData> options, Action<IAssignableIdentity, GameObject> on_load_action, Action<GameObject, int> set_value_action, Comparison<IAssignableIdentity> sort_comparer, NumericDropDownTableColumn.ToolTipCallbacks callbacks, Func<bool> revealed = null) : base(on_load_action, sort_comparer, callbacks.headerTooltip, callbacks.headerSortTooltip, revealed, false, "")
	{
		this.userData = user_data;
		this.set_value_action = set_value_action;
		this.options = options;
		this.callbacks = callbacks;
	}

	// Token: 0x06006AE7 RID: 27367 RVA: 0x00287954 File Offset: 0x00285B54
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006AE8 RID: 27368 RVA: 0x0028795D File Offset: 0x00285B5D
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006AE9 RID: 27369 RVA: 0x00287968 File Offset: 0x00285B68
	private GameObject GetWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.NumericDropDown, parent, true);
		TMP_Dropdown componentInChildren = widget_go.transform.GetComponentInChildren<TMP_Dropdown>();
		componentInChildren.options = this.options;
		componentInChildren.onValueChanged.AddListener(delegate(int new_value)
		{
			this.set_value_action(widget_go, new_value);
		});
		ToolTip tt = widget_go.transform.GetComponentInChildren<ToolTip>();
		if (tt != null)
		{
			tt.OnToolTip = (() => this.GetTooltip(tt));
		}
		return widget_go;
	}

	// Token: 0x06006AEA RID: 27370 RVA: 0x00287A14 File Offset: 0x00285C14
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		NumericDropDownTableColumn.<>c__DisplayClass9_0 CS$<>8__locals1 = new NumericDropDownTableColumn.<>c__DisplayClass9_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.DropDownHeader, parent, true);
		HierarchyReferences component = CS$<>8__locals1.widget_go.GetComponent<HierarchyReferences>();
		Component reference = component.GetReference("Label");
		MultiToggle componentInChildren = reference.GetComponentInChildren<MultiToggle>(true);
		this.column_sort_toggle = componentInChildren;
		MultiToggle multiToggle = componentInChildren;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			CS$<>8__locals1.<>4__this.screen.SetSortComparison(CS$<>8__locals1.<>4__this.sort_comparer, CS$<>8__locals1.<>4__this);
			CS$<>8__locals1.<>4__this.screen.SortRows();
		}));
		ToolTip tt2 = reference.GetComponent<ToolTip>();
		tt2.enabled = true;
		tt2.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerTooltip(null, CS$<>8__locals1.widget_go, tt2);
			return "";
		};
		ToolTip tt3 = componentInChildren.transform.GetComponent<ToolTip>();
		tt3.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerSortTooltip(null, CS$<>8__locals1.widget_go, tt3);
			return "";
		};
		Component reference2 = component.GetReference("DropDown");
		TMP_Dropdown componentInChildren2 = reference2.GetComponentInChildren<TMP_Dropdown>();
		componentInChildren2.options = this.options;
		componentInChildren2.onValueChanged.AddListener(delegate(int new_value)
		{
			CS$<>8__locals1.<>4__this.set_value_action(CS$<>8__locals1.widget_go, new_value);
		});
		ToolTip tt = reference2.GetComponent<ToolTip>();
		tt.OnToolTip = delegate()
		{
			CS$<>8__locals1.<>4__this.callbacks.headerDropdownTooltip(null, CS$<>8__locals1.widget_go, tt);
			return "";
		};
		LayoutElement component2 = CS$<>8__locals1.widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component2.preferredWidth = (component2.minWidth = 83f);
		return CS$<>8__locals1.widget_go;
	}

	// Token: 0x04004993 RID: 18835
	public object userData;

	// Token: 0x04004994 RID: 18836
	private NumericDropDownTableColumn.ToolTipCallbacks callbacks;

	// Token: 0x04004995 RID: 18837
	private Action<GameObject, int> set_value_action;

	// Token: 0x04004996 RID: 18838
	private List<TMP_Dropdown.OptionData> options;

	// Token: 0x02001FB9 RID: 8121
	public class ToolTipCallbacks
	{
		// Token: 0x040093AE RID: 37806
		public Action<IAssignableIdentity, GameObject, ToolTip> headerTooltip;

		// Token: 0x040093AF RID: 37807
		public Action<IAssignableIdentity, GameObject, ToolTip> headerSortTooltip;

		// Token: 0x040093B0 RID: 37808
		public Action<IAssignableIdentity, GameObject, ToolTip> headerDropdownTooltip;
	}
}
