using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E3D RID: 3645
public class FlatTagFilterSideScreen : SideScreenContent
{
	// Token: 0x06007398 RID: 29592 RVA: 0x002C22F9 File Offset: 0x002C04F9
	public override int GetSideScreenSortOrder()
	{
		return 50;
	}

	// Token: 0x06007399 RID: 29593 RVA: 0x002C2300 File Offset: 0x002C0500
	public override bool IsValidForTarget(GameObject target)
	{
		FlatTagFilterable component = target.GetComponent<FlatTagFilterable>();
		return component != null && component.currentlyUserAssignable;
	}

	// Token: 0x0600739A RID: 29594 RVA: 0x002C2325 File Offset: 0x002C0525
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.tagFilterable = target.GetComponent<FlatTagFilterable>();
		this.Build();
	}

	// Token: 0x0600739B RID: 29595 RVA: 0x002C2340 File Offset: 0x002C0540
	private void Build()
	{
		this.headerLabel.SetText(this.tagFilterable.GetHeaderText());
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.tagFilterable.tagOptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
			gameObject.gameObject.name = tag.ProperName();
			this.rows.Add(tag, gameObject);
		}
		this.Refresh();
	}

	// Token: 0x0600739C RID: 29596 RVA: 0x002C2434 File Offset: 0x002C0634
	private void Refresh()
	{
		using (Dictionary<Tag, GameObject>.Enumerator enumerator = this.rows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Tag, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.tagFilterable.ToggleTag(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.tagFilterable.selectedTags.Contains(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(!this.tagFilterable.displayOnlyDiscoveredTags || DiscoveredResources.Instance.IsDiscovered(kvp.Key));
			}
		}
	}

	// Token: 0x0600739D RID: 29597 RVA: 0x002C25F4 File Offset: 0x002C07F4
	public override string GetTitle()
	{
		return this.tagFilterable.gameObject.GetProperName();
	}

	// Token: 0x04004FEF RID: 20463
	private FlatTagFilterable tagFilterable;

	// Token: 0x04004FF0 RID: 20464
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04004FF1 RID: 20465
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04004FF2 RID: 20466
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04004FF3 RID: 20467
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
