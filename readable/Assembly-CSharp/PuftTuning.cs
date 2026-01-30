using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000C1 RID: 193
public static class PuftTuning
{
	// Token: 0x0400029D RID: 669
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftOxyliteEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftBleachstoneEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400029E RID: 670
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ALPHA = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400029F RID: 671
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_OXYLITE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.31f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftOxyliteEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040002A0 RID: 672
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLEACHSTONE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.31f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftBleachstoneEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040002A1 RID: 673
	public static float STANDARD_CALORIES_PER_CYCLE = 200000f;

	// Token: 0x040002A2 RID: 674
	public static float STANDARD_STARVE_CYCLES = 6f;

	// Token: 0x040002A3 RID: 675
	public static float STANDARD_STOMACH_SIZE = PuftTuning.STANDARD_CALORIES_PER_CYCLE * PuftTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040002A4 RID: 676
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x040002A5 RID: 677
	public static float EGG_MASS = 0.5f;
}
