using System;

namespace TUNING
{
	// Token: 0x02000FD3 RID: 4051
	public class NOISE_POLLUTION
	{
		// Token: 0x04005E44 RID: 24132
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x04005E45 RID: 24133
		public static readonly EffectorValues CONE_OF_SILENCE = new EffectorValues
		{
			amount = -120,
			radius = 5
		};

		// Token: 0x04005E46 RID: 24134
		public static float DUPLICANT_TIME_THRESHOLD = 3f;

		// Token: 0x020021FD RID: 8701
		public class LENGTHS
		{
			// Token: 0x04009C6A RID: 40042
			public static float VERYSHORT = 0.25f;

			// Token: 0x04009C6B RID: 40043
			public static float SHORT = 0.5f;

			// Token: 0x04009C6C RID: 40044
			public static float NORMAL = 1f;

			// Token: 0x04009C6D RID: 40045
			public static float LONG = 1.5f;

			// Token: 0x04009C6E RID: 40046
			public static float VERYLONG = 2f;
		}

		// Token: 0x020021FE RID: 8702
		public class NOISY
		{
			// Token: 0x04009C6F RID: 40047
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 45,
				radius = 10
			};

			// Token: 0x04009C70 RID: 40048
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 55,
				radius = 10
			};

			// Token: 0x04009C71 RID: 40049
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 65,
				radius = 10
			};

			// Token: 0x04009C72 RID: 40050
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 75,
				radius = 15
			};

			// Token: 0x04009C73 RID: 40051
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 90,
				radius = 15
			};

			// Token: 0x04009C74 RID: 40052
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 105,
				radius = 20
			};

			// Token: 0x04009C75 RID: 40053
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 125,
				radius = 20
			};
		}

		// Token: 0x020021FF RID: 8703
		public class CREATURES
		{
			// Token: 0x04009C76 RID: 40054
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04009C77 RID: 40055
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 35,
				radius = 5
			};

			// Token: 0x04009C78 RID: 40056
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 45,
				radius = 5
			};

			// Token: 0x04009C79 RID: 40057
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 55,
				radius = 5
			};

			// Token: 0x04009C7A RID: 40058
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 65,
				radius = 5
			};

			// Token: 0x04009C7B RID: 40059
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 75,
				radius = 5
			};

			// Token: 0x04009C7C RID: 40060
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 90,
				radius = 10
			};

			// Token: 0x04009C7D RID: 40061
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 105,
				radius = 10
			};
		}

		// Token: 0x02002200 RID: 8704
		public class DAMPEN
		{
			// Token: 0x04009C7E RID: 40062
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x04009C7F RID: 40063
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x04009C80 RID: 40064
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x04009C81 RID: 40065
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04009C82 RID: 40066
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04009C83 RID: 40067
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}
	}
}
