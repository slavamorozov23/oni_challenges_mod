using System;
using UnityEngine;

// Token: 0x02000D7A RID: 3450
public class PrioritizeRowTableColumn : TableColumn
{
	// Token: 0x06006AF4 RID: 27380 RVA: 0x00287EAC File Offset: 0x002860AC
	public PrioritizeRowTableColumn(object user_data, Action<object, int> on_change_priority, Func<object, int, string> on_hover_widget) : base(null, null, null, null, null, false, "")
	{
		this.userData = user_data;
		this.onChangePriority = on_change_priority;
		this.onHoverWidget = on_hover_widget;
	}

	// Token: 0x06006AF5 RID: 27381 RVA: 0x00287ED4 File Offset: 0x002860D4
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006AF6 RID: 27382 RVA: 0x00287EDD File Offset: 0x002860DD
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return this.GetWidget(parent);
	}

	// Token: 0x06006AF7 RID: 27383 RVA: 0x00287EE6 File Offset: 0x002860E6
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowHeaderWidget, parent, true);
	}

	// Token: 0x06006AF8 RID: 27384 RVA: 0x00287F00 File Offset: 0x00286100
	private GameObject GetWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowWidget, parent, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		this.ConfigureButton(component, "UpButton", 1, gameObject);
		this.ConfigureButton(component, "DownButton", -1, gameObject);
		return gameObject;
	}

	// Token: 0x06006AF9 RID: 27385 RVA: 0x00287F48 File Offset: 0x00286148
	private void ConfigureButton(HierarchyReferences refs, string ref_id, int delta, GameObject widget_go)
	{
		KButton kbutton = refs.GetReference(ref_id) as KButton;
		kbutton.onClick += delegate()
		{
			this.onChangePriority(widget_go, delta);
		};
		ToolTip component = kbutton.GetComponent<ToolTip>();
		if (component != null)
		{
			component.OnToolTip = (() => this.onHoverWidget(widget_go, delta));
		}
	}

	// Token: 0x0400499F RID: 18847
	public object userData;

	// Token: 0x040049A0 RID: 18848
	private Action<object, int> onChangePriority;

	// Token: 0x040049A1 RID: 18849
	private Func<object, int, string> onHoverWidget;
}
