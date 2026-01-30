using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA2 RID: 4002
	public class LandOnAllWorlds : ColonyAchievementRequirement
	{
		// Token: 0x06007E0D RID: 32269 RVA: 0x003204B8 File Offset: 0x0031E6B8
		public override string GetProgress(bool complete)
		{
			int num = 0;
			int num2 = 0;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					num++;
					if (worldContainer.IsDupeVisited || worldContainer.IsRoverVisted)
					{
						num2++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAND_DUPES_ON_ALL_WORLDS, num2, num);
		}

		// Token: 0x06007E0E RID: 32270 RVA: 0x00320548 File Offset: 0x0031E748
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && !worldContainer.IsDupeVisited && !worldContainer.IsRoverVisted)
				{
					return false;
				}
			}
			return true;
		}
	}
}
