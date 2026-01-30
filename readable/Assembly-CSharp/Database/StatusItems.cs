using System;
using System.Diagnostics;

namespace Database
{
	// Token: 0x02000F5E RID: 3934
	public class StatusItems : ResourceSet<StatusItem>
	{
		// Token: 0x06007CE5 RID: 31973 RVA: 0x00319481 File Offset: 0x00317681
		public StatusItems(string id, ResourceSet parent) : base(id, parent)
		{
		}

		// Token: 0x020021AD RID: 8621
		[DebuggerDisplay("{Id}")]
		public class StatusItemInfo : Resource
		{
			// Token: 0x04009B1A RID: 39706
			public string Type;

			// Token: 0x04009B1B RID: 39707
			public string Tooltip;

			// Token: 0x04009B1C RID: 39708
			public bool IsIconTinted;

			// Token: 0x04009B1D RID: 39709
			public StatusItem.IconType IconType;

			// Token: 0x04009B1E RID: 39710
			public string Icon;

			// Token: 0x04009B1F RID: 39711
			public string SoundPath;

			// Token: 0x04009B20 RID: 39712
			public bool ShouldNotify;

			// Token: 0x04009B21 RID: 39713
			public float NotificationDelay;

			// Token: 0x04009B22 RID: 39714
			public NotificationType NotificationType;

			// Token: 0x04009B23 RID: 39715
			public bool AllowMultiples;

			// Token: 0x04009B24 RID: 39716
			public string Effect;

			// Token: 0x04009B25 RID: 39717
			public HashedString Overlay;

			// Token: 0x04009B26 RID: 39718
			public HashedString SecondOverlay;
		}
	}
}
