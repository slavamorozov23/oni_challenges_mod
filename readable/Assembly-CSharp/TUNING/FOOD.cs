using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02000FDA RID: 4058
	public class FOOD
	{
		// Token: 0x04005EAC RID: 24236
		public const float EATING_SECONDS_PER_CALORIE = 2E-05f;

		// Token: 0x04005EAD RID: 24237
		public static float FOOD_CALORIES_PER_CYCLE = -DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE;

		// Token: 0x04005EAE RID: 24238
		public const int FOOD_AMOUNT_INGREDIENT_ONLY = 0;

		// Token: 0x04005EAF RID: 24239
		public const float KCAL_SMALL_PORTION = 600000f;

		// Token: 0x04005EB0 RID: 24240
		public const float KCAL_BONUS_COOKING_LOW = 250000f;

		// Token: 0x04005EB1 RID: 24241
		public const float KCAL_BASIC_PORTION = 800000f;

		// Token: 0x04005EB2 RID: 24242
		public const float KCAL_PREPARED_FOOD = 4000000f;

		// Token: 0x04005EB3 RID: 24243
		public const float KCAL_BONUS_COOKING_BASIC = 400000f;

		// Token: 0x04005EB4 RID: 24244
		public const float KCAL_BONUS_COOKING_DEEPFRIED = 1200000f;

		// Token: 0x04005EB5 RID: 24245
		public const float DEFAULT_PRESERVE_TEMPERATURE = 255.15f;

		// Token: 0x04005EB6 RID: 24246
		public const float DEFAULT_ROT_TEMPERATURE = 277.15f;

		// Token: 0x04005EB7 RID: 24247
		public const float HIGH_PRESERVE_TEMPERATURE = 283.15f;

		// Token: 0x04005EB8 RID: 24248
		public const float HIGH_ROT_TEMPERATURE = 308.15f;

		// Token: 0x04005EB9 RID: 24249
		public const float EGG_COOK_TEMPERATURE = 344.15f;

		// Token: 0x04005EBA RID: 24250
		public const float DEFAULT_MASS = 1f;

		// Token: 0x04005EBB RID: 24251
		public const float DEFAULT_SPICE_MASS = 1f;

		// Token: 0x04005EBC RID: 24252
		public const float ROT_TO_ELEMENT_TIME = 600f;

		// Token: 0x04005EBD RID: 24253
		public const int MUSH_BAR_SPAWN_GERMS = 1000;

		// Token: 0x04005EBE RID: 24254
		public const float IDEAL_TEMPERATURE_TOLERANCE = 10f;

		// Token: 0x04005EBF RID: 24255
		public const int FOOD_QUALITY_AWFUL = -1;

		// Token: 0x04005EC0 RID: 24256
		public const int FOOD_QUALITY_TERRIBLE = 0;

		// Token: 0x04005EC1 RID: 24257
		public const int FOOD_QUALITY_MEDIOCRE = 1;

		// Token: 0x04005EC2 RID: 24258
		public const int FOOD_QUALITY_GOOD = 2;

		// Token: 0x04005EC3 RID: 24259
		public const int FOOD_QUALITY_GREAT = 3;

		// Token: 0x04005EC4 RID: 24260
		public const int FOOD_QUALITY_AMAZING = 4;

		// Token: 0x04005EC5 RID: 24261
		public const int FOOD_QUALITY_WONDERFUL = 5;

		// Token: 0x04005EC6 RID: 24262
		public const int FOOD_QUALITY_MORE_WONDERFUL = 6;

		// Token: 0x0200221D RID: 8733
		public class SPOIL_TIME
		{
			// Token: 0x04009D31 RID: 40241
			public const float DEFAULT = 4800f;

			// Token: 0x04009D32 RID: 40242
			public const float QUICK = 2400f;

			// Token: 0x04009D33 RID: 40243
			public const float SLOW = 9600f;

			// Token: 0x04009D34 RID: 40244
			public const float VERYSLOW = 19200f;
		}

		// Token: 0x0200221E RID: 8734
		public class FOOD_TYPES
		{
			// Token: 0x04009D35 RID: 40245
			public static readonly EdiblesManager.FoodInfo FIELDRATION = new EdiblesManager.FoodInfo("FieldRation", 800000f, -1, 255.15f, 277.15f, 19200f, false, null, null);

			// Token: 0x04009D36 RID: 40246
			public static readonly EdiblesManager.FoodInfo MUSHBAR = new EdiblesManager.FoodInfo("MushBar", 800000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D37 RID: 40247
			public static readonly EdiblesManager.FoodInfo BASICPLANTFOOD = new EdiblesManager.FoodInfo("BasicPlantFood", 600000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D38 RID: 40248
			public static readonly EdiblesManager.FoodInfo VINEFRUIT = new EdiblesManager.FoodInfo(VineFruitConfig.ID, 325000f, 0, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4, null);

			// Token: 0x04009D39 RID: 40249
			public static readonly EdiblesManager.FoodInfo BASICFORAGEPLANT = new EdiblesManager.FoodInfo("BasicForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false, null, null);

			// Token: 0x04009D3A RID: 40250
			public static readonly EdiblesManager.FoodInfo FORESTFORAGEPLANT = new EdiblesManager.FoodInfo("ForestForagePlant", 6400000f, -1, 255.15f, 277.15f, 4800f, false, null, null);

			// Token: 0x04009D3B RID: 40251
			public static readonly EdiblesManager.FoodInfo SWAMPFORAGEPLANT = new EdiblesManager.FoodInfo("SwampForagePlant", 2400000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.EXPANSION1, null);

			// Token: 0x04009D3C RID: 40252
			public static readonly EdiblesManager.FoodInfo ICECAVESFORAGEPLANT = new EdiblesManager.FoodInfo("IceCavesForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.DLC2, null);

			// Token: 0x04009D3D RID: 40253
			public static readonly EdiblesManager.FoodInfo MUSHROOM = new EdiblesManager.FoodInfo(MushroomConfig.ID, 2400000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D3E RID: 40254
			public static readonly EdiblesManager.FoodInfo LETTUCE = new EdiblesManager.FoodInfo("Lettuce", 400000f, 0, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D3F RID: 40255
			public static readonly EdiblesManager.FoodInfo RAWEGG = new EdiblesManager.FoodInfo("RawEgg", 1600000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D40 RID: 40256
			public static readonly EdiblesManager.FoodInfo MEAT = new EdiblesManager.FoodInfo("Meat", 1600000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D41 RID: 40257
			public static readonly EdiblesManager.FoodInfo PLANTMEAT = new EdiblesManager.FoodInfo("PlantMeat", 1200000f, 1, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D42 RID: 40258
			public static readonly EdiblesManager.FoodInfo PRICKLEFRUIT = new EdiblesManager.FoodInfo(PrickleFruitConfig.ID, 1600000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D43 RID: 40259
			public static readonly EdiblesManager.FoodInfo SWAMPFRUIT = new EdiblesManager.FoodInfo(SwampFruitConfig.ID, 1840000f, 0, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D44 RID: 40260
			public static readonly EdiblesManager.FoodInfo FISH_MEAT = new EdiblesManager.FoodInfo("FishMeat", 1000000f, 2, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D45 RID: 40261
			public static readonly EdiblesManager.FoodInfo SHELLFISH_MEAT = new EdiblesManager.FoodInfo("ShellfishMeat", 1000000f, 2, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D46 RID: 40262
			public static readonly EdiblesManager.FoodInfo JAWBOFILLET = new EdiblesManager.FoodInfo("PrehistoricPacuFillet", 1000000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC4, null);

			// Token: 0x04009D47 RID: 40263
			public static readonly EdiblesManager.FoodInfo WORMBASICFRUIT = new EdiblesManager.FoodInfo("WormBasicFruit", 800000f, 0, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D48 RID: 40264
			public static readonly EdiblesManager.FoodInfo WORMSUPERFRUIT = new EdiblesManager.FoodInfo("WormSuperFruit", 250000f, 1, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D49 RID: 40265
			public static readonly EdiblesManager.FoodInfo HARDSKINBERRY = new EdiblesManager.FoodInfo("HardSkinBerry", 800000f, -1, 255.15f, 277.15f, 9600f, true, DlcManager.DLC2, null);

			// Token: 0x04009D4A RID: 40266
			public static readonly EdiblesManager.FoodInfo CARROT = new EdiblesManager.FoodInfo(CarrotConfig.ID, 4000000f, 0, 255.15f, 277.15f, 9600f, true, DlcManager.DLC2, null);

			// Token: 0x04009D4B RID: 40267
			public static readonly EdiblesManager.FoodInfo PEMMICAN = new EdiblesManager.FoodInfo("Pemmican", FOOD.FOOD_TYPES.HARDSKINBERRY.CaloriesPerUnit * 2f + 1000000f, 2, 255.15f, 277.15f, 19200f, false, DlcManager.DLC2, null);

			// Token: 0x04009D4C RID: 40268
			public static readonly EdiblesManager.FoodInfo FRIES_CARROT = new EdiblesManager.FoodInfo("FriesCarrot", 5400000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null);

			// Token: 0x04009D4D RID: 40269
			public static readonly EdiblesManager.FoodInfo BUTTERFLYFOOD = new EdiblesManager.FoodInfo("ButterflyFood", 1500000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4, null);

			// Token: 0x04009D4E RID: 40270
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_MEAT = new EdiblesManager.FoodInfo("DeepFriedMeat", 4000000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null);

			// Token: 0x04009D4F RID: 40271
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_NOSH = new EdiblesManager.FoodInfo("DeepFriedNosh", 5000000f, 3, 255.15f, 277.15f, 4800f, true, DlcManager.DLC2, null);

			// Token: 0x04009D50 RID: 40272
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_FISH = new EdiblesManager.FoodInfo("DeepFriedFish", 4200000f, 4, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D51 RID: 40273
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_SHELLFISH = new EdiblesManager.FoodInfo("DeepFriedShellfish", 4200000f, 4, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D52 RID: 40274
			public static readonly EdiblesManager.FoodInfo GARDENFOODPLANT = new EdiblesManager.FoodInfo("GardenFoodPlantFood", 800000f, -1, 255.15f, 277.15f, 9600f, true, DlcManager.DLC4, null);

			// Token: 0x04009D53 RID: 40275
			public static readonly EdiblesManager.FoodInfo GARDENFORAGEPLANT = new EdiblesManager.FoodInfo("GardenForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.DLC4, null);

			// Token: 0x04009D54 RID: 40276
			public static readonly EdiblesManager.FoodInfo PICKLEDMEAL = new EdiblesManager.FoodInfo("PickledMeal", 1800000f, -1, 255.15f, 277.15f, 19200f, true, null, null);

			// Token: 0x04009D55 RID: 40277
			public static readonly EdiblesManager.FoodInfo BASICPLANTBAR = new EdiblesManager.FoodInfo("BasicPlantBar", 1700000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D56 RID: 40278
			public static readonly EdiblesManager.FoodInfo FRIEDMUSHBAR = new EdiblesManager.FoodInfo("FriedMushBar", 1050000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D57 RID: 40279
			public static readonly EdiblesManager.FoodInfo GAMMAMUSH = new EdiblesManager.FoodInfo("GammaMush", 1050000f, 1, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009D58 RID: 40280
			public static readonly EdiblesManager.FoodInfo GRILLED_PRICKLEFRUIT = new EdiblesManager.FoodInfo("GrilledPrickleFruit", 2000000f, 1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D59 RID: 40281
			public static readonly EdiblesManager.FoodInfo SWAMP_DELIGHTS = new EdiblesManager.FoodInfo("SwampDelights", 2240000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D5A RID: 40282
			public static readonly EdiblesManager.FoodInfo FRIED_MUSHROOM = new EdiblesManager.FoodInfo("FriedMushroom", 2800000f, 1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D5B RID: 40283
			public static readonly EdiblesManager.FoodInfo COOKED_PIKEAPPLE = new EdiblesManager.FoodInfo("CookedPikeapple", 1200000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.DLC2, null);

			// Token: 0x04009D5C RID: 40284
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_BREAD = new EdiblesManager.FoodInfo("ColdWheatBread", 1200000f, 2, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D5D RID: 40285
			public static readonly EdiblesManager.FoodInfo COOKED_EGG = new EdiblesManager.FoodInfo("CookedEgg", 2800000f, 2, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009D5E RID: 40286
			public static readonly EdiblesManager.FoodInfo COOKED_FISH = new EdiblesManager.FoodInfo("CookedFish", 1600000f, 3, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D5F RID: 40287
			public static readonly EdiblesManager.FoodInfo SMOKED_VEGETABLES = new EdiblesManager.FoodInfo("SmokedVegetables", 2862500f, 2, 255.15f, 277.15f, 9600f, true, DlcManager.DLC4, null);

			// Token: 0x04009D60 RID: 40288
			public static readonly EdiblesManager.FoodInfo PANCAKES = new EdiblesManager.FoodInfo("Pancakes", 3600000f, 3, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D61 RID: 40289
			public static readonly EdiblesManager.FoodInfo SMOKED_FISH = new EdiblesManager.FoodInfo("SmokedFish", 2800000f, 3, 255.15f, 277.15f, 19200f, true, DlcManager.DLC4, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D62 RID: 40290
			public static readonly EdiblesManager.FoodInfo COOKED_MEAT = new EdiblesManager.FoodInfo("CookedMeat", 4000000f, 3, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009D63 RID: 40291
			public static readonly EdiblesManager.FoodInfo SMOKED_DINOSAURMEAT = new EdiblesManager.FoodInfo("SmokedDinosaurMeat", 5000000f, 3, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4, null);

			// Token: 0x04009D64 RID: 40292
			public static readonly EdiblesManager.FoodInfo WORMBASICFOOD = new EdiblesManager.FoodInfo("WormBasicFood", 1200000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D65 RID: 40293
			public static readonly EdiblesManager.FoodInfo WORMSUPERFOOD = new EdiblesManager.FoodInfo("WormSuperFood", 2400000f, 3, 255.15f, 277.15f, 19200f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D66 RID: 40294
			public static readonly EdiblesManager.FoodInfo FRUITCAKE = new EdiblesManager.FoodInfo("FruitCake", 4000000f, 3, 255.15f, 277.15f, 19200f, false, null, null);

			// Token: 0x04009D67 RID: 40295
			public static readonly EdiblesManager.FoodInfo SALSA = new EdiblesManager.FoodInfo("Salsa", 4400000f, 4, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009D68 RID: 40296
			public static readonly EdiblesManager.FoodInfo SURF_AND_TURF = new EdiblesManager.FoodInfo("SurfAndTurf", 6000000f, 4, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D69 RID: 40297
			public static readonly EdiblesManager.FoodInfo MUSHROOM_WRAP = new EdiblesManager.FoodInfo("MushroomWrap", 4800000f, 4, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D6A RID: 40298
			public static readonly EdiblesManager.FoodInfo TOFU = new EdiblesManager.FoodInfo("Tofu", 3600000f, 2, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009D6B RID: 40299
			public static readonly EdiblesManager.FoodInfo CURRY = new EdiblesManager.FoodInfo("Curry", 5000000f, 4, 255.15f, 277.15f, 9600f, true, null, null).AddEffects(new List<string>
			{
				"HotStuff",
				"WarmTouchFood"
			}, null, null);

			// Token: 0x04009D6C RID: 40300
			public static readonly EdiblesManager.FoodInfo SPICEBREAD = new EdiblesManager.FoodInfo("SpiceBread", 4000000f, 5, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D6D RID: 40301
			public static readonly EdiblesManager.FoodInfo SPICY_TOFU = new EdiblesManager.FoodInfo("SpicyTofu", 4000000f, 5, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"WarmTouchFood"
			}, null, null);

			// Token: 0x04009D6E RID: 40302
			public static readonly EdiblesManager.FoodInfo QUICHE = new EdiblesManager.FoodInfo("Quiche", 6400000f, 5, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D6F RID: 40303
			public static readonly EdiblesManager.FoodInfo BERRY_PIE = new EdiblesManager.FoodInfo("BerryPie", 4200000f, 5, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009D70 RID: 40304
			public static readonly EdiblesManager.FoodInfo BURGER = new EdiblesManager.FoodInfo("Burger", 6000000f, 6, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"GoodEats"
			}, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009D71 RID: 40305
			public static readonly EdiblesManager.FoodInfo BEAN = new EdiblesManager.FoodInfo("BeanPlantSeed", 0f, 3, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009D72 RID: 40306
			public static readonly EdiblesManager.FoodInfo SPICENUT = new EdiblesManager.FoodInfo(SpiceNutConfig.ID, 0f, 0, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009D73 RID: 40307
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_SEED = new EdiblesManager.FoodInfo("ColdWheatSeed", 0f, 0, 283.15f, 308.15f, 9600f, true, null, null);

			// Token: 0x04009D74 RID: 40308
			public static readonly EdiblesManager.FoodInfo FERNFOOD = new EdiblesManager.FoodInfo(FernFoodConfig.ID, 0f, 2, 255.15f, 277.15f, 9600f, true, DlcManager.DLC4, null);

			// Token: 0x04009D75 RID: 40309
			public static readonly EdiblesManager.FoodInfo BUTTERFLY_SEED = new EdiblesManager.FoodInfo("ButterflyPlantSeed", 0f, 2, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4, null);

			// Token: 0x04009D76 RID: 40310
			public static readonly EdiblesManager.FoodInfo DINOSAURMEAT = new EdiblesManager.FoodInfo("DinosaurMeat", 0f, -1, 255.15f, 277.15f, 2400f, true, DlcManager.DLC4, null);
		}

		// Token: 0x0200221F RID: 8735
		public class RECIPES
		{
			// Token: 0x04009D77 RID: 40311
			public static float SMALL_COOK_TIME = 30f;

			// Token: 0x04009D78 RID: 40312
			public static float STANDARD_COOK_TIME = 50f;
		}
	}
}
