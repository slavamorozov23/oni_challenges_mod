using System;

// Token: 0x020004DF RID: 1247
public abstract class GameplayEvent<StateMachineInstanceType> : GameplayEvent where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x06001AEC RID: 6892 RVA: 0x0009407B File Offset: 0x0009227B
	public GameplayEvent(string id, int priority = 0, int importance = 0, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, priority, importance, requiredDlcIds, forbiddenDlcIds)
	{
	}
}
