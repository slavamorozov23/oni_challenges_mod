using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA4 RID: 4004
	public class HarvestAHiveWithoutBeingStung : ColonyAchievementRequirement
	{
		// Token: 0x06007E13 RID: 32275 RVA: 0x00320619 File Offset: 0x0031E819
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HARVEST_HIVE;
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x00320625 File Offset: 0x0031E825
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung;
		}
	}
}
