using System;
using STRINGS;

// Token: 0x020004C5 RID: 1221
public class TakeMedicineChore : Chore<TakeMedicineChore.StatesInstance>
{
	// Token: 0x060019B8 RID: 6584 RVA: 0x0008FC84 File Offset: 0x0008DE84
	public TakeMedicineChore(MedicinalPillWorkable master) : base(Db.Get().ChoreTypes.TakeMedicine, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.medicine = master;
		this.pickupable = this.medicine.GetComponent<Pickupable>();
		base.smi = new TakeMedicineChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.CanPickup, this.pickupable);
		this.AddPrecondition(TakeMedicineChore.CanCure, this);
		this.AddPrecondition(TakeMedicineChore.IsConsumptionPermitted, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x0008FD18 File Offset: 0x0008DF18
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.source.Set(this.pickupable.gameObject, base.smi, false);
		base.smi.sm.requestedpillcount.Set(1f, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		new TakeMedicineChore(this.medicine);
	}

	// Token: 0x04000EE2 RID: 3810
	private Pickupable pickupable;

	// Token: 0x04000EE3 RID: 3811
	private MedicinalPillWorkable medicine;

	// Token: 0x04000EE4 RID: 3812
	public static readonly Chore.Precondition CanCure = new Chore.Precondition
	{
		id = "CanCure",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_CURE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((TakeMedicineChore)data).medicine.CanBeTakenBy(context.consumerState.gameObject);
		}
	};

	// Token: 0x04000EE5 RID: 3813
	public static readonly Chore.Precondition IsConsumptionPermitted = new Chore.Precondition
	{
		id = "IsConsumptionPermitted",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CONSUMPTION_PERMITTED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			TakeMedicineChore takeMedicineChore = (TakeMedicineChore)data;
			ConsumableConsumer consumableConsumer = context.consumerState.consumableConsumer;
			return consumableConsumer == null || consumableConsumer.IsPermitted(takeMedicineChore.medicine.PrefabID().Name);
		}
	};

	// Token: 0x0200132B RID: 4907
	public class StatesInstance : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.GameInstance
	{
		// Token: 0x06008B22 RID: 35618 RVA: 0x0035DCE1 File Offset: 0x0035BEE1
		public StatesInstance(TakeMedicineChore master) : base(master)
		{
		}
	}

	// Token: 0x0200132C RID: 4908
	public class States : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore>
	{
		// Token: 0x06008B23 RID: 35619 RVA: 0x0035DCEC File Offset: 0x0035BEEC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.eater);
			this.fetch.InitializeStates(this.eater, this.source, this.chunk, this.requestedpillcount, this.actualpillcount, this.takemedicine, null);
			this.takemedicine.ToggleAnims("anim_eat_floor_kanim", 0f).ToggleTag(GameTags.TakingMedicine).ToggleWork("TakeMedicine", delegate(TakeMedicineChore.StatesInstance smi)
			{
				MedicinalPillWorkable workable = this.chunk.Get<MedicinalPillWorkable>(smi);
				this.eater.Get<WorkerBase>(smi).StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (TakeMedicineChore.StatesInstance smi) => this.chunk.Get<MedicinalPill>(smi) != null, null, null);
		}

		// Token: 0x04006A7F RID: 27263
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter eater;

		// Token: 0x04006A80 RID: 27264
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter source;

		// Token: 0x04006A81 RID: 27265
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter chunk;

		// Token: 0x04006A82 RID: 27266
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter requestedpillcount;

		// Token: 0x04006A83 RID: 27267
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter actualpillcount;

		// Token: 0x04006A84 RID: 27268
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FetchSubState fetch;

		// Token: 0x04006A85 RID: 27269
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.State takemedicine;
	}
}
