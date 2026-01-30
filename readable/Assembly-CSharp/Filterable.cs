using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000757 RID: 1879
[AddComponentMenu("KMonoBehaviour/scripts/Filterable")]
public class Filterable : KMonoBehaviour
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06002F81 RID: 12161 RVA: 0x001127D0 File Offset: 0x001109D0
	// (remove) Token: 0x06002F82 RID: 12162 RVA: 0x00112808 File Offset: 0x00110A08
	public event Action<Tag> onFilterChanged;

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06002F83 RID: 12163 RVA: 0x0011283D File Offset: 0x00110A3D
	// (set) Token: 0x06002F84 RID: 12164 RVA: 0x00112845 File Offset: 0x00110A45
	public Tag SelectedTag
	{
		get
		{
			return this.selectedTag;
		}
		set
		{
			this.selectedTag = value;
			this.OnFilterChanged();
		}
	}

	// Token: 0x06002F85 RID: 12165 RVA: 0x00112854 File Offset: 0x00110A54
	public Dictionary<Tag, HashSet<Tag>> GetTagOptions()
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		if (this.filterElementState == Filterable.ElementState.Solid)
		{
			dictionary = DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(Filterable.filterableCategories);
		}
		else
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (!element.disabled && ((element.IsGas && this.filterElementState == Filterable.ElementState.Gas) || (element.IsLiquid && this.filterElementState == Filterable.ElementState.Liquid)))
				{
					Tag materialCategoryTag = element.GetMaterialCategoryTag();
					if (!dictionary.ContainsKey(materialCategoryTag))
					{
						dictionary[materialCategoryTag] = new HashSet<Tag>();
					}
					Tag item = GameTagExtensions.Create(element.id);
					dictionary[materialCategoryTag].Add(item);
				}
			}
		}
		dictionary.Add(GameTags.Void, new HashSet<Tag>
		{
			GameTags.Void
		});
		return dictionary;
	}

	// Token: 0x06002F86 RID: 12166 RVA: 0x00112944 File Offset: 0x00110B44
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Filterable>(-905833192, Filterable.OnCopySettingsDelegate);
	}

	// Token: 0x06002F87 RID: 12167 RVA: 0x00112960 File Offset: 0x00110B60
	private void OnCopySettings(object data)
	{
		Filterable component = ((GameObject)data).GetComponent<Filterable>();
		if (component != null)
		{
			this.SelectedTag = component.SelectedTag;
		}
	}

	// Token: 0x06002F88 RID: 12168 RVA: 0x0011298E File Offset: 0x00110B8E
	protected override void OnSpawn()
	{
		this.OnFilterChanged();
	}

	// Token: 0x06002F89 RID: 12169 RVA: 0x00112998 File Offset: 0x00110B98
	private void OnFilterChanged()
	{
		if (this.onFilterChanged != null)
		{
			this.onFilterChanged(this.selectedTag);
		}
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Filterable.filterSelected, this.selectedTag.IsValid);
		}
	}

	// Token: 0x04001C3F RID: 7231
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C40 RID: 7232
	[Serialize]
	public Filterable.ElementState filterElementState;

	// Token: 0x04001C41 RID: 7233
	[Serialize]
	private Tag selectedTag = GameTags.Void;

	// Token: 0x04001C43 RID: 7235
	private static TagSet filterableCategories = new TagSet(new TagSet[]
	{
		GameTags.CalorieCategories,
		GameTags.UnitCategories,
		GameTags.MaterialCategories,
		GameTags.MaterialBuildingElements
	});

	// Token: 0x04001C44 RID: 7236
	private static readonly Operational.Flag filterSelected = new Operational.Flag("filterSelected", Operational.Flag.Type.Requirement);

	// Token: 0x04001C45 RID: 7237
	private static readonly EventSystem.IntraObjectHandler<Filterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Filterable>(delegate(Filterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200163B RID: 5691
	public enum ElementState
	{
		// Token: 0x04007420 RID: 29728
		None,
		// Token: 0x04007421 RID: 29729
		Solid,
		// Token: 0x04007422 RID: 29730
		Liquid,
		// Token: 0x04007423 RID: 29731
		Gas
	}
}
