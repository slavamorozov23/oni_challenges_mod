using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000751 RID: 1873
[SerializationConfig(MemberSerialization.OptIn)]
public class Electrolyzer : StateMachineComponent<Electrolyzer.StatesInstance>
{
	// Token: 0x06002F5F RID: 12127 RVA: 0x00111934 File Offset: 0x0010FB34
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.hasMeter)
		{
			this.meter = new MeterController(component, "U2H_meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(-0.4f, 0.5f, -0.1f), new string[]
			{
				"U2H_meter_target",
				"U2H_meter_tank",
				"U2H_meter_waterbody",
				"U2H_meter_level"
			});
		}
		base.smi.StartSM();
		this.UpdateMeter();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06002F60 RID: 12128 RVA: 0x001119C9 File Offset: 0x0010FBC9
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06002F61 RID: 12129 RVA: 0x001119E8 File Offset: 0x0010FBE8
	public void UpdateMeter()
	{
		if (this.hasMeter)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06002F62 RID: 12130 RVA: 0x00111A28 File Offset: 0x0010FC28
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.OffsetCell(num, this.emissionOffset);
			return !GameUtil.FloodFillCheck<Electrolyzer>(new Func<int, Electrolyzer, bool>(Electrolyzer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x06002F63 RID: 12131 RVA: 0x00111A6C File Offset: 0x0010FC6C
	private static bool OverPressure(int cell, Electrolyzer electrolyzer)
	{
		return Grid.Mass[cell] > electrolyzer.maxMass;
	}

	// Token: 0x04001C1D RID: 7197
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04001C1E RID: 7198
	[SerializeField]
	public bool hasMeter = true;

	// Token: 0x04001C1F RID: 7199
	[SerializeField]
	public CellOffset emissionOffset = CellOffset.none;

	// Token: 0x04001C20 RID: 7200
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001C21 RID: 7201
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04001C22 RID: 7202
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001C23 RID: 7203
	private MeterController meter;

	// Token: 0x02001635 RID: 5685
	public class StatesInstance : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.GameInstance
	{
		// Token: 0x06009656 RID: 38486 RVA: 0x0037F14E File Offset: 0x0037D34E
		public StatesInstance(Electrolyzer smi) : base(smi)
		{
		}
	}

	// Token: 0x02001636 RID: 5686
	public class States : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer>
	{
		// Token: 0x06009657 RID: 38487 RVA: 0x0037F158 File Offset: 0x0037D358
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (Electrolyzer.StatesInstance smi) => !smi.master.operational.IsOperational).EventHandler(GameHashes.OnStorageChange, delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (Electrolyzer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (Electrolyzer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (Electrolyzer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (Electrolyzer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (Electrolyzer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x04007416 RID: 29718
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State disabled;

		// Token: 0x04007417 RID: 29719
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State waiting;

		// Token: 0x04007418 RID: 29720
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State converting;

		// Token: 0x04007419 RID: 29721
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State overpressure;
	}
}
