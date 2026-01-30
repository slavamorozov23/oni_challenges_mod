using System;
using System.Collections.Generic;

// Token: 0x0200065C RID: 1628
public class IdleTracker : WorldTracker
{
	// Token: 0x06002784 RID: 10116 RVA: 0x000E2F04 File Offset: 0x000E1104
	public IdleTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000E2F10 File Offset: 0x000E1110
	public override void UpdateData()
	{
		this.objectsOfInterest.Clear();
		int num = 0;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (worldItems[i].HasTag(GameTags.Idle))
			{
				num++;
				this.objectsOfInterest.Add(worldItems[i].gameObject);
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x000E2F83 File Offset: 0x000E1183
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
