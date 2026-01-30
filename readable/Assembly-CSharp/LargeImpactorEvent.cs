using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000B84 RID: 2948
public class LargeImpactorEvent : GameplayEvent<LargeImpactorEvent.StatesInstance>
{
	// Token: 0x060057DC RID: 22492 RVA: 0x001FF4FD File Offset: 0x001FD6FD
	public LargeImpactorEvent(string id, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, 0, 0, requiredDlcIds, forbiddenDlcIds)
	{
	}

	// Token: 0x060057DD RID: 22493 RVA: 0x001FF50A File Offset: 0x001FD70A
	public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
	{
		return new LargeImpactorEvent.StatesInstance(manager, eventInstance, this);
	}

	// Token: 0x060057DE RID: 22494 RVA: 0x001FF514 File Offset: 0x001FD714
	private static void SpawnIridiumShowers(LargeImpactorEvent.StatesInstance smi)
	{
		GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.IridiumShowerEvent, smi.eventInstance.worldId, null);
	}

	// Token: 0x060057DF RID: 22495 RVA: 0x001FF53C File Offset: 0x001FD73C
	private static void PreventDemoliorFragmentsBGFromPlaying(LargeImpactorEvent.StatesInstance smi)
	{
		TerrainBG.preventLargeImpactorFragmentsFromProgressing = true;
	}

	// Token: 0x060057E0 RID: 22496 RVA: 0x001FF544 File Offset: 0x001FD744
	private static void AllowDemoliorFragmentsBGFromPlaying(LargeImpactorEvent.StatesInstance smi)
	{
		TerrainBG.preventLargeImpactorFragmentsFromProgressing = false;
	}

	// Token: 0x060057E1 RID: 22497 RVA: 0x001FF54C File Offset: 0x001FD74C
	private static void DestroyEventInstance(LargeImpactorEvent.StatesInstance smi)
	{
		smi.eventInstance.smi.StopSM("end");
	}

	// Token: 0x060057E2 RID: 22498 RVA: 0x001FF563 File Offset: 0x001FD763
	private static bool WasWinAchievementAlreadyGranted(LargeImpactorEvent.StatesInstance smi)
	{
		return SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(Db.Get().ColonyAchievements.AsteroidDestroyed);
	}

	// Token: 0x060057E3 RID: 22499 RVA: 0x001FF583 File Offset: 0x001FD783
	private static void UnlockWinAchievement(LargeImpactorEvent.StatesInstance smi)
	{
		SaveGame.Instance.ColonyAchievementTracker.largeImpactorState = ColonyAchievementTracker.LargeImpactorState.Defeated;
	}

	// Token: 0x060057E4 RID: 22500 RVA: 0x001FF598 File Offset: 0x001FD798
	private static void RegisterDemoliorSize(LargeImpactorEvent.StatesInstance smi)
	{
		ParallaxBackgroundObject component = smi.impactorInstance.GetComponent<ParallaxBackgroundObject>();
		SaveGame.Instance.ColonyAchievementTracker.LargeImpactorBackgroundScale = component.lastScaleUsed;
	}

	// Token: 0x060057E5 RID: 22501 RVA: 0x001FF5C6 File Offset: 0x001FD7C6
	private static void RegisterLandedCycle(LargeImpactorEvent.StatesInstance smi)
	{
		SaveGame.Instance.ColonyAchievementTracker.largeImpactorState = ColonyAchievementTracker.LargeImpactorState.Landed;
		SaveGame.Instance.ColonyAchievementTracker.largeImpactorLandedCycle = GameClock.Instance.GetCycle();
	}

	// Token: 0x060057E6 RID: 22502 RVA: 0x001FF5F4 File Offset: 0x001FD7F4
	private static bool IsSuitablePOISpawnLocation(AxialI location)
	{
		if (!ClusterGrid.Instance.IsValidCell(location))
		{
			return false;
		}
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesOnCell(location))
		{
			if (clusterGridEntity.Layer == EntityLayer.Asteroid || clusterGridEntity.Layer == EntityLayer.POI)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x001FF66C File Offset: 0x001FD86C
	private static List<AxialI> FindAvailablePOISpawnLocations(AxialI location)
	{
		List<AxialI> list = new List<AxialI>();
		if (LargeImpactorEvent.IsSuitablePOISpawnLocation(location))
		{
			list.Add(location);
		}
		for (int i = 1; i <= 2; i++)
		{
			foreach (AxialI v in AxialI.DIRECTIONS)
			{
				AxialI axialI = location + v * i;
				if (LargeImpactorEvent.IsSuitablePOISpawnLocation(axialI))
				{
					list.Add(axialI);
				}
			}
		}
		return list;
	}

	// Token: 0x060057E8 RID: 22504 RVA: 0x001FF6FC File Offset: 0x001FD8FC
	private static void SpawnPOI(string id, AxialI location)
	{
		GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab(id), null, null);
		gameObject.GetComponent<HarvestablePOIClusterGridEntity>().Init(location);
		gameObject.SetActive(true);
	}

	// Token: 0x060057E9 RID: 22505 RVA: 0x001FF724 File Offset: 0x001FD924
	private static void HandleInterception(LargeImpactorEvent.StatesInstance smi)
	{
		if (DlcManager.IsExpansion1Active())
		{
			List<AxialI> list = LargeImpactorEvent.FindAvailablePOISpawnLocations(smi.impactorInstance.GetSMI<ClusterMapLargeImpactor.Instance>().ClusterGridPosition());
			if (list.Count > 0)
			{
				LargeImpactorEvent.SpawnPOI("HarvestableSpacePOI_DLC4ImpactorDebrisField1", list[0]);
			}
			if (list.Count > 1)
			{
				LargeImpactorEvent.SpawnPOI("HarvestableSpacePOI_DLC4ImpactorDebrisField2", list[1]);
			}
			if (list.Count > 2)
			{
				LargeImpactorEvent.SpawnPOI("HarvestableSpacePOI_DLC4ImpactorDebrisField3", list[2]);
			}
		}
		else
		{
			if (!SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination.Id, SpacecraftManager.DestinationLocationSelectionType.Nearest, 0, 2147483647, 3))
			{
				SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination.Id, SpacecraftManager.DestinationLocationSelectionType.Random, 0, int.MaxValue, 5);
			}
			if (!SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination2.Id, SpacecraftManager.DestinationLocationSelectionType.Random, 1, 5, 5))
			{
				SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination2.Id, SpacecraftManager.DestinationLocationSelectionType.Random, 0, int.MaxValue, 5);
			}
			if (!SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination3.Id, SpacecraftManager.DestinationLocationSelectionType.Random, 1, 5, 5))
			{
				SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination3.Id, SpacecraftManager.DestinationLocationSelectionType.Random, 0, int.MaxValue, 5);
			}
		}
		smi.GoTo(smi.sm.finished);
	}

	// Token: 0x060057EA RID: 22506 RVA: 0x001FF89A File Offset: 0x001FDA9A
	private static bool WasKilled(LargeImpactorEvent.StatesInstance smi, object _)
	{
		return smi.impactorInstance.GetSMI<LargeImpactorStatus.Instance>().Health <= 0;
	}

	// Token: 0x060057EB RID: 22507 RVA: 0x001FF8B2 File Offset: 0x001FDAB2
	private static void PrepareForLargeImpactorDefeatedSequence(LargeImpactorEvent.StatesInstance smi)
	{
		smi.impactorInstance.GetComponent<LargeImpactorCrashStamp>();
		LargeImpactorEvent.ToggleOffLandingZoneVisualizer(smi);
		ClusterManager.Instance.GetWorld(smi.eventInstance.worldId).RevealSurface();
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x001FF8E0 File Offset: 0x001FDAE0
	private static void InitializeLandingSequence(LargeImpactorEvent.StatesInstance smi)
	{
		GameObject impactorInstance = smi.impactorInstance;
		LargeImpactorCrashStamp component = impactorInstance.GetComponent<LargeImpactorCrashStamp>();
		ParallaxBackgroundObject component2 = impactorInstance.GetComponent<ParallaxBackgroundObject>();
		LargeImpactorEvent.ToggleOffLandingZoneVisualizer(smi);
		WorldContainer world = ClusterManager.Instance.GetWorld(smi.eventInstance.worldId);
		world.RevealHiddenY();
		world.RevealSurface();
		component.RevealFogOfWar(7);
		component2.SetVisibilityState(false);
		LargeComet comet = LargeImpactorEvent.CreateLargeImpactorInWorldFallingAsteroid(smi, component, world);
		LargeImpactorLandingSequence.Start(component, comet, component, world.id);
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x001FF94C File Offset: 0x001FDB4C
	private static void ToggleOffLandingZoneVisualizer(LargeImpactorEvent.StatesInstance smi)
	{
		LargeImpactorVisualizer component = smi.impactorInstance.GetComponent<LargeImpactorVisualizer>();
		if (component.Active)
		{
			component.Active = false;
		}
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x001FF974 File Offset: 0x001FDB74
	private static LargeComet CreateLargeImpactorInWorldFallingAsteroid(LargeImpactorEvent.StatesInstance smi, LargeImpactorCrashStamp crashStamp, WorldContainer world)
	{
		TemplateContainer asteroidTemplate = crashStamp.asteroidTemplate;
		Vector2I stampLocation = crashStamp.stampLocation;
		float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
		Vector3 position = new Vector3((float)stampLocation.X, (float)(world.Height - world.HiddenYOffset - 1), layerZ);
		GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab(LargeImpactorCometConfig.ID), position, Quaternion.identity, null, null, true, 0);
		LargeComet component = gameObject.GetComponent<LargeComet>();
		gameObject.SetActive(true);
		component.stampLocation = stampLocation;
		component.crashPosition = stampLocation;
		LargeComet largeComet = component;
		largeComet.crashPosition.y = largeComet.crashPosition.y + asteroidTemplate.GetTemplateBounds(0).yMin;
		component.asteroidTemplate = asteroidTemplate;
		component.bottomCellsOffsetOfTemplate = crashStamp.TemplateBottomCellsOffsets;
		return component;
	}

	// Token: 0x060057EF RID: 22511 RVA: 0x001FFA2C File Offset: 0x001FDC2C
	private static GameObject CreateSpacedOutImpactorInstance(LargeImpactorEvent.StatesInstance smi)
	{
		if (!DlcManager.IsExpansion1Active() || ClusterGrid.Instance == null)
		{
			return null;
		}
		GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("LargeImpactor"), null, null);
		float arrivalTime = smi.eventInstance.eventStartTime * 600f + LargeImpactorEvent.GetImpactTime();
		AxialI location = ClusterManager.Instance.GetClusterPOIManager().GetTemporalTear().Location;
		ClusterMapMeteorShowerVisualizer component = gameObject.GetComponent<ClusterMapMeteorShowerVisualizer>();
		component.SetInitialLocation(location);
		component.forceRevealed = true;
		ClusterMapLargeImpactor.Def def = gameObject.AddOrGetDef<ClusterMapLargeImpactor.Def>();
		def.destinationWorldID = 0;
		def.arrivalTime = arrivalTime;
		gameObject.AddOrGet<ParallaxBackgroundObject>().worldId = new int?(smi.eventInstance.worldId);
		return gameObject;
	}

	// Token: 0x060057F0 RID: 22512 RVA: 0x001FFACD File Offset: 0x001FDCCD
	private static GameObject CreateVanillaImpactorInstance(LargeImpactorEvent.StatesInstance smi)
	{
		if (DlcManager.IsExpansion1Active())
		{
			return null;
		}
		return global::Util.KInstantiate(Assets.GetPrefab(LargeImpactorVanillaConfig.ID), null, null);
	}

	// Token: 0x060057F1 RID: 22513 RVA: 0x001FFAF0 File Offset: 0x001FDCF0
	public static void CreateImpactorInstance(LargeImpactorEvent.StatesInstance smi)
	{
		GameObject gameObject;
		if (DlcManager.IsExpansion1Active())
		{
			gameObject = LargeImpactorEvent.CreateSpacedOutImpactorInstance(smi);
		}
		else
		{
			gameObject = LargeImpactorEvent.CreateVanillaImpactorInstance(smi);
		}
		if (gameObject == null)
		{
			KCrashReporter.ReportDevNotification("Failed to create LargeImpactor Object.", Environment.StackTrace, "", false, null);
			smi.StopSM("No Impactor created");
			return;
		}
		gameObject.SetActive(true);
		smi.sm.impactorTarget.Set(gameObject.GetComponent<KPrefabID>(), smi);
	}

	// Token: 0x060057F2 RID: 22514 RVA: 0x001FFB60 File Offset: 0x001FDD60
	public static float GetImpactTime()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		if (currentClusterLayout != null && currentClusterLayout.clusterTags.Contains("DemoliorImminentImpact"))
		{
			return 6000f;
		}
		float num = 200f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.DemoliorDifficulty);
		if (currentQualitySetting.id == "VeryHard")
		{
			num = 100f;
		}
		else if (currentQualitySetting.id == "Hard")
		{
			num = 150f;
		}
		else if (currentQualitySetting.id == "Easy")
		{
			num = 300f;
		}
		else if (currentQualitySetting.id == "VeryEasy")
		{
			num = 500f;
		}
		return num * 600f;
	}

	// Token: 0x02001D06 RID: 7430
	public class States : GameplayEventStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, LargeImpactorEvent>
	{
		// Token: 0x0600AFAC RID: 44972 RVA: 0x003D6D2C File Offset: 0x003D4F2C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			base.InitializeStates(out default_state);
			default_state = this.start;
			this.start.ParamTransition<GameObject>(this.impactorTarget, this.create, GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.IsNull).ParamTransition<GameObject>(this.impactorTarget, this.clusterMap, GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.IsNotNull);
			this.create.ParamTransition<GameObject>(this.impactorTarget, this.clusterMap, GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.IsNotNull).Enter(delegate(LargeImpactorEvent.StatesInstance smi)
			{
				LargeImpactorEvent.CreateImpactorInstance(smi);
			});
			this.clusterMap.Target(this.impactorTarget).EventTransition(GameHashes.LargeImpactorArrived, this.impacting, null).EventTransition(GameHashes.Died, this.killedByPlayer, null);
			this.impacting.EnterTransition(this.finished, new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback(LargeImpactorEvent.WasWinAchievementAlreadyGranted)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.RegisterLandedCycle)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.InitializeLandingSequence)).Target(this.impactorTarget).EventTransition(GameHashes.SequenceCompleted, this.finished, null);
			this.killedByPlayer.EnterTransition(this.finished, new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback(LargeImpactorEvent.WasWinAchievementAlreadyGranted)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.PrepareForLargeImpactorDefeatedSequence)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.PreventDemoliorFragmentsBGFromPlaying)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.UnlockWinAchievement)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.RegisterDemoliorSize)).Exit(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.AllowDemoliorFragmentsBGFromPlaying)).Exit(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.SpawnIridiumShowers)).Target(this.impactorTarget).EventHandler(GameHashes.SequenceCompleted, new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.HandleInterception));
			this.finished.Enter(delegate(LargeImpactorEvent.StatesInstance smi)
			{
				global::Util.KDestroyGameObject(smi.sm.impactorTarget.Get(smi));
			}).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.DestroyEventInstance)).GoTo(null);
		}

		// Token: 0x04008A14 RID: 35348
		public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State start;

		// Token: 0x04008A15 RID: 35349
		public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State create;

		// Token: 0x04008A16 RID: 35350
		public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State clusterMap;

		// Token: 0x04008A17 RID: 35351
		public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State killedByPlayer;

		// Token: 0x04008A18 RID: 35352
		public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State impacting;

		// Token: 0x04008A19 RID: 35353
		public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State finished;

		// Token: 0x04008A1A RID: 35354
		[Serialize]
		public StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.TargetParameter impactorTarget = new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.TargetParameter();
	}

	// Token: 0x02001D07 RID: 7431
	public class StatesInstance : GameplayEventStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, LargeImpactorEvent>.GameplayEventStateMachineInstance
	{
		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x0600AFAE RID: 44974 RVA: 0x003D6F4D File Offset: 0x003D514D
		public GameObject impactorInstance
		{
			get
			{
				return base.sm.impactorTarget.Get(base.smi);
			}
		}

		// Token: 0x0600AFAF RID: 44975 RVA: 0x003D6F65 File Offset: 0x003D5165
		public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, LargeImpactorEvent largeImpactorEvent) : base(master, eventInstance, largeImpactorEvent)
		{
		}
	}
}
