using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F5 RID: 2293
[AddComponentMenu("KMonoBehaviour/scripts/DietManager")]
public class DietManager : KMonoBehaviour
{
	// Token: 0x06003F9C RID: 16284 RVA: 0x00165F22 File Offset: 0x00164122
	public static void DestroyInstance()
	{
		DietManager.Instance = null;
	}

	// Token: 0x06003F9D RID: 16285 RVA: 0x00165F2A File Offset: 0x0016412A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.diets = DietManager.CollectSaveDiets(null);
		DietManager.Instance = this;
	}

	// Token: 0x06003F9E RID: 16286 RVA: 0x00165F44 File Offset: 0x00164144
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscovered())
		{
			this.Discover(tag);
		}
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			Diet.Info[] infos = keyValuePair.Value.infos;
			for (int i = 0; i < infos.Length; i++)
			{
				foreach (Tag tag2 in infos[i].consumedTags)
				{
					if (Assets.GetPrefab(tag2) == null)
					{
						global::Debug.LogError(string.Format("Could not find prefab {0}, required by diet for {1}", tag2, keyValuePair.Key));
					}
				}
			}
		}
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
	}

	// Token: 0x06003F9F RID: 16287 RVA: 0x0016608C File Offset: 0x0016428C
	private void Discover(Tag tag)
	{
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			if (keyValuePair.Value.GetDietInfo(tag) != null)
			{
				DiscoveredResources.Instance.Discover(tag, keyValuePair.Key);
			}
		}
	}

	// Token: 0x06003FA0 RID: 16288 RVA: 0x001660FC File Offset: 0x001642FC
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		this.Discover(tag);
	}

	// Token: 0x06003FA1 RID: 16289 RVA: 0x00166108 File Offset: 0x00164308
	public static Dictionary<Tag, Diet> CollectDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = diet;
			}
		}
		return dictionary;
	}

	// Token: 0x06003FA2 RID: 16290 RVA: 0x001661B0 File Offset: 0x001643B0
	public static Dictionary<Tag, Diet> CollectSaveDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = new Diet(diet);
				dictionary[kprefabID.PrefabTag].FilterDLC();
			}
		}
		return dictionary;
	}

	// Token: 0x06003FA3 RID: 16291 RVA: 0x00166270 File Offset: 0x00164470
	public Diet GetPrefabDiet(GameObject owner)
	{
		Diet result;
		if (this.diets.TryGetValue(owner.GetComponent<KPrefabID>().PrefabTag, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0400276A RID: 10090
	private Dictionary<Tag, Diet> diets;

	// Token: 0x0400276B RID: 10091
	public static DietManager Instance;
}
