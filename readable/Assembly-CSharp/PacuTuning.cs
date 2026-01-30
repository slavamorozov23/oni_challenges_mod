using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000BD RID: 189
public static class PacuTuning
{
	// Token: 0x0400027C RID: 636
	public static float MASS = 200f;

	// Token: 0x0400027D RID: 637
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400027E RID: 638
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_TROPICAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400027F RID: 639
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_CLEANER = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000280 RID: 640
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000281 RID: 641
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000282 RID: 642
	public static float STANDARD_STOMACH_SIZE = PacuTuning.STANDARD_CALORIES_PER_CYCLE * PacuTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000283 RID: 643
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER2;

	// Token: 0x04000284 RID: 644
	public static float EGG_MASS = 4f;
}
