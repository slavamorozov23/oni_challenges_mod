using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F8C RID: 3980
	public class ExploreOilFieldSubZone : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DBB RID: 32187 RVA: 0x0031F248 File Offset: 0x0031D448
		public override bool Success()
		{
			return Game.Instance.savedInfo.discoveredOilField;
		}

		// Token: 0x06007DBC RID: 32188 RVA: 0x0031F259 File Offset: 0x0031D459
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DBD RID: 32189 RVA: 0x0031F25B File Offset: 0x0031D45B
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ENTER_OIL_BIOME;
		}
	}
}
