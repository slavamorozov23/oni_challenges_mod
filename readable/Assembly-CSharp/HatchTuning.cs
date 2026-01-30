using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000AF RID: 175
public static class HatchTuning
{
	// Token: 0x0400022C RID: 556
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400022D RID: 557
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HARD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400022E RID: 558
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_VEGGIE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x0400022F RID: 559
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_METAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.11f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.22f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x04000230 RID: 560
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x04000231 RID: 561
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000232 RID: 562
	public static float STANDARD_STOMACH_SIZE = HatchTuning.STANDARD_CALORIES_PER_CYCLE * HatchTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000233 RID: 563
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000234 RID: 564
	public static float EGG_MASS = 2f;
}
