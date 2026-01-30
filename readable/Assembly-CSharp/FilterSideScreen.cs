using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E3B RID: 3643
public class FilterSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x0600738E RID: 29582 RVA: 0x002C1F14 File Offset: 0x002C0114
	public override bool IsValidForTarget(GameObject target)
	{
		bool flag;
		if (this.isLogicFilter)
		{
			flag = (target.GetComponent<ConduitElementSensor>() != null || target.GetComponent<LogicElementSensor>() != null);
		}
		else
		{
			flag = (target.GetComponent<ElementFilter>() != null || target.GetComponent<RocketConduitStorageAccess>() != null || target.GetComponent<DevPump>() != null);
		}
		return flag && target.GetComponent<Filterable>() != null;
	}

	// Token: 0x0600738F RID: 29583 RVA: 0x002C1F88 File Offset: 0x002C0188
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetFilterable = target.GetComponent<Filterable>();
		if (this.targetFilterable == null)
		{
			return;
		}
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
			goto IL_87;
		case Filterable.ElementState.Gas:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
			goto IL_87;
		}
		this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
		IL_87:
		this.Configure(this.targetFilterable);
		this.SetFilterTag(this.targetFilterable.SelectedTag);
	}

	// Token: 0x06007390 RID: 29584 RVA: 0x002C2039 File Offset: 0x002C0239
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetFilterTag(rowClicked.tag);
		base.ItemRowClicked(rowClicked);
	}

	// Token: 0x06007391 RID: 29585 RVA: 0x002C2050 File Offset: 0x002C0250
	private void Configure(Filterable filterable)
	{
		Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
		Tag tag = GameTags.Void;
		foreach (Tag tag2 in tagOptions.Keys)
		{
			using (HashSet<Tag>.Enumerator enumerator2 = tagOptions[tag2].GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == filterable.SelectedTag)
					{
						tag = tag2;
						break;
					}
				}
			}
		}
		this.SetData(tagOptions);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (this.categories.TryGetValue(GameTags.Void, out category))
		{
			category.SetProihibedState(true);
		}
		if (tag != GameTags.Void)
		{
			this.categories[tag].SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}
		if (this.voidRow == null)
		{
			this.voidRow = this.GetOrCreateItemRow(GameTags.Void);
		}
		this.voidRow.transform.SetAsFirstSibling();
		if (filterable.SelectedTag != GameTags.Void)
		{
			this.SetSelectedItem(filterable.SelectedTag);
		}
		else
		{
			this.SetSelectedItem(this.voidRow);
		}
		this.RefreshUI();
	}

	// Token: 0x06007392 RID: 29586 RVA: 0x002C21A0 File Offset: 0x002C03A0
	private void SetFilterTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		if (tag.IsValid)
		{
			this.targetFilterable.SelectedTag = tag;
		}
		this.RefreshUI();
	}

	// Token: 0x06007393 RID: 29587 RVA: 0x002C21CC File Offset: 0x002C03CC
	private void RefreshUI()
	{
		LocString loc_string;
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID;
			goto IL_38;
		case Filterable.ElementState.Gas:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
			goto IL_38;
		}
		loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID;
		IL_38:
		this.currentSelectionLabel.text = string.Format(loc_string, UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
		if (base.CurrentSelectedItem == null || base.CurrentSelectedItem.tag != this.targetFilterable.SelectedTag)
		{
			this.SetSelectedItem(this.targetFilterable.SelectedTag);
		}
		if (this.targetFilterable.SelectedTag != GameTags.Void)
		{
			this.currentSelectionLabel.text = string.Format(loc_string, this.targetFilterable.SelectedTag.ProperName());
			return;
		}
		this.currentSelectionLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
	}

	// Token: 0x04004FE4 RID: 20452
	public HierarchyReferences categoryFoldoutPrefab;

	// Token: 0x04004FE5 RID: 20453
	public RectTransform elementEntryContainer;

	// Token: 0x04004FE6 RID: 20454
	public Image outputIcon;

	// Token: 0x04004FE7 RID: 20455
	public Image everythingElseIcon;

	// Token: 0x04004FE8 RID: 20456
	public LocText outputElementHeaderLabel;

	// Token: 0x04004FE9 RID: 20457
	public LocText everythingElseHeaderLabel;

	// Token: 0x04004FEA RID: 20458
	public LocText selectElementHeaderLabel;

	// Token: 0x04004FEB RID: 20459
	public LocText currentSelectionLabel;

	// Token: 0x04004FEC RID: 20460
	private SingleItemSelectionRow voidRow;

	// Token: 0x04004FED RID: 20461
	public bool isLogicFilter;

	// Token: 0x04004FEE RID: 20462
	private Filterable targetFilterable;
}
