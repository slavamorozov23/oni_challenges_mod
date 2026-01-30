using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
[SerializationConfig(MemberSerialization.OptIn)]
public class OilRefinery : StateMachineComponent<OilRefinery.StatesInstance>
{
	// Token: 0x06003505 RID: 13573 RVA: 0x0012C4D8 File Offset: 0x0012A6D8
	protected override void OnSpawn()
	{
		base.Subscribe<OilRefinery>(-1697596308, OilRefinery.OnStorageChangedDelegate);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, null);
		base.smi.StartSM();
		this.maxSrcMass = base.GetComponent<ConduitConsumer>().capacityKG;
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x0012C538 File Offset: 0x0012A738
	private void OnStorageChanged(object data)
	{
		float positionPercent = Mathf.Clamp01(this.storage.GetMassAvailable(SimHashes.CrudeOil) / this.maxSrcMass);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x0012C570 File Offset: 0x0012A770
	private static bool UpdateStateCb(int cell, object data)
	{
		OilRefinery oilRefinery = data as OilRefinery;
		if (Grid.Element[cell].IsGas)
		{
			oilRefinery.cellCount += 1f;
			oilRefinery.envPressure += Grid.Mass[cell];
		}
		return true;
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x0012C5C0 File Offset: 0x0012A7C0
	private void TestAreaPressure()
	{
		this.envPressure = 0f;
		this.cellCount = 0f;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, new Func<int, object, bool>(OilRefinery.UpdateStateCb));
			this.envPressure /= this.cellCount;
		}
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x0012C636 File Offset: 0x0012A836
	private bool IsOverPressure()
	{
		return this.envPressure >= this.overpressureMass;
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x0012C649 File Offset: 0x0012A849
	private bool IsOverWarningPressure()
	{
		return this.envPressure >= this.overpressureWarningMass;
	}

	// Token: 0x04002011 RID: 8209
	private bool wasOverPressure;

	// Token: 0x04002012 RID: 8210
	[SerializeField]
	public float overpressureWarningMass = 4.5f;

	// Token: 0x04002013 RID: 8211
	[SerializeField]
	public float overpressureMass = 5f;

	// Token: 0x04002014 RID: 8212
	private float maxSrcMass;

	// Token: 0x04002015 RID: 8213
	private float envPressure;

	// Token: 0x04002016 RID: 8214
	private float cellCount;

	// Token: 0x04002017 RID: 8215
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002018 RID: 8216
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002019 RID: 8217
	[MyCmpAdd]
	private OilRefinery.WorkableTarget workable;

	// Token: 0x0400201A RID: 8218
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x0400201B RID: 8219
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x0400201C RID: 8220
	private const bool hasMeter = true;

	// Token: 0x0400201D RID: 8221
	private MeterController meter;

	// Token: 0x0400201E RID: 8222
	private static readonly EventSystem.IntraObjectHandler<OilRefinery> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<OilRefinery>(delegate(OilRefinery component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x0200171E RID: 5918
	public class StatesInstance : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.GameInstance
	{
		// Token: 0x06009A0C RID: 39436 RVA: 0x0038F7CB File Offset: 0x0038D9CB
		public StatesInstance(OilRefinery smi) : base(smi)
		{
		}

		// Token: 0x06009A0D RID: 39437 RVA: 0x0038F7D4 File Offset: 0x0038D9D4
		public void TestAreaPressure()
		{
			base.smi.master.TestAreaPressure();
			bool flag = base.smi.master.IsOverPressure();
			bool flag2 = base.smi.master.IsOverWarningPressure();
			if (flag)
			{
				base.smi.master.wasOverPressure = true;
				base.sm.isOverPressure.Set(true, this, false);
				return;
			}
			if (base.smi.master.wasOverPressure && !flag2)
			{
				base.sm.isOverPressure.Set(false, this, false);
			}
		}
	}

	// Token: 0x0200171F RID: 5919
	public class States : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery>
	{
		// Token: 0x06009A0E RID: 39438 RVA: 0x0038F864 File Offset: 0x0038DA64
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OilRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.needResources, (OilRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.needResources.EventTransition(GameHashes.OnStorageChange, this.ready, (OilRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.ready.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.overpressure, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsTrue).Transition(this.needResources, (OilRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false), UpdateRate.SIM_200ms).ToggleChore((OilRefinery.StatesInstance smi) => new WorkChore<OilRefinery.WorkableTarget>(Db.Get().ChoreTypes.Fabricate, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), new Action<OilRefinery.StatesInstance, Chore>(OilRefinery.States.SetRemoteChore), this.needResources);
			this.overpressure.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.ready, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null);
		}

		// Token: 0x06009A0F RID: 39439 RVA: 0x0038FA21 File Offset: 0x0038DC21
		private static void SetRemoteChore(OilRefinery.StatesInstance smi, Chore chore)
		{
			smi.master.remoteChore.SetChore(chore);
		}

		// Token: 0x040076F1 RID: 30449
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressure;

		// Token: 0x040076F2 RID: 30450
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressureWarning;

		// Token: 0x040076F3 RID: 30451
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State disabled;

		// Token: 0x040076F4 RID: 30452
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State overpressure;

		// Token: 0x040076F5 RID: 30453
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State needResources;

		// Token: 0x040076F6 RID: 30454
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State ready;
	}

	// Token: 0x02001720 RID: 5920
	[AddComponentMenu("KMonoBehaviour/Workable/WorkableTarget")]
	public class WorkableTarget : Workable
	{
		// Token: 0x06009A11 RID: 39441 RVA: 0x0038FA3C File Offset: 0x0038DC3C
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.showProgressBar = false;
			this.workerStatusItem = null;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_oilrefinery_kanim")
			};
		}

		// Token: 0x06009A12 RID: 39442 RVA: 0x0038FAA0 File Offset: 0x0038DCA0
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x06009A13 RID: 39443 RVA: 0x0038FAB3 File Offset: 0x0038DCB3
		protected override void OnStartWork(WorkerBase worker)
		{
			this.operational.SetActive(true, false);
		}

		// Token: 0x06009A14 RID: 39444 RVA: 0x0038FAC2 File Offset: 0x0038DCC2
		protected override void OnStopWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06009A15 RID: 39445 RVA: 0x0038FAD1 File Offset: 0x0038DCD1
		protected override void OnCompleteWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06009A16 RID: 39446 RVA: 0x0038FAE0 File Offset: 0x0038DCE0
		public override bool InstantlyFinish(WorkerBase worker)
		{
			return false;
		}

		// Token: 0x040076F7 RID: 30455
		[MyCmpGet]
		public Operational operational;
	}
}
