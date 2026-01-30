using System;

// Token: 0x020004DA RID: 1242
public abstract class Usable : KMonoBehaviour, IStateMachineTarget
{
	// Token: 0x06001ABF RID: 6847
	public abstract void StartUsing(User user);

	// Token: 0x06001AC0 RID: 6848 RVA: 0x00093810 File Offset: 0x00091A10
	protected void StartUsing(StateMachine.Instance smi, User user)
	{
		DebugUtil.Assert(this.smi == null);
		DebugUtil.Assert(smi != null);
		this.smi = smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
		smi.StartSM();
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x00093864 File Offset: 0x00091A64
	public void StopUsing(User user)
	{
		if (this.smi != null)
		{
			StateMachine.Instance instance = this.smi;
			instance.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(instance.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
			this.smi.StopSM("Usable.StopUsing");
			this.smi = null;
		}
	}

	// Token: 0x04000F64 RID: 3940
	private StateMachine.Instance smi;
}
