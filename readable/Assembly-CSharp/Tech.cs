using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
public class Tech : Resource
{
	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x060051FC RID: 20988 RVA: 0x001DC3C0 File Offset: 0x001DA5C0
	public bool FoundNode
	{
		get
		{
			return this.node != null;
		}
	}

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x060051FD RID: 20989 RVA: 0x001DC3CB File Offset: 0x001DA5CB
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x060051FE RID: 20990 RVA: 0x001DC3D8 File Offset: 0x001DA5D8
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x060051FF RID: 20991 RVA: 0x001DC3E5 File Offset: 0x001DA5E5
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06005200 RID: 20992 RVA: 0x001DC3F2 File Offset: 0x001DA5F2
	public List<ResourceTreeNode.Edge> edges
	{
		get
		{
			return this.node.edges;
		}
	}

	// Token: 0x06005201 RID: 20993 RVA: 0x001DC400 File Offset: 0x001DA600
	public Tech(string id, List<string> unlockedItemIDs, Techs techs, Dictionary<string, float> overrideDefaultCosts = null) : base(id, techs, Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".NAME"))
	{
		this.desc = Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".DESC");
		this.unlockedItemIDs = unlockedItemIDs;
		if (overrideDefaultCosts != null && DlcManager.IsExpansion1Active())
		{
			foreach (KeyValuePair<string, float> keyValuePair in overrideDefaultCosts)
			{
				this.costsByResearchTypeID.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06005202 RID: 20994 RVA: 0x001DC504 File Offset: 0x001DA704
	public void AddUnlockedItemIDs(params string[] ids)
	{
		foreach (string item in ids)
		{
			this.unlockedItemIDs.Add(item);
		}
	}

	// Token: 0x06005203 RID: 20995 RVA: 0x001DC534 File Offset: 0x001DA734
	public void RemoveUnlockedItemIDs(params string[] ids)
	{
		foreach (string text in ids)
		{
			if (!this.unlockedItemIDs.Remove(text))
			{
				DebugUtil.DevLogError("Tech item '" + text + "' does not exist to remove");
			}
		}
	}

	// Token: 0x06005204 RID: 20996 RVA: 0x001DC578 File Offset: 0x001DA778
	public bool RequiresResearchType(string type)
	{
		return this.costsByResearchTypeID.ContainsKey(type) && this.costsByResearchTypeID[type] > 0f;
	}

	// Token: 0x06005205 RID: 20997 RVA: 0x001DC59D File Offset: 0x001DA79D
	public void SetNode(ResourceTreeNode node, string categoryID)
	{
		this.node = node;
		this.category = categoryID;
	}

	// Token: 0x06005206 RID: 20998 RVA: 0x001DC5B0 File Offset: 0x001DA7B0
	public bool CanAfford(ResearchPointInventory pointInventory)
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			if (pointInventory.PointsByTypeID[keyValuePair.Key] < keyValuePair.Value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005207 RID: 20999 RVA: 0x001DC620 File Offset: 0x001DA820
	public string CostString(ResearchTypes types)
	{
		string text = "";
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			text += string.Format("{0}:{1}", types.GetResearchType(keyValuePair.Key).name.ToString(), keyValuePair.Value.ToString());
			text += "\n";
		}
		return text;
	}

	// Token: 0x06005208 RID: 21000 RVA: 0x001DC6B8 File Offset: 0x001DA8B8
	public bool IsComplete()
	{
		if (Research.Instance != null)
		{
			TechInstance techInstance = Research.Instance.Get(this);
			return techInstance != null && techInstance.IsComplete();
		}
		return false;
	}

	// Token: 0x06005209 RID: 21001 RVA: 0x001DC6EC File Offset: 0x001DA8EC
	public bool ArePrerequisitesComplete()
	{
		using (List<Tech>.Enumerator enumerator = this.requiredTech.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600520A RID: 21002 RVA: 0x001DC748 File Offset: 0x001DA948
	public void AddSearchTerms(string newSearchTerms)
	{
		SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
	}

	// Token: 0x04003778 RID: 14200
	public List<Tech> requiredTech = new List<Tech>();

	// Token: 0x04003779 RID: 14201
	public List<Tech> unlockedTech = new List<Tech>();

	// Token: 0x0400377A RID: 14202
	public List<TechItem> unlockedItems = new List<TechItem>();

	// Token: 0x0400377B RID: 14203
	public List<string> unlockedItemIDs = new List<string>();

	// Token: 0x0400377C RID: 14204
	public int tier;

	// Token: 0x0400377D RID: 14205
	public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();

	// Token: 0x0400377E RID: 14206
	public string desc;

	// Token: 0x0400377F RID: 14207
	public string category;

	// Token: 0x04003780 RID: 14208
	public List<string> searchTerms = new List<string>();

	// Token: 0x04003781 RID: 14209
	private ResourceTreeNode node;
}
