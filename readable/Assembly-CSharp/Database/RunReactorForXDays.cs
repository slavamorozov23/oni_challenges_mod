using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000FA6 RID: 4006
	public class RunReactorForXDays : ColonyAchievementRequirement
	{
		// Token: 0x06007E19 RID: 32281 RVA: 0x003206F4 File Offset: 0x0031E8F4
		public RunReactorForXDays(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x00320704 File Offset: 0x0031E904
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (Reactor reactor in Components.NuclearReactors.Items)
			{
				if (reactor.numCyclesRunning > num)
				{
					num = reactor.numCyclesRunning;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RUN_A_REACTOR, complete ? this.numCycles : num, this.numCycles);
		}

		// Token: 0x06007E1B RID: 32283 RVA: 0x00320794 File Offset: 0x0031E994
		public override bool Success()
		{
			using (List<Reactor>.Enumerator enumerator = Components.NuclearReactors.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.numCyclesRunning >= this.numCycles)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005C60 RID: 23648
		private int numCycles;
	}
}
