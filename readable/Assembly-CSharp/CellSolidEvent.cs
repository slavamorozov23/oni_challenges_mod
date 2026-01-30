using System;
using System.Diagnostics;

// Token: 0x02000941 RID: 2369
public class CellSolidEvent : CellEvent
{
	// Token: 0x06004223 RID: 16931 RVA: 0x00175520 File Offset: 0x00173720
	public CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x00175530 File Offset: 0x00173730
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, bool solid)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, solid ? 1 : 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06004225 RID: 16933 RVA: 0x00175564 File Offset: 0x00173764
	public override string GetDescription(EventInstanceBase ev)
	{
		if ((ev as CellEventInstance).data == 1)
		{
			return base.GetMessagePrefix() + "Solid=true (" + this.reason + ")";
		}
		return base.GetMessagePrefix() + "Solid=false (" + this.reason + ")";
	}
}
