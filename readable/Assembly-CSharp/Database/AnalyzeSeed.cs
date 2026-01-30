using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA0 RID: 4000
	public class AnalyzeSeed : ColonyAchievementRequirement
	{
		// Token: 0x06007E07 RID: 32263 RVA: 0x0032040C File Offset: 0x0031E60C
		public AnalyzeSeed(string seedname)
		{
			this.seedName = seedname;
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x0032041B File Offset: 0x0031E61B
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ANALYZE_SEED, this.seedName.ProperName());
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x0032043C File Offset: 0x0031E63C
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.analyzedSeeds.Contains(this.seedName);
		}

		// Token: 0x04005C5B RID: 23643
		private string seedName;
	}
}
