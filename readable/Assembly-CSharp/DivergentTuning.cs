using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000AB RID: 171
public static class DivergentTuning
{
	// Token: 0x04000218 RID: 536
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BEETLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000219 RID: 537
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WORM = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x0400021A RID: 538
	public static int TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION = 2;

	// Token: 0x0400021B RID: 539
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x0400021C RID: 540
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x0400021D RID: 541
	public static float STANDARD_STOMACH_SIZE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE * DivergentTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400021E RID: 542
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400021F RID: 543
	public static int PEN_SIZE_PER_CREATURE_WORM = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x04000220 RID: 544
	public static float EGG_MASS = 2f;
}
