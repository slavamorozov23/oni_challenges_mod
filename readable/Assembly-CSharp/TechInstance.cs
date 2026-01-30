using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
public class TechInstance
{
	// Token: 0x0600520B RID: 21003 RVA: 0x001DC756 File Offset: 0x001DA956
	public TechInstance(Tech tech)
	{
		this.tech = tech;
	}

	// Token: 0x0600520C RID: 21004 RVA: 0x001DC77B File Offset: 0x001DA97B
	public bool IsComplete()
	{
		return this.complete;
	}

	// Token: 0x0600520D RID: 21005 RVA: 0x001DC783 File Offset: 0x001DA983
	public void Purchased()
	{
		if (!this.complete)
		{
			this.complete = true;
		}
	}

	// Token: 0x0600520E RID: 21006 RVA: 0x001DC794 File Offset: 0x001DA994
	public void UnlockPOITech(string tech_id)
	{
		TechItem techItem = Db.Get().TechItems.Get(tech_id);
		if (techItem == null || !techItem.isPOIUnlock)
		{
			return;
		}
		if (!this.UnlockedPOITechIds.Contains(tech_id))
		{
			this.UnlockedPOITechIds.Add(tech_id);
			BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
			if (buildingDef != null)
			{
				Game.Instance.Trigger(-107300940, buildingDef);
			}
		}
	}

	// Token: 0x0600520F RID: 21007 RVA: 0x001DC800 File Offset: 0x001DAA00
	public float GetTotalPercentageComplete()
	{
		float num = 0f;
		int num2 = 0;
		foreach (string type in this.progressInventory.PointsByTypeID.Keys)
		{
			if (this.tech.RequiresResearchType(type))
			{
				num += this.PercentageCompleteResearchType(type);
				num2++;
			}
		}
		return num / (float)num2;
	}

	// Token: 0x06005210 RID: 21008 RVA: 0x001DC880 File Offset: 0x001DAA80
	public float PercentageCompleteResearchType(string type)
	{
		if (!this.tech.RequiresResearchType(type))
		{
			return 1f;
		}
		return Mathf.Clamp01(this.progressInventory.PointsByTypeID[type] / this.tech.costsByResearchTypeID[type]);
	}

	// Token: 0x06005211 RID: 21009 RVA: 0x001DC8C0 File Offset: 0x001DAAC0
	public TechInstance.SaveData Save()
	{
		string[] array = new string[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Keys.CopyTo(array, 0);
		float[] array2 = new float[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
		string[] unlockedPOIIDs = this.UnlockedPOITechIds.ToArray();
		return new TechInstance.SaveData
		{
			techId = this.tech.Id,
			complete = this.complete,
			inventoryIDs = array,
			inventoryValues = array2,
			unlockedPOIIDs = unlockedPOIIDs
		};
	}

	// Token: 0x06005212 RID: 21010 RVA: 0x001DC974 File Offset: 0x001DAB74
	public void Load(TechInstance.SaveData save_data)
	{
		this.complete = save_data.complete;
		for (int i = 0; i < save_data.inventoryIDs.Length; i++)
		{
			this.progressInventory.AddResearchPoints(save_data.inventoryIDs[i], save_data.inventoryValues[i]);
		}
		if (save_data.unlockedPOIIDs != null)
		{
			this.UnlockedPOITechIds = new List<string>(save_data.unlockedPOIIDs);
		}
	}

	// Token: 0x04003782 RID: 14210
	public Tech tech;

	// Token: 0x04003783 RID: 14211
	private bool complete;

	// Token: 0x04003784 RID: 14212
	public ResearchPointInventory progressInventory = new ResearchPointInventory();

	// Token: 0x04003785 RID: 14213
	public List<string> UnlockedPOITechIds = new List<string>();

	// Token: 0x02001C3B RID: 7227
	public struct SaveData
	{
		// Token: 0x04008751 RID: 34641
		public string techId;

		// Token: 0x04008752 RID: 34642
		public bool complete;

		// Token: 0x04008753 RID: 34643
		public string[] inventoryIDs;

		// Token: 0x04008754 RID: 34644
		public float[] inventoryValues;

		// Token: 0x04008755 RID: 34645
		public string[] unlockedPOIIDs;
	}
}
