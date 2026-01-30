using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000C3 RID: 195
public static class RaptorTuning
{
	// Token: 0x040002A6 RID: 678
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "RaptorEgg".ToTag(),
			weight = 1f
		}
	};

	// Token: 0x040002A7 RID: 679
	public static float STANDARD_CALORIES_PER_CYCLE = 0.5f * FOOD.FOOD_TYPES.MEAT.CaloriesPerUnit;

	// Token: 0x040002A8 RID: 680
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040002A9 RID: 681
	public static float STANDARD_STOMACH_SIZE = RaptorTuning.STANDARD_CALORIES_PER_CYCLE * 10f;

	// Token: 0x040002AA RID: 682
	public const float EGG_MASS = 8f;

	// Token: 0x040002AB RID: 683
	public static float CALORIES_PER_UNIT_EATEN = FOOD.FOOD_TYPES.MEAT.CaloriesPerUnit;

	// Token: 0x040002AC RID: 684
	public const float MIN_POOP_SIZE_IN_KG = 0.1f;

	// Token: 0x040002AD RID: 685
	public static float BASE_PRODUCTION_RATE = 128f;

	// Token: 0x040002AE RID: 686
	public static float PREY_PRODUCTION_RATE = 256f;

	// Token: 0x040002AF RID: 687
	public static Tag POOP_ELEMENT = SimHashes.BrineIce.CreateTag();

	// Token: 0x040002B0 RID: 688
	public static int ROARS_PER_CYCLE = 2;

	// Token: 0x040002B1 RID: 689
	public static float ROAR_COOLDOWN = 60f;
}
