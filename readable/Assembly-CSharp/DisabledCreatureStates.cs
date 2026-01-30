using System;
using STRINGS;

// Token: 0x020000EF RID: 239
public class DisabledCreatureStates : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>
{
	// Token: 0x06000460 RID: 1120 RVA: 0x00024184 File Offset: 0x00022384
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.disableCreature;
		GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.DISABLED.NAME;
		string tooltip = CREATURES.STATUSITEMS.DISABLED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TagTransition(GameTags.Creatures.Behaviours.DisableCreature, this.behaviourcomplete, true);
		this.disableCreature.PlayAnim((DisabledCreatureStates.Instance smi) => smi.def.disabledAnim, KAnim.PlayMode.Once);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.DisableCreature, false);
	}

	// Token: 0x04000343 RID: 835
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State disableCreature;

	// Token: 0x04000344 RID: 836
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State behaviourcomplete;

	// Token: 0x02001129 RID: 4393
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060083E0 RID: 33760 RVA: 0x00344148 File Offset: 0x00342348
		public Def(string anim)
		{
			this.disabledAnim = anim;
		}

		// Token: 0x04006422 RID: 25634
		public string disabledAnim = "off";
	}

	// Token: 0x0200112A RID: 4394
	public new class Instance : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.GameInstance
	{
		// Token: 0x060083E1 RID: 33761 RVA: 0x00344162 File Offset: 0x00342362
		public Instance(Chore<DisabledCreatureStates.Instance> chore, DisabledCreatureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Behaviours.DisableCreature);
		}
	}
}
