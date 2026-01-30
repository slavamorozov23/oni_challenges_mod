using System;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class RecoverFromHeatChore : Chore<RecoverFromHeatChore.Instance>
{
	// Token: 0x0600198A RID: 6538 RVA: 0x0008EC34 File Offset: 0x0008CE34
	public RecoverFromHeatChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverFromHeat, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromHeatChore.Instance(this, target.gameObject);
		HeatImmunityMonitor.Instance chillyBones = target.gameObject.GetSMI<HeatImmunityMonitor.Instance>();
		Func<int> data = () => chillyBones.ShelterCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x0200130D RID: 4877
	public class States : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore>
	{
		// Token: 0x06008A92 RID: 35474 RVA: 0x00359CE0 File Offset: 0x00357EE0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateHeatImmunityProvider", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.heatImmunityProvider, null).Enter("AnimOverride", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.cachedAnimName = RecoverFromHeatChore.States.GetAnimFileName(smi);
				smi.GetComponent<KAnimControllerBase>().AddAnimOverrides(Assets.GetAnim(smi.cachedAnimName), 0f);
			}).Exit(delegate(RecoverFromHeatChore.Instance smi)
			{
				if (smi.cachedAnimName != HashedString.Invalid)
				{
					smi.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(Assets.GetAnim(smi.cachedAnimName));
				}
			}).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringFromHeat);
			this.recover.pre.Face(this.heatImmunityProvider, 0f).PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Not(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback(RecoverFromHeatChore.States.ApplyHeatImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x06008A93 RID: 35475 RVA: 0x00359FA0 File Offset: 0x003581A0
		public static bool IsImmunityProviderStillValid(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x06008A94 RID: 35476 RVA: 0x00359FC0 File Offset: 0x003581C0
		public static void ApplyHeatImmunityEffect(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x06008A95 RID: 35477 RVA: 0x00359FE4 File Offset: 0x003581E4
		public static HashedString GetAnimFileName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x06008A96 RID: 35478 RVA: 0x0035A01A File Offset: 0x0035821A
		public static string GetPreAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x06008A97 RID: 35479 RVA: 0x0035A041 File Offset: 0x00358241
		public static string GetLoopAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x06008A98 RID: 35480 RVA: 0x0035A068 File Offset: 0x00358268
		public static string GetPstAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x06008A99 RID: 35481 RVA: 0x0035A090 File Offset: 0x00358290
		public static string GetAnimFromHeatImmunityProvider(RecoverFromHeatChore.Instance smi, Func<HeatImmunityProvider.Instance, string> getCallback)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x04006A03 RID: 27139
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04006A04 RID: 27140
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.PreLoopPostState recover;

		// Token: 0x04006A05 RID: 27141
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State remove_suit;

		// Token: 0x04006A06 RID: 27142
		public RecoverFromHeatChore.States.CompleteStates complete;

		// Token: 0x04006A07 RID: 27143
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter heatImmunityProvider;

		// Token: 0x04006A08 RID: 27144
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter entityRecovering;

		// Token: 0x04006A09 RID: 27145
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter locator;

		// Token: 0x020027D1 RID: 10193
		public class CompleteStates : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State
		{
			// Token: 0x0400B08A RID: 45194
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State evaluate;

			// Token: 0x0400B08B RID: 45195
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State fail;

			// Token: 0x0400B08C RID: 45196
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State success;
		}
	}

	// Token: 0x0200130E RID: 4878
	public class Instance : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.GameInstance
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06008A9B RID: 35483 RVA: 0x0035A0B8 File Offset: 0x003582B8
		public HeatImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.heatImmunityProvider.Get(this) == null))
				{
					return base.sm.heatImmunityProvider.Get(this).GetSMI<HeatImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06008A9C RID: 35484 RVA: 0x0035A0EB File Offset: 0x003582EB
		public HeatImmunityMonitor.Instance heatImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<HeatImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06008A9D RID: 35485 RVA: 0x0035A104 File Offset: 0x00358304
		public Instance(RecoverFromHeatChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			HeatImmunityMonitor.Instance heatImmunityMonitor = this.heatImmunityMonitor;
			if (heatImmunityMonitor.NearestImmunityProvider != null && !heatImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.heatImmunityProvider.Set(heatImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06008A9E RID: 35486 RVA: 0x0035A168 File Offset: 0x00358368
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06008A9F RID: 35487 RVA: 0x0035A1A0 File Offset: 0x003583A0
		public void UpdateImmunityProvider()
		{
			HeatImmunityProvider.Instance nearestImmunityProvider = this.heatImmunityMonitor.NearestImmunityProvider;
			base.sm.heatImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06008AA0 RID: 35488 RVA: 0x0035A1E0 File Offset: 0x003583E0
		public void UpdateLocator()
		{
			int num = this.heatImmunityMonitor.ShelterCell;
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

		// Token: 0x06008AA1 RID: 35489 RVA: 0x0035A257 File Offset: 0x00358457
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x04006A0A RID: 27146
		private int targetCell;

		// Token: 0x04006A0B RID: 27147
		public HashedString cachedAnimName;
	}
}
