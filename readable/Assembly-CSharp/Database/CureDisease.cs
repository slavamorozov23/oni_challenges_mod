using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F94 RID: 3988
	public class CureDisease : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DDB RID: 32219 RVA: 0x0031FB48 File Offset: 0x0031DD48
		public override bool Success()
		{
			return Game.Instance.savedInfo.curedDisease;
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x0031FB59 File Offset: 0x0031DD59
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x0031FB5B File Offset: 0x0031DD5B
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CURED_DISEASE;
		}
	}
}
