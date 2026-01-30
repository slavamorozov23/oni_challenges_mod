using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F7B RID: 3963
	public class DefeatPrehistoricAsteroid : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D70 RID: 32112 RVA: 0x0031DD68 File Offset: 0x0031BF68
		public override string GetProgress(bool complete)
		{
			int num = 1000;
			int num2 = complete ? num : 0;
			GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
			if (gameplayEventInstance != null)
			{
				LargeImpactorEvent.StatesInstance statesInstance = (LargeImpactorEvent.StatesInstance)gameplayEventInstance.smi;
				if (statesInstance != null && statesInstance.impactorInstance != null)
				{
					LargeImpactorStatus.Instance smi = statesInstance.impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
					num = smi.def.MAX_HEALTH;
					num2 = num - smi.Health;
				}
			}
			return GameUtil.SafeStringFormat(COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.REQUIREMENT_DESCRIPTION, new object[]
			{
				GameUtil.GetFormattedInt((float)num2, GameUtil.TimeSlice.None),
				GameUtil.GetFormattedInt((float)num, GameUtil.TimeSlice.None)
			});
		}

		// Token: 0x06007D71 RID: 32113 RVA: 0x0031DE17 File Offset: 0x0031C017
		public override string Description()
		{
			return COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.DESCRIPTION;
		}

		// Token: 0x06007D72 RID: 32114 RVA: 0x0031DE23 File Offset: 0x0031C023
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.largeImpactorState == ColonyAchievementTracker.LargeImpactorState.Defeated;
		}

		// Token: 0x06007D73 RID: 32115 RVA: 0x0031DE37 File Offset: 0x0031C037
		public override bool Fail()
		{
			return SaveGame.Instance.ColonyAchievementTracker.largeImpactorState == ColonyAchievementTracker.LargeImpactorState.Landed;
		}

		// Token: 0x06007D74 RID: 32116 RVA: 0x0031DE4B File Offset: 0x0031C04B
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.REQUIREMENT_NAME;
		}
	}
}
