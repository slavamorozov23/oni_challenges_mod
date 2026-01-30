using System;
using System.Diagnostics;

// Token: 0x02000942 RID: 2370
public class CellSolidFilterEvent : CellEvent
{
	// Token: 0x06004226 RID: 16934 RVA: 0x001755B6 File Offset: 0x001737B6
	public CellSolidFilterEvent(string id, bool enable_logging = true) : base(id, "filtered", false, enable_logging)
	{
	}

	// Token: 0x06004227 RID: 16935 RVA: 0x001755C8 File Offset: 0x001737C8
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

	// Token: 0x06004228 RID: 16936 RVA: 0x001755FC File Offset: 0x001737FC
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Filtered Solid Event solid=" + cellEventInstance.data.ToString();
	}
}
