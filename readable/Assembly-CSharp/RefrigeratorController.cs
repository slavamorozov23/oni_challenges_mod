using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007EA RID: 2026
public class RefrigeratorController : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>
{
	// Token: 0x0600361B RID: 13851 RVA: 0x001310DC File Offset: 0x0012F2DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.DefaultState(this.operational.steady).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Not(new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational))).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(false, false);
		});
		this.operational.cooling.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplyCoolingExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.steady, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AllFoodCool), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeCooling, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main);
		this.operational.steady.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplySteadyExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.cooling, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AnyWarmFood), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeSteady, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(true);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(false);
		});
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x0013130C File Offset: 0x0012F50C
	private bool AllFoodCool(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStopBuffer)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x001313AC File Offset: 0x0012F5AC
	private bool AnyWarmFood(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStartBuffer)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600361E RID: 13854 RVA: 0x0013144C File Offset: 0x0012F64C
	private bool IsOperational(RefrigeratorController.StatesInstance smi)
	{
		return smi.operational.IsOperational;
	}

	// Token: 0x040020EA RID: 8426
	public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State inoperational;

	// Token: 0x040020EB RID: 8427
	public RefrigeratorController.OperationalStates operational;

	// Token: 0x02001752 RID: 5970
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009AC7 RID: 39623 RVA: 0x00392BF8 File Offset: 0x00390DF8
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.AddRange(SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature));
			Descriptor item = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.coolingHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item);
			return list;
		}

		// Token: 0x0400776F RID: 30575
		public float activeCoolingStartBuffer = 2f;

		// Token: 0x04007770 RID: 30576
		public float activeCoolingStopBuffer = 0.1f;

		// Token: 0x04007771 RID: 30577
		public float simulatedInternalTemperature = 274.15f;

		// Token: 0x04007772 RID: 30578
		public float simulatedInternalHeatCapacity = 400f;

		// Token: 0x04007773 RID: 30579
		public float simulatedThermalConductivity = 1000f;

		// Token: 0x04007774 RID: 30580
		public float powerSaverEnergyUsage;

		// Token: 0x04007775 RID: 30581
		public float coolingHeatKW;

		// Token: 0x04007776 RID: 30582
		public float steadyHeatKW;
	}

	// Token: 0x02001753 RID: 5971
	public class OperationalStates : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State
	{
		// Token: 0x04007777 RID: 30583
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State cooling;

		// Token: 0x04007778 RID: 30584
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State steady;
	}

	// Token: 0x02001754 RID: 5972
	public class StatesInstance : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.GameInstance
	{
		// Token: 0x06009ACA RID: 39626 RVA: 0x00392CAC File Offset: 0x00390EAC
		public StatesInstance(IStateMachineTarget master, RefrigeratorController.Def def) : base(master, def)
		{
			this.temperatureAdjuster = new SimulatedTemperatureAdjuster(def.simulatedInternalTemperature, def.simulatedInternalHeatCapacity, def.simulatedThermalConductivity, this.storage);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		}

		// Token: 0x06009ACB RID: 39627 RVA: 0x00392CFA File Offset: 0x00390EFA
		protected override void OnCleanUp()
		{
			this.temperatureAdjuster.CleanUp();
			base.OnCleanUp();
		}

		// Token: 0x06009ACC RID: 39628 RVA: 0x00392D0D File Offset: 0x00390F0D
		public float GetSaverPower()
		{
			return base.def.powerSaverEnergyUsage;
		}

		// Token: 0x06009ACD RID: 39629 RVA: 0x00392D1A File Offset: 0x00390F1A
		public float GetNormalPower()
		{
			return base.GetComponent<EnergyConsumer>().WattsNeededWhenActive;
		}

		// Token: 0x06009ACE RID: 39630 RVA: 0x00392D28 File Offset: 0x00390F28
		public void SetEnergySaver(bool energySaving)
		{
			EnergyConsumer component = base.GetComponent<EnergyConsumer>();
			if (energySaving)
			{
				component.BaseWattageRating = this.GetSaverPower();
				return;
			}
			component.BaseWattageRating = this.GetNormalPower();
		}

		// Token: 0x06009ACF RID: 39631 RVA: 0x00392D58 File Offset: 0x00390F58
		public void ApplyCoolingExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.coolingHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x06009AD0 RID: 39632 RVA: 0x00392D82 File Offset: 0x00390F82
		public void ApplySteadyExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.steadyHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x04007779 RID: 30585
		[MyCmpReq]
		public Operational operational;

		// Token: 0x0400777A RID: 30586
		[MyCmpReq]
		public Storage storage;

		// Token: 0x0400777B RID: 30587
		private HandleVector<int>.Handle structureTemperature;

		// Token: 0x0400777C RID: 30588
		private SimulatedTemperatureAdjuster temperatureAdjuster;
	}
}
