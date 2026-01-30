using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02000AD7 RID: 2775
public class RemoteWorkerSM : StateMachineComponent<RemoteWorkerSM.StatesInstance>
{
	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x060050A3 RID: 20643 RVA: 0x001D3A08 File Offset: 0x001D1C08
	// (set) Token: 0x060050A4 RID: 20644 RVA: 0x001D3A10 File Offset: 0x001D1C10
	public bool Docked
	{
		get
		{
			return this.docked;
		}
		set
		{
			this.docked = value;
		}
	}

	// Token: 0x060050A5 RID: 20645 RVA: 0x001D3A19 File Offset: 0x001D1C19
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060050A6 RID: 20646 RVA: 0x001D3A2C File Offset: 0x001D1C2C
	public void SetNextChore(Chore.Precondition.Context next)
	{
		if (this.nextChore != null)
		{
			this.nextChore.Value.chore.Reserve(null);
		}
		this.nextChore = new Chore.Precondition.Context?(next);
		next.chore.Reserve(this.driver);
	}

	// Token: 0x060050A7 RID: 20647 RVA: 0x001D3A79 File Offset: 0x001D1C79
	public void StartNextChore()
	{
		if (this.nextChore != null)
		{
			this.driver.SetChore(this.nextChore.Value);
			this.nextChore = null;
		}
	}

	// Token: 0x060050A8 RID: 20648 RVA: 0x001D3AAA File Offset: 0x001D1CAA
	public bool HasChoreQueued()
	{
		return this.nextChore != null;
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x060050A9 RID: 20649 RVA: 0x001D3AB7 File Offset: 0x001D1CB7
	// (set) Token: 0x060050AA RID: 20650 RVA: 0x001D3ACA File Offset: 0x001D1CCA
	public RemoteWorkerDock HomeDepot
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.homeDepot;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			this.homeDepot = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x060050AB RID: 20651 RVA: 0x001D3AD8 File Offset: 0x001D1CD8
	public ChoreConsumerState ConsumerState
	{
		get
		{
			return this.consumer.consumerState;
		}
	}

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x060050AC RID: 20652 RVA: 0x001D3AE5 File Offset: 0x001D1CE5
	// (set) Token: 0x060050AD RID: 20653 RVA: 0x001D3AED File Offset: 0x001D1CED
	public bool ActivelyControlled { get; set; }

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x060050AE RID: 20654 RVA: 0x001D3AF6 File Offset: 0x001D1CF6
	// (set) Token: 0x060050AF RID: 20655 RVA: 0x001D3AFE File Offset: 0x001D1CFE
	public bool ActivelyWorking { get; set; }

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x060050B0 RID: 20656 RVA: 0x001D3B07 File Offset: 0x001D1D07
	// (set) Token: 0x060050B1 RID: 20657 RVA: 0x001D3B0F File Offset: 0x001D1D0F
	public bool Available { get; set; }

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x060050B2 RID: 20658 RVA: 0x001D3B18 File Offset: 0x001D1D18
	public bool RequiresMaintnence
	{
		get
		{
			return this.power.IsLowPower;
		}
	}

	// Token: 0x060050B3 RID: 20659 RVA: 0x001D3B28 File Offset: 0x001D1D28
	public void TickResources(float dt)
	{
		this.power.ApplyDeltaEnergy(-0.1f * dt);
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		this.storage.ConsumeAndGetDisease(GameTags.LubricatingOil, 0.033333335f * dt, out num, out diseaseInfo, out temperature);
		if (num > 0f)
		{
			this.storage.AddElement(SimHashes.LiquidGunk, num, temperature, diseaseInfo.idx, diseaseInfo.count, true, true);
		}
	}

	// Token: 0x060050B4 RID: 20660 RVA: 0x001D3B8E File Offset: 0x001D1D8E
	public GameObject FindStation()
	{
		if (Components.ComplexFabricators.Count == 0)
		{
			return null;
		}
		return Components.ComplexFabricators[0].gameObject;
	}

	// Token: 0x060050B5 RID: 20661 RVA: 0x001D3BAE File Offset: 0x001D1DAE
	public bool HasHomeDepot()
	{
		return !this.HomeDepot.IsNullOrDestroyed();
	}

	// Token: 0x040035CA RID: 13770
	[MyCmpAdd]
	private RemoteWorkerCapacitor power;

	// Token: 0x040035CB RID: 13771
	[MyCmpAdd]
	private RemoteWorkerGunkMonitor gunk;

	// Token: 0x040035CC RID: 13772
	[MyCmpAdd]
	private RemoteWorkerOilMonitor oil;

	// Token: 0x040035CD RID: 13773
	[MyCmpAdd]
	private ChoreDriver driver;

	// Token: 0x040035CE RID: 13774
	[MyCmpGet]
	private ChoreConsumer consumer;

	// Token: 0x040035CF RID: 13775
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040035D0 RID: 13776
	public bool playNewWorker;

	// Token: 0x040035D1 RID: 13777
	[Serialize]
	private bool docked = true;

	// Token: 0x040035D2 RID: 13778
	private Chore.Precondition.Context? nextChore;

	// Token: 0x040035D3 RID: 13779
	private const string LostAnim_pre = "sos_pre";

	// Token: 0x040035D4 RID: 13780
	private const string LostAnim_loop = "sos_loop";

	// Token: 0x040035D5 RID: 13781
	private const string LostAnim_pst = "sos_pst";

	// Token: 0x040035D6 RID: 13782
	private const string DeathAnim = "explode";

	// Token: 0x040035D7 RID: 13783
	[Serialize]
	private Ref<RemoteWorkerDock> homeDepot;

	// Token: 0x02001C19 RID: 7193
	public class StatesInstance : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.GameInstance
	{
		// Token: 0x0600AC74 RID: 44148 RVA: 0x003CC399 File Offset: 0x003CA599
		public StatesInstance(RemoteWorkerSM master) : base(master)
		{
			base.sm.homedock.Set(base.smi.master.HomeDepot, base.smi);
		}
	}

	// Token: 0x02001C1A RID: 7194
	public class States : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM>
	{
		// Token: 0x0600AC75 RID: 44149 RVA: 0x003CC3C8 File Offset: 0x003CA5C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.uncontrolled;
			this.root.Update(delegate(RemoteWorkerSM.StatesInstance smi, float dt)
			{
				smi.GetComponent<Navigator>().UpdateProbe(false);
			}, UpdateRate.SIM_4000ms, false);
			this.controlled.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			}).EnterTransition(this.controlled.exit_dock, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)).EnterTransition(this.controlled.working, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock))).Transition(this.uncontrolled, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator)), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms).Update(new Action<RemoteWorkerSM.StatesInstance, float>(RemoteWorkerSM.States.TickResources), UpdateRate.SIM_200ms, false);
			this.controlled.exit_dock.ToggleWork<RemoteWorkerDock.ExitableDock>(this.homedock, this.controlled.working, this.controlled.working, (RemoteWorkerSM.StatesInstance _) => true);
			this.controlled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = true;
			}).Exit(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = false;
			}).DefaultState(this.controlled.working.find_work);
			this.controlled.working.find_work.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				if (RemoteWorkerSM.States.HasChore(smi))
				{
					smi.GoTo(this.controlled.working.do_work);
					return;
				}
				RemoteWorkerSM.States.SetNextChore(smi);
				smi.GoTo(RemoteWorkerSM.States.HasChore(smi) ? this.controlled.working.do_work : this.controlled.no_work);
			});
			this.controlled.working.do_work.Exit(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).Transition(this.controlled.working.find_work, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore)), UpdateRate.SIM_200ms);
			this.controlled.no_work.Transition(this.controlled.working.do_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore), UpdateRate.SIM_200ms).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChoreQueued), UpdateRate.SIM_200ms);
			this.uncontrolled.EnterTransition(this.uncontrolled.working.new_worker, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)).EnterTransition(this.uncontrolled.idle, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock), GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)))).EnterTransition(this.uncontrolled.approach_dock, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)), GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)))).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.uncontrolled.approach_dock.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).MoveTo<IApproachable>(this.homedock, this.uncontrolled.working.enter, this.incapacitated.lost, null, null);
			this.uncontrolled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			});
			this.uncontrolled.working.new_worker.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.playNewWorker = false;
			}).ToggleWork<RemoteWorkerDock.NewWorker>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.working.recharge, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.enter.ToggleWork<RemoteWorkerDock.EnterableDock>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge.ToggleWork<RemoteWorkerDock.WorkerRecharger>(this.homedock, this.uncontrolled.working.recharge_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge_pst.OnAnimQueueComplete(this.uncontrolled.working.drain_gunk).ScheduleGoTo(1f, this.uncontrolled.working.drain_gunk);
			this.uncontrolled.working.drain_gunk.ToggleWork<RemoteWorkerDock.WorkerGunkRemover>(this.homedock, this.uncontrolled.working.drain_gunk_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.drain_gunk_pst.OnAnimQueueComplete(this.uncontrolled.working.fill_oil).ScheduleGoTo(1f, this.uncontrolled.working.fill_oil);
			this.uncontrolled.working.fill_oil.ToggleWork<RemoteWorkerDock.WorkerOilRefiller>(this.homedock, this.uncontrolled.working.fill_oil_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.fill_oil_pst.OnAnimQueueComplete(this.uncontrolled.idle).ScheduleGoTo(1f, this.uncontrolled.idle);
			this.uncontrolled.idle.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).PlayAnim(RemoteWorkerConfig.IDLE_IN_DOCK_ANIM, KAnim.PlayMode.Loop).Transition(this.uncontrolled.working.recharge, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.RequiresMaintnence), new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.DockIsOperational)), UpdateRate.SIM_1000ms);
			this.incapacitated.lost.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.Play("sos_pre", KAnim.PlayMode.Once);
				smi.Queue("sos_loop", KAnim.PlayMode.Loop);
				RemoteWorkerSM.States.ClearChore(smi);
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.UnreachableDock, null).Transition(this.incapacitated.lost_recovery, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.incapacitated.lost_recovery.PlayAnim("sos_pst").OnAnimQueueComplete(this.controlled);
			this.incapacitated.die.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).PlayAnim("explode").OnAnimQueueComplete(this.incapacitated.explode).ToggleStatusItem(Db.Get().DuplicantStatusItems.NoHomeDock, null);
			this.incapacitated.explode.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.Explode));
		}

		// Token: 0x0600AC76 RID: 44150 RVA: 0x003CCBDE File Offset: 0x003CADDE
		public static bool IsNewWorker(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.playNewWorker;
		}

		// Token: 0x0600AC77 RID: 44151 RVA: 0x003CCBEB File Offset: 0x003CADEB
		public static void SetNextChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.StartNextChore();
		}

		// Token: 0x0600AC78 RID: 44152 RVA: 0x003CCBF8 File Offset: 0x003CADF8
		public static void ClearChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.driver.StopChore();
		}

		// Token: 0x0600AC79 RID: 44153 RVA: 0x003CCC0A File Offset: 0x003CAE0A
		public static bool HasChore(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.driver.HasChore();
		}

		// Token: 0x0600AC7A RID: 44154 RVA: 0x003CCC1C File Offset: 0x003CAE1C
		public static bool HasChoreQueued(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HasChoreQueued();
		}

		// Token: 0x0600AC7B RID: 44155 RVA: 0x003CCC2C File Offset: 0x003CAE2C
		public static bool CanReachDepot(RemoteWorkerSM.StatesInstance smi)
		{
			int depotCell = RemoteWorkerSM.States.GetDepotCell(smi);
			return depotCell != Grid.InvalidCell && smi.master.GetComponent<Navigator>().CanReach(depotCell);
		}

		// Token: 0x0600AC7C RID: 44156 RVA: 0x003CCC5C File Offset: 0x003CAE5C
		public static int GetDepotCell(RemoteWorkerSM.StatesInstance smi)
		{
			RemoteWorkerDock homeDepot = smi.master.HomeDepot;
			if (homeDepot == null)
			{
				return Grid.InvalidCell;
			}
			return Grid.PosToCell(homeDepot);
		}

		// Token: 0x0600AC7D RID: 44157 RVA: 0x003CCC8A File Offset: 0x003CAE8A
		public static bool HasRemoteOperator(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.ActivelyControlled;
		}

		// Token: 0x0600AC7E RID: 44158 RVA: 0x003CCC97 File Offset: 0x003CAE97
		public static bool RequiresMaintnence(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.RequiresMaintnence;
		}

		// Token: 0x0600AC7F RID: 44159 RVA: 0x003CCCA4 File Offset: 0x003CAEA4
		public static bool DockIsOperational(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HomeDepot != null && smi.master.HomeDepot.IsOperational;
		}

		// Token: 0x0600AC80 RID: 44160 RVA: 0x003CCCCB File Offset: 0x003CAECB
		public static bool HasHomeDepot(RemoteWorkerSM.StatesInstance smi)
		{
			return RemoteWorkerSM.States.GetDepotCell(smi) != Grid.InvalidCell;
		}

		// Token: 0x0600AC81 RID: 44161 RVA: 0x003CCCDD File Offset: 0x003CAEDD
		public static void StopWork(RemoteWorkerSM.StatesInstance smi)
		{
			if (smi.master.driver.HasChore())
			{
				smi.master.driver.StopChore();
			}
		}

		// Token: 0x0600AC82 RID: 44162 RVA: 0x003CCD01 File Offset: 0x003CAF01
		public static bool IsInsideDock(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.Docked;
		}

		// Token: 0x0600AC83 RID: 44163 RVA: 0x003CCD10 File Offset: 0x003CAF10
		public static void Explode(RemoteWorkerSM.StatesInstance smi)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0f);
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			component.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), 42f, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, false, false);
			Util.KDestroyGameObject(smi.master.gameObject);
		}

		// Token: 0x0600AC84 RID: 44164 RVA: 0x003CCD9F File Offset: 0x003CAF9F
		public static void TickResources(RemoteWorkerSM.StatesInstance smi, float dt)
		{
			if (dt > 0f)
			{
				smi.master.TickResources(dt);
			}
		}

		// Token: 0x040086E5 RID: 34533
		public RemoteWorkerSM.States.ControlledStates controlled;

		// Token: 0x040086E6 RID: 34534
		public RemoteWorkerSM.States.UncontrolledStates uncontrolled;

		// Token: 0x040086E7 RID: 34535
		public RemoteWorkerSM.States.IncapacitatedStates incapacitated;

		// Token: 0x040086E8 RID: 34536
		public StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.TargetParameter homedock;

		// Token: 0x02002A12 RID: 10770
		public class ControlledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x0400B9FA RID: 47610
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State exit_dock;

			// Token: 0x0400B9FB RID: 47611
			public RemoteWorkerSM.States.ControlledStates.WorkingStates working;

			// Token: 0x0400B9FC RID: 47612
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State no_work;

			// Token: 0x02003A52 RID: 14930
			public class WorkingStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x0400EB8D RID: 60301
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State find_work;

				// Token: 0x0400EB8E RID: 60302
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State do_work;
			}
		}

		// Token: 0x02002A13 RID: 10771
		public class UncontrolledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x0400B9FD RID: 47613
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State approach_dock;

			// Token: 0x0400B9FE RID: 47614
			public RemoteWorkerSM.States.UncontrolledStates.WorkingDockStates working;

			// Token: 0x0400B9FF RID: 47615
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State idle;

			// Token: 0x02003A53 RID: 14931
			public class WorkingDockStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x0400EB8F RID: 60303
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State new_worker;

				// Token: 0x0400EB90 RID: 60304
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State enter;

				// Token: 0x0400EB91 RID: 60305
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge;

				// Token: 0x0400EB92 RID: 60306
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge_pst;

				// Token: 0x0400EB93 RID: 60307
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk;

				// Token: 0x0400EB94 RID: 60308
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk_pst;

				// Token: 0x0400EB95 RID: 60309
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil;

				// Token: 0x0400EB96 RID: 60310
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil_pst;
			}
		}

		// Token: 0x02002A14 RID: 10772
		public class IncapacitatedStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x0400BA00 RID: 47616
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost;

			// Token: 0x0400BA01 RID: 47617
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost_recovery;

			// Token: 0x0400BA02 RID: 47618
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State die;

			// Token: 0x0400BA03 RID: 47619
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State explode;
		}
	}
}
