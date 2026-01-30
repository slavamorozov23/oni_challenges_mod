using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
[SerializationConfig(MemberSerialization.OptIn)]
public class SolarPanel : Generator
{
	// Token: 0x060036C2 RID: 14018 RVA: 0x00134AC4 File Offset: 0x00132CC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cachedCell = Grid.PosToCell(this);
		base.Subscribe<SolarPanel>(824508782, SolarPanel.OnActiveChangedDelegate);
		this.smi = new SolarPanel.StatesInstance(this);
		this.smi.StartSM();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x00134B6A File Offset: 0x00132D6A
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x00134B98 File Offset: 0x00132D98
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x00134BF0 File Offset: 0x00132DF0
	private void UpdateStatusItem()
	{
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage, false);
		if (this.statusHandle == Guid.Empty)
		{
			this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.SolarPanelWattage, this);
			return;
		}
		if (this.statusHandle != Guid.Empty)
		{
			this.selectable.ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.SolarPanelWattage, this);
		}
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x00134C84 File Offset: 0x00132E84
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		foreach (CellOffset offset in this.solarCellOffsets)
		{
			int num2 = Grid.LightIntensity[Grid.OffsetCell(this.cachedCell, offset)];
			num += (float)num2 * 0.00053f;
		}
		this.operational.SetActive(num > 0f, false);
		num = Mathf.Clamp(num, 0f, 380f);
		Game.Instance.accumulators.Accumulate(this.accumulator, num * dt);
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 380f);
		this.UpdateStatusItem();
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x060036C7 RID: 14023 RVA: 0x00134D9C File Offset: 0x00132F9C
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x0400213C RID: 8508
	private MeterController meter;

	// Token: 0x0400213D RID: 8509
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x0400213E RID: 8510
	private SolarPanel.StatesInstance smi;

	// Token: 0x0400213F RID: 8511
	private Guid statusHandle;

	// Token: 0x04002140 RID: 8512
	private int cachedCell;

	// Token: 0x04002141 RID: 8513
	private CellOffset[] solarCellOffsets = new CellOffset[]
	{
		new CellOffset(-3, 2),
		new CellOffset(-2, 2),
		new CellOffset(-1, 2),
		new CellOffset(0, 2),
		new CellOffset(1, 2),
		new CellOffset(2, 2),
		new CellOffset(3, 2),
		new CellOffset(-3, 1),
		new CellOffset(-2, 1),
		new CellOffset(-1, 1),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(2, 1),
		new CellOffset(3, 1)
	};

	// Token: 0x04002142 RID: 8514
	private static readonly EventSystem.IntraObjectHandler<SolarPanel> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<SolarPanel>(delegate(SolarPanel component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x02001772 RID: 6002
	public class StatesInstance : GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel, object>.GameInstance
	{
		// Token: 0x06009B3D RID: 39741 RVA: 0x003942A1 File Offset: 0x003924A1
		public StatesInstance(SolarPanel master) : base(master)
		{
		}
	}

	// Token: 0x02001773 RID: 6003
	public class States : GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel>
	{
		// Token: 0x06009B3E RID: 39742 RVA: 0x003942AA File Offset: 0x003924AA
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.DoNothing();
		}

		// Token: 0x040077C5 RID: 30661
		public GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel, object>.State idle;
	}
}
