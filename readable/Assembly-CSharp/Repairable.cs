using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000627 RID: 1575
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Repairable")]
public class Repairable : Workable
{
	// Token: 0x0600257C RID: 9596 RVA: 0x000D7200 File Offset: 0x000D5400
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		base.Subscribe<Repairable>(493375141, Repairable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x000D72B0 File Offset: 0x000D54B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Repairable.SMInstance(this);
		this.smi.StartSM();
		this.workTime = float.PositiveInfinity;
		this.workTimeRemaining = float.PositiveInfinity;
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x000D72E5 File Offset: 0x000D54E5
	private void OnProxyStorageChanged(object data)
	{
		base.Trigger(-1697596308, data);
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x000D72F3 File Offset: 0x000D54F3
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000D7302 File Offset: 0x000D5502
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("Destroy Repairable");
		}
		base.OnCleanUp();
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000D7324 File Offset: 0x000D5524
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject != null && this.smi != null)
		{
			if (this.smi.GetCurrentState() == this.smi.sm.forbidden)
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.NAME, new System.Action(this.AllowRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.NAME, new System.Action(this.CancelRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
		}
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x000D7408 File Offset: 0x000D5608
	private void AllowRepair()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.hp.Repair(this.hp.MaxHitPoints);
			this.OnCompleteWork(null);
		}
		this.smi.sm.allow.Trigger(this.smi);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000D745B File Offset: 0x000D565B
	public void CancelRepair()
	{
		if (this.smi != null)
		{
			this.smi.sm.forbid.Trigger(this.smi);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x000D7488 File Offset: 0x000D5688
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, false);
		}
		this.smi.sm.worker.Set(worker, this.smi);
		this.timeSpentRepairing = 0f;
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x000D74E0 File Offset: 0x000D56E0
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float num = Mathf.Sqrt(base.GetComponent<PrimaryElement>().Mass);
		float num2 = ((this.expectedRepairTime < 0f) ? num : this.expectedRepairTime) * 0.1f;
		if (this.timeSpentRepairing >= num2)
		{
			this.timeSpentRepairing -= num2;
			int num3 = 0;
			if (worker != null)
			{
				num3 = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
			}
			int repair_amount = Mathf.CeilToInt((float)(10 + Math.Max(0, num3 * 10)) * 0.1f);
			this.hp.Repair(repair_amount);
			if (this.hp.HitPoints >= this.hp.MaxHitPoints)
			{
				return true;
			}
		}
		this.timeSpentRepairing += dt;
		return false;
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x000D75A8 File Offset: 0x000D57A8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x000D75D8 File Offset: 0x000D57D8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x000D7604 File Offset: 0x000D5804
	public void CreateStorageProxy()
	{
		if (this.storageProxy == null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(RepairableStorageProxy.ID), base.transform.gameObject, null);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			this.storageProxy = gameObject.GetComponent<Storage>();
			this.storageProxy.prioritizable = base.transform.GetComponent<Prioritizable>();
			this.storageProxy.prioritizable.AddRef();
			gameObject.GetComponent<KSelectable>().entityName = base.transform.gameObject.GetProperName();
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x000D76A8 File Offset: 0x000D58A8
	[OnSerializing]
	private void OnSerializing()
	{
		this.storedData = null;
		if (this.storageProxy != null && !this.storageProxy.IsEmpty())
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					this.storageProxy.Serialize(binaryWriter);
				}
				this.storedData = memoryStream.ToArray();
			}
		}
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x000D7730 File Offset: 0x000D5930
	[OnSerialized]
	private void OnSerialized()
	{
		this.storedData = null;
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x000D773C File Offset: 0x000D593C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.storedData != null)
		{
			FastReader reader = new FastReader(this.storedData);
			this.CreateStorageProxy();
			this.storageProxy.Deserialize(reader);
			this.storedData = null;
		}
	}

	// Token: 0x040015FA RID: 5626
	public float expectedRepairTime = -1f;

	// Token: 0x040015FB RID: 5627
	[MyCmpGet]
	private BuildingHP hp;

	// Token: 0x040015FC RID: 5628
	private Repairable.SMInstance smi;

	// Token: 0x040015FD RID: 5629
	private Storage storageProxy;

	// Token: 0x040015FE RID: 5630
	[Serialize]
	private byte[] storedData;

	// Token: 0x040015FF RID: 5631
	private float timeSpentRepairing;

	// Token: 0x04001600 RID: 5632
	private static readonly Operational.Flag repairedFlag = new Operational.Flag("repaired", Operational.Flag.Type.Functional);

	// Token: 0x04001601 RID: 5633
	private static readonly EventSystem.IntraObjectHandler<Repairable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Repairable>(delegate(Repairable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x020014FA RID: 5370
	public class SMInstance : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.GameInstance
	{
		// Token: 0x060091B4 RID: 37300 RVA: 0x00371AC0 File Offset: 0x0036FCC0
		public SMInstance(Repairable smi) : base(smi)
		{
		}

		// Token: 0x060091B5 RID: 37301 RVA: 0x00371ACC File Offset: 0x0036FCCC
		public bool HasRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			return primaryElement != null && primaryElement.Mass >= num;
		}

		// Token: 0x060091B6 RID: 37302 RVA: 0x00371B20 File Offset: 0x0036FD20
		public KeyValuePair<Tag, float> GetRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float value = (primaryElement != null) ? Math.Max(0f, num - primaryElement.Mass) : num;
			return new KeyValuePair<Tag, float>(component.Element.tag, value);
		}

		// Token: 0x060091B7 RID: 37303 RVA: 0x00371B8D File Offset: 0x0036FD8D
		public void ConsumeRepairMaterials()
		{
			base.smi.master.storageProxy.ConsumeAllIgnoringDisease();
		}

		// Token: 0x060091B8 RID: 37304 RVA: 0x00371BA4 File Offset: 0x0036FDA4
		public void DestroyStorageProxy()
		{
			if (base.smi.master.storageProxy != null)
			{
				base.smi.master.transform.GetComponent<Prioritizable>().RemoveRef();
				List<GameObject> list = new List<GameObject>();
				Storage storageProxy = base.smi.master.storageProxy;
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list;
				storageProxy.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				GameObject gameObject = base.smi.sm.worker.Get(base.smi);
				if (gameObject != null)
				{
					foreach (GameObject go in list)
					{
						go.Trigger(580035959, gameObject.GetComponent<WorkerBase>());
					}
				}
				base.smi.sm.worker.Set(null, base.smi);
				Util.KDestroyGameObject(base.smi.master.storageProxy.gameObject);
			}
		}

		// Token: 0x060091B9 RID: 37305 RVA: 0x00371CB8 File Offset: 0x0036FEB8
		public bool NeedsRepairs()
		{
			return base.smi.master.GetComponent<BuildingHP>().NeedsRepairs;
		}

		// Token: 0x0400701E RID: 28702
		private const float REQUIRED_MASS_SCALE = 0.1f;
	}

	// Token: 0x020014FB RID: 5371
	public class States : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable>
	{
		// Token: 0x060091BA RID: 37306 RVA: 0x00371CD0 File Offset: 0x0036FED0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.repaired;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.forbidden.OnSignal(this.allow, this.repaired);
			this.allowed.Enter(delegate(Repairable.SMInstance smi)
			{
				smi.master.CreateStorageProxy();
			}).DefaultState(this.allowed.needMass).EventHandler(GameHashes.BuildingFullyRepaired, delegate(Repairable.SMInstance smi)
			{
				smi.ConsumeRepairMaterials();
			}).EventTransition(GameHashes.BuildingFullyRepaired, this.repaired, null).OnSignal(this.forbid, this.forbidden).Exit(delegate(Repairable.SMInstance smi)
			{
				smi.DestroyStorageProxy();
			});
			this.allowed.needMass.Enter(delegate(Repairable.SMInstance smi)
			{
				Prioritizable.AddRef(smi.master.storageProxy.transform.parent.gameObject);
			}).Exit(delegate(Repairable.SMInstance smi)
			{
				if (!smi.isMasterNull && smi.master.storageProxy != null)
				{
					Prioritizable.RemoveRef(smi.master.storageProxy.transform.parent.gameObject);
				}
			}).EventTransition(GameHashes.OnStorageChange, this.allowed.repairable, (Repairable.SMInstance smi) => smi.HasRequiredMass()).ToggleChore(new Func<Repairable.SMInstance, Chore>(this.CreateFetchChore), this.allowed.repairable, this.allowed.needMass).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForRepairMaterials, (Repairable.SMInstance smi) => smi.GetRequiredMass());
			this.allowed.repairable.ToggleRecurringChore(new Func<Repairable.SMInstance, Chore>(this.CreateRepairChore), null).ToggleStatusItem(Db.Get().BuildingStatusItems.PendingRepair, null);
			this.repaired.EventTransition(GameHashes.BuildingReceivedDamage, this.allowed, (Repairable.SMInstance smi) => smi.NeedsRepairs()).OnSignal(this.allow, this.allowed).OnSignal(this.forbid, this.forbidden);
		}

		// Token: 0x060091BB RID: 37307 RVA: 0x00371F1C File Offset: 0x0037011C
		private Chore CreateFetchChore(Repairable.SMInstance smi)
		{
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			PrimaryElement primaryElement = smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float amount = component.Mass * 0.1f - ((primaryElement != null) ? primaryElement.Mass : 0f);
			HashSet<Tag> tags = new HashSet<Tag>
			{
				GameTagExtensions.Create(component.ElementID)
			};
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storageProxy, amount, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x060091BC RID: 37308 RVA: 0x00371FB8 File Offset: 0x003701B8
		private Chore CreateRepairChore(Repairable.SMInstance smi)
		{
			WorkChore<Repairable> workChore = new WorkChore<Repairable>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			Deconstructable component = smi.master.GetComponent<Deconstructable>();
			if (component != null)
			{
				workChore.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
			}
			Breakable component2 = smi.master.GetComponent<Breakable>();
			if (component2 != null)
			{
				workChore.AddPrecondition(Repairable.States.IsNotBeingAttacked, component2);
			}
			workChore.AddPrecondition(Repairable.States.IsNotAngry, null);
			return workChore;
		}

		// Token: 0x0400701F RID: 28703
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal allow;

		// Token: 0x04007020 RID: 28704
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal forbid;

		// Token: 0x04007021 RID: 28705
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State forbidden;

		// Token: 0x04007022 RID: 28706
		public Repairable.States.AllowedState allowed;

		// Token: 0x04007023 RID: 28707
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repaired;

		// Token: 0x04007024 RID: 28708
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.TargetParameter worker;

		// Token: 0x04007025 RID: 28709
		public static readonly Chore.Precondition IsNotBeingAttacked = new Chore.Precondition
		{
			id = "IsNotBeingAttacked",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				bool result = true;
				if (data != null)
				{
					result = (((Breakable)data).worker == null);
				}
				return result;
			}
		};

		// Token: 0x04007026 RID: 28710
		public static readonly Chore.Precondition IsNotAngry = new Chore.Precondition
		{
			id = "IsNotAngry",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_ANGRY,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Traits traits = context.consumerState.traits;
				AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject);
				return !(traits != null) || amountInstance == null || amountInstance.value < STRESS.ACTING_OUT_RESET || !traits.HasTrait("Aggressive");
			}
		};

		// Token: 0x020028A9 RID: 10409
		public class AllowedState : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State
		{
			// Token: 0x0400B31D RID: 45853
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State needMass;

			// Token: 0x0400B31E RID: 45854
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repairable;
		}
	}
}
