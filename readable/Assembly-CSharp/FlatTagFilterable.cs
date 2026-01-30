using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class FlatTagFilterable : KMonoBehaviour
{
	// Token: 0x06002FAB RID: 12203 RVA: 0x0011328F File Offset: 0x0011148F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.filterByStorageCategoriesOnSpawn = false;
		component.UpdateFilters(new HashSet<Tag>(this.selectedTags));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x06002FAC RID: 12204 RVA: 0x001132CC File Offset: 0x001114CC
	public void SelectTag(Tag tag, bool state)
	{
		global::Debug.Assert(this.tagOptions.Contains(tag), "The tag " + tag.Name + " is not valid for this filterable - it must be added to tagOptions");
		if (state)
		{
			if (!this.selectedTags.Contains(tag))
			{
				this.selectedTags.Add(tag);
			}
		}
		else if (this.selectedTags.Contains(tag))
		{
			this.selectedTags.Remove(tag);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x06002FAD RID: 12205 RVA: 0x00113350 File Offset: 0x00111550
	public void ToggleTag(Tag tag)
	{
		this.SelectTag(tag, !this.selectedTags.Contains(tag));
	}

	// Token: 0x06002FAE RID: 12206 RVA: 0x00113368 File Offset: 0x00111568
	public string GetHeaderText()
	{
		return this.headerText;
	}

	// Token: 0x06002FAF RID: 12207 RVA: 0x00113370 File Offset: 0x00111570
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (base.GetComponent<KPrefabID>().PrefabID() != gameObject.GetComponent<KPrefabID>().PrefabID())
		{
			return;
		}
		this.selectedTags.Clear();
		foreach (Tag tag in gameObject.GetComponent<FlatTagFilterable>().selectedTags)
		{
			this.SelectTag(tag, true);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x04001C56 RID: 7254
	[Serialize]
	public List<Tag> selectedTags = new List<Tag>();

	// Token: 0x04001C57 RID: 7255
	public List<Tag> tagOptions = new List<Tag>();

	// Token: 0x04001C58 RID: 7256
	public string headerText;

	// Token: 0x04001C59 RID: 7257
	public bool displayOnlyDiscoveredTags = true;

	// Token: 0x04001C5A RID: 7258
	public bool currentlyUserAssignable = true;
}
