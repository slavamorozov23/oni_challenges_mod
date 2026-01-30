using System;
using System.Collections.Generic;
using TUNING;

namespace Database
{
	// Token: 0x02000FB2 RID: 4018
	public class Skill : Resource, IHasDlcRestrictions
	{
		// Token: 0x06007E3F RID: 32319 RVA: 0x0032450C File Offset: 0x0032270C
		public Skill(string id, string name, string description, int tier, string hat, string badge, string skillGroup, List<SkillPerk> perks = null, List<string> priorSkills = null, string requiredDuplicantModel = "Minion", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, name)
		{
			this.description = description;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.tier = tier;
			this.hat = hat;
			this.badge = badge;
			this.skillGroup = skillGroup;
			this.perks = perks;
			if (this.perks == null)
			{
				this.perks = new List<SkillPerk>();
			}
			this.priorSkills = priorSkills;
			if (this.priorSkills == null)
			{
				this.priorSkills = new List<string>();
			}
			this.requiredDuplicantModel = requiredDuplicantModel;
		}

		// Token: 0x06007E40 RID: 32320 RVA: 0x00324598 File Offset: 0x00322798
		[Obsolete]
		public Skill(string id, string name, string description, string dlcId, int tier, string hat, string badge, string skillGroup, List<SkillPerk> perks = null, List<string> priorSkills = null, string requiredDuplicantModel = "Minion") : this(id, name, description, tier, hat, badge, skillGroup, perks, priorSkills, requiredDuplicantModel, null, null)
		{
		}

		// Token: 0x06007E41 RID: 32321 RVA: 0x003245BE File Offset: 0x003227BE
		public int GetMoraleExpectation()
		{
			return SKILLS.SKILL_TIER_MORALE_COST[this.tier];
		}

		// Token: 0x06007E42 RID: 32322 RVA: 0x003245CC File Offset: 0x003227CC
		public bool GivesPerk(SkillPerk perk)
		{
			return this.perks.Contains(perk);
		}

		// Token: 0x06007E43 RID: 32323 RVA: 0x003245DC File Offset: 0x003227DC
		public bool GivesPerk(HashedString perkId)
		{
			using (List<SkillPerk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IdHash == perkId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007E44 RID: 32324 RVA: 0x0032463C File Offset: 0x0032283C
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06007E45 RID: 32325 RVA: 0x00324644 File Offset: 0x00322844
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x04005D26 RID: 23846
		public string description;

		// Token: 0x04005D27 RID: 23847
		public string[] requiredDlcIds;

		// Token: 0x04005D28 RID: 23848
		public string[] forbiddenDlcIds;

		// Token: 0x04005D29 RID: 23849
		public string skillGroup;

		// Token: 0x04005D2A RID: 23850
		public string hat;

		// Token: 0x04005D2B RID: 23851
		public string badge;

		// Token: 0x04005D2C RID: 23852
		public int tier;

		// Token: 0x04005D2D RID: 23853
		public bool deprecated;

		// Token: 0x04005D2E RID: 23854
		public List<SkillPerk> perks;

		// Token: 0x04005D2F RID: 23855
		public List<string> priorSkills;

		// Token: 0x04005D30 RID: 23856
		public string requiredDuplicantModel;
	}
}
