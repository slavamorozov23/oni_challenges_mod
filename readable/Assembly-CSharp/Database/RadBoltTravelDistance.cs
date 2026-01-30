using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA3 RID: 4003
	public class RadBoltTravelDistance : ColonyAchievementRequirement
	{
		// Token: 0x06007E10 RID: 32272 RVA: 0x003205C0 File Offset: 0x0031E7C0
		public RadBoltTravelDistance(int travelDistance)
		{
			this.travelDistance = travelDistance;
		}

		// Token: 0x06007E11 RID: 32273 RVA: 0x003205CF File Offset: 0x0031E7CF
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RADBOLT_TRAVEL, SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance, this.travelDistance);
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x003205FF File Offset: 0x0031E7FF
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance > (float)this.travelDistance;
		}

		// Token: 0x04005C5D RID: 23645
		private int travelDistance;
	}
}
