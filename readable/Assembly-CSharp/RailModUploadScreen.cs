using System;
using KSerialization;
using TMPro;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class RailModUploadScreen : KModalScreen
{
	// Token: 0x04000012 RID: 18
	[SerializeField]
	private KButton[] closeButtons;

	// Token: 0x04000013 RID: 19
	[SerializeField]
	private KButton submitButton;

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private ToolTip submitButtonTooltip;

	// Token: 0x04000015 RID: 21
	[SerializeField]
	private TMP_InputField modName;

	// Token: 0x04000016 RID: 22
	[SerializeField]
	private TMP_InputField modDesc;

	// Token: 0x04000017 RID: 23
	[SerializeField]
	private TMP_InputField modVersion;

	// Token: 0x04000018 RID: 24
	[SerializeField]
	private TMP_InputField contentFolder;

	// Token: 0x04000019 RID: 25
	[SerializeField]
	private TMP_InputField previewImage;

	// Token: 0x0400001A RID: 26
	[SerializeField]
	private MultiToggle[] shareTypeToggles;

	// Token: 0x0400001B RID: 27
	[Serialize]
	private string previousFolderPath;
}
