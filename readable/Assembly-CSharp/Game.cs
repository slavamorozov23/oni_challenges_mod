using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using FMOD.Studio;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Token: 0x0200095D RID: 2397
[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour
{
	// Token: 0x060042F9 RID: 17145 RVA: 0x0017A8AE File Offset: 0x00178AAE
	public static bool IsOnMainThread()
	{
		return Game.MainThread == Thread.CurrentThread;
	}

	// Token: 0x060042FA RID: 17146 RVA: 0x0017A8BC File Offset: 0x00178ABC
	public static bool IsQuitting()
	{
		return Game.quitting;
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x060042FB RID: 17147 RVA: 0x0017A8C3 File Offset: 0x00178AC3
	// (set) Token: 0x060042FC RID: 17148 RVA: 0x0017A8CB File Offset: 0x00178ACB
	public KInputHandler inputHandler { get; set; }

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x060042FD RID: 17149 RVA: 0x0017A8D4 File Offset: 0x00178AD4
	// (set) Token: 0x060042FE RID: 17150 RVA: 0x0017A8DB File Offset: 0x00178ADB
	public static Game Instance { get; private set; }

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x060042FF RID: 17151 RVA: 0x0017A8E3 File Offset: 0x00178AE3
	public static Camera MainCamera
	{
		get
		{
			if (Game.m_CachedCamera == null)
			{
				Game.m_CachedCamera = Camera.main;
			}
			return Game.m_CachedCamera;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06004300 RID: 17152 RVA: 0x0017A901 File Offset: 0x00178B01
	// (set) Token: 0x06004301 RID: 17153 RVA: 0x0017A924 File Offset: 0x00178B24
	public bool SaveToCloudActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, value2);
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06004302 RID: 17154 RVA: 0x0017A951 File Offset: 0x00178B51
	// (set) Token: 0x06004303 RID: 17155 RVA: 0x0017A974 File Offset: 0x00178B74
	public bool FastWorkersModeActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.FastWorkersMode).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.FastWorkersMode, value2);
		}
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06004304 RID: 17156 RVA: 0x0017A9A1 File Offset: 0x00178BA1
	// (set) Token: 0x06004305 RID: 17157 RVA: 0x0017A9AC File Offset: 0x00178BAC
	public bool SandboxModeActive
	{
		get
		{
			return this.sandboxModeActive;
		}
		set
		{
			this.sandboxModeActive = value;
			base.Trigger(-1948169901, null);
			if (PlanScreen.Instance != null)
			{
				PlanScreen.Instance.Refresh();
			}
			if (BuildMenu.Instance != null)
			{
				BuildMenu.Instance.Refresh();
			}
			if (OverlayMenu.Instance != null)
			{
				OverlayMenu.Instance.Refresh();
			}
			if (ManagementMenu.Instance != null)
			{
				ManagementMenu.Instance.Refresh();
			}
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06004306 RID: 17158 RVA: 0x0017AA28 File Offset: 0x00178C28
	public bool DebugOnlyBuildingsAllowed
	{
		get
		{
			return DebugHandler.enabled && (this.SandboxModeActive || DebugHandler.InstantBuildMode);
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06004307 RID: 17159 RVA: 0x0017AA42 File Offset: 0x00178C42
	// (set) Token: 0x06004308 RID: 17160 RVA: 0x0017AA4A File Offset: 0x00178C4A
	public StatusItemRenderer statusItemRenderer { get; private set; }

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06004309 RID: 17161 RVA: 0x0017AA53 File Offset: 0x00178C53
	// (set) Token: 0x0600430A RID: 17162 RVA: 0x0017AA5B File Offset: 0x00178C5B
	public PrioritizableRenderer prioritizableRenderer { get; private set; }

	// Token: 0x0600430B RID: 17163 RVA: 0x0017AA64 File Offset: 0x00178C64
	protected override void OnPrefabInit()
	{
		UnityEngine.Debug.unityLogger.logHandler = new LogCatcher(UnityEngine.Debug.unityLogger.logHandler);
		DebugUtil.LogArgs(new object[]
		{
			Time.realtimeSinceStartup,
			"Level Loaded....",
			SceneManager.GetActiveScene().name
		});
		Components.EntityCellVisualizers.OnAdd += this.OnAddBuildingCellVisualizer;
		Components.EntityCellVisualizers.OnRemove += this.OnRemoveBuildingCellVisualizer;
		Singleton<KBatchedAnimUpdater>.CreateInstance();
		Singleton<CellChangeMonitor>.CreateInstance();
		this.userMenu = new UserMenu();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.StopBE));
		Game.Instance = this;
		this.statusItemRenderer = new StatusItemRenderer();
		this.prioritizableRenderer = new PrioritizableRenderer();
		this.LoadEventHashes();
		this.savedInfo.InitializeEmptyVariables();
		this.gasFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasConduits) - 0.4f);
		this.liquidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.LiquidConduits) - 0.4f);
		this.solidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.SolidConduitContents) - 0.4f);
		Shader.WarmupAllShaders();
		Db.Get();
		Game.quitting = false;
		Game.PickupableLayer = LayerMask.NameToLayer("Pickupable");
		Game.BlockSelectionLayerMask = LayerMask.GetMask(new string[]
		{
			"BlockSelection"
		});
		this.world = World.Instance;
		KPrefabID.NextUniqueID = KPlayerPrefs.GetInt(Game.NextUniqueIDKey, 0);
		this.circuitManager = new CircuitManager();
		this.energySim = new EnergySim();
		this.gasConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 13);
		this.liquidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 17);
		this.electricalConduitSystem = new UtilityNetworkManager<ElectricalUtilityNetwork, Wire>(Grid.WidthInCells, Grid.HeightInCells, 27);
		this.logicCircuitSystem = new UtilityNetworkManager<LogicCircuitNetwork, LogicWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
		this.logicCircuitManager = new LogicCircuitManager(this.logicCircuitSystem);
		this.travelTubeSystem = new UtilityNetworkTubesManager(Grid.WidthInCells, Grid.HeightInCells, 35);
		this.solidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, SolidConduit>(Grid.WidthInCells, Grid.HeightInCells, 21);
		this.conduitTemperatureManager = new ConduitTemperatureManager();
		this.conduitDiseaseManager = new ConduitDiseaseManager(this.conduitTemperatureManager);
		this.gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, this.gasConduitSystem, 1f, 0.25f);
		this.liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, this.liquidConduitSystem, 10f, 0.75f);
		this.solidConduitFlow = new SolidConduitFlow(Grid.CellCount, this.solidConduitSystem, 0.75f);
		this.gasFlowVisualizer = new ConduitFlowVisualizer(this.gasConduitFlow, this.gasConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundGas, Lighting.Instance.Settings.GasConduit);
		this.liquidFlowVisualizer = new ConduitFlowVisualizer(this.liquidConduitFlow, this.liquidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundLiquid, Lighting.Instance.Settings.LiquidConduit);
		this.solidFlowVisualizer = new SolidConduitFlowVisualizer(this.solidConduitFlow, this.solidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundSolid, Lighting.Instance.Settings.SolidConduit);
		this.accumulators = new Accumulators();
		this.plantElementAbsorbers = new PlantElementAbsorbers();
		this.activeFX = new ushort[Grid.CellCount];
		this.UnsafePrefabInit();
		Shader.SetGlobalVector("_MetalParameters", new Vector4(0f, 0f, 0f, 0f));
		Shader.SetGlobalVector("_WaterParameters", new Vector4(0f, 0f, 0f, 0f));
		this.InitializeFXSpawners();
		PathFinder.Initialize();
		new GameNavGrids(Pathfinding.Instance);
		this.screenMgr = global::Util.KInstantiate(this.screenManagerPrefab, null, null).GetComponent<GameScreenManager>();
		this.roomProber = new RoomProber();
		this.spaceScannerNetworkManager = new SpaceScannerNetworkManager();
		this.fetchManager = base.gameObject.AddComponent<FetchManager>();
		this.ediblesManager = base.gameObject.AddComponent<EdiblesManager>();
		Singleton<CellChangeMonitor>.Instance.SetGridSize(Grid.WidthInCells, Grid.HeightInCells);
		this.unlocks = base.GetComponent<Unlocks>();
		this.changelistsPlayedOn = new List<uint>();
		this.changelistsPlayedOn.Add(706793U);
		this.dateGenerated = System.DateTime.UtcNow.ToString("U", CultureInfo.InvariantCulture);
		AsyncPathProber.CreateInstance(1);
	}

	// Token: 0x0600430C RID: 17164 RVA: 0x0017AEFD File Offset: 0x001790FD
	public void SetGameStarted()
	{
		this.gameStarted = true;
	}

	// Token: 0x0600430D RID: 17165 RVA: 0x0017AF06 File Offset: 0x00179106
	public bool GameStarted()
	{
		return this.gameStarted;
	}

	// Token: 0x0600430E RID: 17166 RVA: 0x0017AF0E File Offset: 0x0017910E
	private IEnumerator SanityCheckBoundsNextFrame()
	{
		yield return null;
		using (List<WorldContainer>.Enumerator enumerator = ClusterManager.Instance.WorldContainers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				WorldContainer worldContainer = enumerator.Current;
				if (worldContainer.IsDiscovered && !worldContainer.IsModuleInterior)
				{
					for (int i = worldContainer.WorldOffset.X; i < worldContainer.WorldOffset.X + worldContainer.WorldSize.X; i++)
					{
						for (int j = 0; j < Grid.TopBorderHeight; j++)
						{
							int num = Grid.XYToCell(i, worldContainer.WorldOffset.Y + worldContainer.WorldSize.Y - j);
							if (Grid.IsSolidCell(num) && Grid.Element[num].id != SimHashes.Unobtanium)
							{
								SimMessages.Dig(num, -1, true);
							}
						}
					}
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x0600430F RID: 17167 RVA: 0x0017AF16 File Offset: 0x00179116
	private void UnsafePrefabInit()
	{
		this.StepTheSim(0f);
		base.StartCoroutine(this.SanityCheckBoundsNextFrame());
	}

	// Token: 0x06004310 RID: 17168 RVA: 0x0017AF31 File Offset: 0x00179131
	protected override void OnLoadLevel()
	{
		base.Unsubscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate, false);
		base.Unsubscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate, false);
		base.OnLoadLevel();
	}

	// Token: 0x06004311 RID: 17169 RVA: 0x0017AF5B File Offset: 0x0017915B
	private void MarkStatusItemRendererDirty(object _)
	{
		this.statusItemRenderer.MarkAllDirty();
	}

	// Token: 0x06004312 RID: 17170 RVA: 0x0017AF68 File Offset: 0x00179168
	protected override void OnForcedCleanUp()
	{
		if (this.prioritizableRenderer != null)
		{
			this.prioritizableRenderer.Cleanup();
			this.prioritizableRenderer = null;
		}
		if (this.statusItemRenderer != null)
		{
			this.statusItemRenderer.Destroy();
			this.statusItemRenderer = null;
		}
		if (this.conduitTemperatureManager != null)
		{
			this.conduitTemperatureManager.Shutdown();
		}
		this.gasFlowVisualizer.FreeResources();
		this.liquidFlowVisualizer.FreeResources();
		this.solidFlowVisualizer.FreeResources();
		LightGridManager.Shutdown();
		RadiationGridManager.Shutdown();
		App.OnPreLoadScene = (System.Action)Delegate.Remove(App.OnPreLoadScene, new System.Action(this.StopBE));
		base.OnForcedCleanUp();
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x0017B010 File Offset: 0x00179210
	protected override void OnSpawn()
	{
		global::Debug.Log("-- GAME --");
		Game.BrainScheduler = base.GetComponent<BrainScheduler>();
		PropertyTextures.FogOfWarScale = 0f;
		if (CameraController.Instance != null)
		{
			CameraController.Instance.EnableFreeCamera(false);
		}
		this.LocalPlayer = this.SpawnPlayer();
		WaterCubes.Instance.Init();
		SpeedControlScreen.Instance.Pause(false, false);
		LightGridManager.Initialise();
		RadiationGridManager.Initialise();
		this.RefreshRadiationLoop();
		this.UnsafeOnSpawn();
		Time.timeScale = 0f;
		if (this.tempIntroScreenPrefab != null)
		{
			global::Util.KInstantiate(this.tempIntroScreenPrefab, null, null);
		}
		if (SaveLoader.Instance.Cluster != null)
		{
			foreach (WorldGen worldGen in SaveLoader.Instance.Cluster.worlds)
			{
				this.Reset(worldGen.data.gameSpawnData, worldGen.WorldOffset);
			}
			NewBaseScreen.SetInitialCamera();
		}
		TagManager.FillMissingProperNames();
		CameraController.Instance.OrthographicSize = 20f;
		if (SaveLoader.Instance.loadedFromSave)
		{
			this.baseAlreadyCreated = true;
			base.Trigger(-1992507039, null);
			base.Trigger(-838649377, null);
		}
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
		for (int i = 0; i < array.Length; i++)
		{
			((MeshRenderer)array[i]).reflectionProbeUsage = ReflectionProbeUsage.Off;
		}
		base.Subscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate);
		base.Subscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate);
		this.solidConduitFlow.Initialize();
		SimAndRenderScheduler.instance.Add(this.roomProber, false);
		SimAndRenderScheduler.instance.Add(this.spaceScannerNetworkManager, false);
		SimAndRenderScheduler.instance.Add(KComponentSpawn.instance, false);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(new UpdateBucketWithUpdater<ISim200ms>.BatchUpdateDelegate(AmountInstance.BatchUpdate));
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(new UpdateBucketWithUpdater<ISim1000ms>.BatchUpdateDelegate(SolidTransferArm.BatchUpdate));
		if (!SaveLoader.Instance.loadedFromSave)
		{
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.SandboxMode.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SandboxMode);
			SaveGame.Instance.sandboxEnabled = !settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}
		this.mingleCellTracker = base.gameObject.AddComponent<MingleCellTracker>();
		if (Global.Instance != null)
		{
			Global.Instance.GetComponent<PerformanceMonitor>().Reset();
			Global.Instance.modManager.NotifyDialog(UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.TITLE, UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.MESSAGE, Global.Instance.globalCanvas);
		}
	}

	// Token: 0x06004314 RID: 17172 RVA: 0x0017B2C8 File Offset: 0x001794C8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SimAndRenderScheduler.instance.Remove(KComponentSpawn.instance);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(null);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(null);
		this.DestroyInstances();
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x0017B2FB File Offset: 0x001794FB
	private new void OnDestroy()
	{
		base.OnDestroy();
		this.DestroyInstances();
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x0017B309 File Offset: 0x00179509
	private void UnsafeOnSpawn()
	{
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, 0, null, 0, null);
	}

	// Token: 0x06004317 RID: 17175 RVA: 0x0017B328 File Offset: 0x00179528
	private void RefreshRadiationLoop()
	{
		GameScheduler.Instance.Schedule("UpdateRadiation", 1f, delegate(object obj)
		{
			RadiationGridManager.Refresh();
			this.RefreshRadiationLoop();
		}, null, null);
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x0017B34D File Offset: 0x0017954D
	public void SetMusicEnabled(bool enabled)
	{
		if (enabled)
		{
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			return;
		}
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x0017B374 File Offset: 0x00179574
	private Player SpawnPlayer()
	{
		Player component = global::Util.KInstantiate(this.playerPrefab, base.gameObject, null).GetComponent<Player>();
		component.ScreenManager = this.screenMgr;
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HudScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HoverTextScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.ToolTipScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		this.cameraController = global::Util.KInstantiate(this.cameraControllerPrefab, null, null).GetComponent<CameraController>();
		component.CameraController = this.cameraController;
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.cameraController, 1);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.cameraController, 1);
		}
		Global.GetInputManager().usedMenus.Add(this.cameraController);
		this.playerController = component.GetComponent<PlayerController>();
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.playerController, 20);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.playerController, 20);
		}
		Global.GetInputManager().usedMenus.Add(this.playerController);
		return component;
	}

	// Token: 0x0600431A RID: 17178 RVA: 0x0017B4B9 File Offset: 0x001796B9
	public void SetDupePassableSolid(int cell, bool passable, bool solid)
	{
		Grid.DupePassable[cell] = passable;
		this.gameSolidInfo.Add(new SolidInfo(cell, solid));
	}

	// Token: 0x0600431B RID: 17179 RVA: 0x0017B4DC File Offset: 0x001796DC
	private unsafe Sim.GameDataUpdate* StepTheSim(float dt)
	{
		IntPtr intPtr = IntPtr.Zero;
		if (Grid.Visible == null || Grid.Visible.Length == 0)
		{
			global::Debug.LogError("Invalid Grid.Visible, what have you done?!");
			return null;
		}
		intPtr = Sim.HandleMessage(SimMessageHashes.PrepareGameData, Grid.Visible.Length, Grid.Visible);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)intPtr);
		Grid.elementIdx = ptr->elementIdx;
		Grid.temperature = ptr->temperature;
		Grid.mass = ptr->mass;
		Grid.radiation = ptr->radiation;
		Grid.properties = ptr->properties;
		Grid.strengthInfo = ptr->strengthInfo;
		Grid.insulation = ptr->insulation;
		Grid.diseaseIdx = ptr->diseaseIdx;
		Grid.diseaseCount = ptr->diseaseCount;
		Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
		Grid.exposedToSunlight = (byte*)((void*)ptr->propertyTextureExposedToSunlight);
		PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
		PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
		PropertyTextures.externalLiquidDataTex = ptr->propertyTextureLiquidData;
		PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
		List<Element> elements = ElementLoader.elements;
		this.simData.emittedMassEntries = ptr->emittedMassEntries;
		this.simData.elementChunks = ptr->elementChunkInfos;
		this.simData.buildingTemperatures = ptr->buildingTemperatures;
		this.simData.diseaseEmittedInfos = ptr->diseaseEmittedInfos;
		this.simData.diseaseConsumedInfos = ptr->diseaseConsumedInfos;
		for (int i = 0; i < ptr->numSubstanceChangeInfo; i++)
		{
			Sim.SubstanceChangeInfo substanceChangeInfo = ptr->substanceChangeInfo[i];
			Element element = elements[(int)substanceChangeInfo.newElemIdx];
			Grid.Element[substanceChangeInfo.cellIdx] = element;
		}
		for (int j = 0; j < ptr->numSolidInfo; j++)
		{
			Sim.SolidInfo solidInfo = ptr->solidInfo[j];
			if (!this.solidChangedFilter.Contains(solidInfo.cellIdx))
			{
				this.solidInfo.Add(new SolidInfo(solidInfo.cellIdx, solidInfo.isSolid != 0));
				bool flag = solidInfo.isSolid != 0;
				Grid.SetSolid(solidInfo.cellIdx, flag, CellEventLogger.Instance.SimMessagesSolid);
				if (flag && Grid.IsWorldValidCell(solidInfo.cellIdx))
				{
					int num = (int)Grid.WorldIdx[solidInfo.cellIdx];
					if (num >= 0 && num < ClusterManager.Instance.WorldContainers.Count)
					{
						WorldContainer worldContainer = ClusterManager.Instance.WorldContainers[num];
						int num2;
						int num3;
						Grid.CellToXY(solidInfo.cellIdx, out num2, out num3);
						if (!worldContainer.IsModuleInterior && num3 > worldContainer.WorldOffset.Y + worldContainer.WorldSize.Y - Grid.TopBorderHeight)
						{
							SimMessages.Dig(solidInfo.cellIdx, -1, true);
						}
					}
				}
			}
		}
		for (int k = 0; k < ptr->numCallbackInfo; k++)
		{
			Sim.CallbackInfo callbackInfo = ptr->callbackInfo[k];
			HandleVector<Game.CallbackInfo>.Handle handle = new HandleVector<Game.CallbackInfo>.Handle
			{
				index = callbackInfo.callbackIdx
			};
			if (!this.IsManuallyReleasedHandle(handle))
			{
				this.callbackInfo.Add(new Klei.CallbackInfo(handle));
			}
		}
		int numSpawnFallingLiquidInfo = ptr->numSpawnFallingLiquidInfo;
		for (int l = 0; l < numSpawnFallingLiquidInfo; l++)
		{
			Sim.SpawnFallingLiquidInfo spawnFallingLiquidInfo = ptr->spawnFallingLiquidInfo[l];
			FallingWater.instance.AddParticle(spawnFallingLiquidInfo.cellIdx, spawnFallingLiquidInfo.elemIdx, spawnFallingLiquidInfo.mass, spawnFallingLiquidInfo.temperature, spawnFallingLiquidInfo.diseaseIdx, spawnFallingLiquidInfo.diseaseCount, false, false, false, false);
		}
		int numDigInfo = ptr->numDigInfo;
		WorldDamage component = this.world.GetComponent<WorldDamage>();
		for (int m = 0; m < numDigInfo; m++)
		{
			Sim.SpawnOreInfo spawnOreInfo = ptr->digInfo[m];
			if (spawnOreInfo.temperature <= 0f && spawnOreInfo.mass > 0f)
			{
				global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
			}
			component.OnDigComplete(spawnOreInfo.cellIdx, spawnOreInfo.mass, spawnOreInfo.temperature, spawnOreInfo.elemIdx, spawnOreInfo.diseaseIdx, spawnOreInfo.diseaseCount);
		}
		int numSpawnOreInfo = ptr->numSpawnOreInfo;
		for (int n = 0; n < numSpawnOreInfo; n++)
		{
			Sim.SpawnOreInfo spawnOreInfo2 = ptr->spawnOreInfo[n];
			Vector3 position = Grid.CellToPosCCC(spawnOreInfo2.cellIdx, Grid.SceneLayer.Ore);
			Element element2 = ElementLoader.elements[(int)spawnOreInfo2.elemIdx];
			if (spawnOreInfo2.temperature <= 0f && spawnOreInfo2.mass > 0f)
			{
				global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
			}
			element2.substance.SpawnResource(position, spawnOreInfo2.mass, spawnOreInfo2.temperature, spawnOreInfo2.diseaseIdx, spawnOreInfo2.diseaseCount, false, false, false);
		}
		int numSpawnFXInfo = ptr->numSpawnFXInfo;
		for (int num4 = 0; num4 < numSpawnFXInfo; num4++)
		{
			Sim.SpawnFXInfo spawnFXInfo = ptr->spawnFXInfo[num4];
			this.SpawnFX((SpawnFXHashes)spawnFXInfo.fxHash, spawnFXInfo.cellIdx, spawnFXInfo.rotation);
		}
		UnstableGroundManager component2 = this.world.GetComponent<UnstableGroundManager>();
		int numUnstableCellInfo = ptr->numUnstableCellInfo;
		for (int num5 = 0; num5 < numUnstableCellInfo; num5++)
		{
			Sim.UnstableCellInfo unstableCellInfo = ptr->unstableCellInfo[num5];
			if (unstableCellInfo.fallingInfo == 0)
			{
				component2.Spawn(unstableCellInfo.cellIdx, ElementLoader.elements[(int)unstableCellInfo.elemIdx], unstableCellInfo.mass, unstableCellInfo.temperature, unstableCellInfo.diseaseIdx, unstableCellInfo.diseaseCount);
			}
		}
		int numWorldDamageInfo = ptr->numWorldDamageInfo;
		for (int num6 = 0; num6 < numWorldDamageInfo; num6++)
		{
			Sim.WorldDamageInfo damage_info = ptr->worldDamageInfo[num6];
			WorldDamage.Instance.ApplyDamage(damage_info);
		}
		for (int num7 = 0; num7 < ptr->numRemovedMassEntries; num7++)
		{
			ElementConsumer.AddMass(ptr->removedMassEntries[num7]);
		}
		int numMassConsumedCallbacks = ptr->numMassConsumedCallbacks;
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle2 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle);
		for (int num8 = 0; num8 < numMassConsumedCallbacks; num8++)
		{
			Sim.MassConsumedCallback massConsumedCallback = ptr->massConsumedCallbacks[num8];
			handle2.index = massConsumedCallback.callbackIdx;
			Game.ComplexCallbackInfo<Sim.MassConsumedCallback> complexCallbackInfo = this.massConsumedCallbackManager.Release(handle2, "massConsumedCB");
			if (complexCallbackInfo.cb != null)
			{
				complexCallbackInfo.cb(massConsumedCallback, complexCallbackInfo.callbackData);
			}
		}
		int numMassEmittedCallbacks = ptr->numMassEmittedCallbacks;
		HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle handle3 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle);
		for (int num9 = 0; num9 < numMassEmittedCallbacks; num9++)
		{
			Sim.MassEmittedCallback massEmittedCallback = ptr->massEmittedCallbacks[num9];
			handle3.index = massEmittedCallback.callbackIdx;
			if (this.massEmitCallbackManager.IsVersionValid(handle3))
			{
				Game.ComplexCallbackInfo<Sim.MassEmittedCallback> item = this.massEmitCallbackManager.GetItem(handle3);
				if (item.cb != null)
				{
					item.cb(massEmittedCallback, item.callbackData);
				}
			}
		}
		int numDiseaseConsumptionCallbacks = ptr->numDiseaseConsumptionCallbacks;
		HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle handle4 = default(HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle);
		for (int num10 = 0; num10 < numDiseaseConsumptionCallbacks; num10++)
		{
			Sim.DiseaseConsumptionCallback diseaseConsumptionCallback = ptr->diseaseConsumptionCallbacks[num10];
			handle4.index = diseaseConsumptionCallback.callbackIdx;
			if (this.diseaseConsumptionCallbackManager.IsVersionValid(handle4))
			{
				Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback> item2 = this.diseaseConsumptionCallbackManager.GetItem(handle4);
				if (item2.cb != null)
				{
					item2.cb(diseaseConsumptionCallback, item2.callbackData);
				}
			}
		}
		int numComponentStateChangedMessages = ptr->numComponentStateChangedMessages;
		HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle5 = default(HandleVector<Game.ComplexCallbackInfo<int>>.Handle);
		for (int num11 = 0; num11 < numComponentStateChangedMessages; num11++)
		{
			Sim.ComponentStateChangedMessage componentStateChangedMessage = ptr->componentStateChangedMessages[num11];
			handle5.index = componentStateChangedMessage.callbackIdx;
			if (this.simComponentCallbackManager.IsVersionValid(handle5))
			{
				Game.ComplexCallbackInfo<int> complexCallbackInfo2 = this.simComponentCallbackManager.Release(handle5, "component state changed cb");
				if (complexCallbackInfo2.cb != null)
				{
					complexCallbackInfo2.cb(componentStateChangedMessage.simHandle, complexCallbackInfo2.callbackData);
				}
			}
		}
		int numRadiationConsumedCallbacks = ptr->numRadiationConsumedCallbacks;
		HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle handle6 = default(HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle);
		for (int num12 = 0; num12 < numRadiationConsumedCallbacks; num12++)
		{
			Sim.ConsumedRadiationCallback consumedRadiationCallback = ptr->radiationConsumedCallbacks[num12];
			handle6.index = consumedRadiationCallback.callbackIdx;
			Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback> complexCallbackInfo3 = this.radiationConsumedCallbackManager.Release(handle6, "radiationConsumedCB");
			if (complexCallbackInfo3.cb != null)
			{
				complexCallbackInfo3.cb(consumedRadiationCallback, complexCallbackInfo3.callbackData);
			}
		}
		int numElementChunkMeltedInfos = ptr->numElementChunkMeltedInfos;
		for (int num13 = 0; num13 < numElementChunkMeltedInfos; num13++)
		{
			SimTemperatureTransfer.DoOreMeltTransition(ptr->elementChunkMeltedInfos[num13].handle);
		}
		int numBuildingOverheatInfos = ptr->numBuildingOverheatInfos;
		for (int num14 = 0; num14 < numBuildingOverheatInfos; num14++)
		{
			StructureTemperatureComponents.DoOverheat(ptr->buildingOverheatInfos[num14].handle);
		}
		int numBuildingNoLongerOverheatedInfos = ptr->numBuildingNoLongerOverheatedInfos;
		for (int num15 = 0; num15 < numBuildingNoLongerOverheatedInfos; num15++)
		{
			StructureTemperatureComponents.DoNoLongerOverheated(ptr->buildingNoLongerOverheatedInfos[num15].handle);
		}
		int numBuildingMeltedInfos = ptr->numBuildingMeltedInfos;
		for (int num16 = 0; num16 < numBuildingMeltedInfos; num16++)
		{
			StructureTemperatureComponents.DoStateTransition(ptr->buildingMeltedInfos[num16].handle);
		}
		int numCellMeltedInfos = ptr->numCellMeltedInfos;
		for (int num17 = 0; num17 < numCellMeltedInfos; num17++)
		{
			int gameCell = ptr->cellMeltedInfos[num17].gameCell;
			GameObject gameObject = Grid.Objects[gameCell, 9];
			if (gameObject != null)
			{
				gameObject.Trigger(675471409, null);
				global::Util.KDestroyGameObject(gameObject);
			}
		}
		if (dt > 0f)
		{
			this.conduitTemperatureManager.Sim200ms(0.2f);
			this.conduitDiseaseManager.Sim200ms(0.2f);
			this.gasConduitFlow.Sim200ms(0.2f);
			this.liquidConduitFlow.Sim200ms(0.2f);
			this.solidConduitFlow.Sim200ms(0.2f);
			this.accumulators.Sim200ms(0.2f);
			this.plantElementAbsorbers.Sim200ms(0.2f);
		}
		Sim.DebugProperties debugProperties;
		debugProperties.buildingTemperatureScale = 100f;
		debugProperties.buildingToBuildingTemperatureScale = 0.001f;
		debugProperties.biomeTemperatureLerpRate = 0.001f;
		debugProperties.isDebugEditing = ((DebugPaintElementScreen.Instance != null && DebugPaintElementScreen.Instance.gameObject.activeSelf) ? 1 : 0);
		debugProperties.pad0 = (debugProperties.pad1 = (debugProperties.pad2 = 0));
		SimMessages.SetDebugProperties(debugProperties);
		if (dt > 0f)
		{
			if (this.circuitManager != null)
			{
				this.circuitManager.Sim200msFirst(dt);
			}
			if (this.energySim != null)
			{
				this.energySim.EnergySim200ms(dt);
			}
			if (this.circuitManager != null)
			{
				this.circuitManager.Sim200msLast(dt);
			}
		}
		return ptr;
	}

	// Token: 0x0600431C RID: 17180 RVA: 0x0017C000 File Offset: 0x0017A200
	public void AddSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Add(cell);
	}

	// Token: 0x0600431D RID: 17181 RVA: 0x0017C00F File Offset: 0x0017A20F
	public void RemoveSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Remove(cell);
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x0017C01E File Offset: 0x0017A21E
	public void SetIsLoading()
	{
		this.isLoading = true;
	}

	// Token: 0x0600431F RID: 17183 RVA: 0x0017C027 File Offset: 0x0017A227
	public bool IsLoading()
	{
		return this.isLoading;
	}

	// Token: 0x06004320 RID: 17184 RVA: 0x0017C030 File Offset: 0x0017A230
	private void ShowDebugCellInfo()
	{
		int mouseCell = DebugHandler.GetMouseCell();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(mouseCell, out num, out num2);
		string text = string.Concat(new string[]
		{
			mouseCell.ToString(),
			" (",
			num.ToString(),
			", ",
			num2.ToString(),
			")"
		});
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x06004321 RID: 17185 RVA: 0x0017C0AB File Offset: 0x0017A2AB
	public void ForceSimStep()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Force-stepping the sim"
		});
		this.simDt = 0.2f;
	}

	// Token: 0x06004322 RID: 17186 RVA: 0x0017C0CC File Offset: 0x0017A2CC
	private void Update()
	{
		if (this.isLoading)
		{
			return;
		}
		SuperluminalPerf.BeginEvent("Game.Update", null);
		float deltaTime = Time.deltaTime;
		if (global::Debug.developerConsoleVisible)
		{
			global::Debug.developerConsoleVisible = false;
		}
		if (DebugHandler.DebugCellInfo)
		{
			this.ShowDebugCellInfo();
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		this.circuitManager.RenderEveryTick(deltaTime);
		this.logicCircuitManager.RenderEveryTick(deltaTime);
		this.solidConduitFlow.RenderEveryTick(deltaTime);
		Pathfinding.Instance.RenderEveryTick();
		Singleton<CellChangeMonitor>.Instance.RenderEveryTick();
		this.SimEveryTick(deltaTime);
		SuperluminalPerf.EndEvent();
		AsyncPathProber.Instance.TickFrame();
	}

	// Token: 0x06004323 RID: 17187 RVA: 0x0017C180 File Offset: 0x0017A380
	private void SimEveryTick(float dt)
	{
		dt = Mathf.Min(dt, 0.2f);
		this.simDt += dt;
		if (this.simDt >= 0.016666668f)
		{
			do
			{
				this.simSubTick++;
				this.simSubTick %= 12;
				if (this.simSubTick == 0)
				{
					this.hasFirstSimTickRun = true;
					this.UnsafeSim200ms(0.2f);
				}
				if (this.hasFirstSimTickRun)
				{
					Singleton<StateMachineUpdater>.Instance.AdvanceOneSimSubTick();
				}
				this.simDt -= 0.016666668f;
			}
			while (this.simDt >= 0.016666668f);
			return;
		}
		this.UnsafeSim200ms(0f);
	}

	// Token: 0x06004324 RID: 17188 RVA: 0x0017C22C File Offset: 0x0017A42C
	private unsafe void UnsafeSim200ms(float dt)
	{
		this.simActiveRegions.Clear();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered)
			{
				Game.SimActiveRegion simActiveRegion = new Game.SimActiveRegion();
				simActiveRegion.region = new Pair<Vector2I, Vector2I>(worldContainer.WorldOffset, worldContainer.WorldOffset + worldContainer.WorldSize);
				simActiveRegion.currentSunlightIntensity = worldContainer.currentSunlightIntensity;
				simActiveRegion.currentCosmicRadiationIntensity = worldContainer.currentCosmicIntensity;
				this.simActiveRegions.Add(simActiveRegion);
			}
		}
		global::Debug.Assert(this.simActiveRegions.Count > 0, "Cannot send a frame to the sim with zero active regions");
		SimMessages.NewGameFrame(dt, this.simActiveRegions);
		Sim.GameDataUpdate* ptr = this.StepTheSim(dt);
		if (ptr == null)
		{
			global::Debug.LogError("UNEXPECTED!");
			return;
		}
		if (ptr->numFramesProcessed <= 0)
		{
			return;
		}
		this.gameSolidInfo.AddRange(this.solidInfo);
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, ptr->numSolidSubstanceChangeInfo, ptr->solidSubstanceChangeInfo, ptr->numLiquidChangeInfo, ptr->liquidChangeInfo);
		this.gameSolidInfo.Clear();
		this.solidInfo.Clear();
		this.callbackInfo.Clear();
		this.callbackManagerManuallyReleasedHandles.Clear();
		Pathfinding.Instance.UpdateNavGrids(false);
	}

	// Token: 0x06004325 RID: 17189 RVA: 0x0017C398 File Offset: 0x0017A598
	private void LateUpdateComponents()
	{
		this.UpdateOverlayScreen();
	}

	// Token: 0x06004326 RID: 17190 RVA: 0x0017C3A0 File Offset: 0x0017A5A0
	private void OnAddBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		this.lastDrawnOverlayMode = default(HashedString);
		if (PlayerController.Instance != null)
		{
			BuildTool buildTool = PlayerController.Instance.ActiveTool as BuildTool;
			if (buildTool != null && buildTool.visualizer == entity_cell_visualizer.gameObject)
			{
				this.previewVisualizer = entity_cell_visualizer;
			}
		}
	}

	// Token: 0x06004327 RID: 17191 RVA: 0x0017C3F9 File Offset: 0x0017A5F9
	private void OnRemoveBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		if (this.previewVisualizer == entity_cell_visualizer)
		{
			this.previewVisualizer = null;
		}
	}

	// Token: 0x06004328 RID: 17192 RVA: 0x0017C410 File Offset: 0x0017A610
	private void UpdateOverlayScreen()
	{
		if (OverlayScreen.Instance == null)
		{
			return;
		}
		HashedString mode = OverlayScreen.Instance.GetMode();
		if (this.previewVisualizer != null)
		{
			this.previewVisualizer.DrawIcons(mode);
		}
		if (mode == this.lastDrawnOverlayMode)
		{
			return;
		}
		foreach (EntityCellVisualizer entityCellVisualizer in Components.EntityCellVisualizers.Items)
		{
			entityCellVisualizer.DrawIcons(mode);
		}
		this.lastDrawnOverlayMode = mode;
	}

	// Token: 0x06004329 RID: 17193 RVA: 0x0017C4B0 File Offset: 0x0017A6B0
	public void ForceOverlayUpdate(bool clearLastMode = false)
	{
		this.previousOverlayMode = OverlayModes.None.ID;
		if (clearLastMode)
		{
			this.lastDrawnOverlayMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x0600432A RID: 17194 RVA: 0x0017C4CC File Offset: 0x0017A6CC
	private void LateUpdate()
	{
		SuperluminalPerf.BeginEvent("Game.LateUpdate", null);
		if (this.OnSpawnComplete != null)
		{
			this.OnSpawnComplete();
			this.OnSpawnComplete = null;
		}
		if (Time.timeScale == 0f && !this.IsPaused)
		{
			this.IsPaused = true;
			base.Trigger(-1788536802, BoxedBools.Box(this.IsPaused));
		}
		else if (Time.timeScale != 0f && this.IsPaused)
		{
			this.IsPaused = false;
			base.Trigger(-1788536802, BoxedBools.Box(this.IsPaused));
		}
		if (Input.GetMouseButton(0))
		{
			this.VisualTunerElement = null;
			int mouseCell = DebugHandler.GetMouseCell();
			if (Grid.IsValidCell(mouseCell))
			{
				Element visualTunerElement = Grid.Element[mouseCell];
				this.VisualTunerElement = visualTunerElement;
			}
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		HashedString mode = SimDebugView.Instance.GetMode();
		if (mode != this.previousOverlayMode)
		{
			this.previousOverlayMode = mode;
			if (mode == OverlayModes.LiquidConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(true, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.GasConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(true, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.SolidConveyor.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(true, true);
			}
			else
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, false);
				this.gasFlowVisualizer.ColourizePipeContents(false, false);
				this.solidFlowVisualizer.ColourizePipeContents(false, false);
			}
		}
		this.gasFlowVisualizer.Render(this.gasFlowPos.z, 0, this.gasConduitFlow.ContinuousLerpPercent, mode == OverlayModes.GasConduits.ID && this.gasConduitFlow.DiscreteLerpPercent != this.previousGasConduitFlowDiscreteLerpPercent);
		this.liquidFlowVisualizer.Render(this.liquidFlowPos.z, 0, this.liquidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.LiquidConduits.ID && this.liquidConduitFlow.DiscreteLerpPercent != this.previousLiquidConduitFlowDiscreteLerpPercent);
		this.solidFlowVisualizer.Render(this.solidFlowPos.z, 0, this.solidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.SolidConveyor.ID && this.solidConduitFlow.DiscreteLerpPercent != this.previousSolidConduitFlowDiscreteLerpPercent);
		this.previousGasConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.GasConduits.ID) ? this.gasConduitFlow.DiscreteLerpPercent : -1f);
		this.previousLiquidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.LiquidConduits.ID) ? this.liquidConduitFlow.DiscreteLerpPercent : -1f);
		this.previousSolidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.SolidConveyor.ID) ? this.solidConduitFlow.DiscreteLerpPercent : -1f);
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Shader.SetGlobalVector("_WsToCs", new Vector4(vector.x / (float)Grid.WidthInCells, vector.y / (float)Grid.HeightInCells, (vector2.x - vector.x) / (float)Grid.WidthInCells, (vector2.y - vector.y) / (float)Grid.HeightInCells));
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 value = new Vector4((vector.x - (float)worldOffset.x) / (float)worldSize.x, (vector.y - (float)worldOffset.y) / (float)(worldSize.y - activeWorld.HiddenYOffset), (vector2.x - vector.x) / (float)worldSize.x, (vector2.y - vector.y) / (float)(worldSize.y - activeWorld.HiddenYOffset));
		Shader.SetGlobalVector("_WsToCcs", value);
		if (this.drawStatusItems)
		{
			this.statusItemRenderer.RenderEveryTick();
			this.prioritizableRenderer.RenderEveryTick();
		}
		this.LateUpdateComponents();
		Singleton<StateMachineUpdater>.Instance.Render(Time.unscaledDeltaTime);
		Singleton<StateMachineUpdater>.Instance.RenderEveryTick(Time.unscaledDeltaTime);
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
			if (component != null)
			{
				component.DrawPath();
			}
		}
		KFMOD.RenderEveryTick(Time.deltaTime);
		SuperluminalPerf.EndEvent();
		if (GenericGameSettings.instance.scriptedProfile.frameCount != 0 && this.IsPaused && SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
	}

	// Token: 0x0600432B RID: 17195 RVA: 0x0017C9F4 File Offset: 0x0017ABF4
	public void Reset(GameSpawnData gsd, Vector2I world_offset)
	{
		if (gsd == null)
		{
			return;
		}
		foreach (KeyValuePair<Vector2I, bool> keyValuePair in gsd.preventFoWReveal)
		{
			if (keyValuePair.Value)
			{
				Vector2I v = new Vector2I(keyValuePair.Key.X + world_offset.X, keyValuePair.Key.Y + world_offset.Y);
				Grid.PreventFogOfWarReveal[Grid.PosToCell(v)] = keyValuePair.Value;
			}
		}
	}

	// Token: 0x0600432C RID: 17196 RVA: 0x0017CAA0 File Offset: 0x0017ACA0
	private void OnApplicationQuit()
	{
		Game.quitting = true;
		Sim.Shutdown();
		AudioMixer.Destroy();
		if (this.screenMgr != null && this.screenMgr.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.screenMgr.gameObject);
		}
		Console.WriteLine("Game.OnApplicationQuit()");
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x0017CAF8 File Offset: 0x0017ACF8
	private void InitializeFXSpawners()
	{
		for (int i = 0; i < this.fxSpawnData.Length; i++)
		{
			int fx_idx = i;
			this.fxSpawnData[fx_idx].fxPrefab.SetActive(false);
			ushort fx_mask = (ushort)(1 << fx_idx);
			Action<SpawnFXHashes, GameObject> destroyer = delegate(SpawnFXHashes fxid, GameObject go)
			{
				if (!Game.IsQuitting())
				{
					int num = Grid.PosToCell(go);
					ushort[] array = this.activeFX;
					int num2 = num;
					array[num2] &= ~fx_mask;
					go.GetComponent<KAnimControllerBase>().enabled = false;
					this.fxPools[(int)fxid].ReleaseInstance(go);
				}
			};
			Func<GameObject> instantiator = delegate()
			{
				GameObject gameObject = GameUtil.KInstantiate(this.fxSpawnData[fx_idx].fxPrefab, Grid.SceneLayer.Front, null, 0);
				KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				gameObject.SetActive(true);
				component.onDestroySelf = delegate(GameObject go)
				{
					destroyer(this.fxSpawnData[fx_idx].id, go);
				};
				return gameObject;
			};
			GameObjectPool pool = new GameObjectPool(instantiator, delegate(GameObject _)
			{
			}, this.fxSpawnData[fx_idx].initialCount);
			this.fxPools[(int)this.fxSpawnData[fx_idx].id] = pool;
			this.fxSpawner[(int)this.fxSpawnData[fx_idx].id] = delegate(Vector3 pos, float rotation)
			{
				Game.SpawnFXParams spawnFXParams = Game.SpawnFXParams.Pool.Get();
				spawnFXParams.pool = pool;
				spawnFXParams.fx_idx = fx_idx;
				spawnFXParams.pos = pos;
				spawnFXParams.rotation = rotation;
				if (Game.Instance.IsPaused)
				{
					Game.GameInitializeFXSpawners_SpawnFX(spawnFXParams);
					return;
				}
				GameScheduler.Instance.Schedule("SpawnFX", 0f, Game.GameInitializeFXSpawners_SpawnFX, spawnFXParams, null);
			};
		}
	}

	// Token: 0x0600432E RID: 17198 RVA: 0x0017CC14 File Offset: 0x0017AE14
	public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation)
	{
		Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
		if (CameraController.Instance.IsVisiblePos(vector))
		{
			this.fxSpawner[(int)fx_id](vector, rotation);
		}
	}

	// Token: 0x0600432F RID: 17199 RVA: 0x0017CC4A File Offset: 0x0017AE4A
	public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation)
	{
		this.fxSpawner[(int)fx_id](pos, rotation);
	}

	// Token: 0x06004330 RID: 17200 RVA: 0x0017CC5F File Offset: 0x0017AE5F
	public static void SaveSettings(BinaryWriter writer)
	{
		Serializer.Serialize(new Game.Settings(Game.Instance), writer);
	}

	// Token: 0x06004331 RID: 17201 RVA: 0x0017CC74 File Offset: 0x0017AE74
	public static void LoadSettings(Deserializer deserializer)
	{
		Game.Settings settings = new Game.Settings();
		deserializer.Deserialize(settings);
		KPlayerPrefs.SetInt(Game.NextUniqueIDKey, settings.nextUniqueID);
		KleiMetrics.SetGameID(settings.gameID);
	}

	// Token: 0x06004332 RID: 17202 RVA: 0x0017CCAC File Offset: 0x0017AEAC
	public void Save(BinaryWriter writer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = SaveLoader.Instance.clusterDetailSave;
		gameSaveData.debugWasUsed = this.debugWasUsed;
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		gameSaveData.autoPrioritizeRoles = this.autoPrioritizeRoles;
		gameSaveData.advancedPersonalPriorities = this.advancedPersonalPriorities;
		gameSaveData.savedInfo = this.savedInfo;
		global::Debug.Assert(gameSaveData.worldDetail != null, "World detail null");
		gameSaveData.dateGenerated = this.dateGenerated;
		if (!this.changelistsPlayedOn.Contains(706793U))
		{
			this.changelistsPlayedOn.Add(706793U);
		}
		gameSaveData.changelistsPlayedOn = this.changelistsPlayedOn;
		if (this.OnSave != null)
		{
			this.OnSave(gameSaveData);
		}
		Serializer.Serialize(gameSaveData, writer);
	}

	// Token: 0x06004333 RID: 17203 RVA: 0x0017CDC8 File Offset: 0x0017AFC8
	public void Load(Deserializer deserializer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = new WorldDetailSave();
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		deserializer.Deserialize(gameSaveData);
		this.gasConduitFlow = gameSaveData.gasConduitFlow;
		this.liquidConduitFlow = gameSaveData.liquidConduitFlow;
		this.debugWasUsed = gameSaveData.debugWasUsed;
		this.autoPrioritizeRoles = gameSaveData.autoPrioritizeRoles;
		this.advancedPersonalPriorities = gameSaveData.advancedPersonalPriorities;
		this.dateGenerated = gameSaveData.dateGenerated;
		this.changelistsPlayedOn = (gameSaveData.changelistsPlayedOn ?? new List<uint>());
		if (gameSaveData.dateGenerated.IsNullOrWhiteSpace())
		{
			this.dateGenerated = "Before U41 (Feb 2022)";
		}
		DebugUtil.LogArgs(new object[]
		{
			"SAVEINFO"
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Generated: " + this.dateGenerated
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Played on: " + string.Join<uint>(", ", this.changelistsPlayedOn)
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Debug was used: " + Game.Instance.debugWasUsed.ToString()
		});
		this.savedInfo = gameSaveData.savedInfo;
		this.savedInfo.InitializeEmptyVariables();
		CustomGameSettings.Instance.Print();
		KCrashReporter.debugWasUsed = this.debugWasUsed;
		SaveLoader.Instance.SetWorldDetail(gameSaveData.worldDetail);
		if (this.OnLoad != null)
		{
			this.OnLoad(gameSaveData);
		}
		base.StartCoroutine(PerformanceCaptureMonitor.FinishedLoadingSave());
	}

	// Token: 0x06004334 RID: 17204 RVA: 0x0017CF9F File Offset: 0x0017B19F
	public void SetAutoSaveCallbacks(Game.SavingPreCB activatePreCB, Game.SavingActiveCB activateActiveCB, Game.SavingPostCB activatePostCB)
	{
		this.activatePreCB = activatePreCB;
		this.activateActiveCB = activateActiveCB;
		this.activatePostCB = activatePostCB;
	}

	// Token: 0x06004335 RID: 17205 RVA: 0x0017CFB6 File Offset: 0x0017B1B6
	public void StartDelayedInitialSave()
	{
		base.StartCoroutine(this.DelayedInitialSave());
	}

	// Token: 0x06004336 RID: 17206 RVA: 0x0017CFC5 File Offset: 0x0017B1C5
	private IEnumerator DelayedInitialSave()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (GenericGameSettings.instance.devAutoWorldGenActive)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.SetDiscovered(true);
			}
			SaveGame.Instance.worldGenSpawner.SpawnEverything();
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().DEBUG_REVEAL_ENTIRE_MAP();
			if (CameraController.Instance != null)
			{
				CameraController.Instance.EnableFreeCamera(true);
			}
			for (int num2 = 0; num2 != Grid.WidthInCells * Grid.HeightInCells; num2++)
			{
				Grid.Reveal(num2, byte.MaxValue, false);
			}
			GenericGameSettings.instance.devAutoWorldGenActive = false;
		}
		SaveLoader.Instance.InitialSave();
		yield break;
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x0017CFD0 File Offset: 0x0017B1D0
	public void StartDelayedSave(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		if (this.activatePreCB != null)
		{
			this.activatePreCB(delegate
			{
				this.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
			});
			return;
		}
		base.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x0017D03E File Offset: 0x0017B23E
	private IEnumerator DelayedSave(string filename, bool isAutoSave, bool updateSavePointer)
	{
		while (PlayerController.Instance.IsDragging())
		{
			yield return null;
		}
		PlayerController.Instance.CancelDragging();
		PlayerController.Instance.AllowDragging(false);
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (this.activateActiveCB != null)
		{
			this.activateActiveCB();
			for (int i = 0; i < 1; i = num)
			{
				yield return null;
				num = i + 1;
			}
		}
		SaveLoader.Instance.Save(filename, isAutoSave, updateSavePointer);
		if (this.activatePostCB != null)
		{
			this.activatePostCB();
		}
		for (int i = 0; i < 5; i = num)
		{
			yield return null;
			num = i + 1;
		}
		PlayerController.Instance.AllowDragging(true);
		yield break;
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x0017D062 File Offset: 0x0017B262
	public void StartDelayed(int tick_delay, System.Action action)
	{
		base.StartCoroutine(this.DelayedExecutor(tick_delay, action));
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x0017D073 File Offset: 0x0017B273
	private IEnumerator DelayedExecutor(int tick_delay, System.Action action)
	{
		int num;
		for (int i = 0; i < tick_delay; i = num)
		{
			yield return null;
			num = i + 1;
		}
		action();
		yield break;
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x0017D08C File Offset: 0x0017B28C
	private void LoadEventHashes()
	{
		foreach (object obj in Enum.GetValues(typeof(GameHashes)))
		{
			GameHashes hash = (GameHashes)obj;
			HashCache.Get().Add((int)hash, hash.ToString());
		}
		foreach (object obj2 in Enum.GetValues(typeof(UtilHashes)))
		{
			UtilHashes hash2 = (UtilHashes)obj2;
			HashCache.Get().Add((int)hash2, hash2.ToString());
		}
		foreach (object obj3 in Enum.GetValues(typeof(UIHashes)))
		{
			UIHashes hash3 = (UIHashes)obj3;
			HashCache.Get().Add((int)hash3, hash3.ToString());
		}
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x0017D1C8 File Offset: 0x0017B3C8
	public void StopFE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = false;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		}
		MainMenu.Instance.StopMainMenuMusic();
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x0017D22E File Offset: 0x0017B42E
	public void StartBE()
	{
		Resources.UnloadUnusedAssets();
		AudioMixer.instance.Reset();
		AudioMixer.instance.StartPersistentSnapshots();
		MusicManager.instance.ConfigureSongs();
		if (MusicManager.instance.ShouldPlayDynamicMusicLoadedGame())
		{
			MusicManager.instance.PlayDynamicMusic();
		}
	}

	// Token: 0x0600433E RID: 17214 RVA: 0x0017D26C File Offset: 0x0017B46C
	public void StopBE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = true;
		}
		LoopingSoundManager loopingSoundManager = LoopingSoundManager.Get();
		if (loopingSoundManager != null)
		{
			loopingSoundManager.StopAllSounds();
		}
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.StopPersistentSnapshots();
		foreach (List<SaveLoadRoot> list in SaveLoader.Instance.saveManager.GetLists().Values)
		{
			foreach (SaveLoadRoot saveLoadRoot in list)
			{
				if (saveLoadRoot.gameObject != null)
				{
					global::Util.KDestroyGameObject(saveLoadRoot.gameObject);
				}
			}
		}
		base.GetComponent<EntombedItemVisualizer>().Clear();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		KComponentSpawn.instance.comps.Clear();
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.cameraController);
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.playerController);
		Sim.Shutdown();
		SimAndRenderScheduler.instance.Reset();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x0600433F RID: 17215 RVA: 0x0017D3BC File Offset: 0x0017B5BC
	public void SetStatusItemOffset(Transform transform, Vector3 offset)
	{
		this.statusItemRenderer.SetOffset(transform, offset);
	}

	// Token: 0x06004340 RID: 17216 RVA: 0x0017D3CB File Offset: 0x0017B5CB
	public void AddStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Add(transform, status_item);
	}

	// Token: 0x06004341 RID: 17217 RVA: 0x0017D3DA File Offset: 0x0017B5DA
	public void RemoveStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Remove(transform, status_item);
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06004342 RID: 17218 RVA: 0x0017D3E9 File Offset: 0x0017B5E9
	public float LastTimeWorkStarted
	{
		get
		{
			return this.lastTimeWorkStarted;
		}
	}

	// Token: 0x06004343 RID: 17219 RVA: 0x0017D3F1 File Offset: 0x0017B5F1
	public void StartedWork()
	{
		this.lastTimeWorkStarted = Time.time;
	}

	// Token: 0x06004344 RID: 17220 RVA: 0x0017D3FE File Offset: 0x0017B5FE
	private void SpawnOxygenBubbles(Vector3 position, float angle)
	{
	}

	// Token: 0x06004345 RID: 17221 RVA: 0x0017D400 File Offset: 0x0017B600
	public void ManualReleaseHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.callbackManagerManuallyReleasedHandles.Add(handle.index);
		this.callbackManager.Release(handle);
	}

	// Token: 0x06004346 RID: 17222 RVA: 0x0017D42B File Offset: 0x0017B62B
	private bool IsManuallyReleasedHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		return !this.callbackManager.IsVersionValid(handle) && this.callbackManagerManuallyReleasedHandles.Contains(handle.index);
	}

	// Token: 0x06004347 RID: 17223 RVA: 0x0017D452 File Offset: 0x0017B652
	[ContextMenu("Print")]
	private void Print()
	{
		Console.WriteLine("This is a console writeline test");
		global::Debug.Log("This is a debug log test");
	}

	// Token: 0x06004348 RID: 17224 RVA: 0x0017D468 File Offset: 0x0017B668
	private void DestroyInstances()
	{
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		Db.Get().ResetProblematicDbs();
		AsyncPathProber.DestroyInstance();
		GridSettings.ClearGrid();
		StateMachineManager.ResetParameters();
		ChoreTable.Instance.ResetParameters();
		BubbleManager.DestroyInstance();
		AmbientSoundManager.Destroy();
		AutoDisinfectableManager.DestroyInstance();
		BuildMenu.DestroyInstance();
		CancelTool.DestroyInstance();
		ClearTool.DestroyInstance();
		ChoreGroupManager.DestroyInstance();
		CO2Manager.DestroyInstance();
		ConsumerManager.DestroyInstance();
		CopySettingsTool.DestroyInstance();
		global::DateTime.DestroyInstance();
		DebugBaseTemplateButton.DestroyInstance();
		DebugPaintElementScreen.DestroyInstance();
		DetailsScreen.DestroyInstance();
		DietManager.DestroyInstance();
		DebugText.DestroyInstance();
		FactionManager.DestroyInstance();
		EmptyPipeTool.DestroyInstance();
		FetchListStatusItemUpdater.DestroyInstance();
		FishOvercrowingManager.DestroyInstance();
		FallingWater.DestroyInstance();
		GridCompositor.DestroyInstance();
		Infrared.DestroyInstance();
		KPrefabIDTracker.DestroyInstance();
		ManagementMenu.DestroyInstance();
		ClusterMapScreen.DestroyInstance();
		Messenger.DestroyInstance();
		LoopingSoundManager.DestroyInstance();
		MeterScreen.DestroyInstance();
		MinionGroupProber.DestroyInstance();
		NavPathDrawer.DestroyInstance();
		MinionIdentity.DestroyStatics();
		PathFinder.DestroyStatics();
		Pathfinding.DestroyInstance();
		PrebuildTool.DestroyInstance();
		PrioritizeTool.DestroyInstance();
		SelectTool.DestroyInstance();
		PopFXManager.DestroyInstance();
		ProgressBarsConfig.DestroyInstance();
		PropertyTextures.DestroyInstance();
		WorldResourceAmountTracker<RationTracker>.DestroyInstance();
		WorldResourceAmountTracker<ElectrobankTracker>.DestroyInstance();
		ReportManager.DestroyInstance();
		Research.DestroyInstance();
		RootMenu.DestroyInstance();
		SaveLoader.DestroyInstance();
		Scenario.DestroyInstance();
		SimDebugView.DestroyInstance();
		SpriteSheetAnimManager.DestroyInstance();
		ScheduleManager.DestroyInstance();
		Sounds.DestroyInstance();
		ToolMenu.DestroyInstance();
		WorldDamage.DestroyInstance();
		WaterCubes.DestroyInstance();
		WireBuildTool.DestroyInstance();
		VisibilityTester.DestroyInstance();
		Traces.DestroyInstance();
		TopLeftControlScreen.DestroyInstance();
		UtilityBuildTool.DestroyInstance();
		ReportScreen.DestroyInstance();
		ChorePreconditions.DestroyInstance();
		SandboxBrushTool.DestroyInstance();
		SandboxHeatTool.DestroyInstance();
		SandboxStressTool.DestroyInstance();
		SandboxCritterTool.DestroyInstance();
		SandboxClearFloorTool.DestroyInstance();
		GameScreenManager.DestroyInstance();
		GameScheduler.DestroyInstance();
		NavigationReservations.DestroyInstance();
		Tutorial.DestroyInstance();
		CameraController.DestroyInstance();
		CellEventLogger.DestroyInstance();
		GameFlowManager.DestroyInstance();
		Immigration.DestroyInstance();
		BuildTool.DestroyInstance();
		DebugTool.DestroyInstance();
		DeconstructTool.DestroyInstance();
		DisconnectTool.DestroyInstance();
		DigTool.DestroyInstance();
		DisinfectTool.DestroyInstance();
		HarvestTool.DestroyInstance();
		MopTool.DestroyInstance();
		MoveToLocationTool.DestroyInstance();
		PlaceTool.DestroyInstance();
		SpacecraftManager.DestroyInstance();
		GameplayEventManager.DestroyInstance();
		BuildingInventory.DestroyInstance();
		PlantSubSpeciesCatalog.DestroyInstance();
		SandboxDestroyerTool.DestroyInstance();
		SandboxFOWTool.DestroyInstance();
		SandboxFloodTool.DestroyInstance();
		SandboxSprinkleTool.DestroyInstance();
		StampTool.DestroyInstance();
		OnDemandUpdater.DestroyInstance();
		HoverTextScreen.DestroyInstance();
		ImmigrantScreen.DestroyInstance();
		OverlayMenu.DestroyInstance();
		NameDisplayScreen.DestroyInstance();
		PlanScreen.DestroyInstance();
		ResourceCategoryScreen.DestroyInstance();
		ResourceRemainingDisplayScreen.DestroyInstance();
		SandboxToolParameterMenu.DestroyInstance();
		SpeedControlScreen.DestroyInstance();
		Vignette.DestroyInstance();
		PlayerController.DestroyInstance();
		NotificationScreen.DestroyInstance();
		NotificationScreen_TemporaryActions.DestroyInstance();
		BuildingCellVisualizerResources.DestroyInstance();
		PauseScreen.DestroyInstance();
		SaveLoadRoot.DestroyStatics();
		KTime.DestroyInstance();
		DemoTimer.DestroyInstance();
		UIScheduler.DestroyInstance();
		SaveGame.DestroyInstance();
		GameClock.DestroyInstance();
		TimeOfDay.DestroyInstance();
		DeserializeWarnings.DestroyInstance();
		UISounds.DestroyInstance();
		RenderTextureDestroyer.DestroyInstance();
		HoverTextHelper.DestroyStatics();
		LoadScreen.DestroyInstance();
		LoadingOverlay.DestroyInstance();
		SimAndRenderScheduler.DestroyInstance();
		Singleton<CellChangeMonitor>.DestroyInstance();
		Singleton<StateMachineManager>.Instance.Clear();
		Singleton<StateMachineUpdater>.Instance.Clear();
		UpdateObjectCountParameter.Clear();
		MaterialSelectionPanel.ClearStatics();
		StarmapHexCellInventory.ClearStatics();
		StarmapScreen.DestroyInstance();
		ClusterNameDisplayScreen.DestroyInstance();
		ClusterManager.DestroyInstance();
		ClusterGrid.DestroyInstance();
		PathFinderQueries.Reset();
		KBatchedAnimUpdater instance = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance != null)
		{
			instance.InitializeGrid();
		}
		GlobalChoreProvider.DestroyInstance();
		WorldSelector.DestroyInstance();
		ColonyDiagnosticUtility.DestroyInstance();
		DiscoveredResources.DestroyInstance();
		ClusterMapSelectTool.DestroyInstance();
		StoryManager.DestroyInstance();
		AnimEventHandlerManager.DestroyInstance();
		GridRestrictionSerializer.DestroyInstance();
		EdiblesManager.ClearSaveFoodCache();
		Game.Instance = null;
		Game.BrainScheduler = null;
		Grid.OnReveal = null;
		this.VisualTunerElement = null;
		Assets.ClearOnAddPrefab();
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		(KComponentSpawn.instance.comps as GameComps).Clear();
	}

	// Token: 0x06004349 RID: 17225 RVA: 0x0017D7C4 File Offset: 0x0017B9C4
	public static bool IsDlcActiveForCurrentSave(string dlcId)
	{
		if (Game.Instance == null)
		{
			DebugUtil.DevLogError("Game.IsDlcActiveForCurrentSave can only be called when the game is running");
			return false;
		}
		if (dlcId == "" || dlcId == null)
		{
			return true;
		}
		if (DlcManager.CONTENT_ONLY_DLC_IDS.Contains(dlcId))
		{
			return DlcManager.IsContentSubscribed(dlcId);
		}
		return SaveLoader.Instance.GameInfo.dlcIds.Contains(dlcId);
	}

	// Token: 0x0600434A RID: 17226 RVA: 0x0017D828 File Offset: 0x0017BA28
	public static bool IsCorrectDlcActiveForCurrentSave(IHasDlcRestrictions restrictions)
	{
		if (Game.Instance == null)
		{
			DebugUtil.DevLogError("Game.IsCorrectDlcActiveForCurrentSave can only be called when the game is running");
			return false;
		}
		return (restrictions.GetAnyRequiredDlcIds() == null || Game.IsAnyDlcActiveForCurrentSave(restrictions.GetAnyRequiredDlcIds())) && Game.IsAllDlcActiveForCurrentSave(restrictions.GetRequiredDlcIds()) && !Game.IsAnyDlcActiveForCurrentSave(restrictions.GetForbiddenDlcIds());
	}

	// Token: 0x0600434B RID: 17227 RVA: 0x0017D880 File Offset: 0x0017BA80
	private static bool IsAllDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && !Game.IsDlcActiveForCurrentSave(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600434C RID: 17228 RVA: 0x0017D8C4 File Offset: 0x0017BAC4
	private static bool IsAnyDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return false;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && Game.IsDlcActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04002A24 RID: 10788
	private static readonly Thread MainThread = Thread.CurrentThread;

	// Token: 0x04002A25 RID: 10789
	private static readonly string NextUniqueIDKey = "NextUniqueID";

	// Token: 0x04002A26 RID: 10790
	public static string clusterId = null;

	// Token: 0x04002A27 RID: 10791
	private PlayerController playerController;

	// Token: 0x04002A28 RID: 10792
	private CameraController cameraController;

	// Token: 0x04002A29 RID: 10793
	public Action<Game.GameSaveData> OnSave;

	// Token: 0x04002A2A RID: 10794
	public Action<Game.GameSaveData> OnLoad;

	// Token: 0x04002A2B RID: 10795
	public System.Action OnSpawnComplete;

	// Token: 0x04002A2C RID: 10796
	[NonSerialized]
	public bool baseAlreadyCreated;

	// Token: 0x04002A2D RID: 10797
	[NonSerialized]
	public bool autoPrioritizeRoles;

	// Token: 0x04002A2E RID: 10798
	[NonSerialized]
	public bool advancedPersonalPriorities;

	// Token: 0x04002A2F RID: 10799
	public Game.SavedInfo savedInfo;

	// Token: 0x04002A30 RID: 10800
	public static bool quitting = false;

	// Token: 0x04002A32 RID: 10802
	public AssignmentManager assignmentManager;

	// Token: 0x04002A33 RID: 10803
	public GameObject playerPrefab;

	// Token: 0x04002A34 RID: 10804
	public GameObject screenManagerPrefab;

	// Token: 0x04002A35 RID: 10805
	public GameObject cameraControllerPrefab;

	// Token: 0x04002A37 RID: 10807
	private static Camera m_CachedCamera = null;

	// Token: 0x04002A38 RID: 10808
	public GameObject tempIntroScreenPrefab;

	// Token: 0x04002A39 RID: 10809
	public static int BlockSelectionLayerMask;

	// Token: 0x04002A3A RID: 10810
	public static int PickupableLayer;

	// Token: 0x04002A3B RID: 10811
	public static BrainScheduler BrainScheduler;

	// Token: 0x04002A3C RID: 10812
	public Element VisualTunerElement;

	// Token: 0x04002A3D RID: 10813
	public float currentFallbackSunlightIntensity;

	// Token: 0x04002A3E RID: 10814
	public RoomProber roomProber;

	// Token: 0x04002A3F RID: 10815
	public SpaceScannerNetworkManager spaceScannerNetworkManager;

	// Token: 0x04002A40 RID: 10816
	public FetchManager fetchManager;

	// Token: 0x04002A41 RID: 10817
	public EdiblesManager ediblesManager;

	// Token: 0x04002A42 RID: 10818
	public SpacecraftManager spacecraftManager;

	// Token: 0x04002A43 RID: 10819
	public UserMenu userMenu;

	// Token: 0x04002A44 RID: 10820
	public Unlocks unlocks;

	// Token: 0x04002A45 RID: 10821
	public Timelapser timelapser;

	// Token: 0x04002A46 RID: 10822
	private bool sandboxModeActive;

	// Token: 0x04002A47 RID: 10823
	public HandleVector<Game.CallbackInfo> callbackManager = new HandleVector<Game.CallbackInfo>(256);

	// Token: 0x04002A48 RID: 10824
	public List<int> callbackManagerManuallyReleasedHandles = new List<int>();

	// Token: 0x04002A49 RID: 10825
	public Game.ComplexCallbackHandleVector<int> simComponentCallbackManager = new Game.ComplexCallbackHandleVector<int>(256);

	// Token: 0x04002A4A RID: 10826
	public Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);

	// Token: 0x04002A4B RID: 10827
	public Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);

	// Token: 0x04002A4C RID: 10828
	public Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager = new Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);

	// Token: 0x04002A4D RID: 10829
	public Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback> radiationConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback>(256);

	// Token: 0x04002A4E RID: 10830
	[NonSerialized]
	public Player LocalPlayer;

	// Token: 0x04002A4F RID: 10831
	[SerializeField]
	public TextAsset maleNamesFile;

	// Token: 0x04002A50 RID: 10832
	[SerializeField]
	public TextAsset femaleNamesFile;

	// Token: 0x04002A51 RID: 10833
	[NonSerialized]
	public World world;

	// Token: 0x04002A52 RID: 10834
	[NonSerialized]
	public CircuitManager circuitManager;

	// Token: 0x04002A53 RID: 10835
	[NonSerialized]
	public EnergySim energySim;

	// Token: 0x04002A54 RID: 10836
	[NonSerialized]
	public LogicCircuitManager logicCircuitManager;

	// Token: 0x04002A55 RID: 10837
	private GameScreenManager screenMgr;

	// Token: 0x04002A56 RID: 10838
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;

	// Token: 0x04002A57 RID: 10839
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;

	// Token: 0x04002A58 RID: 10840
	public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;

	// Token: 0x04002A59 RID: 10841
	public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;

	// Token: 0x04002A5A RID: 10842
	public UtilityNetworkTubesManager travelTubeSystem;

	// Token: 0x04002A5B RID: 10843
	public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;

	// Token: 0x04002A5C RID: 10844
	public ConduitFlow gasConduitFlow;

	// Token: 0x04002A5D RID: 10845
	public ConduitFlow liquidConduitFlow;

	// Token: 0x04002A5E RID: 10846
	public SolidConduitFlow solidConduitFlow;

	// Token: 0x04002A5F RID: 10847
	public Accumulators accumulators;

	// Token: 0x04002A60 RID: 10848
	public PlantElementAbsorbers plantElementAbsorbers;

	// Token: 0x04002A61 RID: 10849
	public Game.TemperatureOverlayModes temperatureOverlayMode;

	// Token: 0x04002A62 RID: 10850
	public bool showExpandedTemperatures;

	// Token: 0x04002A63 RID: 10851
	public List<Tag> tileOverlayFilters = new List<Tag>();

	// Token: 0x04002A64 RID: 10852
	public bool showGasConduitDisease;

	// Token: 0x04002A65 RID: 10853
	public bool showLiquidConduitDisease;

	// Token: 0x04002A66 RID: 10854
	public ConduitFlowVisualizer gasFlowVisualizer;

	// Token: 0x04002A67 RID: 10855
	public ConduitFlowVisualizer liquidFlowVisualizer;

	// Token: 0x04002A68 RID: 10856
	public SolidConduitFlowVisualizer solidFlowVisualizer;

	// Token: 0x04002A69 RID: 10857
	public ConduitTemperatureManager conduitTemperatureManager;

	// Token: 0x04002A6A RID: 10858
	public ConduitDiseaseManager conduitDiseaseManager;

	// Token: 0x04002A6B RID: 10859
	public MingleCellTracker mingleCellTracker;

	// Token: 0x04002A6C RID: 10860
	private int simSubTick;

	// Token: 0x04002A6D RID: 10861
	private bool hasFirstSimTickRun;

	// Token: 0x04002A6E RID: 10862
	private float simDt;

	// Token: 0x04002A6F RID: 10863
	public string dateGenerated;

	// Token: 0x04002A70 RID: 10864
	public List<uint> changelistsPlayedOn;

	// Token: 0x04002A71 RID: 10865
	[SerializeField]
	public Game.ConduitVisInfo liquidConduitVisInfo;

	// Token: 0x04002A72 RID: 10866
	[SerializeField]
	public Game.ConduitVisInfo gasConduitVisInfo;

	// Token: 0x04002A73 RID: 10867
	[SerializeField]
	public Game.ConduitVisInfo solidConduitVisInfo;

	// Token: 0x04002A74 RID: 10868
	[SerializeField]
	private Material liquidFlowMaterial;

	// Token: 0x04002A75 RID: 10869
	[SerializeField]
	private Material gasFlowMaterial;

	// Token: 0x04002A76 RID: 10870
	[SerializeField]
	private Color flowColour;

	// Token: 0x04002A77 RID: 10871
	private Vector3 gasFlowPos;

	// Token: 0x04002A78 RID: 10872
	private Vector3 liquidFlowPos;

	// Token: 0x04002A79 RID: 10873
	private Vector3 solidFlowPos;

	// Token: 0x04002A7A RID: 10874
	public bool drawStatusItems = true;

	// Token: 0x04002A7B RID: 10875
	private List<SolidInfo> solidInfo = new List<SolidInfo>();

	// Token: 0x04002A7C RID: 10876
	private List<Klei.CallbackInfo> callbackInfo = new List<Klei.CallbackInfo>();

	// Token: 0x04002A7D RID: 10877
	private List<SolidInfo> gameSolidInfo = new List<SolidInfo>();

	// Token: 0x04002A7E RID: 10878
	private bool IsPaused;

	// Token: 0x04002A7F RID: 10879
	private HashSet<int> solidChangedFilter = new HashSet<int>();

	// Token: 0x04002A80 RID: 10880
	private HashedString lastDrawnOverlayMode;

	// Token: 0x04002A81 RID: 10881
	private EntityCellVisualizer previewVisualizer;

	// Token: 0x04002A84 RID: 10884
	public SafetyConditions safetyConditions = new SafetyConditions();

	// Token: 0x04002A85 RID: 10885
	public SimData simData = new SimData();

	// Token: 0x04002A86 RID: 10886
	[MyCmpGet]
	private GameScenePartitioner gameScenePartitioner;

	// Token: 0x04002A87 RID: 10887
	private bool gameStarted;

	// Token: 0x04002A88 RID: 10888
	private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.MarkStatusItemRendererDirty(data);
	});

	// Token: 0x04002A89 RID: 10889
	private static readonly EventSystem.IntraObjectHandler<Game> ActiveWorldChangedDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.ForceOverlayUpdate(true);
	});

	// Token: 0x04002A8A RID: 10890
	private ushort[] activeFX;

	// Token: 0x04002A8B RID: 10891
	public bool debugWasUsed;

	// Token: 0x04002A8C RID: 10892
	private bool isLoading;

	// Token: 0x04002A8D RID: 10893
	private List<Game.SimActiveRegion> simActiveRegions = new List<Game.SimActiveRegion>();

	// Token: 0x04002A8E RID: 10894
	private HashedString previousOverlayMode = OverlayModes.None.ID;

	// Token: 0x04002A8F RID: 10895
	private float previousGasConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x04002A90 RID: 10896
	private float previousLiquidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x04002A91 RID: 10897
	private float previousSolidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x04002A92 RID: 10898
	[SerializeField]
	private Game.SpawnPoolData[] fxSpawnData;

	// Token: 0x04002A93 RID: 10899
	private Dictionary<int, Action<Vector3, float>> fxSpawner = new Dictionary<int, Action<Vector3, float>>();

	// Token: 0x04002A94 RID: 10900
	private Dictionary<int, GameObjectPool> fxPools = new Dictionary<int, GameObjectPool>();

	// Token: 0x04002A95 RID: 10901
	private static Action<object> GameInitializeFXSpawners_SpawnFX = delegate(object obj)
	{
		Game.SpawnFXParams spawnFXParams = Unsafe.As<Game.SpawnFXParams>(obj);
		ushort num = (ushort)(1 << spawnFXParams.fx_idx);
		int num2 = Grid.PosToCell(spawnFXParams.pos);
		if ((Game.Instance.activeFX[num2] & num) == 0)
		{
			ushort[] array = Game.Instance.activeFX;
			int num3 = num2;
			array[num3] |= num;
			GameObject instance = spawnFXParams.pool.GetInstance();
			Game.SpawnPoolData spawnPoolData = Game.Instance.fxSpawnData[spawnFXParams.fx_idx];
			Quaternion rotation = Quaternion.identity;
			bool flipX = false;
			string s = spawnPoolData.initialAnim;
			Game.SpawnRotationConfig rotationConfig = spawnPoolData.rotationConfig;
			if (rotationConfig != Game.SpawnRotationConfig.Normal)
			{
				if (rotationConfig == Game.SpawnRotationConfig.StringName)
				{
					int num4 = (int)(spawnFXParams.rotation / 90f);
					if (num4 < 0)
					{
						num4 += spawnPoolData.rotationData.Length;
					}
					s = spawnPoolData.rotationData[num4].animName;
					flipX = spawnPoolData.rotationData[num4].flip;
				}
			}
			else
			{
				rotation = Quaternion.Euler(0f, 0f, spawnFXParams.rotation);
			}
			spawnFXParams.pos += spawnPoolData.spawnOffset;
			Vector2 vector = UnityEngine.Random.insideUnitCircle;
			vector.x *= spawnPoolData.spawnRandomOffset.x;
			vector.y *= spawnPoolData.spawnRandomOffset.y;
			vector = rotation * vector;
			Game.SpawnFXParams spawnFXParams2 = spawnFXParams;
			spawnFXParams2.pos.x = spawnFXParams2.pos.x + vector.x;
			Game.SpawnFXParams spawnFXParams3 = spawnFXParams;
			spawnFXParams3.pos.y = spawnFXParams3.pos.y + vector.y;
			instance.transform.SetPosition(spawnFXParams.pos);
			instance.transform.rotation = rotation;
			KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
			component.FlipX = flipX;
			component.TintColour = spawnPoolData.colour;
			component.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = true;
		}
		Game.SpawnFXParams.Pool.Release(spawnFXParams);
	};

	// Token: 0x04002A96 RID: 10902
	private Game.SavingPreCB activatePreCB;

	// Token: 0x04002A97 RID: 10903
	private Game.SavingActiveCB activateActiveCB;

	// Token: 0x04002A98 RID: 10904
	private Game.SavingPostCB activatePostCB;

	// Token: 0x04002A99 RID: 10905
	[SerializeField]
	public Game.UIColours uiColours = new Game.UIColours();

	// Token: 0x04002A9A RID: 10906
	private float lastTimeWorkStarted = float.NegativeInfinity;

	// Token: 0x0200194C RID: 6476
	[Serializable]
	public struct SavedInfo
	{
		// Token: 0x0600A1DD RID: 41437 RVA: 0x003AD011 File Offset: 0x003AB211
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeEmptyVariables();
		}

		// Token: 0x0600A1DE RID: 41438 RVA: 0x003AD019 File Offset: 0x003AB219
		public void InitializeEmptyVariables()
		{
			if (this.creaturePoopAmount == null)
			{
				this.creaturePoopAmount = new Dictionary<Tag, float>();
			}
			if (this.powerCreatedbyGeneratorType == null)
			{
				this.powerCreatedbyGeneratorType = new Dictionary<Tag, float>();
			}
		}

		// Token: 0x04007D69 RID: 32105
		public bool discoveredSurface;

		// Token: 0x04007D6A RID: 32106
		public bool discoveredOilField;

		// Token: 0x04007D6B RID: 32107
		public bool curedDisease;

		// Token: 0x04007D6C RID: 32108
		public bool blockedCometWithBunkerDoor;

		// Token: 0x04007D6D RID: 32109
		public Dictionary<Tag, float> creaturePoopAmount;

		// Token: 0x04007D6E RID: 32110
		public Dictionary<Tag, float> powerCreatedbyGeneratorType;
	}

	// Token: 0x0200194D RID: 6477
	public struct CallbackInfo
	{
		// Token: 0x0600A1DF RID: 41439 RVA: 0x003AD041 File Offset: 0x003AB241
		public CallbackInfo(System.Action cb, bool manually_release = false)
		{
			this.cb = cb;
			this.manuallyRelease = manually_release;
		}

		// Token: 0x04007D6F RID: 32111
		public System.Action cb;

		// Token: 0x04007D70 RID: 32112
		public bool manuallyRelease;
	}

	// Token: 0x0200194E RID: 6478
	public struct ComplexCallbackInfo<DataType>
	{
		// Token: 0x0600A1E0 RID: 41440 RVA: 0x003AD051 File Offset: 0x003AB251
		public ComplexCallbackInfo(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			this.cb = cb;
			this.debugInfo = debug_info;
			this.callbackData = callback_data;
		}

		// Token: 0x04007D71 RID: 32113
		public Action<DataType, object> cb;

		// Token: 0x04007D72 RID: 32114
		public object callbackData;

		// Token: 0x04007D73 RID: 32115
		public string debugInfo;
	}

	// Token: 0x0200194F RID: 6479
	public class ComplexCallbackHandleVector<DataType>
	{
		// Token: 0x0600A1E1 RID: 41441 RVA: 0x003AD068 File Offset: 0x003AB268
		public ComplexCallbackHandleVector(int initial_size)
		{
			this.baseMgr = new HandleVector<Game.ComplexCallbackInfo<DataType>>(initial_size);
		}

		// Token: 0x0600A1E2 RID: 41442 RVA: 0x003AD087 File Offset: 0x003AB287
		public HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle Add(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			return this.baseMgr.Add(new Game.ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
		}

		// Token: 0x0600A1E3 RID: 41443 RVA: 0x003AD09C File Offset: 0x003AB29C
		public Game.ComplexCallbackInfo<DataType> GetItem(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			Game.ComplexCallbackInfo<DataType> item;
			try
			{
				item = this.baseMgr.GetItem(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was released ...... magically", null);
				}
				throw ex;
			}
			return item;
		}

		// Token: 0x0600A1E4 RID: 41444 RVA: 0x003AD10C File Offset: 0x003AB30C
		public Game.ComplexCallbackInfo<DataType> Release(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle, string release_info)
		{
			Game.ComplexCallbackInfo<DataType> result;
			try
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandle(handle, out b, out key);
				this.releaseInfo[key] = release_info;
				result = this.baseMgr.Release(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, release_info + "is trying to release handle but it was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, release_info + "is trying to release a handle that was already released by some unknown thing", null);
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x0600A1E5 RID: 41445 RVA: 0x003AD1A0 File Offset: 0x003AB3A0
		public void Clear()
		{
			this.baseMgr.Clear();
		}

		// Token: 0x0600A1E6 RID: 41446 RVA: 0x003AD1AD File Offset: 0x003AB3AD
		public bool IsVersionValid(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			return this.baseMgr.IsVersionValid(handle);
		}

		// Token: 0x04007D74 RID: 32116
		private HandleVector<Game.ComplexCallbackInfo<DataType>> baseMgr;

		// Token: 0x04007D75 RID: 32117
		private Dictionary<int, string> releaseInfo = new Dictionary<int, string>();
	}

	// Token: 0x02001950 RID: 6480
	public enum TemperatureOverlayModes
	{
		// Token: 0x04007D77 RID: 32119
		AbsoluteTemperature,
		// Token: 0x04007D78 RID: 32120
		AdaptiveTemperature,
		// Token: 0x04007D79 RID: 32121
		HeatFlow,
		// Token: 0x04007D7A RID: 32122
		StateChange,
		// Token: 0x04007D7B RID: 32123
		RelativeTemperature
	}

	// Token: 0x02001951 RID: 6481
	[Serializable]
	public class ConduitVisInfo
	{
		// Token: 0x04007D7C RID: 32124
		public GameObject prefab;

		// Token: 0x04007D7D RID: 32125
		[Header("Main View")]
		public Color32 tint;

		// Token: 0x04007D7E RID: 32126
		public Color32 insulatedTint;

		// Token: 0x04007D7F RID: 32127
		public Color32 radiantTint;

		// Token: 0x04007D80 RID: 32128
		[Header("Overlay")]
		public string overlayTintName;

		// Token: 0x04007D81 RID: 32129
		public string overlayInsulatedTintName;

		// Token: 0x04007D82 RID: 32130
		public string overlayRadiantTintName;

		// Token: 0x04007D83 RID: 32131
		public Vector2 overlayMassScaleRange = new Vector2f(1f, 1000f);

		// Token: 0x04007D84 RID: 32132
		public Vector2 overlayMassScaleValues = new Vector2f(0.1f, 1f);
	}

	// Token: 0x02001952 RID: 6482
	private class WorldRegion
	{
		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x0600A1E8 RID: 41448 RVA: 0x003AD1F7 File Offset: 0x003AB3F7
		public Vector2I regionMin
		{
			get
			{
				return this.min;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x0600A1E9 RID: 41449 RVA: 0x003AD1FF File Offset: 0x003AB3FF
		public Vector2I regionMax
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x0600A1EA RID: 41450 RVA: 0x003AD208 File Offset: 0x003AB408
		public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1)
		{
			this.min.x = Mathf.Max(0, x0);
			this.min.y = Mathf.Max(0, y0);
			this.max.x = Mathf.Max(x1, this.regionMax.x);
			this.max.y = Mathf.Max(y1, this.regionMax.y);
		}

		// Token: 0x0600A1EB RID: 41451 RVA: 0x003AD272 File Offset: 0x003AB472
		public void UpdateGameActiveRegion(Vector2I simActiveRegionMin, Vector2I simActiveRegionMax)
		{
			this.min = simActiveRegionMin;
			this.max = simActiveRegionMax;
		}

		// Token: 0x04007D85 RID: 32133
		private Vector2I min;

		// Token: 0x04007D86 RID: 32134
		private Vector2I max;

		// Token: 0x04007D87 RID: 32135
		public bool isActive;
	}

	// Token: 0x02001953 RID: 6483
	public class SimActiveRegion
	{
		// Token: 0x0600A1ED RID: 41453 RVA: 0x003AD28A File Offset: 0x003AB48A
		public SimActiveRegion()
		{
			this.region = default(Pair<Vector2I, Vector2I>);
			this.currentSunlightIntensity = (float)FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
			this.currentCosmicRadiationIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}

		// Token: 0x04007D88 RID: 32136
		public Pair<Vector2I, Vector2I> region;

		// Token: 0x04007D89 RID: 32137
		public float currentSunlightIntensity;

		// Token: 0x04007D8A RID: 32138
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x02001954 RID: 6484
	private enum SpawnRotationConfig
	{
		// Token: 0x04007D8C RID: 32140
		Normal,
		// Token: 0x04007D8D RID: 32141
		StringName
	}

	// Token: 0x02001955 RID: 6485
	[Serializable]
	private struct SpawnRotationData
	{
		// Token: 0x04007D8E RID: 32142
		public string animName;

		// Token: 0x04007D8F RID: 32143
		public bool flip;
	}

	// Token: 0x02001956 RID: 6486
	[Serializable]
	private struct SpawnPoolData
	{
		// Token: 0x04007D90 RID: 32144
		[HashedEnum]
		public SpawnFXHashes id;

		// Token: 0x04007D91 RID: 32145
		public int initialCount;

		// Token: 0x04007D92 RID: 32146
		public Color32 colour;

		// Token: 0x04007D93 RID: 32147
		public GameObject fxPrefab;

		// Token: 0x04007D94 RID: 32148
		public string initialAnim;

		// Token: 0x04007D95 RID: 32149
		public Vector3 spawnOffset;

		// Token: 0x04007D96 RID: 32150
		public Vector2 spawnRandomOffset;

		// Token: 0x04007D97 RID: 32151
		public Game.SpawnRotationConfig rotationConfig;

		// Token: 0x04007D98 RID: 32152
		public Game.SpawnRotationData[] rotationData;
	}

	// Token: 0x02001957 RID: 6487
	private class SpawnFXParams
	{
		// Token: 0x04007D99 RID: 32153
		public GameObjectPool pool;

		// Token: 0x04007D9A RID: 32154
		public int fx_idx;

		// Token: 0x04007D9B RID: 32155
		public Vector3 pos;

		// Token: 0x04007D9C RID: 32156
		public float rotation;

		// Token: 0x04007D9D RID: 32157
		public static ObjectPool<Game.SpawnFXParams> Pool = new ObjectPool<Game.SpawnFXParams>(() => new Game.SpawnFXParams(), null, null, null, false, 128, 1024);
	}

	// Token: 0x02001958 RID: 6488
	[Serializable]
	private class Settings
	{
		// Token: 0x0600A1F0 RID: 41456 RVA: 0x003AD2E8 File Offset: 0x003AB4E8
		public Settings(Game game)
		{
			this.nextUniqueID = KPrefabID.NextUniqueID;
			this.gameID = KleiMetrics.GameID();
		}

		// Token: 0x0600A1F1 RID: 41457 RVA: 0x003AD306 File Offset: 0x003AB506
		public Settings()
		{
		}

		// Token: 0x04007D9E RID: 32158
		public int nextUniqueID;

		// Token: 0x04007D9F RID: 32159
		public int gameID;
	}

	// Token: 0x02001959 RID: 6489
	public class GameSaveData
	{
		// Token: 0x04007DA0 RID: 32160
		public ConduitFlow gasConduitFlow;

		// Token: 0x04007DA1 RID: 32161
		public ConduitFlow liquidConduitFlow;

		// Token: 0x04007DA2 RID: 32162
		public FallingWater fallingWater;

		// Token: 0x04007DA3 RID: 32163
		public UnstableGroundManager unstableGround;

		// Token: 0x04007DA4 RID: 32164
		public WorldDetailSave worldDetail;

		// Token: 0x04007DA5 RID: 32165
		public CustomGameSettings customGameSettings;

		// Token: 0x04007DA6 RID: 32166
		public StoryManager storySetings;

		// Token: 0x04007DA7 RID: 32167
		public SpaceScannerNetworkManager spaceScannerNetworkManager;

		// Token: 0x04007DA8 RID: 32168
		public bool debugWasUsed;

		// Token: 0x04007DA9 RID: 32169
		public bool autoPrioritizeRoles;

		// Token: 0x04007DAA RID: 32170
		public bool advancedPersonalPriorities;

		// Token: 0x04007DAB RID: 32171
		public Game.SavedInfo savedInfo;

		// Token: 0x04007DAC RID: 32172
		public string dateGenerated;

		// Token: 0x04007DAD RID: 32173
		public List<uint> changelistsPlayedOn;
	}

	// Token: 0x0200195A RID: 6490
	// (Invoke) Token: 0x0600A1F4 RID: 41460
	public delegate void CansaveCB();

	// Token: 0x0200195B RID: 6491
	// (Invoke) Token: 0x0600A1F8 RID: 41464
	public delegate void SavingPreCB(Game.CansaveCB cb);

	// Token: 0x0200195C RID: 6492
	// (Invoke) Token: 0x0600A1FC RID: 41468
	public delegate void SavingActiveCB();

	// Token: 0x0200195D RID: 6493
	// (Invoke) Token: 0x0600A200 RID: 41472
	public delegate void SavingPostCB();

	// Token: 0x0200195E RID: 6494
	[Serializable]
	public struct LocationColours
	{
		// Token: 0x04007DAE RID: 32174
		public Color unreachable;

		// Token: 0x04007DAF RID: 32175
		public Color invalidLocation;

		// Token: 0x04007DB0 RID: 32176
		public Color validLocation;

		// Token: 0x04007DB1 RID: 32177
		public Color requiresRole;

		// Token: 0x04007DB2 RID: 32178
		public Color unreachable_requiresRole;
	}

	// Token: 0x0200195F RID: 6495
	[Serializable]
	public class UIColours
	{
		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x0600A203 RID: 41475 RVA: 0x003AD316 File Offset: 0x003AB516
		public Game.LocationColours Dig
		{
			get
			{
				return this.digColours;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600A204 RID: 41476 RVA: 0x003AD31E File Offset: 0x003AB51E
		public Game.LocationColours Build
		{
			get
			{
				return this.buildColours;
			}
		}

		// Token: 0x04007DB3 RID: 32179
		[SerializeField]
		private Game.LocationColours digColours;

		// Token: 0x04007DB4 RID: 32180
		[SerializeField]
		private Game.LocationColours buildColours;
	}
}
