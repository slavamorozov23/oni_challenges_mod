using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008FC RID: 2300
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStation")]
public class DoctorStation : Workable
{
	// Token: 0x06003FDC RID: 16348 RVA: 0x001678F8 File Offset: 0x00165AF8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003FDD RID: 16349 RVA: 0x00167900 File Offset: 0x00165B00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.doctor_workable = base.GetComponent<DoctorStationDoctorWorkable>();
		base.SetWorkTime(float.PositiveInfinity);
		this.smi = new DoctorStation.StatesInstance(this);
		this.smi.StartSM();
		this.OnStorageChange(null);
		base.Subscribe<DoctorStation>(-1697596308, DoctorStation.OnStorageChangeDelegate);
	}

	// Token: 0x06003FDE RID: 16350 RVA: 0x00167964 File Offset: 0x00165B64
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
			this.smi = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x00167998 File Offset: 0x00165B98
	private void OnStorageChange(object data = null)
	{
		this.treatments_available.Clear();
		foreach (GameObject gameObject in this.storage.items)
		{
			MedicinalPill component = gameObject.GetComponent<MedicinalPill>();
			if (component != null)
			{
				Tag tag = gameObject.PrefabID();
				foreach (string id in component.info.curedSicknesses)
				{
					this.AddTreatment(id, tag);
				}
			}
		}
		bool value = this.treatments_available.Count > 0;
		this.smi.sm.hasSupplies.Set(value, this.smi, false);
	}

	// Token: 0x06003FE0 RID: 16352 RVA: 0x00167A88 File Offset: 0x00165C88
	private void AddTreatment(string id, Tag tag)
	{
		if (!this.treatments_available.ContainsKey(id))
		{
			this.treatments_available.Add(id, tag);
		}
	}

	// Token: 0x06003FE1 RID: 16353 RVA: 0x00167AAF File Offset: 0x00165CAF
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.hasPatient.Set(true, this.smi, false);
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x00167AD6 File Offset: 0x00165CD6
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.hasPatient.Set(false, this.smi, false);
	}

	// Token: 0x06003FE3 RID: 16355 RVA: 0x00167AFD File Offset: 0x00165CFD
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x00167B00 File Offset: 0x00165D00
	public void SetHasDoctor(bool has)
	{
		this.smi.sm.hasDoctor.Set(has, this.smi, false);
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x00167B20 File Offset: 0x00165D20
	public void CompleteDoctoring()
	{
		if (!base.worker)
		{
			return;
		}
		this.CompleteDoctoring(base.worker.gameObject);
	}

	// Token: 0x06003FE6 RID: 16358 RVA: 0x00167B44 File Offset: 0x00165D44
	private void CompleteDoctoring(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			bool flag = false;
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					Game.Instance.savedInfo.curedDisease = true;
					sicknessInstance.Cure();
					this.storage.ConsumeIgnoringDisease(tag, 1f);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				global::Debug.LogWarningFormat(base.gameObject, "Failed to treat any disease for {0}", new object[]
				{
					target
				});
			}
		}
	}

	// Token: 0x06003FE7 RID: 16359 RVA: 0x00167BF8 File Offset: 0x00165DF8
	public bool IsDoctorAvailable(GameObject target)
	{
		if (!string.IsNullOrEmpty(this.doctor_workable.requiredSkillPerk))
		{
			MinionResume component = target.GetComponent<MinionResume>();
			if (!MinionResume.AnyOtherMinionHasPerk(this.doctor_workable.requiredSkillPerk, component))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003FE8 RID: 16360 RVA: 0x00167C34 File Offset: 0x00165E34
	public bool IsTreatmentAvailable(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04002789 RID: 10121
	private static readonly EventSystem.IntraObjectHandler<DoctorStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DoctorStation>(delegate(DoctorStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0400278A RID: 10122
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400278B RID: 10123
	[MyCmpReq]
	public Operational operational;

	// Token: 0x0400278C RID: 10124
	private DoctorStationDoctorWorkable doctor_workable;

	// Token: 0x0400278D RID: 10125
	private Dictionary<HashedString, Tag> treatments_available = new Dictionary<HashedString, Tag>();

	// Token: 0x0400278E RID: 10126
	private DoctorStation.StatesInstance smi;

	// Token: 0x0400278F RID: 10127
	public static readonly Chore.Precondition TreatmentAvailable = new Chore.Precondition
	{
		id = "TreatmentAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.TREATMENT_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsTreatmentAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x04002790 RID: 10128
	public static readonly Chore.Precondition DoctorAvailable = new Chore.Precondition
	{
		id = "DoctorAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsDoctorAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x020018F7 RID: 6391
	public class States : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation>
	{
		// Token: 0x0600A0E0 RID: 41184 RVA: 0x003AAA80 File Offset: 0x003A8C80
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DoctorStation.StatesInstance smi) => smi.master.operational.IsOperational);
			this.operational.EventTransition(GameHashes.OperationalChanged, this.unoperational, (DoctorStation.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.operational.not_ready);
			this.operational.not_ready.ParamTransition<bool>(this.hasSupplies, this.operational.ready, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.DefaultState(this.operational.ready.idle).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreatePatientChore), null).ParamTransition<bool>(this.hasSupplies, this.operational.not_ready, (DoctorStation.StatesInstance smi, bool p) => !p);
			this.operational.ready.idle.ParamTransition<bool>(this.hasPatient, this.operational.ready.has_patient, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.ParamTransition<bool>(this.hasPatient, this.operational.ready.idle, (DoctorStation.StatesInstance smi, bool p) => !p).DefaultState(this.operational.ready.has_patient.waiting).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreateDoctorChore), null);
			this.operational.ready.has_patient.waiting.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.being_treated, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.being_treated.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.waiting, (DoctorStation.StatesInstance smi, bool p) => !p).Enter(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x0600A0E1 RID: 41185 RVA: 0x003AAD78 File Offset: 0x003A8F78
		private Chore CreatePatientChore(DoctorStation.StatesInstance smi)
		{
			WorkChore<DoctorStation> workChore = new WorkChore<DoctorStation>(Db.Get().ChoreTypes.GetDoctored, smi.master, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
			workChore.AddPrecondition(DoctorStation.TreatmentAvailable, smi.master);
			workChore.AddPrecondition(DoctorStation.DoctorAvailable, smi.master);
			return workChore;
		}

		// Token: 0x0600A0E2 RID: 41186 RVA: 0x003AADD4 File Offset: 0x003A8FD4
		private Chore CreateDoctorChore(DoctorStation.StatesInstance smi)
		{
			DoctorStationDoctorWorkable component = smi.master.GetComponent<DoctorStationDoctorWorkable>();
			return new WorkChore<DoctorStationDoctorWorkable>(Db.Get().ChoreTypes.Doctor, component, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x04007C8C RID: 31884
		public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State unoperational;

		// Token: 0x04007C8D RID: 31885
		public DoctorStation.States.OperationalStates operational;

		// Token: 0x04007C8E RID: 31886
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasSupplies;

		// Token: 0x04007C8F RID: 31887
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasPatient;

		// Token: 0x04007C90 RID: 31888
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasDoctor;

		// Token: 0x0200299A RID: 10650
		public class OperationalStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x0400B800 RID: 47104
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State not_ready;

			// Token: 0x0400B801 RID: 47105
			public DoctorStation.States.ReadyStates ready;
		}

		// Token: 0x0200299B RID: 10651
		public class ReadyStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x0400B802 RID: 47106
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State idle;

			// Token: 0x0400B803 RID: 47107
			public DoctorStation.States.PatientStates has_patient;
		}

		// Token: 0x0200299C RID: 10652
		public class PatientStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x0400B804 RID: 47108
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State waiting;

			// Token: 0x0400B805 RID: 47109
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State being_treated;
		}
	}

	// Token: 0x020018F8 RID: 6392
	public class StatesInstance : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.GameInstance
	{
		// Token: 0x0600A0E4 RID: 41188 RVA: 0x003AAE1B File Offset: 0x003A901B
		public StatesInstance(DoctorStation master) : base(master)
		{
		}
	}
}
