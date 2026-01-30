using System;
using UnityEngine;

// Token: 0x02000A24 RID: 2596
public class EmoteMonitor : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance>
{
	// Token: 0x06004BEE RID: 19438 RVA: 0x001B93D0 File Offset: 0x001B75D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.ScheduleGoTo((EmoteMonitor.Instance smi) => (float)UnityEngine.Random.Range(30, 90), this.ready);
		this.ready.ToggleUrge(Db.Get().Urges.Emote).EventHandler(GameHashes.BeginChore, delegate(EmoteMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
	}

	// Token: 0x04003257 RID: 12887
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003258 RID: 12888
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x02001ADA RID: 6874
	public new class Instance : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A753 RID: 42835 RVA: 0x003BC02D File Offset: 0x003BA22D
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A754 RID: 42836 RVA: 0x003BC036 File Offset: 0x003BA236
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.Emote))
			{
				this.GoTo(base.sm.satisfied);
			}
		}
	}
}
