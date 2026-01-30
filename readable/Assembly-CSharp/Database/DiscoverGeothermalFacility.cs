using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F74 RID: 3956
	public class DiscoverGeothermalFacility : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D4C RID: 32076 RVA: 0x0031DB1F File Offset: 0x0031BD1F
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x0031DB2D File Offset: 0x0031BD2D
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_DESCRIPTION;
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x0031DB39 File Offset: 0x0031BD39
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_TITLE;
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x0031DB45 File Offset: 0x0031BD45
		public override bool Success()
		{
			return GeothermalPlantComponent.GeothermalFacilityDiscovered();
		}
	}
}
