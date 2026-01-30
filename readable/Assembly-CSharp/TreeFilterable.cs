using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000821 RID: 2081
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterable")]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170003CE RID: 974
	// (get) Token: 0x0600389D RID: 14493 RVA: 0x0013CA95 File Offset: 0x0013AC95
	public HashSet<Tag> AcceptedTags
	{
		get
		{
			return this.acceptedTagSet;
		}
	}

	// Token: 0x0600389E RID: 14494 RVA: 0x0013CAA0 File Offset: 0x0013ACA0
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.filterByStorageCategoriesOnSpawn = false;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.acceptedTagSet.UnionWith(this.acceptedTags);
			this.acceptedTagSet.ExceptWith(this.ForbiddenTags);
			this.acceptedTags = null;
		}
	}

	// Token: 0x0600389F RID: 14495 RVA: 0x0013CB0C File Offset: 0x0013AD0C
	private void OnDiscover(Tag category_tag, Tag tag)
	{
		if (this.preventAutoAddOnDiscovery)
		{
			return;
		}
		if (this.storage.storageFilters.Contains(category_tag))
		{
			bool flag = false;
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
			{
				foreach (Tag tag2 in this.storage.storageFilters)
				{
					if (!(tag2 == category_tag) && DiscoveredResources.Instance.IsDiscovered(tag2))
					{
						flag = true;
						foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2))
						{
							if (!this.acceptedTagSet.Contains(item))
							{
								return;
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
			}
			foreach (Tag tag3 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag))
			{
				if (!(tag3 == tag) && !this.acceptedTagSet.Contains(tag3))
				{
					return;
				}
			}
			this.AddTagToFilter(tag);
		}
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x0013CC68 File Offset: 0x0013AE68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<TreeFilterable>(-905833192, TreeFilterable.OnCopySettingsDelegate);
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x0013CC84 File Offset: 0x0013AE84
	protected override void OnSpawn()
	{
		DiscoveredResources.Instance.OnDiscover += this.OnDiscover;
		if (this.storageToFilterTag != Tag.Invalid)
		{
			foreach (Storage storage in base.GetComponents<Storage>())
			{
				if (storage.storageID == this.storageToFilterTag)
				{
					this.storage = storage;
					break;
				}
			}
		}
		if (this.autoSelectStoredOnLoad && this.storage != null)
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
			hashSet.UnionWith(this.storage.GetAllIDsInStorage());
			this.UpdateFilters(hashSet);
		}
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (this.filterByStorageCategoriesOnSpawn)
		{
			this.RemoveIncorrectAcceptedTags();
		}
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x0013CD58 File Offset: 0x0013AF58
	private void RemoveIncorrectAcceptedTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (Tag item in this.acceptedTagSet)
		{
			bool flag = false;
			foreach (Tag tag in this.storage.storageFilters)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(item);
			}
		}
		foreach (Tag t in list)
		{
			this.RemoveTagFromFilter(t);
		}
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x0013CE50 File Offset: 0x0013B050
	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= this.OnDiscover;
		base.OnCleanUp();
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x0013CE70 File Offset: 0x0013B070
	private void OnCopySettings(object data)
	{
		if (this.copySettingsEnabled)
		{
			TreeFilterable component = ((GameObject)data).GetComponent<TreeFilterable>();
			if (component != null)
			{
				this.UpdateFilters(component.GetTags());
			}
		}
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x0013CEA6 File Offset: 0x0013B0A6
	public Storage GetFilterStorage()
	{
		return this.storage;
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x0013CEAE File Offset: 0x0013B0AE
	public HashSet<Tag> GetTags()
	{
		return this.acceptedTagSet;
	}

	// Token: 0x060038A7 RID: 14503 RVA: 0x0013CEB6 File Offset: 0x0013B0B6
	public bool ContainsTag(Tag t)
	{
		return this.acceptedTagSet.Contains(t);
	}

	// Token: 0x060038A8 RID: 14504 RVA: 0x0013CEC4 File Offset: 0x0013B0C4
	public void AddTagToFilter(Tag t)
	{
		if (this.ContainsTag(t))
		{
			return;
		}
		this.UpdateFilters(new HashSet<Tag>(this.acceptedTagSet)
		{
			t
		});
	}

	// Token: 0x060038A9 RID: 14505 RVA: 0x0013CEF8 File Offset: 0x0013B0F8
	public void RemoveTagFromFilter(Tag t)
	{
		if (!this.ContainsTag(t))
		{
			return;
		}
		HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
		hashSet.Remove(t);
		this.UpdateFilters(hashSet);
	}

	// Token: 0x060038AA RID: 14506 RVA: 0x0013CF2C File Offset: 0x0013B12C
	public void UpdateFilters(HashSet<Tag> filters)
	{
		this.acceptedTagSet.Clear();
		this.acceptedTagSet.UnionWith(filters);
		this.acceptedTagSet.ExceptWith(this.ForbiddenTags);
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (!this.dropIncorrectOnFilterChange || this.storage == null || this.storage.items == null)
		{
			return;
		}
		if (!this.filterAllStoragesOnBuilding)
		{
			this.DropFilteredItemsFromTargetStorage(this.storage);
			return;
		}
		foreach (Storage targetStorage in base.GetComponents<Storage>())
		{
			this.DropFilteredItemsFromTargetStorage(targetStorage);
		}
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x0013CFDC File Offset: 0x0013B1DC
	private void DropFilteredItemsFromTargetStorage(Storage targetStorage)
	{
		for (int i = targetStorage.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = targetStorage.items[i];
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!this.acceptedTagSet.Contains(component.PrefabTag))
				{
					targetStorage.Drop(gameObject, true);
				}
			}
		}
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x0013D03C File Offset: 0x0013B23C
	public string GetTagsAsStatus(int maxDisplays = 6)
	{
		string text = "Tags:\n";
		List<Tag> list = new List<Tag>(this.storage.storageFilters);
		list.Intersect(this.acceptedTagSet);
		for (int i = 0; i < Mathf.Min(list.Count, maxDisplays); i++)
		{
			text += list[i].ProperName();
			if (i < Mathf.Min(list.Count, maxDisplays) - 1)
			{
				text += "\n";
			}
			if (i == maxDisplays - 1 && list.Count > maxDisplays)
			{
				text += "\n...";
				break;
			}
		}
		if (base.tag.Length == 0)
		{
			text = "No tags selected";
		}
		return text;
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x0013D0E8 File Offset: 0x0013B2E8
	private void RefreshTint()
	{
		bool flag = this.acceptedTagSet != null && this.acceptedTagSet.Count != 0;
		if (this.tintOnNoFiltersSet)
		{
			base.GetComponent<KBatchedAnimController>().TintColour = (flag ? this.filterTint : this.noFilterTint);
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoStorageFilterSet, !flag, this);
	}

	// Token: 0x0400226A RID: 8810
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400226B RID: 8811
	public Tag storageToFilterTag = Tag.Invalid;

	// Token: 0x0400226C RID: 8812
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400226D RID: 8813
	public static readonly Color32 FILTER_TINT = Color.white;

	// Token: 0x0400226E RID: 8814
	public static readonly Color32 NO_FILTER_TINT = new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);

	// Token: 0x0400226F RID: 8815
	public Color32 filterTint = TreeFilterable.FILTER_TINT;

	// Token: 0x04002270 RID: 8816
	public Color32 noFilterTint = TreeFilterable.NO_FILTER_TINT;

	// Token: 0x04002271 RID: 8817
	[SerializeField]
	public bool dropIncorrectOnFilterChange = true;

	// Token: 0x04002272 RID: 8818
	[SerializeField]
	public bool autoSelectStoredOnLoad = true;

	// Token: 0x04002273 RID: 8819
	public bool showUserMenu = true;

	// Token: 0x04002274 RID: 8820
	public bool copySettingsEnabled = true;

	// Token: 0x04002275 RID: 8821
	public bool preventAutoAddOnDiscovery;

	// Token: 0x04002276 RID: 8822
	public string allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;

	// Token: 0x04002277 RID: 8823
	public bool filterAllStoragesOnBuilding;

	// Token: 0x04002278 RID: 8824
	public bool tintOnNoFiltersSet = true;

	// Token: 0x04002279 RID: 8825
	public TreeFilterable.UISideScreenHeight uiHeight = TreeFilterable.UISideScreenHeight.Tall;

	// Token: 0x0400227A RID: 8826
	public bool filterByStorageCategoriesOnSpawn = true;

	// Token: 0x0400227B RID: 8827
	[SerializeField]
	[Serialize]
	[Obsolete("Deprecated, use acceptedTagSet")]
	private List<Tag> acceptedTags = new List<Tag>();

	// Token: 0x0400227C RID: 8828
	[SerializeField]
	[Serialize]
	private HashSet<Tag> acceptedTagSet = new HashSet<Tag>();

	// Token: 0x0400227D RID: 8829
	public HashSet<Tag> ForbiddenTags = new HashSet<Tag>();

	// Token: 0x0400227E RID: 8830
	public Action<HashSet<Tag>> OnFilterChanged;

	// Token: 0x0400227F RID: 8831
	private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>(delegate(TreeFilterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020017C4 RID: 6084
	public enum UISideScreenHeight
	{
		// Token: 0x040078AF RID: 30895
		Short,
		// Token: 0x040078B0 RID: 30896
		Tall
	}
}
