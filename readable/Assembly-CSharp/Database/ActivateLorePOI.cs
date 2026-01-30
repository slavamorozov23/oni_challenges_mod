using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F92 RID: 3986
	public class ActivateLorePOI : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DD3 RID: 32211 RVA: 0x0031F9E2 File Offset: 0x0031DBE2
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DD4 RID: 32212 RVA: 0x0031F9E4 File Offset: 0x0031DBE4
		public override bool Success()
		{
			foreach (BuildingComplete buildingComplete in Components.TemplateBuildings.Items)
			{
				if (!(buildingComplete == null))
				{
					Unsealable component = buildingComplete.GetComponent<Unsealable>();
					if (component != null && component.unsealed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x0031FA5C File Offset: 0x0031DC5C
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.INVESTIGATE_A_POI;
		}
	}
}
