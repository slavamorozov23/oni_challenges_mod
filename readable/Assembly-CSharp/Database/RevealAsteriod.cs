using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F96 RID: 3990
	public class RevealAsteriod : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DE3 RID: 32227 RVA: 0x0031FD0D File Offset: 0x0031DF0D
		public RevealAsteriod(float percentToReveal)
		{
			this.percentToReveal = percentToReveal;
		}

		// Token: 0x06007DE4 RID: 32228 RVA: 0x0031FD1C File Offset: 0x0031DF1C
		public override bool Success()
		{
			this.amountRevealed = 0f;
			float num = 0f;
			WorldContainer startWorld = ClusterManager.Instance.GetStartWorld();
			Vector2 minimumBounds = startWorld.minimumBounds;
			Vector2 maximumBounds = startWorld.maximumBounds;
			int num2 = (int)minimumBounds.x;
			while ((float)num2 <= maximumBounds.x)
			{
				int num3 = (int)minimumBounds.y;
				while ((float)num3 <= maximumBounds.y)
				{
					if (Grid.Visible[Grid.PosToCell(new Vector2((float)num2, (float)num3))] > 0)
					{
						num += 1f;
					}
					num3++;
				}
				num2++;
			}
			this.amountRevealed = num / (float)(startWorld.Width * startWorld.Height);
			return this.amountRevealed > this.percentToReveal;
		}

		// Token: 0x06007DE5 RID: 32229 RVA: 0x0031FDD0 File Offset: 0x0031DFD0
		public void Deserialize(IReader reader)
		{
			this.percentToReveal = reader.ReadSingle();
		}

		// Token: 0x06007DE6 RID: 32230 RVA: 0x0031FDDE File Offset: 0x0031DFDE
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REVEALED, this.amountRevealed * 100f, this.percentToReveal * 100f);
		}

		// Token: 0x04005C51 RID: 23633
		private float percentToReveal;

		// Token: 0x04005C52 RID: 23634
		private float amountRevealed;
	}
}
