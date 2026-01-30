using System;

namespace TUNING
{
	// Token: 0x02000FD0 RID: 4048
	public class SKILLS
	{
		// Token: 0x04005E32 RID: 24114
		public static int TARGET_SKILLS_EARNED = 15;

		// Token: 0x04005E33 RID: 24115
		public static int TARGET_SKILLS_CYCLE = 250;

		// Token: 0x04005E34 RID: 24116
		public static float EXPERIENCE_LEVEL_POWER = 1.44f;

		// Token: 0x04005E35 RID: 24117
		public static float PASSIVE_EXPERIENCE_PORTION = 0.5f;

		// Token: 0x04005E36 RID: 24118
		public static float ACTIVE_EXPERIENCE_PORTION = 0.6f;

		// Token: 0x04005E37 RID: 24119
		public static float FULL_EXPERIENCE = 1f;

		// Token: 0x04005E38 RID: 24120
		public static float ALL_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.9f;

		// Token: 0x04005E39 RID: 24121
		public static float MOST_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.75f;

		// Token: 0x04005E3A RID: 24122
		public static float PART_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.5f;

		// Token: 0x04005E3B RID: 24123
		public static float BARELY_EVER_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.25f;

		// Token: 0x04005E3C RID: 24124
		public static float APTITUDE_EXPERIENCE_MULTIPLIER = 0.5f;

		// Token: 0x04005E3D RID: 24125
		public static int[] SKILL_TIER_MORALE_COST = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
	}
}
