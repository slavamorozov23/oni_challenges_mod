using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class RoboDancerChore : Chore<RoboDancerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x0600199F RID: 6559 RVA: 0x0008F3D8 File Offset: 0x0008D5D8
	public RoboDancerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new RoboDancerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x0008F472 File Offset: 0x0008D672
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000EDD RID: 3805
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x02001319 RID: 4889
	public class States : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore>
	{
		// Token: 0x06008ADB RID: 35547 RVA: 0x0035B2A0 File Offset: 0x003594A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.roboDancer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksTick, this.goToStand, (RoboDancerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((RoboDancerChore.StatesInstance smi) => smi.GetTargetCell(), this.dancing, this.idle, false);
			this.dancing.ToggleEffect("Dancing").ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.dancing.pre).Update(delegate(RoboDancerChore.StatesInstance smi, float dt)
			{
				RoboDancer.Instance smi2 = this.roboDancer.Get(smi).GetSMI<RoboDancer.Instance>();
				RoboDancer sm = smi2.sm;
				sm.hasAudience.Set(smi.HasAudience(), smi2, false);
				sm.timeSpentDancing.Set(sm.timeSpentDancing.Get(smi2) + dt, smi2, false);
			}, UpdateRate.SIM_33ms, false).Exit(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
			});
			this.dancing.pre.QueueAnim("robotdance_pre", false, null).OnAnimQueueComplete(this.dancing.variation_1).Enter(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
				smi.CreateAudienceWorkables();
			});
			this.dancing.variation_1.QueueAnim("robotdance_loop", false, null).OnAnimQueueComplete(this.dancing.variation_2);
			this.dancing.variation_2.QueueAnim("robotdance_2_loop", false, null).OnAnimQueueComplete(this.dancing.pst);
			this.dancing.pst.QueueAnim("robotdance_pst", false, null).OnAnimQueueComplete(this.dancing.pre);
		}

		// Token: 0x04006A37 RID: 27191
		public StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.TargetParameter roboDancer;

		// Token: 0x04006A38 RID: 27192
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State idle;

		// Token: 0x04006A39 RID: 27193
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State goToStand;

		// Token: 0x04006A3A RID: 27194
		public RoboDancerChore.States.DancingStates dancing;

		// Token: 0x020027E2 RID: 10210
		public class DancingStates : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State
		{
			// Token: 0x0400B0C3 RID: 45251
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pre;

			// Token: 0x0400B0C4 RID: 45252
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_1;

			// Token: 0x0400B0C5 RID: 45253
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_2;

			// Token: 0x0400B0C6 RID: 45254
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pst;
		}
	}

	// Token: 0x0200131A RID: 4890
	public class StatesInstance : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.GameInstance
	{
		// Token: 0x06008ADE RID: 35550 RVA: 0x0035B4BA File Offset: 0x003596BA
		public StatesInstance(RoboDancerChore master, GameObject roboDancer) : base(master)
		{
			this.roboDancer = roboDancer;
			base.sm.roboDancer.Set(roboDancer, base.smi, false);
		}

		// Token: 0x06008ADF RID: 35551 RVA: 0x0035B4EF File Offset: 0x003596EF
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06008AE0 RID: 35552 RVA: 0x0035B510 File Offset: 0x00359710
		public int GetTargetCell()
		{
			Navigator component = base.GetComponent<Navigator>();
			float num = float.MaxValue;
			SocialGatheringPoint socialGatheringPoint = null;
			foreach (SocialGatheringPoint socialGatheringPoint2 in Components.SocialGatheringPoints.GetItems((int)Grid.WorldIdx[Grid.PosToCell(this)]))
			{
				float num2 = (float)component.GetNavigationCost(Grid.PosToCell(socialGatheringPoint2));
				if (num2 != -1f && num2 < num)
				{
					num = num2;
					socialGatheringPoint = socialGatheringPoint2;
				}
			}
			if (socialGatheringPoint != null)
			{
				return Grid.PosToCell(socialGatheringPoint);
			}
			return Grid.PosToCell(base.master.gameObject);
		}

		// Token: 0x06008AE1 RID: 35553 RVA: 0x0035B5C0 File Offset: 0x003597C0
		public bool HasAudience()
		{
			if (base.smi.watchWorkables == null)
			{
				return false;
			}
			WatchRoboDancerWorkable[] array = base.smi.watchWorkables;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].worker)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008AE2 RID: 35554 RVA: 0x0035B608 File Offset: 0x00359808
		public void CreateAudienceWorkables()
		{
			int num = Grid.PosToCell(base.gameObject);
			Vector3Int[] array = new Vector3Int[]
			{
				Vector3Int.left * 3,
				Vector3Int.left * 2,
				Vector3Int.left,
				Vector3Int.right,
				Vector3Int.right * 2,
				Vector3Int.right * 3
			};
			int num2 = 0;
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				int cell = Grid.OffsetCell(num, array[i].x, array[i].y);
				if (Grid.IsValidCellInWorld(cell, (int)Grid.WorldIdx[num]))
				{
					GameObject gameObject = ChoreHelpers.CreateLocator("WatchRoboDancerWorkable", Grid.CellToPos(cell));
					this.audienceWorkables[i] = gameObject;
					KSelectable kselectable = gameObject.AddOrGet<KSelectable>();
					kselectable.SetName("WatchRoboDancerWorkable");
					kselectable.IsSelectable = false;
					WatchRoboDancerWorkable watchRoboDancerWorkable = gameObject.AddOrGet<WatchRoboDancerWorkable>();
					watchRoboDancerWorkable.owner = this.roboDancer;
					WorkChore<WatchRoboDancerWorkable> workChore = new WorkChore<WatchRoboDancerWorkable>(Db.Get().ChoreTypes.JoyReaction, watchRoboDancerWorkable, null, true, null, null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
					workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
					workChore.AddPrecondition(RoboDancerChore.StatesInstance.IsNotRoboHyped, workChore);
					num2++;
				}
			}
			this.watchWorkables = new WatchRoboDancerWorkable[num2];
			for (int j = 0; j < num2; j++)
			{
				this.watchWorkables[j] = this.audienceWorkables[j].GetComponent<WatchRoboDancerWorkable>();
			}
		}

		// Token: 0x06008AE3 RID: 35555 RVA: 0x0035B7B0 File Offset: 0x003599B0
		public void ClearAudienceWorkables()
		{
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				if (!(this.audienceWorkables[i] == null))
				{
					WorkerBase worker = this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().worker;
					if (worker != null)
					{
						this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().CompleteWork(worker);
					}
					ChoreHelpers.DestroyLocator(this.audienceWorkables[i]);
				}
			}
			this.watchWorkables = null;
		}

		// Token: 0x04006A3B RID: 27195
		private GameObject roboDancer;

		// Token: 0x04006A3C RID: 27196
		private GameObject[] audienceWorkables = new GameObject[4];

		// Token: 0x04006A3D RID: 27197
		private WatchRoboDancerWorkable[] watchWorkables;

		// Token: 0x04006A3E RID: 27198
		private static Chore.Precondition IsNotRoboHyped = new Chore.Precondition
		{
			id = "IsNotRoboHyped",
			description = "__ Duplicant hasn't watched the dance yet",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return !(context.consumerState.consumer == null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT);
			}
		};
	}
}
