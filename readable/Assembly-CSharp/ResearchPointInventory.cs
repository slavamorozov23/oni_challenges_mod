using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000AFD RID: 2813
public class ResearchPointInventory
{
	// Token: 0x060051E9 RID: 20969 RVA: 0x001DBD24 File Offset: 0x001D9F24
	public ResearchPointInventory()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.PointsByTypeID.Add(researchType.id, 0f);
		}
	}

	// Token: 0x060051EA RID: 20970 RVA: 0x001DBDA0 File Offset: 0x001D9FA0
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.PointsByTypeID.ContainsKey(researchTypeID))
		{
			Debug.LogWarning("Research inventory is missing research point key " + researchTypeID);
			return;
		}
		Dictionary<string, float> pointsByTypeID = this.PointsByTypeID;
		pointsByTypeID[researchTypeID] += points;
	}

	// Token: 0x060051EB RID: 20971 RVA: 0x001DBDE5 File Offset: 0x001D9FE5
	public void RemoveResearchPoints(string researchTypeID, float points)
	{
		this.AddResearchPoints(researchTypeID, -points);
	}

	// Token: 0x060051EC RID: 20972 RVA: 0x001DBDF0 File Offset: 0x001D9FF0
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (!this.PointsByTypeID.ContainsKey(researchType.id))
			{
				this.PointsByTypeID.Add(researchType.id, 0f);
			}
		}
	}

	// Token: 0x0400376F RID: 14191
	public Dictionary<string, float> PointsByTypeID = new Dictionary<string, float>();
}
