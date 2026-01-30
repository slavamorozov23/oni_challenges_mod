using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DEB RID: 3563
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenEntry")]
public class ReportScreenEntry : KMonoBehaviour
{
	// Token: 0x0600702F RID: 28719 RVA: 0x002A9A60 File Offset: 0x002A7C60
	public void SetMainEntry(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenEntryRow>();
			MultiToggle toggle = this.mainRow.toggle;
			toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren = this.mainRow.name.GetComponentInChildren<MultiToggle>();
			componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren2 = this.mainRow.added.GetComponentInChildren<MultiToggle>();
			componentInChildren2.onClick = (System.Action)Delegate.Combine(componentInChildren2.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren3 = this.mainRow.removed.GetComponentInChildren<MultiToggle>();
			componentInChildren3.onClick = (System.Action)Delegate.Combine(componentInChildren3.onClick, new System.Action(this.ToggleContext));
			MultiToggle componentInChildren4 = this.mainRow.net.GetComponentInChildren<MultiToggle>();
			componentInChildren4.onClick = (System.Action)Delegate.Combine(componentInChildren4.onClick, new System.Action(this.ToggleContext));
		}
		this.mainRow.SetLine(entry, reportGroup);
		this.currentContextCount = entry.contextEntries.Count;
		for (int i = 0; i < entry.contextEntries.Count; i++)
		{
			if (i >= this.contextRows.Count)
			{
				ReportScreenEntryRow component = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, false).GetComponent<ReportScreenEntryRow>();
				this.contextRows.Add(component);
			}
			this.contextRows[i].SetLine(entry.contextEntries[i], reportGroup);
		}
		this.UpdateVisibility();
	}

	// Token: 0x06007030 RID: 28720 RVA: 0x002A9C1F File Offset: 0x002A7E1F
	private void ToggleContext()
	{
		this.mainRow.toggle.NextState();
		this.UpdateVisibility();
	}

	// Token: 0x06007031 RID: 28721 RVA: 0x002A9C38 File Offset: 0x002A7E38
	private void UpdateVisibility()
	{
		int i;
		for (i = 0; i < this.currentContextCount; i++)
		{
			this.contextRows[i].gameObject.SetActive(this.mainRow.toggle.CurrentState == 1);
		}
		while (i < this.contextRows.Count)
		{
			this.contextRows[i].gameObject.SetActive(false);
			i++;
		}
	}

	// Token: 0x04004CFA RID: 19706
	[SerializeField]
	private ReportScreenEntryRow rowTemplate;

	// Token: 0x04004CFB RID: 19707
	private ReportScreenEntryRow mainRow;

	// Token: 0x04004CFC RID: 19708
	private List<ReportScreenEntryRow> contextRows = new List<ReportScreenEntryRow>();

	// Token: 0x04004CFD RID: 19709
	private int currentContextCount;
}
