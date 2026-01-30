using System;

namespace TUNING
{
	// Token: 0x02000FD1 RID: 4049
	public class STRESS
	{
		// Token: 0x04005E3E RID: 24126
		public static float ACTING_OUT_RESET = 60f;

		// Token: 0x04005E3F RID: 24127
		public static float VOMIT_AMOUNT = 0.90000004f;

		// Token: 0x04005E40 RID: 24128
		public static float TEARS_RATE = 0.040000003f;

		// Token: 0x04005E41 RID: 24129
		public static int BANSHEE_WAIL_RADIUS = 8;

		// Token: 0x020021F9 RID: 8697
		public class SHOCKER
		{
			// Token: 0x04009C4F RID: 40015
			public static int SHOCK_RADIUS = 4;

			// Token: 0x04009C50 RID: 40016
			public static float DAMAGE_RATE = 2.5f;

			// Token: 0x04009C51 RID: 40017
			public static float POWER_CONSUMPTION_RATE = 2000f;

			// Token: 0x04009C52 RID: 40018
			public static float FAKE_POWER_CONSUMPTION_RATE = STRESS.SHOCKER.POWER_CONSUMPTION_RATE * 0.25f;

			// Token: 0x04009C53 RID: 40019
			public static float MAX_POWER_USE = 120000f;
		}
	}
}
