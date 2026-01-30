using System;
using KSerialization;

// Token: 0x02000DA1 RID: 3489
public class TutorialMessage : GenericMessage, IHasDlcRestrictions
{
	// Token: 0x06006C9D RID: 27805 RVA: 0x00291C26 File Offset: 0x0028FE26
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06006C9E RID: 27806 RVA: 0x00291C2E File Offset: 0x0028FE2E
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x06006C9F RID: 27807 RVA: 0x00291C36 File Offset: 0x0028FE36
	public TutorialMessage()
	{
	}

	// Token: 0x06006CA0 RID: 27808 RVA: 0x00291C40 File Offset: 0x0028FE40
	public TutorialMessage(Tutorial.TutorialMessages messageId, string title, string body, string tooltip, string videoClipId = null, string videoOverlayName = null, string videoTitleText = null, string icon = "", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(title, body, tooltip, null)
	{
		this.messageId = messageId;
		this.videoClipId = videoClipId;
		this.videoOverlayName = videoOverlayName;
		this.videoTitleText = videoTitleText;
		this.icon = icon;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x04004A4E RID: 19022
	[Serialize]
	public Tutorial.TutorialMessages messageId;

	// Token: 0x04004A4F RID: 19023
	public string videoClipId;

	// Token: 0x04004A50 RID: 19024
	public string videoOverlayName;

	// Token: 0x04004A51 RID: 19025
	public string videoTitleText;

	// Token: 0x04004A52 RID: 19026
	public string icon;

	// Token: 0x04004A53 RID: 19027
	public string[] requiredDlcIds;

	// Token: 0x04004A54 RID: 19028
	public string[] forbiddenDlcIds;
}
