using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F9E RID: 3998
	public class DefrostDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x06007E01 RID: 32257 RVA: 0x00320354 File Offset: 0x0031E554
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.DEFROST_DUPLICANT;
		}

		// Token: 0x06007E02 RID: 32258 RVA: 0x00320360 File Offset: 0x0031E560
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant;
		}
	}
}
