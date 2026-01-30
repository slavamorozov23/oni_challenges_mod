using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F7A RID: 3962
	public class NoDuplicantsCanDie : ColonyAchievementRequirement
	{
		// Token: 0x06007D6C RID: 32108 RVA: 0x0031DD2F File Offset: 0x0031BF2F
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.NO_DUPES_HAVE_DIED.REQUIREMENT_NAME;
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x0031DD3B File Offset: 0x0031BF3B
		public override bool Success()
		{
			return !SaveGame.Instance.ColonyAchievementTracker.HasAnyDupeDied;
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x0031DD4F File Offset: 0x0031BF4F
		public override bool Fail()
		{
			return SaveGame.Instance.ColonyAchievementTracker.HasAnyDupeDied;
		}
	}
}
