using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007F5 RID: 2037
[SerializationConfig(MemberSerialization.OptIn)]
public class RustDeoxidizer : StateMachineComponent<RustDeoxidizer.StatesInstance>
{
	// Token: 0x06003689 RID: 13961 RVA: 0x00133AF9 File Offset: 0x00131CF9
	protected override void OnSpawn()
	{
		base.smi.StartSM();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x00133B1B File Offset: 0x00131D1B
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x0600368B RID: 13963 RVA: 0x00133B3C File Offset: 0x00131D3C
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.CellAbove(num);
			return !GameUtil.FloodFillCheck<RustDeoxidizer>(new Func<int, RustDeoxidizer, bool>(RustDeoxidizer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x00133B7A File Offset: 0x00131D7A
	private static bool OverPressure(int cell, RustDeoxidizer rustDeoxidizer)
	{
		return Grid.Mass[cell] > rustDeoxidizer.maxMass;
	}

	// Token: 0x0400211B RID: 8475
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x0400211C RID: 8476
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400211D RID: 8477
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x0400211E RID: 8478
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400211F RID: 8479
	private MeterController meter;

	// Token: 0x02001769 RID: 5993
	public class StatesInstance : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.GameInstance
	{
		// Token: 0x06009B25 RID: 39717 RVA: 0x00393CE2 File Offset: 0x00391EE2
		public StatesInstance(RustDeoxidizer smi) : base(smi)
		{
		}
	}

	// Token: 0x0200176A RID: 5994
	public class States : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer>
	{
		// Token: 0x06009B26 RID: 39718 RVA: 0x00393CEC File Offset: 0x00391EEC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (RustDeoxidizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (RustDeoxidizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (RustDeoxidizer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (RustDeoxidizer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x040077AF RID: 30639
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State disabled;

		// Token: 0x040077B0 RID: 30640
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State waiting;

		// Token: 0x040077B1 RID: 30641
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State converting;

		// Token: 0x040077B2 RID: 30642
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State overpressure;
	}
}
