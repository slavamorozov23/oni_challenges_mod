using System;

// Token: 0x02000A19 RID: 2585
public class CreatureDebugGoToMonitor : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>
{
	// Token: 0x06004BC7 RID: 19399 RVA: 0x001B86C2 File Offset: 0x001B68C2
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.HasDebugDestination, new StateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.Transition.ConditionCallback(CreatureDebugGoToMonitor.HasTargetCell), new Action<CreatureDebugGoToMonitor.Instance>(CreatureDebugGoToMonitor.ClearTargetCell));
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001B86F5 File Offset: 0x001B68F5
	private static bool HasTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		return smi.targetCell != Grid.InvalidCell;
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x001B8707 File Offset: 0x001B6907
	private static void ClearTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		smi.targetCell = Grid.InvalidCell;
	}

	// Token: 0x02001ABC RID: 6844
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001ABD RID: 6845
	public new class Instance : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x0600A6F0 RID: 42736 RVA: 0x003BB07E File Offset: 0x003B927E
		public Instance(IStateMachineTarget target, CreatureDebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x0600A6F1 RID: 42737 RVA: 0x003BB093 File Offset: 0x003B9293
		public void GoToCursor()
		{
			this.targetCell = DebugHandler.GetMouseCell();
		}

		// Token: 0x0600A6F2 RID: 42738 RVA: 0x003BB0A0 File Offset: 0x003B92A0
		public void GoToCell(int cellIndex)
		{
			this.targetCell = cellIndex;
		}

		// Token: 0x04008298 RID: 33432
		public int targetCell = Grid.InvalidCell;
	}
}
