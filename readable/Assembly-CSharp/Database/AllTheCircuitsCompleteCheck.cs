using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA8 RID: 4008
	public class AllTheCircuitsCompleteCheck : ColonyAchievementRequirement
	{
		// Token: 0x06007E1F RID: 32287 RVA: 0x0032081D File Offset: 0x0031EA1D
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MVB_DESCRIPTION, 8);
		}

		// Token: 0x06007E20 RID: 32288 RVA: 0x00320834 File Offset: 0x0031EA34
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.fullyBoostedBionic;
		}
	}
}
