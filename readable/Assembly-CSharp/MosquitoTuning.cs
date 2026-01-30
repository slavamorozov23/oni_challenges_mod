using System;
using System.Collections.Generic;

// Token: 0x020000B8 RID: 184
public static class MosquitoTuning
{
	// Token: 0x0400026B RID: 619
	public const float BASE_EGG_DROP_TIME = 0.9f;

	// Token: 0x0400026C RID: 620
	public const float EGG_MASS = 1f;

	// Token: 0x0400026D RID: 621
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MosquitoEgg".ToTag(),
			weight = 1f
		}
	};
}
