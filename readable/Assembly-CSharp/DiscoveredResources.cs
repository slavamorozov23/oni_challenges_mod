using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x020008F6 RID: 2294
[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
	// Token: 0x06003FA5 RID: 16293 RVA: 0x001662A2 File Offset: 0x001644A2
	public static void DestroyInstance()
	{
		DiscoveredResources.Instance = null;
	}

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06003FA6 RID: 16294 RVA: 0x001662AC File Offset: 0x001644AC
	// (remove) Token: 0x06003FA7 RID: 16295 RVA: 0x001662E4 File Offset: 0x001644E4
	public event Action<Tag, Tag> OnDiscover;

	// Token: 0x06003FA8 RID: 16296 RVA: 0x0016631C File Offset: 0x0016451C
	public void Discover(Tag tag, Tag categoryTag)
	{
		bool flag = this.Discovered.Add(tag);
		this.DiscoverCategory(categoryTag, tag);
		if (flag)
		{
			if (this.OnDiscover != null)
			{
				this.OnDiscover(categoryTag, tag);
			}
			if (!this.newDiscoveries.ContainsKey(tag))
			{
				this.newDiscoveries.Add(tag, (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage());
			}
		}
	}

	// Token: 0x06003FA9 RID: 16297 RVA: 0x00166384 File Offset: 0x00164584
	public void Discover(Tag tag)
	{
		this.Discover(tag, DiscoveredResources.GetCategoryForEntity(Assets.GetPrefab(tag).GetComponent<KPrefabID>()));
	}

	// Token: 0x06003FAA RID: 16298 RVA: 0x0016639D File Offset: 0x0016459D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DiscoveredResources.Instance = this;
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x001663AB File Offset: 0x001645AB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FilterDisabledContent();
	}

	// Token: 0x06003FAC RID: 16300 RVA: 0x001663BC File Offset: 0x001645BC
	private void FilterDisabledContent()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		foreach (Tag tag in this.Discovered)
		{
			Element element = ElementLoader.GetElement(tag);
			if (element != null && element.disabled)
			{
				hashSet.Add(tag);
			}
			else
			{
				GameObject gameObject = Assets.TryGetPrefab(tag);
				if (gameObject != null && gameObject.HasTag(GameTags.DeprecatedContent))
				{
					hashSet.Add(tag);
				}
				else if (gameObject == null)
				{
					hashSet.Add(tag);
				}
			}
		}
		foreach (Tag item in hashSet)
		{
			this.Discovered.Remove(item);
		}
		foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in this.DiscoveredCategories)
		{
			foreach (Tag item2 in hashSet)
			{
				if (keyValuePair.Value.Contains(item2))
				{
					keyValuePair.Value.Remove(item2);
				}
			}
		}
		foreach (string s in new List<string>
		{
			"Pacu",
			"PacuCleaner",
			"PacuTropical",
			"PacuBaby",
			"PacuCleanerBaby",
			"PacuTropicalBaby"
		})
		{
			if (this.DiscoveredCategories.ContainsKey(s))
			{
				List<Tag> list = this.DiscoveredCategories[s].ToList<Tag>();
				SolidConsumerMonitor.Def def = Assets.GetPrefab(s).GetDef<SolidConsumerMonitor.Def>();
				foreach (Tag tag2 in list)
				{
					if (def.diet.GetDietInfo(tag2) == null)
					{
						this.DiscoveredCategories[s].Remove(tag2);
					}
				}
			}
		}
		if (this.DiscoveredCategories.ContainsKey(GameTags.IndustrialIngredient))
		{
			foreach (string s2 in new List<string>
			{
				"CrabShell",
				"CrabWoodShell"
			})
			{
				if (this.DiscoveredCategories[GameTags.IndustrialIngredient].Contains(s2))
				{
					this.DiscoveredCategories[GameTags.IndustrialIngredient].Remove(s2);
					this.DiscoverCategory(GameTags.Organics, s2);
				}
			}
		}
		if (this.DiscoveredCategories.ContainsKey(GameTags.IndustrialIngredient))
		{
			foreach (string s3 in new List<string>
			{
				"OrbitalResearchDatabank",
				"ResearchDatabank"
			})
			{
				if (this.DiscoveredCategories[GameTags.IndustrialIngredient].Contains(s3))
				{
					this.DiscoveredCategories[GameTags.IndustrialIngredient].Remove(s3);
					this.DiscoverCategory(GameTags.TechComponents, s3);
				}
			}
		}
	}

	// Token: 0x06003FAD RID: 16301 RVA: 0x001667CC File Offset: 0x001649CC
	public bool CheckAllDiscoveredAreNew()
	{
		foreach (Tag key in this.Discovered)
		{
			if (!this.newDiscoveries.ContainsKey(key))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003FAE RID: 16302 RVA: 0x00166830 File Offset: 0x00164A30
	private void DiscoverCategory(Tag category_tag, Tag item_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.DiscoveredCategories.TryGetValue(category_tag, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.DiscoveredCategories[category_tag] = hashSet;
		}
		hashSet.Add(item_tag);
	}

	// Token: 0x06003FAF RID: 16303 RVA: 0x00166868 File Offset: 0x00164A68
	public HashSet<Tag> GetDiscovered()
	{
		return this.Discovered;
	}

	// Token: 0x06003FB0 RID: 16304 RVA: 0x00166870 File Offset: 0x00164A70
	public bool IsDiscovered(Tag tag)
	{
		return this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);
	}

	// Token: 0x06003FB1 RID: 16305 RVA: 0x00166890 File Offset: 0x00164A90
	public bool AnyDiscovered(ICollection<Tag> tags)
	{
		foreach (Tag tag in tags)
		{
			if (this.IsDiscovered(tag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003FB2 RID: 16306 RVA: 0x001668E4 File Offset: 0x00164AE4
	public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
	{
		return this.DiscoveredCategories.TryGetValue(tag, out resources);
	}

	// Token: 0x06003FB3 RID: 16307 RVA: 0x001668F4 File Offset: 0x00164AF4
	public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
	{
		HashSet<Tag> result;
		if (this.DiscoveredCategories.TryGetValue(tag, out result))
		{
			return result;
		}
		return new HashSet<Tag>();
	}

	// Token: 0x06003FB4 RID: 16308 RVA: 0x00166918 File Offset: 0x00164B18
	public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(TagSet tagSet)
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		foreach (Tag key in tagSet)
		{
			HashSet<Tag> value;
			if (this.DiscoveredCategories.TryGetValue(key, out value))
			{
				dictionary[key] = value;
			}
		}
		return dictionary;
	}

	// Token: 0x06003FB5 RID: 16309 RVA: 0x00166978 File Offset: 0x00164B78
	public static Tag GetCategoryForTags(HashSet<Tag> tags)
	{
		Tag result = Tag.Invalid;
		foreach (Tag tag in tags)
		{
			if (GameTags.AllCategories.Contains(tag) || GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				result = tag;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003FB6 RID: 16310 RVA: 0x001669E4 File Offset: 0x00164BE4
	public static Tag GetCategoryForEntity(KPrefabID entity)
	{
		ElementChunk component = entity.GetComponent<ElementChunk>();
		if (component != null)
		{
			return component.GetComponent<PrimaryElement>().Element.materialCategory;
		}
		return DiscoveredResources.GetCategoryForTags(entity.Tags);
	}

	// Token: 0x06003FB7 RID: 16311 RVA: 0x00166A20 File Offset: 0x00164C20
	public void Sim4000ms(float dt)
	{
		float num = GameClock.Instance.GetTimeInCycles() + GameClock.Instance.GetCurrentCycleAsPercentage();
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, float> keyValuePair in this.newDiscoveries)
		{
			if (num - keyValuePair.Value > 3f)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.newDiscoveries.Remove(key);
		}
	}

	// Token: 0x0400276C RID: 10092
	public static DiscoveredResources Instance;

	// Token: 0x0400276D RID: 10093
	[Serialize]
	private HashSet<Tag> Discovered = new HashSet<Tag>();

	// Token: 0x0400276E RID: 10094
	[Serialize]
	private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();

	// Token: 0x04002770 RID: 10096
	[Serialize]
	public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();
}
