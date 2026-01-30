using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
public class CritterTrapPlant : StateMachineComponent<CritterTrapPlant.StatesInstance>, IPlantConsumeEntities
{
	// Token: 0x06004EC4 RID: 20164 RVA: 0x001CA054 File Offset: 0x001C8254
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.master.growing.enabled = false;
		base.Subscribe<CritterTrapPlant>(-216549700, CritterTrapPlant.OnUprootedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x001CA08E File Offset: 0x001C828E
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x06004EC6 RID: 20166 RVA: 0x001CA0A8 File Offset: 0x001C82A8
	private void OnUprooted(object data = null)
	{
		GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), base.gameObject.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
		base.gameObject.Trigger(1623392196, null);
		base.gameObject.GetComponent<KBatchedAnimController>().StopAndClear();
		UnityEngine.Object.Destroy(base.gameObject.GetComponent<KBatchedAnimController>());
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001CA11F File Offset: 0x001C831F
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001CA138 File Offset: 0x001C8338
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06004EC9 RID: 20169 RVA: 0x001CA195 File Offset: 0x001C8395
	public string GetConsumableEntitiesCategoryName()
	{
		return CREATURES.SPECIES.CRITTERTRAPPLANT.VICTIM_IDENTIFIER;
	}

	// Token: 0x06004ECA RID: 20170 RVA: 0x001CA1A1 File Offset: 0x001C83A1
	public string GetRequirementText()
	{
		return CREATURES.SPECIES.CRITTERTRAPPLANT.PLANT_HUNGER_REQUIREMENT;
	}

	// Token: 0x06004ECB RID: 20171 RVA: 0x001CA1AD File Offset: 0x001C83AD
	public bool AreEntitiesConsumptionRequirementsSatisfied()
	{
		return base.smi != null && base.smi.sm.hasEatenCreature.Get(base.smi);
	}

	// Token: 0x06004ECC RID: 20172 RVA: 0x001CA1D4 File Offset: 0x001C83D4
	public string GetConsumedEntityName()
	{
		if (base.smi != null)
		{
			return base.smi.LastConsumedEntityName;
		}
		return "Unknown Critter";
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x001CA1F0 File Offset: 0x001C83F0
	public List<KPrefabID> GetPrefabsOfPossiblePrey()
	{
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<CreatureBrain>();
		List<KPrefabID> list = new List<KPrefabID>();
		for (int i = 0; i < prefabsWithComponent.Count; i++)
		{
			KPrefabID component = prefabsWithComponent[i].GetComponent<KPrefabID>();
			if (!list.Contains(component) && this.IsEntityEdible(component) && Game.IsCorrectDlcActiveForCurrentSave(component))
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x001CA24C File Offset: 0x001C844C
	public string[] GetFormattedPossiblePreyList()
	{
		List<string> list = new List<string>();
		foreach (KPrefabID kprefabID in this.GetPrefabsOfPossiblePrey())
		{
			CreatureBrain component = kprefabID.GetComponent<CreatureBrain>();
			if (component != null)
			{
				string item = component.species.ProperName();
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001CA2D0 File Offset: 0x001C84D0
	public bool IsEntityEdible(GameObject entity)
	{
		return this.IsEntityEdible(entity.GetComponent<KPrefabID>());
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001CA2DE File Offset: 0x001C84DE
	public bool IsEntityEdible(KPrefabID entity)
	{
		return entity.HasAnyTags(this.CONSUMABLE_TAGs) && entity.GetComponent<Trappable>() != null && entity.GetComponent<OccupyArea>().OccupiedCellsOffsets.Length < 3;
	}

	// Token: 0x04003492 RID: 13458
	private const string CONSUMED_ENTITY_NAME_FALLBACK = "Unknown Critter";

	// Token: 0x04003493 RID: 13459
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04003494 RID: 13460
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003495 RID: 13461
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04003496 RID: 13462
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04003497 RID: 13463
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04003498 RID: 13464
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x04003499 RID: 13465
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400349A RID: 13466
	public Tag[] CONSUMABLE_TAGs = new Tag[0];

	// Token: 0x0400349B RID: 13467
	public float gasOutputRate;

	// Token: 0x0400349C RID: 13468
	public float gasVentThreshold;

	// Token: 0x0400349D RID: 13469
	public SimHashes outputElement;

	// Token: 0x0400349E RID: 13470
	private float GAS_TEMPERATURE_DELTA = 10f;

	// Token: 0x0400349F RID: 13471
	private static readonly EventSystem.IntraObjectHandler<CritterTrapPlant> OnUprootedDelegate = new EventSystem.IntraObjectHandler<CritterTrapPlant>(delegate(CritterTrapPlant component, object data)
	{
		component.OnUprooted(data);
	});

	// Token: 0x02001BB9 RID: 7097
	public class StatesInstance : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.GameInstance
	{
		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x0600AAD6 RID: 43734 RVA: 0x003C5A76 File Offset: 0x003C3C76
		public string LastConsumedEntityName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.lastConsumedEntityPrefabID))
				{
					return Assets.GetPrefab(this.lastConsumedEntityPrefabID).GetProperName();
				}
				return "Unknown Critter";
			}
		}

		// Token: 0x0600AAD7 RID: 43735 RVA: 0x003C5AA0 File Offset: 0x003C3CA0
		public StatesInstance(CritterTrapPlant master) : base(master)
		{
		}

		// Token: 0x0600AAD8 RID: 43736 RVA: 0x003C5AA9 File Offset: 0x003C3CA9
		public void OnTrapTriggered(object data)
		{
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}

		// Token: 0x0600AAD9 RID: 43737 RVA: 0x003C5AC8 File Offset: 0x003C3CC8
		public void AddGas(float dt)
		{
			float temperature = base.smi.GetComponent<PrimaryElement>().Temperature + base.smi.master.GAS_TEMPERATURE_DELTA;
			base.smi.master.storage.AddGasChunk(base.smi.master.outputElement, base.smi.master.gasOutputRate * dt, temperature, byte.MaxValue, 0, false, true);
			if (this.ShouldVentGas())
			{
				base.smi.sm.ventGas.Trigger(base.smi);
			}
		}

		// Token: 0x0600AADA RID: 43738 RVA: 0x003C5B5C File Offset: 0x003C3D5C
		public void VentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.smi.transform.GetPosition()), primaryElement.ElementID, CellEventLogger.Instance.Dumpable, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				base.smi.master.storage.ConsumeIgnoringDisease(primaryElement.gameObject);
			}
		}

		// Token: 0x0600AADB RID: 43739 RVA: 0x003C5BF8 File Offset: 0x003C3DF8
		public bool ShouldVentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			return !(primaryElement == null) && primaryElement.Mass >= base.smi.master.gasVentThreshold;
		}

		// Token: 0x0400858F RID: 34191
		[Serialize]
		public string lastConsumedEntityPrefabID;
	}

	// Token: 0x02001BBA RID: 7098
	public class States : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant>
	{
		// Token: 0x0600AADC RID: 43740 RVA: 0x003C5C54 File Offset: 0x003C3E54
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.trap;
			this.trap.DefaultState(this.trap.open);
			this.trap.open.ToggleComponent<TrapTrigger>(false).ToggleStatusItem(Db.Get().CreatureStatusItems.CarnivorousPlantAwaitingVictim, (CritterTrapPlant.StatesInstance smi) => smi.master.GetComponent<IPlantConsumeEntities>()).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).EventHandler(GameHashes.TrapTriggered, delegate(CritterTrapPlant.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).EventTransition(GameHashes.Wilt, this.trap.wilting, null).OnSignal(this.trapTriggered, this.trap.trigger).ParamTransition<bool>(this.hasEatenCreature, this.trap.digesting, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.IsTrue).PlayAnim("idle_open", KAnim.PlayMode.Loop);
			this.trap.trigger.PlayAnim("trap", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				GameObject gameObject = smi.master.storage.FindFirst(GameTags.Creature);
				smi.lastConsumedEntityPrefabID = ((gameObject != null) ? gameObject.PrefabID().ToString() : null);
				smi.master.storage.ConsumeAllIgnoringDisease();
				smi.sm.hasEatenCreature.Set(true, smi, false);
			}).OnAnimQueueComplete(this.trap.digesting);
			this.trap.digesting.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).ToggleComponent<Growing>(false).EventTransition(GameHashes.Grow, this.fruiting.enter, (CritterTrapPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.Wilt, this.trap.wilting, null).DefaultState(this.trap.digesting.idle);
			this.trap.digesting.idle.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).Update(delegate(CritterTrapPlant.StatesInstance smi, float dt)
			{
				smi.AddGas(dt);
			}, UpdateRate.SIM_4000ms, false).OnSignal(this.ventGas, this.trap.digesting.vent_pre);
			this.trap.digesting.vent_pre.PlayAnim("vent_pre").Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
			}).OnAnimQueueComplete(this.trap.digesting.vent);
			this.trap.digesting.vent.PlayAnim("vent_loop", KAnim.PlayMode.Once).QueueAnim("vent_pst", false, null).OnAnimQueueComplete(this.trap.digesting.idle);
			this.trap.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.trap, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.fruiting.EventTransition(GameHashes.Wilt, this.fruiting.wilting, null).EventTransition(GameHashes.Harvest, this.harvest, null).DefaultState(this.fruiting.idle);
			this.fruiting.enter.PlayAnim("open_harvest", KAnim.PlayMode.Once).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).OnAnimQueueComplete(this.fruiting.idle);
			this.fruiting.idle.PlayAnim("harvestable_loop", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.old, new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld), UpdateRate.SIM_4000ms);
			this.fruiting.old.PlayAnim("wilt1", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.idle, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Not(new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld)), UpdateRate.SIM_4000ms);
			this.fruiting.wilting.PlayAnim("wilt1", KAnim.PlayMode.Once).EventTransition(GameHashes.WiltRecover, this.fruiting, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.harvest.PlayAnim("harvest", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (GameScheduler.Instance != null && smi.master != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.harvestable.SetCanBeHarvested(false);
			}).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.sm.hasEatenCreature.Set(false, smi, false);
			}).OnAnimQueueComplete(this.trap.open);
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable harvestable = smi.master.harvestable;
				if (harvestable != null && harvestable.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
		}

		// Token: 0x0600AADD RID: 43741 RVA: 0x003C6194 File Offset: 0x003C4394
		public bool IsOld(CritterTrapPlant.StatesInstance smi)
		{
			return smi.master.growing.PercentOldAge() > 0.5f;
		}

		// Token: 0x04008590 RID: 34192
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal trapTriggered;

		// Token: 0x04008591 RID: 34193
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal ventGas;

		// Token: 0x04008592 RID: 34194
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.BoolParameter hasEatenCreature;

		// Token: 0x04008593 RID: 34195
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State dead;

		// Token: 0x04008594 RID: 34196
		public CritterTrapPlant.States.FruitingStates fruiting;

		// Token: 0x04008595 RID: 34197
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State harvest;

		// Token: 0x04008596 RID: 34198
		public CritterTrapPlant.States.TrapStates trap;

		// Token: 0x020029F2 RID: 10738
		public class DigestingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400B969 RID: 47465
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x0400B96A RID: 47466
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent_pre;

			// Token: 0x0400B96B RID: 47467
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent;
		}

		// Token: 0x020029F3 RID: 10739
		public class TrapStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400B96C RID: 47468
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State open;

			// Token: 0x0400B96D RID: 47469
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State trigger;

			// Token: 0x0400B96E RID: 47470
			public CritterTrapPlant.States.DigestingStates digesting;

			// Token: 0x0400B96F RID: 47471
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}

		// Token: 0x020029F4 RID: 10740
		public class FruitingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400B970 RID: 47472
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State enter;

			// Token: 0x0400B971 RID: 47473
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x0400B972 RID: 47474
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State old;

			// Token: 0x0400B973 RID: 47475
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}
	}
}
