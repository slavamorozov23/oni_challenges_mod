using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA5 RID: 4005
	public class SurviveARocketWithMinimumMorale : ColonyAchievementRequirement
	{
		// Token: 0x06007E16 RID: 32278 RVA: 0x0032063E File Offset: 0x0031E83E
		public SurviveARocketWithMinimumMorale(float minimumMorale, int numberOfCycles)
		{
			this.minimumMorale = minimumMorale;
			this.numberOfCycles = numberOfCycles;
		}

		// Token: 0x06007E17 RID: 32279 RVA: 0x00320654 File Offset: 0x0031E854
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE_COMPLETE, this.minimumMorale, this.numberOfCycles);
			}
			return base.GetProgress(complete);
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x00320688 File Offset: 0x0031E888
		public override bool Success()
		{
			foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement)
			{
				if (keyValuePair.Value >= this.numberOfCycles)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005C5E RID: 23646
		public float minimumMorale;

		// Token: 0x04005C5F RID: 23647
		public int numberOfCycles;
	}
}
