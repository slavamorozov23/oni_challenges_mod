using System;

namespace TUNING
{
	// Token: 0x02000FD7 RID: 4055
	public class DISEASE
	{
		// Token: 0x04005E8E RID: 24206
		public const int COUNT_SCALER = 1000;

		// Token: 0x04005E8F RID: 24207
		public const int GENERIC_EMIT_COUNT = 100000;

		// Token: 0x04005E90 RID: 24208
		public const float GENERIC_EMIT_INTERVAL = 5f;

		// Token: 0x04005E91 RID: 24209
		public const float GENERIC_INFECTION_RADIUS = 1.5f;

		// Token: 0x04005E92 RID: 24210
		public const float GENERIC_INFECTION_INTERVAL = 5f;

		// Token: 0x04005E93 RID: 24211
		public const float STINKY_EMIT_MASS = 0.0025000002f;

		// Token: 0x04005E94 RID: 24212
		public const float STINKY_EMIT_INTERVAL = 2.5f;

		// Token: 0x04005E95 RID: 24213
		public const float STORAGE_TRANSFER_RATE = 0.05f;

		// Token: 0x04005E96 RID: 24214
		public const float WORKABLE_TRANSFER_RATE = 0.33f;

		// Token: 0x04005E97 RID: 24215
		public const float LADDER_TRANSFER_RATE = 0.005f;

		// Token: 0x04005E98 RID: 24216
		public const float INTERNAL_GERM_DEATH_MULTIPLIER = -0.00066666666f;

		// Token: 0x04005E99 RID: 24217
		public const float INTERNAL_GERM_DEATH_ADDEND = -0.8333333f;

		// Token: 0x04005E9A RID: 24218
		public const float MINIMUM_IMMUNE_DAMAGE = 0.00016666666f;

		// Token: 0x02002218 RID: 8728
		public class DURATION
		{
			// Token: 0x04009D0D RID: 40205
			public const float LONG = 10800f;

			// Token: 0x04009D0E RID: 40206
			public const float LONGISH = 4620f;

			// Token: 0x04009D0F RID: 40207
			public const float NORMAL = 2220f;

			// Token: 0x04009D10 RID: 40208
			public const float SHORT = 1020f;

			// Token: 0x04009D11 RID: 40209
			public const float TEMPORARY = 180f;

			// Token: 0x04009D12 RID: 40210
			public const float VERY_BRIEF = 60f;
		}

		// Token: 0x02002219 RID: 8729
		public class IMMUNE_ATTACK_STRENGTH_PERCENT
		{
			// Token: 0x04009D13 RID: 40211
			public const float SLOW_3 = 0.00025f;

			// Token: 0x04009D14 RID: 40212
			public const float SLOW_2 = 0.0005f;

			// Token: 0x04009D15 RID: 40213
			public const float SLOW_1 = 0.00125f;

			// Token: 0x04009D16 RID: 40214
			public const float NORMAL = 0.005f;

			// Token: 0x04009D17 RID: 40215
			public const float FAST_1 = 0.0125f;

			// Token: 0x04009D18 RID: 40216
			public const float FAST_2 = 0.05f;

			// Token: 0x04009D19 RID: 40217
			public const float FAST_3 = 0.125f;
		}

		// Token: 0x0200221A RID: 8730
		public class RADIATION_KILL_RATE
		{
			// Token: 0x04009D1A RID: 40218
			public const float NO_EFFECT = 0f;

			// Token: 0x04009D1B RID: 40219
			public const float SLOW = 1f;

			// Token: 0x04009D1C RID: 40220
			public const float NORMAL = 2.5f;

			// Token: 0x04009D1D RID: 40221
			public const float FAST = 5f;
		}

		// Token: 0x0200221B RID: 8731
		public static class GROWTH_FACTOR
		{
			// Token: 0x04009D1E RID: 40222
			public const float NONE = float.PositiveInfinity;

			// Token: 0x04009D1F RID: 40223
			public const float DEATH_1 = 12000f;

			// Token: 0x04009D20 RID: 40224
			public const float DEATH_2 = 6000f;

			// Token: 0x04009D21 RID: 40225
			public const float DEATH_3 = 3000f;

			// Token: 0x04009D22 RID: 40226
			public const float DEATH_4 = 1200f;

			// Token: 0x04009D23 RID: 40227
			public const float DEATH_5 = 300f;

			// Token: 0x04009D24 RID: 40228
			public const float DEATH_MAX = 10f;

			// Token: 0x04009D25 RID: 40229
			public const float DEATH_INSTANT = 0f;

			// Token: 0x04009D26 RID: 40230
			public const float GROWTH_1 = -12000f;

			// Token: 0x04009D27 RID: 40231
			public const float GROWTH_2 = -6000f;

			// Token: 0x04009D28 RID: 40232
			public const float GROWTH_3 = -3000f;

			// Token: 0x04009D29 RID: 40233
			public const float GROWTH_4 = -1200f;

			// Token: 0x04009D2A RID: 40234
			public const float GROWTH_5 = -600f;

			// Token: 0x04009D2B RID: 40235
			public const float GROWTH_6 = -300f;

			// Token: 0x04009D2C RID: 40236
			public const float GROWTH_7 = -150f;
		}

		// Token: 0x0200221C RID: 8732
		public static class UNDERPOPULATION_DEATH_RATE
		{
			// Token: 0x04009D2D RID: 40237
			public const float NONE = 0f;

			// Token: 0x04009D2E RID: 40238
			private const float BASE_NUM_TO_KILL = 400f;

			// Token: 0x04009D2F RID: 40239
			public const float SLOW = 0.6666667f;

			// Token: 0x04009D30 RID: 40240
			public const float FAST = 2.6666667f;
		}
	}
}
