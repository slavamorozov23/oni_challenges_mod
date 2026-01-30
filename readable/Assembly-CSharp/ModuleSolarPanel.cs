using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
[SerializationConfig(MemberSerialization.OptIn)]
public class ModuleSolarPanel : Generator
{
	// Token: 0x060034BC RID: 13500 RVA: 0x0012B1E4 File Offset: 0x001293E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.IsVirtual = true;
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x0012B1F4 File Offset: 0x001293F4
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		base.Subscribe<ModuleSolarPanel>(824508782, ModuleSolarPanel.OnActiveChangedDelegate);
		this.smi = new ModuleSolarPanel.StatesInstance(this);
		this.smi.StartSM();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		Grid.PosToCell(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x0012B2CA File Offset: 0x001294CA
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x0012B2F8 File Offset: 0x001294F8
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x0012B350 File Offset: 0x00129550
	private void UpdateStatusItem()
	{
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage, false);
		if (this.statusHandle == Guid.Empty)
		{
			this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
			return;
		}
		if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
		}
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x0012B3E4 File Offset: 0x001295E4
	public override void EnergySim200ms(float dt)
	{
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, true);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		if (Grid.IsValidCell(Grid.PosToCell(this)) && Grid.WorldIdx[Grid.PosToCell(this)] != 255)
		{
			foreach (CellOffset offset in this.solarCellOffsets)
			{
				int num2 = Grid.LightIntensity[Grid.OffsetCell(Grid.PosToCell(this), offset)];
				num += (float)num2 * 0.00053f;
			}
		}
		else
		{
			num = 60f;
		}
		num = Mathf.Clamp(num, 0f, 60f);
		this.operational.SetActive(num > 0f, false);
		Game.Instance.accumulators.Accumulate(this.accumulator, num * dt);
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 60f);
		this.UpdateStatusItem();
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x060034C2 RID: 13506 RVA: 0x0012B522 File Offset: 0x00129722
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04001FE9 RID: 8169
	private MeterController meter;

	// Token: 0x04001FEA RID: 8170
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001FEB RID: 8171
	private ModuleSolarPanel.StatesInstance smi;

	// Token: 0x04001FEC RID: 8172
	private Guid statusHandle;

	// Token: 0x04001FED RID: 8173
	private CellOffset[] solarCellOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04001FEE RID: 8174
	private static readonly EventSystem.IntraObjectHandler<ModuleSolarPanel> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ModuleSolarPanel>(delegate(ModuleSolarPanel component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x02001714 RID: 5908
	public class StatesInstance : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.GameInstance
	{
		// Token: 0x060099EA RID: 39402 RVA: 0x0038F28D File Offset: 0x0038D48D
		public StatesInstance(ModuleSolarPanel master) : base(master)
		{
		}
	}

	// Token: 0x02001715 RID: 5909
	public class States : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel>
	{
		// Token: 0x060099EB RID: 39403 RVA: 0x0038F296 File Offset: 0x0038D496
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.EventTransition(GameHashes.DoLaunchRocket, this.launch, null).DoNothing();
			this.launch.EventTransition(GameHashes.RocketLanded, this.idle, null);
		}

		// Token: 0x040076DB RID: 30427
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State idle;

		// Token: 0x040076DC RID: 30428
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State launch;
	}
}
