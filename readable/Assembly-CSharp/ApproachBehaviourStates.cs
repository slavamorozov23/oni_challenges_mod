using System;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class ApproachBehaviourStates : GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>
{
	// Token: 0x060003CB RID: 971 RVA: 0x000202DC File Offset: 0x0001E4DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.approach;
		this.root.Enter(new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.RefreshTarget)).Enter(new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.Reserve)).Exit(new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.Unreserve)).EventHandler(GameHashes.ApproachableTargetChanged, new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.RefreshTarget));
		this.approach.InitializeStates(this.self, this.target, (ApproachBehaviourStates.Instance smi) => smi.targetOffsets, this.interact, this.failure, null).ToggleMainStatusItem((ApproachBehaviourStates.Instance smi) => smi.GetMonitor().GetApproachStatusItem(), null);
		this.interact.Enter(delegate(ApproachBehaviourStates.Instance smi)
		{
			smi.GetMonitor().OnArrive();
		}).DefaultState(this.interact.pre).OnTargetLost(this.target, this.failure).ToggleMainStatusItem((ApproachBehaviourStates.Instance smi) => smi.GetMonitor().GetBehaviourStatusItem(), null);
		this.interact.pre.PlayAnim((ApproachBehaviourStates.Instance smi) => smi.def.preAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.interact.loop);
		this.interact.loop.PlayAnim((ApproachBehaviourStates.Instance smi) => smi.def.loopAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.interact.pst);
		this.interact.pst.PlayAnim((ApproachBehaviourStates.Instance smi) => smi.def.pstAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.BehaviourComplete((ApproachBehaviourStates.Instance smi) => smi.def.behaviourTag, false).Exit(delegate(ApproachBehaviourStates.Instance smi)
		{
			smi.GetMonitor().OnSuccess();
		});
		this.failure.Enter(delegate(ApproachBehaviourStates.Instance smi)
		{
			smi.GetMonitor().OnFailure();
		}).GoTo(null);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0002055A File Offset: 0x0001E75A
	private static void Reserve(ApproachBehaviourStates.Instance smi)
	{
		if (smi.def.reserveTag != Tag.Invalid)
		{
			smi.sm.target.Get(smi).GetComponent<KPrefabID>().SetTag(smi.def.reserveTag, true);
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0002059C File Offset: 0x0001E79C
	private static void Unreserve(ApproachBehaviourStates.Instance smi)
	{
		if (smi.def.reserveTag != Tag.Invalid && smi.sm.target.Get(smi) != null)
		{
			smi.sm.target.Get(smi).GetComponent<KPrefabID>().RemoveTag(smi.def.reserveTag);
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00020600 File Offset: 0x0001E800
	public static void RefreshTarget(ApproachBehaviourStates.Instance smi)
	{
		GameObject gameObject = smi.GetMonitor().GetTarget();
		if (gameObject == null)
		{
			smi.GoTo(smi.sm.failure);
			return;
		}
		smi.targetOffsets = smi.GetMonitor().GetApproachOffsets();
		smi.sm.target.Set(gameObject, smi, false);
	}

	// Token: 0x040002EC RID: 748
	public ApproachBehaviourStates.InteractState interact;

	// Token: 0x040002ED RID: 749
	public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State behaviourComplete;

	// Token: 0x040002EE RID: 750
	public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.ApproachSubState<IApproachable> approach;

	// Token: 0x040002EF RID: 751
	public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State failure;

	// Token: 0x040002F0 RID: 752
	public StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.TargetParameter self;

	// Token: 0x040002F1 RID: 753
	public StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.TargetParameter target;

	// Token: 0x020010CF RID: 4303
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600830C RID: 33548 RVA: 0x00342BC8 File Offset: 0x00340DC8
		public Def(Tag monitorId, Tag behaviourTag)
		{
			this.monitorId = monitorId;
			this.behaviourTag = behaviourTag;
		}

		// Token: 0x04006344 RID: 25412
		public Tag monitorId;

		// Token: 0x04006345 RID: 25413
		public Tag behaviourTag;

		// Token: 0x04006346 RID: 25414
		public Tag reserveTag = GameTags.Creatures.ReservedByCreature;

		// Token: 0x04006347 RID: 25415
		public string preAnim = "";

		// Token: 0x04006348 RID: 25416
		public string loopAnim = "";

		// Token: 0x04006349 RID: 25417
		public string pstAnim = "";
	}

	// Token: 0x020010D0 RID: 4304
	public class InteractState : GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State
	{
		// Token: 0x0400634A RID: 25418
		public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State pre;

		// Token: 0x0400634B RID: 25419
		public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State loop;

		// Token: 0x0400634C RID: 25420
		public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State pst;
	}

	// Token: 0x020010D1 RID: 4305
	public new class Instance : GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.GameInstance
	{
		// Token: 0x0600830E RID: 33550 RVA: 0x00342C20 File Offset: 0x00340E20
		public Instance(Chore<ApproachBehaviourStates.Instance> chore, ApproachBehaviourStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
			base.sm.self.Set(base.smi.gameObject, base.smi, false);
		}

		// Token: 0x0600830F RID: 33551 RVA: 0x00342C73 File Offset: 0x00340E73
		public IApproachableBehaviour GetMonitor()
		{
			if (this.monitor.IsNullOrDestroyed())
			{
				this.SetMonitor();
			}
			return this.monitor;
		}

		// Token: 0x06008310 RID: 33552 RVA: 0x00342C90 File Offset: 0x00340E90
		private void SetMonitor()
		{
			foreach (ICreatureMonitor creatureMonitor in base.gameObject.GetAllSMI<ICreatureMonitor>())
			{
				if (creatureMonitor.Id == base.def.monitorId)
				{
					this.monitor = (creatureMonitor as IApproachableBehaviour);
					break;
				}
			}
			global::Debug.Assert(base.smi.monitor != null, "Could not find monitor with ID");
		}

		// Token: 0x0400634D RID: 25421
		private IApproachableBehaviour monitor;

		// Token: 0x0400634E RID: 25422
		public CellOffset[] targetOffsets;
	}
}
