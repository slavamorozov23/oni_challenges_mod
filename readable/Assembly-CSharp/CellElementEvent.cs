using System;
using System.Diagnostics;

// Token: 0x0200093C RID: 2364
public class CellElementEvent : CellEvent
{
	// Token: 0x06004215 RID: 16917 RVA: 0x00174B32 File Offset: 0x00172D32
	public CellElementEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x06004216 RID: 16918 RVA: 0x00174B40 File Offset: 0x00172D40
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06004217 RID: 16919 RVA: 0x00174B6C File Offset: 0x00172D6C
	public override string GetDescription(EventInstanceBase ev)
	{
		SimHashes data = (SimHashes)(ev as CellEventInstance).data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
