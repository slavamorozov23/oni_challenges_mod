using System;

namespace STRINGS
{
	// Token: 0x02001003 RID: 4099
	public static class WORLD_TRAITS
	{
		// Token: 0x04006059 RID: 24665
		public static LocString MISSING_TRAIT = "<missing traits>";

		// Token: 0x020026FB RID: 9979
		public static class NO_TRAITS
		{
			// Token: 0x0400AD59 RID: 44377
			public static LocString NAME = "<i>This world is stable and has no unusual features.</i>";

			// Token: 0x0400AD5A RID: 44378
			public static LocString NAME_SHORTHAND = "No unusual features";

			// Token: 0x0400AD5B RID: 44379
			public static LocString DESCRIPTION = "This world exists in a particularly stable configuration each time it is encountered";
		}

		// Token: 0x020026FC RID: 9980
		public static class BOULDERS_LARGE
		{
			// Token: 0x0400AD5C RID: 44380
			public static LocString NAME = "Large Boulders";

			// Token: 0x0400AD5D RID: 44381
			public static LocString DESCRIPTION = "Huge boulders make digging through this world more difficult";
		}

		// Token: 0x020026FD RID: 9981
		public static class BOULDERS_MEDIUM
		{
			// Token: 0x0400AD5E RID: 44382
			public static LocString NAME = "Medium Boulders";

			// Token: 0x0400AD5F RID: 44383
			public static LocString DESCRIPTION = "Mid-sized boulders make digging through this world more difficult";
		}

		// Token: 0x020026FE RID: 9982
		public static class BOULDERS_MIXED
		{
			// Token: 0x0400AD60 RID: 44384
			public static LocString NAME = "Mixed Boulders";

			// Token: 0x0400AD61 RID: 44385
			public static LocString DESCRIPTION = "Boulders of various sizes make digging through this world more difficult";
		}

		// Token: 0x020026FF RID: 9983
		public static class BOULDERS_SMALL
		{
			// Token: 0x0400AD62 RID: 44386
			public static LocString NAME = "Small Boulders";

			// Token: 0x0400AD63 RID: 44387
			public static LocString DESCRIPTION = "Tiny boulders make digging through this world more difficult";
		}

		// Token: 0x02002700 RID: 9984
		public static class DEEP_OIL
		{
			// Token: 0x0400AD64 RID: 44388
			public static LocString NAME = "Trapped Oil";

			// Token: 0x0400AD65 RID: 44389
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Most of the ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" in this world will need to be extracted with ",
				BUILDINGS.PREFABS.OILWELLCAP.NAME,
				"s"
			});
		}

		// Token: 0x02002701 RID: 9985
		public static class FROZEN_CORE
		{
			// Token: 0x0400AD66 RID: 44390
			public static LocString NAME = "Frozen Core";

			// Token: 0x0400AD67 RID: 44391
			public static LocString DESCRIPTION = "This world has a chilly core of solid " + ELEMENTS.ICE.NAME;
		}

		// Token: 0x02002702 RID: 9986
		public static class GEOACTIVE
		{
			// Token: 0x0400AD68 RID: 44392
			public static LocString NAME = "Geoactive";

			// Token: 0x0400AD69 RID: 44393
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has more ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02002703 RID: 9987
		public static class GEODES
		{
			// Token: 0x0400AD6A RID: 44394
			public static LocString NAME = "Geodes";

			// Token: 0x0400AD6B RID: 44395
			public static LocString DESCRIPTION = "Large geodes containing rare material caches are deposited across this world";
		}

		// Token: 0x02002704 RID: 9988
		public static class GEODORMANT
		{
			// Token: 0x0400AD6C RID: 44396
			public static LocString NAME = "Geodormant";

			// Token: 0x0400AD6D RID: 44397
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has fewer ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02002705 RID: 9989
		public static class GLACIERS_LARGE
		{
			// Token: 0x0400AD6E RID: 44398
			public static LocString NAME = "Large Glaciers";

			// Token: 0x0400AD6F RID: 44399
			public static LocString DESCRIPTION = "Huge chunks of primordial " + ELEMENTS.ICE.NAME + " are scattered across this world";
		}

		// Token: 0x02002706 RID: 9990
		public static class IRREGULAR_OIL
		{
			// Token: 0x0400AD70 RID: 44400
			public static LocString NAME = "Irregular Oil";

			// Token: 0x0400AD71 RID: 44401
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" on this asteroid is anything but regular!"
			});
		}

		// Token: 0x02002707 RID: 9991
		public static class MAGMA_VENTS
		{
			// Token: 0x0400AD72 RID: 44402
			public static LocString NAME = "Magma Channels";

			// Token: 0x0400AD73 RID: 44403
			public static LocString DESCRIPTION = "The " + ELEMENTS.MAGMA.NAME + " from this world's core has leaked into the mantle and crust";
		}

		// Token: 0x02002708 RID: 9992
		public static class METAL_POOR
		{
			// Token: 0x0400AD74 RID: 44404
			public static LocString NAME = "Metal Poor";

			// Token: 0x0400AD75 RID: 44405
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"There is a reduced amount of ",
				UI.PRE_KEYWORD,
				"Metal Ore",
				UI.PST_KEYWORD,
				" on this world, proceed with caution!"
			});
		}

		// Token: 0x02002709 RID: 9993
		public static class METAL_RICH
		{
			// Token: 0x0400AD76 RID: 44406
			public static LocString NAME = "Metal Rich";

			// Token: 0x0400AD77 RID: 44407
			public static LocString DESCRIPTION = "This asteroid is an abundant source of " + UI.PRE_KEYWORD + "Metal Ore" + UI.PST_KEYWORD;
		}

		// Token: 0x0200270A RID: 9994
		public static class MISALIGNED_START
		{
			// Token: 0x0400AD78 RID: 44408
			public static LocString NAME = "Alternate Pod Location";

			// Token: 0x0400AD79 RID: 44409
			public static LocString DESCRIPTION = "The " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " didn't end up in the asteroid's exact center this time... but it's still nowhere near the surface";
		}

		// Token: 0x0200270B RID: 9995
		public static class SLIME_SPLATS
		{
			// Token: 0x0400AD7A RID: 44410
			public static LocString NAME = "Slime Molds";

			// Token: 0x0400AD7B RID: 44411
			public static LocString DESCRIPTION = "Sickly " + ELEMENTS.SLIMEMOLD.NAME + " growths have crept all over this world";
		}

		// Token: 0x0200270C RID: 9996
		public static class SUBSURFACE_OCEAN
		{
			// Token: 0x0400AD7C RID: 44412
			public static LocString NAME = "Subsurface Ocean";

			// Token: 0x0400AD7D RID: 44413
			public static LocString DESCRIPTION = "Below the crust of this world is a " + ELEMENTS.SALTWATER.NAME + " sea";
		}

		// Token: 0x0200270D RID: 9997
		public static class VOLCANOES
		{
			// Token: 0x0400AD7E RID: 44414
			public static LocString NAME = "Volcanic Activity";

			// Token: 0x0400AD7F RID: 44415
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Several active ",
				UI.PRE_KEYWORD,
				"Volcanoes",
				UI.PST_KEYWORD,
				" have been detected in this world"
			});
		}

		// Token: 0x0200270E RID: 9998
		public static class RADIOACTIVE_CRUST
		{
			// Token: 0x0400AD80 RID: 44416
			public static LocString NAME = "Radioactive Crust";

			// Token: 0x0400AD81 RID: 44417
			public static LocString DESCRIPTION = "Deposits of " + ELEMENTS.URANIUMORE.NAME + " are found in this world's crust";
		}

		// Token: 0x0200270F RID: 9999
		public static class LUSH_CORE
		{
			// Token: 0x0400AD82 RID: 44418
			public static LocString NAME = "Lush Core";

			// Token: 0x0400AD83 RID: 44419
			public static LocString DESCRIPTION = "This world has a lush forest core";
		}

		// Token: 0x02002710 RID: 10000
		public static class METAL_CAVES
		{
			// Token: 0x0400AD84 RID: 44420
			public static LocString NAME = "Metallic Caves";

			// Token: 0x0400AD85 RID: 44421
			public static LocString DESCRIPTION = "This world has caves of metal ore";
		}

		// Token: 0x02002711 RID: 10001
		public static class DISTRESS_SIGNAL
		{
			// Token: 0x0400AD86 RID: 44422
			public static LocString NAME = "Frozen Friend";

			// Token: 0x0400AD87 RID: 44423
			public static LocString DESCRIPTION = "This world contains a frozen friend from a long time ago";
		}

		// Token: 0x02002712 RID: 10002
		public static class CRASHED_SATELLITES
		{
			// Token: 0x0400AD88 RID: 44424
			public static LocString NAME = "Crashed Satellites";

			// Token: 0x0400AD89 RID: 44425
			public static LocString DESCRIPTION = "This world contains crashed radioactive satellites";
		}
	}
}
