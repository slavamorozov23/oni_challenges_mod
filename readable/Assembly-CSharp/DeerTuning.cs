using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A9 RID: 169
public static class DeerTuning
{
	// Token: 0x0400020E RID: 526
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "WoodDeerEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GlassDeerEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400020F RID: 527
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_GLASS = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "WoodDeerEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GlassDeerEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000210 RID: 528
	public const float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000211 RID: 529
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000212 RID: 530
	public const float STANDARD_STOMACH_SIZE = 1000000f;

	// Token: 0x04000213 RID: 531
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000214 RID: 532
	public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x04000215 RID: 533
	public const float MORPH_DECOR_TRESHOLD = 100f;

	// Token: 0x04000216 RID: 534
	public static float EGG_MASS = 2f;

	// Token: 0x04000217 RID: 535
	public static float DROP_ANTLER_DURATION = 1200f;
}
