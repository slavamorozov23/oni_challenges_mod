using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F6C RID: 3948
	public class NumberOfDupes : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D1F RID: 32031 RVA: 0x0031D4A2 File Offset: 0x0031B6A2
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, this.numDupes);
		}

		// Token: 0x06007D20 RID: 32032 RVA: 0x0031D4BE File Offset: 0x0031B6BE
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, this.numDupes);
		}

		// Token: 0x06007D21 RID: 32033 RVA: 0x0031D4DA File Offset: 0x0031B6DA
		public NumberOfDupes(int num)
		{
			this.numDupes = num;
		}

		// Token: 0x06007D22 RID: 32034 RVA: 0x0031D4E9 File Offset: 0x0031B6E9
		public override bool Success()
		{
			return Components.LiveMinionIdentities.Items.Count >= this.numDupes;
		}

		// Token: 0x06007D23 RID: 32035 RVA: 0x0031D505 File Offset: 0x0031B705
		public void Deserialize(IReader reader)
		{
			this.numDupes = reader.ReadInt32();
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x0031D513 File Offset: 0x0031B713
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, complete ? this.numDupes : Components.LiveMinionIdentities.Items.Count, this.numDupes);
		}

		// Token: 0x04005C27 RID: 23591
		private int numDupes;
	}
}
