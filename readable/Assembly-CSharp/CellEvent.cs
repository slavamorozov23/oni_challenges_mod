using System;

// Token: 0x0200093D RID: 2365
public class CellEvent : EventBase
{
	// Token: 0x06004218 RID: 16920 RVA: 0x00174BCA File Offset: 0x00172DCA
	public CellEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id)
	{
		this.reason = reason;
		this.isSend = is_send;
		this.enableLogging = enable_logging;
	}

	// Token: 0x06004219 RID: 16921 RVA: 0x00174BE9 File Offset: 0x00172DE9
	public string GetMessagePrefix()
	{
		if (this.isSend)
		{
			return ">>>: ";
		}
		return "<<<: ";
	}

	// Token: 0x0400293F RID: 10559
	public string reason;

	// Token: 0x04002940 RID: 10560
	public bool isSend;

	// Token: 0x04002941 RID: 10561
	public bool enableLogging;
}
