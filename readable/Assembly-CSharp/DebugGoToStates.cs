using System;
using STRINGS;

// Token: 0x020000EC RID: 236
public class DebugGoToStates : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>
{
	// Token: 0x06000456 RID: 1110 RVA: 0x00023E8C File Offset: 0x0002208C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State state = this.moving.MoveTo(new Func<DebugGoToStates.Instance, int>(DebugGoToStates.GetTargetCell), this.behaviourcomplete, this.behaviourcomplete, true);
		string name = CREATURES.STATUSITEMS.DEBUGGOTO.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEBUGGOTO.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.behaviourcomplete.BehaviourComplete(GameTags.HasDebugDestination, false);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00023F1A File Offset: 0x0002211A
	private static int GetTargetCell(DebugGoToStates.Instance smi)
	{
		return smi.GetSMI<CreatureDebugGoToMonitor.Instance>().targetCell;
	}

	// Token: 0x0400033B RID: 827
	public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State moving;

	// Token: 0x0400033C RID: 828
	public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete;

	// Token: 0x02001120 RID: 4384
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001121 RID: 4385
	public new class Instance : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.GameInstance
	{
		// Token: 0x060083D4 RID: 33748 RVA: 0x00344053 File Offset: 0x00342253
		public Instance(Chore<DebugGoToStates.Instance> chore, DebugGoToStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.HasDebugDestination);
		}
	}
}
