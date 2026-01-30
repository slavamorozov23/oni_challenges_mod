using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B03 RID: 2819
public class TechItem : Resource, IHasDlcRestrictions
{
	// Token: 0x06005213 RID: 21011 RVA: 0x001DC9D4 File Offset: 0x001DABD4
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06005214 RID: 21012 RVA: 0x001DC9DC File Offset: 0x001DABDC
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x001DC9E4 File Offset: 0x001DABE4
	[Obsolete("Use constructor with requiredDlcIds and forbiddenDlcIds")]
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] dlcIds, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out this.requiredDlcIds, out this.forbiddenDlcIds);
	}

	// Token: 0x06005216 RID: 21014 RVA: 0x001DCA38 File Offset: 0x001DAC38
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06005217 RID: 21015 RVA: 0x001DCA89 File Offset: 0x001DAC89
	public Tech ParentTech
	{
		get
		{
			return Db.Get().Techs.Get(this.parentTechId);
		}
	}

	// Token: 0x06005218 RID: 21016 RVA: 0x001DCAA0 File Offset: 0x001DACA0
	public Sprite UISprite()
	{
		return this.getUISprite("ui", false);
	}

	// Token: 0x06005219 RID: 21017 RVA: 0x001DCAB3 File Offset: 0x001DACB3
	public bool IsComplete()
	{
		return this.ParentTech.IsComplete() || this.IsPOIUnlocked();
	}

	// Token: 0x0600521A RID: 21018 RVA: 0x001DCACC File Offset: 0x001DACCC
	private bool IsPOIUnlocked()
	{
		if (this.isPOIUnlock)
		{
			TechInstance techInstance = Research.Instance.Get(this.ParentTech);
			if (techInstance != null)
			{
				return techInstance.UnlockedPOITechIds.Contains(this.Id);
			}
		}
		return false;
	}

	// Token: 0x0600521B RID: 21019 RVA: 0x001DCB08 File Offset: 0x001DAD08
	public void POIUnlocked()
	{
		DebugUtil.DevAssert(this.isPOIUnlock, "Trying to unlock tech item " + this.Id + " via POI and it's not marked as POI unlockable.", null);
		if (this.isPOIUnlock && !this.IsComplete())
		{
			Research.Instance.Get(this.ParentTech).UnlockPOITech(this.Id);
		}
	}

	// Token: 0x0600521C RID: 21020 RVA: 0x001DCB64 File Offset: 0x001DAD64
	public void AddSearchTerms(List<string> newSearchTerms)
	{
		foreach (string item in newSearchTerms)
		{
			this.searchTerms.Add(item);
		}
	}

	// Token: 0x0600521D RID: 21021 RVA: 0x001DCBB8 File Offset: 0x001DADB8
	public void AddSearchTerms(string newSearchTerms)
	{
		SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
	}

	// Token: 0x04003786 RID: 14214
	public string description;

	// Token: 0x04003787 RID: 14215
	public Func<string, bool, Sprite> getUISprite;

	// Token: 0x04003788 RID: 14216
	public string parentTechId;

	// Token: 0x04003789 RID: 14217
	public bool isPOIUnlock;

	// Token: 0x0400378A RID: 14218
	[Obsolete("Use required/forbidden instead")]
	public string[] dlcIds;

	// Token: 0x0400378B RID: 14219
	public string[] requiredDlcIds;

	// Token: 0x0400378C RID: 14220
	public string[] forbiddenDlcIds;

	// Token: 0x0400378D RID: 14221
	public List<string> searchTerms = new List<string>();
}
