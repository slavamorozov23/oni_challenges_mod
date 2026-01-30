using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6E RID: 3438
public class CrewListScreen<EntryType> : KScreen where EntryType : CrewListEntry
{
	// Token: 0x06006AA7 RID: 27303 RVA: 0x00285CD8 File Offset: 0x00283ED8
	protected override void OnActivate()
	{
		base.OnActivate();
		this.ClearEntries();
		this.SpawnEntries();
		this.PositionColumnTitles();
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006AA8 RID: 27304 RVA: 0x00285D07 File Offset: 0x00283F07
	protected override void OnCmpEnable()
	{
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		this.Reconstruct();
	}

	// Token: 0x06006AA9 RID: 27305 RVA: 0x00285D20 File Offset: 0x00283F20
	private void ClearEntries()
	{
		for (int i = this.EntryObjects.Count - 1; i > -1; i--)
		{
			Util.KDestroyGameObject(this.EntryObjects[i]);
		}
		this.EntryObjects.Clear();
	}

	// Token: 0x06006AAA RID: 27306 RVA: 0x00285D66 File Offset: 0x00283F66
	protected void RefreshCrewPortraitContent()
	{
		this.EntryObjects.ForEach(delegate(EntryType eo)
		{
			eo.RefreshCrewPortraitContent();
		});
	}

	// Token: 0x06006AAB RID: 27307 RVA: 0x00285D92 File Offset: 0x00283F92
	protected virtual void SpawnEntries()
	{
		if (this.EntryObjects.Count != 0)
		{
			this.ClearEntries();
		}
	}

	// Token: 0x06006AAC RID: 27308 RVA: 0x00285DA8 File Offset: 0x00283FA8
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		bool flag = false;
		List<MinionIdentity> liveIdentities = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		if (this.EntryObjects.Count != liveIdentities.Count || this.EntryObjects.FindAll((EntryType o) => liveIdentities.Contains(o.Identity)).Count != this.EntryObjects.Count)
		{
			flag = true;
		}
		if (flag)
		{
			this.Reconstruct();
		}
		this.UpdateScroll();
	}

	// Token: 0x06006AAD RID: 27309 RVA: 0x00285E39 File Offset: 0x00284039
	public void Reconstruct()
	{
		this.ClearEntries();
		this.SpawnEntries();
	}

	// Token: 0x06006AAE RID: 27310 RVA: 0x00285E48 File Offset: 0x00284048
	private void UpdateScroll()
	{
		if (this.PanelScrollbar)
		{
			if (this.EntryObjects.Count <= this.maxEntriesBeforeScroll)
			{
				this.PanelScrollbar.value = Mathf.Lerp(this.PanelScrollbar.value, 1f, 10f);
				this.PanelScrollbar.gameObject.SetActive(false);
				return;
			}
			this.PanelScrollbar.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006AAF RID: 27311 RVA: 0x00285EC0 File Offset: 0x002840C0
	private void SetHeadersActive(bool state)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			this.ColumnTitlesContainer.GetChild(i).gameObject.SetActive(state);
		}
	}

	// Token: 0x06006AB0 RID: 27312 RVA: 0x00285EFC File Offset: 0x002840FC
	protected virtual void PositionColumnTitles()
	{
		if (this.ColumnTitlesContainer == null)
		{
			return;
		}
		if (this.EntryObjects.Count <= 0)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		int childCount = this.EntryObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiate(this.Prefab_ColumnTitle, null, null);
				gameObject.name = component.Column_DisplayName;
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				gameObject.transform.SetParent(this.ColumnTitlesContainer);
				componentInChildren.text = (component.StringLookup ? Strings.Get(component.Column_DisplayName) : component.Column_DisplayName);
				gameObject.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.SORTCOLUMN, componentInChildren.text);
				gameObject.rectTransform().anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x, 0f);
				OverviewColumnIdentity overviewColumnIdentity = gameObject.GetComponent<OverviewColumnIdentity>();
				if (overviewColumnIdentity == null)
				{
					overviewColumnIdentity = gameObject.AddComponent<OverviewColumnIdentity>();
				}
				overviewColumnIdentity.Column_DisplayName = component.Column_DisplayName;
				overviewColumnIdentity.columnID = component.columnID;
				overviewColumnIdentity.xPivot = component.xPivot;
				overviewColumnIdentity.Sortable = component.Sortable;
				if (overviewColumnIdentity.Sortable)
				{
					overviewColumnIdentity.GetComponentInChildren<ImageToggleState>(true).gameObject.SetActive(true);
				}
			}
		}
		this.UpdateColumnTitles();
		this.sortToggleGroup = base.gameObject.AddComponent<ToggleGroup>();
		this.sortToggleGroup.allowSwitchOff = true;
	}

	// Token: 0x06006AB1 RID: 27313 RVA: 0x002860C0 File Offset: 0x002842C0
	protected void SortByName(bool reverse)
	{
		List<EntryType> list = new List<EntryType>(this.EntryObjects);
		list.Sort(delegate(EntryType a, EntryType b)
		{
			string text = a.Identity.GetProperName() + a.gameObject.GetInstanceID().ToString();
			string strB = b.Identity.GetProperName() + b.gameObject.GetInstanceID().ToString();
			return text.CompareTo(strB);
		});
		this.ReorderEntries(list, reverse);
	}

	// Token: 0x06006AB2 RID: 27314 RVA: 0x00286108 File Offset: 0x00284308
	protected void UpdateColumnTitles()
	{
		if (this.EntryObjects.Count <= 0 || !this.EntryObjects[0].gameObject.activeSelf)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			RectTransform rectTransform = this.ColumnTitlesContainer.GetChild(i).rectTransform();
			for (int j = 0; j < this.EntryObjects[0].transform.childCount; j++)
			{
				OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(j).GetComponent<OverviewColumnIdentity>();
				if (component != null && component.Column_DisplayName == rectTransform.name)
				{
					rectTransform.pivot = new Vector2(component.xPivot, rectTransform.pivot.y);
					rectTransform.anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x + this.columnTitleHorizontalOffset, 0f);
					rectTransform.sizeDelta = new Vector2(component.rectTransform().sizeDelta.x, rectTransform.sizeDelta.y);
					if (rectTransform.anchoredPosition.x == 0f)
					{
						rectTransform.gameObject.SetActive(false);
					}
					else
					{
						rectTransform.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x06006AB3 RID: 27315 RVA: 0x00286284 File Offset: 0x00284484
	protected void ReorderEntries(List<EntryType> sortedEntries, bool reverse)
	{
		for (int i = 0; i < sortedEntries.Count; i++)
		{
			if (reverse)
			{
				sortedEntries[i].transform.SetSiblingIndex(sortedEntries.Count - 1 - i);
			}
			else
			{
				sortedEntries[i].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x0400495D RID: 18781
	public GameObject Prefab_CrewEntry;

	// Token: 0x0400495E RID: 18782
	public List<EntryType> EntryObjects = new List<EntryType>();

	// Token: 0x0400495F RID: 18783
	public Transform ScrollRectTransform;

	// Token: 0x04004960 RID: 18784
	public Transform EntriesPanelTransform;

	// Token: 0x04004961 RID: 18785
	protected Vector2 EntryRectSize = new Vector2(750f, 64f);

	// Token: 0x04004962 RID: 18786
	public int maxEntriesBeforeScroll = 5;

	// Token: 0x04004963 RID: 18787
	public Scrollbar PanelScrollbar;

	// Token: 0x04004964 RID: 18788
	protected ToggleGroup sortToggleGroup;

	// Token: 0x04004965 RID: 18789
	protected Toggle lastSortToggle;

	// Token: 0x04004966 RID: 18790
	protected bool lastSortReversed;

	// Token: 0x04004967 RID: 18791
	public GameObject Prefab_ColumnTitle;

	// Token: 0x04004968 RID: 18792
	public Transform ColumnTitlesContainer;

	// Token: 0x04004969 RID: 18793
	public bool autoColumn;

	// Token: 0x0400496A RID: 18794
	public float columnTitleHorizontalOffset;
}
