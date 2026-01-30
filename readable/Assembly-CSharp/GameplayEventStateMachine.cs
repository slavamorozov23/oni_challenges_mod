using System;
using UnityEngine;

// Token: 0x02000530 RID: 1328
public abstract class GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType> where StateMachineType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> where StateMachineInstanceType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType>.GameplayEventStateMachineInstance where MasterType : IStateMachineTarget where SecondMasterType : GameplayEvent<StateMachineInstanceType>
{
	// Token: 0x06001CA4 RID: 7332 RVA: 0x0009D098 File Offset: 0x0009B298
	public void MonitorStart(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1660384580, smi.eventInstance);
		}
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x0009D0CC File Offset: 0x0009B2CC
	public void MonitorChanged(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1122598290, smi.eventInstance);
		}
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x0009D100 File Offset: 0x0009B300
	public void MonitorStop(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-828272459, smi.eventInstance);
		}
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x0009D134 File Offset: 0x0009B334
	public virtual EventInfoData GenerateEventPopupData(StateMachineInstanceType smi)
	{
		return null;
	}

	// Token: 0x020013BA RID: 5050
	public class GameplayEventStateMachineInstance : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.GameInstance
	{
		// Token: 0x06008D8C RID: 36236 RVA: 0x0036650A File Offset: 0x0036470A
		public GameplayEventStateMachineInstance(MasterType master, GameplayEventInstance eventInstance, SecondMasterType gameplayEvent) : base(master)
		{
			this.gameplayEvent = gameplayEvent;
			this.eventInstance = eventInstance;
			eventInstance.GetEventPopupData = (() => base.smi.sm.GenerateEventPopupData(base.smi));
			this.serializationSuffix = gameplayEvent.Id;
		}

		// Token: 0x04006C38 RID: 27704
		public GameplayEventInstance eventInstance;

		// Token: 0x04006C39 RID: 27705
		public SecondMasterType gameplayEvent;
	}
}
