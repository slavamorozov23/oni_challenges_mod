using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F81 RID: 3969
	public class SkillBranchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D8B RID: 32139 RVA: 0x0031E32C File Offset: 0x0031C52C
		public SkillBranchComplete(List<Skill> skillsToMaster)
		{
			this.skillsToMaster = skillsToMaster;
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x0031E33C File Offset: 0x0031C53C
		public override bool Success()
		{
			foreach (MinionResume minionResume in Components.MinionResumes.Items)
			{
				foreach (Skill skill in this.skillsToMaster)
				{
					if (minionResume.HasMasteredSkill(skill.Id))
					{
						if (!minionResume.HasBeenGrantedSkill(skill))
						{
							return true;
						}
						List<Skill> allPriorSkills = Db.Get().Skills.GetAllPriorSkills(skill);
						bool flag = true;
						foreach (Skill skill2 in allPriorSkills)
						{
							flag = (flag && minionResume.HasMasteredSkill(skill2.Id));
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x0031E45C File Offset: 0x0031C65C
		public void Deserialize(IReader reader)
		{
			this.skillsToMaster = new List<Skill>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string id = reader.ReadKleiString();
				this.skillsToMaster.Add(Db.Get().Skills.Get(id));
			}
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x0031E4A9 File Offset: 0x0031C6A9
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SKILL_BRANCH;
		}

		// Token: 0x04005C35 RID: 23605
		private List<Skill> skillsToMaster;
	}
}
