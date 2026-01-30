using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x0200077F RID: 1919
public class IceKettle : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>
{
	// Token: 0x060030E7 RID: 12519 RVA: 0x0011A40C File Offset: 0x0011860C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.root.EventHandlerTransition(GameHashes.WorkableStartWork, this.inUse, (IceKettle.Instance smi, object obj) => true).EventHandler(GameHashes.OnStorageChange, delegate(IceKettle.Instance smi)
		{
			smi.UpdateMeter();
		});
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.idle);
		this.operational.idle.PlayAnim(IceKettle.IDEL_ANIM_STATE).DefaultState(this.operational.idle.waitingForSolids);
		this.operational.idle.waitingForSolids.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientSolids, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.waitingForSpaceInLiquidTank, (IceKettle.Instance smi) => IceKettle.HasEnoughSolidsToMelt(smi));
		this.operational.idle.waitingForSpaceInLiquidTank.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientLiquidSpace, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.notEnoughFuel, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.LiquidTankHasCapacityForNextBatch));
		this.operational.idle.notEnoughFuel.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientFuel, null).EventTransition(GameHashes.OnStorageChange, this.operational.melting, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch));
		this.operational.melting.Toggle("Operational Active State", new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesTrue), new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesFalse)).DefaultState(this.operational.melting.entering);
		this.operational.melting.entering.PlayAnim(IceKettle.BOILING_PRE_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.melting.working);
		this.operational.melting.working.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleMelting, null).DefaultState(this.operational.melting.working.idle).PlayAnim(IceKettle.BOILING_LOOP_ANIM_NAME, KAnim.PlayMode.Loop);
		this.operational.melting.working.idle.ParamTransition<float>(this.MeltingTimer, this.operational.melting.working.complete, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Parameter<float>.Callback(IceKettle.IsDoneMelting)).Update(new Action<IceKettle.Instance, float>(IceKettle.MeltingTimerUpdate), UpdateRate.SIM_200ms, false);
		this.operational.melting.working.complete.Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.ResetMeltingTimer)).Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.MeltNextBatch)).EnterTransition(this.operational.melting.working.idle, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)).EnterTransition(this.operational.melting.exit, GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Not(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)));
		this.operational.melting.exit.PlayAnim(IceKettle.BOILING_PST_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.idle);
		this.inUse.EventHandlerTransition(GameHashes.WorkableStopWork, this.noOperational, (IceKettle.Instance smi, object obj) => true).ScheduleGoTo(new Func<IceKettle.Instance, float>(IceKettle.GetInUseTimeout), this.noOperational);
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x0011A7F5 File Offset: 0x001189F5
	public static void SetOperationalActiveStatesTrue(IceKettle.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x0011A804 File Offset: 0x00118A04
	public static void SetOperationalActiveStatesFalse(IceKettle.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x0011A813 File Offset: 0x00118A13
	public static float GetInUseTimeout(IceKettle.Instance smi)
	{
		return smi.InUseWorkableDuration + 1f;
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x0011A821 File Offset: 0x00118A21
	public static void ResetMeltingTimer(IceKettle.Instance smi)
	{
		smi.sm.MeltingTimer.Set(0f, smi, false);
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x0011A83B File Offset: 0x00118A3B
	public static bool HasEnoughSolidsToMelt(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt;
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x0011A843 File Offset: 0x00118A43
	public static bool LiquidTankHasCapacityForNextBatch(IceKettle.Instance smi)
	{
		return smi.LiquidTankHasCapacityForNextBatch;
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x0011A84B File Offset: 0x00118A4B
	public static bool HasEnoughFuelForNextBacth(IceKettle.Instance smi)
	{
		return smi.HasEnoughFuelUnitsToMeltNextBatch;
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x0011A853 File Offset: 0x00118A53
	public static bool CanMeltNextBatch(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt && IceKettle.LiquidTankHasCapacityForNextBatch(smi) && IceKettle.HasEnoughFuelForNextBacth(smi);
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x0011A86D File Offset: 0x00118A6D
	public static bool IsDoneMelting(IceKettle.Instance smi, float timePassed)
	{
		return timePassed >= smi.MeltDurationPerBatch;
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x0011A87C File Offset: 0x00118A7C
	public static void MeltingTimerUpdate(IceKettle.Instance smi, float dt)
	{
		float num = smi.sm.MeltingTimer.Get(smi);
		smi.sm.MeltingTimer.Set(num + dt, smi, false);
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x0011A8B1 File Offset: 0x00118AB1
	public static void MeltNextBatch(IceKettle.Instance smi)
	{
		smi.MeltNextBatch();
	}

	// Token: 0x04001D47 RID: 7495
	public static string LIQUID_METER_TARGET_NAME = "kettle_meter_target";

	// Token: 0x04001D48 RID: 7496
	public static string LIQUID_METER_ANIM_NAME = "meter_kettle";

	// Token: 0x04001D49 RID: 7497
	public static string IDEL_ANIM_STATE = "on";

	// Token: 0x04001D4A RID: 7498
	public static string BOILING_PRE_ANIM_NAME = "boiling_pre";

	// Token: 0x04001D4B RID: 7499
	public static string BOILING_LOOP_ANIM_NAME = "boiling_loop";

	// Token: 0x04001D4C RID: 7500
	public static string BOILING_PST_ANIM_NAME = "boiling_pst";

	// Token: 0x04001D4D RID: 7501
	private const float InUseTimeout = 5f;

	// Token: 0x04001D4E RID: 7502
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State noOperational;

	// Token: 0x04001D4F RID: 7503
	public IceKettle.OperationalStates operational;

	// Token: 0x04001D50 RID: 7504
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State inUse;

	// Token: 0x04001D51 RID: 7505
	public StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.FloatParameter MeltingTimer;

	// Token: 0x02001698 RID: 5784
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060097E4 RID: 38884 RVA: 0x003868A4 File Offset: 0x00384AA4
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			string txt = string.Format(UI.BUILDINGEFFECTS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGMeltedPerSecond, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string tooltip = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGToMeltPerBatch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			Descriptor item = new Descriptor(txt, tooltip, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}

		// Token: 0x04007555 RID: 30037
		public SimHashes exhaust_tag;

		// Token: 0x04007556 RID: 30038
		public Tag targetElementTag;

		// Token: 0x04007557 RID: 30039
		public Tag fuelElementTag;

		// Token: 0x04007558 RID: 30040
		public float KGToMeltPerBatch;

		// Token: 0x04007559 RID: 30041
		public float KGMeltedPerSecond;

		// Token: 0x0400755A RID: 30042
		public float TargetTemperature;

		// Token: 0x0400755B RID: 30043
		public float EnergyPerUnitOfLumber;

		// Token: 0x0400755C RID: 30044
		public float ExhaustMassPerUnitOfLumber;
	}

	// Token: 0x02001699 RID: 5785
	public class WorkingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x0400755D RID: 30045
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State idle;

		// Token: 0x0400755E RID: 30046
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State complete;
	}

	// Token: 0x0200169A RID: 5786
	public class MeltingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x0400755F RID: 30047
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State entering;

		// Token: 0x04007560 RID: 30048
		public IceKettle.WorkingStates working;

		// Token: 0x04007561 RID: 30049
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State exit;
	}

	// Token: 0x0200169B RID: 5787
	public class IdleStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x04007562 RID: 30050
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State notEnoughFuel;

		// Token: 0x04007563 RID: 30051
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSolids;

		// Token: 0x04007564 RID: 30052
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSpaceInLiquidTank;
	}

	// Token: 0x0200169C RID: 5788
	public class OperationalStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x04007565 RID: 30053
		public IceKettle.MeltingStates melting;

		// Token: 0x04007566 RID: 30054
		public IceKettle.IdleStates idle;
	}

	// Token: 0x0200169D RID: 5789
	public new class Instance : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.GameInstance
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x060097EA RID: 38890 RVA: 0x00386945 File Offset: 0x00384B45
		public float CurrentTemperatureOfSolidsStored
		{
			get
			{
				if (this.kettleStorage.MassStored() <= 0f)
				{
					return 0f;
				}
				return this.kettleStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x060097EB RID: 38891 RVA: 0x0038697A File Offset: 0x00384B7A
		public float MeltDurationPerBatch
		{
			get
			{
				return base.def.KGToMeltPerBatch / base.def.KGMeltedPerSecond;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x060097EC RID: 38892 RVA: 0x00386993 File Offset: 0x00384B93
		public float FuelUnitsAvailable
		{
			get
			{
				return this.fuelStorage.MassStored();
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x060097ED RID: 38893 RVA: 0x003869A0 File Offset: 0x00384BA0
		public bool HasAtLeastOneBatchOfSolidsWaitingToMelt
		{
			get
			{
				return this.kettleStorage.MassStored() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x060097EE RID: 38894 RVA: 0x003869BD File Offset: 0x00384BBD
		public bool HasEnoughFuelUnitsToMeltNextBatch
		{
			get
			{
				return this.kettleStorage.MassStored() <= 0f || this.FuelUnitsAvailable >= this.FuelRequiredForNextBratch;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x060097EF RID: 38895 RVA: 0x003869E4 File Offset: 0x00384BE4
		public bool LiquidTankHasCapacityForNextBatch
		{
			get
			{
				return this.outputStorage.RemainingCapacity() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x060097F0 RID: 38896 RVA: 0x00386A01 File Offset: 0x00384C01
		public float LiquidTankCapacity
		{
			get
			{
				return this.outputStorage.capacityKg;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x060097F1 RID: 38897 RVA: 0x00386A0E File Offset: 0x00384C0E
		public float LiquidStored
		{
			get
			{
				return this.outputStorage.MassStored();
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x060097F2 RID: 38898 RVA: 0x00386A1B File Offset: 0x00384C1B
		public float FuelRequiredForNextBratch
		{
			get
			{
				return this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, this.CurrentTemperatureOfSolidsStored);
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x060097F3 RID: 38899 RVA: 0x00386A3A File Offset: 0x00384C3A
		public float InUseWorkableDuration
		{
			get
			{
				return this.dupeWorkable.workTime;
			}
		}

		// Token: 0x060097F4 RID: 38900 RVA: 0x00386A48 File Offset: 0x00384C48
		public Instance(IStateMachineTarget master, IceKettle.Def def) : base(master, def)
		{
			this.elementToMelt = ElementLoader.GetElement(def.targetElementTag);
			this.LiquidMeter = new MeterController(this.animController, IceKettle.LIQUID_METER_TARGET_NAME, IceKettle.LIQUID_METER_ANIM_NAME, Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.fuelStorage = components[0];
			this.kettleStorage = components[1];
			this.outputStorage = components[2];
		}

		// Token: 0x060097F5 RID: 38901 RVA: 0x00386AB8 File Offset: 0x00384CB8
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateMeter();
		}

		// Token: 0x060097F6 RID: 38902 RVA: 0x00386AC6 File Offset: 0x00384CC6
		public void UpdateMeter()
		{
			this.LiquidMeter.SetPositionPercent(this.outputStorage.MassStored() / this.outputStorage.capacityKg);
		}

		// Token: 0x060097F7 RID: 38903 RVA: 0x00386AEC File Offset: 0x00384CEC
		public void MeltNextBatch()
		{
			if (!this.HasAtLeastOneBatchOfSolidsWaitingToMelt)
			{
				return;
			}
			PrimaryElement component = this.kettleStorage.FindFirst(base.def.targetElementTag).GetComponent<PrimaryElement>();
			float num = Mathf.Min(this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, component.Temperature), this.FuelUnitsAvailable);
			float mass = 0f;
			float num2 = 0f;
			SimUtil.DiseaseInfo diseaseInfo;
			this.kettleStorage.ConsumeAndGetDisease(this.elementToMelt.id.CreateTag(), base.def.KGToMeltPerBatch, out mass, out diseaseInfo, out num2);
			this.outputStorage.AddElement(this.elementToMelt.highTempTransitionTarget, mass, base.def.TargetTemperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			float temperature = this.fuelStorage.FindFirst(base.def.fuelElementTag).GetComponent<PrimaryElement>().Temperature;
			this.fuelStorage.ConsumeIgnoringDisease(base.def.fuelElementTag, num);
			float mass2 = num * base.def.ExhaustMassPerUnitOfLumber;
			Element element = ElementLoader.FindElementByHash(base.def.exhaust_tag);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.gameObject), element.id, null, mass2, temperature, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x060097F8 RID: 38904 RVA: 0x00386C30 File Offset: 0x00384E30
		public float GetUnitsOfFuelRequiredToMelt(Element elementToMelt, float massToMelt_KG, float elementToMelt_initialTemperature)
		{
			if (!elementToMelt.IsSolid)
			{
				return -1f;
			}
			float num = massToMelt_KG * elementToMelt.specificHeatCapacity * elementToMelt_initialTemperature;
			float targetTemperature = base.def.TargetTemperature;
			return (massToMelt_KG * elementToMelt.specificHeatCapacity * targetTemperature - num) / base.def.EnergyPerUnitOfLumber;
		}

		// Token: 0x04007567 RID: 30055
		private Storage fuelStorage;

		// Token: 0x04007568 RID: 30056
		private Storage kettleStorage;

		// Token: 0x04007569 RID: 30057
		private Storage outputStorage;

		// Token: 0x0400756A RID: 30058
		private Element elementToMelt;

		// Token: 0x0400756B RID: 30059
		private MeterController LiquidMeter;

		// Token: 0x0400756C RID: 30060
		[MyCmpGet]
		public Operational operational;

		// Token: 0x0400756D RID: 30061
		[MyCmpGet]
		private IceKettleWorkable dupeWorkable;

		// Token: 0x0400756E RID: 30062
		[MyCmpGet]
		private KBatchedAnimController animController;
	}
}
