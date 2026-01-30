using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02000822 RID: 2082
[AddComponentMenu("KMonoBehaviour/scripts/Turbine")]
public class Turbine : KMonoBehaviour
{
	// Token: 0x060038B0 RID: 14512 RVA: 0x0013D248 File Offset: 0x0013B448
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(Turbine.OnSimEmittedCallback), this, "TurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		this.destCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -1));
			this.destCells[i] = Grid.OffsetCell(cell, new CellOffset(x, def.HeightInCells - 1));
		}
		this.smi = new Turbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x0013D324 File Offset: 0x0013B524
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
		this.smi.UpdateMeter();
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x0013D380 File Offset: 0x0013B580
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "Turbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x0013D3D4 File Offset: 0x0013B5D4
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(Turbine.OnSimConsumeCallback), this, "TurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x0013D442 File Offset: 0x0013B642
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((Turbine)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x0013D450 File Offset: 0x0013B650
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minEmitMass && this.simEmitCBHandle.IsValid())
			{
				float mass = this.storedMass / (float)this.destCells.Length;
				int disease_count = this.diseaseCount / this.destCells.Length;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				int[] array = this.destCells;
				for (int i = 0; i < array.Length; i++)
				{
					SimMessages.EmitMass(array[i], mass_cb_info.elemIdx, mass, this.emitTemperature, this.diseaseIdx, disease_count, this.simEmitCBHandle.index);
				}
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x0013D59A File Offset: 0x0013B79A
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((Turbine)data).OnSimEmitted(info);
	}

	// Token: 0x060038B7 RID: 14519 RVA: 0x0013D5A8 File Offset: 0x0013B7A8
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

	// Token: 0x060038B8 RID: 14520 RVA: 0x0013D66C File Offset: 0x0013B86C
	public static void InitializeStatusItems()
	{
		Turbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.outputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_OUTPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.spinningUpStatusItem = new StatusItem("TURBINE_SPINNING_UP", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = string.Format(str, (int)turbine.currentRPM);
			return str;
		};
		Turbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientMassStatusItem.resolveTooltipCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = str.Replace("{MASS}", GameUtil.GetFormattedMass(turbine.requiredMassFlowDifferential, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
			return str;
		};
		Turbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(Turbine.ResolveStrings);
		Turbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(Turbine.ResolveStrings);
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x0013D7E8 File Offset: 0x0013B9E8
	private static string ResolveStrings(string str, object data)
	{
		Turbine turbine = (Turbine)data;
		str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
		str = str.Replace("{ACTIVE_TEMPERATURE}", GameUtil.GetFormattedTemperature(turbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		return str;
	}

	// Token: 0x04002280 RID: 8832
	public SimHashes srcElem;

	// Token: 0x04002281 RID: 8833
	public float requiredMassFlowDifferential = 3f;

	// Token: 0x04002282 RID: 8834
	public float activePercent = 0.75f;

	// Token: 0x04002283 RID: 8835
	public float minEmitMass;

	// Token: 0x04002284 RID: 8836
	public float minActiveTemperature = 400f;

	// Token: 0x04002285 RID: 8837
	public float emitTemperature = 300f;

	// Token: 0x04002286 RID: 8838
	public float maxRPM;

	// Token: 0x04002287 RID: 8839
	public float rpmAcceleration;

	// Token: 0x04002288 RID: 8840
	public float rpmDeceleration;

	// Token: 0x04002289 RID: 8841
	public float minGenerationRPM;

	// Token: 0x0400228A RID: 8842
	public float pumpKGRate;

	// Token: 0x0400228B RID: 8843
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x0400228C RID: 8844
	[Serialize]
	private float storedMass;

	// Token: 0x0400228D RID: 8845
	[Serialize]
	private float storedTemperature;

	// Token: 0x0400228E RID: 8846
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x0400228F RID: 8847
	[Serialize]
	private int diseaseCount;

	// Token: 0x04002290 RID: 8848
	[MyCmpGet]
	private Generator generator;

	// Token: 0x04002291 RID: 8849
	[Serialize]
	private float currentRPM;

	// Token: 0x04002292 RID: 8850
	private int[] srcCells;

	// Token: 0x04002293 RID: 8851
	private int[] destCells;

	// Token: 0x04002294 RID: 8852
	private Turbine.Instance smi;

	// Token: 0x04002295 RID: 8853
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x04002296 RID: 8854
	private static StatusItem outputBlockedStatusItem;

	// Token: 0x04002297 RID: 8855
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x04002298 RID: 8856
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x04002299 RID: 8857
	private static StatusItem activeStatusItem;

	// Token: 0x0400229A RID: 8858
	private static StatusItem spinningUpStatusItem;

	// Token: 0x0400229B RID: 8859
	private MeterController meter;

	// Token: 0x0400229C RID: 8860
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x020017C6 RID: 6086
	public class States : GameStateMachine<Turbine.States, Turbine.Instance, Turbine>
	{
		// Token: 0x06009C80 RID: 40064 RVA: 0x00399820 File Offset: 0x00397A20
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Turbine.InitializeStatusItems();
			default_state = this.operational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.spinningUp, (Turbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null).Enter(delegate(Turbine.Instance smi)
			{
				smi.master.currentRPM = 0f;
				smi.UpdateMeter();
			});
			this.operational.DefaultState(this.operational.spinningUp).EventTransition(GameHashes.OperationalChanged, this.inoperational, (Turbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(Turbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(Turbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.spinningUp.ToggleStatusItem((Turbine.Instance smi) => Turbine.spinningUpStatusItem, (Turbine.Instance smi) => smi.master).QueueAnim("buildup", true, null);
			this.operational.active.Update("UpdateActive", delegate(Turbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((Turbine.Instance smi) => Turbine.activeStatusItem, (Turbine.Instance smi) => smi.master).Enter(delegate(Turbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(Turbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Turbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x040078B2 RID: 30898
		public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State inoperational;

		// Token: 0x040078B3 RID: 30899
		public Turbine.States.OperationalStates operational;

		// Token: 0x040078B4 RID: 30900
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x02002960 RID: 10592
		public class OperationalStates : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State
		{
			// Token: 0x0400B71F RID: 46879
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State idle;

			// Token: 0x0400B720 RID: 46880
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State spinningUp;

			// Token: 0x0400B721 RID: 46881
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State active;
		}
	}

	// Token: 0x020017C7 RID: 6087
	public class Instance : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.GameInstance
	{
		// Token: 0x06009C83 RID: 40067 RVA: 0x00399AAD File Offset: 0x00397CAD
		public Instance(Turbine master) : base(master)
		{
		}

		// Token: 0x06009C84 RID: 40068 RVA: 0x00399AE4 File Offset: 0x00397CE4
		public void UpdateState(float dt)
		{
			float num = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature) ? base.master.rpmAcceleration : (-base.master.rpmDeceleration);
			base.master.currentRPM = Mathf.Clamp(base.master.currentRPM + dt * num, 0f, base.master.maxRPM);
			this.UpdateMeter();
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (base.master.currentRPM >= base.master.minGenerationRPM)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
				}
				base.smi.master.generator.GenerateJoules(base.smi.master.generator.WattageRating * dt, false);
				return;
			}
			if (base.master.currentRPM > 0f)
			{
				if (currentState != base.sm.operational.spinningUp)
				{
					base.smi.GoTo(base.sm.operational.spinningUp);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x06009C85 RID: 40069 RVA: 0x00399C4C File Offset: 0x00397E4C
		public void UpdateMeter()
		{
			if (base.master.meter != null)
			{
				float num = Mathf.Clamp01(base.master.currentRPM / base.master.maxRPM);
				base.master.meter.SetPositionPercent(num);
				base.master.meter.SetSymbolTint(Turbine.TINT_SYMBOL, (num >= base.master.activePercent) ? Color.green : Color.red);
			}
		}

		// Token: 0x06009C86 RID: 40070 RVA: 0x00399CD0 File Offset: 0x00397ED0
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = float.PositiveInfinity;
			this.isInputBlocked = false;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num4 = base.master.srcCells[i];
				float b = Grid.Mass[num4];
				if (Grid.Element[num4].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
				}
				float b2 = Grid.Temperature[num4];
				num2 = Mathf.Max(num2, b2);
				ushort index = Grid.ElementIdx[num4];
				Element element = ElementLoader.elements[(int)index];
				if (element.IsLiquid || element.IsSolid)
				{
					this.isInputBlocked = true;
				}
			}
			this.isOutputBlocked = false;
			for (int j = 0; j < base.master.destCells.Length; j++)
			{
				int i2 = base.master.destCells[j];
				float b3 = Grid.Mass[i2];
				num3 = Mathf.Min(num3, b3);
				ushort index2 = Grid.ElementIdx[i2];
				Element element2 = ElementLoader.elements[(int)index2];
				if (element2.IsLiquid || element2.IsSolid)
				{
					this.isOutputBlocked = true;
				}
			}
			insufficient_mass = (num - num3 < base.master.requiredMassFlowDifferential);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x06009C87 RID: 40071 RVA: 0x00399E4C File Offset: 0x0039804C
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(Turbine.inputBlockedStatusItem, this.isInputBlocked, this.inputBlockedHandle, component);
			this.outputBlockedHandle = this.UpdateStatusItem(Turbine.outputBlockedStatusItem, this.isOutputBlocked, this.outputBlockedHandle, component);
			this.insufficientMassHandle = this.UpdateStatusItem(Turbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(Turbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
		}

		// Token: 0x06009C88 RID: 40072 RVA: 0x00399ED8 File Offset: 0x003980D8
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

		// Token: 0x06009C89 RID: 40073 RVA: 0x00399F14 File Offset: 0x00398114
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.inputBlockedHandle, false);
			component.RemoveStatusItem(this.outputBlockedHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
		}

		// Token: 0x040078B5 RID: 30901
		public bool isInputBlocked;

		// Token: 0x040078B6 RID: 30902
		public bool isOutputBlocked;

		// Token: 0x040078B7 RID: 30903
		public bool insufficientMass;

		// Token: 0x040078B8 RID: 30904
		public bool insufficientTemperature;

		// Token: 0x040078B9 RID: 30905
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x040078BA RID: 30906
		private Guid outputBlockedHandle = Guid.Empty;

		// Token: 0x040078BB RID: 30907
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x040078BC RID: 30908
		private Guid insufficientTemperatureHandle = Guid.Empty;
	}
}
