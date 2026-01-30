using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F7F RID: 3967
	public class CalorieSurplus : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D82 RID: 32130 RVA: 0x0031E164 File Offset: 0x0031C364
		public CalorieSurplus(float surplusAmount)
		{
			this.surplusAmount = (double)surplusAmount;
		}

		// Token: 0x06007D83 RID: 32131 RVA: 0x0031E174 File Offset: 0x0031C374
		public override bool Success()
		{
			return (double)(ClusterManager.Instance.CountAllRations() / 1000f) >= this.surplusAmount;
		}

		// Token: 0x06007D84 RID: 32132 RVA: 0x0031E192 File Offset: 0x0031C392
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x06007D85 RID: 32133 RVA: 0x0031E19D File Offset: 0x0031C39D
		public void Deserialize(IReader reader)
		{
			this.surplusAmount = reader.ReadDouble();
		}

		// Token: 0x06007D86 RID: 32134 RVA: 0x0031E1AB File Offset: 0x0031C3AB
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, GameUtil.GetFormattedCalories(complete ? ((float)this.surplusAmount) : ClusterManager.Instance.CountAllRations(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.surplusAmount, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04005C34 RID: 23604
		private double surplusAmount;
	}
}
