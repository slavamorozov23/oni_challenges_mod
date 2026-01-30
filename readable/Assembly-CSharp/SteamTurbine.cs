using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000804 RID: 2052
public class SteamTurbine : Generator
{
	// Token: 0x17000396 RID: 918
	// (get) Token: 0x0600372E RID: 14126 RVA: 0x00136631 File Offset: 0x00134831
	// (set) Token: 0x0600372F RID: 14127 RVA: 0x00136639 File Offset: 0x00134839
	public int BlockedInputs { get; private set; }

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06003730 RID: 14128 RVA: 0x00136642 File Offset: 0x00134842
	public int TotalInputs
	{
		get
		{
			return this.srcCells.Length;
		}
	}

	// Token: 0x06003731 RID: 14129 RVA: 0x0013664C File Offset: 0x0013484C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Power", this);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(SteamTurbine.OnSimEmittedCallback), this, "SteamTurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -2));
		}
		this.smi = new SteamTurbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x0013672C File Offset: 0x0013492C
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
	}

	// Token: 0x06003733 RID: 14131 RVA: 0x0013677C File Offset: 0x0013497C
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "SteamTurbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x06003734 RID: 14132 RVA: 0x001367D0 File Offset: 0x001349D0
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(SteamTurbine.OnSimConsumeCallback, this, "SteamTurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x06003735 RID: 14133 RVA: 0x00136838 File Offset: 0x00134A38
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minConvertMass && this.simEmitCBHandle.IsValid())
			{
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				this.gasStorage.AddGasChunk(this.srcElem, this.storedMass, this.storedTemperature, this.diseaseIdx, this.diseaseCount, true, true);
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x00136946 File Offset: 0x00134B46
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((SteamTurbine)data).OnSimEmitted(info);
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x00136954 File Offset: 0x00134B54
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded != 1)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, info.mass, info.temperature);
			this.storedMass += info.mass;
			if (info.diseaseIdx != 255)
			{
				SimUtil.DiseaseInfo a = new SimUtil.DiseaseInfo
				{
					idx = this.diseaseIdx,
					count = this.diseaseCount
				};
				SimUtil.DiseaseInfo b = new SimUtil.DiseaseInfo
				{
					idx = info.diseaseIdx,
					count = info.diseaseCount
				};
				SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(a, b);
				this.diseaseIdx = diseaseInfo.idx;
				this.diseaseCount = diseaseInfo.count;
			}
		}
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x00136A18 File Offset: 0x00134C18
	public static void InitializeStatusItems()
	{
		SteamTurbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem = new StatusItem("TURBINE_PARTIALLY_BLOCKED_INPUT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolvePartialBlockedStatus);
		SteamTurbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientMassStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.buildingTooHotItem = new StatusItem("TURBINE_TOO_HOT", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.buildingTooHotItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.activeWattageStatusItem = new StatusItem("TURBINE_ACTIVE_WATTAGE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.activeWattageStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveWattageStatus);
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x00136BC4 File Offset: 0x00134DC4
	private static string ResolveWattageStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		float num = Game.Instance.accumulators.GetAverageRate(steamTurbine.accumulator) / steamTurbine.WattageRating;
		return str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Max_Wattage}", GameUtil.GetFormattedWattage(steamTurbine.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Efficiency}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None)).Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x00136C58 File Offset: 0x00134E58
	private static string ResolvePartialBlockedStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		return str.Replace("{Blocked}", steamTurbine.BlockedInputs.ToString()).Replace("{Total}", steamTurbine.TotalInputs.ToString());
	}

	// Token: 0x0600373B RID: 14139 RVA: 0x00136CA0 File Offset: 0x00134EA0
	private static string ResolveStrings(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		str = str.Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
		str = str.Replace("{Dest_Element}", ElementLoader.FindElementByHash(steamTurbine.destElem).name);
		str = str.Replace("{Overheat_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.maxBuildingTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Active_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Min_Mass}", GameUtil.GetFormattedMass(steamTurbine.requiredMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		return str;
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x00136D47 File Offset: 0x00134F47
	public void SetStorage(Storage steamStorage, Storage waterStorage)
	{
		this.gasStorage = steamStorage;
		this.liquidStorage = waterStorage;
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x00136D58 File Offset: 0x00134F58
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			this.meter.SetPositionPercent(0f);
			return;
		}
		float num = 0f;
		if (this.gasStorage != null && this.gasStorage.items.Count > 0)
		{
			GameObject gameObject = this.gasStorage.FindFirst(ElementLoader.FindElementByHash(this.srcElem).tag);
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2 = 0.1f;
				if (component.Mass > num2)
				{
					num2 = Mathf.Min(component.Mass, this.pumpKGRate * dt);
					num = Mathf.Min(this.JoulesToGenerate(component) * (num2 / this.pumpKGRate), base.WattageRating * dt);
					float num3 = this.HeatFromCoolingSteam(component) * (num2 / component.Mass);
					float num4 = num2 / component.Mass;
					int num5 = Mathf.RoundToInt((float)component.DiseaseCount * num4);
					component.Mass -= num2;
					component.ModifyDiseaseCount(-num5, "SteamTurbine.EnergySim200ms");
					float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
					this.lastSampleTime = Time.time;
					GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, num3 * this.wasteHeatToTurbinePercent, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, display_dt);
					this.liquidStorage.AddLiquid(this.destElem, num2, this.outputElementTemperature, component.DiseaseIdx, num5, true, true);
				}
			}
		}
		num = Mathf.Clamp(num, 0f, base.WattageRating);
		Game.Instance.accumulators.Accumulate(this.accumulator, num);
		if (num > 0f)
		{
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating);
		this.meter.SetSymbolTint(SteamTurbine.TINT_SYMBOL, Color.Lerp(Color.red, Color.green, Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating));
	}

	// Token: 0x0600373E RID: 14142 RVA: 0x00136FB8 File Offset: 0x001351B8
	public float HeatFromCoolingSteam(PrimaryElement steam)
	{
		float temperature = steam.Temperature;
		return -GameUtil.CalculateEnergyDeltaForElement(steam, temperature, this.outputElementTemperature);
	}

	// Token: 0x0600373F RID: 14143 RVA: 0x00136FDC File Offset: 0x001351DC
	public float JoulesToGenerate(PrimaryElement steam)
	{
		float num = (steam.Temperature - this.outputElementTemperature) / (this.idealSourceElementTemperature - this.outputElementTemperature);
		return base.WattageRating * (float)Math.Pow((double)num, 1.0);
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06003740 RID: 14144 RVA: 0x0013701D File Offset: 0x0013521D
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04002186 RID: 8582
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002187 RID: 8583
	public SimHashes srcElem;

	// Token: 0x04002188 RID: 8584
	public SimHashes destElem;

	// Token: 0x04002189 RID: 8585
	public float requiredMass = 0.001f;

	// Token: 0x0400218A RID: 8586
	public float minActiveTemperature = 398.15f;

	// Token: 0x0400218B RID: 8587
	public float idealSourceElementTemperature = 473.15f;

	// Token: 0x0400218C RID: 8588
	public float maxBuildingTemperature = 373.15f;

	// Token: 0x0400218D RID: 8589
	public float outputElementTemperature = 368.15f;

	// Token: 0x0400218E RID: 8590
	public float minConvertMass;

	// Token: 0x0400218F RID: 8591
	public float pumpKGRate;

	// Token: 0x04002190 RID: 8592
	public float maxSelfHeat;

	// Token: 0x04002191 RID: 8593
	public float wasteHeatToTurbinePercent;

	// Token: 0x04002192 RID: 8594
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x04002193 RID: 8595
	[Serialize]
	private float storedMass;

	// Token: 0x04002194 RID: 8596
	[Serialize]
	private float storedTemperature;

	// Token: 0x04002195 RID: 8597
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x04002196 RID: 8598
	[Serialize]
	private int diseaseCount;

	// Token: 0x04002197 RID: 8599
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x04002198 RID: 8600
	private static StatusItem inputPartiallyBlockedStatusItem;

	// Token: 0x04002199 RID: 8601
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x0400219A RID: 8602
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x0400219B RID: 8603
	private static StatusItem activeWattageStatusItem;

	// Token: 0x0400219C RID: 8604
	private static StatusItem buildingTooHotItem;

	// Token: 0x0400219D RID: 8605
	private static StatusItem activeStatusItem;

	// Token: 0x0400219F RID: 8607
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x040021A0 RID: 8608
	private MeterController meter;

	// Token: 0x040021A1 RID: 8609
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x040021A2 RID: 8610
	private SteamTurbine.Instance smi;

	// Token: 0x040021A3 RID: 8611
	private int[] srcCells;

	// Token: 0x040021A4 RID: 8612
	private Storage gasStorage;

	// Token: 0x040021A5 RID: 8613
	private Storage liquidStorage;

	// Token: 0x040021A6 RID: 8614
	private ElementConsumer consumer;

	// Token: 0x040021A7 RID: 8615
	private Guid statusHandle;

	// Token: 0x040021A8 RID: 8616
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x040021A9 RID: 8617
	private float lastSampleTime = -1f;

	// Token: 0x040021AA RID: 8618
	private static Action<Sim.MassConsumedCallback, object> OnSimConsumeCallback = delegate(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((SteamTurbine)data).OnSimConsume(mass_cb_info);
	};

	// Token: 0x0200178B RID: 6027
	public class States : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine>
	{
		// Token: 0x06009B73 RID: 39795 RVA: 0x003950F8 File Offset: 0x003932F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			SteamTurbine.InitializeStatusItems();
			default_state = this.operational;
			this.root.Update("UpdateBlocked", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateBlocked(dt);
			}, UpdateRate.SIM_200ms, false);
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.active, (SteamTurbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null);
			this.operational.DefaultState(this.operational.active).EventTransition(GameHashes.OperationalChanged, this.inoperational, (SteamTurbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.active.Update("UpdateActive", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SteamTurbine.Instance smi) => SteamTurbine.activeStatusItem, (SteamTurbine.Instance smi) => smi.master).Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.tooHot.Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.TOOHOT_ANIMS, KAnim.PlayMode.Loop);
			});
		}

		// Token: 0x040077F6 RID: 30710
		public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State inoperational;

		// Token: 0x040077F7 RID: 30711
		public SteamTurbine.States.OperationalStates operational;

		// Token: 0x040077F8 RID: 30712
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x040077F9 RID: 30713
		private static readonly HashedString[] TOOHOT_ANIMS = new HashedString[]
		{
			"working_pre"
		};

		// Token: 0x02002949 RID: 10569
		public class OperationalStates : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State
		{
			// Token: 0x0400B6A3 RID: 46755
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State idle;

			// Token: 0x0400B6A4 RID: 46756
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State active;

			// Token: 0x0400B6A5 RID: 46757
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State tooHot;
		}
	}

	// Token: 0x0200178C RID: 6028
	public class Instance : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.GameInstance
	{
		// Token: 0x06009B76 RID: 39798 RVA: 0x0039538C File Offset: 0x0039358C
		public Instance(SteamTurbine master) : base(master)
		{
		}

		// Token: 0x06009B77 RID: 39799 RVA: 0x003953E4 File Offset: 0x003935E4
		public void UpdateBlocked(float dt)
		{
			base.master.BlockedInputs = 0;
			for (int i = 0; i < base.master.TotalInputs; i++)
			{
				int num = base.master.srcCells[i];
				Element element = Grid.Element[num];
				if (element.IsLiquid || element.IsSolid)
				{
					SteamTurbine master = base.master;
					int blockedInputs = master.BlockedInputs;
					master.BlockedInputs = blockedInputs + 1;
				}
			}
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputBlockedStatusItem, base.master.BlockedInputs == base.master.TotalInputs, this.inputBlockedHandle, component);
			this.inputPartiallyBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputPartiallyBlockedStatusItem, base.master.BlockedInputs > 0 && base.master.BlockedInputs < base.master.TotalInputs, this.inputPartiallyBlockedHandle, component);
		}

		// Token: 0x06009B78 RID: 39800 RVA: 0x003954C8 File Offset: 0x003936C8
		public void UpdateState(float dt)
		{
			bool flag = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature);
			bool flag2 = this.IsTooHot(ref this.buildingTooHot);
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (flag2)
			{
				if (currentState != base.sm.operational.tooHot)
				{
					base.smi.GoTo(base.sm.operational.tooHot);
					return;
				}
			}
			else if (flag)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x06009B79 RID: 39801 RVA: 0x00395597 File Offset: 0x00393797
		private bool IsTooHot(ref bool building_too_hot)
		{
			building_too_hot = (base.gameObject.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxBuildingTemperature);
			return building_too_hot;
		}

		// Token: 0x06009B7A RID: 39802 RVA: 0x003955C0 File Offset: 0x003937C0
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num3 = base.master.srcCells[i];
				float b = Grid.Mass[num3];
				if (Grid.Element[num3].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
					float b2 = Grid.Temperature[num3];
					num2 = Mathf.Max(num2, b2);
				}
			}
			insufficient_mass = (num < base.master.requiredMass);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x06009B7B RID: 39803 RVA: 0x00395670 File Offset: 0x00393870
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.insufficientMassHandle = this.UpdateStatusItem(SteamTurbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(SteamTurbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
			this.buildingTooHotHandle = this.UpdateStatusItem(SteamTurbine.buildingTooHotItem, this.buildingTooHot, this.buildingTooHotHandle, component);
			StatusItem status_item = base.master.operational.IsActive ? SteamTurbine.activeWattageStatusItem : Db.Get().BuildingStatusItems.GeneratorOffline;
			this.activeWattageHandle = component.SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, base.master);
		}

		// Token: 0x06009B7C RID: 39804 RVA: 0x0039572C File Offset: 0x0039392C
		private Guid UpdateStatusItem(StatusItem item, bool show, Guid current_handle, KSelectable ksel)
		{
			Guid result = current_handle;
			if (show != (current_handle != Guid.Empty))
			{
				if (show)
				{
					result = ksel.AddStatusItem(item, base.master);
				}
				else
				{
					result = ksel.RemoveStatusItem(current_handle, false);
				}
			}
			return result;
		}

		// Token: 0x06009B7D RID: 39805 RVA: 0x00395768 File Offset: 0x00393968
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.buildingTooHotHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
			component.RemoveStatusItem(this.activeWattageHandle, false);
		}

		// Token: 0x040077FA RID: 30714
		public bool insufficientMass;

		// Token: 0x040077FB RID: 30715
		public bool insufficientTemperature;

		// Token: 0x040077FC RID: 30716
		public bool buildingTooHot;

		// Token: 0x040077FD RID: 30717
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x040077FE RID: 30718
		private Guid inputPartiallyBlockedHandle = Guid.Empty;

		// Token: 0x040077FF RID: 30719
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x04007800 RID: 30720
		private Guid insufficientTemperatureHandle = Guid.Empty;

		// Token: 0x04007801 RID: 30721
		private Guid buildingTooHotHandle = Guid.Empty;

		// Token: 0x04007802 RID: 30722
		private Guid activeWattageHandle = Guid.Empty;
	}
}
