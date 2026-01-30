using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A1 RID: 161
public static class BellyTuning
{
	// Token: 0x040001E3 RID: 483
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "IceBellyEgg".ToTag(),
			weight = 1f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GoldBellyEgg".ToTag(),
			weight = 0f
		}
	};

	// Token: 0x040001E4 RID: 484
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_GOLD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "IceBellyEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GoldBellyEgg".ToTag(),
			weight = 0.98f
		}
	};

	// Token: 0x040001E5 RID: 485
	public const float KW_GENERATED_TO_WARM_UP = 1.3f;

	// Token: 0x040001E6 RID: 486
	public static float STANDARD_CALORIES_PER_CYCLE = 4f * FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit / (CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == CarrotConfig.ID).cropDuration / 600f);

	// Token: 0x040001E7 RID: 487
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001E8 RID: 488
	public static float STANDARD_STOMACH_SIZE = BellyTuning.STANDARD_CALORIES_PER_CYCLE * 10f;

	// Token: 0x040001E9 RID: 489
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001EA RID: 490
	public const float EGG_MASS = 8f;

	// Token: 0x040001EB RID: 491
	public const int GERMS_EMMITED_PER_KG_POOPED = 1000;

	// Token: 0x040001EC RID: 492
	public static string GERM_ID_EMMITED_ON_POOP = "PollenGerms";

	// Token: 0x040001ED RID: 493
	public static float CALORIES_PER_UNIT_EATEN = FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit;

	// Token: 0x040001EE RID: 494
	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == CarrotConfig.ID).cropDuration / 600f;

	// Token: 0x040001EF RID: 495
	public const float CONSUMED_MASS_TO_POOP_MASS_MULTIPLIER = 67.474f;

	// Token: 0x040001F0 RID: 496
	public const float MIN_POOP_SIZE_IN_KG = 1f;
}
