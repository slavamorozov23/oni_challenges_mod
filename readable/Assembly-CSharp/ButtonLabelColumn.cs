using System;
using UnityEngine;

// Token: 0x02000D76 RID: 3446
public class ButtonLabelColumn : LabelTableColumn
{
	// Token: 0x06006AE2 RID: 27362 RVA: 0x0028777B File Offset: 0x0028597B
	public ButtonLabelColumn(Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Action<GameObject> on_click_action, Action<GameObject> on_double_click_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, bool whiteText = false) : base(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, 128, false)
	{
		this.on_click_action = on_click_action;
		this.on_double_click_action = on_double_click_action;
		this.whiteText = whiteText;
	}

	// Token: 0x06006AE3 RID: 27363 RVA: 0x002877A8 File Offset: 0x002859A8
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x06006AE4 RID: 27364 RVA: 0x00287845 File Offset: 0x00285A45
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return base.GetHeaderWidget(parent);
	}

	// Token: 0x06006AE5 RID: 27365 RVA: 0x00287850 File Offset: 0x00285A50
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite : Assets.UIPrefabs.TableScreenWidgets.ButtonLabel, parent, true);
		ToolTip tt = widget_go.GetComponent<ToolTip>();
		tt.OnToolTip = (() => this.GetTooltip(tt));
		if (this.on_click_action != null)
		{
			widget_go.GetComponent<KButton>().onClick += delegate()
			{
				this.on_click_action(widget_go);
			};
		}
		if (this.on_double_click_action != null)
		{
			widget_go.GetComponent<KButton>().onDoubleClick += delegate()
			{
				this.on_double_click_action(widget_go);
			};
		}
		return widget_go;
	}

	// Token: 0x04004990 RID: 18832
	private Action<GameObject> on_click_action;

	// Token: 0x04004991 RID: 18833
	private Action<GameObject> on_double_click_action;

	// Token: 0x04004992 RID: 18834
	private bool whiteText;
}
