using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200077E RID: 1918
[SerializationConfig(MemberSerialization.OptIn)]
public class IceCooledFan : StateMachineComponent<IceCooledFan.StatesInstance>
{
	// Token: 0x060030E0 RID: 12512 RVA: 0x00119FCE File Offset: 0x001181CE
	public bool HasMaterial()
	{
		this.UpdateMeter();
		return this.iceStorage.MassStored() > 0f;
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x00119FE8 File Offset: 0x001181E8
	public void CheckWorking()
	{
		if (base.smi.master.workable.worker == null)
		{
			base.smi.GoTo(base.smi.sm.unworkable);
		}
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x0011A024 File Offset: 0x00118224
	private void UpdateUnworkableStatusItems()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (!base.smi.EnvironmentNeedsCooling())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, this.minCooledTemperature);
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

	// Token: 0x060030E3 RID: 12515 RVA: 0x0011A124 File Offset: 0x00118324
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_waterbody",
			"meter_waterlevel"
		});
		base.smi.StartSM();
		base.GetComponent<ManualDeliveryKG>().SetStorage(this.iceStorage);
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x0011A190 File Offset: 0x00118390
	private void UpdateMeter()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.iceStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			num += component.Temperature;
		}
		num /= (float)this.iceStorage.items.Count;
		float num2 = Mathf.Clamp01((num - this.LOW_ICE_TEMP) / (this.targetTemperature - this.LOW_ICE_TEMP));
		this.meter.SetPositionPercent(1f - num2);
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x0011A238 File Offset: 0x00118438
	private void DoCooling(float dt)
	{
		float kilowatts = this.coolingRate * dt;
		foreach (GameObject gameObject in this.iceStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), kilowatts, this.targetTemperature);
		}
		for (int i = this.iceStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.iceStorage.items[i - 1];
			if (gameObject2 != null && gameObject2.GetComponent<PrimaryElement>().Temperature > gameObject2.GetComponent<PrimaryElement>().Element.highTemp && gameObject2.GetComponent<PrimaryElement>().Element.HasTransitionUp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.iceStorage.AddLiquid(component.Element.highTempTransitionTarget, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.iceStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		for (int j = this.iceStorage.items.Count; j > 0; j--)
		{
			GameObject gameObject3 = this.iceStorage.items[j - 1];
			if (gameObject3 != null && gameObject3.GetComponent<PrimaryElement>().Temperature >= this.targetTemperature)
			{
				this.iceStorage.Transfer(gameObject3, this.liquidStorage, true, true);
			}
		}
		if (!this.liquidStorage.IsEmpty())
		{
			this.liquidStorage.DropAll(false, false, new Vector3(1f, 0f, 0f), true, null);
		}
		this.UpdateMeter();
	}

	// Token: 0x04001D39 RID: 7481
	[SerializeField]
	public float minCooledTemperature;

	// Token: 0x04001D3A RID: 7482
	[SerializeField]
	public float minEnvironmentMass;

	// Token: 0x04001D3B RID: 7483
	[SerializeField]
	public float coolingRate;

	// Token: 0x04001D3C RID: 7484
	[SerializeField]
	public float targetTemperature;

	// Token: 0x04001D3D RID: 7485
	[SerializeField]
	public Vector2I minCoolingRange;

	// Token: 0x04001D3E RID: 7486
	[SerializeField]
	public Vector2I maxCoolingRange;

	// Token: 0x04001D3F RID: 7487
	[SerializeField]
	public Storage iceStorage;

	// Token: 0x04001D40 RID: 7488
	[SerializeField]
	public Storage liquidStorage;

	// Token: 0x04001D41 RID: 7489
	[SerializeField]
	public Tag consumptionTag;

	// Token: 0x04001D42 RID: 7490
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x04001D43 RID: 7491
	private float LOW_ICE_TEMP = 173.15f;

	// Token: 0x04001D44 RID: 7492
	[MyCmpAdd]
	private IceCooledFanWorkable workable;

	// Token: 0x04001D45 RID: 7493
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001D46 RID: 7494
	private MeterController meter;

	// Token: 0x02001696 RID: 5782
	public class StatesInstance : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.GameInstance
	{
		// Token: 0x060097DB RID: 38875 RVA: 0x003863D1 File Offset: 0x003845D1
		public StatesInstance(IceCooledFan smi) : base(smi)
		{
		}

		// Token: 0x060097DC RID: 38876 RVA: 0x003863DC File Offset: 0x003845DC
		public bool IsWorkable()
		{
			bool result = false;
			if (base.master.operational.IsOperational && this.EnvironmentNeedsCooling() && base.smi.master.HasMaterial() && base.smi.EnvironmentHighEnoughPressure())
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060097DD RID: 38877 RVA: 0x00386428 File Offset: 0x00384628
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

		// Token: 0x060097DE RID: 38878 RVA: 0x003864D0 File Offset: 0x003846D0
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

	// Token: 0x02001697 RID: 5783
	public class States : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan>
	{
		// Token: 0x060097DF RID: 38879 RVA: 0x00386570 File Offset: 0x00384770
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unworkable;
			this.root.Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.workable.SetWorkTime(float.PositiveInfinity);
			});
			this.workable.ToggleChore(new Func<IceCooledFan.StatesInstance, Chore>(IceCooledFan.States.CreateUseChore), new Action<IceCooledFan.StatesInstance, Chore>(IceCooledFan.States.SetRemoteChore), this.work_pst).EventTransition(GameHashes.ActiveChanged, this.workable.cooling, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker != null).EventTransition(GameHashes.OperationalChanged, this.workable.cooling, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker != null).Transition(this.unworkable, (IceCooledFan.StatesInstance smi) => !smi.IsWorkable(), UpdateRate.SIM_200ms);
			this.workable.cooling.EventTransition(GameHashes.OperationalChanged, this.unworkable, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker == null).EventHandler(GameHashes.ActiveChanged, delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.CheckWorking();
			}).Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
				if (!smi.EnvironmentNeedsCooling() || !smi.master.HasMaterial() || !smi.EnvironmentHighEnoughPressure())
				{
					smi.GoTo(this.unworkable);
				}
			}).Update("IceCooledFanCooling", delegate(IceCooledFan.StatesInstance smi, float dt)
			{
				smi.master.DoCooling(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(IceCooledFan.StatesInstance smi)
			{
				if (!smi.master.HasMaterial())
				{
					smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Working");
				}
				smi.master.liquidStorage.DropAll(false, false, default(Vector3), true, null);
			});
			this.work_pst.ScheduleGoTo(2f, this.unworkable);
			this.unworkable.Update("IceFanUnworkableStatusItems", delegate(IceCooledFan.StatesInstance smi, float dt)
			{
				smi.master.UpdateUnworkableStatusItems();
			}, UpdateRate.SIM_200ms, false).Transition(this.workable.waiting, (IceCooledFan.StatesInstance smi) => smi.IsWorkable(), UpdateRate.SIM_200ms).Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			}).Exit(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			});
		}

		// Token: 0x060097E0 RID: 38880 RVA: 0x003867F2 File Offset: 0x003849F2
		private static void SetRemoteChore(IceCooledFan.StatesInstance smi, Chore chore)
		{
			smi.master.remoteChore.SetChore(chore);
		}

		// Token: 0x060097E1 RID: 38881 RVA: 0x00386808 File Offset: 0x00384A08
		private static Chore CreateUseChore(IceCooledFan.StatesInstance smi)
		{
			return new WorkChore<IceCooledFanWorkable>(Db.Get().ChoreTypes.IceCooledFan, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04007552 RID: 30034
		public IceCooledFan.States.Workable workable;

		// Token: 0x04007553 RID: 30035
		public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State unworkable;

		// Token: 0x04007554 RID: 30036
		public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State work_pst;

		// Token: 0x02002902 RID: 10498
		public class Workable : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State
		{
			// Token: 0x0400B52A RID: 46378
			public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State waiting;

			// Token: 0x0400B52B RID: 46379
			public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State cooling;
		}
	}
}
