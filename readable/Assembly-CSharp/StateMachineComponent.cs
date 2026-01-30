using System;
using KSerialization;

// Token: 0x02000538 RID: 1336
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class StateMachineComponent : KMonoBehaviour, ISaveLoadable, IStateMachineTarget
{
	// Token: 0x06001CCF RID: 7375
	public abstract StateMachine.Instance GetSMI();

	// Token: 0x040010FE RID: 4350
	[MyCmpAdd]
	protected StateMachineController stateMachineController;
}
