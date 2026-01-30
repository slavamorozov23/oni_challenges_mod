using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000CC RID: 204
public static class StegoTuning
{
	// Token: 0x040002C8 RID: 712
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StegoEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "AlgaeStegoEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002C9 RID: 713
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ALGAE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StegoEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "AlgaeStegoEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040002CA RID: 714
	public static float VINE_FOOD_PER_CYCLE = 4f;

	// Token: 0x040002CB RID: 715
	public static readonly float PEAT_PRODUCED_PER_CYCLE = 200f;

	// Token: 0x040002CC RID: 716
	public static readonly float STANDARD_CALORIES_PER_CYCLE = StegoTuning.VINE_FOOD_PER_CYCLE * 325000f;

	// Token: 0x040002CD RID: 717
	public static readonly float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040002CE RID: 718
	public static readonly float STANDARD_STOMACH_SIZE = StegoTuning.STANDARD_CALORIES_PER_CYCLE * StegoTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040002CF RID: 719
	public static readonly float CALORIES_PER_KG_OF_ORE = StegoTuning.STANDARD_CALORIES_PER_CYCLE / StegoTuning.VINE_FOOD_PER_CYCLE;

	// Token: 0x040002D0 RID: 720
	public static float CALORIES_PER_UNIT_EATEN = FOOD.FOOD_TYPES.VINEFRUIT.CaloriesPerUnit;

	// Token: 0x040002D1 RID: 721
	public static float MIN_POOP_SIZE_IN_KG = StegoTuning.VINE_FOOD_PER_CYCLE;

	// Token: 0x040002D2 RID: 722
	public static Tag POOP_ELEMENT = SimHashes.Peat.CreateTag();

	// Token: 0x040002D3 RID: 723
	public const float EGG_MASS = 8f;

	// Token: 0x040002D4 RID: 724
	public const float STOMP_COOLDOWN = 60f;

	// Token: 0x040002D5 RID: 725
	public static readonly int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x040002D6 RID: 726
	public const int SEARCH_RADIUS = 10;

	// Token: 0x040002D7 RID: 727
	public static int ROARS_PER_CYCLE = 2;

	// Token: 0x040002D8 RID: 728
	public static float ROAR_COOLDOWN = 60f;
}
