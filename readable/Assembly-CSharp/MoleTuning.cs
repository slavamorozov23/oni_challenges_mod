using System;
using System.Collections.Generic;

// Token: 0x020000B4 RID: 180
public static class MoleTuning
{
	// Token: 0x04000245 RID: 581
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000246 RID: 582
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DELICACY = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000247 RID: 583
	public static float STANDARD_CALORIES_PER_CYCLE = 4800000f;

	// Token: 0x04000248 RID: 584
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000249 RID: 585
	public static float STANDARD_STOMACH_SIZE = MoleTuning.STANDARD_CALORIES_PER_CYCLE * MoleTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400024A RID: 586
	public static float DELICACY_STOMACH_SIZE = MoleTuning.STANDARD_STOMACH_SIZE / 2f;

	// Token: 0x0400024B RID: 587
	public static int PEN_SIZE_PER_CREATURE = 0;

	// Token: 0x0400024C RID: 588
	public static float EGG_MASS = 2f;

	// Token: 0x0400024D RID: 589
	public static int DEPTH_TO_HIDE = 2;

	// Token: 0x0400024E RID: 590
	public static HashedString[] GINGER_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger",
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};
}
