using System;
using UnityEngine;

// Token: 0x02000D2E RID: 3374
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenLineItem")]
public class InfoScreenLineItem : KMonoBehaviour
{
	// Token: 0x06006841 RID: 26689 RVA: 0x00275239 File Offset: 0x00273439
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x06006842 RID: 26690 RVA: 0x00275247 File Offset: 0x00273447
	public void SetTooltip(string tooltip)
	{
		this.toolTip.toolTip = tooltip;
	}

	// Token: 0x04004799 RID: 18329
	[SerializeField]
	private LocText locText;

	// Token: 0x0400479A RID: 18330
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x0400479B RID: 18331
	private string text;

	// Token: 0x0400479C RID: 18332
	private string tooltip;
}
