using System;
using UnityEngine;

// Token: 0x02000D7D RID: 3453
public class ConsumableInfoTableColumn : CheckboxTableColumn
{
	// Token: 0x06006B18 RID: 27416 RVA: 0x002895E4 File Offset: 0x002877E4
	public ConsumableInfoTableColumn(IConsumableUIItem consumable_info, Action<IAssignableIdentity, GameObject> load_value_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, Func<GameObject, string> get_header_label, Func<bool> reveal_test) : base(load_value_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, on_sort_tooltip, reveal_test)
	{
		this.consumable_info = consumable_info;
		this.get_header_label = get_header_label;
	}

	// Token: 0x06006B19 RID: 27417 RVA: 0x00289614 File Offset: 0x00287814
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject headerWidget = base.GetHeaderWidget(parent);
		if (headerWidget.GetComponentInChildren<LocText>() != null)
		{
			headerWidget.GetComponentInChildren<LocText>().text = this.get_header_label(headerWidget);
		}
		headerWidget.GetComponentInChildren<MultiToggle>().gameObject.SetActive(false);
		return headerWidget;
	}

	// Token: 0x040049A4 RID: 18852
	public IConsumableUIItem consumable_info;

	// Token: 0x040049A5 RID: 18853
	public Func<GameObject, string> get_header_label;
}
