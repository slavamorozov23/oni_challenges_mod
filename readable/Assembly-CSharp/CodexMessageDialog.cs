using System;
using UnityEngine;

// Token: 0x02000D8C RID: 3468
public class CodexMessageDialog : MessageDialog
{
	// Token: 0x06006C02 RID: 27650 RVA: 0x00290B32 File Offset: 0x0028ED32
	public override bool CanDisplay(Message message)
	{
		return typeof(CodexUnlockedMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06006C03 RID: 27651 RVA: 0x00290B49 File Offset: 0x0028ED49
	public override void SetMessage(Message base_message)
	{
		this.message = (CodexUnlockedMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x00290B6D File Offset: 0x0028ED6D
	public override void OnClickAction()
	{
	}

	// Token: 0x06006C05 RID: 27653 RVA: 0x00290B6F File Offset: 0x0028ED6F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04004A1E RID: 18974
	[SerializeField]
	private LocText description;

	// Token: 0x04004A1F RID: 18975
	private CodexUnlockedMessage message;
}
