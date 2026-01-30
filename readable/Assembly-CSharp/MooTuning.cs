using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public static class MooTuning
{
	// Token: 0x04000250 RID: 592
	public static List<BeckoningMonitor.SongChance> BaseSongChances = new List<BeckoningMonitor.SongChance>
	{
		new BeckoningMonitor.SongChance
		{
			meteorID = GassyMooCometConfig.ID,
			singAnimPre = "beckoning_pre",
			singAnimLoop = "beckoning_loop",
			singAnimPst = "beckoning_pst",
			weight = 0.98f
		},
		new BeckoningMonitor.SongChance
		{
			meteorID = DieselMooCometConfig.ID,
			singAnimPre = "diesel_beckoning_pre",
			singAnimLoop = "diesel_beckoning_loop",
			singAnimPst = "diesel_beckoning_pst",
			weight = 0.02f
		}
	};

	// Token: 0x04000251 RID: 593
	public static List<BeckoningMonitor.SongChance> DieselSongChances = new List<BeckoningMonitor.SongChance>
	{
		new BeckoningMonitor.SongChance
		{
			meteorID = GassyMooCometConfig.ID,
			singAnimPre = "beckoning_pre",
			singAnimLoop = "beckoning_loop",
			singAnimPst = "beckoning_pst",
			weight = 0.3f
		},
		new BeckoningMonitor.SongChance
		{
			meteorID = DieselMooCometConfig.ID,
			singAnimPre = "diesel_beckoning_pre",
			singAnimLoop = "diesel_beckoning_loop",
			singAnimPst = "diesel_beckoning_pst",
			weight = 0.6f
		}
	};

	// Token: 0x04000252 RID: 594
	public static readonly float STANDARD_LIFESPAN = 75f;

	// Token: 0x04000253 RID: 595
	public static readonly float STANDARD_CALORIES_PER_CYCLE = 200000f;

	// Token: 0x04000254 RID: 596
	public static readonly float STANDARD_STARVE_CYCLES = 6f;

	// Token: 0x04000255 RID: 597
	public static readonly float STANDARD_STOMACH_SIZE = MooTuning.STANDARD_CALORIES_PER_CYCLE * MooTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000256 RID: 598
	public static readonly int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x04000257 RID: 599
	public static readonly float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 2f;

	// Token: 0x04000258 RID: 600
	public static float KG_SOLIDS_EATEN_PER_DAY = 200f;

	// Token: 0x04000259 RID: 601
	public static float CALORIES_PER_DAY_OF_SOLID_EATEN = MooTuning.STANDARD_CALORIES_PER_CYCLE / MooTuning.KG_SOLIDS_EATEN_PER_DAY;

	// Token: 0x0400025A RID: 602
	public static float CALORIES_PER_DAY_OF_PLANT_EATEN = MooTuning.STANDARD_CALORIES_PER_CYCLE / MooTuning.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x0400025B RID: 603
	public static float KG_POOP_PER_DAY_OF_PLANT = 5f;

	// Token: 0x0400025C RID: 604
	public static float POOP_KG_COVERSION_RATE_FOR_SOLID_DIET = 10f / MooTuning.KG_SOLIDS_EATEN_PER_DAY;

	// Token: 0x0400025D RID: 605
	public static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x0400025E RID: 606
	public static float MIN_POOP_SIZE_IN_CALORIES = MooTuning.CALORIES_PER_DAY_OF_PLANT_EATEN * MooTuning.MIN_POOP_SIZE_IN_KG / MooTuning.KG_POOP_PER_DAY_OF_PLANT;

	// Token: 0x0400025F RID: 607
	private static readonly float BECKONS_PER_LIFESPAN = 4f;

	// Token: 0x04000260 RID: 608
	private static readonly float BECKON_FUDGE_CYCLES = 11f;

	// Token: 0x04000261 RID: 609
	private static readonly float BECKON_CYCLES = Mathf.Floor((MooTuning.STANDARD_LIFESPAN - MooTuning.BECKON_FUDGE_CYCLES) / MooTuning.BECKONS_PER_LIFESPAN);

	// Token: 0x04000262 RID: 610
	public static readonly float WELLFED_EFFECT = 100f / (600f * MooTuning.BECKON_CYCLES);

	// Token: 0x04000263 RID: 611
	public static readonly float WELLFED_CALORIES_PER_CYCLE = MooTuning.STANDARD_CALORIES_PER_CYCLE * 0.9f;

	// Token: 0x04000264 RID: 612
	public static readonly float ELIGIBLE_MILKING_PERCENTAGE = 1f;

	// Token: 0x04000265 RID: 613
	public static readonly float MILK_PER_CYCLE = 50f;

	// Token: 0x04000266 RID: 614
	public static readonly float DIESEL_PER_CYCLE = 200f;

	// Token: 0x04000267 RID: 615
	private static readonly float CYCLES_UNTIL_MILKING = 4f;

	// Token: 0x04000268 RID: 616
	public static readonly float MILK_CAPACITY = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x04000269 RID: 617
	public static readonly float MILK_AMOUNT_AT_MILKING = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x0400026A RID: 618
	public static readonly float MILK_PRODUCTION_PERCENTAGE_PER_SECOND = 100f / (600f * MooTuning.CYCLES_UNTIL_MILKING);
}
