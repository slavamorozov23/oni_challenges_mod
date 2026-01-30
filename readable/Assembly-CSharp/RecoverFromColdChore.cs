using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class RecoverFromColdChore : Chore<RecoverFromColdChore.Instance>
{
	// Token: 0x06001989 RID: 6537 RVA: 0x0008EBA8 File Offset: 0x0008CDA8
	public RecoverFromColdChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverWarmth, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromColdChore.Instance(this, target.gameObject);
		ColdImmunityMonitor.Instance coldImmunityMonitor = target.gameObject.GetSMI<ColdImmunityMonitor.Instance>();
		Func<int> data = () => coldImmunityMonitor.WarmUpCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x0200130A RID: 4874
	public class States : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore>
	{
		// Token: 0x06008A80 RID: 35456 RVA: 0x00359764 File Offset: 0x00357964
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateColdImmunityProvider", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.coldImmunityProvider, null).ToggleAnims(new Func<RecoverFromColdChore.Instance, HashedString>(RecoverFromColdChore.States.GetAnimFileName)).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringWarmnth);
			this.recover.pre.Face(this.coldImmunityProvider, 0f).PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Not(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback(RecoverFromColdChore.States.ApplyColdImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x06008A81 RID: 35457 RVA: 0x003599E8 File Offset: 0x00357BE8
		public static bool IsImmunityProviderStillValid(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x06008A82 RID: 35458 RVA: 0x00359A08 File Offset: 0x00357C08
		public static void ApplyColdImmunityEffect(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x06008A83 RID: 35459 RVA: 0x00359A2C File Offset: 0x00357C2C
		public static HashedString GetAnimFileName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x06008A84 RID: 35460 RVA: 0x00359A62 File Offset: 0x00357C62
		public static string GetPreAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x06008A85 RID: 35461 RVA: 0x00359A89 File Offset: 0x00357C89
		public static string GetLoopAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x06008A86 RID: 35462 RVA: 0x00359AB0 File Offset: 0x00357CB0
		public static string GetPstAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x06008A87 RID: 35463 RVA: 0x00359AD8 File Offset: 0x00357CD8
		public static string GetAnimFromColdImmunityProvider(RecoverFromColdChore.Instance smi, Func<ColdImmunityProvider.Instance, string> getCallback)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x040069FA RID: 27130
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040069FB RID: 27131
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.PreLoopPostState recover;

		// Token: 0x040069FC RID: 27132
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State remove_suit;

		// Token: 0x040069FD RID: 27133
		public RecoverFromColdChore.States.CompleteStates complete;

		// Token: 0x040069FE RID: 27134
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter coldImmunityProvider;

		// Token: 0x040069FF RID: 27135
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter entityRecovering;

		// Token: 0x04006A00 RID: 27136
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter locator;

		// Token: 0x020027CE RID: 10190
		public class CompleteStates : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State
		{
			// Token: 0x0400B07D RID: 45181
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State evaluate;

			// Token: 0x0400B07E RID: 45182
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State fail;

			// Token: 0x0400B07F RID: 45183
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State success;
		}
	}

	// Token: 0x0200130B RID: 4875
	public class Instance : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.GameInstance
	{
		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06008A89 RID: 35465 RVA: 0x00359B00 File Offset: 0x00357D00
		public ColdImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.coldImmunityProvider.Get(this) == null))
				{
					return base.sm.coldImmunityProvider.Get(this).GetSMI<ColdImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06008A8A RID: 35466 RVA: 0x00359B33 File Offset: 0x00357D33
		public ColdImmunityMonitor.Instance coldImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<ColdImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06008A8B RID: 35467 RVA: 0x00359B4C File Offset: 0x00357D4C
		public Instance(RecoverFromColdChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			ColdImmunityMonitor.Instance coldImmunityMonitor = this.coldImmunityMonitor;
			if (coldImmunityMonitor.NearestImmunityProvider != null && !coldImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.coldImmunityProvider.Set(coldImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06008A8C RID: 35468 RVA: 0x00359BB0 File Offset: 0x00357DB0
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06008A8D RID: 35469 RVA: 0x00359BE8 File Offset: 0x00357DE8
		public void UpdateImmunityProvider()
		{
			ColdImmunityProvider.Instance nearestImmunityProvider = this.coldImmunityMonitor.NearestImmunityProvider;
			base.sm.coldImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06008A8E RID: 35470 RVA: 0x00359C28 File Offset: 0x00357E28
		public void UpdateLocator()
		{
			int num = this.coldImmunityMonitor.WarmUpCell;
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.entityRecovering.Get<Transform>(base.smi).GetPosition());
				this.DestroyLocator();
			}
			else
			{
				Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
				base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
			}
			this.targetCell = num;
		}

		// Token: 0x06008A8F RID: 35471 RVA: 0x00359C9F File Offset: 0x00357E9F
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x04006A01 RID: 27137
		private int targetCell;
	}
}
