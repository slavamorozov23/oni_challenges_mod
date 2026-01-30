using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x0200065D RID: 1629
public class RadiationTracker : WorldTracker
{
	// Token: 0x06002787 RID: 10119 RVA: 0x000E2F8C File Offset: 0x000E118C
	public RadiationTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x000E2F98 File Offset: 0x000E1198
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(base.WorldID, false);
		if (worldItems.Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		foreach (MinionIdentity cmp in worldItems)
		{
			num += cmp.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value;
		}
		float value = num / (float)worldItems.Count;
		base.AddPoint(value);
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x000E3048 File Offset: 0x000E1248
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}
}
