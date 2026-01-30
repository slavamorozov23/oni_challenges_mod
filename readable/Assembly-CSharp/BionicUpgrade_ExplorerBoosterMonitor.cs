using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020006EC RID: 1772
public class BionicUpgrade_ExplorerBoosterMonitor : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>
{
	// Token: 0x06002BC0 RID: 11200 RVA: 0x000FF1B0 File Offset: 0x000FD3B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.attachToBooster;
		this.attachToBooster.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.FindAndAttachToInstalledBooster)).GoTo(this.Inactive);
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore))).EventTransition(GameHashes.ScheduleChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore))).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline))).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive))).DefaultState(this.Active.gatheringData);
		this.Active.gatheringData.OnSignal(this.ReadyToDiscoverSignal, this.Active.discover, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Parameter<StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.SignalParameter>.Callback(BionicUpgrade_ExplorerBoosterMonitor.IsReadyToDiscoverAndThereIsSomethingToDiscover)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicExplorerBooster, null).Update(new Action<BionicUpgrade_ExplorerBoosterMonitor.Instance, float>(BionicUpgrade_ExplorerBoosterMonitor.DataGatheringUpdate), UpdateRate.SIM_200ms, false);
		this.Active.discover.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.ConsumeAllData)).Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.RevealUndiscoveredGeyser)).EnterTransition(this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover))).GoTo(this.Active.gatheringData);
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x000FF3FA File Offset: 0x000FD5FA
	public static bool ShouldBeActive(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline(smi) && BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore(smi) && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x000FF414 File Offset: 0x000FD614
	public static bool IsReadyToDiscoverAndThereIsSomethingToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi, StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.SignalParameter param)
	{
		return smi.IsReadyToDiscover && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x000FF426 File Offset: 0x000FD626
	public static void ConsumeAllData(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.ConsumeAllData();
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x000FF42E File Offset: 0x000FD62E
	public static void FindAndAttachToInstalledBooster(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.Initialize();
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x000FF436 File Offset: 0x000FD636
	public static void DataGatheringUpdate(BionicUpgrade_ExplorerBoosterMonitor.Instance smi, float dt)
	{
		smi.GatheringDataUpdate(dt);
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x000FF440 File Offset: 0x000FD640
	public static bool IsThereGeysersToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			return list.Count > 0;
		}
		return false;
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x000FF4DC File Offset: 0x000FD6DC
	public static void RevealUndiscoveredGeyser(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			if (list.Count > 0)
			{
				WorldGenSpawner.Spawnable random = list.GetRandom<WorldGenSpawner.Spawnable>();
				int baseX;
				int baseY;
				Grid.CellToXY(random.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, 4, 4f);
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification geyserDiscoveredNotification = smi.GetGeyserDiscoveredNotification();
				int cell = random.cell;
				geyserDiscoveredNotification.customClickCallback = delegate(object obj)
				{
					GameUtil.FocusCamera(cell, true);
				};
				notifier.Add(geyserDiscoveredNotification, "");
			}
		}
	}

	// Token: 0x04001A09 RID: 6665
	public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State attachToBooster;

	// Token: 0x04001A0A RID: 6666
	public new BionicUpgrade_ExplorerBoosterMonitor.ActiveStates Active;

	// Token: 0x04001A0B RID: 6667
	public StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Signal ReadyToDiscoverSignal;

	// Token: 0x020015A7 RID: 5543
	public new class Def : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def
	{
		// Token: 0x06009404 RID: 37892 RVA: 0x00377647 File Offset: 0x00375847
		public Def(string upgradeID) : base(upgradeID)
		{
		}

		// Token: 0x06009405 RID: 37893 RVA: 0x00377650 File Offset: 0x00375850
		public override string GetDescription()
		{
			return "BionicUpgrade_ExplorerBoosterMonitor.Def description not implemented";
		}
	}

	// Token: 0x020015A8 RID: 5544
	public class ActiveStates : GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State
	{
		// Token: 0x04007255 RID: 29269
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State gatheringData;

		// Token: 0x04007256 RID: 29270
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State discover;
	}

	// Token: 0x020015A9 RID: 5545
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.BaseInstance
	{
		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06009407 RID: 37895 RVA: 0x0037765F File Offset: 0x0037585F
		public bool IsReadyToDiscover
		{
			get
			{
				return this.explorerBooster != null && this.explorerBooster.IsReady;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06009408 RID: 37896 RVA: 0x00377676 File Offset: 0x00375876
		public float CurrentProgress
		{
			get
			{
				if (this.explorerBooster != null)
				{
					return this.explorerBooster.Progress;
				}
				return 0f;
			}
		}

		// Token: 0x06009409 RID: 37897 RVA: 0x00377691 File Offset: 0x00375891
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBoosterMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600940A RID: 37898 RVA: 0x0037769C File Offset: 0x0037589C
		public void Initialize()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgrade_ExplorerBooster.Instance smi = upgradeComponentSlot.installedUpgradeComponent.GetSMI<BionicUpgrade_ExplorerBooster.Instance>();
					if (smi != null && !smi.IsBeingMonitored)
					{
						this.explorerBooster = smi;
						smi.SetMonitor(this);
						return;
					}
				}
			}
		}

		// Token: 0x0600940B RID: 37899 RVA: 0x003776FA File Offset: 0x003758FA
		protected override void OnCleanUp()
		{
			if (this.explorerBooster != null)
			{
				this.explorerBooster.SetMonitor(null);
			}
			base.OnCleanUp();
		}

		// Token: 0x0600940C RID: 37900 RVA: 0x00377718 File Offset: 0x00375918
		public void GatheringDataUpdate(float dt)
		{
			bool isReadyToDiscover = this.IsReadyToDiscover;
			float dataProgressDelta = (dt == 0f) ? 0f : (dt / 600f);
			this.explorerBooster.AddData(dataProgressDelta);
			if (this.IsReadyToDiscover && !isReadyToDiscover)
			{
				base.sm.ReadyToDiscoverSignal.Trigger(this);
			}
		}

		// Token: 0x0600940D RID: 37901 RVA: 0x0037776B File Offset: 0x0037596B
		public void ConsumeAllData()
		{
			this.explorerBooster.SetDataProgress(0f);
		}

		// Token: 0x0600940E RID: 37902 RVA: 0x00377780 File Offset: 0x00375980
		public Notification GetGeyserDiscoveredNotification()
		{
			return new Notification(DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_NAME, NotificationType.MessageImportant, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		}

		// Token: 0x0600940F RID: 37903 RVA: 0x003777CA File Offset: 0x003759CA
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06009410 RID: 37904 RVA: 0x003777F0 File Offset: 0x003759F0
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x04007257 RID: 29271
		private BionicUpgrade_ExplorerBooster.Instance explorerBooster;
	}
}
