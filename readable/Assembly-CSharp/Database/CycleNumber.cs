using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F6D RID: 3949
	public class CycleNumber : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D25 RID: 32037 RVA: 0x0031D54E File Offset: 0x0031B74E
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, this.cycleNumber);
		}

		// Token: 0x06007D26 RID: 32038 RVA: 0x0031D56A File Offset: 0x0031B76A
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, this.cycleNumber);
		}

		// Token: 0x06007D27 RID: 32039 RVA: 0x0031D586 File Offset: 0x0031B786
		public CycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x06007D28 RID: 32040 RVA: 0x0031D595 File Offset: 0x0031B795
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;
		}

		// Token: 0x06007D29 RID: 32041 RVA: 0x0031D5AE File Offset: 0x0031B7AE
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x06007D2A RID: 32042 RVA: 0x0031D5BC File Offset: 0x0031B7BC
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, complete ? this.cycleNumber : (GameClock.Instance.GetCycle() + 1), this.cycleNumber);
		}

		// Token: 0x04005C28 RID: 23592
		private int cycleNumber;
	}
}
