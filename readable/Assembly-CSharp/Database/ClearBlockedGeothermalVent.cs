using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F77 RID: 3959
	public class ClearBlockedGeothermalVent : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D5B RID: 32091 RVA: 0x0031DBD2 File Offset: 0x0031BDD2
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D5C RID: 32092 RVA: 0x0031DBE0 File Offset: 0x0031BDE0
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_TITLE;
		}

		// Token: 0x06007D5D RID: 32093 RVA: 0x0031DBEC File Offset: 0x0031BDEC
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent;
		}

		// Token: 0x06007D5E RID: 32094 RVA: 0x0031DBFD File Offset: 0x0031BDFD
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_DESCRIPTION;
		}
	}
}
