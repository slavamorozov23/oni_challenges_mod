using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F85 RID: 3973
	public class CoolBuildingToXKelvin : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D9D RID: 32157 RVA: 0x0031EAC8 File Offset: 0x0031CCC8
		public CoolBuildingToXKelvin(int kelvinToCoolTo)
		{
			this.kelvinToCoolTo = kelvinToCoolTo;
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x0031EAD7 File Offset: 0x0031CCD7
		public override bool Success()
		{
			return BuildingComplete.MinKelvinSeen <= (float)this.kelvinToCoolTo;
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x0031EAEA File Offset: 0x0031CCEA
		public void Deserialize(IReader reader)
		{
			this.kelvinToCoolTo = reader.ReadInt32();
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x0031EAF8 File Offset: 0x0031CCF8
		public override string GetProgress(bool complete)
		{
			float minKelvinSeen = BuildingComplete.MinKelvinSeen;
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, minKelvinSeen);
		}

		// Token: 0x04005C41 RID: 23617
		private int kelvinToCoolTo;
	}
}
