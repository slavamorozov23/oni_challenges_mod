using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F99 RID: 3993
	public class DupesVsSolidTransferArmFetch : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DEF RID: 32239 RVA: 0x0031FF36 File Offset: 0x0031E136
		public DupesVsSolidTransferArmFetch(float percentage, int numCycles)
		{
			this.percentage = percentage;
			this.numCycles = numCycles;
		}

		// Token: 0x06007DF0 RID: 32240 RVA: 0x0031FF4C File Offset: 0x0031E14C
		public override bool Success()
		{
			Dictionary<int, int> fetchDupeChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchDupeChoreDeliveries;
			Dictionary<int, int> fetchAutomatedChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchAutomatedChoreDeliveries;
			int num = 0;
			this.currentCycleCount = 0;
			for (int i = GameClock.Instance.GetCycle() - 1; i >= GameClock.Instance.GetCycle() - this.numCycles; i--)
			{
				if (fetchAutomatedChoreDeliveries.ContainsKey(i))
				{
					if (fetchDupeChoreDeliveries.ContainsKey(i) && (float)fetchDupeChoreDeliveries[i] >= (float)fetchAutomatedChoreDeliveries[i] * this.percentage)
					{
						break;
					}
					num++;
				}
				else if (fetchDupeChoreDeliveries.ContainsKey(i))
				{
					num = 0;
					break;
				}
			}
			this.currentCycleCount = Math.Max(this.currentCycleCount, num);
			return num >= this.numCycles;
		}

		// Token: 0x06007DF1 RID: 32241 RVA: 0x00320005 File Offset: 0x0031E205
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
			this.percentage = reader.ReadSingle();
		}

		// Token: 0x04005C55 RID: 23637
		public float percentage;

		// Token: 0x04005C56 RID: 23638
		public int numCycles;

		// Token: 0x04005C57 RID: 23639
		public int currentCycleCount;

		// Token: 0x04005C58 RID: 23640
		public bool armsOutPerformingDupesThisCycle;
	}
}
