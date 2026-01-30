using System;
using STRINGS;

// Token: 0x020004A8 RID: 1192
public class FixedCaptureChore : Chore<FixedCaptureChore.FixedCaptureChoreStates.Instance>
{
	// Token: 0x0600195D RID: 6493 RVA: 0x0008D7F0 File Offset: 0x0008B9F0
	public FixedCaptureChore(KPrefabID capture_point)
	{
		Chore.Precondition isCreatureAvailableForFixedCapture = default(Chore.Precondition);
		isCreatureAvailableForFixedCapture.id = "IsCreatureAvailableForFixedCapture";
		isCreatureAvailableForFixedCapture.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE;
		isCreatureAvailableForFixedCapture.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return (data as FixedCapturePoint.Instance).IsCreatureAvailableForFixedCapture();
		};
		this.IsCreatureAvailableForFixedCapture = isCreatureAvailableForFixedCapture;
		base..ctor(Db.Get().ChoreTypes.Ranch, capture_point, null, false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		this.AddPrecondition(this.IsCreatureAvailableForFixedCapture, capture_point.GetSMI<FixedCapturePoint.Instance>());
		this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanWrangleCreatures.Id);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, capture_point.GetComponent<Building>());
		Operational component = capture_point.GetComponent<Operational>();
		this.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		Deconstructable component2 = capture_point.GetComponent<Deconstructable>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component2);
		BuildingEnabledButton component3 = capture_point.GetComponent<BuildingEnabledButton>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component3);
		base.smi = new FixedCaptureChore.FixedCaptureChoreStates.Instance(capture_point);
		base.SetPrioritizable(capture_point.GetComponent<Prioritizable>());
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x0008D940 File Offset: 0x0008BB40
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rancher.Set(context.consumerState.gameObject, base.smi, false);
		base.smi.sm.creature.Set(base.smi.fixedCapturePoint.targetCapturable.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000EC9 RID: 3785
	public Chore.Precondition IsCreatureAvailableForFixedCapture;

	// Token: 0x020012E4 RID: 4836
	public class FixedCaptureChoreStates : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance>
	{
		// Token: 0x06008A01 RID: 35329 RVA: 0x003569A8 File Offset: 0x00354BA8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.movetopoint;
			base.Target(this.rancher);
			this.root.Exit("ResetCapturePoint", delegate(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
			{
				smi.fixedCapturePoint.ResetCapturePoint();
			});
			this.movetopoint.MoveTo((FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => Grid.PosToCell(smi.transform.GetPosition()), this.waitforcreature_pre, null, false).Target(this.masterTarget).EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed, null);
			this.waitforcreature_pre.EnterTransition(null, (FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => smi.fixedCapturePoint.IsNullOrStopped()).EnterTransition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft)).EnterTransition(this.waitforcreature, (FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => true);
			this.waitforcreature.ToggleAnims("anim_interacts_rancherstation_kanim", 0f).PlayAnim("calling_loop", KAnim.PlayMode.Loop).Transition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft), UpdateRate.SIM_200ms).Face(this.creature, 0f).Enter("SetRancherIsAvailableForCapturing", delegate(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
			{
				smi.fixedCapturePoint.SetRancherIsAvailableForCapturing();
			}).Exit("ClearRancherIsAvailableForCapturing", delegate(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
			{
				smi.fixedCapturePoint.ClearRancherIsAvailableForCapturing();
			}).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtCapturePoint, this.capturecreature, null);
			this.capturecreature.EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed, null).EnterTransition(this.failed, (FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => smi.fixedCapturePoint.targetCapturable.IsNullOrStopped()).ToggleWork<Capturable>(this.creature, this.success, this.failed, null);
			this.failed.GoTo(null);
			this.success.ReturnSuccess();
		}

		// Token: 0x06008A02 RID: 35330 RVA: 0x00356BDF File Offset: 0x00354DDF
		private static bool HasCreatureLeft(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
		{
			return smi.fixedCapturePoint.targetCapturable.IsNullOrStopped() || !smi.fixedCapturePoint.targetCapturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>();
		}

		// Token: 0x04006983 RID: 27011
		public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;

		// Token: 0x04006984 RID: 27012
		public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter creature;

		// Token: 0x04006985 RID: 27013
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State movetopoint;

		// Token: 0x04006986 RID: 27014
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature_pre;

		// Token: 0x04006987 RID: 27015
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature;

		// Token: 0x04006988 RID: 27016
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State capturecreature;

		// Token: 0x04006989 RID: 27017
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State failed;

		// Token: 0x0400698A RID: 27018
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State success;

		// Token: 0x020027B8 RID: 10168
		public new class Instance : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600C9B9 RID: 51641 RVA: 0x0042A337 File Offset: 0x00428537
			public Instance(KPrefabID capture_point) : base(capture_point)
			{
				this.fixedCapturePoint = capture_point.GetSMI<FixedCapturePoint.Instance>();
			}

			// Token: 0x0400B020 RID: 45088
			public FixedCapturePoint.Instance fixedCapturePoint;
		}
	}
}
