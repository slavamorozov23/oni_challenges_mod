using System;
using UnityEngine;

// Token: 0x02000740 RID: 1856
public class DevGenerator : Generator
{
	// Token: 0x06002EC5 RID: 11973 RVA: 0x0010E5B4 File Offset: 0x0010C7B4
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = this.wattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x04001BB2 RID: 7090
	public float wattageRating = 100000f;
}
