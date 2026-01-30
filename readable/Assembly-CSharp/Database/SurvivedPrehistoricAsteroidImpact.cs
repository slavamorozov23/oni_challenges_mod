using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F7C RID: 3964
	public class SurvivedPrehistoricAsteroidImpact : ColonyAchievementRequirement
	{
		// Token: 0x06007D76 RID: 32118 RVA: 0x0031DE5F File Offset: 0x0031C05F
		public SurvivedPrehistoricAsteroidImpact(int requiredCyclesAfterImpact)
		{
			this.requiredCyclesAfterImpact = requiredCyclesAfterImpact;
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x0031DE70 File Offset: 0x0031C070
		public override string GetProgress(bool complete)
		{
			int num = complete ? this.requiredCyclesAfterImpact : 0;
			if (!complete && SaveGame.Instance.ColonyAchievementTracker.largeImpactorLandedCycle >= 0)
			{
				num = Mathf.Clamp(GameClock.Instance.GetCycle() - SaveGame.Instance.ColonyAchievementTracker.largeImpactorLandedCycle, 0, this.requiredCyclesAfterImpact);
			}
			return GameUtil.SafeStringFormat(COLONY_ACHIEVEMENTS.ASTEROID_SURVIVED.REQUIREMENT_DESCRIPTION, new object[]
			{
				GameUtil.GetFormattedInt((float)num, GameUtil.TimeSlice.None),
				GameUtil.GetFormattedInt((float)this.requiredCyclesAfterImpact, GameUtil.TimeSlice.None)
			});
		}

		// Token: 0x06007D78 RID: 32120 RVA: 0x0031DEF6 File Offset: 0x0031C0F6
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.largeImpactorLandedCycle >= 0 && GameClock.Instance.GetCycle() - SaveGame.Instance.ColonyAchievementTracker.largeImpactorLandedCycle >= this.requiredCyclesAfterImpact;
		}

		// Token: 0x06007D79 RID: 32121 RVA: 0x0031DF31 File Offset: 0x0031C131
		public override bool Fail()
		{
			return SaveGame.Instance.ColonyAchievementTracker.largeImpactorState == ColonyAchievementTracker.LargeImpactorState.Defeated;
		}

		// Token: 0x04005C30 RID: 23600
		private int requiredCyclesAfterImpact;
	}
}
