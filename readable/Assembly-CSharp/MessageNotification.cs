using System;
using System.Collections.Generic;

// Token: 0x02000603 RID: 1539
public class MessageNotification : Notification
{
	// Token: 0x060023D3 RID: 9171 RVA: 0x000CF371 File Offset: 0x000CD571
	private string OnToolTip(List<Notification> notifications, string tooltipText)
	{
		return tooltipText;
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000CF374 File Offset: 0x000CD574
	public MessageNotification(Message m) : base(m.GetTitle(), NotificationType.Messages, null, null, false, 0f, null, null, null, true, false, true)
	{
		MessageNotification <>4__this = this;
		this.message = m;
		base.Type = m.GetMessageType();
		this.showDismissButton = m.ShowDismissButton();
		if (!this.message.PlayNotificationSound())
		{
			this.playSound = false;
		}
		base.ToolTip = ((List<Notification> notifications, object data) => <>4__this.OnToolTip(notifications, m.GetTooltip()));
		base.clickFocus = null;
	}

	// Token: 0x040014D6 RID: 5334
	public Message message;
}
