using System;
using KSerialization;

// Token: 0x02000944 RID: 2372
[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
	// Token: 0x0600422B RID: 16939 RVA: 0x00175648 File Offset: 0x00173848
	public EventInstanceBase(EventBase ev)
	{
		this.frame = GameClock.Instance.GetFrame();
		this.eventHash = ev.hash;
		this.ev = ev;
	}

	// Token: 0x0600422C RID: 16940 RVA: 0x00175674 File Offset: 0x00173874
	public override string ToString()
	{
		string str = "[" + this.frame.ToString() + "] ";
		if (this.ev != null)
		{
			return str + this.ev.GetDescription(this);
		}
		return str + "Unknown event";
	}

	// Token: 0x04002986 RID: 10630
	[Serialize]
	public int frame;

	// Token: 0x04002987 RID: 10631
	[Serialize]
	public int eventHash;

	// Token: 0x04002988 RID: 10632
	public EventBase ev;
}
