using System;

// Token: 0x02000534 RID: 1332
public abstract class StateEvent
{
	// Token: 0x06001CAF RID: 7343 RVA: 0x0009D239 File Offset: 0x0009B439
	public StateEvent(string name)
	{
		this.name = name;
		this.debugName = "(Event)" + name;
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x0009D259 File Offset: 0x0009B459
	public virtual StateEvent.Context Subscribe(StateMachine.Instance smi)
	{
		return new StateEvent.Context(this);
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x0009D261 File Offset: 0x0009B461
	public virtual void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
	{
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x0009D263 File Offset: 0x0009B463
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x0009D26B File Offset: 0x0009B46B
	public string GetDebugName()
	{
		return this.debugName;
	}

	// Token: 0x040010EF RID: 4335
	protected string name;

	// Token: 0x040010F0 RID: 4336
	private string debugName;

	// Token: 0x020013BB RID: 5051
	public struct Context
	{
		// Token: 0x06008D8E RID: 36238 RVA: 0x00366566 File Offset: 0x00364766
		public Context(StateEvent state_event)
		{
			this.stateEvent = state_event;
			this.data = 0;
		}

		// Token: 0x04006C3A RID: 27706
		public StateEvent stateEvent;

		// Token: 0x04006C3B RID: 27707
		public int data;
	}
}
