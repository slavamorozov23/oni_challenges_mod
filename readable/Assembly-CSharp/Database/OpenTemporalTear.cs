using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F78 RID: 3960
	public class OpenTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D60 RID: 32096 RVA: 0x0031DC11 File Offset: 0x0031BE11
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.OPEN_TEMPORAL_TEAR;
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x0031DC1D File Offset: 0x0031BE1D
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D62 RID: 32098 RVA: 0x0031DC2B File Offset: 0x0031BE2B
		public override bool Success()
		{
			return ClusterManager.Instance.GetComponent<ClusterPOIManager>().IsTemporalTearOpen();
		}

		// Token: 0x06007D63 RID: 32099 RVA: 0x0031DC3C File Offset: 0x0031BE3C
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.OPEN_TEMPORAL_TEAR;
		}
	}
}
