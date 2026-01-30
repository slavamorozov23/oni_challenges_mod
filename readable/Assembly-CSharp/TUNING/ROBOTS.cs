using System;

namespace TUNING
{
	// Token: 0x02000FD4 RID: 4052
	public class ROBOTS
	{
		// Token: 0x02002201 RID: 8705
		public class SCOUTBOT
		{
			// Token: 0x04009C84 RID: 40068
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY;

			// Token: 0x04009C85 RID: 40069
			public static readonly float DIGGING = 1f;

			// Token: 0x04009C86 RID: 40070
			public static readonly float CONSTRUCTION = 1f;

			// Token: 0x04009C87 RID: 40071
			public static readonly float ATHLETICS = 1f;

			// Token: 0x04009C88 RID: 40072
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009C89 RID: 40073
			public static readonly float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04009C8A RID: 40074
			public static readonly float BATTERY_CAPACITY = ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE * 10f * 600f;
		}

		// Token: 0x02002202 RID: 8706
		public class MORBBOT
		{
			// Token: 0x04009C8B RID: 40075
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x04009C8C RID: 40076
			public const float DIGGING = 1f;

			// Token: 0x04009C8D RID: 40077
			public const float CONSTRUCTION = 1f;

			// Token: 0x04009C8E RID: 40078
			public const float ATHLETICS = 3f;

			// Token: 0x04009C8F RID: 40079
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009C90 RID: 40080
			public const float LIFETIME = 6000f;

			// Token: 0x04009C91 RID: 40081
			public const float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04009C92 RID: 40082
			public const float BATTERY_CAPACITY = 180000f;

			// Token: 0x04009C93 RID: 40083
			public const float DECONSTRUCTION_WORK_TIME = 10f;
		}

		// Token: 0x02002203 RID: 8707
		public class FETCHDRONE
		{
			// Token: 0x04009C94 RID: 40084
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x04009C95 RID: 40085
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009C96 RID: 40086
			public const float BATTERY_DEPLETION_RATE = 50f;
		}
	}
}
