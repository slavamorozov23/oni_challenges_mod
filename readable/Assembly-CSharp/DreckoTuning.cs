using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000AD RID: 173
public static class DreckoTuning
{
	// Token: 0x04000225 RID: 549
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000226 RID: 550
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PLASTIC = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000227 RID: 551
	public static float STANDARD_CALORIES_PER_CYCLE = 2000000f;

	// Token: 0x04000228 RID: 552
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000229 RID: 553
	public static float STANDARD_STOMACH_SIZE = DreckoTuning.STANDARD_CALORIES_PER_CYCLE * DreckoTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400022A RID: 554
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400022B RID: 555
	public static float EGG_MASS = 2f;
}
