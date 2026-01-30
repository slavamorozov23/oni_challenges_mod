using System;

namespace TUNING
{
	// Token: 0x02000FDE RID: 4062
	public class RADIATION
	{
		// Token: 0x04005EDF RID: 24287
		public const float GERM_RAD_SCALE = 0.01f;

		// Token: 0x04005EE0 RID: 24288
		public const float STANDARD_DAILY_RECOVERY = 100f;

		// Token: 0x04005EE1 RID: 24289
		public const float EXTRA_VOMIT_RECOVERY = 20f;

		// Token: 0x04005EE2 RID: 24290
		public const float REACT_THRESHOLD = 133f;

		// Token: 0x0200222C RID: 8748
		public class STANDARD_EMITTER
		{
			// Token: 0x04009DBC RID: 40380
			public const float STEADY_PULSE_RATE = 0.2f;

			// Token: 0x04009DBD RID: 40381
			public const float DOUBLE_SPEED_PULSE_RATE = 0.1f;

			// Token: 0x04009DBE RID: 40382
			public const float RADIUS_SCALE = 1f;
		}

		// Token: 0x0200222D RID: 8749
		public class RADIATION_PER_SECOND
		{
			// Token: 0x04009DBF RID: 40383
			public const float TRIVIAL = 60f;

			// Token: 0x04009DC0 RID: 40384
			public const float VERY_LOW = 120f;

			// Token: 0x04009DC1 RID: 40385
			public const float LOW = 240f;

			// Token: 0x04009DC2 RID: 40386
			public const float MODERATE = 600f;

			// Token: 0x04009DC3 RID: 40387
			public const float HIGH = 1800f;

			// Token: 0x04009DC4 RID: 40388
			public const float VERY_HIGH = 4800f;

			// Token: 0x04009DC5 RID: 40389
			public const int EXTREME = 9600;
		}

		// Token: 0x0200222E RID: 8750
		public class RADIATION_CONSTANT_RADS_PER_CYCLE
		{
			// Token: 0x04009DC6 RID: 40390
			public const float LESS_THAN_TRIVIAL = 60f;

			// Token: 0x04009DC7 RID: 40391
			public const float TRIVIAL = 120f;

			// Token: 0x04009DC8 RID: 40392
			public const float VERY_LOW = 240f;

			// Token: 0x04009DC9 RID: 40393
			public const float LOW = 480f;

			// Token: 0x04009DCA RID: 40394
			public const float MODERATE = 1200f;

			// Token: 0x04009DCB RID: 40395
			public const float MODERATE_PLUS = 2400f;

			// Token: 0x04009DCC RID: 40396
			public const float HIGH = 3600f;

			// Token: 0x04009DCD RID: 40397
			public const float VERY_HIGH = 8400f;

			// Token: 0x04009DCE RID: 40398
			public const int EXTREME = 16800;
		}
	}
}
