using System;

namespace TUNING
{
	// Token: 0x02000FE9 RID: 4073
	public class METEORS
	{
		// Token: 0x02002246 RID: 8774
		public class DIFFICULTY
		{
			// Token: 0x02002AB5 RID: 10933
			public class PEROID_MULTIPLIER
			{
				// Token: 0x0400BC66 RID: 48230
				public const float INFREQUENT = 2f;

				// Token: 0x0400BC67 RID: 48231
				public const float INTENSE = 1f;

				// Token: 0x0400BC68 RID: 48232
				public const float DOOMED = 1f;
			}

			// Token: 0x02002AB6 RID: 10934
			public class SECONDS_PER_METEOR_MULTIPLIER
			{
				// Token: 0x0400BC69 RID: 48233
				public const float INFREQUENT = 1.5f;

				// Token: 0x0400BC6A RID: 48234
				public const float INTENSE = 0.8f;

				// Token: 0x0400BC6B RID: 48235
				public const float DOOMED = 0.5f;
			}

			// Token: 0x02002AB7 RID: 10935
			public class BOMBARD_OFF_MULTIPLIER
			{
				// Token: 0x0400BC6C RID: 48236
				public const float INFREQUENT = 1f;

				// Token: 0x0400BC6D RID: 48237
				public const float INTENSE = 1f;

				// Token: 0x0400BC6E RID: 48238
				public const float DOOMED = 0.5f;
			}

			// Token: 0x02002AB8 RID: 10936
			public class BOMBARD_ON_MULTIPLIER
			{
				// Token: 0x0400BC6F RID: 48239
				public const float INFREQUENT = 1f;

				// Token: 0x0400BC70 RID: 48240
				public const float INTENSE = 1f;

				// Token: 0x0400BC71 RID: 48241
				public const float DOOMED = 1f;
			}

			// Token: 0x02002AB9 RID: 10937
			public class MASS_MULTIPLIER
			{
				// Token: 0x0400BC72 RID: 48242
				public const float INFREQUENT = 1f;

				// Token: 0x0400BC73 RID: 48243
				public const float INTENSE = 0.8f;

				// Token: 0x0400BC74 RID: 48244
				public const float DOOMED = 0.5f;
			}
		}

		// Token: 0x02002247 RID: 8775
		public class IDENTIFY_DURATION
		{
			// Token: 0x04009E6A RID: 40554
			public const float TIER1 = 20f;
		}

		// Token: 0x02002248 RID: 8776
		public class PEROID
		{
			// Token: 0x04009E6B RID: 40555
			public const float TIER1 = 5f;

			// Token: 0x04009E6C RID: 40556
			public const float TIER2 = 10f;

			// Token: 0x04009E6D RID: 40557
			public const float TIER3 = 20f;

			// Token: 0x04009E6E RID: 40558
			public const float TIER4 = 30f;
		}

		// Token: 0x02002249 RID: 8777
		public class DURATION
		{
			// Token: 0x04009E6F RID: 40559
			public const float TIER0 = 1800f;

			// Token: 0x04009E70 RID: 40560
			public const float TIER1 = 3000f;

			// Token: 0x04009E71 RID: 40561
			public const float TIER2 = 4200f;

			// Token: 0x04009E72 RID: 40562
			public const float TIER3 = 6000f;
		}

		// Token: 0x0200224A RID: 8778
		public class DURATION_CLUSTER
		{
			// Token: 0x04009E73 RID: 40563
			public const float TIER0 = 75f;

			// Token: 0x04009E74 RID: 40564
			public const float TIER1 = 150f;

			// Token: 0x04009E75 RID: 40565
			public const float TIER2 = 300f;

			// Token: 0x04009E76 RID: 40566
			public const float TIER3 = 600f;

			// Token: 0x04009E77 RID: 40567
			public const float TIER4 = 1800f;

			// Token: 0x04009E78 RID: 40568
			public const float TIER5 = 3000f;
		}

		// Token: 0x0200224B RID: 8779
		public class TRAVEL_DURATION
		{
			// Token: 0x04009E79 RID: 40569
			public const float TIER0 = 600f;

			// Token: 0x04009E7A RID: 40570
			public const float TIER1 = 3000f;

			// Token: 0x04009E7B RID: 40571
			public const float TIER2 = 4500f;

			// Token: 0x04009E7C RID: 40572
			public const float TIER3 = 6000f;

			// Token: 0x04009E7D RID: 40573
			public const float TIER4 = 12000f;

			// Token: 0x04009E7E RID: 40574
			public const float TIER5 = 30000f;

			// Token: 0x04009E7F RID: 40575
			public const float TIER6 = 60000f;
		}

		// Token: 0x0200224C RID: 8780
		public class BOMBARDMENT_ON
		{
			// Token: 0x04009E80 RID: 40576
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);

			// Token: 0x04009E81 RID: 40577
			public static MathUtil.MinMax UNLIMITED = new MathUtil.MinMax(10000f, 10000f);

			// Token: 0x04009E82 RID: 40578
			public static MathUtil.MinMax CYCLE = new MathUtil.MinMax(600f, 600f);
		}

		// Token: 0x0200224D RID: 8781
		public class BOMBARDMENT_OFF
		{
			// Token: 0x04009E83 RID: 40579
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);
		}

		// Token: 0x0200224E RID: 8782
		public class TRAVELDURATION
		{
			// Token: 0x04009E84 RID: 40580
			public static float TIER0 = 0f;

			// Token: 0x04009E85 RID: 40581
			public static float TIER1 = 5f;

			// Token: 0x04009E86 RID: 40582
			public static float TIER2 = 10f;

			// Token: 0x04009E87 RID: 40583
			public static float TIER3 = 20f;

			// Token: 0x04009E88 RID: 40584
			public static float TIER4 = 30f;
		}
	}
}
