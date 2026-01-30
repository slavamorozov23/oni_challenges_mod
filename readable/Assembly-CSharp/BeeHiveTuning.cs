using System;

// Token: 0x0200009F RID: 159
public static class BeeHiveTuning
{
	// Token: 0x040001D3 RID: 467
	public static float ORE_DELIVERY_AMOUNT = 1f;

	// Token: 0x040001D4 RID: 468
	public static float KG_ORE_EATEN_PER_CYCLE = BeeHiveTuning.ORE_DELIVERY_AMOUNT * 10f;

	// Token: 0x040001D5 RID: 469
	public static float STANDARD_CALORIES_PER_CYCLE = 1500000f;

	// Token: 0x040001D6 RID: 470
	public static float STANDARD_STARVE_CYCLES = 30f;

	// Token: 0x040001D7 RID: 471
	public static float STANDARD_STOMACH_SIZE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE * BeeHiveTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001D8 RID: 472
	public static float CALORIES_PER_KG_OF_ORE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE / BeeHiveTuning.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040001D9 RID: 473
	public static float POOP_CONVERSTION_RATE = 0.9f;

	// Token: 0x040001DA RID: 474
	public static Tag CONSUMED_ORE = SimHashes.UraniumOre.CreateTag();

	// Token: 0x040001DB RID: 475
	public static Tag PRODUCED_ORE = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x040001DC RID: 476
	public static float HIVE_GROWTH_TIME = 2f;

	// Token: 0x040001DD RID: 477
	public static float WASTE_DROPPED_ON_DEATH = 5f;

	// Token: 0x040001DE RID: 478
	public static int GERMS_DROPPED_ON_DEATH = 10000;
}
