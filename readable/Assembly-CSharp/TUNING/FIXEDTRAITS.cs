using System;

namespace TUNING
{
	// Token: 0x02000FCD RID: 4045
	public class FIXEDTRAITS
	{
		// Token: 0x020021ED RID: 8685
		public class NORTHERNLIGHTS
		{
			// Token: 0x04009C13 RID: 39955
			public static int NONE = 0;

			// Token: 0x04009C14 RID: 39956
			public static int ENABLED = 1;

			// Token: 0x04009C15 RID: 39957
			public static int DEFAULT_VALUE = FIXEDTRAITS.NORTHERNLIGHTS.NONE;

			// Token: 0x02002A92 RID: 10898
			public class NAME
			{
				// Token: 0x0400BBCA RID: 48074
				public static string NONE = "northernLightsNone";

				// Token: 0x0400BBCB RID: 48075
				public static string ENABLED = "northernLightsOn";

				// Token: 0x0400BBCC RID: 48076
				public static string DEFAULT = FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE;
			}
		}

		// Token: 0x020021EE RID: 8686
		public class LARGEIMPACTORFRAGMENTS
		{
			// Token: 0x04009C16 RID: 39958
			public static int NONE = 0;

			// Token: 0x04009C17 RID: 39959
			public static int ALLOWED = 1;

			// Token: 0x04009C18 RID: 39960
			public static int DEFAULT_VALUE = FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NONE;

			// Token: 0x02002A93 RID: 10899
			public class NAME
			{
				// Token: 0x0400BBCD RID: 48077
				public static string NONE = "largeImpactorFragmentsNone";

				// Token: 0x0400BBCE RID: 48078
				public static string ALLOWED = "largeImpactorFragmentsAllowed";

				// Token: 0x0400BBCF RID: 48079
				public static string DEFAULT = FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NAME.NONE;
			}
		}

		// Token: 0x020021EF RID: 8687
		public class SUNLIGHT
		{
			// Token: 0x04009C19 RID: 39961
			public static int DEFAULT_SPACED_OUT_SUNLIGHT = 40000;

			// Token: 0x04009C1A RID: 39962
			public static int NONE = 0;

			// Token: 0x04009C1B RID: 39963
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.25f);

			// Token: 0x04009C1C RID: 39964
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.5f);

			// Token: 0x04009C1D RID: 39965
			public static int LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.75f);

			// Token: 0x04009C1E RID: 39966
			public static int MED_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.875f);

			// Token: 0x04009C1F RID: 39967
			public static int MED = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT;

			// Token: 0x04009C20 RID: 39968
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.25f);

			// Token: 0x04009C21 RID: 39969
			public static int HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.5f);

			// Token: 0x04009C22 RID: 39970
			public static int VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2;

			// Token: 0x04009C23 RID: 39971
			public static int VERY_VERY_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2.5f);

			// Token: 0x04009C24 RID: 39972
			public static int VERY_VERY_VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 3;

			// Token: 0x04009C25 RID: 39973
			public static int DEFAULT_VALUE = FIXEDTRAITS.SUNLIGHT.VERY_HIGH;

			// Token: 0x02002A94 RID: 10900
			public class NAME
			{
				// Token: 0x0400BBD0 RID: 48080
				public static string NONE = "sunlightNone";

				// Token: 0x0400BBD1 RID: 48081
				public static string VERY_VERY_LOW = "sunlightVeryVeryLow";

				// Token: 0x0400BBD2 RID: 48082
				public static string VERY_LOW = "sunlightVeryLow";

				// Token: 0x0400BBD3 RID: 48083
				public static string LOW = "sunlightLow";

				// Token: 0x0400BBD4 RID: 48084
				public static string MED_LOW = "sunlightMedLow";

				// Token: 0x0400BBD5 RID: 48085
				public static string MED = "sunlightMed";

				// Token: 0x0400BBD6 RID: 48086
				public static string MED_HIGH = "sunlightMedHigh";

				// Token: 0x0400BBD7 RID: 48087
				public static string HIGH = "sunlightHigh";

				// Token: 0x0400BBD8 RID: 48088
				public static string VERY_HIGH = "sunlightVeryHigh";

				// Token: 0x0400BBD9 RID: 48089
				public static string VERY_VERY_HIGH = "sunlightVeryVeryHigh";

				// Token: 0x0400BBDA RID: 48090
				public static string VERY_VERY_VERY_HIGH = "sunlightVeryVeryVeryHigh";

				// Token: 0x0400BBDB RID: 48091
				public static string DEFAULT = FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH;
			}
		}

		// Token: 0x020021F0 RID: 8688
		public class COSMICRADIATION
		{
			// Token: 0x04009C26 RID: 39974
			public static int BASELINE = 250;

			// Token: 0x04009C27 RID: 39975
			public static int NONE = 0;

			// Token: 0x04009C28 RID: 39976
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.25f);

			// Token: 0x04009C29 RID: 39977
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.5f);

			// Token: 0x04009C2A RID: 39978
			public static int LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.75f);

			// Token: 0x04009C2B RID: 39979
			public static int MED_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.875f);

			// Token: 0x04009C2C RID: 39980
			public static int MED = FIXEDTRAITS.COSMICRADIATION.BASELINE;

			// Token: 0x04009C2D RID: 39981
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.25f);

			// Token: 0x04009C2E RID: 39982
			public static int HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.5f);

			// Token: 0x04009C2F RID: 39983
			public static int VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 2;

			// Token: 0x04009C30 RID: 39984
			public static int VERY_VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 3;

			// Token: 0x04009C31 RID: 39985
			public static int DEFAULT_VALUE = FIXEDTRAITS.COSMICRADIATION.MED;

			// Token: 0x04009C32 RID: 39986
			public static float TELESCOPE_RADIATION_SHIELDING = 0.5f;

			// Token: 0x02002A95 RID: 10901
			public class NAME
			{
				// Token: 0x0400BBDC RID: 48092
				public static string NONE = "cosmicRadiationNone";

				// Token: 0x0400BBDD RID: 48093
				public static string VERY_VERY_LOW = "cosmicRadiationVeryVeryLow";

				// Token: 0x0400BBDE RID: 48094
				public static string VERY_LOW = "cosmicRadiationVeryLow";

				// Token: 0x0400BBDF RID: 48095
				public static string LOW = "cosmicRadiationLow";

				// Token: 0x0400BBE0 RID: 48096
				public static string MED_LOW = "cosmicRadiationMedLow";

				// Token: 0x0400BBE1 RID: 48097
				public static string MED = "cosmicRadiationMed";

				// Token: 0x0400BBE2 RID: 48098
				public static string MED_HIGH = "cosmicRadiationMedHigh";

				// Token: 0x0400BBE3 RID: 48099
				public static string HIGH = "cosmicRadiationHigh";

				// Token: 0x0400BBE4 RID: 48100
				public static string VERY_HIGH = "cosmicRadiationVeryHigh";

				// Token: 0x0400BBE5 RID: 48101
				public static string VERY_VERY_HIGH = "cosmicRadiationVeryVeryHigh";

				// Token: 0x0400BBE6 RID: 48102
				public static string DEFAULT = FIXEDTRAITS.COSMICRADIATION.NAME.MED;
			}
		}
	}
}
