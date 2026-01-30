using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F9F RID: 3999
	public class BuildALaunchPad : ColonyAchievementRequirement
	{
		// Token: 0x06007E04 RID: 32260 RVA: 0x00320379 File Offset: 0x0031E579
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILD_A_LAUNCHPAD;
		}

		// Token: 0x06007E05 RID: 32261 RVA: 0x00320388 File Offset: 0x0031E588
		public override bool Success()
		{
			foreach (LaunchPad component in Components.LaunchPads.Items)
			{
				WorldContainer myWorld = component.GetMyWorld();
				if (!myWorld.IsStartWorld && Components.WarpReceivers.GetWorldItems(myWorld.id, false).Count == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
