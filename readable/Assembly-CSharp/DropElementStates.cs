using System;
using STRINGS;

// Token: 0x020000F1 RID: 241
public class DropElementStates : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>
{
	// Token: 0x06000471 RID: 1137 RVA: 0x00024748 File Offset: 0x00022948
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.dropping;
		GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.EXPELLING_GAS.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.dropping.PlayAnim("dirty").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.Enter("DropElement", delegate(DropElementStates.Instance smi)
		{
			smi.GetSMI<ElementDropperMonitor.Instance>().DropPeriodicElement();
		}).QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.WantsToDropElements, false);
	}

	// Token: 0x0400034A RID: 842
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State dropping;

	// Token: 0x0400034B RID: 843
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State behaviourcomplete;

	// Token: 0x02001130 RID: 4400
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001131 RID: 4401
	public new class Instance : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.GameInstance
	{
		// Token: 0x060083F2 RID: 33778 RVA: 0x003443D8 File Offset: 0x003425D8
		public Instance(Chore<DropElementStates.Instance> chore, DropElementStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToDropElements);
		}
	}
}
