using System;
using System.Diagnostics;

// Token: 0x0200093A RID: 2362
public class CellCallbackEvent : CellEvent
{
	// Token: 0x0600420F RID: 16911 RVA: 0x00174A74 File Offset: 0x00172C74
	public CellCallbackEvent(string id, bool is_send, bool enable_logging = true) : base(id, "Callback", is_send, enable_logging)
	{
	}

	// Token: 0x06004210 RID: 16912 RVA: 0x00174A84 File Offset: 0x00172C84
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, callback_id, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06004211 RID: 16913 RVA: 0x00174AB0 File Offset: 0x00172CB0
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Callback=" + cellEventInstance.data.ToString();
	}
}
