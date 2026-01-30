using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F88 RID: 3976
	public class EatXCalories : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DAA RID: 32170 RVA: 0x0031ED21 File Offset: 0x0031CF21
		public EatXCalories(int numCalories)
		{
			this.numCalories = numCalories;
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x0031ED30 File Offset: 0x0031CF30
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed() / 1000f > (float)this.numCalories;
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x0031ED4B File Offset: 0x0031CF4B
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x0031ED5C File Offset: 0x0031CF5C
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04005C44 RID: 23620
		private int numCalories;
	}
}
