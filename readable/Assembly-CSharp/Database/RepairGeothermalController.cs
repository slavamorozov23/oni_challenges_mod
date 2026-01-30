using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F75 RID: 3957
	public class RepairGeothermalController : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D51 RID: 32081 RVA: 0x0031DB54 File Offset: 0x0031BD54
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D52 RID: 32082 RVA: 0x0031DB62 File Offset: 0x0031BD62
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_DESCRIPTION;
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x0031DB6E File Offset: 0x0031BD6E
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_TITLE;
		}

		// Token: 0x06007D54 RID: 32084 RVA: 0x0031DB7A File Offset: 0x0031BD7A
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
		}
	}
}
