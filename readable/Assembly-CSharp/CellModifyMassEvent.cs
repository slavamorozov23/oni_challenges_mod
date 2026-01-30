using System;
using System.Diagnostics;

// Token: 0x02000940 RID: 2368
public class CellModifyMassEvent : CellEvent
{
	// Token: 0x06004220 RID: 16928 RVA: 0x0017545F File Offset: 0x0017365F
	public CellModifyMassEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x0017546C File Offset: 0x0017366C
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06004222 RID: 16930 RVA: 0x001754A0 File Offset: 0x001736A0
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
