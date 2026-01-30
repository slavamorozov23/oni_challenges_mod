using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000801 RID: 2049
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceHeater : StateMachineComponent<SpaceHeater.StatesInstance>, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06003705 RID: 14085 RVA: 0x00135E69 File Offset: 0x00134069
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06003706 RID: 14086 RVA: 0x00135E71 File Offset: 0x00134071
	public float MaxPower
	{
		get
		{
			return 240f;
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06003707 RID: 14087 RVA: 0x00135E78 File Offset: 0x00134078
	public float MinPower
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06003708 RID: 14088 RVA: 0x00135E7F File Offset: 0x0013407F
	public float MaxSelfHeatKWs
	{
		get
		{
			return 32f;
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06003709 RID: 14089 RVA: 0x00135E86 File Offset: 0x00134086
	public float MinSelfHeatKWs
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x0600370A RID: 14090 RVA: 0x00135E8D File Offset: 0x0013408D
	public float MaxExhaustedKWs
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x0600370B RID: 14091 RVA: 0x00135E94 File Offset: 0x00134094
	public float MinExhaustedKWs
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x0600370C RID: 14092 RVA: 0x00135E9B File Offset: 0x0013409B
	public float CurrentSelfHeatKW
	{
		get
		{
			return Mathf.Lerp(this.MinSelfHeatKWs, this.MaxSelfHeatKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x0600370D RID: 14093 RVA: 0x00135EB4 File Offset: 0x001340B4
	public float CurrentExhaustedKW
	{
		get
		{
			return Mathf.Lerp(this.MinExhaustedKWs, this.MaxExhaustedKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x0600370E RID: 14094 RVA: 0x00135ECD File Offset: 0x001340CD
	public float CurrentPowerConsumption
	{
		get
		{
			return Mathf.Lerp(this.MinPower, this.MaxPower, this.UserSliderSetting);
		}
	}

	// Token: 0x0600370F RID: 14095 RVA: 0x00135EE6 File Offset: 0x001340E6
	public static void GenerateHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		if (smi.master.produceHeat)
		{
			SpaceHeater.AddExhaustHeat(smi, dt);
			SpaceHeater.AddSelfHeat(smi, dt);
		}
	}

	// Token: 0x06003710 RID: 14096 RVA: 0x00135F08 File Offset: 0x00134108
	private static float AddExhaustHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentExhaustedKW = smi.master.CurrentExhaustedKW;
		StructureTemperatureComponents.ExhaustHeat(smi.master.extents, currentExhaustedKW, smi.master.overheatTemperature, dt);
		return currentExhaustedKW;
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x00135F40 File Offset: 0x00134140
	public static void RefreshHeatEffect(SpaceHeater.StatesInstance smi)
	{
		if (smi.master.heatEffect != null && smi.master.produceHeat)
		{
			float heatBeingProducedValue = smi.IsInsideState(smi.sm.online.heating) ? (smi.master.CurrentExhaustedKW + smi.master.CurrentSelfHeatKW) : 0f;
			smi.master.heatEffect.SetHeatBeingProducedValue(heatBeingProducedValue);
		}
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x00135FB8 File Offset: 0x001341B8
	private static float AddSelfHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentSelfHeatKW = smi.master.CurrentSelfHeatKW;
		GameComps.StructureTemperatures.ProduceEnergy(smi.master.structureTemperature, currentSelfHeatKW * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return currentSelfHeatKW;
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x00135FF8 File Offset: 0x001341F8
	public void SetUserSpecifiedPowerConsumptionValue(float value)
	{
		if (this.produceHeat)
		{
			this.UserSliderSetting = (value - this.MinPower) / (this.MaxPower - this.MinPower);
			SpaceHeater.RefreshHeatEffect(base.smi);
			this.energyConsumer.BaseWattageRating = this.CurrentPowerConsumption;
		}
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x00136048 File Offset: 0x00134248
	protected override void OnPrefabInit()
	{
		if (this.produceHeat)
		{
			this.heatStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.heatStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = string.Format(str, GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				return str;
			};
			this.heatStatusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = str.Replace("{0}", GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				string text = string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentSelfHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentExhaustedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				str = str.Replace("{1}", text);
				return str;
			};
		}
		base.OnPrefabInit();
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x001360E0 File Offset: 0x001342E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.extents = base.GetComponent<OccupyArea>().GetExtents();
		this.overheatTemperature = base.GetComponent<BuildingComplete>().Def.OverheatTemperature;
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.smi.StartSM();
		this.SetUserSpecifiedPowerConsumptionValue(this.CurrentPowerConsumption);
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x0013617D File Offset: 0x0013437D
	public void SetLiquidHeater()
	{
		this.heatLiquid = true;
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x00136188 File Offset: 0x00134388
	private SpaceHeater.MonitorState MonitorHeating(float dt)
	{
		this.monitorCells.Clear();
		GameUtil.GetNonSolidCells(Grid.PosToCell(base.transform.GetPosition()), this.radius, this.monitorCells);
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < this.monitorCells.Count; i++)
		{
			if (Grid.Mass[this.monitorCells[i]] > this.minimumCellMass && ((Grid.Element[this.monitorCells[i]].IsGas && !this.heatLiquid) || (Grid.Element[this.monitorCells[i]].IsLiquid && this.heatLiquid)))
			{
				num++;
				num2 += Grid.Temperature[this.monitorCells[i]];
			}
		}
		if (num == 0)
		{
			if (!this.heatLiquid)
			{
				return SpaceHeater.MonitorState.NotEnoughGas;
			}
			return SpaceHeater.MonitorState.NotEnoughLiquid;
		}
		else
		{
			if (num2 / (float)num >= this.targetTemperature)
			{
				return SpaceHeater.MonitorState.TooHot;
			}
			return SpaceHeater.MonitorState.ReadyToHeat;
		}
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x00136288 File Offset: 0x00134488
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06003719 RID: 14105 RVA: 0x001362ED File Offset: 0x001344ED
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TITLE";
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x0600371A RID: 14106 RVA: 0x001362F4 File Offset: 0x001344F4
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.ELECTRICAL.WATT;
		}
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x00136300 File Offset: 0x00134500
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x00136303 File Offset: 0x00134503
	public float GetSliderMin(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MinPower;
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x00136319 File Offset: 0x00134519
	public float GetSliderMax(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MaxPower;
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x0013632F File Offset: 0x0013452F
	public float GetSliderValue(int index)
	{
		return this.CurrentPowerConsumption;
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x00136337 File Offset: 0x00134537
	public void SetSliderValue(float value, int index)
	{
		this.SetUserSpecifiedPowerConsumptionValue(value);
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x00136340 File Offset: 0x00134540
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP";
	}

	// Token: 0x06003721 RID: 14113 RVA: 0x00136347 File Offset: 0x00134547
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP"), GameUtil.GetFormattedHeatEnergyRate((this.CurrentSelfHeatKW + this.CurrentExhaustedKW) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
	}

	// Token: 0x04002172 RID: 8562
	public float targetTemperature = 308.15f;

	// Token: 0x04002173 RID: 8563
	public float minimumCellMass;

	// Token: 0x04002174 RID: 8564
	public int radius = 2;

	// Token: 0x04002175 RID: 8565
	[SerializeField]
	private bool heatLiquid;

	// Token: 0x04002176 RID: 8566
	[Serialize]
	public float UserSliderSetting;

	// Token: 0x04002177 RID: 8567
	public bool produceHeat;

	// Token: 0x04002178 RID: 8568
	private StatusItem heatStatusItem;

	// Token: 0x04002179 RID: 8569
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x0400217A RID: 8570
	private Extents extents;

	// Token: 0x0400217B RID: 8571
	private float overheatTemperature;

	// Token: 0x0400217C RID: 8572
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400217D RID: 8573
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400217E RID: 8574
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x0400217F RID: 8575
	[MyCmpGet]
	private EnergyConsumer energyConsumer;

	// Token: 0x04002180 RID: 8576
	private List<int> monitorCells = new List<int>();

	// Token: 0x02001785 RID: 6021
	public class StatesInstance : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.GameInstance
	{
		// Token: 0x06009B67 RID: 39783 RVA: 0x00394BC5 File Offset: 0x00392DC5
		public StatesInstance(SpaceHeater master) : base(master)
		{
		}
	}

	// Token: 0x02001786 RID: 6022
	public class States : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater>
	{
		// Token: 0x06009B68 RID: 39784 RVA: 0x00394BD0 File Offset: 0x00392DD0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			base.serializable = StateMachine.SerializeType.Never;
			this.statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp = new StatusItem("statusItemOverTemp", BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp.resolveStringCallback = delegate(string str, object obj)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)obj;
				return string.Format(str, GameUtil.GetFormattedTemperature(statesInstance.master.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.offline.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).EventTransition(GameHashes.OperationalChanged, this.online, (SpaceHeater.StatesInstance smi) => smi.master.operational.IsOperational);
			this.online.EventTransition(GameHashes.OperationalChanged, this.offline, (SpaceHeater.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.online.heating).Update("spaceheater_online", delegate(SpaceHeater.StatesInstance smi, float dt)
			{
				switch (smi.master.MonitorHeating(dt))
				{
				case SpaceHeater.MonitorState.ReadyToHeat:
					smi.GoTo(this.online.heating);
					return;
				case SpaceHeater.MonitorState.TooHot:
					smi.GoTo(this.online.overtemp);
					return;
				case SpaceHeater.MonitorState.NotEnoughLiquid:
					smi.GoTo(this.online.undermassliquid);
					return;
				case SpaceHeater.MonitorState.NotEnoughGas:
					smi.GoTo(this.online.undermassgas);
					return;
				default:
					return;
				}
			}, UpdateRate.SIM_4000ms, false);
			this.online.heating.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).Enter(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).ToggleStatusItem((SpaceHeater.StatesInstance smi) => smi.master.heatStatusItem, (SpaceHeater.StatesInstance smi) => smi).Update(new Action<SpaceHeater.StatesInstance, float>(SpaceHeater.GenerateHeat), UpdateRate.SIM_200ms, false).Exit(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Exit(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect));
			this.online.undermassliquid.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassLiquid, null);
			this.online.undermassgas.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassGas, null);
			this.online.overtemp.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemOverTemp, null);
		}

		// Token: 0x040077E6 RID: 30694
		public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State offline;

		// Token: 0x040077E7 RID: 30695
		public SpaceHeater.States.OnlineStates online;

		// Token: 0x040077E8 RID: 30696
		private StatusItem statusItemUnderMassLiquid;

		// Token: 0x040077E9 RID: 30697
		private StatusItem statusItemUnderMassGas;

		// Token: 0x040077EA RID: 30698
		private StatusItem statusItemOverTemp;

		// Token: 0x02002946 RID: 10566
		public class OnlineStates : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State
		{
			// Token: 0x0400B693 RID: 46739
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State heating;

			// Token: 0x0400B694 RID: 46740
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State overtemp;

			// Token: 0x0400B695 RID: 46741
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassliquid;

			// Token: 0x0400B696 RID: 46742
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassgas;
		}
	}

	// Token: 0x02001787 RID: 6023
	private enum MonitorState
	{
		// Token: 0x040077EC RID: 30700
		ReadyToHeat,
		// Token: 0x040077ED RID: 30701
		TooHot,
		// Token: 0x040077EE RID: 30702
		NotEnoughLiquid,
		// Token: 0x040077EF RID: 30703
		NotEnoughGas
	}
}
