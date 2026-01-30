using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F98 RID: 3992
	public class BlockedCometWithBunkerDoor : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DEC RID: 32236 RVA: 0x0031FF17 File Offset: 0x0031E117
		public override bool Success()
		{
			return Game.Instance.savedInfo.blockedCometWithBunkerDoor;
		}

		// Token: 0x06007DED RID: 32237 RVA: 0x0031FF28 File Offset: 0x0031E128
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DEE RID: 32238 RVA: 0x0031FF2A File Offset: 0x0031E12A
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BLOCKED_A_COMET;
		}
	}
}
