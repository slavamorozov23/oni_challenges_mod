using System;
using UnityEngine;

// Token: 0x02000DA0 RID: 3488
public class TargetMessageDialog : MessageDialog
{
	// Token: 0x06006C98 RID: 27800 RVA: 0x00291B9F File Offset: 0x0028FD9F
	public override bool CanDisplay(Message message)
	{
		return typeof(TargetMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06006C99 RID: 27801 RVA: 0x00291BB6 File Offset: 0x0028FDB6
	public override void SetMessage(Message base_message)
	{
		this.message = (TargetMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06006C9A RID: 27802 RVA: 0x00291BDC File Offset: 0x0028FDDC
	public override void OnClickAction()
	{
		MessageTarget target = this.message.GetTarget();
		SelectTool.Instance.SelectAndFocus(target.GetPosition(), target.GetSelectable());
	}

	// Token: 0x06006C9B RID: 27803 RVA: 0x00291C0B File Offset: 0x0028FE0B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04004A4C RID: 19020
	[SerializeField]
	private LocText description;

	// Token: 0x04004A4D RID: 19021
	private TargetMessage message;
}
