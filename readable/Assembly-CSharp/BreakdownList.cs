using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EC7 RID: 3783
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownList")]
public class BreakdownList : KMonoBehaviour
{
	// Token: 0x06007923 RID: 31011 RVA: 0x002E8EAC File Offset: 0x002E70AC
	public BreakdownListRow AddRow()
	{
		BreakdownListRow breakdownListRow;
		if (this.unusedListRows.Count > 0)
		{
			breakdownListRow = this.unusedListRows[0];
			this.unusedListRows.RemoveAt(0);
		}
		else
		{
			breakdownListRow = UnityEngine.Object.Instantiate<BreakdownListRow>(this.listRowTemplate);
		}
		breakdownListRow.gameObject.transform.SetParent(base.transform);
		breakdownListRow.gameObject.transform.SetAsLastSibling();
		this.listRows.Add(breakdownListRow);
		breakdownListRow.gameObject.SetActive(true);
		return breakdownListRow;
	}

	// Token: 0x06007924 RID: 31012 RVA: 0x002E8F2D File Offset: 0x002E712D
	public GameObject AddCustomRow(GameObject newRow)
	{
		newRow.transform.SetParent(base.transform);
		newRow.gameObject.transform.SetAsLastSibling();
		this.customRows.Add(newRow);
		newRow.SetActive(true);
		return newRow;
	}

	// Token: 0x06007925 RID: 31013 RVA: 0x002E8F64 File Offset: 0x002E7164
	public void ClearRows()
	{
		foreach (BreakdownListRow breakdownListRow in this.listRows)
		{
			this.unusedListRows.Add(breakdownListRow);
			breakdownListRow.gameObject.SetActive(false);
			breakdownListRow.ClearTooltip();
		}
		this.listRows.Clear();
		foreach (GameObject gameObject in this.customRows)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x06007926 RID: 31014 RVA: 0x002E901C File Offset: 0x002E721C
	public void SetTitle(string title)
	{
		this.headerTitle.text = title;
	}

	// Token: 0x06007927 RID: 31015 RVA: 0x002E902A File Offset: 0x002E722A
	public void SetDescription(string description)
	{
		if (description != null && description.Length >= 0)
		{
			this.infoTextLabel.gameObject.SetActive(true);
			this.infoTextLabel.text = description;
			return;
		}
		this.infoTextLabel.gameObject.SetActive(false);
	}

	// Token: 0x06007928 RID: 31016 RVA: 0x002E9067 File Offset: 0x002E7267
	public void SetIcon(Sprite icon)
	{
		this.headerIcon.sprite = icon;
	}

	// Token: 0x0400547E RID: 21630
	public Image headerIcon;

	// Token: 0x0400547F RID: 21631
	public Sprite headerIconSprite;

	// Token: 0x04005480 RID: 21632
	public Image headerBar;

	// Token: 0x04005481 RID: 21633
	public LocText headerTitle;

	// Token: 0x04005482 RID: 21634
	public LocText headerValue;

	// Token: 0x04005483 RID: 21635
	public LocText infoTextLabel;

	// Token: 0x04005484 RID: 21636
	public BreakdownListRow listRowTemplate;

	// Token: 0x04005485 RID: 21637
	private List<BreakdownListRow> listRows = new List<BreakdownListRow>();

	// Token: 0x04005486 RID: 21638
	private List<BreakdownListRow> unusedListRows = new List<BreakdownListRow>();

	// Token: 0x04005487 RID: 21639
	private List<GameObject> customRows = new List<GameObject>();
}
