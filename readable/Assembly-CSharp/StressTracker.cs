using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000659 RID: 1625
public class StressTracker : WorldTracker
{
	// Token: 0x0600277B RID: 10107 RVA: 0x000E2DEA File Offset: 0x000E0FEA
	public StressTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000E2DF4 File Offset: 0x000E0FF4
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			if (Components.LiveMinionIdentities[i].GetMyWorldId() == base.WorldID)
			{
				num = Mathf.Max(num, Components.LiveMinionIdentities[i].gameObject.GetAmounts().GetValue(Db.Get().Amounts.Stress.Id));
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000E2E75 File Offset: 0x000E1075
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
