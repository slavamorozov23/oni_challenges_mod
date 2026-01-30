using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F9D RID: 3997
	public class TeleportDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x06007DFE RID: 32254 RVA: 0x003202E0 File Offset: 0x0031E4E0
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TELEPORT_DUPLICANT;
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x003202EC File Offset: 0x0031E4EC
		public override bool Success()
		{
			using (List<WarpReceiver>.Enumerator enumerator = Components.WarpReceivers.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Used)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
