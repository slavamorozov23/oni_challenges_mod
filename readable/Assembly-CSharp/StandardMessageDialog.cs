using System;
using UnityEngine;

// Token: 0x02000D9E RID: 3486
public class StandardMessageDialog : MessageDialog
{
	// Token: 0x06006C90 RID: 27792 RVA: 0x00291B2E File Offset: 0x0028FD2E
	public override bool CanDisplay(Message message)
	{
		return typeof(Message).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06006C91 RID: 27793 RVA: 0x00291B45 File Offset: 0x0028FD45
	public override void SetMessage(Message base_message)
	{
		this.message = base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06006C92 RID: 27794 RVA: 0x00291B64 File Offset: 0x0028FD64
	public override void OnClickAction()
	{
	}

	// Token: 0x04004A49 RID: 19017
	[SerializeField]
	private LocText description;

	// Token: 0x04004A4A RID: 19018
	private Message message;
}
