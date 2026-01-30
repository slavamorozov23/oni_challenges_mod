using System;
using UnityEngine;

// Token: 0x0200088B RID: 2187
public class CallAdultMonitor : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>
{
	// Token: 0x06003C31 RID: 15409 RVA: 0x00150DD8 File Offset: 0x0014EFD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.CallAdultBehaviour, new StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback(CallAdultMonitor.ShouldCallAdult), delegate(CallAdultMonitor.Instance smi)
		{
			smi.RefreshCallTime();
		});
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x00150E29 File Offset: 0x0014F029
	public static bool ShouldCallAdult(CallAdultMonitor.Instance smi)
	{
		return Time.time >= smi.nextCallTime;
	}

	// Token: 0x0200185A RID: 6234
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007AAD RID: 31405
		public float callMinInterval = 120f;

		// Token: 0x04007AAE RID: 31406
		public float callMaxInterval = 240f;
	}

	// Token: 0x0200185B RID: 6235
	public new class Instance : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.GameInstance
	{
		// Token: 0x06009EA2 RID: 40610 RVA: 0x003A3DC7 File Offset: 0x003A1FC7
		public Instance(IStateMachineTarget master, CallAdultMonitor.Def def) : base(master, def)
		{
			this.RefreshCallTime();
		}

		// Token: 0x06009EA3 RID: 40611 RVA: 0x003A3DD7 File Offset: 0x003A1FD7
		public void RefreshCallTime()
		{
			this.nextCallTime = Time.time + UnityEngine.Random.value * (base.def.callMaxInterval - base.def.callMinInterval) + base.def.callMinInterval;
		}

		// Token: 0x04007AAF RID: 31407
		public float nextCallTime;
	}
}
