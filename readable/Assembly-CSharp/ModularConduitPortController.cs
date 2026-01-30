using System;

// Token: 0x020007C6 RID: 1990
public class ModularConduitPortController : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>
{
	// Token: 0x060034AA RID: 13482 RVA: 0x0012A8EC File Offset: 0x00128AEC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		ModularConduitPortController.InitializeStatusItems();
		this.off.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on, (ModularConduitPortController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (ModularConduitPortController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.on.idle.PlayAnim("idle").ParamTransition<bool>(this.hasRocket, this.on.finished, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.idleStatusItem, null);
		this.on.finished.PlayAnim("finished", KAnim.PlayMode.Loop).ParamTransition<bool>(this.hasRocket, this.on.idle, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.isUnloading, this.on.unloading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ParamTransition<bool>(this.isLoading, this.on.loading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.loadedStatusItem, null);
		this.on.unloading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("unloading_pre").QueueAnim("unloading_loop", true, null).ParamTransition<bool>(this.isUnloading, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.unloadingStatusItem, null);
		this.on.unloading_pst.PlayAnim("unloading_pst").OnAnimQueueComplete(this.on.finished);
		this.on.loading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("loading_pre").QueueAnim("loading_loop", true, null).ParamTransition<bool>(this.isLoading, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.loadingStatusItem, null);
		this.on.loading_pst.PlayAnim("loading_pst").OnAnimQueueComplete(this.on.finished);
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x0012ABF4 File Offset: 0x00128DF4
	private static void InitializeStatusItems()
	{
		if (ModularConduitPortController.idleStatusItem == null)
		{
			ModularConduitPortController.idleStatusItem = new StatusItem("ROCKET_PORT_IDLE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.unloadingStatusItem = new StatusItem("ROCKET_PORT_UNLOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadingStatusItem = new StatusItem("ROCKET_PORT_LOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadedStatusItem = new StatusItem("ROCKET_PORT_LOADED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	// Token: 0x04001FCA RID: 8138
	private GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State off;

	// Token: 0x04001FCB RID: 8139
	private ModularConduitPortController.OnStates on;

	// Token: 0x04001FCC RID: 8140
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isUnloading;

	// Token: 0x04001FCD RID: 8141
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isLoading;

	// Token: 0x04001FCE RID: 8142
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter hasRocket;

	// Token: 0x04001FCF RID: 8143
	private static StatusItem idleStatusItem;

	// Token: 0x04001FD0 RID: 8144
	private static StatusItem unloadingStatusItem;

	// Token: 0x04001FD1 RID: 8145
	private static StatusItem loadingStatusItem;

	// Token: 0x04001FD2 RID: 8146
	private static StatusItem loadedStatusItem;

	// Token: 0x0200170E RID: 5902
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040076C4 RID: 30404
		public ModularConduitPortController.Mode mode;
	}

	// Token: 0x0200170F RID: 5903
	public enum Mode
	{
		// Token: 0x040076C6 RID: 30406
		Unload,
		// Token: 0x040076C7 RID: 30407
		Both,
		// Token: 0x040076C8 RID: 30408
		Load
	}

	// Token: 0x02001710 RID: 5904
	private class OnStates : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State
	{
		// Token: 0x040076C9 RID: 30409
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State idle;

		// Token: 0x040076CA RID: 30410
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading;

		// Token: 0x040076CB RID: 30411
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading_pst;

		// Token: 0x040076CC RID: 30412
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading;

		// Token: 0x040076CD RID: 30413
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading_pst;

		// Token: 0x040076CE RID: 30414
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State finished;
	}

	// Token: 0x02001711 RID: 5905
	public new class Instance : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.GameInstance
	{
		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x060099DB RID: 39387 RVA: 0x0038F1A7 File Offset: 0x0038D3A7
		public ModularConduitPortController.Mode SelectedMode
		{
			get
			{
				return base.def.mode;
			}
		}

		// Token: 0x060099DC RID: 39388 RVA: 0x0038F1B4 File Offset: 0x0038D3B4
		public Instance(IStateMachineTarget master, ModularConduitPortController.Def def) : base(master, def)
		{
		}

		// Token: 0x060099DD RID: 39389 RVA: 0x0038F1BE File Offset: 0x0038D3BE
		public ConduitType GetConduitType()
		{
			return base.GetComponent<IConduitConsumer>().ConduitType;
		}

		// Token: 0x060099DE RID: 39390 RVA: 0x0038F1CB File Offset: 0x0038D3CB
		public void SetUnloading(bool isUnloading)
		{
			base.sm.isUnloading.Set(isUnloading, this, false);
		}

		// Token: 0x060099DF RID: 39391 RVA: 0x0038F1E1 File Offset: 0x0038D3E1
		public void SetLoading(bool isLoading)
		{
			base.sm.isLoading.Set(isLoading, this, false);
		}

		// Token: 0x060099E0 RID: 39392 RVA: 0x0038F1F7 File Offset: 0x0038D3F7
		public void SetRocket(bool hasRocket)
		{
			base.sm.hasRocket.Set(hasRocket, this, false);
		}

		// Token: 0x060099E1 RID: 39393 RVA: 0x0038F20D File Offset: 0x0038D40D
		public bool IsLoading()
		{
			return base.sm.isLoading.Get(this);
		}

		// Token: 0x040076CF RID: 30415
		[MyCmpGet]
		public Operational operational;
	}
}
