using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class AggressiveChore : Chore<AggressiveChore.StatesInstance>
{
	// Token: 0x060018D3 RID: 6355 RVA: 0x00089D4C File Offset: 0x00087F4C
	public AggressiveChore(IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.StressActingOut, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new AggressiveChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x00089D93 File Offset: 0x00087F93
	public override void Cleanup()
	{
		base.Cleanup();
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x00089D9C File Offset: 0x00087F9C
	public void PunchWallDamage(float dt)
	{
		if (Grid.Solid[base.smi.sm.wallCellToBreak] && Grid.StrengthInfo[base.smi.sm.wallCellToBreak] < 100)
		{
			WorldDamage.Instance.ApplyDamage(base.smi.sm.wallCellToBreak, 0.06f * dt, base.smi.sm.wallCellToBreak, BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION);
		}
	}

	// Token: 0x020012B3 RID: 4787
	public class StatesInstance : GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.GameInstance
	{
		// Token: 0x0600891D RID: 35101 RVA: 0x0034FE41 File Offset: 0x0034E041
		public StatesInstance(AggressiveChore master, GameObject breaker) : base(master)
		{
			base.sm.breaker.Set(breaker, base.smi, false);
		}

		// Token: 0x0600891E RID: 35102 RVA: 0x0034FE64 File Offset: 0x0034E064
		public void FindBreakable()
		{
			Navigator navigator = base.GetComponent<Navigator>();
			int num = int.MaxValue;
			Breakable breakable = null;
			if (UnityEngine.Random.Range(0, 100) >= 50)
			{
				foreach (Breakable breakable2 in Components.Breakables.Items)
				{
					if (!(breakable2 == null) && !breakable2.IsInvincible && !breakable2.isBroken())
					{
						int navigationCost = navigator.GetNavigationCost(breakable2);
						if (navigationCost != -1 && navigationCost < num)
						{
							num = navigationCost;
							breakable = breakable2;
						}
					}
				}
			}
			if (breakable == null)
			{
				int value = GameUtil.FloodFillFind<object>((int cell, object arg) => !Grid.Solid[cell] && navigator.CanReach(cell) && ((Grid.IsValidCell(Grid.CellLeft(cell)) && Grid.Solid[Grid.CellLeft(cell)]) || (Grid.IsValidCell(Grid.CellRight(cell)) && Grid.Solid[Grid.CellRight(cell)]) || (Grid.IsValidCell(Grid.OffsetCell(cell, 1, 1)) && Grid.Solid[Grid.OffsetCell(cell, 1, 1)]) || (Grid.IsValidCell(Grid.OffsetCell(cell, -1, 1)) && Grid.Solid[Grid.OffsetCell(cell, -1, 1)])), null, Grid.PosToCell(navigator.gameObject), 128, true, true);
				base.sm.moveToWallTarget.Set(value, base.smi, false);
				this.GoTo(base.sm.move_notarget);
				return;
			}
			base.sm.breakable.Set(breakable, base.smi);
			this.GoTo(base.sm.move_target);
		}
	}

	// Token: 0x020012B4 RID: 4788
	public class States : GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore>
	{
		// Token: 0x0600891F RID: 35103 RVA: 0x0034FFA0 File Offset: 0x0034E1A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findbreakable;
			base.Target(this.breaker);
			this.root.ToggleAnims("anim_loco_destructive_kanim", 0f);
			this.noTarget.Enter(delegate(AggressiveChore.StatesInstance smi)
			{
				smi.StopSM("complete/no more food");
			});
			this.findbreakable.Enter("FindBreakable", delegate(AggressiveChore.StatesInstance smi)
			{
				smi.FindBreakable();
			});
			this.move_notarget.MoveTo((AggressiveChore.StatesInstance smi) => smi.sm.moveToWallTarget.Get(smi), this.breaking_wall, this.noTarget, false);
			this.move_target.InitializeStates(this.breaker, this.breakable, this.breaking, this.findbreakable, null, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.LashingOut, null);
			this.breaking_wall.DefaultState(this.breaking_wall.Pre).Enter(delegate(AggressiveChore.StatesInstance smi)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (Grid.Solid[Grid.OffsetCell(cell, 1, 0)])
				{
					smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_low_kanim"), 0f);
					int num = Grid.OffsetCell(cell, 1, 0);
					this.wallCellToBreak = num;
				}
				else if (Grid.Solid[Grid.OffsetCell(cell, -1, 0)])
				{
					smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_low_kanim"), 0f);
					int num2 = Grid.OffsetCell(cell, -1, 0);
					this.wallCellToBreak = num2;
				}
				else if (Grid.Solid[Grid.OffsetCell(cell, 1, 1)])
				{
					smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_high_kanim"), 0f);
					int num3 = Grid.OffsetCell(cell, 1, 1);
					this.wallCellToBreak = num3;
				}
				else if (Grid.Solid[Grid.OffsetCell(cell, -1, 1)])
				{
					smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_high_kanim"), 0f);
					int num4 = Grid.OffsetCell(cell, -1, 1);
					this.wallCellToBreak = num4;
				}
				smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.wallCellToBreak));
			}).Exit(delegate(AggressiveChore.StatesInstance smi)
			{
				smi.sm.masterTarget.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_high_kanim"));
				smi.sm.masterTarget.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_low_kanim"));
			});
			this.breaking_wall.Pre.PlayAnim("working_pre").OnAnimQueueComplete(this.breaking_wall.Loop);
			this.breaking_wall.Loop.ScheduleGoTo(26f, this.breaking_wall.Pst).Update("PunchWallDamage", delegate(AggressiveChore.StatesInstance smi, float dt)
			{
				smi.master.PunchWallDamage(dt);
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(AggressiveChore.StatesInstance smi)
			{
				smi.Play("working_loop", KAnim.PlayMode.Loop);
			}).Update(delegate(AggressiveChore.StatesInstance smi, float dt)
			{
				if (!Grid.Solid[smi.sm.wallCellToBreak])
				{
					smi.GoTo(this.breaking_wall.Pst);
				}
			}, UpdateRate.SIM_200ms, false);
			this.breaking_wall.Pst.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.noTarget);
			this.breaking.ToggleWork<Breakable>(this.breakable, null, null, null);
		}

		// Token: 0x04006899 RID: 26777
		public StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.TargetParameter breaker;

		// Token: 0x0400689A RID: 26778
		public StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.TargetParameter breakable;

		// Token: 0x0400689B RID: 26779
		public StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.IntParameter moveToWallTarget;

		// Token: 0x0400689C RID: 26780
		public int wallCellToBreak;

		// Token: 0x0400689D RID: 26781
		public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.ApproachSubState<Breakable> move_target;

		// Token: 0x0400689E RID: 26782
		public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State move_notarget;

		// Token: 0x0400689F RID: 26783
		public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State findbreakable;

		// Token: 0x040068A0 RID: 26784
		public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State noTarget;

		// Token: 0x040068A1 RID: 26785
		public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State breaking;

		// Token: 0x040068A2 RID: 26786
		public AggressiveChore.States.BreakingWall breaking_wall;

		// Token: 0x02002791 RID: 10129
		public class BreakingWall : GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State
		{
			// Token: 0x0400AF7E RID: 44926
			public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State Pre;

			// Token: 0x0400AF7F RID: 44927
			public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State Loop;

			// Token: 0x0400AF80 RID: 44928
			public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State Pst;
		}
	}
}
