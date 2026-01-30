using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000BF RID: 191
public static class PrehistoricPacuTuning
{
	// Token: 0x04000290 RID: 656
	public const float LIFESPAWN = 100f;

	// Token: 0x04000291 RID: 657
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PrehistoricPacuEgg".ToTag(),
			weight = 1f
		}
	};

	// Token: 0x04000292 RID: 658
	public const int PACUS_EATEN_PER_CYCLE = 1;

	// Token: 0x04000293 RID: 659
	public const float KG_PACU_MEAT_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000294 RID: 660
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000295 RID: 661
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000296 RID: 662
	public static float STANDARD_STOMACH_SIZE = PrehistoricPacuTuning.STANDARD_CALORIES_PER_CYCLE * PrehistoricPacuTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000297 RID: 663
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000298 RID: 664
	public const float POOP_MASS_KG = 60f;

	// Token: 0x04000299 RID: 665
	public static Tag POOP_ELEMENT = SimHashes.Rust.CreateTag();

	// Token: 0x0400029A RID: 666
	public static float EGG_MASS = 4f;
}
