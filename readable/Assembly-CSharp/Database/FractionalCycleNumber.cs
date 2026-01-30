using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F6F RID: 3951
	public class FractionalCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D30 RID: 32048 RVA: 0x0031D66D File Offset: 0x0031B86D
		public FractionalCycleNumber(float fractionalCycleNumber)
		{
			this.fractionalCycleNumber = fractionalCycleNumber;
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x0031D67C File Offset: 0x0031B87C
		public override bool Success()
		{
			int num = (int)this.fractionalCycleNumber;
			float num2 = this.fractionalCycleNumber - (float)num;
			return (float)(GameClock.Instance.GetCycle() + 1) > this.fractionalCycleNumber || (GameClock.Instance.GetCycle() + 1 == num && GameClock.Instance.GetCurrentCycleAsPercentage() >= num2);
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x0031D6D3 File Offset: 0x0031B8D3
		public void Deserialize(IReader reader)
		{
			this.fractionalCycleNumber = reader.ReadSingle();
		}

		// Token: 0x06007D33 RID: 32051 RVA: 0x0031D6E4 File Offset: 0x0031B8E4
		public override string GetProgress(bool complete)
		{
			float num = (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage();
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.FRACTIONAL_CYCLE, complete ? this.fractionalCycleNumber : num, this.fractionalCycleNumber);
		}

		// Token: 0x04005C2A RID: 23594
		private float fractionalCycleNumber;
	}
}
