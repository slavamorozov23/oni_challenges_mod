using System;

namespace Database
{
	// Token: 0x02000FAB RID: 4011
	public class SkillPerk : Resource, IHasDlcRestrictions
	{
		// Token: 0x06007E2A RID: 32298 RVA: 0x00322842 File Offset: 0x00320A42
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x0032284A File Offset: 0x00320A4A
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06007E2C RID: 32300 RVA: 0x0032284D File Offset: 0x00320A4D
		// (set) Token: 0x06007E2D RID: 32301 RVA: 0x00322855 File Offset: 0x00320A55
		public Action<MinionResume> OnApply { get; protected set; }

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06007E2E RID: 32302 RVA: 0x0032285E File Offset: 0x00320A5E
		// (set) Token: 0x06007E2F RID: 32303 RVA: 0x00322866 File Offset: 0x00320A66
		public Action<MinionResume> OnRemove { get; protected set; }

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06007E30 RID: 32304 RVA: 0x0032286F File Offset: 0x00320A6F
		// (set) Token: 0x06007E31 RID: 32305 RVA: 0x00322877 File Offset: 0x00320A77
		public Action<MinionResume> OnMinionsChanged { get; protected set; }

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06007E32 RID: 32306 RVA: 0x00322880 File Offset: 0x00320A80
		// (set) Token: 0x06007E33 RID: 32307 RVA: 0x00322888 File Offset: 0x00320A88
		public bool affectAll { get; protected set; }

		// Token: 0x06007E34 RID: 32308 RVA: 0x00322894 File Offset: 0x00320A94
		public static string GetDescription(string perkID)
		{
			string text = GameUtil.NamesOfBuildingsRequiringSkillPerk(perkID);
			if (text == null)
			{
				return Db.Get().SkillPerks.Get(perkID).Name;
			}
			return text;
		}

		// Token: 0x06007E35 RID: 32309 RVA: 0x003228C2 File Offset: 0x00320AC2
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
		}

		// Token: 0x06007E36 RID: 32310 RVA: 0x003228EB File Offset: 0x00320AEB
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, string[] requiredDlcIds = null, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
			this.requiredDlcIds = requiredDlcIds;
		}

		// Token: 0x04005CA2 RID: 23714
		public string[] requiredDlcIds;
	}
}
