using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
public class FleeChore : Chore<FleeChore.StatesInstance>
{
	// Token: 0x0600195F RID: 6495 RVA: 0x0008D9B0 File Offset: 0x0008BBB0
	public FleeChore(IStateMachineTarget target, GameObject enemy) : base(Db.Get().ChoreTypes.Flee, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new FleeChore.StatesInstance(this);
		base.smi.sm.self.Set(this.gameObject, base.smi, false);
		this.nav = this.gameObject.GetComponent<Navigator>();
		base.smi.sm.fleeFromTarget.Set(enemy, base.smi, false);
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x0008DA44 File Offset: 0x0008BC44
	private bool isInFavoredDirection(int cell, int fleeFromCell)
	{
		bool flag = Grid.CellToPos(fleeFromCell).x < this.gameObject.transform.GetPosition().x;
		bool flag2 = Grid.CellToPos(fleeFromCell).x < Grid.CellToPos(cell).x;
		return flag == flag2;
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x0008DA98 File Offset: 0x0008BC98
	private bool CanFleeTo(int cell)
	{
		return this.nav.CanReach(cell) || this.nav.CanReach(Grid.OffsetCell(cell, -1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, -1, 1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, 1));
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x0008DB07 File Offset: 0x0008BD07
	public GameObject CreateLocator(Vector3 pos)
	{
		return ChoreHelpers.CreateLocator("GoToLocator", pos);
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x0008DB14 File Offset: 0x0008BD14
	protected override void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (base.smi.sm.fleeToTarget.Get(base.smi) != null)
		{
			ChoreHelpers.DestroyLocator(base.smi.sm.fleeToTarget.Get(base.smi));
		}
		base.OnStateMachineStop(reason, status);
	}

	// Token: 0x04000ECA RID: 3786
	private Navigator nav;

	// Token: 0x020012E6 RID: 4838
	public class StatesInstance : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.GameInstance
	{
		// Token: 0x06008A07 RID: 35335 RVA: 0x00356C36 File Offset: 0x00354E36
		public StatesInstance(FleeChore master) : base(master)
		{
		}
	}

	// Token: 0x020012E7 RID: 4839
	public class States : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore>
	{
		// Token: 0x06008A08 RID: 35336 RVA: 0x00356C40 File Offset: 0x00354E40
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.planFleeRoute;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fleeing, null);
			this.planFleeRoute.Enter(delegate(FleeChore.StatesInstance smi)
			{
				int num = Grid.PosToCell(this.fleeFromTarget.Get(smi));
				HashSet<int> hashSet = GameUtil.FloodCollectCells(Grid.PosToCell(smi.master.gameObject), new Func<int, bool>(smi.master.CanFleeTo), 300, null, true);
				int num2 = -1;
				int num3 = -1;
				foreach (int num4 in hashSet)
				{
					if (smi.master.nav.CanReach(num4))
					{
						int num5 = -1;
						num5 += Grid.GetCellDistance(num4, num);
						if (smi.master.isInFavoredDirection(num4, num))
						{
							num5 += 8;
						}
						if (num5 > num3)
						{
							num3 = num5;
							num2 = num4;
						}
					}
				}
				int num6 = num2;
				if (num6 == -1)
				{
					smi.GoTo(this.cower);
					return;
				}
				smi.sm.fleeToTarget.Set(smi.master.CreateLocator(Grid.CellToPos(num6)), smi, false);
				smi.sm.fleeToTarget.Get(smi).name = "FleeLocator";
				if (num6 == num)
				{
					smi.GoTo(this.cower);
					return;
				}
				smi.GoTo(this.flee);
			});
			this.flee.InitializeStates(this.self, this.fleeToTarget, this.cower, this.cower, null, NavigationTactics.ReduceTravelDistance).ToggleAnims("anim_loco_run_insane_kanim", 2f);
			this.cower.ToggleAnims("anim_cringe_kanim", 4f).PlayAnim("cringe_pre").QueueAnim("cringe_loop", false, null).QueueAnim("cringe_pst", false, null).OnAnimQueueComplete(this.end);
			this.end.Enter(delegate(FleeChore.StatesInstance smi)
			{
				smi.StopSM("stopped");
			});
		}

		// Token: 0x0400698D RID: 27021
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeFromTarget;

		// Token: 0x0400698E RID: 27022
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeToTarget;

		// Token: 0x0400698F RID: 27023
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter self;

		// Token: 0x04006990 RID: 27024
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State planFleeRoute;

		// Token: 0x04006991 RID: 27025
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.ApproachSubState<IApproachable> flee;

		// Token: 0x04006992 RID: 27026
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State cower;

		// Token: 0x04006993 RID: 27027
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State end;
	}
}
