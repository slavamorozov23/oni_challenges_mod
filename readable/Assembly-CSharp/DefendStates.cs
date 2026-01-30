using System;
using STRINGS;

// Token: 0x020000ED RID: 237
public class DefendStates : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>
{
	// Token: 0x06000459 RID: 1113 RVA: 0x00023F30 File Offset: 0x00022130
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.protectEntity.moveToThreat;
		GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State state = this.root.Enter("SetTarget", delegate(DefendStates.Instance smi)
		{
			this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi, false);
		});
		string name = CREATURES.STATUSITEMS.ATTACKINGENTITY.NAME;
		string tooltip = CREATURES.STATUSITEMS.ATTACKINGENTITY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.protectEntity.moveToThreat.InitializeStates(this.masterTarget, this.target, this.protectEntity.attackThreat, null, CrabTuning.DEFEND_OFFSETS, null);
		this.protectEntity.attackThreat.Enter(delegate(DefendStates.Instance smi)
		{
			smi.animcontroller.Play("slap_pre", KAnim.PlayMode.Once, 1f, 0f);
			smi.animcontroller.Queue("slap", KAnim.PlayMode.Once, 1f, 0f);
			smi.animcontroller.Queue("slap_pst", KAnim.PlayMode.Once, 1f, 0f);
			smi.Schedule(0.5f, delegate
			{
				smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi));
			}, null);
		}).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Defend, false);
	}

	// Token: 0x0400033D RID: 829
	public StateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.TargetParameter target;

	// Token: 0x0400033E RID: 830
	public DefendStates.ProtectStates protectEntity;

	// Token: 0x0400033F RID: 831
	public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State behaviourcomplete;

	// Token: 0x02001122 RID: 4386
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001123 RID: 4387
	public new class Instance : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.GameInstance
	{
		// Token: 0x060083D6 RID: 33750 RVA: 0x0034407F File Offset: 0x0034227F
		public Instance(Chore<DefendStates.Instance> chore, DefendStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Defend);
		}

		// Token: 0x0400641B RID: 25627
		[MyCmpGet]
		public KBatchedAnimController animcontroller;
	}

	// Token: 0x02001124 RID: 4388
	public class ProtectStates : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State
	{
		// Token: 0x0400641C RID: 25628
		public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.ApproachSubState<AttackableBase> moveToThreat;

		// Token: 0x0400641D RID: 25629
		public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State attackThreat;
	}
}
