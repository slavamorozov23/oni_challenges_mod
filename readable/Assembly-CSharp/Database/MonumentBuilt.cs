using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x02000F6B RID: 3947
	public class MonumentBuilt : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D19 RID: 32025 RVA: 0x0031D402 File Offset: 0x0031B602
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x0031D40E File Offset: 0x0031B60E
		public override string Description()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;
		}

		// Token: 0x06007D1B RID: 32027 RVA: 0x0031D41C File Offset: 0x0031B61C
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.MonumentParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((MonumentPart)enumerator.Current).IsMonumentCompleted())
					{
						Game.Instance.unlocks.Unlock("thriving", true);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007D1C RID: 32028 RVA: 0x0031D490 File Offset: 0x0031B690
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x0031D492 File Offset: 0x0031B692
		public override string GetProgress(bool complete)
		{
			return this.Name();
		}
	}
}
