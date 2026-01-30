using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000856 RID: 2134
[AddComponentMenu("KMonoBehaviour/scripts/ColonyAchievementTracker")]
public class ColonyAchievementTracker : KMonoBehaviour, ISaveLoadableDetails, IRenderEveryTick
{
	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x00147519 File Offset: 0x00145719
	public bool HasAnyDupeDied
	{
		get
		{
			return this.deadDupeCounter > 0;
		}
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06003AA5 RID: 15013 RVA: 0x00147524 File Offset: 0x00145724
	// (set) Token: 0x06003AA6 RID: 15014 RVA: 0x00147531 File Offset: 0x00145731
	public bool GeothermalFacilityDiscovered
	{
		get
		{
			return (this.geothermalProgress & 1) == 1;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 1;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -2;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06003AA7 RID: 15015 RVA: 0x00147560 File Offset: 0x00145760
	// (set) Token: 0x06003AA8 RID: 15016 RVA: 0x0014756D File Offset: 0x0014576D
	public bool GeothermalControllerRepaired
	{
		get
		{
			return (this.geothermalProgress & 2) == 2;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 2;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -3;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x0014759C File Offset: 0x0014579C
	// (set) Token: 0x06003AAA RID: 15018 RVA: 0x001475A9 File Offset: 0x001457A9
	public bool GeothermalControllerHasVented
	{
		get
		{
			return (this.geothermalProgress & 4) == 4;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 4;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -5;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06003AAB RID: 15019 RVA: 0x001475D8 File Offset: 0x001457D8
	// (set) Token: 0x06003AAC RID: 15020 RVA: 0x001475E5 File Offset: 0x001457E5
	public bool GeothermalClearedEntombedVent
	{
		get
		{
			return (this.geothermalProgress & 8) == 8;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 8;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -9;
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06003AAD RID: 15021 RVA: 0x00147614 File Offset: 0x00145814
	// (set) Token: 0x06003AAE RID: 15022 RVA: 0x00147623 File Offset: 0x00145823
	public bool GeothermalVictoryPopupDismissed
	{
		get
		{
			return (this.geothermalProgress & 16) == 16;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 16;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -17;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06003AAF RID: 15023 RVA: 0x00147653 File Offset: 0x00145853
	public List<string> achievementsToDisplay
	{
		get
		{
			return this.completedAchievementsToDisplay;
		}
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x0014765B File Offset: 0x0014585B
	public void ClearDisplayAchievements()
	{
		this.achievementsToDisplay.Clear();
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x00147668 File Offset: 0x00145868
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (!this.achievements.ContainsKey(colonyAchievement.Id))
			{
				ColonyAchievementStatus value = new ColonyAchievementStatus(colonyAchievement.Id);
				this.achievements.Add(colonyAchievement.Id, value);
			}
		}
		this.forceCheckAchievementHandle = Game.Instance.Subscribe(395452326, new Action<object>(this.CheckAchievements));
		base.Subscribe<ColonyAchievementTracker>(631075836, ColonyAchievementTracker.OnNewDayDelegate);
		this.UpgradeTamedCritterAchievements();
	}

	// Token: 0x06003AB2 RID: 15026 RVA: 0x0014772C File Offset: 0x0014592C
	private void UpgradeTamedCritterAchievements()
	{
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in Db.Get().ColonyAchievements.TameAllBasicCritters.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits = colonyAchievementRequirement as CritterTypesWithTraits;
			if (critterTypesWithTraits != null)
			{
				critterTypesWithTraits.UpdateSavedState();
			}
		}
		foreach (ColonyAchievementRequirement colonyAchievementRequirement2 in Db.Get().ColonyAchievements.TameAGassyMoo.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits2 = colonyAchievementRequirement2 as CritterTypesWithTraits;
			if (critterTypesWithTraits2 != null)
			{
				critterTypesWithTraits2.UpdateSavedState();
			}
		}
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x001477EC File Offset: 0x001459EC
	public void RenderEveryTick(float dt)
	{
		if (this.updatingAchievement >= this.achievements.Count)
		{
			this.updatingAchievement = 0;
		}
		KeyValuePair<string, ColonyAchievementStatus> keyValuePair = this.achievements.ElementAt(this.updatingAchievement);
		this.updatingAchievement++;
		if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
		{
			keyValuePair.Value.UpdateAchievement();
			if (keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
				this.completedAchievementsToDisplay.Add(keyValuePair.Key);
				this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				RetireColonyUtility.SaveColonySummaryData();
			}
		}
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x001478AC File Offset: 0x00145AAC
	private void CheckAchievements(object data = null)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				keyValuePair.Value.UpdateAchievement();
				if (keyValuePair.Value.success && !keyValuePair.Value.failed)
				{
					ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
					this.completedAchievementsToDisplay.Add(keyValuePair.Key);
					this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				}
			}
		}
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x00147974 File Offset: 0x00145B74
	private static void UnlockPlatformAchievement(string achievement_id)
	{
		if (DebugHandler.InstantBuildMode)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: instant build mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (SaveGame.Instance.sandboxEnabled)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: sandbox mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (Game.Instance.debugWasUsed)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: debug was used.", new object[]
			{
				achievement_id
			});
			return;
		}
		ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.Get(achievement_id);
		if (colonyAchievement != null && !string.IsNullOrEmpty(colonyAchievement.platformAchievementId))
		{
			if (SteamAchievementService.Instance)
			{
				SteamAchievementService.Instance.Unlock(colonyAchievement.platformAchievementId);
				return;
			}
			global::Debug.LogWarningFormat("Steam achievement [{0}] was achieved, but achievement service was null", new object[]
			{
				colonyAchievement.platformAchievementId
			});
		}
	}

	// Token: 0x06003AB6 RID: 15030 RVA: 0x00147A36 File Offset: 0x00145C36
	public void DebugTriggerAchievement(string id)
	{
		this.achievements[id].failed = false;
		this.achievements[id].success = true;
	}

	// Token: 0x06003AB7 RID: 15031 RVA: 0x00147A5C File Offset: 0x00145C5C
	private void BeginVictorySequence(string achievementID)
	{
		RootMenu.Instance.canTogglePauseScreen = false;
		CameraController.Instance.DisableUserCameraControl = true;
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
		this.ToggleVictoryUI(true);
		StoryMessageScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.StoryMessageScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<StoryMessageScreen>();
		component.restoreInterfaceOnClose = false;
		component.title = COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_HEADER;
		component.body = string.Format(COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_BODY, "<b>" + Db.Get().ColonyAchievements.Get(achievementID).Name + "</b>\n" + Db.Get().ColonyAchievements.Get(achievementID).description);
		component.Show(true);
		CameraController.Instance.SetWorldInteractive(false);
		component.OnClose = (System.Action)Delegate.Combine(component.OnClose, new System.Action(delegate()
		{
			SpeedControlScreen.Instance.SetSpeed(1);
			if (!SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			CameraController.Instance.SetWorldInteractive(true);
			Db.Get().ColonyAchievements.Get(achievementID).victorySequence(this);
		}));
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x00147BA0 File Offset: 0x00145DA0
	public bool IsAchievementUnlocked(ColonyAchievement achievement)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (keyValuePair.Key == achievement.Id)
			{
				if (keyValuePair.Value.success)
				{
					return true;
				}
				keyValuePair.Value.UpdateAchievement();
				return keyValuePair.Value.success;
			}
		}
		return false;
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x00147C30 File Offset: 0x00145E30
	protected override void OnCleanUp()
	{
		this.victorySchedulerHandle.ClearScheduler();
		Game.Instance.Unsubscribe(this.forceCheckAchievementHandle);
		this.checkAchievementsHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x00147C60 File Offset: 0x00145E60
	private void TriggerNewAchievementCompleted(string achievement, GameObject cameraTarget = null)
	{
		this.unlockedAchievementMetric[ColonyAchievementTracker.UnlockedAchievementKey] = achievement;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedAchievementMetric, "TriggerNewAchievementCompleted");
		bool flag = false;
		if (Db.Get().ColonyAchievements.Get(achievement).isVictoryCondition)
		{
			flag = true;
			this.BeginVictorySequence(achievement);
		}
		if (!flag)
		{
			AchievementEarnedMessage message = new AchievementEarnedMessage();
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x00147CCC File Offset: 0x00145ECC
	private void ToggleVictoryUI(bool victoryUIActive)
	{
		List<KScreen> list = new List<KScreen>();
		list.Add(NotificationScreen.Instance);
		list.Add(OverlayMenu.Instance);
		if (PlanScreen.Instance != null)
		{
			list.Add(PlanScreen.Instance);
		}
		if (BuildMenu.Instance != null)
		{
			list.Add(BuildMenu.Instance);
		}
		list.Add(ManagementMenu.Instance);
		list.Add(ToolMenu.Instance);
		list.Add(ToolMenu.Instance.PriorityScreen);
		list.Add(ResourceCategoryScreen.Instance);
		list.Add(TopLeftControlScreen.Instance);
		list.Add(global::DateTime.Instance);
		list.Add(BuildWatermark.Instance);
		list.Add(HoverTextScreen.Instance);
		list.Add(DetailsScreen.Instance);
		list.Add(DebugPaintElementScreen.Instance);
		list.Add(DebugBaseTemplateButton.Instance);
		list.Add(StarmapScreen.Instance);
		foreach (KScreen kscreen in list)
		{
			if (kscreen != null)
			{
				kscreen.Show(!victoryUIActive);
			}
		}
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x00147DFC File Offset: 0x00145FFC
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.achievements.Count);
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			writer.WriteKleiString(keyValuePair.Key);
			keyValuePair.Value.Serialize(writer);
		}
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x00147E74 File Offset: 0x00146074
	public void Deserialize(IReader reader)
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 10))
		{
			return;
		}
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = reader.ReadKleiString();
			ColonyAchievementStatus value = ColonyAchievementStatus.Deserialize(reader, text);
			if (Db.Get().ColonyAchievements.Exists(text))
			{
				this.achievements.Add(text, value);
			}
		}
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x00147EDC File Offset: 0x001460DC
	public void LogFetchChore(GameObject fetcher, ChoreType choreType)
	{
		if (choreType == Db.Get().ChoreTypes.StorageFetch || choreType == Db.Get().ChoreTypes.BuildFetch || choreType == Db.Get().ChoreTypes.RepairFetch || choreType == Db.Get().ChoreTypes.FoodFetch || choreType == Db.Get().ChoreTypes.Transport)
		{
			return;
		}
		Dictionary<int, int> dictionary = null;
		if (fetcher.GetComponent<SolidTransferArm>() != null)
		{
			dictionary = this.fetchAutomatedChoreDeliveries;
		}
		else if (fetcher.GetComponent<MinionIdentity>() != null)
		{
			dictionary = this.fetchDupeChoreDeliveries;
		}
		if (dictionary != null)
		{
			int cycle = GameClock.Instance.GetCycle();
			if (!dictionary.ContainsKey(cycle))
			{
				dictionary.Add(cycle, 0);
			}
			Dictionary<int, int> dictionary2 = dictionary;
			int key = cycle;
			int num = dictionary2[key];
			dictionary2[key] = num + 1;
		}
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x00147FA5 File Offset: 0x001461A5
	public void LogCritterTamed(Tag prefabId)
	{
		this.tamedCritterTypes.Add(prefabId);
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x00147FB4 File Offset: 0x001461B4
	public void LogSuitChore(ChoreDriver driver)
	{
		if (driver == null || driver.GetComponent<MinionIdentity>() == null)
		{
			return;
		}
		bool flag = false;
		foreach (AssignableSlotInstance assignableSlotInstance in driver.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			Equippable equippable = ((EquipmentSlotInstance)assignableSlotInstance).assignable as Equippable;
			if (equippable && equippable.GetComponent<KPrefabID>().IsAnyPrefabID(ColonyAchievementTracker.SuitTags))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			int cycle = GameClock.Instance.GetCycle();
			int instanceID = driver.GetComponent<KPrefabID>().InstanceID;
			if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
			{
				this.dupesCompleteChoresInSuits.Add(cycle, new List<int>
				{
					instanceID
				});
				return;
			}
			if (!this.dupesCompleteChoresInSuits[cycle].Contains(instanceID))
			{
				this.dupesCompleteChoresInSuits[cycle].Add(instanceID);
			}
		}
	}

	// Token: 0x06003AC1 RID: 15041 RVA: 0x001480BC File Offset: 0x001462BC
	public void LogAnalyzedSeed(Tag seed)
	{
		this.analyzedSeeds.Add(seed);
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x001480CC File Offset: 0x001462CC
	public void OnNewDay(object data)
	{
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			if (minionStorage.GetComponent<CommandModule>() != null)
			{
				List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
				if (storedMinionInfo.Count > 0)
				{
					int cycle = GameClock.Instance.GetCycle();
					if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
					{
						this.dupesCompleteChoresInSuits.Add(cycle, new List<int>());
					}
					for (int i = 0; i < storedMinionInfo.Count; i++)
					{
						KPrefabID kprefabID = storedMinionInfo[i].serializedMinion.Get();
						if (kprefabID != null)
						{
							this.dupesCompleteChoresInSuits[cycle].Add(kprefabID.InstanceID);
						}
					}
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			SurviveARocketWithMinimumMorale surviveARocketWithMinimumMorale = Db.Get().ColonyAchievements.SurviveInARocket.requirementChecklist[0] as SurviveARocketWithMinimumMorale;
			if (surviveARocketWithMinimumMorale != null)
			{
				float minimumMorale = surviveARocketWithMinimumMorale.minimumMorale;
				int numberOfCycles = surviveARocketWithMinimumMorale.numberOfCycles;
				foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
				{
					if (worldContainer.IsModuleInterior)
					{
						if (!this.cyclesRocketDupeMoraleAboveRequirement.ContainsKey(worldContainer.id))
						{
							this.cyclesRocketDupeMoraleAboveRequirement.Add(worldContainer.id, 0);
						}
						if (worldContainer.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
						{
							List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(worldContainer.id, false);
							bool flag = worldItems.Count > 0;
							foreach (MinionIdentity cmp in worldItems)
							{
								if (Db.Get().Attributes.QualityOfLife.Lookup(cmp).GetTotalValue() < minimumMorale)
								{
									flag = false;
									break;
								}
							}
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = (flag ? (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] + 1) : 0);
						}
						else if (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] < numberOfCycles)
						{
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = 0;
						}
					}
				}
			}
		}
	}

	// Token: 0x0400239F RID: 9119
	public Dictionary<string, ColonyAchievementStatus> achievements = new Dictionary<string, ColonyAchievementStatus>();

	// Token: 0x040023A0 RID: 9120
	[Serialize]
	public Dictionary<int, int> fetchAutomatedChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x040023A1 RID: 9121
	[Serialize]
	public Dictionary<int, int> fetchDupeChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x040023A2 RID: 9122
	[Serialize]
	public Dictionary<int, List<int>> dupesCompleteChoresInSuits = new Dictionary<int, List<int>>();

	// Token: 0x040023A3 RID: 9123
	[Serialize]
	public HashSet<Tag> tamedCritterTypes = new HashSet<Tag>();

	// Token: 0x040023A4 RID: 9124
	[Serialize]
	public bool defrostedDuplicant;

	// Token: 0x040023A5 RID: 9125
	[Serialize]
	public HashSet<Tag> analyzedSeeds = new HashSet<Tag>();

	// Token: 0x040023A6 RID: 9126
	[Serialize]
	public float totalMaterialsHarvestFromPOI;

	// Token: 0x040023A7 RID: 9127
	[Serialize]
	public float radBoltTravelDistance;

	// Token: 0x040023A8 RID: 9128
	[Serialize]
	public bool harvestAHiveWithoutGettingStung;

	// Token: 0x040023A9 RID: 9129
	[Serialize]
	public Dictionary<int, int> cyclesRocketDupeMoraleAboveRequirement = new Dictionary<int, int>();

	// Token: 0x040023AA RID: 9130
	[Serialize]
	public bool efficientlyGatheredData;

	// Token: 0x040023AB RID: 9131
	[Serialize]
	public bool fullyBoostedBionic;

	// Token: 0x040023AC RID: 9132
	[Serialize]
	public int deadDupeCounter;

	// Token: 0x040023AD RID: 9133
	[Serialize]
	private int geothermalProgress;

	// Token: 0x040023AE RID: 9134
	[Serialize]
	public ColonyAchievementTracker.LargeImpactorState largeImpactorState;

	// Token: 0x040023AF RID: 9135
	[Serialize]
	public float LargeImpactorBackgroundScale = 0.6f;

	// Token: 0x040023B0 RID: 9136
	[Serialize]
	public int largeImpactorLandedCycle = -1;

	// Token: 0x040023B1 RID: 9137
	private const int GEO_DISCOVERED_BIT = 1;

	// Token: 0x040023B2 RID: 9138
	private const int GEO_CONTROLLER_REPAIRED_BIT = 2;

	// Token: 0x040023B3 RID: 9139
	private const int GEO_CONTROLLER_VENTED_BIT = 4;

	// Token: 0x040023B4 RID: 9140
	private const int GEO_CLEARED_ENTOMBED_BIT = 8;

	// Token: 0x040023B5 RID: 9141
	private const int GEO_VICTORY_ACK_BIT = 16;

	// Token: 0x040023B6 RID: 9142
	private SchedulerHandle checkAchievementsHandle;

	// Token: 0x040023B7 RID: 9143
	private int forceCheckAchievementHandle = -1;

	// Token: 0x040023B8 RID: 9144
	[Serialize]
	private int updatingAchievement;

	// Token: 0x040023B9 RID: 9145
	[Serialize]
	private List<string> completedAchievementsToDisplay = new List<string>();

	// Token: 0x040023BA RID: 9146
	private SchedulerHandle victorySchedulerHandle;

	// Token: 0x040023BB RID: 9147
	public static readonly string UnlockedAchievementKey = "UnlockedAchievement";

	// Token: 0x040023BC RID: 9148
	private Dictionary<string, object> unlockedAchievementMetric = new Dictionary<string, object>
	{
		{
			ColonyAchievementTracker.UnlockedAchievementKey,
			null
		}
	};

	// Token: 0x040023BD RID: 9149
	private static readonly Tag[] SuitTags = new Tag[]
	{
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	// Token: 0x040023BE RID: 9150
	private static readonly EventSystem.IntraObjectHandler<ColonyAchievementTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<ColonyAchievementTracker>(delegate(ColonyAchievementTracker component, object data)
	{
		component.OnNewDay(data);
	});

	// Token: 0x02001808 RID: 6152
	public enum LargeImpactorState
	{
		// Token: 0x04007978 RID: 31096
		Alive,
		// Token: 0x04007979 RID: 31097
		Defeated,
		// Token: 0x0400797A RID: 31098
		Landed
	}
}
