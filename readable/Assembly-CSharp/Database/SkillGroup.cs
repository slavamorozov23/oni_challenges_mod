using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000FB0 RID: 4016
	public class SkillGroup : Resource, IListableOption
	{
		// Token: 0x06007E3C RID: 32316 RVA: 0x00323D0C File Offset: 0x00321F0C
		string IListableOption.GetProperName()
		{
			return Strings.Get("STRINGS.DUPLICANTS.SKILLGROUPS." + this.Id.ToUpper() + ".NAME");
		}

		// Token: 0x06007E3D RID: 32317 RVA: 0x00323D32 File Offset: 0x00321F32
		public SkillGroup(string id, string choreGroupID, string name, string icon, string archetype_icon) : base(id, name)
		{
			this.choreGroupID = choreGroupID;
			this.choreGroupIcon = icon;
			this.archetypeIcon = archetype_icon;
		}

		// Token: 0x04005D12 RID: 23826
		public string choreGroupID;

		// Token: 0x04005D13 RID: 23827
		public List<Klei.AI.Attribute> relevantAttributes;

		// Token: 0x04005D14 RID: 23828
		public List<string> requiredChoreGroups;

		// Token: 0x04005D15 RID: 23829
		public string choreGroupIcon;

		// Token: 0x04005D16 RID: 23830
		public string archetypeIcon;

		// Token: 0x04005D17 RID: 23831
		public bool allowAsAptitude = true;
	}
}
