using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200076B RID: 1899
public class GeothermalController : StateMachineComponent<GeothermalController.StatesInstance>
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06003027 RID: 12327 RVA: 0x00116041 File Offset: 0x00114241
	// (set) Token: 0x06003028 RID: 12328 RVA: 0x00116049 File Offset: 0x00114249
	public GeothermalController.ProgressState State
	{
		get
		{
			return this.state;
		}
		protected set
		{
			this.state = value;
		}
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x00116054 File Offset: 0x00114254
	public List<GeothermalVent> FindVents(bool requireEnabled)
	{
		if (!requireEnabled)
		{
			return Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId());
		}
		List<GeothermalVent> list = new List<GeothermalVent>();
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsVentConnected())
			{
				list.Add(geothermalVent);
			}
		}
		return list;
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x001160D0 File Offset: 0x001142D0
	public void PushToVents(GeothermalVent.ElementInfo info)
	{
		List<GeothermalVent> list = this.FindVents(true);
		if (list.Count == 0)
		{
			return;
		}
		float[] array = new float[list.Count];
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = GeothermalControllerConfig.OUTPUT_VENT_WEIGHT_RANGE.Get();
			num += array[i];
		}
		GeothermalVent.ElementInfo info2 = info;
		for (int j = 0; j < list.Count; j++)
		{
			info2.mass = array[j] * info.mass / num;
			info2.diseaseCount = (int)(array[j] * (float)info.diseaseCount / num);
			list[j].addMaterial(info2);
		}
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x00116179 File Offset: 0x00114379
	public bool IsFull()
	{
		return this.storage.MassStored() > 11999.9f;
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x00116190 File Offset: 0x00114390
	public float ComputeContentTemperature()
	{
		float num = 0f;
		float num2 = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			float num3 = component.Mass * component.Element.specificHeatCapacity;
			num += num3 * component.Temperature;
			num2 += num3;
		}
		float result = 0f;
		if (num2 != 0f)
		{
			result = num / num2;
		}
		return result;
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x00116230 File Offset: 0x00114430
	public List<GeothermalVent.ElementInfo> ComputeOutputs()
	{
		float num = this.ComputeContentTemperature();
		float temperature = GeothermalControllerConfig.CalculateOutputTemperature(num);
		GeothermalController.ImpuritiesHelper impuritiesHelper = new GeothermalController.ImpuritiesHelper();
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			impuritiesHelper.AddMaterial(component.Element.idx, component.Mass * 0.92f, temperature, component.DiseaseIdx, component.DiseaseCount);
		}
		foreach (GeothermalControllerConfig.Impurity impurity in GeothermalControllerConfig.GetImpurities())
		{
			MathUtil.MinMax required_temp_range = impurity.required_temp_range;
			if (required_temp_range.Contains(num))
			{
				impuritiesHelper.AddMaterial(impurity.elementIdx, impurity.mass_kg, temperature, byte.MaxValue, 0);
			}
		}
		return impuritiesHelper.results;
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x0011633C File Offset: 0x0011453C
	public void PushToVents()
	{
		SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented = true;
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent && list[0].temperature >= 602f)
		{
			GeothermalPlantComponent.OnVentingHotMaterial(this.GetMyWorldId());
		}
		foreach (GeothermalVent.ElementInfo info in list)
		{
			this.PushToVents(info);
		}
		this.storage.ConsumeAllIgnoringDisease();
		this.fakeProgress = 1f;
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x001163E8 File Offset: 0x001145E8
	private void TryAddConduitConsumers()
	{
		if (base.GetComponents<EntityConduitConsumer>().Length != 0)
		{
			return;
		}
		foreach (CellOffset offset in new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(2, 0),
			new CellOffset(-2, 0)
		})
		{
			EntityConduitConsumer entityConduitConsumer = base.gameObject.AddComponent<EntityConduitConsumer>();
			entityConduitConsumer.offset = offset;
			entityConduitConsumer.conduitType = ConduitType.Liquid;
		}
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x00116460 File Offset: 0x00114660
	public float GetPressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			return this.storage.MassStored() / 12000f;
		}
		return 0f;
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x00116497 File Offset: 0x00114697
	private void FakeMeterDraining(float time)
	{
		this.fakeProgress -= time / 16f;
		if (this.fakeProgress < 0f)
		{
			this.fakeProgress = 0f;
		}
		this.barometer.SetPositionPercent(this.fakeProgress);
	}

	// Token: 0x06003032 RID: 12338 RVA: 0x001164D8 File Offset: 0x001146D8
	private void UpdatePressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			float pressure = this.GetPressure();
			this.barometer.SetPositionPercent(pressure);
			float num = this.ComputeContentTemperature();
			if (num > 0f)
			{
				this.thermometer.SetPositionPercent((num - 50f) / 2450f);
			}
			int num2 = 0;
			for (int i = 1; i < GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS.Length; i++)
			{
				if (pressure >= GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS[i])
				{
					num2 = i;
				}
			}
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (((currentAnim != null) ? currentAnim.name : null) != GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2])
			{
				this.animController.Play(GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2], KAnim.PlayMode.Loop, 1f, 0f);
			}
			return;
		}
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x001165A8 File Offset: 0x001147A8
	public bool IsObstructed()
	{
		if (this.IsFull())
		{
			bool flag = false;
			foreach (GeothermalVent geothermalVent in this.FindVents(false))
			{
				if (geothermalVent.IsEntombed())
				{
					return true;
				}
				if (geothermalVent.IsVentConnected())
				{
					if (!geothermalVent.CanVent())
					{
						return true;
					}
					flag = true;
				}
			}
			return !flag;
		}
		return false;
	}

	// Token: 0x06003034 RID: 12340 RVA: 0x0011662C File Offset: 0x0011482C
	public GeothermalVent FirstObstructedVent()
	{
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsEntombed())
			{
				return geothermalVent;
			}
			if (geothermalVent.IsVentConnected() && !geothermalVent.CanVent())
			{
				return geothermalVent;
			}
		}
		return null;
	}

	// Token: 0x06003035 RID: 12341 RVA: 0x0011669C File Offset: 0x0011489C
	public Notification CreateFirstBatchReadyNotification()
	{
		this.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY_TOOLTIP, null, false, 0f, null, null, base.transform, true, false, false);
		return this.dismissOnSelect;
	}

	// Token: 0x06003036 RID: 12342 RVA: 0x001166F8 File Offset: 0x001148F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.GeothermalControllers.Add(this.GetMyWorldId(), this);
		this.operational.SetFlag(GeothermalController.allowInputFlag, false);
		base.smi.StartSM();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.barometer = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.BAROMETER_SYMBOLS);
		this.thermometer = new MeterController(this.animController, "meter_target", "meter_temp", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.THERMOMETER_SYMBOLS);
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x001167A4 File Offset: 0x001149A4
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
		if (this.listener != null)
		{
			Components.GeothermalVents.Unregister(this.GetMyWorldId(), this.listener.onAdd, this.listener.onRemove);
		}
		Components.GeothermalControllers.Remove(this.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x00116810 File Offset: 0x00114A10
	protected void OnBuildingSelected(object clicked)
	{
		if (!((Boxed<bool>)clicked).value)
		{
			return;
		}
		if (this.dismissOnSelect != null)
		{
			if (this.dismissOnSelect.customClickCallback != null)
			{
				this.dismissOnSelect.customClickCallback(this.dismissOnSelect.customClickData);
				return;
			}
			this.dismissOnSelect.Clear();
			this.dismissOnSelect = null;
		}
	}

	// Token: 0x06003039 RID: 12345 RVA: 0x00116870 File Offset: 0x00114A70
	public bool VentingCanFreeKeepsake()
	{
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		return list.Count != 0 && list[0].temperature >= 602f;
	}

	// Token: 0x04001CAC RID: 7340
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001CAD RID: 7341
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001CAE RID: 7342
	private MeterController thermometer;

	// Token: 0x04001CAF RID: 7343
	private MeterController barometer;

	// Token: 0x04001CB0 RID: 7344
	private KBatchedAnimController animController;

	// Token: 0x04001CB1 RID: 7345
	public Notification dismissOnSelect;

	// Token: 0x04001CB2 RID: 7346
	public static Operational.Flag allowInputFlag = new Operational.Flag("allowInputFlag", Operational.Flag.Type.Requirement);

	// Token: 0x04001CB3 RID: 7347
	private GeothermalController.VentRegistrationListener listener;

	// Token: 0x04001CB4 RID: 7348
	[Serialize]
	private GeothermalController.ProgressState state;

	// Token: 0x04001CB5 RID: 7349
	private float fakeProgress;

	// Token: 0x02001662 RID: 5730
	public class ReconnectPipes : Workable
	{
		// Token: 0x06009708 RID: 38664 RVA: 0x00381DA9 File Offset: 0x0037FFA9
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.SetWorkTime(5f);
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(GeothermalControllerConfig.RECONNECT_PUMP_ANIM_OVERRIDE)
			};
			this.synchronizeAnims = false;
			this.faceTargetWhenWorking = true;
		}

		// Token: 0x06009709 RID: 38665 RVA: 0x00381DE3 File Offset: 0x0037FFE3
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			if (this.storage != null)
			{
				this.storage.ConsumeAllIgnoringDisease();
			}
		}

		// Token: 0x040074BE RID: 29886
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x02001663 RID: 5731
	private class VentRegistrationListener
	{
		// Token: 0x040074BF RID: 29887
		public Action<GeothermalVent> onAdd;

		// Token: 0x040074C0 RID: 29888
		public Action<GeothermalVent> onRemove;
	}

	// Token: 0x02001664 RID: 5732
	public enum ProgressState
	{
		// Token: 0x040074C2 RID: 29890
		NOT_STARTED,
		// Token: 0x040074C3 RID: 29891
		FETCHING_STEEL,
		// Token: 0x040074C4 RID: 29892
		RECONNECTING_PIPES,
		// Token: 0x040074C5 RID: 29893
		NOTIFY_REPAIRED,
		// Token: 0x040074C6 RID: 29894
		REPAIRED,
		// Token: 0x040074C7 RID: 29895
		AT_CAPACITY,
		// Token: 0x040074C8 RID: 29896
		COMPLETE
	}

	// Token: 0x02001665 RID: 5733
	private class ImpuritiesHelper
	{
		// Token: 0x0600970C RID: 38668 RVA: 0x00381E18 File Offset: 0x00380018
		public void AddMaterial(ushort elementIdx, float mass, float temperature, byte diseaseIdx, int diseaseCount)
		{
			Element element = ElementLoader.elements[(int)elementIdx];
			if (element.lowTemp > temperature)
			{
				Element lowTempTransition = element.lowTempTransition;
				Element element2 = ElementLoader.FindElementByHash(element.lowTempTransitionOreID);
				this.AddMaterial(lowTempTransition.idx, mass * (1f - element.lowTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.lowTempTransitionOreMassConversion)));
				if (element2 != null)
				{
					this.AddMaterial(element2.idx, mass * element.lowTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.lowTempTransitionOreMassConversion));
				}
				return;
			}
			if (element.highTemp < temperature)
			{
				Element highTempTransition = element.highTempTransition;
				Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionOreID);
				this.AddMaterial(highTempTransition.idx, mass * (1f - element.highTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.highTempTransitionOreMassConversion)));
				if (element3 != null)
				{
					this.AddMaterial(element3.idx, mass * element.highTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.highTempTransitionOreMassConversion));
				}
				return;
			}
			GeothermalVent.ElementInfo elementInfo = default(GeothermalVent.ElementInfo);
			for (int i = 0; i < this.results.Count; i++)
			{
				if (this.results[i].elementIdx == elementIdx)
				{
					elementInfo = this.results[i];
					elementInfo.mass += mass;
					this.results[i] = elementInfo;
					return;
				}
			}
			elementInfo.elementHash = element.id;
			elementInfo.elementIdx = elementIdx;
			elementInfo.mass = mass;
			elementInfo.temperature = temperature;
			elementInfo.diseaseCount = diseaseCount;
			elementInfo.diseaseIdx = diseaseIdx;
			elementInfo.isSolid = ElementLoader.elements[(int)elementIdx].IsSolid;
			this.results.Add(elementInfo);
		}

		// Token: 0x040074C9 RID: 29897
		public List<GeothermalVent.ElementInfo> results = new List<GeothermalVent.ElementInfo>();
	}

	// Token: 0x02001666 RID: 5734
	public class States : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController>
	{
		// Token: 0x0600970E RID: 38670 RVA: 0x00381FE4 File Offset: 0x003801E4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.online, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.COMPLETE).EnterTransition(this.offline, (GeothermalController.StatesInstance smi) => smi.master.State != GeothermalController.ProgressState.COMPLETE);
			this.offline.EnterTransition(this.offline.initial, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOT_STARTED).EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.RECONNECTING_PIPES).EnterTransition(this.offline.notifyRepaired, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOTIFY_REPAIRED).EnterTransition(this.offline.filling, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.REPAIRED).EnterTransition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.AT_CAPACITY).PlayAnim("off");
			this.offline.initial.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.storage.DropAll(false, false, default(Vector3), true, null);
			}).Transition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.fetchSteel.ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairFetchChore(smi, GeothermalControllerConfig.STEEL_FETCH_TAGS, 1200f - smi.master.storage.MassStored()), this.offline.checkSupplies).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, (GeothermalController.StatesInstance smi) => smi.GetFetchListForStatusItem());
			this.offline.checkSupplies.EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() < 1200f).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() >= 1200f).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.reconnectPipes.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.RECONNECTING_PIPES;
			}).ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairChore(smi), this.offline.notifyRepaired, this.offline.reconnectPipes).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoQuestPendingReconnectPipes, null);
			this.offline.notifyRepaired.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.NOTIFY_REPAIRED;
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleNotification((GeothermalController.StatesInstance smi) => this.CreateRepairedNotification(smi)).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.repaired.Exit(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.State = GeothermalController.ProgressState.REPAIRED;
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.offline.filling).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filling.PlayAnim("on").Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).ToggleOperationalFlag(GeothermalController.allowInputFlag).Transition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.IsFull(), UpdateRate.SIM_200ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.AT_CAPACITY;
				smi.master.TryAddConduitConsumers();
			}).ToggleNotification((GeothermalController.StatesInstance smi) => smi.master.CreateFirstBatchReadyNotification()).EnterTransition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed()).EnterTransition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed()).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.filled.ready.PlayAnim("on").Transition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.obstructed.Transition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_200ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).defaultState = this.online.active;
			this.online.active.PlayAnim("on").Transition(this.online.venting, (GeothermalController.StatesInstance smi) => smi.master.IsFull() && !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleOperationalFlag(GeothermalController.allowInputFlag).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.venting.Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.PushToVents();
			}).PlayAnim("venting_loop", KAnim.PlayMode.Loop).Update(delegate(GeothermalController.StatesInstance smi, float f)
			{
				smi.master.FakeMeterDraining(f);
			}, UpdateRate.SIM_1000ms, false).ScheduleGoTo(16f, this.online.active).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.obstructed.Transition(this.online.active, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
		}

		// Token: 0x0600970F RID: 38671 RVA: 0x00382B38 File Offset: 0x00380D38
		protected Chore CreateRepairFetchChore(GeothermalController.StatesInstance smi, HashSet<Tag> tags, float mass_required)
		{
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storage, mass_required, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x06009710 RID: 38672 RVA: 0x00382B74 File Offset: 0x00380D74
		protected Chore CreateRepairChore(GeothermalController.StatesInstance smi)
		{
			return new WorkChore<GeothermalController.ReconnectPipes>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x06009711 RID: 38673 RVA: 0x00382BAC File Offset: 0x00380DAC
		protected Notification CreateRepairedNotification(GeothermalController.StatesInstance smi)
		{
			smi.master.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED_TOOLTIP, null, false, 0f, delegate(object _)
			{
				smi.master.dismissOnSelect = null;
				this.SetProgressionToRepaired(smi);
			}, null, null, true, true, false);
			return smi.master.dismissOnSelect;
		}

		// Token: 0x06009712 RID: 38674 RVA: 0x00382C34 File Offset: 0x00380E34
		protected void SetProgressionToRepaired(GeothermalController.StatesInstance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired = true;
			GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_DESC, "geothermalplantonline_kanim", delegate
			{
				smi.GoTo(this.offline.repaired);
				SelectTool.Instance.Select(smi.master.GetComponent<KSelectable>(), true);
			}, smi.master.transform);
		}

		// Token: 0x040074CA RID: 29898
		public GeothermalController.States.OfflineStates offline;

		// Token: 0x040074CB RID: 29899
		public GeothermalController.States.OnlineStates online;

		// Token: 0x020028E9 RID: 10473
		public class OfflineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x0400B489 RID: 46217
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State initial;

			// Token: 0x0400B48A RID: 46218
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State fetchSteel;

			// Token: 0x0400B48B RID: 46219
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State checkSupplies;

			// Token: 0x0400B48C RID: 46220
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State reconnectPipes;

			// Token: 0x0400B48D RID: 46221
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State notifyRepaired;

			// Token: 0x0400B48E RID: 46222
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State repaired;

			// Token: 0x0400B48F RID: 46223
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State filling;

			// Token: 0x0400B490 RID: 46224
			public GeothermalController.States.OfflineStates.FilledStates filled;

			// Token: 0x02003A46 RID: 14918
			public class FilledStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x0400EB76 RID: 60278
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State ready;

				// Token: 0x0400EB77 RID: 60279
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;
			}
		}

		// Token: 0x020028EA RID: 10474
		public class OnlineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x0400B491 RID: 46225
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State active;

			// Token: 0x0400B492 RID: 46226
			public GeothermalController.States.OnlineStates.WorkingStates venting;

			// Token: 0x0400B493 RID: 46227
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;

			// Token: 0x02003A47 RID: 14919
			public class WorkingStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x0400EB78 RID: 60280
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State pre;

				// Token: 0x0400EB79 RID: 60281
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State loop;

				// Token: 0x0400EB7A RID: 60282
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State post;
			}
		}
	}

	// Token: 0x02001667 RID: 5735
	public class StatesInstance : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06009717 RID: 38679 RVA: 0x00382CE2 File Offset: 0x00380EE2
		public StatesInstance(GeothermalController smi) : base(smi)
		{
		}

		// Token: 0x06009718 RID: 38680 RVA: 0x00382CEC File Offset: 0x00380EEC
		public IFetchList GetFetchListForStatusItem()
		{
			GeothermalController.StatesInstance.FakeList fakeList = new GeothermalController.StatesInstance.FakeList();
			float value = 1200f - base.smi.master.storage.MassStored();
			fakeList.remaining[GameTagExtensions.Create(SimHashes.Steel)] = value;
			return fakeList;
		}

		// Token: 0x06009719 RID: 38681 RVA: 0x00382D30 File Offset: 0x00380F30
		bool ISidescreenButtonControl.SidescreenButtonInteractable()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return true;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return false;
			case GeothermalController.ProgressState.AT_CAPACITY:
				return !base.smi.master.IsObstructed();
			case GeothermalController.ProgressState.COMPLETE:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x0600971A RID: 38682 RVA: 0x00382D8D File Offset: 0x00380F8D
		bool ISidescreenButtonControl.SidescreenEnabled()
		{
			return base.smi.master.State != GeothermalController.ProgressState.COMPLETE;
		}

		// Token: 0x0600971B RID: 38683 RVA: 0x00382DA8 File Offset: 0x00380FA8
		private string getSidescreenButtonText()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_TITLE;
			default:
				return "";
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x0600971C RID: 38684 RVA: 0x00382E10 File Offset: 0x00381010
		string ISidescreenButtonControl.SidescreenButtonText
		{
			get
			{
				return this.getSidescreenButtonText();
			}
		}

		// Token: 0x0600971D RID: 38685 RVA: 0x00382E18 File Offset: 0x00381018
		private string getSidescreenButtonTooltip()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_FILLING_TOOLTIP;
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				if (base.smi.master.IsObstructed())
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_UNAVAILABLE_TOOLTIP;
				}
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_READY_TOOLTIP;
			default:
				return "";
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600971E RID: 38686 RVA: 0x00382EA8 File Offset: 0x003810A8
		string ISidescreenButtonControl.SidescreenButtonTooltip
		{
			get
			{
				return this.getSidescreenButtonTooltip();
			}
		}

		// Token: 0x0600971F RID: 38687 RVA: 0x00382EB0 File Offset: 0x003810B0
		void ISidescreenButtonControl.OnSidescreenButtonPressed()
		{
			switch (base.smi.master.state)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				base.smi.master.State = GeothermalController.ProgressState.FETCHING_STEEL;
				return;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				base.smi.master.State = GeothermalController.ProgressState.NOT_STARTED;
				base.smi.GoTo(base.sm.offline.initial);
				return;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.COMPLETE:
				break;
			case GeothermalController.ProgressState.AT_CAPACITY:
			{
				MusicManager.instance.PlaySong("Music_Imperative_complete_DLC2", false);
				bool flag = base.smi.master.VentingCanFreeKeepsake();
				base.smi.master.state = GeothermalController.ProgressState.COMPLETE;
				base.smi.GoTo(base.sm.online.venting);
				if (!flag)
				{
					GeothermalFirstEmissionSequence.Start(base.smi.master);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06009720 RID: 38688 RVA: 0x00382F8E File Offset: 0x0038118E
		void ISidescreenButtonControl.SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009721 RID: 38689 RVA: 0x00382F95 File Offset: 0x00381195
		int ISidescreenButtonControl.HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x06009722 RID: 38690 RVA: 0x00382F98 File Offset: 0x00381198
		int ISidescreenButtonControl.ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x020028EE RID: 10478
		protected class FakeList : IFetchList
		{
			// Token: 0x17000D6B RID: 3435
			// (get) Token: 0x0600CE71 RID: 52849 RVA: 0x00432986 File Offset: 0x00430B86
			Storage IFetchList.Destination
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x0600CE72 RID: 52850 RVA: 0x0043298D File Offset: 0x00430B8D
			float IFetchList.GetMinimumAmount(Tag tag)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600CE73 RID: 52851 RVA: 0x00432994 File Offset: 0x00430B94
			Dictionary<Tag, float> IFetchList.GetRemaining()
			{
				return this.remaining;
			}

			// Token: 0x0600CE74 RID: 52852 RVA: 0x0043299C File Offset: 0x00430B9C
			Dictionary<Tag, float> IFetchList.GetRemainingMinimum()
			{
				throw new NotImplementedException();
			}

			// Token: 0x0400B4CB RID: 46283
			public Dictionary<Tag, float> remaining = new Dictionary<Tag, float>();
		}
	}
}
