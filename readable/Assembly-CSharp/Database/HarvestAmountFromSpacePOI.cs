using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA1 RID: 4001
	public class HarvestAmountFromSpacePOI : ColonyAchievementRequirement
	{
		// Token: 0x06007E0A RID: 32266 RVA: 0x0032045D File Offset: 0x0031E65D
		public HarvestAmountFromSpacePOI(float amountToHarvest)
		{
			this.amountToHarvest = amountToHarvest;
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x0032046C File Offset: 0x0031E66C
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MINE_SPACE_POI, SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI, this.amountToHarvest);
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x0032049C File Offset: 0x0031E69C
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI > this.amountToHarvest;
		}

		// Token: 0x04005C5C RID: 23644
		private float amountToHarvest;
	}
}
