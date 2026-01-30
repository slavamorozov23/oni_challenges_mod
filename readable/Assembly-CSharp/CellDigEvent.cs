using System;
using System.Diagnostics;

// Token: 0x0200093B RID: 2363
public class CellDigEvent : CellEvent
{
	// Token: 0x06004212 RID: 16914 RVA: 0x00174ADF File Offset: 0x00172CDF
	public CellDigEvent(bool enable_logging = true) : base("Dig", "Dig", true, enable_logging)
	{
	}

	// Token: 0x06004213 RID: 16915 RVA: 0x00174AF4 File Offset: 0x00172CF4
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06004214 RID: 16916 RVA: 0x00174B20 File Offset: 0x00172D20
	public override string GetDescription(EventInstanceBase ev)
	{
		return base.GetMessagePrefix() + "Dig=true";
	}
}
