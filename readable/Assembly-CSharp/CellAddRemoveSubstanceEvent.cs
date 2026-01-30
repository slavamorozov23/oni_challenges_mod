using System;
using System.Diagnostics;

// Token: 0x02000939 RID: 2361
public class CellAddRemoveSubstanceEvent : CellEvent
{
	// Token: 0x0600420C RID: 16908 RVA: 0x001749B1 File Offset: 0x00172BB1
	public CellAddRemoveSubstanceEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x0600420D RID: 16909 RVA: 0x001749C0 File Offset: 0x00172BC0
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x0600420E RID: 16910 RVA: 0x001749F4 File Offset: 0x00172BF4
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		SimHashes data = (SimHashes)cellEventInstance.data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			", Mass=",
			((float)cellEventInstance.data2 / 1000f).ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
