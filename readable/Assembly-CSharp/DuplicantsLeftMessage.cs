using System;
using STRINGS;

// Token: 0x02000D90 RID: 3472
public class DuplicantsLeftMessage : Message
{
	// Token: 0x06006C1F RID: 27679 RVA: 0x00290CEF File Offset: 0x0028EEEF
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06006C20 RID: 27680 RVA: 0x00290CF6 File Offset: 0x0028EEF6
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;
	}

	// Token: 0x06006C21 RID: 27681 RVA: 0x00290D02 File Offset: 0x0028EF02
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;
	}

	// Token: 0x06006C22 RID: 27682 RVA: 0x00290D0E File Offset: 0x0028EF0E
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
	}
}
