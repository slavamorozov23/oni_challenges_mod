using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000BA RID: 186
public static class OilFloaterTuning
{
	// Token: 0x0400026E RID: 622
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400026F RID: 623
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HIGHTEMP = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000270 RID: 624
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DECOR = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.66f
		}
	};

	// Token: 0x04000271 RID: 625
	public static float STANDARD_CALORIES_PER_CYCLE = 120000f;

	// Token: 0x04000272 RID: 626
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000273 RID: 627
	public static float STANDARD_STOMACH_SIZE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE * OilFloaterTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000274 RID: 628
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000275 RID: 629
	public static float EGG_MASS = 2f;
}
