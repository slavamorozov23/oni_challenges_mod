using System;
using KSerialization;

// Token: 0x020008B9 RID: 2233
public class RoverChoreMonitor : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>
{
	// Token: 0x06003D8B RID: 15755 RVA: 0x00157690 File Offset: 0x00155890
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.ToggleBehaviour(GameTags.Creatures.Tunnel, (RoverChoreMonitor.Instance smi) => true, null).ToggleBehaviour(GameTags.Creatures.Builder, (RoverChoreMonitor.Instance smi) => true, null);
	}

	// Token: 0x040025FE RID: 9726
	public GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.State loop;

	// Token: 0x020018C5 RID: 6341
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018C6 RID: 6342
	public new class Instance : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.GameInstance
	{
		// Token: 0x0600A041 RID: 41025 RVA: 0x003A959D File Offset: 0x003A779D
		public Instance(IStateMachineTarget master, RoverChoreMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A042 RID: 41026 RVA: 0x003A95AE File Offset: 0x003A77AE
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x04007BEF RID: 31727
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x04007BF0 RID: 31728
		private Action<object> OnDestinationReachedDelegate;
	}
}
