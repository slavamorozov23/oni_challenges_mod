using System;

namespace Database
{
	// Token: 0x02000FAE RID: 4014
	public class SimpleSkillPerk : SkillPerk
	{
		// Token: 0x06007E39 RID: 32313 RVA: 0x00322A8D File Offset: 0x00320C8D
		public SimpleSkillPerk(string id, string description) : base(id, description, null, null, null, false)
		{
		}

		// Token: 0x06007E3A RID: 32314 RVA: 0x00322A9B File Offset: 0x00320C9B
		public SimpleSkillPerk(string id, string description, string[] requiredDlcIds) : base(id, description, null, null, null, requiredDlcIds, false)
		{
		}
	}
}
