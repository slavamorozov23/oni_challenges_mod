using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000B1 RID: 177
public static class LightBugTuning
{
	// Token: 0x04000235 RID: 565
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000236 RID: 566
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ORANGE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000237 RID: 567
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PURPLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000238 RID: 568
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PINK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000239 RID: 569
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLUE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400023A RID: 570
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLACK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400023B RID: 571
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_CRYSTAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400023C RID: 572
	public static float STANDARD_CALORIES_PER_CYCLE = 40000f;

	// Token: 0x0400023D RID: 573
	public static float STANDARD_STARVE_CYCLES = 8f;

	// Token: 0x0400023E RID: 574
	public static float STANDARD_STOMACH_SIZE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE * LightBugTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400023F RID: 575
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3 / 2;

	// Token: 0x04000240 RID: 576
	public static float EGG_MASS = 1f;
}
