using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007E8 RID: 2024
public class Reactor : StateMachineComponent<Reactor.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x17000375 RID: 885
	// (get) Token: 0x060035F8 RID: 13816 RVA: 0x00130496 File Offset: 0x0012E696
	// (set) Token: 0x060035F9 RID: 13817 RVA: 0x0013049E File Offset: 0x0012E69E
	private float ReactionMassTarget
	{
		get
		{
			return this.reactionMassTarget;
		}
		set
		{
			this.fuelDelivery.capacity = value * 2f;
			this.fuelDelivery.refillMass = value * 0.2f;
			this.fuelDelivery.MinimumMass = value * 0.2f;
			this.reactionMassTarget = value;
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x060035FA RID: 13818 RVA: 0x001304DD File Offset: 0x0012E6DD
	public float FuelTemperature
	{
		get
		{
			if (this.reactionStorage.items.Count > 0)
			{
				return this.reactionStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
			return -1f;
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x060035FB RID: 13819 RVA: 0x00130514 File Offset: 0x0012E714
	public float ReserveCoolantMass
	{
		get
		{
			PrimaryElement storedCoolant = this.GetStoredCoolant();
			if (!(storedCoolant == null))
			{
				return storedCoolant.Mass;
			}
			return 0f;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x060035FC RID: 13820 RVA: 0x0013053D File Offset: 0x0012E73D
	public bool On
	{
		get
		{
			return base.smi.IsInsideState(base.smi.sm.on);
		}
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x0013055C File Offset: 0x0012E75C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.NuclearReactors.Add(this);
		Storage[] components = base.GetComponents<Storage>();
		this.supplyStorage = components[0];
		this.reactionStorage = components[1];
		this.wasteStorage = components[2];
		this.CreateMeters();
		base.smi.StartSM();
		this.fuelDelivery = base.GetComponent<ManualDeliveryKG>();
		this.CheckLogicInputValueChanged(true);
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x001305C0 File Offset: 0x0012E7C0
	protected override void OnCleanUp()
	{
		Components.NuclearReactors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x001305D3 File Offset: 0x0012E7D3
	private void Update()
	{
		this.CheckLogicInputValueChanged(false);
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x001305DC File Offset: 0x0012E7DC
	public Notification CreateMeltdownNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.REACTORMELTDOWN.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REACTORMELTDOWN.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06003601 RID: 13825 RVA: 0x0013063B File Offset: 0x0012E83B
	public void SetStorages(Storage supply, Storage reaction, Storage waste)
	{
		this.supplyStorage = supply;
		this.reactionStorage = reaction;
		this.wasteStorage = waste;
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x00130654 File Offset: 0x0012E854
	private void CheckLogicInputValueChanged(bool onLoad = false)
	{
		int num = 1;
		if (this.logicPorts.IsPortConnected("CONTROL_FUEL_DELIVERY"))
		{
			num = this.logicPorts.GetInputValue("CONTROL_FUEL_DELIVERY");
		}
		if (num == 0 && (this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = -1f;
			this.fuelDeliveryEnabled = false;
			this.fuelDelivery.AbortDelivery("AutomationDisabled");
			return;
		}
		if (num == 1 && (!this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = this.reactionMassTarget * 0.2f;
			this.fuelDeliveryEnabled = true;
		}
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x001306F4 File Offset: 0x0012E8F4
	private void OnLogicConnectionChanged(int value, bool connection)
	{
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x001306F8 File Offset: 0x0012E8F8
	private void CreateMeters()
	{
		this.temperatureMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "temperature_meter_target", "meter_temperature", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"temperature_meter_target"
		});
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "water_meter_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"water_meter_target"
		});
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x00130760 File Offset: 0x0012E960
	private void TransferFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		PrimaryElement storedFuel = this.GetStoredFuel();
		float num = (activeFuel != null) ? activeFuel.Mass : 0f;
		float num2 = (storedFuel != null) ? storedFuel.Mass : 0f;
		float num3 = this.ReactionMassTarget - num;
		num3 = Mathf.Min(num2, num3);
		if (num3 > 0.5f || num3 == num2)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.fuelTag, num3, false, true);
		}
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x001307E8 File Offset: 0x0012E9E8
	private void TransferCoolant()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement storedCoolant = this.GetStoredCoolant();
		float num = (activeCoolant != null) ? activeCoolant.Mass : 0f;
		float a = (storedCoolant != null) ? storedCoolant.Mass : 0f;
		float num2 = 30f - num;
		num2 = Mathf.Min(a, num2);
		if (num2 > 0f)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.coolantTag, num2, false, true);
		}
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x00130864 File Offset: 0x0012EA64
	private PrimaryElement GetStoredFuel()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x001308A0 File Offset: 0x0012EAA0
	private PrimaryElement GetActiveFuel()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x001308DC File Offset: 0x0012EADC
	private PrimaryElement GetStoredCoolant()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x0600360A RID: 13834 RVA: 0x00130918 File Offset: 0x0012EB18
	private PrimaryElement GetActiveCoolant()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x00130954 File Offset: 0x0012EB54
	private bool CanStartReaction()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement activeFuel = this.GetActiveFuel();
		return activeCoolant && activeFuel && activeCoolant.Mass >= 30f && activeFuel.Mass >= 0.5f;
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x0013099C File Offset: 0x0012EB9C
	private void Cool(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel == null)
		{
			return;
		}
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		if (activeCoolant == null)
		{
			return;
		}
		GameUtil.ForceConduction(activeFuel, activeCoolant, dt * 5f);
		if (activeCoolant.Temperature > 673.15f)
		{
			base.smi.sm.doVent.Trigger(base.smi);
		}
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x00130A04 File Offset: 0x0012EC04
	private void React(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null && activeFuel.Mass >= 0.25f)
		{
			float num = GameUtil.EnergyToTemperatureDelta(-100f * dt * activeFuel.Mass, activeFuel);
			activeFuel.Temperature += num;
			this.spentFuel += dt * 0.016666668f;
		}
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x00130A65 File Offset: 0x0012EC65
	private void SetEmitRads(float rads)
	{
		base.smi.master.radEmitter.emitRads = rads;
		base.smi.master.radEmitter.Refresh();
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x00130A94 File Offset: 0x0012EC94
	private bool ReadyToCool()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		return activeCoolant != null && activeCoolant.Mass > 0f;
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x00130AC0 File Offset: 0x0012ECC0
	private void DumpSpentFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null)
		{
			if (this.spentFuel <= 0f)
			{
				return;
			}
			float num = this.spentFuel * 100f;
			if (num > 0f)
			{
				this.wasteStorage.AddLiquid(SimHashes.NuclearWaste, num, activeFuel.Temperature, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id), Mathf.RoundToInt(num * 50f), false, true);
			}
			if (this.wasteStorage.MassStored() >= 100f)
			{
				this.wasteStorage.DropAll(true, true, default(Vector3), true, null);
			}
			if (this.spentFuel >= activeFuel.Mass)
			{
				Util.KDestroyGameObject(activeFuel.gameObject);
				this.spentFuel = 0f;
				return;
			}
			activeFuel.Mass -= this.spentFuel;
			this.spentFuel = 0f;
		}
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x00130BBC File Offset: 0x0012EDBC
	private void UpdateVentStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.ClearToVent())
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
			{
				base.smi.sm.canVent.Set(true, base.smi, false);
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
		{
			base.smi.sm.canVent.Set(false, base.smi, false);
			component.AddStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, null);
		}
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x00130C74 File Offset: 0x0012EE74
	private void UpdateCoolantStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.GetStoredCoolant() != null || base.smi.GetCurrentState() == base.smi.sm.meltdown || base.smi.GetCurrentState() == base.smi.sm.dead)
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoCoolant, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.NoCoolant, null);
		}
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x00130D30 File Offset: 0x0012EF30
	private void InitVentCells()
	{
		if (this.ventCells == null)
		{
			this.ventCells = new int[]
			{
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.zero),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left + Vector3.left)
			};
		}
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x00130F9C File Offset: 0x0012F19C
	public int GetVentCell()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return this.ventCells[i];
			}
		}
		return -1;
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x00130FFC File Offset: 0x0012F1FC
	private bool ClearToVent()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x00131052 File Offset: 0x0012F252
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x040020D3 RID: 8403
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040020D4 RID: 8404
	[MyCmpGet]
	private RadiationEmitter radEmitter;

	// Token: 0x040020D5 RID: 8405
	[MyCmpGet]
	private ManualDeliveryKG fuelDelivery;

	// Token: 0x040020D6 RID: 8406
	private MeterController temperatureMeter;

	// Token: 0x040020D7 RID: 8407
	private MeterController waterMeter;

	// Token: 0x040020D8 RID: 8408
	private Storage supplyStorage;

	// Token: 0x040020D9 RID: 8409
	private Storage reactionStorage;

	// Token: 0x040020DA RID: 8410
	private Storage wasteStorage;

	// Token: 0x040020DB RID: 8411
	private Tag fuelTag = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x040020DC RID: 8412
	private Tag coolantTag = GameTags.AnyWater;

	// Token: 0x040020DD RID: 8413
	private Vector3 dumpOffset = new Vector3(0f, 5f, 0f);

	// Token: 0x040020DE RID: 8414
	public static string MELTDOWN_STINGER = "Stinger_Loop_NuclearMeltdown";

	// Token: 0x040020DF RID: 8415
	private static float meterFrameScaleHack = 3f;

	// Token: 0x040020E0 RID: 8416
	[Serialize]
	private float spentFuel;

	// Token: 0x040020E1 RID: 8417
	private float timeSinceMeltdownEmit;

	// Token: 0x040020E2 RID: 8418
	private const float reactorMeltDownBonusMassAmount = 10f;

	// Token: 0x040020E3 RID: 8419
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x040020E4 RID: 8420
	private LogicEventHandler fuelControlPort;

	// Token: 0x040020E5 RID: 8421
	private bool fuelDeliveryEnabled = true;

	// Token: 0x040020E6 RID: 8422
	public Guid refuelStausHandle;

	// Token: 0x040020E7 RID: 8423
	[Serialize]
	public int numCyclesRunning;

	// Token: 0x040020E8 RID: 8424
	private float reactionMassTarget = 60f;

	// Token: 0x040020E9 RID: 8425
	private int[] ventCells;

	// Token: 0x0200174E RID: 5966
	public class StatesInstance : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.GameInstance
	{
		// Token: 0x06009ABB RID: 39611 RVA: 0x00392335 File Offset: 0x00390535
		public StatesInstance(Reactor smi) : base(smi)
		{
		}
	}

	// Token: 0x0200174F RID: 5967
	public class States : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor>
	{
		// Token: 0x06009ABC RID: 39612 RVA: 0x00392340 File Offset: 0x00390540
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.off;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement storedCoolant = smi.master.GetStoredCoolant();
				if (!storedCoolant)
				{
					smi.master.waterMeter.SetPositionPercent(0f);
					return;
				}
				smi.master.waterMeter.SetPositionPercent(storedCoolant.Mass / 90f);
			});
			this.off_pre.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off);
			this.off.PlayAnim("off").Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(false);
				smi.master.SetEmitRads(0f);
			}).ParamTransition<bool>(this.reactionUnderway, this.on, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.melted, this.dead, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.meltingDown, this.meltdown, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				if (smi.master.CanStartReaction())
				{
					smi.GoTo(this.on);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(true, smi, false);
				smi.master.operational.SetActive(true, false);
				smi.master.SetEmitRads(2400f);
				smi.master.radEmitter.SetEmitting(true);
			}).EventHandler(GameHashes.NewDay, (Reactor.StatesInstance smi) => GameClock.Instance, delegate(Reactor.StatesInstance smi)
			{
				smi.master.numCyclesRunning++;
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(false, smi, false);
				smi.master.numCyclesRunning = 0;
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				smi.master.React(dt);
				smi.master.UpdateCoolantStatus();
				smi.master.UpdateVentStatus();
				smi.master.DumpSpentFuel();
				if (!smi.master.fuelDeliveryEnabled)
				{
					smi.master.refuelStausHandle = smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, null);
				}
				else
				{
					smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, false);
					smi.master.refuelStausHandle = Guid.Empty;
				}
				if (smi.master.GetActiveCoolant() != null)
				{
					smi.master.Cool(dt);
				}
				PrimaryElement activeFuel = smi.master.GetActiveFuel();
				if (activeFuel != null)
				{
					smi.master.temperatureMeter.SetPositionPercent(Mathf.Clamp01(activeFuel.Temperature / 3000f) / Reactor.meterFrameScaleHack);
					if (activeFuel.Temperature >= 3000f)
					{
						smi.sm.meltdownMassRemaining.Set(10f + smi.master.supplyStorage.MassStored() + smi.master.reactionStorage.MassStored() + smi.master.wasteStorage.MassStored(), smi, false);
						smi.master.supplyStorage.ConsumeAllIgnoringDisease();
						smi.master.reactionStorage.ConsumeAllIgnoringDisease();
						smi.master.wasteStorage.ConsumeAllIgnoringDisease();
						smi.GoTo(this.meltdown.pre);
						return;
					}
					if (activeFuel.Mass <= 0.25f)
					{
						smi.GoTo(this.off_pre);
						smi.master.temperatureMeter.SetPositionPercent(0f);
						return;
					}
				}
				else
				{
					smi.GoTo(this.off_pre);
					smi.master.temperatureMeter.SetPositionPercent(0f);
				}
			}, UpdateRate.SIM_200ms, false).DefaultState(this.on.pre);
			this.on.pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.on.reacting).OnSignal(this.doVent, this.on.venting);
			this.on.reacting.PlayAnim("working_loop", KAnim.PlayMode.Loop).OnSignal(this.doVent, this.on.venting);
			this.on.venting.ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.canVent, this.on.venting.ventIssue, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsFalse);
			this.on.venting.ventIssue.PlayAnim("venting_issue", KAnim.PlayMode.Loop).ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue);
			this.on.venting.vent.PlayAnim("venting").Enter(delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement activeCoolant = smi.master.GetActiveCoolant();
				if (activeCoolant != null)
				{
					activeCoolant.GetComponent<Dumpable>().Dump(Grid.CellToPos(smi.master.GetVentCell()));
				}
			}).OnAnimQueueComplete(this.on.reacting);
			this.meltdown.ToggleStatusItem(Db.Get().BuildingStatusItems.ReactorMeltdown, null).ToggleNotification((Reactor.StatesInstance smi) => smi.master.CreateMeltdownNotification()).ParamTransition<float>(this.meltdownMassRemaining, this.dead, (Reactor.StatesInstance smi, float p) => p <= 0f).ToggleTag(GameTags.DeadReactor).DefaultState(this.meltdown.loop);
			this.meltdown.pre.PlayAnim("almost_meltdown_pre", KAnim.PlayMode.Once).QueueAnim("almost_meltdown_loop", false, null).QueueAnim("meltdown_pre", false, null).OnAnimQueueComplete(this.meltdown.loop);
			this.meltdown.loop.PlayAnim("meltdown_loop", KAnim.PlayMode.Loop).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(true);
				smi.master.SetEmitRads(4800f);
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.UpdateCoolantStatus();
				if (this.meltingDown.Get(smi))
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.StopDynamicMusic(false);
				}
				else
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_PlayStinger", 1f, true);
					MusicManager.instance.StopDynamicMusic(false);
				}
				this.meltingDown.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				this.meltingDown.Set(false, smi, false);
				MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_NuclearMeltdownActive", 0f, true);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.timeSinceMeltdownEmit += dt;
				float num = 0.5f;
				float b = 5f;
				if (smi.master.timeSinceMeltdownEmit > num && smi.sm.meltdownMassRemaining.Get(smi) > 0f)
				{
					smi.master.timeSinceMeltdownEmit -= num;
					float num2 = Mathf.Min(smi.sm.meltdownMassRemaining.Get(smi), b);
					smi.sm.meltdownMassRemaining.Delta(-num2, smi);
					for (int i = 0; i < 3; i++)
					{
						if (num2 >= NuclearWasteCometConfig.MASS)
						{
							GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(NuclearWasteCometConfig.ID), smi.master.transform.position + Vector3.up * 2f, Quaternion.identity, null, null, true, 0);
							gameObject.SetActive(true);
							Comet component = gameObject.GetComponent<Comet>();
							component.ignoreObstacleForDamage.Set(smi.master.gameObject.GetComponent<KPrefabID>());
							component.addTiles = 1;
							int num3 = 270;
							while (num3 > 225 && num3 < 335)
							{
								num3 = UnityEngine.Random.Range(0, 360);
							}
							float f = (float)num3 * 3.1415927f / 180f;
							component.Velocity = new Vector2(-Mathf.Cos(f) * 20f, Mathf.Sin(f) * 20f);
							component.GetComponent<KBatchedAnimController>().Rotation = (float)(-(float)num3) - 90f;
							num2 -= NuclearWasteCometConfig.MASS;
						}
					}
					for (int j = 0; j < 3; j++)
					{
						if (num2 >= 0.001f)
						{
							SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.transform.position + Vector3.up * 3f + Vector3.right * (float)j * 2f), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, num2 / 3f, 3000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt(50f * (num2 / 3f)), true, -1);
						}
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.dead.PlayAnim("dead").ToggleTag(GameTags.DeadReactor).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, smi);
				this.melted.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, false);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.sm.timeSinceMeltdown.Delta(dt, smi);
				smi.master.radEmitter.emitRads = Mathf.Lerp(4800f, 0f, smi.sm.timeSinceMeltdown.Get(smi) / 3000f);
				smi.master.radEmitter.Refresh();
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x0400775F RID: 30559
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Signal doVent;

		// Token: 0x04007760 RID: 30560
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter canVent = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(true);

		// Token: 0x04007761 RID: 30561
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter reactionUnderway = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter();

		// Token: 0x04007762 RID: 30562
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter meltdownMassRemaining = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x04007763 RID: 30563
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter timeSinceMeltdown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x04007764 RID: 30564
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter meltingDown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x04007765 RID: 30565
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter melted = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x04007766 RID: 30566
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off;

		// Token: 0x04007767 RID: 30567
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off_pre;

		// Token: 0x04007768 RID: 30568
		public Reactor.States.ReactingStates on;

		// Token: 0x04007769 RID: 30569
		public Reactor.States.MeltdownStates meltdown;

		// Token: 0x0400776A RID: 30570
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State dead;

		// Token: 0x0200292F RID: 10543
		public class ReactingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x0400B62F RID: 46639
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x0400B630 RID: 46640
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State reacting;

			// Token: 0x0400B631 RID: 46641
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pst;

			// Token: 0x0400B632 RID: 46642
			public Reactor.States.ReactingStates.VentingStates venting;

			// Token: 0x02003A4A RID: 14922
			public class VentingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
			{
				// Token: 0x0400EB7E RID: 60286
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State ventIssue;

				// Token: 0x0400EB7F RID: 60287
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State vent;
			}
		}

		// Token: 0x02002930 RID: 10544
		public class MeltdownStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x0400B633 RID: 46643
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_pre;

			// Token: 0x0400B634 RID: 46644
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_loop;

			// Token: 0x0400B635 RID: 46645
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x0400B636 RID: 46646
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State loop;
		}
	}
}
