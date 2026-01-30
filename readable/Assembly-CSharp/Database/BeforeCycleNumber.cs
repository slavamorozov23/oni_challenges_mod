using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F6E RID: 3950
	public class BeforeCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D2B RID: 32043 RVA: 0x0031D5F4 File Offset: 0x0031B7F4
		public BeforeCycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x06007D2C RID: 32044 RVA: 0x0031D603 File Offset: 0x0031B803
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 <= this.cycleNumber;
		}

		// Token: 0x06007D2D RID: 32045 RVA: 0x0031D61C File Offset: 0x0031B81C
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x06007D2E RID: 32046 RVA: 0x0031D627 File Offset: 0x0031B827
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x0031D635 File Offset: 0x0031B835
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REMAINING_CYCLES, Mathf.Max(this.cycleNumber - GameClock.Instance.GetCycle(), 0), this.cycleNumber);
		}

		// Token: 0x04005C29 RID: 23593
		private int cycleNumber;
	}
}
