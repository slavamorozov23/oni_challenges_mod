using System;

namespace Database
{
	// Token: 0x02000F4B RID: 3915
	public abstract class PermitResource : Resource, IHasDlcRestrictions
	{
		// Token: 0x06007C9E RID: 31902 RVA: 0x00314E50 File Offset: 0x00313050
		public PermitResource(string id, string Name, string Desc, PermitCategory permitCategory, PermitRarity rarity, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, Name)
		{
			DebugUtil.DevAssert(Name != null, "Name must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			DebugUtil.DevAssert(Desc != null, "Description must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			this.Description = Desc;
			this.Category = permitCategory;
			this.Rarity = rarity;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x06007C9F RID: 31903
		public abstract PermitPresentationInfo GetPermitPresentationInfo();

		// Token: 0x06007CA0 RID: 31904 RVA: 0x00314ED6 File Offset: 0x003130D6
		public bool IsOwnableOnServer()
		{
			return this.Rarity != PermitRarity.Universal && this.Rarity != PermitRarity.UniversalLocked;
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x00314EEF File Offset: 0x003130EF
		public bool IsUnlocked()
		{
			return this.Rarity == PermitRarity.Universal || PermitItems.IsPermitUnlocked(this);
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x00314F02 File Offset: 0x00313102
		public string GetDlcIdFrom()
		{
			return DlcManager.GetMostSignificantDlc(this);
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x00314F0A File Offset: 0x0031310A
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x00314F12 File Offset: 0x00313112
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x04005B09 RID: 23305
		public string Description;

		// Token: 0x04005B0A RID: 23306
		public PermitCategory Category;

		// Token: 0x04005B0B RID: 23307
		public PermitRarity Rarity;

		// Token: 0x04005B0C RID: 23308
		public string[] requiredDlcIds;

		// Token: 0x04005B0D RID: 23309
		public string[] forbiddenDlcIds;
	}
}
