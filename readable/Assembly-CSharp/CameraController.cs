using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Token: 0x020005A2 RID: 1442
[AddComponentMenu("KMonoBehaviour/scripts/CameraController")]
public class CameraController : KMonoBehaviour, IInputHandler
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x0600207C RID: 8316 RVA: 0x000BB47C File Offset: 0x000B967C
	public string handlerName
	{
		get
		{
			return base.gameObject.name;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x0600207D RID: 8317 RVA: 0x000BB489 File Offset: 0x000B9689
	// (set) Token: 0x0600207E RID: 8318 RVA: 0x000BB4AC File Offset: 0x000B96AC
	public float OrthographicSize
	{
		get
		{
			if (!(this.baseCamera == null))
			{
				return this.baseCamera.orthographicSize;
			}
			return 0f;
		}
		set
		{
			for (int i = 0; i < this.cameras.Count; i++)
			{
				this.cameras[i].orthographicSize = value;
			}
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x0600207F RID: 8319 RVA: 0x000BB4E1 File Offset: 0x000B96E1
	// (set) Token: 0x06002080 RID: 8320 RVA: 0x000BB4E9 File Offset: 0x000B96E9
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06002081 RID: 8321 RVA: 0x000BB4F2 File Offset: 0x000B96F2
	// (set) Token: 0x06002082 RID: 8322 RVA: 0x000BB4FA File Offset: 0x000B96FA
	public float targetOrthographicSize { get; private set; }

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06002083 RID: 8323 RVA: 0x000BB503 File Offset: 0x000B9703
	// (set) Token: 0x06002084 RID: 8324 RVA: 0x000BB50B File Offset: 0x000B970B
	public bool isTargetPosSet { get; set; }

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06002085 RID: 8325 RVA: 0x000BB514 File Offset: 0x000B9714
	// (set) Token: 0x06002086 RID: 8326 RVA: 0x000BB51C File Offset: 0x000B971C
	public Vector3 targetPos { get; private set; }

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06002087 RID: 8327 RVA: 0x000BB525 File Offset: 0x000B9725
	// (set) Token: 0x06002088 RID: 8328 RVA: 0x000BB52D File Offset: 0x000B972D
	public bool ignoreClusterFX { get; private set; }

	// Token: 0x06002089 RID: 8329 RVA: 0x000BB536 File Offset: 0x000B9736
	public void ToggleClusterFX()
	{
		this.ignoreClusterFX = !this.ignoreClusterFX;
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x000BB548 File Offset: 0x000B9748
	protected override void OnForcedCleanUp()
	{
		GameInputManager inputManager = Global.GetInputManager();
		if (inputManager == null)
		{
			return;
		}
		inputManager.usedMenus.Remove(this);
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x0600208B RID: 8331 RVA: 0x000BB56C File Offset: 0x000B976C
	public int cameraActiveCluster
	{
		get
		{
			if (ClusterManager.Instance == null)
			{
				return 255;
			}
			return ClusterManager.Instance.activeWorldId;
		}
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x000BB58C File Offset: 0x000B978C
	public void GetWorldCamera(out Vector2I worldOffset, out Vector2I worldSize)
	{
		WorldContainer worldContainer = null;
		if (ClusterManager.Instance != null)
		{
			worldContainer = ClusterManager.Instance.activeWorld;
		}
		if (!this.ignoreClusterFX && worldContainer != null)
		{
			worldOffset = worldContainer.WorldOffset;
			worldSize = worldContainer.WorldSize;
			return;
		}
		worldOffset = new Vector2I(0, 0);
		worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x0600208D RID: 8333 RVA: 0x000BB5FF File Offset: 0x000B97FF
	// (set) Token: 0x0600208E RID: 8334 RVA: 0x000BB607 File Offset: 0x000B9807
	public bool DisableUserCameraControl
	{
		get
		{
			return this.userCameraControlDisabled;
		}
		set
		{
			this.userCameraControlDisabled = value;
			if (this.userCameraControlDisabled)
			{
				this.panning = false;
				this.panLeft = false;
				this.panRight = false;
				this.panUp = false;
				this.panDown = false;
			}
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x0600208F RID: 8335 RVA: 0x000BB63B File Offset: 0x000B983B
	// (set) Token: 0x06002090 RID: 8336 RVA: 0x000BB642 File Offset: 0x000B9842
	public static CameraController Instance { get; private set; }

	// Token: 0x06002091 RID: 8337 RVA: 0x000BB64A File Offset: 0x000B984A
	public static void DestroyInstance()
	{
		CameraController.Instance = null;
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x000BB652 File Offset: 0x000B9852
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.mrt.ToggleColouredOverlayView(enabled);
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x000BB660 File Offset: 0x000B9860
	public void EnableKAnimPostProcessingEffect(KAnimConverter.PostProcessingEffects effect)
	{
		this.kAnimPostProcessingEffects.EnableEffect(effect);
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x000BB66E File Offset: 0x000B986E
	public void DisableKAnimPostProcessingEffect(KAnimConverter.PostProcessingEffects effect)
	{
		this.kAnimPostProcessingEffects.DisableEffect(effect);
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x000BB67C File Offset: 0x000B987C
	public void RegisterCustomScreenPostProcessingEffect(Func<RenderTexture, Material> effectFn)
	{
		this.customActiveScreenPostProcessingEffects.RegisterEffect(effectFn);
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x000BB68A File Offset: 0x000B988A
	public void UnregisterCustomScreenPostProcessingEffect(Func<RenderTexture, Material> effectFn)
	{
		this.customActiveScreenPostProcessingEffects.UnregisterEffect(effectFn);
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x000BB698 File Offset: 0x000B9898
	protected override void OnPrefabInit()
	{
		global::Util.Reset(base.transform);
		base.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -100f));
		this.targetOrthographicSize = this.maxOrthographicSize;
		CameraController.Instance = this;
		this.DisableUserCameraControl = false;
		this.baseCamera = this.CopyCamera(Camera.main, "baseCamera");
		this.mrt = this.baseCamera.gameObject.AddComponent<MultipleRenderTarget>();
		this.mrt.onSetupComplete += this.OnMRTSetupComplete;
		this.baseCamera.gameObject.AddComponent<LightBufferCompositor>();
		this.baseCamera.transparencySortMode = TransparencySortMode.Orthographic;
		this.baseCamera.transform.parent = base.transform;
		global::Util.Reset(this.baseCamera.transform);
		int mask = LayerMask.GetMask(new string[]
		{
			"PlaceWithDepth",
			"Overlay"
		});
		int mask2 = LayerMask.GetMask(new string[]
		{
			"Construction"
		});
		this.baseCamera.cullingMask &= ~mask;
		this.baseCamera.cullingMask |= mask2;
		this.baseCamera.tag = "Untagged";
		this.baseCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_LitTex";
		this.infraredCamera = this.CopyCamera(this.baseCamera, "Infrared");
		this.infraredCamera.cullingMask = 0;
		this.infraredCamera.clearFlags = CameraClearFlags.Color;
		this.infraredCamera.depth = this.baseCamera.depth - 1f;
		this.infraredCamera.transform.parent = base.transform;
		this.infraredCamera.gameObject.AddComponent<Infrared>();
		if (SimDebugView.Instance != null)
		{
			this.simOverlayCamera = this.CopyCamera(this.baseCamera, "SimOverlayCamera");
			this.simOverlayCamera.cullingMask = LayerMask.GetMask(new string[]
			{
				"SimDebugView"
			});
			this.simOverlayCamera.clearFlags = CameraClearFlags.Color;
			this.simOverlayCamera.depth = this.baseCamera.depth + 1f;
			this.simOverlayCamera.transform.parent = base.transform;
			this.simOverlayCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_SimDebugViewTex";
		}
		this.overlayCamera = Camera.main;
		this.overlayCamera.name = "Overlay";
		this.overlayCamera.cullingMask = (mask | mask2);
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.overlayCamera.transform.parent = base.transform;
		this.overlayCamera.depth = this.baseCamera.depth + 3f;
		this.overlayCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayCamera.transform.localRotation = Quaternion.identity;
		this.overlayCamera.renderingPath = RenderingPath.Forward;
		this.overlayCamera.allowHDR = false;
		this.overlayCamera.tag = "Untagged";
		this.kAnimPostProcessingEffects = this.overlayCamera.GetComponent<KAnimActivePostProcessingEffects>();
		this.customActiveScreenPostProcessingEffects = this.overlayCamera.GetComponent<CustomActiveScreenPostProcessingEffects>();
		this.overlayCamera.gameObject.AddComponent<CameraReferenceTexture>().referenceCamera = this.baseCamera;
		ColorCorrectionLookup component = this.overlayCamera.GetComponent<ColorCorrectionLookup>();
		component.Convert(this.dayColourCube, "");
		component.Convert2(this.nightColourCube, "");
		this.cameras.Add(this.overlayCamera);
		this.lightBufferCamera = this.CopyCamera(this.overlayCamera, "Light Buffer");
		this.lightBufferCamera.clearFlags = CameraClearFlags.Color;
		this.lightBufferCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"Lights"
		});
		this.lightBufferCamera.depth = this.baseCamera.depth - 1f;
		this.lightBufferCamera.transform.parent = base.transform;
		this.lightBufferCamera.transform.SetLocalPosition(Vector3.zero);
		this.lightBufferCamera.rect = new Rect(0f, 0f, 1f, 1f);
		LightBuffer lightBuffer = this.lightBufferCamera.gameObject.AddComponent<LightBuffer>();
		lightBuffer.Material = this.LightBufferMaterial;
		lightBuffer.CircleMaterial = this.LightCircleOverlay;
		lightBuffer.ConeMaterial = this.LightConeOverlay;
		this.overlayNoDepthCamera = this.CopyCamera(this.overlayCamera, "overlayNoDepth");
		int mask3 = LayerMask.GetMask(new string[]
		{
			"Overlay",
			"Place"
		});
		this.baseCamera.cullingMask &= ~mask3;
		this.overlayNoDepthCamera.clearFlags = CameraClearFlags.Depth;
		this.overlayNoDepthCamera.cullingMask = mask3;
		this.overlayNoDepthCamera.transform.parent = base.transform;
		this.overlayNoDepthCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayNoDepthCamera.depth = this.baseCamera.depth + 4f;
		this.overlayNoDepthCamera.tag = "MainCamera";
		this.overlayNoDepthCamera.gameObject.AddComponent<NavPathDrawer>();
		this.overlayNoDepthCamera.gameObject.AddComponent<RangeVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<SkyVisibilityVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<ScannerNetworkVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<RocketLaunchConditionVisualizerEffect>();
		if (DlcManager.IsContentSubscribed("DLC4_ID"))
		{
			this.overlayNoDepthCamera.gameObject.AddComponent<LargeImpactorVisualizerEffect>();
		}
		this.uiCamera = this.CopyCamera(this.overlayCamera, "uiCamera");
		this.uiCamera.clearFlags = CameraClearFlags.Depth;
		this.uiCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"UI"
		});
		this.uiCamera.transform.parent = base.transform;
		this.uiCamera.transform.SetLocalPosition(Vector3.zero);
		this.uiCamera.depth = this.baseCamera.depth + 5f;
		if (Game.Instance != null)
		{
			this.timelapseFreezeCamera = this.CopyCamera(this.uiCamera, "timelapseFreezeCamera");
			this.timelapseFreezeCamera.depth = this.uiCamera.depth + 3f;
			this.timelapseFreezeCamera.gameObject.AddComponent<FillRenderTargetEffect>();
			this.timelapseFreezeCamera.enabled = false;
			Camera camera = CameraController.CloneCamera(this.overlayCamera, "timelapseCamera");
			Timelapser timelapser = camera.gameObject.AddComponent<Timelapser>();
			camera.transparencySortMode = TransparencySortMode.Orthographic;
			camera.depth = this.baseCamera.depth + 2f;
			Game.Instance.timelapser = timelapser;
		}
		if (GameScreenManager.Instance != null)
		{
			for (int i = 0; i < this.uiCameraTargets.Count; i++)
			{
				GameScreenManager.Instance.SetCamera(this.uiCameraTargets[i], this.uiCamera);
			}
			this.infoText = GameScreenManager.Instance.screenshotModeCanvas.GetComponentInChildren<LocText>();
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
		this.SetSpeedFromPrefs(null);
		Game.Instance.Subscribe(75424175, new Action<object>(this.SetSpeedFromPrefs));
		this.VisibleArea.Update();
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x000BBE0D File Offset: 0x000BA00D
	private void SetSpeedFromPrefs(object data = null)
	{
		this.keyPanningSpeed = Mathf.Clamp(0.1f, KPlayerPrefs.GetFloat("CameraSpeed"), 2f);
	}

	// Token: 0x06002099 RID: 8345 RVA: 0x000BBE30 File Offset: 0x000BA030
	public int GetCursorCell()
	{
		Vector3 rhs = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		Vector3 vector = Vector3.Max(ClusterManager.Instance.activeWorld.minimumBounds, rhs);
		vector = Vector3.Min(ClusterManager.Instance.activeWorld.maximumBounds, vector);
		return Grid.PosToCell(vector);
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x000BBE89 File Offset: 0x000BA089
	public static Camera CloneCamera(Camera camera, string name)
	{
		Camera camera2 = new GameObject
		{
			name = name
		}.AddComponent<Camera>();
		camera2.CopyFrom(camera);
		return camera2;
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x000BBEA4 File Offset: 0x000BA0A4
	private Camera CopyCamera(Camera camera, string name)
	{
		Camera camera2 = CameraController.CloneCamera(camera, name);
		this.cameras.Add(camera2);
		return camera2;
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x000BBEC6 File Offset: 0x000BA0C6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Restore();
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x000BBED4 File Offset: 0x000BA0D4
	public static void SetDefaultCameraSpeed()
	{
		KPlayerPrefs.SetFloat("CameraSpeed", 1f);
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600209E RID: 8350 RVA: 0x000BBEE5 File Offset: 0x000BA0E5
	// (set) Token: 0x0600209F RID: 8351 RVA: 0x000BBEED File Offset: 0x000BA0ED
	public Coroutine activeFadeRoutine { get; private set; }

	// Token: 0x060020A0 RID: 8352 RVA: 0x000BBEF6 File Offset: 0x000BA0F6
	public void FadeOut(float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 0f, targetPercentage, speed, callback));
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x000BBF27 File Offset: 0x000BA127
	public void FadeIn(float targetPercentage = 0f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 1f, targetPercentage, speed, callback));
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x000BBF58 File Offset: 0x000BA158
	public void FadeOutColor(Color color, float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
	{
		this.FadeOutColor(color, 1f, targetPercentage, speed, callback);
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x000BBF6C File Offset: 0x000BA16C
	public void FadeOutColor(Color color, float initialPercentage, float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithColor(true, initialPercentage, targetPercentage, color, speed, callback));
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x000BBFA8 File Offset: 0x000BA1A8
	public void FadeInColor(Color color, float targetPercentage = 0f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithColor(true, 1f, targetPercentage, color, speed, callback));
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x000BBFE8 File Offset: 0x000BA1E8
	public void ActiveWorldStarWipe(int id, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, false, default(Vector3), 10f, callback);
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x000BC00C File Offset: 0x000BA20C
	public void ActiveWorldStarWipe(int id, Vector3 position, float forceOrthgraphicSize = 10f, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, true, position, forceOrthgraphicSize, callback);
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x000BC01C File Offset: 0x000BA21C
	private void ActiveWorldStarWipe(int id, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action callback)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		if (ClusterManager.Instance.activeWorldId != id)
		{
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.DeselectAndClose();
			}
			this.activeFadeRoutine = base.StartCoroutine(this.SwapToWorldFade(id, useForcePosition, forcePosition, forceOrthgraphicSize, callback));
			return;
		}
		ManagementMenu.Instance.CloseAll();
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, 8f, true);
			if (callback != null)
			{
				callback();
			}
		}
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x000BC0A4 File Offset: 0x000BA2A4
	private IEnumerator SwapToWorldFade(int worldId, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action newWorldCallback)
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot);
		ClusterManager.Instance.UpdateWorldReverbSnapshot(worldId);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 0f, 1f, 3f, null));
		ClusterManager.Instance.SetActiveWorld(worldId);
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, forceOrthgraphicSize, false);
			CameraController.Instance.SetPosition(forcePosition);
		}
		if (newWorldCallback != null)
		{
			newWorldCallback();
		}
		ManagementMenu.Instance.CloseAll();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 1f, 0f, 3f, null));
		yield break;
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x000BC0D8 File Offset: 0x000BA2D8
	public void SetWorldInteractive(bool state)
	{
		GameScreenManager.Instance.fadePlaneFront.raycastTarget = !state;
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000BC0ED File Offset: 0x000BA2ED
	private IEnumerator FadeWithBlack(bool fadeUI, float startBlackPercent, float targetBlackPercent, float speed = 1f, System.Action callback = null)
	{
		return this.FadeWithColor(fadeUI, startBlackPercent, targetBlackPercent, Color.black, speed, callback);
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x000BC101 File Offset: 0x000BA301
	private IEnumerator FadeWithColor(bool fadeUI, float startPercent, float targetPercent, Color color, float speed = 1f, System.Action callback = null)
	{
		Image fadePlane = fadeUI ? GameScreenManager.Instance.fadePlaneFront : GameScreenManager.Instance.fadePlaneBack;
		float percent = 0f;
		while (percent < 1f)
		{
			percent += Time.unscaledDeltaTime * speed;
			float a = MathUtil.ReRange(percent, 0f, 1f, startPercent, targetPercent);
			color.a = a;
			fadePlane.color = color;
			yield return SequenceUtil.WaitForNextFrame;
		}
		color.a = targetPercent;
		fadePlane.color = color;
		if (callback != null)
		{
			callback();
		}
		this.activeFadeRoutine = null;
		yield return SequenceUtil.WaitForNextFrame;
		yield break;
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x000BC13D File Offset: 0x000BA33D
	public void EnableFreeCamera(bool enable)
	{
		this.FreeCameraEnabled = enable;
		this.SetInfoText("Screenshot Mode (ESC to exit)");
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x000BC154 File Offset: 0x000BA354
	private static bool WithinInputField()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null)
		{
			return false;
		}
		bool result = false;
		if (current.currentSelectedGameObject != null && (current.currentSelectedGameObject.GetComponent<KInputTextField>() != null || current.currentSelectedGameObject.GetComponent<InputField>() != null))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060020AE RID: 8366 RVA: 0x000BC1AC File Offset: 0x000BA3AC
	public static bool IsMouseOverGameWindow
	{
		get
		{
			return 0f <= Input.mousePosition.x && 0f <= Input.mousePosition.y && (float)Screen.width >= Input.mousePosition.x && (float)Screen.height >= Input.mousePosition.y;
		}
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x000BC204 File Offset: 0x000BA404
	private void SetInfoText(string text)
	{
		this.infoText.text = text;
		Color color = this.infoText.color;
		color.a = 0.5f;
		this.infoText.color = color;
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x000BC244 File Offset: 0x000BA444
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (SaveGame.Instance != null && SaveGame.Instance.GetComponent<UserNavigation>().Handle(e))
		{
			return;
		}
		if (!this.ChangeWorldInput(e))
		{
			if (e.TryConsume(global::Action.TogglePause))
			{
				SpeedControlScreen.Instance.TogglePause(false);
			}
			else if (e.TryConsume(global::Action.ZoomIn) && CameraController.IsMouseOverGameWindow)
			{
				float a = this.targetOrthographicSize * (1f / this.zoomFactor);
				this.targetOrthographicSize = Mathf.Max(a, this.minOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.ZoomOut) && CameraController.IsMouseOverGameWindow)
			{
				float a2 = this.targetOrthographicSize * this.zoomFactor;
				this.targetOrthographicSize = Mathf.Min(a2, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
			{
				this.panning = true;
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (this.FreeCameraEnabled && e.TryConsume(global::Action.CinemaCamEnable))
			{
				this.cinemaCamEnabled = !this.cinemaCamEnabled;
				DebugUtil.LogArgs(new object[]
				{
					"Cinema Cam Enabled ",
					this.cinemaCamEnabled
				});
				this.SetInfoText(this.cinemaCamEnabled ? "Cinema Cam Enabled" : "Cinema Cam Disabled");
			}
			else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				if (e.TryConsume(global::Action.CinemaToggleLock))
				{
					this.cinemaToggleLock = !this.cinemaToggleLock;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Lock ",
						this.cinemaToggleLock
					});
					this.SetInfoText(this.cinemaToggleLock ? "Cinema Input Lock ON" : "Cinema Input Lock OFF");
				}
				else if (e.TryConsume(global::Action.CinemaToggleEasing))
				{
					this.cinemaToggleEasing = !this.cinemaToggleEasing;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Easing ",
						this.cinemaToggleEasing
					});
					this.SetInfoText(this.cinemaToggleEasing ? "Cinema Easing ON" : "Cinema Easing OFF");
				}
				else if (e.TryConsume(global::Action.CinemaUnpauseOnMove))
				{
					this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Unpause Next Move ",
						this.cinemaUnpauseNextMove
					});
					this.SetInfoText(this.cinemaUnpauseNextMove ? "Cinema Unpause Next Move ON" : "Cinema Unpause Next Move OFF");
				}
				else if (e.TryConsume(global::Action.CinemaPanLeft))
				{
					this.cinemaPanLeft = (!this.cinemaToggleLock || !this.cinemaPanLeft);
					this.cinemaPanRight = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanRight))
				{
					this.cinemaPanRight = (!this.cinemaToggleLock || !this.cinemaPanRight);
					this.cinemaPanLeft = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanUp))
				{
					this.cinemaPanUp = (!this.cinemaToggleLock || !this.cinemaPanUp);
					this.cinemaPanDown = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanDown))
				{
					this.cinemaPanDown = (!this.cinemaToggleLock || !this.cinemaPanDown);
					this.cinemaPanUp = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomIn))
				{
					this.cinemaZoomIn = (!this.cinemaToggleLock || !this.cinemaZoomIn);
					this.cinemaZoomOut = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomOut))
				{
					this.cinemaZoomOut = (!this.cinemaToggleLock || !this.cinemaZoomOut);
					this.cinemaZoomIn = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedPlus))
				{
					this.cinemaZoomSpeed++;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedMinus))
				{
					this.cinemaZoomSpeed--;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
			}
			else if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
			}
			else if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
			}
			else if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
			}
			else if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
			}
		}
		if (!e.Consumed && OverlayMenu.Instance != null)
		{
			OverlayMenu.Instance.OnKeyDown(e);
		}
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x000BC79C File Offset: 0x000BA99C
	public bool ChangeWorldInput(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return true;
		}
		int num = -1;
		if (e.TryConsume(global::Action.SwitchActiveWorld1))
		{
			num = 0;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld2))
		{
			num = 1;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld3))
		{
			num = 2;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld4))
		{
			num = 3;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld5))
		{
			num = 4;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld6))
		{
			num = 5;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld7))
		{
			num = 6;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld8))
		{
			num = 7;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld9))
		{
			num = 8;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld10))
		{
			num = 9;
		}
		if (num != -1)
		{
			List<int> discoveredAsteroidIDsSorted = ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();
			if (num < discoveredAsteroidIDsSorted.Count && num >= 0)
			{
				num = discoveredAsteroidIDsSorted[num];
				WorldContainer world = ClusterManager.Instance.GetWorld(num);
				if (world != null && world.IsDiscovered && ClusterManager.Instance.activeWorldId != world.id)
				{
					ManagementMenu.Instance.CloseClusterMap();
					this.ActiveWorldStarWipe(world.id, null);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x000BC8D4 File Offset: 0x000BAAD4
	public void OnKeyUp(KButtonEvent e)
	{
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
		{
			this.panning = false;
			return;
		}
		if (this.FreeCameraEnabled && this.cinemaCamEnabled)
		{
			if (e.TryConsume(global::Action.CinemaPanLeft))
			{
				this.cinemaPanLeft = (this.cinemaToggleLock && this.cinemaPanLeft);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanRight))
			{
				this.cinemaPanRight = (this.cinemaToggleLock && this.cinemaPanRight);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanUp))
			{
				this.cinemaPanUp = (this.cinemaToggleLock && this.cinemaPanUp);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanDown))
			{
				this.cinemaPanDown = (this.cinemaToggleLock && this.cinemaPanDown);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomIn))
			{
				this.cinemaZoomIn = (this.cinemaToggleLock && this.cinemaZoomIn);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomOut))
			{
				this.cinemaZoomOut = (this.cinemaToggleLock && this.cinemaZoomOut);
				return;
			}
		}
		else
		{
			if (e.TryConsume(global::Action.CameraHome))
			{
				this.CameraGoHome(2f, true);
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
			}
		}
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x000BCA70 File Offset: 0x000BAC70
	public void ForcePanningState(bool state)
	{
		this.panning = false;
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x000BCA7C File Offset: 0x000BAC7C
	public void CameraGoHome(float speed = 2f, bool showCameraReturnButton = false)
	{
		GameObject activeTelepad = GameUtil.GetActiveTelepad();
		if (activeTelepad != null && ClusterUtil.ActiveWorldHasPrinter())
		{
			GameUtil.FocusCamera(new Vector3(activeTelepad.transform.GetPosition().x, activeTelepad.transform.GetPosition().y + 1f, base.transform.GetPosition().z), speed, true, showCameraReturnButton);
			this.SetOverrideZoomSpeed(speed);
		}
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x000BCAE9 File Offset: 0x000BACE9
	public void CameraGoTo(Vector3 pos, float speed = 2f, bool playSound = true)
	{
		pos.z = base.transform.GetPosition().z;
		this.SetTargetPos(pos, 10f, playSound);
		this.SetOverrideZoomSpeed(speed);
	}

	// Token: 0x060020B6 RID: 8374 RVA: 0x000BCB18 File Offset: 0x000BAD18
	public void SnapTo(Vector3 pos)
	{
		this.ClearFollowTarget();
		pos.z = -100f;
		this.targetPos = Vector3.zero;
		this.isTargetPosSet = false;
		base.transform.SetPosition(pos);
		this.keyPanDelta = Vector3.zero;
		this.OrthographicSize = this.targetOrthographicSize;
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x000BCB6D File Offset: 0x000BAD6D
	public void SnapTo(Vector3 pos, float orthographicSize)
	{
		this.targetOrthographicSize = orthographicSize;
		this.SnapTo(pos);
	}

	// Token: 0x060020B8 RID: 8376 RVA: 0x000BCB7D File Offset: 0x000BAD7D
	public void SetOverrideZoomSpeed(float tempZoomSpeed)
	{
		this.overrideZoomSpeed = tempZoomSpeed;
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000BCB88 File Offset: 0x000BAD88
	public void SetTargetPos(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		if ((int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			this.targetOrthographicSize = 20f;
			this.ActiveWorldStarWipe((int)Grid.WorldIdx[num], pos, 10f, delegate()
			{
				this.targetPos = pos;
				this.isTargetPosSet = true;
				this.OrthographicSize = orthographic_size + 5f;
				this.targetOrthographicSize = orthographic_size;
			});
		}
		else
		{
			this.targetPos = pos;
			this.isTargetPosSet = true;
			this.targetOrthographicSize = orthographic_size;
		}
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x000BCC90 File Offset: 0x000BAE90
	public void SetTargetPosForWorldChange(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		this.targetPos = pos;
		this.isTargetPosSet = true;
		this.targetOrthographicSize = orthographic_size;
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
		this.SetPosition(pos);
		this.OrthographicSize = orthographic_size;
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x000BCD34 File Offset: 0x000BAF34
	public void SetMaxOrthographicSize(float size)
	{
		this.maxOrthographicSize = size;
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x000BCD3D File Offset: 0x000BAF3D
	public void SetPosition(Vector3 pos)
	{
		base.transform.SetPosition(pos);
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x000BCD4C File Offset: 0x000BAF4C
	public IEnumerator DoCinematicZoom(float targetOrthographicSize)
	{
		this.cinemaCamEnabled = true;
		this.FreeCameraEnabled = true;
		this.targetOrthographicSize = targetOrthographicSize;
		while (targetOrthographicSize - this.OrthographicSize >= 0.001f)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OrthographicSize = targetOrthographicSize;
		this.FreeCameraEnabled = false;
		this.cinemaCamEnabled = false;
		yield break;
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x000BCD64 File Offset: 0x000BAF64
	private Vector3 PointUnderCursor(Vector3 mousePos, Camera cam)
	{
		Ray ray = cam.ScreenPointToRay(mousePos);
		Vector3 direction = ray.direction;
		Vector3 b = direction * Mathf.Abs(cam.transform.GetPosition().z / direction.z);
		return ray.origin + b;
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x000BCDB4 File Offset: 0x000BAFB4
	private void CinemaCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		Vector3 localPosition = base.transform.GetLocalPosition();
		float num = Mathf.Pow((float)this.cinemaZoomSpeed, 3f);
		if (this.cinemaZoomIn)
		{
			this.overrideZoomSpeed = -num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else if (this.cinemaZoomOut)
		{
			this.overrideZoomSpeed = num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else
		{
			this.overrideZoomSpeed = 0f;
		}
		if (this.cinemaToggleEasing)
		{
			this.cinemaZoomVelocity += (this.overrideZoomSpeed - this.cinemaZoomVelocity) * this.cinemaEasing;
		}
		else
		{
			this.cinemaZoomVelocity = this.overrideZoomSpeed;
		}
		if (this.cinemaZoomVelocity != 0f)
		{
			this.OrthographicSize = main.orthographicSize + this.cinemaZoomVelocity * unscaledDeltaTime * (main.orthographicSize / 20f);
			this.targetOrthographicSize = main.orthographicSize;
		}
		float num2 = num / TuningData<CameraController.Tuning>.Get().cinemaZoomToFactor;
		float num3 = this.keyPanningSpeed / 20f * main.orthographicSize;
		float num4 = num3 * (num / TuningData<CameraController.Tuning>.Get().cinemaPanToFactor);
		if (!this.isTargetPosSet && this.targetOrthographicSize != main.orthographicSize)
		{
			float t = Mathf.Min(num2 * unscaledDeltaTime, 0.1f);
			this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		}
		Vector3 b = Vector3.zero;
		if (this.isTargetPosSet)
		{
			float num5 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetZoomEasingFactor;
			float num6 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetPanEasingFactor;
			float num7 = this.targetOrthographicSize - main.orthographicSize;
			Vector3 vector = this.targetPos - localPosition;
			float num8;
			float num9;
			if (!this.cinemaToggleEasing)
			{
				num8 = num2 * unscaledDeltaTime;
				num9 = num4 * unscaledDeltaTime;
			}
			else
			{
				DebugUtil.LogArgs(new object[]
				{
					"Min zoom of:",
					num2 * unscaledDeltaTime,
					Mathf.Abs(num7) * num5 * unscaledDeltaTime
				});
				num8 = Mathf.Min(num2 * unscaledDeltaTime, Mathf.Abs(num7) * num5 * unscaledDeltaTime);
				DebugUtil.LogArgs(new object[]
				{
					"Min pan of:",
					num4 * unscaledDeltaTime,
					vector.magnitude * num6 * unscaledDeltaTime
				});
				num9 = Mathf.Min(num4 * unscaledDeltaTime, vector.magnitude * num6 * unscaledDeltaTime);
			}
			float num10;
			if (Mathf.Abs(num7) < num8)
			{
				num10 = num7;
			}
			else
			{
				num10 = Mathf.Sign(num7) * num8;
			}
			if (vector.magnitude < num9)
			{
				b = vector;
			}
			else
			{
				b = vector.normalized * num9;
			}
			if (Mathf.Abs(num10) < 0.001f && b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				num10 = num7;
				b = vector;
			}
			this.OrthographicSize = main.orthographicSize + num10 * (main.orthographicSize / 20f);
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else
		{
			float num11 = num / TuningData<CameraController.Tuning>.Get().cinemaPanFactor;
			Vector3 zero = Vector3.zero;
			if (this.cinemaPanLeft)
			{
				this.ClearFollowTarget();
				zero.x = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanRight)
			{
				this.ClearFollowTarget();
				zero.x = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanUp)
			{
				this.ClearFollowTarget();
				zero.y = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanDown)
			{
				this.ClearFollowTarget();
				zero.y = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaToggleEasing)
			{
				this.keyPanDelta += (zero - this.keyPanDelta) * this.cinemaEasing;
			}
			else
			{
				this.keyPanDelta = zero;
			}
		}
		Vector3 vector2 = localPosition + b + b2 + this.keyPanDelta * unscaledDeltaTime;
		if (this.followTarget != null)
		{
			vector2.x = this.followTargetPos.x;
			vector2.y = this.followTargetPos.y;
		}
		vector2.z = -100f;
		if ((double)(vector2 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector2);
		}
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x000BD27C File Offset: 0x000BB47C
	private void NormalCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		this.smoothDt = this.smoothDt * 2f / 3f + unscaledDeltaTime / 3f;
		float num = (this.overrideZoomSpeed != 0f) ? this.overrideZoomSpeed : this.zoomSpeed;
		Vector3 localPosition = base.transform.GetLocalPosition();
		Vector3 vector = (this.overrideZoomSpeed != 0f) ? new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f) : KInputManager.GetMousePos();
		Vector3 position = this.PointUnderCursor(vector, main);
		Vector3 position2 = main.ScreenToViewportPoint(vector);
		float num2 = this.keyPanningSpeed / 20f * main.orthographicSize;
		num2 *= Mathf.Min(unscaledDeltaTime / 0.016666666f, 10f);
		float t = num * Mathf.Min(this.smoothDt, 0.3f);
		this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		base.transform.SetLocalPosition(localPosition);
		Vector3 vector2 = main.WorldToViewportPoint(position);
		position2.z = vector2.z;
		Vector3 b = main.ViewportToWorldPoint(vector2) - main.ViewportToWorldPoint(position2);
		if (this.isTargetPosSet)
		{
			b = Vector3.Lerp(localPosition, this.targetPos, num * this.smoothDt) - localPosition;
			if (b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				b = this.targetPos - localPosition;
			}
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
		}
		Vector3 vector3 = localPosition + b + b2;
		if (this.panning)
		{
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else if (!this.DisableUserCameraControl)
		{
			if (this.panLeft)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panRight)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panUp)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panDown)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				Vector2 vector4 = num2 * KInputManager.steamInputInterpreter.GetSteamCameraMovement();
				if (Mathf.Abs(vector4.x) > Mathf.Epsilon || Mathf.Abs(vector4.y) > Mathf.Epsilon)
				{
					this.ClearFollowTarget();
					this.isTargetPosSet = false;
					this.overrideZoomSpeed = 0f;
				}
				this.keyPanDelta += new Vector3(vector4.x, vector4.y, 0f);
			}
			Vector3 vector5 = new Vector3(Mathf.Lerp(0f, this.keyPanDelta.x, this.smoothDt * this.keyPanningEasing), Mathf.Lerp(0f, this.keyPanDelta.y, this.smoothDt * this.keyPanningEasing), 0f);
			this.keyPanDelta -= vector5;
			vector3.x += vector5.x;
			vector3.y += vector5.y;
		}
		if (this.followTarget != null)
		{
			vector3.x = this.followTargetPos.x;
			vector3.y = this.followTargetPos.y;
		}
		vector3.z = -100f;
		if ((double)(vector3 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector3);
		}
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x000BD6C8 File Offset: 0x000BB8C8
	private void Update()
	{
		if (Game.Instance == null || !Game.Instance.timelapser.CapturingTimelapseScreenshot)
		{
			if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				this.CinemaCamUpdate();
			}
			else
			{
				this.NormalCamUpdate();
			}
		}
		if (this.infoText != null && this.infoText.color.a > 0f)
		{
			Color color = this.infoText.color;
			color.a = Mathf.Max(0f, this.infoText.color.a - Time.unscaledDeltaTime * 0.5f);
			this.infoText.color = color;
		}
		this.ConstrainToWorld();
		Vector3 vector = this.PointUnderCursor(KInputManager.GetMousePos(), Camera.main);
		Shader.SetGlobalVector("_WorldCameraPos", new Vector4(base.transform.GetPosition().x, base.transform.GetPosition().y, base.transform.GetPosition().z, Camera.main.orthographicSize));
		Shader.SetGlobalVector("_WorldCursorPos", new Vector4(vector.x, vector.y, 0f, 0f));
		this.VisibleArea.Update();
		this.soundCuller = SoundCuller.CreateCuller();
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x000BD818 File Offset: 0x000BBA18
	private Vector3 GetFollowPos()
	{
		if (this.followTarget != null)
		{
			Vector3 result = this.followTarget.transform.GetPosition();
			KAnimControllerBase component = this.followTarget.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				result = component.GetWorldPivot();
			}
			return result;
		}
		return Vector3.zero;
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x000BD868 File Offset: 0x000BBA68
	public static float GetHighestVisibleCell_Height(byte worldID = 255)
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Camera main = Camera.main;
		float orthographicSize = main.orthographicSize;
		main.orthographicSize = 20f;
		Ray ray = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		Vector3 vector2 = CameraController.Instance.transform.GetPosition() - ray.origin;
		main.orthographicSize = orthographicSize;
		if (ClusterManager.Instance != null)
		{
			WorldContainer worldContainer = (worldID == byte.MaxValue) ? ClusterManager.Instance.activeWorld : ClusterManager.Instance.GetWorld((int)worldID);
			worldContainer.minimumBounds * Grid.CellSizeInMeters;
			vector = worldContainer.maximumBounds * Grid.CellSizeInMeters;
			new Vector2((float)worldContainer.Width, (float)worldContainer.Height) * Grid.CellSizeInMeters;
		}
		return vector.y * 1.1f + 20f + vector2.y;
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000BD970 File Offset: 0x000BBB70
	private void ConstrainToWorld()
	{
		if (Game.Instance != null && Game.Instance.IsLoading())
		{
			return;
		}
		if (this.FreeCameraEnabled)
		{
			return;
		}
		Camera main = Camera.main;
		Ray ray = main.ViewportPointToRay(Vector3.zero + Vector3.one * 0.33f);
		Ray ray2 = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector3 point2 = ray2.GetPoint(distance2);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Vector2 vector3 = vector2;
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			vector = activeWorld.minimumBounds * Grid.CellSizeInMeters;
			vector2 = activeWorld.maximumBounds * Grid.CellSizeInMeters;
			vector3 = new Vector2((float)activeWorld.Width, (float)activeWorld.Height) * Grid.CellSizeInMeters;
		}
		if (point2.x - point.x > vector3.x || point2.y - point.y > vector3.y)
		{
			return;
		}
		Vector3 b = base.transform.GetPosition() - ray.origin;
		Vector3 vector4 = point;
		vector4.x = Mathf.Max(vector.x, vector4.x);
		vector4.y = Mathf.Max(vector.y * Grid.CellSizeInMeters, vector4.y);
		ray.origin = vector4;
		ray.direction = -ray.direction;
		vector4 = ray.GetPoint(distance);
		base.transform.SetPosition(vector4 + b);
		b = base.transform.GetPosition() - ray2.origin;
		vector4 = point2;
		vector4.x = Mathf.Min(vector2.x, vector4.x);
		vector4.y = Mathf.Min(vector2.y * 1.1f, vector4.y);
		ray2.origin = vector4;
		ray2.direction = -ray2.direction;
		vector4 = ray2.GetPoint(distance2);
		Vector3 position = vector4 + b;
		position.z = -100f;
		base.transform.SetPosition(position);
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x000BDC10 File Offset: 0x000BBE10
	public void Save(BinaryWriter writer)
	{
		writer.Write(base.transform.GetPosition());
		writer.Write(base.transform.localScale);
		writer.Write(base.transform.rotation);
		writer.Write(this.targetOrthographicSize);
		CameraSaveData.position = base.transform.GetPosition();
		CameraSaveData.localScale = base.transform.localScale;
		CameraSaveData.rotation = base.transform.rotation;
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x000BDC8C File Offset: 0x000BBE8C
	private void Restore()
	{
		if (CameraSaveData.valid)
		{
			int cell = Grid.PosToCell(CameraSaveData.position);
			if (Grid.IsValidCell(cell) && !Grid.IsVisible(cell))
			{
				global::Debug.LogWarning("Resetting Camera Position... camera was saved in an undiscovered area of the map.");
				this.CameraGoHome(2f, false);
				return;
			}
			base.transform.SetPosition(CameraSaveData.position);
			base.transform.localScale = CameraSaveData.localScale;
			base.transform.rotation = CameraSaveData.rotation;
			this.targetOrthographicSize = Mathf.Clamp(CameraSaveData.orthographicsSize, this.minOrthographicSize, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
			this.SnapTo(base.transform.GetPosition());
		}
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x000BDD47 File Offset: 0x000BBF47
	private void OnMRTSetupComplete(Camera cam)
	{
		this.cameras.Add(cam);
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x000BDD55 File Offset: 0x000BBF55
	public bool IsAudibleSound(Vector2 pos)
	{
		return this.soundCuller.IsAudible(pos);
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x000BDD64 File Offset: 0x000BBF64
	public bool IsAudibleSound(Vector3 pos, EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.soundCuller.IsAudible(pos, eventReferencePath);
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000BDD8F File Offset: 0x000BBF8F
	public bool IsAudibleSound(Vector3 pos, HashedString sound_path)
	{
		return this.soundCuller.IsAudible(pos, sound_path);
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000BDDA3 File Offset: 0x000BBFA3
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		return this.soundCuller.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000BDDB4 File Offset: 0x000BBFB4
	public bool IsVisiblePos(Vector3 pos)
	{
		return this.VisibleArea.CurrentArea.Contains(pos);
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x000BDDD8 File Offset: 0x000BBFD8
	public bool IsVisiblePosExtended(Vector3 pos)
	{
		return this.VisibleArea.CurrentAreaExtended.Contains(pos);
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x000BDDF9 File Offset: 0x000BBFF9
	protected override void OnCleanUp()
	{
		CameraController.Instance = null;
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x000BDE04 File Offset: 0x000BC004
	public void SetFollowTarget(Transform follow_target)
	{
		this.ClearFollowTarget();
		if (follow_target == null)
		{
			return;
		}
		this.followTarget = follow_target;
		this.OrthographicSize = 6f;
		this.targetOrthographicSize = 6f;
		Vector3 followPos = this.GetFollowPos();
		this.followTargetPos = new Vector3(followPos.x, followPos.y, base.transform.GetPosition().z);
		base.transform.SetPosition(this.followTargetPos);
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-1506069671, null);
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x000BDE94 File Offset: 0x000BC094
	public void ClearFollowTarget()
	{
		if (this.followTarget == null)
		{
			return;
		}
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-485480405, null);
		this.followTarget = null;
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x000BDEC4 File Offset: 0x000BC0C4
	public void UpdateFollowTarget()
	{
		if (this.followTarget != null)
		{
			Vector3 followPos = this.GetFollowPos();
			Vector2 a = new Vector2(base.transform.GetLocalPosition().x, base.transform.GetLocalPosition().y);
			byte b = Grid.WorldIdx[Grid.PosToCell(followPos)];
			if (ClusterManager.Instance.activeWorldId != (int)b)
			{
				Transform transform = this.followTarget;
				this.SetFollowTarget(null);
				ClusterManager.Instance.SetActiveWorld((int)b);
				this.SetFollowTarget(transform);
				return;
			}
			Vector2 vector = Vector2.Lerp(a, followPos, Time.unscaledDeltaTime * 25f);
			this.followTargetPos = new Vector3(vector.x, vector.y, base.transform.GetLocalPosition().z);
		}
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x000BDF90 File Offset: 0x000BC190
	public void RenderForTimelapser(ref RenderTexture tex)
	{
		this.RenderCameraForTimelapse(this.baseCamera, ref tex, this.timelapseCameraCullingMask, -1f);
		CameraClearFlags clearFlags = this.overlayCamera.clearFlags;
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.RenderCameraForTimelapse(this.overlayCamera, ref tex, this.timelapseOverlayCameraCullingMask, -1f);
		this.overlayCamera.clearFlags = clearFlags;
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x000BDFF4 File Offset: 0x000BC1F4
	private void RenderCameraForTimelapse(Camera cam, ref RenderTexture tex, LayerMask mask, float overrideAspect = -1f)
	{
		int cullingMask = cam.cullingMask;
		RenderTexture targetTexture = cam.targetTexture;
		cam.targetTexture = tex;
		cam.aspect = (float)tex.width / (float)tex.height;
		if (overrideAspect != -1f)
		{
			cam.aspect = overrideAspect;
		}
		if (mask != -1)
		{
			cam.cullingMask = mask;
		}
		cam.Render();
		cam.ResetAspect();
		cam.cullingMask = cullingMask;
		cam.targetTexture = targetTexture;
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x000BE06E File Offset: 0x000BC26E
	private void CheckMoveUnpause()
	{
		if (this.cinemaCamEnabled && this.cinemaUnpauseNextMove)
		{
			this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
			if (SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
		}
	}

	// Token: 0x040012EA RID: 4842
	public const float DEFAULT_MAX_ORTHO_SIZE = 20f;

	// Token: 0x040012EB RID: 4843
	public const float MAX_Y_SCALE = 1.1f;

	// Token: 0x040012EC RID: 4844
	public LocText infoText;

	// Token: 0x040012ED RID: 4845
	private const float FIXED_Z = -100f;

	// Token: 0x040012EF RID: 4847
	public bool FreeCameraEnabled;

	// Token: 0x040012F0 RID: 4848
	public float zoomSpeed;

	// Token: 0x040012F1 RID: 4849
	public float minOrthographicSize;

	// Token: 0x040012F2 RID: 4850
	public float zoomFactor;

	// Token: 0x040012F3 RID: 4851
	public float keyPanningSpeed;

	// Token: 0x040012F4 RID: 4852
	public float keyPanningEasing;

	// Token: 0x040012F5 RID: 4853
	public Texture2D dayColourCube;

	// Token: 0x040012F6 RID: 4854
	public Texture2D nightColourCube;

	// Token: 0x040012F7 RID: 4855
	public Material LightBufferMaterial;

	// Token: 0x040012F8 RID: 4856
	public Material LightCircleOverlay;

	// Token: 0x040012F9 RID: 4857
	public Material LightConeOverlay;

	// Token: 0x040012FA RID: 4858
	public Transform followTarget;

	// Token: 0x040012FB RID: 4859
	public Vector3 followTargetPos;

	// Token: 0x040012FC RID: 4860
	public GridVisibleArea VisibleArea = new GridVisibleArea(8);

	// Token: 0x040012FE RID: 4862
	private float maxOrthographicSize = 20f;

	// Token: 0x040012FF RID: 4863
	private float overrideZoomSpeed;

	// Token: 0x04001300 RID: 4864
	private bool panning;

	// Token: 0x04001301 RID: 4865
	private const float MaxEdgePaddingPercent = 0.33f;

	// Token: 0x04001302 RID: 4866
	private Vector3 keyPanDelta;

	// Token: 0x04001305 RID: 4869
	[SerializeField]
	private LayerMask timelapseCameraCullingMask;

	// Token: 0x04001306 RID: 4870
	[SerializeField]
	private LayerMask timelapseOverlayCameraCullingMask;

	// Token: 0x04001308 RID: 4872
	private bool userCameraControlDisabled;

	// Token: 0x04001309 RID: 4873
	private bool panLeft;

	// Token: 0x0400130A RID: 4874
	private bool panRight;

	// Token: 0x0400130B RID: 4875
	private bool panUp;

	// Token: 0x0400130C RID: 4876
	private bool panDown;

	// Token: 0x0400130E RID: 4878
	[NonSerialized]
	public Camera baseCamera;

	// Token: 0x0400130F RID: 4879
	[NonSerialized]
	public Camera overlayCamera;

	// Token: 0x04001310 RID: 4880
	[NonSerialized]
	public Camera overlayNoDepthCamera;

	// Token: 0x04001311 RID: 4881
	[NonSerialized]
	public Camera uiCamera;

	// Token: 0x04001312 RID: 4882
	[NonSerialized]
	public Camera lightBufferCamera;

	// Token: 0x04001313 RID: 4883
	[NonSerialized]
	public Camera simOverlayCamera;

	// Token: 0x04001314 RID: 4884
	[NonSerialized]
	public Camera infraredCamera;

	// Token: 0x04001315 RID: 4885
	[NonSerialized]
	public Camera timelapseFreezeCamera;

	// Token: 0x04001316 RID: 4886
	[SerializeField]
	private List<GameScreenManager.UIRenderTarget> uiCameraTargets;

	// Token: 0x04001317 RID: 4887
	public List<Camera> cameras = new List<Camera>();

	// Token: 0x04001318 RID: 4888
	private MultipleRenderTarget mrt;

	// Token: 0x04001319 RID: 4889
	private KAnimActivePostProcessingEffects kAnimPostProcessingEffects;

	// Token: 0x0400131A RID: 4890
	private CustomActiveScreenPostProcessingEffects customActiveScreenPostProcessingEffects;

	// Token: 0x0400131B RID: 4891
	public SoundCuller soundCuller;

	// Token: 0x0400131C RID: 4892
	private bool cinemaCamEnabled;

	// Token: 0x0400131D RID: 4893
	private bool cinemaToggleLock;

	// Token: 0x0400131E RID: 4894
	private bool cinemaToggleEasing;

	// Token: 0x0400131F RID: 4895
	private bool cinemaUnpauseNextMove;

	// Token: 0x04001320 RID: 4896
	private bool cinemaPanLeft;

	// Token: 0x04001321 RID: 4897
	private bool cinemaPanRight;

	// Token: 0x04001322 RID: 4898
	private bool cinemaPanUp;

	// Token: 0x04001323 RID: 4899
	private bool cinemaPanDown;

	// Token: 0x04001324 RID: 4900
	private bool cinemaZoomIn;

	// Token: 0x04001325 RID: 4901
	private bool cinemaZoomOut;

	// Token: 0x04001326 RID: 4902
	private int cinemaZoomSpeed = 10;

	// Token: 0x04001327 RID: 4903
	private float cinemaEasing = 0.05f;

	// Token: 0x04001328 RID: 4904
	private float cinemaZoomVelocity;

	// Token: 0x0400132A RID: 4906
	private float smoothDt;

	// Token: 0x02001423 RID: 5155
	public class Tuning : TuningData<CameraController.Tuning>
	{
		// Token: 0x04006DA5 RID: 28069
		public float maxOrthographicSizeDebug;

		// Token: 0x04006DA6 RID: 28070
		public float cinemaZoomFactor = 100f;

		// Token: 0x04006DA7 RID: 28071
		public float cinemaPanFactor = 50f;

		// Token: 0x04006DA8 RID: 28072
		public float cinemaZoomToFactor = 100f;

		// Token: 0x04006DA9 RID: 28073
		public float cinemaPanToFactor = 50f;

		// Token: 0x04006DAA RID: 28074
		public float targetZoomEasingFactor = 400f;

		// Token: 0x04006DAB RID: 28075
		public float targetPanEasingFactor = 100f;
	}
}
