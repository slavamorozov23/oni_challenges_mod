using System;

// Token: 0x02000537 RID: 1335
public static class StateMachineExtensions
{
	// Token: 0x06001CCE RID: 7374 RVA: 0x0009D6E8 File Offset: 0x0009B8E8
	public static bool IsNullOrStopped(this StateMachine.Instance smi)
	{
		return smi == null || !smi.IsRunning();
	}
}
