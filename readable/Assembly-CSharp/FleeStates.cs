using System;
using STRINGS;

// Token: 0x020000F7 RID: 247
public class FleeStates : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>
{
	// Token: 0x06000490 RID: 1168 RVA: 0x00025774 File Offset: 0x00023974
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.plan;
		GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State state = this.root.Enter("SetFleeTarget", delegate(FleeStates.Instance smi)
		{
			this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi.GetSMI<ThreatMonitor.Instance>().MainThreat), smi, false);
		});
		string name = CREATURES.STATUSITEMS.FLEEING.NAME;
		string tooltip = CREATURES.STATUSITEMS.FLEEING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.plan.Enter(delegate(FleeStates.Instance smi)
		{
			ThreatMonitor.Instance smi2 = smi.master.gameObject.GetSMI<ThreatMonitor.Instance>();
			this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi2.MainThreat), smi, false);
			if (this.fleeToTarget.Get(smi) != null)
			{
				smi.GoTo(this.approach);
				return;
			}
			smi.GoTo(this.cower);
		});
		this.approach.InitializeStates(this.mover, this.fleeToTarget, this.cower, this.cower, null, NavigationTactics.ReduceTravelDistance).Enter(delegate(FleeStates.Instance smi)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, CREATURES.STATUSITEMS.FLEEING.NAME.text, smi.master.transform, 1.5f, false);
		});
		this.cower.Enter(delegate(FleeStates.Instance smi)
		{
			string s = "DEFAULT COWER ANIMATION";
			if (smi.Get<KBatchedAnimController>().HasAnimation("cower"))
			{
				s = "cower";
			}
			else if (smi.Get<KBatchedAnimController>().HasAnimation("idle"))
			{
				s = "idle";
			}
			else if (smi.Get<KBatchedAnimController>().HasAnimation("idle_loop"))
			{
				s = "idle_loop";
			}
			smi.Get<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		}).ScheduleGoTo(2f, this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Flee, false);
	}

	// Token: 0x04000362 RID: 866
	private StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter mover;

	// Token: 0x04000363 RID: 867
	public StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter fleeToTarget;

	// Token: 0x04000364 RID: 868
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State plan;

	// Token: 0x04000365 RID: 869
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.ApproachSubState<IApproachable> approach;

	// Token: 0x04000366 RID: 870
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State cower;

	// Token: 0x04000367 RID: 871
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State behaviourcomplete;

	// Token: 0x02001145 RID: 4421
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001146 RID: 4422
	public new class Instance : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.GameInstance
	{
		// Token: 0x06008428 RID: 33832 RVA: 0x003447A2 File Offset: 0x003429A2
		public Instance(Chore<FleeStates.Instance> chore, FleeStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Flee);
			base.sm.mover.Set(base.GetComponent<Navigator>(), base.smi);
		}
	}
}
