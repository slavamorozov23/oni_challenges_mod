using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F9B RID: 3995
	public class SentCraftIntoTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007DF6 RID: 32246 RVA: 0x00320218 File Offset: 0x0031E418
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x0032022E File Offset: 0x0031E42E
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x06007DF8 RID: 32248 RVA: 0x00320244 File Offset: 0x0031E444
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x06007DF9 RID: 32249 RVA: 0x00320250 File Offset: 0x0031E450
		public override bool Success()
		{
			return ClusterManager.Instance.GetClusterPOIManager().HasTemporalTearConsumedCraft();
		}
	}
}
