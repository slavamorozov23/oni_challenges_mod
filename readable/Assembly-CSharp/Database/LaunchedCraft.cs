using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x02000F9C RID: 3996
	public class LaunchedCraft : ColonyAchievementRequirement
	{
		// Token: 0x06007DFB RID: 32251 RVA: 0x00320269 File Offset: 0x0031E469
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
		}

		// Token: 0x06007DFC RID: 32252 RVA: 0x00320278 File Offset: 0x0031E478
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Clustercraft)enumerator.Current).Status == Clustercraft.CraftStatus.InFlight)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
