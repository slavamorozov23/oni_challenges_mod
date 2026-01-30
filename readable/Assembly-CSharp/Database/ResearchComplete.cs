using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F80 RID: 3968
	public class ResearchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D87 RID: 32135 RVA: 0x0031E1E8 File Offset: 0x0031C3E8
		public override bool Success()
		{
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007D88 RID: 32136 RVA: 0x0031E24C File Offset: 0x0031C44C
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007D89 RID: 32137 RVA: 0x0031E250 File Offset: 0x0031C450
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, Db.Get().Techs.resources.Count, Db.Get().Techs.resources.Count);
			}
			int num = 0;
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsComplete())
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, num, Db.Get().Techs.resources.Count);
		}
	}
}
