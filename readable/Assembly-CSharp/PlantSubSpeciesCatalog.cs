using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA6 RID: 2726
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantSubSpeciesCatalog : KMonoBehaviour
{
	// Token: 0x06004EF9 RID: 20217 RVA: 0x001CAA5F File Offset: 0x001C8C5F
	public static void DestroyInstance()
	{
		PlantSubSpeciesCatalog.Instance = null;
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06004EFA RID: 20218 RVA: 0x001CAA68 File Offset: 0x001C8C68
	public bool AnyNonOriginalDiscovered
	{
		get
		{
			foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
			{
				if (keyValuePair.Value.Find((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !ss.IsOriginal).IsValid)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06004EFB RID: 20219 RVA: 0x001CAAF0 File Offset: 0x001C8CF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PlantSubSpeciesCatalog.Instance = this;
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x001CAAFE File Offset: 0x001C8CFE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.EnsureOriginalSubSpecies();
		this.RemoveInvalidMutantPlants();
	}

	// Token: 0x06004EFD RID: 20221 RVA: 0x001CAB12 File Offset: 0x001C8D12
	public List<Tag> GetAllDiscoveredSpecies()
	{
		return this.discoveredSubspeciesBySpecies.Keys.ToList<Tag>();
	}

	// Token: 0x06004EFE RID: 20222 RVA: 0x001CAB24 File Offset: 0x001C8D24
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllSubSpeciesForSpecies(Tag speciesID)
	{
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> result;
		if (this.discoveredSubspeciesBySpecies.TryGetValue(speciesID, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004EFF RID: 20223 RVA: 0x001CAB44 File Offset: 0x001C8D44
	public bool GetOriginalSubSpecies(Tag speciesID, out PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo)
	{
		if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
		{
			subSpeciesInfo = default(PlantSubSpeciesCatalog.SubSpeciesInfo);
			return false;
		}
		subSpeciesInfo = this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.IsOriginal);
		return true;
	}

	// Token: 0x06004F00 RID: 20224 RVA: 0x001CABA0 File Offset: 0x001C8DA0
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpecies(Tag speciesID, Tag subSpeciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID);
	}

	// Token: 0x06004F01 RID: 20225 RVA: 0x001CABD8 File Offset: 0x001C8DD8
	public PlantSubSpeciesCatalog.SubSpeciesInfo FindSubSpecies(Tag subSpeciesID)
	{
		Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> <>9__0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
		{
			List<PlantSubSpeciesCatalog.SubSpeciesInfo> value = keyValuePair.Value;
			Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> match;
			if ((match = <>9__0) == null)
			{
				match = (<>9__0 = ((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID));
			}
			PlantSubSpeciesCatalog.SubSpeciesInfo result = value.Find(match);
			if (result.ID.IsValid)
			{
				return result;
			}
		}
		return default(PlantSubSpeciesCatalog.SubSpeciesInfo);
	}

	// Token: 0x06004F02 RID: 20226 RVA: 0x001CAC80 File Offset: 0x001C8E80
	public void DiscoverSubSpecies(PlantSubSpeciesCatalog.SubSpeciesInfo newSubSpeciesInfo, MutantPlant source)
	{
		if (!this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Contains(newSubSpeciesInfo))
		{
			this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Add(newSubSpeciesInfo);
			Notification notification = new Notification(MISC.NOTIFICATIONS.NEWMUTANTSEED.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(this.NewSubspeciesTooltipCB), newSubSpeciesInfo, true, 0f, null, null, source.transform, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x06004F03 RID: 20227 RVA: 0x001CAD08 File Offset: 0x001C8F08
	private string NewSubspeciesTooltipCB(List<Notification> notifications, object data)
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = (PlantSubSpeciesCatalog.SubSpeciesInfo)data;
		return MISC.NOTIFICATIONS.NEWMUTANTSEED.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x06004F04 RID: 20228 RVA: 0x001CAD38 File Offset: 0x001C8F38
	public void IdentifySubSpecies(Tag subSpeciesID)
	{
		if (this.identifiedSubSpecies.Add(subSpeciesID))
		{
			this.FindSubSpecies(subSpeciesID);
			foreach (object obj in Components.MutantPlants)
			{
				MutantPlant mutantPlant = (MutantPlant)obj;
				if (mutantPlant.HasTag(subSpeciesID))
				{
					mutantPlant.UpdateNameAndTags();
				}
			}
			GeneticAnalysisCompleteMessage message = new GeneticAnalysisCompleteMessage(subSpeciesID);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x06004F05 RID: 20229 RVA: 0x001CADC0 File Offset: 0x001C8FC0
	public bool IsSubSpeciesIdentified(Tag subSpeciesID)
	{
		return this.identifiedSubSpecies.Contains(subSpeciesID);
	}

	// Token: 0x06004F06 RID: 20230 RVA: 0x001CADCE File Offset: 0x001C8FCE
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllUnidentifiedSubSpecies(Tag speciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].FindAll((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !this.IsSubSpeciesIdentified(ss.ID));
	}

	// Token: 0x06004F07 RID: 20231 RVA: 0x001CADF0 File Offset: 0x001C8FF0
	public bool IsValidPlantableSeed(Tag seedID, Tag subspeciesID)
	{
		if (!seedID.IsValid)
		{
			return false;
		}
		MutantPlant component = Assets.GetPrefab(seedID).GetComponent<MutantPlant>();
		if (component == null)
		{
			return !subspeciesID.IsValid;
		}
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(component.SpeciesID);
		return allSubSpeciesForSpecies != null && allSubSpeciesForSpecies.FindIndex((PlantSubSpeciesCatalog.SubSpeciesInfo s) => s.ID == subspeciesID) != -1 && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subspeciesID);
	}

	// Token: 0x06004F08 RID: 20232 RVA: 0x001CAE74 File Offset: 0x001C9074
	private void EnsureOriginalSubSpecies()
	{
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<MutantPlant>())
		{
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			Tag speciesID = component.SpeciesID;
			if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
			{
				this.discoveredSubspeciesBySpecies[speciesID] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>();
				this.discoveredSubspeciesBySpecies[speciesID].Add(component.GetSubSpeciesInfo());
			}
			this.identifiedSubSpecies.Add(component.SubSpeciesID);
		}
	}

	// Token: 0x06004F09 RID: 20233 RVA: 0x001CAF14 File Offset: 0x001C9114
	private void RemoveInvalidMutantPlants()
	{
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
		{
			GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
			if (prefab != null && prefab.GetComponent<MutantPlant>() == null)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			foreach (PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo in this.discoveredSubspeciesBySpecies[key])
			{
				this.identifiedSubSpecies.Remove(subSpeciesInfo.ID);
			}
			this.discoveredSubspeciesBySpecies.Remove(key);
		}
	}

	// Token: 0x040034CA RID: 13514
	public static PlantSubSpeciesCatalog Instance;

	// Token: 0x040034CB RID: 13515
	[Serialize]
	private Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> discoveredSubspeciesBySpecies = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();

	// Token: 0x040034CC RID: 13516
	[Serialize]
	private HashSet<Tag> identifiedSubSpecies = new HashSet<Tag>();

	// Token: 0x02001BCE RID: 7118
	[Serializable]
	public struct SubSpeciesInfo : IEquatable<PlantSubSpeciesCatalog.SubSpeciesInfo>
	{
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x0600AB39 RID: 43833 RVA: 0x003C7F5F File Offset: 0x003C615F
		public bool IsValid
		{
			get
			{
				return this.ID.IsValid;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x0600AB3A RID: 43834 RVA: 0x003C7F6C File Offset: 0x003C616C
		public bool IsOriginal
		{
			get
			{
				return this.mutationIDs == null || this.mutationIDs.Count == 0;
			}
		}

		// Token: 0x0600AB3B RID: 43835 RVA: 0x003C7F86 File Offset: 0x003C6186
		public SubSpeciesInfo(Tag speciesID, List<string> mutationIDs)
		{
			this.speciesID = speciesID;
			this.mutationIDs = ((mutationIDs != null) ? new List<string>(mutationIDs) : new List<string>());
			this.ID = PlantSubSpeciesCatalog.SubSpeciesInfo.SubSpeciesIDFromMutations(speciesID, mutationIDs);
		}

		// Token: 0x0600AB3C RID: 43836 RVA: 0x003C7FB4 File Offset: 0x003C61B4
		public static Tag SubSpeciesIDFromMutations(Tag speciesID, List<string> mutationIDs)
		{
			if (mutationIDs == null || mutationIDs.Count == 0)
			{
				Tag tag = speciesID;
				return tag.ToString() + "_Original";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(speciesID);
			foreach (string value in mutationIDs)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString().ToTag();
		}

		// Token: 0x0600AB3D RID: 43837 RVA: 0x003C8058 File Offset: 0x003C6258
		public string GetMutationsNames()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.PLANT_MUTATIONS.NONE.NAME;
			}
			return string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs));
		}

		// Token: 0x0600AB3E RID: 43838 RVA: 0x003C80A4 File Offset: 0x003C62A4
		public string GetNameWithMutations(string properName, bool identified, bool cleanOriginal)
		{
			string result;
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				if (cleanOriginal)
				{
					result = properName;
				}
				else
				{
					result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.NONE.NAME);
				}
			}
			else if (!identified)
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.UNIDENTIFIED);
			}
			else
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs)));
			}
			return result;
		}

		// Token: 0x0600AB3F RID: 43839 RVA: 0x003C815C File Offset: 0x003C635C
		public static bool operator ==(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return obj1.Equals(obj2);
		}

		// Token: 0x0600AB40 RID: 43840 RVA: 0x003C8166 File Offset: 0x003C6366
		public static bool operator !=(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return !(obj1 == obj2);
		}

		// Token: 0x0600AB41 RID: 43841 RVA: 0x003C8172 File Offset: 0x003C6372
		public override bool Equals(object other)
		{
			return other is PlantSubSpeciesCatalog.SubSpeciesInfo && this == (PlantSubSpeciesCatalog.SubSpeciesInfo)other;
		}

		// Token: 0x0600AB42 RID: 43842 RVA: 0x003C818F File Offset: 0x003C638F
		public bool Equals(PlantSubSpeciesCatalog.SubSpeciesInfo other)
		{
			return this.ID == other.ID;
		}

		// Token: 0x0600AB43 RID: 43843 RVA: 0x003C81A2 File Offset: 0x003C63A2
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x0600AB44 RID: 43844 RVA: 0x003C81B8 File Offset: 0x003C63B8
		public string GetMutationsTooltip()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.STATUSITEMS.ORIGINALPLANTMUTATION.TOOLTIP;
			}
			if (!PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.ID))
			{
				return CREATURES.STATUSITEMS.UNKNOWNMUTATION.TOOLTIP;
			}
			string id = this.mutationIDs[0];
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			return CREATURES.STATUSITEMS.SPECIFICPLANTMUTATION.TOOLTIP.Replace("{MutationName}", plantMutation.Name) + "\n" + plantMutation.GetTooltip();
		}

		// Token: 0x040085C3 RID: 34243
		public Tag speciesID;

		// Token: 0x040085C4 RID: 34244
		public Tag ID;

		// Token: 0x040085C5 RID: 34245
		public List<string> mutationIDs;

		// Token: 0x040085C6 RID: 34246
		private const string ORIGINAL_SUFFIX = "_Original";
	}
}
