using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000C6 RID: 198
public static class SealTuning
{
	// Token: 0x040002B2 RID: 690
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SealEgg".ToTag(),
			weight = 1f
		}
	};

	// Token: 0x040002B3 RID: 691
	public const float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040002B4 RID: 692
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040002B5 RID: 693
	public const float STANDARD_STOMACH_SIZE = 1000000f;

	// Token: 0x040002B6 RID: 694
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040002B7 RID: 695
	public static float EGG_MASS = 2f;
}
