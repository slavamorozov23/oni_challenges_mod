using System;
using Klei.AI;

// Token: 0x02000A2F RID: 2607
public class InSpaceMonitor : GameStateMachine<InSpaceMonitor, InSpaceMonitor.Instance>
{
	// Token: 0x06004C2E RID: 19502 RVA: 0x001BAB1C File Offset: 0x001B8D1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Enter(delegate(InSpaceMonitor.Instance smi)
		{
			if (smi.IsInSpace())
			{
				smi.GoTo(this.inSpace);
			}
		});
		this.idle.EventTransition(GameHashes.MinionMigration, (InSpaceMonitor.Instance smi) => Game.Instance, this.inSpace, (InSpaceMonitor.Instance smi) => smi.IsInSpace()).Enter(delegate(InSpaceMonitor.Instance smi)
		{
			Effects component = smi.master.gameObject.GetComponent<Effects>();
			if (component != null && component.HasEffect("SpaceBuzz"))
			{
				component.Remove("SpaceBuzz");
			}
		});
		this.inSpace.EventTransition(GameHashes.MinionMigration, (InSpaceMonitor.Instance smi) => Game.Instance, this.idle, (InSpaceMonitor.Instance smi) => !smi.IsInSpace()).ToggleEffect("SpaceBuzz");
	}

	// Token: 0x04003292 RID: 12946
	private const string SPACE_EFFECT_NAME = "SpaceBuzz";

	// Token: 0x04003293 RID: 12947
	public GameStateMachine<InSpaceMonitor, InSpaceMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04003294 RID: 12948
	public GameStateMachine<InSpaceMonitor, InSpaceMonitor.Instance, IStateMachineTarget, object>.State inSpace;

	// Token: 0x02001AF8 RID: 6904
	public new class Instance : GameStateMachine<InSpaceMonitor, InSpaceMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A7E4 RID: 42980 RVA: 0x003BDEEF File Offset: 0x003BC0EF
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A7E5 RID: 42981 RVA: 0x003BDEF8 File Offset: 0x003BC0F8
		public bool IsInSpace()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			return myWorld.GetComponent<Clustercraft>() && parentWorldId == id;
		}
	}
}
