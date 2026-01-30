using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F76 RID: 3958
	public class UseGeothermalPlant : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D56 RID: 32086 RVA: 0x0031DB93 File Offset: 0x0031BD93
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D57 RID: 32087 RVA: 0x0031DBA1 File Offset: 0x0031BDA1
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_TITLE;
		}

		// Token: 0x06007D58 RID: 32088 RVA: 0x0031DBAD File Offset: 0x0031BDAD
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented;
		}

		// Token: 0x06007D59 RID: 32089 RVA: 0x0031DBBE File Offset: 0x0031BDBE
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_DESCRIPTION;
		}
	}
}
