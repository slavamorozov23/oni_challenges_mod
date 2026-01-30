using System;

// Token: 0x02000723 RID: 1827
public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
	// Token: 0x06002DEE RID: 11758 RVA: 0x0010AFEA File Offset: 0x001091EA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001B4D RID: 6989
	[MyCmpGet]
	private ComplexFabricator fabricator;

	// Token: 0x04001B4E RID: 6990
	public StatusItem idleQueue_StatusItem = Db.Get().BuildingStatusItems.FabricatorIdle;

	// Token: 0x04001B4F RID: 6991
	public StatusItem waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FabricatorEmpty;

	// Token: 0x04001B50 RID: 6992
	public StatusItem waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.PendingWork;

	// Token: 0x04001B51 RID: 6993
	public string idleAnimationName = "off";

	// Token: 0x020015FF RID: 5631
	public class StatesInstance : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.GameInstance
	{
		// Token: 0x0600959A RID: 38298 RVA: 0x0037C67F File Offset: 0x0037A87F
		public StatesInstance(ComplexFabricatorSM master) : base(master)
		{
		}
	}

	// Token: 0x02001600 RID: 5632
	public class States : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM>
	{
		// Token: 0x0600959B RID: 38299 RVA: 0x0037C688 File Offset: 0x0037A888
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.idle, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.DefaultState(this.idle.idleQueue).PlayAnim(new Func<ComplexFabricatorSM.StatesInstance, string>(ComplexFabricatorSM.States.GetIdleAnimName), KAnim.PlayMode.Once).EventTransition(GameHashes.OperationalChanged, this.off, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.idle.idleQueue.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.idleQueue_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForMaterial, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.HasAnyOrder);
			this.idle.waitingForMaterial.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForMaterial_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.HasAnyOrder).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForWorker, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.WaitingForWorker).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.idle.waitingForWorker.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForWorker_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.WaitingForWorker).EnterTransition(this.operating, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.duplicantOperated).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.operating.DefaultState(this.operating.working_pre).ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.workingStatusItem, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<ComplexFabricator>());
			this.operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operating.working_loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.Workable, this.operating.working_pst_complete).OnAnimQueueComplete(this.idle);
			this.operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x0600959C RID: 38300 RVA: 0x0037CB44 File Offset: 0x0037AD44
		public void RefreshHEPStatus(ComplexFabricatorSM.StatesInstance smi)
		{
			if (smi.master.GetComponent<HighEnergyParticleStorage>() != null && smi.master.fabricator.NeedsMoreHEPForQueuedRecipe())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, smi.master.fabricator);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, false);
		}

		// Token: 0x0600959D RID: 38301 RVA: 0x0037CBC3 File Offset: 0x0037ADC3
		public static string GetIdleAnimName(ComplexFabricatorSM.StatesInstance smi)
		{
			return smi.master.idleAnimationName;
		}

		// Token: 0x04007386 RID: 29574
		public ComplexFabricatorSM.States.IdleStates off;

		// Token: 0x04007387 RID: 29575
		public ComplexFabricatorSM.States.IdleStates idle;

		// Token: 0x04007388 RID: 29576
		public ComplexFabricatorSM.States.OperatingStates operating;

		// Token: 0x020028D0 RID: 10448
		public class IdleStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x0400B3D3 RID: 46035
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State idleQueue;

			// Token: 0x0400B3D4 RID: 46036
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForMaterial;

			// Token: 0x0400B3D5 RID: 46037
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForWorker;
		}

		// Token: 0x020028D1 RID: 10449
		public class OperatingStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x0400B3D6 RID: 46038
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pre;

			// Token: 0x0400B3D7 RID: 46039
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_loop;

			// Token: 0x0400B3D8 RID: 46040
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst;

			// Token: 0x0400B3D9 RID: 46041
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst_complete;
		}
	}
}
