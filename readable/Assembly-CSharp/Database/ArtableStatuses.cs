using System;

namespace Database
{
	// Token: 0x02000F1B RID: 3867
	public class ArtableStatuses : ResourceSet<ArtableStatusItem>
	{
		// Token: 0x06007C03 RID: 31747 RVA: 0x00302544 File Offset: 0x00300744
		public ArtableStatuses(ResourceSet parent) : base("ArtableStatuses", parent)
		{
			this.AwaitingArting = this.Add("AwaitingArting", ArtableStatuses.ArtableStatusType.AwaitingArting);
			this.LookingUgly = this.Add("LookingUgly", ArtableStatuses.ArtableStatusType.LookingUgly);
			this.LookingOkay = this.Add("LookingOkay", ArtableStatuses.ArtableStatusType.LookingOkay);
			this.LookingGreat = this.Add("LookingGreat", ArtableStatuses.ArtableStatusType.LookingGreat);
		}

		// Token: 0x06007C04 RID: 31748 RVA: 0x003025A8 File Offset: 0x003007A8
		public ArtableStatusItem Add(string id, ArtableStatuses.ArtableStatusType statusType)
		{
			ArtableStatusItem artableStatusItem = new ArtableStatusItem(id, statusType);
			this.resources.Add(artableStatusItem);
			return artableStatusItem;
		}

		// Token: 0x04005687 RID: 22151
		public ArtableStatusItem AwaitingArting;

		// Token: 0x04005688 RID: 22152
		public ArtableStatusItem LookingUgly;

		// Token: 0x04005689 RID: 22153
		public ArtableStatusItem LookingOkay;

		// Token: 0x0400568A RID: 22154
		public ArtableStatusItem LookingGreat;

		// Token: 0x0200218E RID: 8590
		public enum ArtableStatusType
		{
			// Token: 0x04009997 RID: 39319
			AwaitingArting,
			// Token: 0x04009998 RID: 39320
			LookingUgly,
			// Token: 0x04009999 RID: 39321
			LookingOkay,
			// Token: 0x0400999A RID: 39322
			LookingGreat
		}
	}
}
