using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA7 RID: 4007
	public class EfficientDataMiningCheck : ColonyAchievementRequirement
	{
		// Token: 0x06007E1C RID: 32284 RVA: 0x003207F8 File Offset: 0x0031E9F8
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.DATA_DRIVEN_DESCRIPTION;
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x00320804 File Offset: 0x0031EA04
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.efficientlyGatheredData;
		}
	}
}
