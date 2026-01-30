using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A5 RID: 165
public static class ChameleonTuning
{
	// Token: 0x040001FE RID: 510
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "ChameleonEgg".ToTag(),
			weight = 0.98f
		}
	};

	// Token: 0x040001FF RID: 511
	public static float STANDARD_CALORIES_PER_CYCLE = 2000000f;

	// Token: 0x04000200 RID: 512
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000201 RID: 513
	public static float STANDARD_STOMACH_SIZE = ChameleonTuning.STANDARD_CALORIES_PER_CYCLE * ChameleonTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000202 RID: 514
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x04000203 RID: 515
	public static float EGG_MASS = 2f;

	// Token: 0x04000204 RID: 516
	public const float HARVEST_COOLDOWN = 150f;
}
