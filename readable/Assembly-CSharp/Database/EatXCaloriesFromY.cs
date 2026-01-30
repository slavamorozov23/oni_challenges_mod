using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F87 RID: 3975
	public class EatXCaloriesFromY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DA6 RID: 32166 RVA: 0x0031EC39 File Offset: 0x0031CE39
		public EatXCaloriesFromY(int numCalories, List<string> fromFoodType)
		{
			this.numCalories = numCalories;
			this.fromFoodType = fromFoodType;
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x0031EC5A File Offset: 0x0031CE5A
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType) / 1000f > (float)this.numCalories;
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x0031EC7C File Offset: 0x0031CE7C
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
			int num = reader.ReadInt32();
			this.fromFoodType = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadKleiString();
				this.fromFoodType.Add(item);
			}
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x0031ECC8 File Offset: 0x0031CEC8
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIES_FROM_MEAT, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04005C42 RID: 23618
		private int numCalories;

		// Token: 0x04005C43 RID: 23619
		private List<string> fromFoodType = new List<string>();
	}
}
