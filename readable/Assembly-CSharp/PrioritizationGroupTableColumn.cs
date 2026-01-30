using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D79 RID: 3449
public class PrioritizationGroupTableColumn : TableColumn
{
	// Token: 0x06006AEF RID: 27375 RVA: 0x00287C68 File Offset: 0x00285E68
	public PrioritizationGroupTableColumn(object user_data, Action<IAssignableIdentity, GameObject> on_load_action, Action<object, int> on_change_priority, Func<object, string> on_hover_widget, Action<object, int> on_change_header_priority, Func<object, string> on_hover_header_option_selector, Action<object> on_sort_clicked, Func<object, string> on_sort_hovered) : base(on_load_action, null, null, null, null, false, "")
	{
		this.userData = user_data;
		this.onChangePriority = on_change_priority;
		this.onHoverWidget = on_hover_widget;
		this.onHoverHeaderOptionSelector = on_hover_header_option_selector;
		this.onSortClicked = on_sort_clicked;
		this.onSortHovered = on_sort_hovered;
	}

	// Token: 0x06006AF0 RID: 27376 RVA: 0x00287CB4 File Offset: 0x00285EB4
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006AF1 RID: 27377 RVA: 0x00287CBD File Offset: 0x00285EBD
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006AF2 RID: 27378 RVA: 0x00287CC8 File Offset: 0x00285EC8
	private GameObject GetWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PriorityGroupSelector, parent, true);
		OptionSelector component = widget_go.GetComponent<OptionSelector>();
		component.Initialize(widget_go);
		component.OnChangePriority = delegate(object widget, int delta)
		{
			this.onChangePriority(widget, delta);
		};
		ToolTip[] componentsInChildren = widget_go.transform.GetComponentsInChildren<ToolTip>();
		if (componentsInChildren != null)
		{
			Func<string> <>9__1;
			foreach (ToolTip toolTip in componentsInChildren)
			{
				Func<string> onToolTip;
				if ((onToolTip = <>9__1) == null)
				{
					onToolTip = (<>9__1 = (() => this.onHoverWidget(widget_go)));
				}
				toolTip.OnToolTip = onToolTip;
			}
		}
		return widget_go;
	}

	// Token: 0x06006AF3 RID: 27379 RVA: 0x00287D7C File Offset: 0x00285F7C
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PriorityGroupSelectorHeader, parent, true);
		HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
		LayoutElement component2 = widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component2.preferredWidth = (component2.minWidth = 63f);
		Component reference = component.GetReference("Label");
		reference.GetComponent<LocText>().raycastTarget = true;
		ToolTip component3 = reference.GetComponent<ToolTip>();
		if (component3 != null)
		{
			component3.OnToolTip = (() => this.onHoverWidget(widget_go));
		}
		MultiToggle componentInChildren = widget_go.GetComponentInChildren<MultiToggle>(true);
		this.column_sort_toggle = componentInChildren;
		MultiToggle multiToggle = componentInChildren;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.onSortClicked(widget_go);
		}));
		ToolTip component4 = componentInChildren.GetComponent<ToolTip>();
		if (component4 != null)
		{
			component4.OnToolTip = (() => this.onSortHovered(widget_go));
		}
		ToolTip component5 = (component.GetReference("PrioritizeButton") as KButton).GetComponent<ToolTip>();
		if (component5 != null)
		{
			component5.OnToolTip = (() => this.onHoverHeaderOptionSelector(widget_go));
		}
		return widget_go;
	}

	// Token: 0x04004999 RID: 18841
	public object userData;

	// Token: 0x0400499A RID: 18842
	private Action<object, int> onChangePriority;

	// Token: 0x0400499B RID: 18843
	private Func<object, string> onHoverWidget;

	// Token: 0x0400499C RID: 18844
	private Func<object, string> onHoverHeaderOptionSelector;

	// Token: 0x0400499D RID: 18845
	private Action<object> onSortClicked;

	// Token: 0x0400499E RID: 18846
	private Func<object, string> onSortHovered;
}
