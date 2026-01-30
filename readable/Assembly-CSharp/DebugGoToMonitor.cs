using System;

// Token: 0x02000A1E RID: 2590
public class DebugGoToMonitor : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>
{
	// Token: 0x06004BDD RID: 19421 RVA: 0x001B8D14 File Offset: 0x001B6F14
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.hastarget.ToggleChore((DebugGoToMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.DebugGoTo, (MoveChore.StatesInstance smii) => smi.targetCellIndex, false), this.satisfied);
	}

	// Token: 0x0400324C RID: 12876
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State satisfied;

	// Token: 0x0400324D RID: 12877
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State hastarget;

	// Token: 0x02001ACA RID: 6858
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001ACB RID: 6859
	public new class Instance : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x0600A71D RID: 42781 RVA: 0x003BB795 File Offset: 0x003B9995
		public Instance(IStateMachineTarget target, DebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x0600A71E RID: 42782 RVA: 0x003BB7AC File Offset: 0x003B99AC
		public void GoToCursor()
		{
			this.targetCellIndex = DebugHandler.GetMouseCell();
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x0600A71F RID: 42783 RVA: 0x003BB7FC File Offset: 0x003B99FC
		public void GoToCell(int cellIndex)
		{
			this.targetCellIndex = cellIndex;
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x040082B6 RID: 33462
		public int targetCellIndex = Grid.InvalidCell;
	}
}
