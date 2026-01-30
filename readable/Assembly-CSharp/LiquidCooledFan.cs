using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200078A RID: 1930
[SerializationConfig(MemberSerialization.OptIn)]
public class LiquidCooledFan : StateMachineComponent<LiquidCooledFan.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003145 RID: 12613 RVA: 0x0011C284 File Offset: 0x0011A484
	public bool HasMaterial()
	{
		ListPool<GameObject, LiquidCooledFan>.PooledList pooledList = ListPool<GameObject, LiquidCooledFan>.Allocate();
		base.smi.master.gasStorage.Find(GameTags.Water, pooledList);
		if (pooledList.Count > 0)
		{
			global::Debug.LogWarning("Liquid Cooled fan Gas storage contains water - A duplicant probably delivered to the wrong storage - moving it to liquid storage.");
			foreach (GameObject go in pooledList)
			{
				base.smi.master.gasStorage.Transfer(go, base.smi.master.liquidStorage, false, false);
			}
		}
		pooledList.Recycle();
		this.UpdateMeter();
		return this.liquidStorage.MassStored() > 0f;
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x0011C348 File Offset: 0x0011A548
	public void CheckWorking()
	{
		if (base.smi.master.workable.worker == null)
		{
			base.smi.GoTo(base.smi.sm.unworkable);
		}
	}

	// Token: 0x06003147 RID: 12615 RVA: 0x0011C384 File Offset: 0x0011A584
	private void UpdateUnworkableStatusItems()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (!base.smi.EnvironmentNeedsCooling())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, null);
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, false);
		}
		if (!base.smi.EnvironmentHighEnoughPressure())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.UnderPressure, this.minEnvironmentMass);
				return;
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.UnderPressure, false);
		}
	}

	// Token: 0x06003148 RID: 12616 RVA: 0x0011C478 File Offset: 0x0011A678
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_waterbody",
			"meter_waterlevel"
		});
		base.GetComponent<ElementConsumer>().EnableConsumption(true);
		base.smi.StartSM();
		base.smi.master.waterConsumptionAccumulator = Game.Instance.accumulators.Add("waterConsumptionAccumulator", this);
		base.GetComponent<ElementConsumer>().storage = this.gasStorage;
		base.GetComponent<ManualDeliveryKG>().SetStorage(this.liquidStorage);
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x0011C525 File Offset: 0x0011A725
	private void UpdateMeter()
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.liquidStorage.MassStored() / this.liquidStorage.capacityKg));
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x0011C550 File Offset: 0x0011A750
	private void EmitContents()
	{
		if (this.gasStorage.items.Count == 0)
		{
			return;
		}
		float num = 0.1f;
		PrimaryElement primaryElement = null;
		for (int i = 0; i < this.gasStorage.items.Count; i++)
		{
			PrimaryElement component = this.gasStorage.items[i].GetComponent<PrimaryElement>();
			if (component.Mass > num && component.Element.IsGas)
			{
				primaryElement = component;
				num = primaryElement.Mass;
			}
		}
		if (primaryElement != null)
		{
			SimMessages.AddRemoveSubstance(Grid.CellRight(Grid.CellAbove(Grid.PosToCell(base.gameObject))), ElementLoader.GetElementIndex(primaryElement.ElementID), CellEventLogger.Instance.ExhaustSimUpdate, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
			this.gasStorage.ConsumeIgnoringDisease(primaryElement.gameObject);
		}
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x0011C62C File Offset: 0x0011A82C
	private void CoolContents(float dt)
	{
		if (this.gasStorage.items.Count == 0)
		{
			return;
		}
		float num = float.PositiveInfinity;
		float num2 = 0f;
		foreach (GameObject gameObject in this.gasStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (!(component == null) && component.Mass >= 0.1f && component.Temperature >= this.minCooledTemperature)
			{
				float thermalEnergy = GameUtil.GetThermalEnergy(component);
				if (num > thermalEnergy)
				{
					num = thermalEnergy;
				}
			}
		}
		foreach (GameObject gameObject2 in this.gasStorage.items)
		{
			PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
			if (!(component == null) && component.Mass >= 0.1f && component.Temperature >= this.minCooledTemperature)
			{
				float num3 = Mathf.Min(num, 10f);
				GameUtil.DeltaThermalEnergy(component, -num3, this.minCooledTemperature);
				num2 += num3;
			}
		}
		float num4 = Mathf.Abs(num2 * this.waterKGConsumedPerKJ);
		Game.Instance.accumulators.Accumulate(base.smi.master.waterConsumptionAccumulator, num4);
		if (num4 != 0f)
		{
			float num5;
			SimUtil.DiseaseInfo diseaseInfo;
			float num6;
			this.liquidStorage.ConsumeAndGetDisease(GameTags.Water, num4, out num5, out diseaseInfo, out num6);
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(base.gameObject), diseaseInfo.idx, diseaseInfo.count);
			this.UpdateMeter();
		}
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x0011C7DC File Offset: 0x0011A9DC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATCONSUMED, GameUtil.GetFormattedHeatEnergy(this.coolingKilowatts, GameUtil.HeatEnergyFormatterUnit.Automatic)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATCONSUMED, GameUtil.GetFormattedHeatEnergy(this.coolingKilowatts, GameUtil.HeatEnergyFormatterUnit.Automatic)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04001D94 RID: 7572
	[SerializeField]
	public float coolingKilowatts;

	// Token: 0x04001D95 RID: 7573
	[SerializeField]
	public float minCooledTemperature;

	// Token: 0x04001D96 RID: 7574
	[SerializeField]
	public float minEnvironmentMass;

	// Token: 0x04001D97 RID: 7575
	[SerializeField]
	public float waterKGConsumedPerKJ;

	// Token: 0x04001D98 RID: 7576
	[SerializeField]
	public Vector2I minCoolingRange;

	// Token: 0x04001D99 RID: 7577
	[SerializeField]
	public Vector2I maxCoolingRange;

	// Token: 0x04001D9A RID: 7578
	private float flowRate = 0.3f;

	// Token: 0x04001D9B RID: 7579
	[SerializeField]
	public Storage gasStorage;

	// Token: 0x04001D9C RID: 7580
	[SerializeField]
	public Storage liquidStorage;

	// Token: 0x04001D9D RID: 7581
	[MyCmpAdd]
	private LiquidCooledFanWorkable workable;

	// Token: 0x04001D9E RID: 7582
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001D9F RID: 7583
	private HandleVector<int>.Handle waterConsumptionAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001DA0 RID: 7584
	private MeterController meter;

	// Token: 0x020016AC RID: 5804
	public class StatesInstance : GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.GameInstance
	{
		// Token: 0x06009821 RID: 38945 RVA: 0x00387535 File Offset: 0x00385735
		public StatesInstance(LiquidCooledFan smi) : base(smi)
		{
		}

		// Token: 0x06009822 RID: 38946 RVA: 0x00387540 File Offset: 0x00385740
		public bool IsWorkable()
		{
			bool result = false;
			if (base.master.operational.IsOperational && this.EnvironmentNeedsCooling() && base.smi.master.HasMaterial() && base.smi.EnvironmentHighEnoughPressure())
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06009823 RID: 38947 RVA: 0x0038758C File Offset: 0x0038578C
		public bool EnvironmentNeedsCooling()
		{
			bool result = false;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Temperature[i2] > base.master.minCooledTemperature)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06009824 RID: 38948 RVA: 0x00387634 File Offset: 0x00385834
		public bool EnvironmentHighEnoughPressure()
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Mass[i2] >= base.master.minEnvironmentMass)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x020016AD RID: 5805
	public class States : GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan>
	{
		// Token: 0x06009825 RID: 38949 RVA: 0x003876D4 File Offset: 0x003858D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unworkable;
			this.root.Enter(delegate(LiquidCooledFan.StatesInstance smi)
			{
				smi.master.workable.SetWorkTime(float.PositiveInfinity);
			});
			this.workable.ToggleChore(new Func<LiquidCooledFan.StatesInstance, Chore>(this.CreateUseChore), this.work_pst).EventTransition(GameHashes.ActiveChanged, this.workable.consuming, (LiquidCooledFan.StatesInstance smi) => smi.master.workable.worker != null).EventTransition(GameHashes.OperationalChanged, this.workable.consuming, (LiquidCooledFan.StatesInstance smi) => smi.master.workable.worker != null).Transition(this.unworkable, (LiquidCooledFan.StatesInstance smi) => !smi.IsWorkable(), UpdateRate.SIM_200ms);
			this.work_pst.Update("LiquidFanEmitCooledContents", delegate(LiquidCooledFan.StatesInstance smi, float dt)
			{
				smi.master.EmitContents();
			}, UpdateRate.SIM_200ms, false).ScheduleGoTo(2f, this.unworkable);
			this.unworkable.Update("LiquidFanEmitCooledContents", delegate(LiquidCooledFan.StatesInstance smi, float dt)
			{
				smi.master.EmitContents();
			}, UpdateRate.SIM_200ms, false).Update("LiquidFanUnworkableStatusItems", delegate(LiquidCooledFan.StatesInstance smi, float dt)
			{
				smi.master.UpdateUnworkableStatusItems();
			}, UpdateRate.SIM_200ms, false).Transition(this.workable.waiting, (LiquidCooledFan.StatesInstance smi) => smi.IsWorkable(), UpdateRate.SIM_200ms).Enter(delegate(LiquidCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			}).Exit(delegate(LiquidCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			});
			this.workable.consuming.EventTransition(GameHashes.OperationalChanged, this.unworkable, (LiquidCooledFan.StatesInstance smi) => smi.master.workable.worker == null).EventHandler(GameHashes.ActiveChanged, delegate(LiquidCooledFan.StatesInstance smi)
			{
				smi.master.CheckWorking();
			}).Enter(delegate(LiquidCooledFan.StatesInstance smi)
			{
				if (!smi.EnvironmentNeedsCooling() || !smi.master.HasMaterial() || !smi.EnvironmentHighEnoughPressure())
				{
					smi.GoTo(this.unworkable);
				}
				ElementConsumer component = smi.master.GetComponent<ElementConsumer>();
				component.consumptionRate = smi.master.flowRate;
				component.RefreshConsumptionRate();
			}).Update(delegate(LiquidCooledFan.StatesInstance smi, float dt)
			{
				smi.master.CoolContents(dt);
			}, UpdateRate.SIM_200ms, false).ScheduleGoTo(12f, this.workable.emitting).Exit(delegate(LiquidCooledFan.StatesInstance smi)
			{
				ElementConsumer component = smi.master.GetComponent<ElementConsumer>();
				component.consumptionRate = 0f;
				component.RefreshConsumptionRate();
			});
			this.workable.emitting.EventTransition(GameHashes.ActiveChanged, this.unworkable, (LiquidCooledFan.StatesInstance smi) => smi.master.workable.worker == null).EventTransition(GameHashes.OperationalChanged, this.unworkable, (LiquidCooledFan.StatesInstance smi) => smi.master.workable.worker == null).ScheduleGoTo(3f, this.workable.consuming).Update(delegate(LiquidCooledFan.StatesInstance smi, float dt)
			{
				smi.master.CoolContents(dt);
			}, UpdateRate.SIM_200ms, false).Update("LiquidFanEmitCooledContents", delegate(LiquidCooledFan.StatesInstance smi, float dt)
			{
				smi.master.EmitContents();
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x06009826 RID: 38950 RVA: 0x00387A80 File Offset: 0x00385C80
		private Chore CreateUseChore(LiquidCooledFan.StatesInstance smi)
		{
			return new WorkChore<LiquidCooledFanWorkable>(Db.Get().ChoreTypes.LiquidCooledFan, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04007590 RID: 30096
		public LiquidCooledFan.States.Workable workable;

		// Token: 0x04007591 RID: 30097
		public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State unworkable;

		// Token: 0x04007592 RID: 30098
		public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State work_pst;

		// Token: 0x02002908 RID: 10504
		public class Workable : GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State
		{
			// Token: 0x0400B54F RID: 46415
			public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State waiting;

			// Token: 0x0400B550 RID: 46416
			public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State consuming;

			// Token: 0x0400B551 RID: 46417
			public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State emitting;
		}
	}
}
