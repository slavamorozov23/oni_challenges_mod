using System;

// Token: 0x02000A40 RID: 2624
public class RocketPassengerMonitor : GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance>
{
	// Token: 0x06004C8C RID: 19596 RVA: 0x001BD1B8 File Offset: 0x001BB3B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.satisfied.ParamTransition<int>(this.targetCell, this.moving, (RocketPassengerMonitor.Instance smi, int p) => p != Grid.InvalidCell);
		this.moving.ParamTransition<int>(this.targetCell, this.satisfied, (RocketPassengerMonitor.Instance smi, int p) => p == Grid.InvalidCell).ToggleChore((RocketPassengerMonitor.Instance smi) => this.CreateChore(smi), this.satisfied).Exit(delegate(RocketPassengerMonitor.Instance smi)
		{
			this.targetCell.Set(Grid.InvalidCell, smi, false);
		});
		this.movingToModuleDeployPre.Enter(delegate(RocketPassengerMonitor.Instance smi)
		{
			this.targetCell.Set(smi.moduleDeployTaskTargetMoveCell, smi, false);
			smi.GoTo(this.movingToModuleDeploy);
		});
		this.movingToModuleDeploy.ParamTransition<int>(this.targetCell, this.satisfied, (RocketPassengerMonitor.Instance smi, int p) => p == Grid.InvalidCell).ToggleChore((RocketPassengerMonitor.Instance smi) => this.CreateChore(smi), this.moduleDeploy);
		this.moduleDeploy.Enter(delegate(RocketPassengerMonitor.Instance smi)
		{
			smi.moduleDeployCompleteCallback(null);
			this.targetCell.Set(Grid.InvalidCell, smi, false);
			smi.moduleDeployCompleteCallback = null;
			smi.GoTo(smi.sm.satisfied);
		});
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x001BD2E8 File Offset: 0x001BB4E8
	public Chore CreateChore(RocketPassengerMonitor.Instance smi)
	{
		MoveChore moveChore = new MoveChore(smi.master, Db.Get().ChoreTypes.RocketEnterExit, (MoveChore.StatesInstance mover_smi) => this.targetCell.Get(smi), false);
		moveChore.AddPrecondition(ChorePreconditions.instance.CanMoveToCell, this.targetCell.Get(smi));
		return moveChore;
	}

	// Token: 0x040032E8 RID: 13032
	public StateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.IntParameter targetCell = new StateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.IntParameter(Grid.InvalidCell);

	// Token: 0x040032E9 RID: 13033
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040032EA RID: 13034
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State moving;

	// Token: 0x040032EB RID: 13035
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State movingToModuleDeployPre;

	// Token: 0x040032EC RID: 13036
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State movingToModuleDeploy;

	// Token: 0x040032ED RID: 13037
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State moduleDeploy;

	// Token: 0x02001B25 RID: 6949
	public new class Instance : GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A89A RID: 43162 RVA: 0x003BF683 File Offset: 0x003BD883
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A89B RID: 43163 RVA: 0x003BF68C File Offset: 0x003BD88C
		public bool ShouldMoveThroughRocketDoor()
		{
			int num = base.sm.targetCell.Get(this);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if ((int)Grid.WorldIdx[num] == this.GetMyWorldId())
			{
				base.sm.targetCell.Set(Grid.InvalidCell, this, false);
				return false;
			}
			return true;
		}

		// Token: 0x0600A89C RID: 43164 RVA: 0x003BF6DF File Offset: 0x003BD8DF
		public void SetMoveTarget(int cell)
		{
			if ((int)Grid.WorldIdx[cell] == this.GetMyWorldId())
			{
				return;
			}
			base.sm.targetCell.Set(cell, this, false);
		}

		// Token: 0x0600A89D RID: 43165 RVA: 0x003BF705 File Offset: 0x003BD905
		public void SetModuleDeployChore(int cell, Action<Chore> OnChoreCompleteCallback)
		{
			this.moduleDeployCompleteCallback = OnChoreCompleteCallback;
			this.moduleDeployTaskTargetMoveCell = cell;
			this.GoTo(base.sm.movingToModuleDeployPre);
			base.sm.targetCell.Set(cell, this, false);
		}

		// Token: 0x0600A89E RID: 43166 RVA: 0x003BF73A File Offset: 0x003BD93A
		public void CancelModuleDeployChore()
		{
			this.moduleDeployCompleteCallback = null;
			this.moduleDeployTaskTargetMoveCell = Grid.InvalidCell;
			base.sm.targetCell.Set(Grid.InvalidCell, base.smi, false);
		}

		// Token: 0x0600A89F RID: 43167 RVA: 0x003BF76C File Offset: 0x003BD96C
		public void ClearMoveTarget(int testCell)
		{
			int num = base.sm.targetCell.Get(this);
			if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == Grid.WorldIdx[testCell])
			{
				base.sm.targetCell.Set(Grid.InvalidCell, this, false);
				if (base.IsInsideState(base.sm.moving))
				{
					this.GoTo(base.sm.satisfied);
				}
			}
		}

		// Token: 0x040083E4 RID: 33764
		public int lastWorldID;

		// Token: 0x040083E5 RID: 33765
		public Action<Chore> moduleDeployCompleteCallback;

		// Token: 0x040083E6 RID: 33766
		public int moduleDeployTaskTargetMoveCell;
	}
}
