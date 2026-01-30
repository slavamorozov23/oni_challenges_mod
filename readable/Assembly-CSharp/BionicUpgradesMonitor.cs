using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000A0E RID: 2574
public class BionicUpgradesMonitor : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>
{
	// Token: 0x06004B83 RID: 19331 RVA: 0x001B6EA0 File Offset: 0x001B50A0
	public static void CreateAssignableSlots(MinionAssignablesProxy minionAssignablesProxy)
	{
		AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
		int num = Mathf.Max(0, 7);
		for (int i = 0; i < num; i++)
		{
			string idsufix = (i + 2).ToString();
			BionicUpgradesMonitor.AddAssignableSlot(bionicUpgrade, idsufix, minionAssignablesProxy);
		}
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x001B6EE8 File Offset: 0x001B50E8
	private static void AddAssignableSlot(AssignableSlot bionicUpgradeSlot, string IDSufix, MinionAssignablesProxy minionAssignablesProxy)
	{
		Ownables component = minionAssignablesProxy.GetComponent<Ownables>();
		if (bionicUpgradeSlot is OwnableSlot)
		{
			OwnableSlotInstance ownableSlotInstance = new OwnableSlotInstance(component, (OwnableSlot)bionicUpgradeSlot);
			OwnableSlotInstance ownableSlotInstance2 = ownableSlotInstance;
			ownableSlotInstance2.ID += IDSufix;
			component.Add(ownableSlotInstance);
			return;
		}
		if (bionicUpgradeSlot is EquipmentSlot)
		{
			Equipment component2 = component.GetComponent<Equipment>();
			EquipmentSlotInstance equipmentSlotInstance = new EquipmentSlotInstance(component2, (EquipmentSlot)bionicUpgradeSlot);
			EquipmentSlotInstance equipmentSlotInstance2 = equipmentSlotInstance;
			equipmentSlotInstance2.ID += IDSufix;
			component2.Add(equipmentSlotInstance);
		}
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x001B6F60 File Offset: 0x001B5160
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.initialize;
		this.initialize.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.InitializeSlots)).EnterTransition(this.firstSpawn, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsFirstTimeSpawningThisBionic)).EnterGoTo(this.inactive);
		this.firstSpawn.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.SpawnAndInstallInitialUpgrade));
		this.inactive.EventTransition(GameHashes.BionicOnline, this.active, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
		this.active.DefaultState(this.active.idle).EventTransition(GameHashes.BionicOffline, this.inactive, GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Not(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline))).EventHandler(GameHashes.BionicUpgradeWattageChanged, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
		this.active.idle.OnSignal(this.UpgradeSlotAssignationChanged, this.active.seeking, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Parameter<StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter>.Callback(BionicUpgradesMonitor.WantsToInstallNewUpgrades));
		this.active.seeking.OnSignal(this.UpgradeSlotAssignationChanged, this.active.idle, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Parameter<StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter>.Callback(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).DefaultState(this.active.seeking.inProgress);
		this.active.seeking.inProgress.ToggleChore((BionicUpgradesMonitor.Instance smi) => new SeekAndInstallBionicUpgradeChore(smi.master), this.active.idle, this.active.seeking.failed);
		this.active.seeking.failed.EnterTransition(this.active.idle, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).GoTo(this.active.seeking.inProgress);
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x001B7164 File Offset: 0x001B5364
	public static void InitializeSlots(BionicUpgradesMonitor.Instance smi)
	{
		smi.InitializeSlots();
	}

	// Token: 0x06004B87 RID: 19335 RVA: 0x001B716C File Offset: 0x001B536C
	public static bool IsBionicOnline(BionicUpgradesMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06004B88 RID: 19336 RVA: 0x001B7174 File Offset: 0x001B5374
	public static bool WantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi, StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter param)
	{
		return smi.HasAnyUpgradeAssigned;
	}

	// Token: 0x06004B89 RID: 19337 RVA: 0x001B717C File Offset: 0x001B537C
	public static bool DoesNotWantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
	{
		return BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades(smi, null);
	}

	// Token: 0x06004B8A RID: 19338 RVA: 0x001B7185 File Offset: 0x001B5385
	public static bool DoesNotWantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi, StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter param)
	{
		return !BionicUpgradesMonitor.WantsToInstallNewUpgrades(smi, param);
	}

	// Token: 0x06004B8B RID: 19339 RVA: 0x001B7191 File Offset: 0x001B5391
	public static bool HasUpgradesInstalled(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeInstalled;
	}

	// Token: 0x06004B8C RID: 19340 RVA: 0x001B7199 File Offset: 0x001B5399
	public static bool IsFirstTimeSpawningThisBionic(BionicUpgradesMonitor.Instance smi)
	{
		return !smi.sm.InitialUpgradeSpawned.Get(smi);
	}

	// Token: 0x06004B8D RID: 19341 RVA: 0x001B71AF File Offset: 0x001B53AF
	public static void UpdateBatteryMonitorWattageModifiers(BionicUpgradesMonitor.Instance smi)
	{
		smi.UpdateBatteryMonitorWattageModifiers();
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x001B71B8 File Offset: 0x001B53B8
	public static void SpawnAndInstallInitialUpgrade(BionicUpgradesMonitor.Instance smi)
	{
		string text = smi.GetComponent<Traits>().GetTraitIds().Find((string t) => DUPLICANTSTATS.BIONICUPGRADETRAITS.Find((DUPLICANTSTATS.TraitVal st) => st.id == t).id == t);
		if (text != null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(BionicUpgradeComponentConfig.GetBionicUpgradePrefabIDWithTraitID(text)), smi.master.transform.position);
			gameObject.SetActive(true);
			IAssignableIdentity component = smi.GetComponent<IAssignableIdentity>();
			BionicUpgradeComponent component2 = gameObject.GetComponent<BionicUpgradeComponent>();
			component2.Assign(component);
			smi.InstallUpgrade(component2);
		}
		smi.sm.InitialUpgradeSpawned.Set(true, smi, false);
		smi.GoTo(smi.sm.inactive);
	}

	// Token: 0x0400320D RID: 12813
	public const int MAX_POSSIBLE_SLOT_COUNT = 8;

	// Token: 0x0400320E RID: 12814
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State initialize;

	// Token: 0x0400320F RID: 12815
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State firstSpawn;

	// Token: 0x04003210 RID: 12816
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inactive;

	// Token: 0x04003211 RID: 12817
	public BionicUpgradesMonitor.ActiveStates active;

	// Token: 0x04003212 RID: 12818
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeSlotAssignationChanged;

	// Token: 0x04003213 RID: 12819
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.BoolParameter InitialUpgradeSpawned;

	// Token: 0x02001A99 RID: 6809
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A9A RID: 6810
	public class SeekingStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x04008231 RID: 33329
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inProgress;

		// Token: 0x04008232 RID: 33330
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State failed;
	}

	// Token: 0x02001A9B RID: 6811
	public class ActiveStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x04008233 RID: 33331
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State idle;

		// Token: 0x04008234 RID: 33332
		public BionicUpgradesMonitor.SeekingStates seeking;
	}

	// Token: 0x02001A9C RID: 6812
	public new class Instance : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.GameInstance
	{
		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x0600A648 RID: 42568 RVA: 0x003B9632 File Offset: 0x003B7832
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x0600A649 RID: 42569 RVA: 0x003B9649 File Offset: 0x003B7849
		public bool HasAnyUpgradeAssigned
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyAssignedSlot() != null;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600A64A RID: 42570 RVA: 0x003B965E File Offset: 0x003B785E
		public bool HasAnyUpgradeInstalled
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyInstalledUpgradeSlot() != null;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x0600A64B RID: 42571 RVA: 0x003B9673 File Offset: 0x003B7873
		public int UnlockedSlotCount
		{
			get
			{
				return Math.Clamp((int)base.gameObject.GetAttributes().Get(Db.Get().Attributes.BionicBoosterSlots.Id).GetTotalValue(), 0, 8);
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x0600A64C RID: 42572 RVA: 0x003B96A8 File Offset: 0x003B78A8
		public int AssignedSlotCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
				{
					if (this.upgradeComponentSlots[i].assignedUpgradeComponent != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600A64D RID: 42573 RVA: 0x003B96E4 File Offset: 0x003B78E4
		public Instance(IStateMachineTarget master, BionicUpgradesMonitor.Def def) : base(master, def)
		{
			IAssignableIdentity component = base.GetComponent<IAssignableIdentity>();
			this.dataHolder = base.GetComponent<MinionStorageDataHolder>();
			MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
			minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Combine(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.navigator = base.GetComponent<Navigator>();
			this.minionOwnables = component.GetSoleOwner();
			this.upgradesStorage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicUpgradeStorage);
			this.CreateUpgradeSlots();
			base.Subscribe(540773776, new Action<object>(this.OnSlotCountAttributeChanged));
			Game.Instance.Trigger(-1523247426, this);
		}

		// Token: 0x0600A64E RID: 42574 RVA: 0x003B97C0 File Offset: 0x003B79C0
		private void OnCopyMinionBegins(StoredMinionIdentity destination)
		{
			Tag[] array = new Tag[this.upgradeComponentSlots.Length];
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				array[i] = this.upgradeComponentSlots[i].InstalledUpgradeID;
			}
			MinionStorageDataHolder.DataPackData data = new MinionStorageDataHolder.DataPackData
			{
				Bools = new bool[]
				{
					base.smi.sm.InitialUpgradeSpawned.Get(base.smi)
				},
				Tags = array
			};
			this.dataHolder.UpdateData(data);
		}

		// Token: 0x0600A64F RID: 42575 RVA: 0x003B9848 File Offset: 0x003B7A48
		public override void PostParamsInitialized()
		{
			MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicUpgradesMonitor.Instance>();
			if (dataPack != null && dataPack.IsStoringNewData)
			{
				MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
				if (dataPackData != null)
				{
					base.sm.InitialUpgradeSpawned.Set(dataPackData.Bools[0], base.smi, false);
					if (dataPackData.Tags != null)
					{
						for (int i = 0; i < Mathf.Min(dataPackData.Tags.Length, this.upgradeComponentSlots.Length); i++)
						{
							Tag installedUpgradePrefabID = dataPackData.Tags[i];
							this.upgradeComponentSlots[i].DeserializeAction_OverrideInstalledUpgradePrefabID(installedUpgradePrefabID);
						}
					}
				}
			}
			base.PostParamsInitialized();
		}

		// Token: 0x0600A650 RID: 42576 RVA: 0x003B98DF File Offset: 0x003B7ADF
		protected override void OnCleanUp()
		{
			if (this.dataHolder != null)
			{
				MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
				minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Remove(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			}
			base.OnCleanUp();
		}

		// Token: 0x0600A651 RID: 42577 RVA: 0x003B991C File Offset: 0x003B7B1C
		public void LockSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			this.UninstallUpgrade(slot);
			if (slot.HasUpgradeComponentAssigned && slot.HasSpawned)
			{
				slot.InternalUninstall();
			}
			slot.InternalLock();
		}

		// Token: 0x0600A652 RID: 42578 RVA: 0x003B9941 File Offset: 0x003B7B41
		public void UnlockSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			slot.InternalUnlock();
		}

		// Token: 0x0600A653 RID: 42579 RVA: 0x003B994C File Offset: 0x003B7B4C
		public void InstallUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot slotForAssignedUpgrade = this.GetSlotForAssignedUpgrade(upgradeComponent);
			if (slotForAssignedUpgrade == null)
			{
				return;
			}
			slotForAssignedUpgrade.InternalInstall();
			Game.Instance.Trigger(-1523247426, this);
		}

		// Token: 0x0600A654 RID: 42580 RVA: 0x003B997B File Offset: 0x003B7B7B
		public void UninstallUpgrade(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			if (slot != null && slot.HasUpgradeInstalled)
			{
				slot.InternalUninstall();
				Game.Instance.Trigger(-1523247426, this);
			}
		}

		// Token: 0x0600A655 RID: 42581 RVA: 0x003B99A0 File Offset: 0x003B7BA0
		public void UpdateBatteryMonitorWattageModifiers()
		{
			bool flag = true;
			bool flag2 = false;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				flag &= this.upgradeComponentSlots[i].HasUpgradeInstalled;
				string text = "UPGRADE_SLOT_" + i.ToString();
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.HasUpgradeInstalled)
				{
					flag2 |= this.batteryMonitor.RemoveModifier(text, false);
				}
				else
				{
					BionicBatteryMonitor.WattageModifier modifier = new BionicBatteryMonitor.WattageModifier
					{
						id = text,
						name = upgradeComponentSlot.installedUpgradeComponent.CurrentWattageName,
						value = upgradeComponentSlot.installedUpgradeComponent.CurrentWattage,
						potentialValue = upgradeComponentSlot.installedUpgradeComponent.PotentialWattage
					};
					flag2 |= this.batteryMonitor.AddOrUpdateModifier(modifier, false);
				}
			}
			if (flag2)
			{
				this.batteryMonitor.Trigger(1361471071, null);
			}
			if (flag)
			{
				SaveGame.Instance.ColonyAchievementTracker.fullyBoostedBionic = true;
			}
		}

		// Token: 0x0600A656 RID: 42582 RVA: 0x003B9A98 File Offset: 0x003B7C98
		private void OnSlotCountAttributeChanged(object data)
		{
			int unlockedSlotCount = this.UnlockedSlotCount;
			bool flag = false;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				bool flag2 = i >= unlockedSlotCount;
				if (upgradeComponentSlot.IsLocked != flag2)
				{
					flag = true;
					if (flag2)
					{
						this.LockSlot(upgradeComponentSlot);
					}
					else
					{
						this.UnlockSlot(upgradeComponentSlot);
					}
				}
			}
			this.UpdateBatteryMonitorWattageModifiers();
			if (flag)
			{
				base.Trigger(1095596132, null);
			}
		}

		// Token: 0x0600A657 RID: 42583 RVA: 0x003B9B08 File Offset: 0x003B7D08
		private void CreateUpgradeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			this.minionOwnables.GetSlots(bionicUpgrade);
			this.upgradeComponentSlots = new BionicUpgradesMonitor.UpgradeComponentSlot[8];
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = new BionicUpgradesMonitor.UpgradeComponentSlot();
				this.upgradeComponentSlots[i] = upgradeComponentSlot;
			}
		}

		// Token: 0x0600A658 RID: 42584 RVA: 0x003B9B60 File Offset: 0x003B7D60
		public void InitializeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			AssignableSlotInstance[] slots = this.minionOwnables.GetSlots(bionicUpgrade);
			int unlockedSlotCount = this.UnlockedSlotCount;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot slot = this.upgradeComponentSlots[i];
				this.InitializeUpgradeSlot(slot, slots[i]);
			}
			for (int j = 0; j < this.upgradeComponentSlots.Length; j++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[j];
				upgradeComponentSlot.OnSpawn(this);
				bool flag = j >= unlockedSlotCount;
				if (flag != upgradeComponentSlot.IsLocked)
				{
					if (flag)
					{
						this.LockSlot(upgradeComponentSlot);
					}
					else
					{
						this.UnlockSlot(upgradeComponentSlot);
					}
				}
			}
		}

		// Token: 0x0600A659 RID: 42585 RVA: 0x003B9C10 File Offset: 0x003B7E10
		private void InitializeUpgradeSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot, AssignableSlotInstance assignableSlotInstance)
		{
			slot.Initialize(assignableSlotInstance, this.upgradesStorage, this);
			slot.OnInstalledUpgradeReassigned = (Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>)Delegate.Combine(slot.OnInstalledUpgradeReassigned, new Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>(this.OnInstalledUpgradeComponentReassigned));
			slot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(slot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnSlotAssignationChanged));
		}

		// Token: 0x0600A65A RID: 42586 RVA: 0x003B9C6F File Offset: 0x003B7E6F
		private void OnSlotAssignationChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			base.sm.UpgradeSlotAssignationChanged.Trigger(this);
		}

		// Token: 0x0600A65B RID: 42587 RVA: 0x003B9C82 File Offset: 0x003B7E82
		private void OnInstalledUpgradeComponentReassigned(BionicUpgradesMonitor.UpgradeComponentSlot slot, IAssignableIdentity new_assignee)
		{
			if (!slot.AssignedUpgradeMatchesInstalledUpgrade)
			{
				this.UninstallUpgrade(slot);
			}
		}

		// Token: 0x0600A65C RID: 42588 RVA: 0x003B9C94 File Offset: 0x003B7E94
		private BionicUpgradesMonitor.UpgradeComponentSlot GetSlotForAssignedUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && upgradeComponentSlot.assignedUpgradeComponent == upgradeComponent)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x0600A65D RID: 42589 RVA: 0x003B9CE4 File Offset: 0x003B7EE4
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyAssignedSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x0600A65E RID: 42590 RVA: 0x003B9D24 File Offset: 0x003B7F24
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyReachableAssignedSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && this.IsBionicUpgradeComponentObjectAbleToBePickedUp(upgradeComponentSlot.assignedUpgradeComponent))
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x0600A65F RID: 42591 RVA: 0x003B9D74 File Offset: 0x003B7F74
		public bool IsBionicUpgradeComponentObjectAbleToBePickedUp(BionicUpgradeComponent upgradecComponent)
		{
			Pickupable component = upgradecComponent.GetComponent<Pickupable>();
			return !(component == null) && !component.KPrefabID.HasTag(GameTags.StoredPrivate) && component.CouldBePickedUpByMinion(base.GetComponent<KPrefabID>().InstanceID) && this.navigator.CanReach(component);
		}

		// Token: 0x0600A660 RID: 42592 RVA: 0x003B9DD0 File Offset: 0x003B7FD0
		private BionicUpgradesMonitor.UpgradeComponentSlot GetAnyInstalledUpgradeSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && upgradeComponentSlot.HasUpgradeInstalled)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x0600A661 RID: 42593 RVA: 0x003B9E08 File Offset: 0x003B8008
		public BionicUpgradesMonitor.UpgradeComponentSlot GetFirstEmptyAvailableSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.IsLocked && !upgradeComponentSlot.HasUpgradeInstalled && !upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x0600A662 RID: 42594 RVA: 0x003B9E4C File Offset: 0x003B804C
		public int CountBoosterAssignments(Tag boosterID)
		{
			int num = 0;
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in this.upgradeComponentSlots)
			{
				if (!(upgradeComponentSlot.assignedUpgradeComponent == null) && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == boosterID)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04008235 RID: 33333
		[Serialize]
		public BionicUpgradesMonitor.UpgradeComponentSlot[] upgradeComponentSlots;

		// Token: 0x04008236 RID: 33334
		private BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x04008237 RID: 33335
		private Storage upgradesStorage;

		// Token: 0x04008238 RID: 33336
		private Ownables minionOwnables;

		// Token: 0x04008239 RID: 33337
		private MinionStorageDataHolder dataHolder;

		// Token: 0x0400823A RID: 33338
		private Navigator navigator;

		// Token: 0x020029D5 RID: 10709
		[SerializationConfig(MemberSerialization.OptIn)]
		private struct StorageDataHolderData
		{
			// Token: 0x0400B904 RID: 47364
			[Serialize]
			public bool initialUpgradesSpawned;

			// Token: 0x0400B905 RID: 47365
			[Serialize]
			public Tag[] upgradeComponentSlotsInstalledTags;
		}
	}

	// Token: 0x02001A9D RID: 6813
	[SerializationConfig(MemberSerialization.OptIn)]
	public class UpgradeComponentSlot
	{
		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x0600A663 RID: 42595 RVA: 0x003B9E9A File Offset: 0x003B809A
		public bool HasUpgradeInstalled
		{
			get
			{
				return this.installedUpgradePrefabID != Tag.Invalid;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600A664 RID: 42596 RVA: 0x003B9EAC File Offset: 0x003B80AC
		public bool HasUpgradeComponentAssigned
		{
			get
			{
				return this.assignableSlotInstance.IsAssigned() && !this.assignableSlotInstance.IsUnassigning();
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600A665 RID: 42597 RVA: 0x003B9ECB File Offset: 0x003B80CB
		public bool AssignedUpgradeMatchesInstalledUpgrade
		{
			get
			{
				return this.assignedUpgradeComponent == this.installedUpgradeComponent;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600A667 RID: 42599 RVA: 0x003B9EE7 File Offset: 0x003B80E7
		// (set) Token: 0x0600A666 RID: 42598 RVA: 0x003B9EDE File Offset: 0x003B80DE
		public bool HasSpawned { get; private set; }

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600A669 RID: 42601 RVA: 0x003B9EF8 File Offset: 0x003B80F8
		// (set) Token: 0x0600A668 RID: 42600 RVA: 0x003B9EEF File Offset: 0x003B80EF
		public bool IsLocked { get; private set; }

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x0600A66A RID: 42602 RVA: 0x003B9F00 File Offset: 0x003B8100
		public float WattageCost
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return 0f;
				}
				return this.installedUpgradeComponent.CurrentWattage;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x0600A66B RID: 42603 RVA: 0x003B9F1B File Offset: 0x003B811B
		public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return null;
				}
				return this.installedUpgradeComponent.StateMachine;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600A66C RID: 42604 RVA: 0x003B9F32 File Offset: 0x003B8132
		public Tag InstalledUpgradeID
		{
			get
			{
				return this.installedUpgradePrefabID;
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x0600A66D RID: 42605 RVA: 0x003B9F3A File Offset: 0x003B813A
		public BionicUpgradeComponent assignedUpgradeComponent
		{
			get
			{
				if (!this.assignableSlotInstance.IsUnassigning())
				{
					return this.assignableSlotInstance.assignable as BionicUpgradeComponent;
				}
				return null;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600A66E RID: 42606 RVA: 0x003B9F5C File Offset: 0x003B815C
		public BionicUpgradeComponent installedUpgradeComponent
		{
			get
			{
				if (this.HasUpgradeInstalled)
				{
					if (this._installedUpgradeComponent == null)
					{
						global::Debug.LogWarning("Error on BionicUpgradeMonitor. storage does not contains bionic upgrade with id " + this.InstalledUpgradeID.ToString() + " this could be due to loading an old save on a new version");
						this.installedUpgradePrefabID = Tag.Invalid;
					}
					return this._installedUpgradeComponent;
				}
				this._installedUpgradeComponent = null;
				return null;
			}
		}

		// Token: 0x0600A66F RID: 42607 RVA: 0x003B9FC1 File Offset: 0x003B81C1
		public void DeserializeAction_OverrideInstalledUpgradePrefabID(Tag installedUpgradePrefabID)
		{
			this.installedUpgradePrefabID = installedUpgradePrefabID;
		}

		// Token: 0x0600A671 RID: 42609 RVA: 0x003B9FE4 File Offset: 0x003B81E4
		public void Initialize(AssignableSlotInstance assignableSlotInstance, Storage storage, BionicUpgradesMonitor.Instance master)
		{
			this.assignableSlotInstance = assignableSlotInstance;
			this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().Subscribe(-1585839766, new Action<object>(this.OnAssignablesChanged));
			this.storage = storage;
			this.master = master;
			this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
		}

		// Token: 0x0600A672 RID: 42610 RVA: 0x003BA03E File Offset: 0x003B823E
		public AssignableSlotInstance GetAssignableSlotInstance()
		{
			return this.assignableSlotInstance;
		}

		// Token: 0x0600A673 RID: 42611 RVA: 0x003BA048 File Offset: 0x003B8248
		public void OnSpawn(BionicUpgradesMonitor.Instance smi)
		{
			if (this.HasUpgradeInstalled && this._installedUpgradeComponent == null)
			{
				GameObject gameObject = null;
				int num = 0;
				List<GameObject> list = new List<GameObject>();
				this.storage.Find(this.InstalledUpgradeID, list);
				while (num < list.Count && this._installedUpgradeComponent == null)
				{
					GameObject gameObject2 = list[num];
					bool flag = false;
					foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
					{
						if (upgradeComponentSlot != this && upgradeComponentSlot.HasSpawned && !(upgradeComponentSlot.InstalledUpgradeID != this.InstalledUpgradeID) && upgradeComponentSlot.installedUpgradeComponent.gameObject == gameObject2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						gameObject = gameObject2;
						break;
					}
					num++;
				}
				if (gameObject != null)
				{
					this._installedUpgradeComponent = gameObject.GetComponent<BionicUpgradeComponent>();
					this.StartBoosterSM();
				}
			}
			if (this.HasUpgradeInstalled && this.installedUpgradeComponent != null)
			{
				if (!this.HasUpgradeComponentAssigned)
				{
					this.installedUpgradeComponent.Assign(this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>(), this.assignableSlotInstance);
				}
				this.SubscribeToInstallledUpgradeAssignable();
			}
			this.HasSpawned = true;
		}

		// Token: 0x0600A674 RID: 42612 RVA: 0x003BA185 File Offset: 0x003B8385
		public void SubscribeToInstallledUpgradeAssignable()
		{
			this.UnsubscribeFromInstalledUpgradeAssignable();
			this.installedUpgradeSubscribeCallbackIDX = this.installedUpgradeComponent.Subscribe(684616645, new Action<object>(this.OnInstalledComponentReassigned));
		}

		// Token: 0x0600A675 RID: 42613 RVA: 0x003BA1AF File Offset: 0x003B83AF
		public void UnsubscribeFromInstalledUpgradeAssignable()
		{
			if (this.installedUpgradeSubscribeCallbackIDX != -1)
			{
				this.installedUpgradeComponent.Unsubscribe(this.installedUpgradeSubscribeCallbackIDX);
				this.installedUpgradeSubscribeCallbackIDX = -1;
			}
		}

		// Token: 0x0600A676 RID: 42614 RVA: 0x003BA1D4 File Offset: 0x003B83D4
		private void OnInstalledComponentReassigned(object obj)
		{
			IAssignableIdentity arg = (obj == null) ? null : ((IAssignableIdentity)obj);
			Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> onInstalledUpgradeReassigned = this.OnInstalledUpgradeReassigned;
			if (onInstalledUpgradeReassigned == null)
			{
				return;
			}
			onInstalledUpgradeReassigned(this, arg);
		}

		// Token: 0x0600A677 RID: 42615 RVA: 0x003BA200 File Offset: 0x003B8400
		private void OnAssignablesChanged(object o)
		{
			if (this._lastAssignedUpgradeComponent != this.assignedUpgradeComponent)
			{
				this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
				Action<BionicUpgradesMonitor.UpgradeComponentSlot> onAssignedUpgradeChanged = this.OnAssignedUpgradeChanged;
				if (onAssignedUpgradeChanged == null)
				{
					return;
				}
				onAssignedUpgradeChanged(this);
			}
		}

		// Token: 0x0600A678 RID: 42616 RVA: 0x003BA232 File Offset: 0x003B8432
		private void StartBoosterSM()
		{
			this._upgradeSmi = this.installedUpgradeComponent.StateMachine(this.master);
			this._upgradeSmi.StartSM();
		}

		// Token: 0x0600A679 RID: 42617 RVA: 0x003BA25C File Offset: 0x003B845C
		public void InternalInstall()
		{
			if (!this.HasUpgradeInstalled && this.HasUpgradeComponentAssigned)
			{
				this.storage.Store(this.assignedUpgradeComponent.gameObject, true, false, true, false);
				this.installedUpgradePrefabID = this.assignedUpgradeComponent.PrefabID();
				this._installedUpgradeComponent = this.assignedUpgradeComponent;
				this.SubscribeToInstallledUpgradeAssignable();
				this.StartBoosterSM();
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x0600A67A RID: 42618 RVA: 0x003BA2E8 File Offset: 0x003B84E8
		public void InternalUninstall()
		{
			if (this.HasUpgradeInstalled)
			{
				this.UnsubscribeFromInstalledUpgradeAssignable();
				GameObject gameObject = this.installedUpgradeComponent.gameObject;
				this.installedUpgradeComponent.Unassign();
				this.storage.Drop(gameObject, true);
				this.installedUpgradePrefabID = Tag.Invalid;
				this._installedUpgradeComponent = null;
				if (this._upgradeSmi != null)
				{
					this._upgradeSmi.StopSM("Uninstall");
					this._upgradeSmi = null;
				}
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x0600A67B RID: 42619 RVA: 0x003BA387 File Offset: 0x003B8587
		public void InternalLock()
		{
			this.IsLocked = true;
		}

		// Token: 0x0600A67C RID: 42620 RVA: 0x003BA390 File Offset: 0x003B8590
		public void InternalUnlock()
		{
			this.IsLocked = false;
		}

		// Token: 0x0400823D RID: 33341
		private BionicUpgradeComponent _installedUpgradeComponent;

		// Token: 0x0400823E RID: 33342
		private BionicUpgradeComponent _lastAssignedUpgradeComponent;

		// Token: 0x0400823F RID: 33343
		[Serialize]
		private Tag installedUpgradePrefabID = Tag.Invalid;

		// Token: 0x04008240 RID: 33344
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> OnInstalledUpgradeReassigned;

		// Token: 0x04008241 RID: 33345
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot> OnAssignedUpgradeChanged;

		// Token: 0x04008242 RID: 33346
		private AssignableSlotInstance assignableSlotInstance;

		// Token: 0x04008243 RID: 33347
		private Storage storage;

		// Token: 0x04008244 RID: 33348
		private int installedUpgradeSubscribeCallbackIDX = -1;

		// Token: 0x04008245 RID: 33349
		private StateMachine.Instance _upgradeSmi;

		// Token: 0x04008246 RID: 33350
		private BionicUpgradesMonitor.Instance master;
	}
}
