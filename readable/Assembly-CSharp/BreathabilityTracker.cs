using System;
using UnityEngine;

// Token: 0x02000654 RID: 1620
public class BreathabilityTracker : WorldTracker
{
	// Token: 0x0600276A RID: 10090 RVA: 0x000E2A67 File Offset: 0x000E0C67
	public BreathabilityTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600276B RID: 10091 RVA: 0x000E2A70 File Offset: 0x000E0C70
	public override void UpdateData()
	{
		float num = 0f;
		if (Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false).Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		int num2 = 0;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false))
		{
			OxygenBreather component = minionIdentity.GetComponent<OxygenBreather>();
			if (!(component == null))
			{
				OxygenBreather.IGasProvider currentGasProvider = component.GetCurrentGasProvider();
				num2++;
				if (!component.IsOutOfOxygen)
				{
					num += 100f;
					if (currentGasProvider.IsLowOxygen())
					{
						num -= 50f;
					}
				}
			}
		}
		num /= (float)num2;
		base.AddPoint((float)Mathf.RoundToInt(num));
	}

	// Token: 0x0600276C RID: 10092 RVA: 0x000E2B40 File Offset: 0x000E0D40
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
