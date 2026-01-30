using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F79 RID: 3961
	public class EstablishColonies : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D65 RID: 32101 RVA: 0x0031DC50 File Offset: 0x0031BE50
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ESTABLISH_COLONIES.Replace("{goalBaseCount}", EstablishColonies.BASE_COUNT.ToString()).Replace("{baseCount}", this.GetColonyCount().ToString()).Replace("{neededCount}", EstablishColonies.BASE_COUNT.ToString());
		}

		// Token: 0x06007D66 RID: 32102 RVA: 0x0031DCA7 File Offset: 0x0031BEA7
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x0031DCB5 File Offset: 0x0031BEB5
		public override bool Success()
		{
			return this.GetColonyCount() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x0031DCC7 File Offset: 0x0031BEC7
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.SEVERAL_COLONIES;
		}

		// Token: 0x06007D69 RID: 32105 RVA: 0x0031DCD4 File Offset: 0x0031BED4
		private int GetColonyCount()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04005C2F RID: 23599
		public static int BASE_COUNT = 5;
	}
}
