using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E2C RID: 3628
public class ConditionListSideScreen : SideScreenContent
{
	// Token: 0x06007333 RID: 29491 RVA: 0x002C094D File Offset: 0x002BEB4D
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x06007334 RID: 29492 RVA: 0x002C0950 File Offset: 0x002BEB50
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target != null)
		{
			this.targetConditionSet = target.GetComponent<IProcessConditionSet>();
		}
	}

	// Token: 0x06007335 RID: 29493 RVA: 0x002C096E File Offset: 0x002BEB6E
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh();
		}
	}

	// Token: 0x06007336 RID: 29494 RVA: 0x002C0980 File Offset: 0x002BEB80
	private void Refresh()
	{
		bool flag = false;
		List<ProcessCondition> list;
		using (ProcessCondition.ListPool.Get(out list))
		{
			this.targetConditionSet.PopulateConditionSet(ProcessCondition.ProcessConditionType.All, list);
			foreach (ProcessCondition key in list)
			{
				if (!this.rows.ContainsKey(key))
				{
					flag = true;
					break;
				}
			}
			foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
			{
				if (!list.Contains(keyValuePair.Key))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.Rebuild();
			}
			foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair2 in this.rows)
			{
				ConditionListSideScreen.SetRowState(keyValuePair2.Value, keyValuePair2.Key);
			}
		}
	}

	// Token: 0x06007337 RID: 29495 RVA: 0x002C0ABC File Offset: 0x002BECBC
	public static void SetRowState(GameObject row, ProcessCondition condition)
	{
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		ProcessCondition.Status status = condition.EvaluateCondition();
		component.GetReference<LocText>("Label").text = condition.GetStatusMessage(status);
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.failedColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.failedColor;
			break;
		case ProcessCondition.Status.Warning:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.warningColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.warningColor;
			break;
		case ProcessCondition.Status.Ready:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.readyColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.readyColor;
			break;
		}
		component.GetReference<Image>("Check").gameObject.SetActive(status == ProcessCondition.Status.Ready);
		component.GetReference<Image>("Dash").gameObject.SetActive(false);
		row.GetComponent<ToolTip>().SetSimpleTooltip(condition.GetStatusTooltip(status));
	}

	// Token: 0x06007338 RID: 29496 RVA: 0x002C0BC8 File Offset: 0x002BEDC8
	private void Rebuild()
	{
		this.ClearRows();
		this.BuildRows();
	}

	// Token: 0x06007339 RID: 29497 RVA: 0x002C0BD8 File Offset: 0x002BEDD8
	private void ClearRows()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
	}

	// Token: 0x0600733A RID: 29498 RVA: 0x002C0C3C File Offset: 0x002BEE3C
	private void BuildRows()
	{
		List<ProcessCondition> list;
		using (ProcessCondition.ListPool.Get(out list))
		{
			this.targetConditionSet.PopulateConditionSet(ProcessCondition.ProcessConditionType.All, list);
			foreach (ProcessCondition processCondition in list)
			{
				if (processCondition.ShowInUI())
				{
					GameObject value = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
					this.rows.Add(processCondition, value);
				}
			}
		}
	}

	// Token: 0x04004FB2 RID: 20402
	public GameObject rowPrefab;

	// Token: 0x04004FB3 RID: 20403
	public GameObject rowContainer;

	// Token: 0x04004FB4 RID: 20404
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public static Color readyColor = Color.black;

	// Token: 0x04004FB5 RID: 20405
	public static Color failedColor = Color.red;

	// Token: 0x04004FB6 RID: 20406
	public static Color warningColor = new Color(1f, 0.3529412f, 0f, 1f);

	// Token: 0x04004FB7 RID: 20407
	private IProcessConditionSet targetConditionSet;

	// Token: 0x04004FB8 RID: 20408
	private Dictionary<ProcessCondition, GameObject> rows = new Dictionary<ProcessCondition, GameObject>();
}
