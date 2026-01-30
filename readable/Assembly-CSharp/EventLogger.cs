using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000945 RID: 2373
[SerializationConfig(MemberSerialization.OptIn)]
public class EventLogger<EventInstanceType, EventType> : KMonoBehaviour, ISaveLoadable where EventInstanceType : EventInstanceBase where EventType : EventBase
{
	// Token: 0x0600422D RID: 16941 RVA: 0x001756C2 File Offset: 0x001738C2
	public IEnumerator<EventInstanceType> GetEnumerator()
	{
		return this.EventInstances.GetEnumerator();
	}

	// Token: 0x0600422E RID: 16942 RVA: 0x001756D4 File Offset: 0x001738D4
	public EventType AddEvent(EventType ev)
	{
		for (int i = 0; i < this.Events.Count; i++)
		{
			if (this.Events[i].hash == ev.hash)
			{
				this.Events[i] = ev;
				return this.Events[i];
			}
		}
		this.Events.Add(ev);
		return ev;
	}

	// Token: 0x0600422F RID: 16943 RVA: 0x00175741 File Offset: 0x00173941
	public EventInstanceType Add(EventInstanceType ev)
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveAt(0);
		}
		this.EventInstances.Add(ev);
		return ev;
	}

	// Token: 0x06004230 RID: 16944 RVA: 0x00175770 File Offset: 0x00173970
	[OnDeserialized]
	protected internal void OnDeserialized()
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveRange(0, this.EventInstances.Count - 10000);
		}
		for (int i = 0; i < this.EventInstances.Count; i++)
		{
			for (int j = 0; j < this.Events.Count; j++)
			{
				if (this.Events[j].hash == this.EventInstances[i].eventHash)
				{
					this.EventInstances[i].ev = this.Events[j];
					break;
				}
			}
		}
	}

	// Token: 0x06004231 RID: 16945 RVA: 0x0017582F File Offset: 0x00173A2F
	public void Clear()
	{
		this.EventInstances.Clear();
	}

	// Token: 0x04002989 RID: 10633
	private const int MAX_NUM_EVENTS = 10000;

	// Token: 0x0400298A RID: 10634
	private List<EventType> Events = new List<EventType>();

	// Token: 0x0400298B RID: 10635
	[Serialize]
	private List<EventInstanceType> EventInstances = new List<EventInstanceType>();
}
