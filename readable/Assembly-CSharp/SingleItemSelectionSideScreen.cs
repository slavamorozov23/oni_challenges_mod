using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E7B RID: 3707
public class SingleItemSelectionSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x060075F6 RID: 30198 RVA: 0x002D097C File Offset: 0x002CEB7C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<StorageTile.Instance>() != null && target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x060075F7 RID: 30199 RVA: 0x002D0994 File Offset: 0x002CEB94
	private Tag GetTargetCurrentSelectedTag()
	{
		if (this.CurrentTarget != null)
		{
			return this.CurrentTarget.TargetTag;
		}
		return this.INVALID_OPTION_TAG;
	}

	// Token: 0x060075F8 RID: 30200 RVA: 0x002D09B0 File Offset: 0x002CEBB0
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.CurrentTarget = target.GetSMI<StorageTile.Instance>();
		if (this.CurrentTarget != null)
		{
			Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
			foreach (Tag tag in new HashSet<Tag>(this.CurrentTarget.GetComponent<Storage>().storageFilters))
			{
				HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag);
				if (discoveredResourcesFromTag != null && discoveredResourcesFromTag.Count > 0)
				{
					dictionary.Add(tag, discoveredResourcesFromTag);
				}
			}
			this.SetData(dictionary);
			SingleItemSelectionSideScreenBase.Category category = null;
			if (!this.categories.TryGetValue(this.INVALID_OPTION_TAG, out category))
			{
				category = base.GetCategoryWithItem(this.INVALID_OPTION_TAG, false);
			}
			if (category != null)
			{
				category.SetProihibedState(true);
			}
			this.CreateNoneOption();
			Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
			if (targetCurrentSelectedTag != this.INVALID_OPTION_TAG)
			{
				this.SetSelectedItem(targetCurrentSelectedTag);
				base.GetCategoryWithItem(targetCurrentSelectedTag, false).SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
			}
			else
			{
				this.SetSelectedItem(this.noneOptionRow);
			}
			this.selectedItemLabel.SetItem(targetCurrentSelectedTag);
		}
	}

	// Token: 0x060075F9 RID: 30201 RVA: 0x002D0AD8 File Offset: 0x002CECD8
	private void CreateNoneOption()
	{
		if (this.noneOptionRow == null)
		{
			this.noneOptionRow = this.GetOrCreateItemRow(this.INVALID_OPTION_TAG);
		}
		this.noneOptionRow.transform.SetAsFirstSibling();
	}

	// Token: 0x060075FA RID: 30202 RVA: 0x002D0B0C File Offset: 0x002CED0C
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		base.ItemRowClicked(rowClicked);
		this.selectedItemLabel.SetItem(rowClicked.tag);
		Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
		if (this.CurrentTarget != null && targetCurrentSelectedTag != rowClicked.tag)
		{
			this.CurrentTarget.SetTargetItem(rowClicked.tag);
		}
	}

	// Token: 0x040051A9 RID: 20905
	[SerializeField]
	private SingleItemSelectionSideScreen_SelectedItemSection selectedItemLabel;

	// Token: 0x040051AA RID: 20906
	private StorageTile.Instance CurrentTarget;

	// Token: 0x040051AB RID: 20907
	private SingleItemSelectionRow noneOptionRow;

	// Token: 0x040051AC RID: 20908
	private Tag INVALID_OPTION_TAG = GameTags.Void;
}
