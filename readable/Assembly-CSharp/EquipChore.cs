using System;

// Token: 0x020004A3 RID: 1187
public class EquipChore : Chore<EquipChore.StatesInstance>
{
	// Token: 0x06001926 RID: 6438 RVA: 0x0008C484 File Offset: 0x0008A684
	public EquipChore(IStateMachineTarget equippable) : base(Db.Get().ChoreTypes.Equip, equippable, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EquipChore.StatesInstance(this);
		base.smi.sm.equippable_source.Set(equippable.gameObject, base.smi, false);
		base.smi.sm.requested_units.Set(1f, base.smi, false);
		this.showAvailabilityInHoverText = false;
		Prioritizable.AddRef(equippable.gameObject);
		Game.Instance.Trigger(1980521255, equippable.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, equippable.GetComponent<Assignable>());
		this.AddPrecondition(ChorePreconditions.instance.CanPickup, equippable.GetComponent<Pickupable>());
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x0008C558 File Offset: 0x0008A758
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			Debug.LogError("EquipChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			Debug.LogError("EquipChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			Debug.LogError("EquipChore null smi.sm");
			return;
		}
		if (base.smi.sm.equippable_source == null)
		{
			Debug.LogError("EquipChore null smi.sm.equippable_source");
			return;
		}
		base.smi.sm.equipper.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x020012D7 RID: 4823
	public class StatesInstance : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.GameInstance
	{
		// Token: 0x060089D1 RID: 35281 RVA: 0x00354BE1 File Offset: 0x00352DE1
		public StatesInstance(EquipChore master) : base(master)
		{
		}
	}

	// Token: 0x020012D8 RID: 4824
	public class States : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore>
	{
		// Token: 0x060089D2 RID: 35282 RVA: 0x00354BEC File Offset: 0x00352DEC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.equipper);
			this.root.DoNothing();
			this.fetch.InitializeStates(this.equipper, this.equippable_source, this.equippable_result, this.requested_units, this.actual_units, this.equip, null);
			this.equip.ToggleWork<EquippableWorkable>(this.equippable_result, null, null, null);
		}

		// Token: 0x0400694F RID: 26959
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equipper;

		// Token: 0x04006950 RID: 26960
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_source;

		// Token: 0x04006951 RID: 26961
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_result;

		// Token: 0x04006952 RID: 26962
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter requested_units;

		// Token: 0x04006953 RID: 26963
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter actual_units;

		// Token: 0x04006954 RID: 26964
		public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FetchSubState fetch;

		// Token: 0x04006955 RID: 26965
		public EquipChore.States.Equip equip;

		// Token: 0x020027AF RID: 10159
		public class Equip : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State
		{
			// Token: 0x0400AFF0 RID: 45040
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pre;

			// Token: 0x0400AFF1 RID: 45041
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pst;
		}
	}
}
