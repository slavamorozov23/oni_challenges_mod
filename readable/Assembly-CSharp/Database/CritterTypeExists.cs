using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F93 RID: 3987
	public class CritterTypeExists : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DD7 RID: 32215 RVA: 0x0031FA70 File Offset: 0x0031DC70
		public CritterTypeExists(List<Tag> critterTypes)
		{
			this.critterTypes = critterTypes;
		}

		// Token: 0x06007DD8 RID: 32216 RVA: 0x0031FA8C File Offset: 0x0031DC8C
		public override bool Success()
		{
			foreach (Capturable cmp in Components.Capturables.Items)
			{
				if (this.critterTypes.Contains(cmp.PrefabID()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x0031FAF8 File Offset: 0x0031DCF8
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.critterTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.critterTypes.Add(new Tag(name));
			}
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x0031FB3C File Offset: 0x0031DD3C
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
		}

		// Token: 0x04005C4E RID: 23630
		private List<Tag> critterTypes = new List<Tag>();
	}
}
