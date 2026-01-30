using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200085A RID: 2138
public static class LargeImpactorDestroyedSequence
{
	// Token: 0x06003ACB RID: 15051 RVA: 0x001484C4 File Offset: 0x001466C4
	public static Coroutine Start()
	{
		GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
		if (gameplayEventInstance != null)
		{
			LargeImpactorEvent.StatesInstance statesInstance = (LargeImpactorEvent.StatesInstance)gameplayEventInstance.smi;
			if (statesInstance != null && statesInstance.impactorInstance != null)
			{
				LargeImpactorCrashStamp component = statesInstance.impactorInstance.GetComponent<LargeImpactorCrashStamp>();
				return component.StartCoroutine(LargeImpactorDestroyedSequence.Sequence(component, statesInstance.eventInstance.worldId));
			}
		}
		return null;
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x00148539 File Offset: 0x00146739
	private static IEnumerator Sequence(KMonoBehaviour controller, int worldID)
	{
		yield return null;
		WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
		ParallaxBackgroundObject parallaxBackgroundObj = controller.GetComponent<ParallaxBackgroundObject>();
		GameObject telepad = GameUtil.GetTelepad(worldID);
		int centredCell = 0;
		if (telepad != null)
		{
			centredCell = Grid.PosToCell(telepad);
		}
		else
		{
			Vector2 pos = world.WorldOffset * Grid.CellSizeInMeters;
			pos.x += (float)world.Width * Grid.CellSizeInMeters * 0.5f;
			pos.y += (float)world.Height * Grid.CellSizeInMeters * 0.5f;
			centredCell = Grid.PosToCell(pos);
		}
		int cell = Grid.XYToCell(Grid.CellToXY(centredCell).x, world.WorldOffset.y + world.Height);
		int num = centredCell;
		int midSkyCell = Grid.InvalidCell;
		int num2 = Grid.InvalidCell;
		while (num2 == Grid.InvalidCell && Grid.CellToXY(num).y < world.WorldOffset.y + world.Height)
		{
			if (Grid.IsCellBiomeSpaceBiome(num))
			{
				num2 = num;
				break;
			}
			num = Grid.CellAbove(num);
		}
		midSkyCell = Grid.XYToCell(Grid.CellToXY(centredCell).x, (int)((float)(Grid.CellToXY(cell).y + Grid.CellToXY(num2).y) * 0.5f));
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		RootMenu.Instance.canTogglePauseScreen = false;
		CameraController.Instance.DisableUserCameraControl = true;
		CameraController.Instance.SetWorldInteractive(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
		ManagementMenu.Instance.CloseAll();
		StoryMessageScreen.HideInterface(true);
		OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
		CameraController.Instance.SetOverrideZoomSpeed(0.6f);
		yield return null;
		CameraController.Instance.FadeIn(0f, 1f, null);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS", false);
		KFMOD.PlayUISound(GlobalAssets.GetSound("Asteroid_destroyed_start", false));
		CameraController.Instance.SetTargetPos(Grid.CellToPos(midSkyCell), 20f, false);
		yield return SequenceUtil.WaitForSecondsRealtime(4f);
		parallaxBackgroundObj.PlayExplosion();
		yield return SequenceUtil.WaitForSecondsRealtime(2.2f);
		TerrainBG.preventLargeImpactorFragmentsFromProgressing = false;
		bool fadeOutCompleted = false;
		CameraController.Instance.FadeOutColor(Color.white, 0f, 1f, 1f, delegate()
		{
			fadeOutCompleted = true;
		});
		yield return new WaitUntil(() => fadeOutCompleted);
		MissileLauncher.Instance instance = null;
		float num3 = float.MaxValue;
		Vector3 position = CameraController.Instance.overlayCamera.transform.position;
		position.z = 0f;
		foreach (object obj in Components.MissileLaunchers)
		{
			MissileLauncher.Instance instance2 = (MissileLauncher.Instance)obj;
			if (instance2 != null && instance2.GetMyWorldId() == worldID)
			{
				Vector3 position2 = instance2.transform.position;
				position2.z = 0f;
				float magnitude = (position - position2).magnitude;
				if (magnitude < num3)
				{
					num3 = magnitude;
					instance = instance2;
				}
			}
		}
		int num4 = Grid.InvalidCell;
		int num5 = Grid.InvalidCell;
		bool flag = instance != null;
		if (flag)
		{
			num5 = Grid.PosToCell(instance.gameObject);
		}
		else
		{
			num4 = Grid.XYToCell(Grid.CellToXY(centredCell).x, world.WorldOffset.y + world.Height);
			num5 = num4;
		}
		if (flag)
		{
			int num6 = num5;
			int y = CameraController.Instance.VisibleArea.CurrentArea.Max.Y;
			while (Grid.CellToXY(num6).y < y)
			{
				int num7 = Grid.CellAbove(num6);
				if (!Grid.IsValidCellInWorld(num7, worldID) || Grid.Solid[num7])
				{
					break;
				}
				num6 = num7;
			}
			num4 = num6;
		}
		LargeImpactorDestroyedSequence.SpawnKeepsake(Grid.CellToPos(num4));
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		MusicManager.instance.StopSong("Music_Victory_02_NIS", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot, STOP_MODE.ALLOWFADEOUT);
		yield return null;
		bool videoCompleted = false;
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		VideoScreen screen = null;
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		screen = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<VideoScreen>();
		screen.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.AsteroidDestroyed.shortVideoName), true, AudioMixerSnapshots.Get().VictoryNISGenericSnapshot, false, true);
		screen.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.AsteroidDestroyed.messageBody, Db.Get().ColonyAchievements.AsteroidDestroyed.Id, Db.Get().ColonyAchievements.AsteroidDestroyed.loopVideoName, true, false);
		System.Action onVideoCompletedCallback = delegate()
		{
			videoCompleted = true;
		};
		VideoScreen videoScreen = screen;
		videoScreen.OnStop = (System.Action)Delegate.Combine(videoScreen.OnStop, onVideoCompletedCallback);
		yield return new WaitUntil(() => videoCompleted);
		VideoScreen videoScreen2 = screen;
		videoScreen2.OnStop = (System.Action)Delegate.Remove(videoScreen2.OnStop, onVideoCompletedCallback);
		SpeedControlScreen.Instance.SetSpeed(0);
		CameraController.Instance.FadeIn(0f, 1f, null);
		CameraController.Instance.SetOverrideZoomSpeed(1f);
		CameraController.Instance.SetWorldInteractive(true);
		CameraController.Instance.DisableUserCameraControl = false;
		CameraController.Instance.SetMaxOrthographicSize(20f);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot, STOP_MODE.ALLOWFADEOUT);
		RootMenu.Instance.canTogglePauseScreen = true;
		HoverTextScreen.Instance.Show(true);
		StoryMessageScreen.HideInterface(false);
		Game.Instance.Subscribe(-821118536, new Action<object>(LargeImpactorDestroyedSequence.OnScreenClosed));
		controller.Trigger(-467702038, null);
		yield break;
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x0014854F File Offset: 0x0014674F
	private static void OnScreenClosed(object screenData)
	{
		if (screenData != null && screenData is RetiredColonyInfoScreen)
		{
			LargeImpactorDestroyedSequence.OnAchievementScreenClosed();
		}
	}

	// Token: 0x06003ACE RID: 15054 RVA: 0x00148564 File Offset: 0x00146764
	private static void OnAchievementScreenClosed()
	{
		if (SpeedControlScreen.Instance != null && SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
			SpeedControlScreen.Instance.SetSpeed(0);
		}
		Game.Instance.Unsubscribe(-821118536, new Action<object>(LargeImpactorDestroyedSequence.OnScreenClosed));
	}

	// Token: 0x06003ACF RID: 15055 RVA: 0x001485BC File Offset: 0x001467BC
	private static void SpawnKeepsake(Vector3 position)
	{
		GameObject prefab = Assets.GetPrefab("keepsake_largeimpactor");
		if (prefab != null)
		{
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			GameObject gameObject = global::Util.KInstantiate(prefab, position);
			gameObject.SetActive(true);
			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
		}
	}

	// Token: 0x040023C1 RID: 9153
	private const string SongName = "Music_Victory_02_NIS";

	// Token: 0x040023C2 RID: 9154
	private const string Sound_Destroyed_Victory_Start_Sequence = "Asteroid_destroyed_start";
}
