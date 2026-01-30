using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000C8 RID: 200
public static class SquirrelTuning
{
	// Token: 0x040002B8 RID: 696
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002B9 RID: 697
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HUG = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040002BA RID: 698
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040002BB RID: 699
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040002BC RID: 700
	public static float STANDARD_STOMACH_SIZE = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE * SquirrelTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040002BD RID: 701
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040002BE RID: 702
	public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x040002BF RID: 703
	public static float EGG_MASS = 2f;
}
