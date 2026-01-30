using System;

namespace Database
{
	// Token: 0x02000F1C RID: 3868
	public class ArtableStatusItem : StatusItem
	{
		// Token: 0x06007C05 RID: 31749 RVA: 0x003025CC File Offset: 0x003007CC
		public ArtableStatusItem(string id, ArtableStatuses.ArtableStatusType statusType) : base(id, "BUILDING", "", StatusItem.IconType.Info, (statusType == ArtableStatuses.ArtableStatusType.AwaitingArting) ? NotificationType.BadMinor : NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null)
		{
			this.StatusType = statusType;
		}

		// Token: 0x0400568B RID: 22155
		public ArtableStatuses.ArtableStatusType StatusType;
	}
}
