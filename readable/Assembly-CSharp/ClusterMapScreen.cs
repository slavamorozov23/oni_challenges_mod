using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000CB8 RID: 3256
public class ClusterMapScreen : KScreen
{
	// Token: 0x060063B3 RID: 25523 RVA: 0x00251C76 File Offset: 0x0024FE76
	public static void DestroyInstance()
	{
		ClusterMapScreen.Instance = null;
	}

	// Token: 0x060063B4 RID: 25524 RVA: 0x00251C7E File Offset: 0x0024FE7E
	public ClusterMapVisualizer GetEntityVisAnim(ClusterGridEntity entity)
	{
		if (this.m_gridEntityAnims.ContainsKey(entity))
		{
			return this.m_gridEntityAnims[entity];
		}
		return null;
	}

	// Token: 0x060063B5 RID: 25525 RVA: 0x00251C9C File Offset: 0x0024FE9C
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x060063B6 RID: 25526 RVA: 0x00251CB1 File Offset: 0x0024FEB1
	public float CurrentZoomPercentage()
	{
		return (this.m_currentZoomScale - 50f) / 100f;
	}

	// Token: 0x060063B7 RID: 25527 RVA: 0x00251CC5 File Offset: 0x0024FEC5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.m_selectMarker = global::Util.KInstantiateUI<SelectMarker>(this.selectMarkerPrefab, base.gameObject, false);
		this.m_selectMarker.gameObject.SetActive(false);
		ClusterMapScreen.Instance = this;
	}

	// Token: 0x060063B8 RID: 25528 RVA: 0x00251CFC File Offset: 0x0024FEFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		global::Debug.Assert(this.cellVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the cellVisPrefab hex must be 1");
		global::Debug.Assert(this.terrainVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the terrainVisPrefab hex must be 1");
		global::Debug.Assert(this.mobileVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the mobileVisPrefab hex must be 1");
		global::Debug.Assert(this.staticVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the staticVisPrefab hex must be 1");
		int num;
		int num2;
		int num3;
		int num4;
		this.GenerateGridVis(out num, out num2, out num3, out num4);
		this.Show(false);
		this.mapScrollRect.content.sizeDelta = new Vector2((float)(num2 * 4), (float)(num4 * 4));
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		this.m_onDestinationChangedDelegate = new Action<object>(this.OnDestinationChanged);
		this.m_onSelectObjectDelegate = new Action<object>(this.OnSelectObject);
		base.Subscribe(1980521255, delegate(object _)
		{
			this.UpdateVis(false);
		});
	}

	// Token: 0x060063B9 RID: 25529 RVA: 0x00251E5C File Offset: 0x0025005C
	private void RefreshHexCellsInventoryVisuals()
	{
		foreach (StarmapHexCellInventory starmapHexCellInventory in StarmapHexCellInventory.AllInventories.Values)
		{
			starmapHexCellInventory.GetComponent<StarmapHexCellInventoryVisuals>().RefreshVisuals();
		}
	}

	// Token: 0x060063BA RID: 25530 RVA: 0x00251EB8 File Offset: 0x002500B8
	protected void MoveToNISPosition()
	{
		if (!this.movingToTargetNISPosition)
		{
			return;
		}
		Vector3 b = new Vector3(-this.targetNISPosition.x * this.mapScrollRect.content.localScale.x, -this.targetNISPosition.y * this.mapScrollRect.content.localScale.y, this.targetNISPosition.z);
		this.m_targetZoomScale = Mathf.Lerp(this.m_targetZoomScale, this.targetNISZoom, Time.unscaledDeltaTime * 2f);
		this.mapScrollRect.content.SetLocalPosition(Vector3.Lerp(this.mapScrollRect.content.GetLocalPosition(), b, Time.unscaledDeltaTime * 2.5f));
		float num = Vector3.Distance(this.mapScrollRect.content.GetLocalPosition(), b);
		if (num < 100f)
		{
			ClusterMapHex component = this.m_cellVisByLocation[this.selectOnMoveNISComplete].GetComponent<ClusterMapHex>();
			if (this.m_selectedHex != component)
			{
				this.SelectHex(component);
			}
			if (num < 10f)
			{
				this.movingToTargetNISPosition = false;
			}
		}
	}

	// Token: 0x060063BB RID: 25531 RVA: 0x00251FD2 File Offset: 0x002501D2
	public void SetTargetFocusPosition(AxialI targetPosition, float delayBeforeMove = 0.5f)
	{
		if (this.activeMoveToTargetRoutine != null)
		{
			base.StopCoroutine(this.activeMoveToTargetRoutine);
		}
		this.activeMoveToTargetRoutine = base.StartCoroutine(this.MoveToTargetRoutine(targetPosition, delayBeforeMove));
	}

	// Token: 0x060063BC RID: 25532 RVA: 0x00251FFC File Offset: 0x002501FC
	private IEnumerator MoveToTargetRoutine(AxialI targetPosition, float delayBeforeMove)
	{
		delayBeforeMove = Mathf.Max(delayBeforeMove, 0f);
		yield return SequenceUtil.WaitForSecondsRealtime(delayBeforeMove);
		this.targetNISPosition = AxialUtil.AxialToWorld((float)targetPosition.r, (float)targetPosition.q);
		this.targetNISZoom = 150f;
		this.movingToTargetNISPosition = true;
		this.selectOnMoveNISComplete = targetPosition;
		yield break;
	}

	// Token: 0x060063BD RID: 25533 RVA: 0x0025201C File Offset: 0x0025021C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut)) && CameraController.IsMouseOverGameWindow)
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
			pointerEventData.position = KInputManager.GetMousePos();
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (current != null)
			{
				current.RaycastAll(pointerEventData, list);
				bool flag = false;
				foreach (RaycastResult raycastResult in list)
				{
					if (!raycastResult.gameObject.transform.IsChildOf(base.transform))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					float num;
					if (KInputManager.currentControllerIsGamepad)
					{
						num = 25f;
						num *= (float)(e.IsAction(global::Action.ZoomIn) ? 1 : -1);
					}
					else
					{
						num = Input.mouseScrollDelta.y * 25f;
					}
					this.m_targetZoomScale = Mathf.Clamp(this.m_targetZoomScale + num, 50f, 150f);
					e.TryConsume(global::Action.ZoomIn);
					if (!e.Consumed)
					{
						e.TryConsume(global::Action.ZoomOut);
					}
				}
			}
		}
		CameraController.Instance.ChangeWorldInput(e);
		base.OnKeyDown(e);
	}

	// Token: 0x060063BE RID: 25534 RVA: 0x00252174 File Offset: 0x00250374
	public bool TryHandleCancel()
	{
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination && !this.m_closeOnSelect)
		{
			ClusterDestinationSelector destinationSelector = this.m_destinationSelector;
			this.SetMode(ClusterMapScreen.Mode.Default);
			if (destinationSelector != null)
			{
				destinationSelector.Trigger(94158097, null);
			}
			return true;
		}
		return false;
	}

	// Token: 0x060063BF RID: 25535 RVA: 0x002521B8 File Offset: 0x002503B8
	public void ShowInSelectDestinationMode(ClusterDestinationSelector destination_selector)
	{
		this.m_destinationSelector = destination_selector;
		if (!this.IsScreenActive())
		{
			ManagementMenu.Instance.ToggleClusterMap();
			this.m_closeOnSelect = true;
		}
		ClusterGridEntity component = destination_selector.GetComponent<ClusterGridEntity>();
		this.SetSelectedEntity(component, false);
		if (this.m_selectedEntity != null)
		{
			this.m_selectedHex = this.m_cellVisByLocation[this.m_selectedEntity.Location].GetComponent<ClusterMapHex>();
		}
		else
		{
			AxialI myWorldLocation = destination_selector.GetMyWorldLocation();
			ClusterMapHex component2 = this.m_cellVisByLocation[myWorldLocation].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component2;
		}
		this.SetMode(ClusterMapScreen.Mode.SelectDestination);
	}

	// Token: 0x060063C0 RID: 25536 RVA: 0x0025224C File Offset: 0x0025044C
	private void SetMode(ClusterMapScreen.Mode mode)
	{
		this.m_mode = mode;
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			this.m_destinationSelector = null;
		}
		this.UpdateVis(false);
	}

	// Token: 0x060063C1 RID: 25537 RVA: 0x0025226B File Offset: 0x0025046B
	public ClusterMapScreen.Mode GetMode()
	{
		return this.m_mode;
	}

	// Token: 0x060063C2 RID: 25538 RVA: 0x00252274 File Offset: 0x00250474
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.MoveToNISPosition();
			this.UpdateVis(true);
			Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
			Game.Instance.Subscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
			Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
			ClusterMapSelectTool.Instance.Activate();
			this.SetShowingNonClusterMapHud(false);
			CameraController.Instance.DisableUserCameraControl = true;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
			MusicManager.instance.PlaySong("Music_Starmap", false);
			this.UpdateTearStatus();
			this.RefreshHexCellsInventoryVisuals();
			return;
		}
		Game.Instance.Unsubscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		this.m_mode = ClusterMapScreen.Mode.Default;
		this.m_closeOnSelect = false;
		this.m_destinationSelector = null;
		SelectTool.Instance.Activate();
		this.SetShowingNonClusterMapHud(true);
		CameraController.Instance.DisableUserCameraControl = false;
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_Starmap"))
		{
			MusicManager.instance.StopSong("Music_Starmap", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060063C3 RID: 25539 RVA: 0x002523F7 File Offset: 0x002505F7
	private void SetShowingNonClusterMapHud(bool show)
	{
		PlanScreen.Instance.gameObject.SetActive(show);
		ToolMenu.Instance.gameObject.SetActive(show);
		OverlayScreen.Instance.gameObject.SetActive(show);
	}

	// Token: 0x060063C4 RID: 25540 RVA: 0x0025242C File Offset: 0x0025062C
	private void SetSelectedEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Unsubscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Unsubscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		this.m_selectedEntity = entity;
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Subscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Subscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		KSelectable new_selected = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<KSelectable>() : null;
		if (frameDelay)
		{
			ClusterMapSelectTool.Instance.SelectNextFrame(new_selected, false);
			return;
		}
		ClusterMapSelectTool.Instance.Select(new_selected, false);
	}

	// Token: 0x060063C5 RID: 25541 RVA: 0x002524EF File Offset: 0x002506EF
	private void OnDestinationChanged(object _)
	{
		this.UpdateVis(false);
	}

	// Token: 0x060063C6 RID: 25542 RVA: 0x002524F8 File Offset: 0x002506F8
	private void OnSelectObject(object _)
	{
		if (this.m_selectedEntity == null)
		{
			return;
		}
		KSelectable component = this.m_selectedEntity.GetComponent<KSelectable>();
		if (component == null || component.IsSelected)
		{
			return;
		}
		this.SetSelectedEntity(null, false);
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			if (this.m_closeOnSelect)
			{
				ManagementMenu.Instance.CloseAll();
			}
			else
			{
				this.SetMode(ClusterMapScreen.Mode.Default);
			}
		}
		this.UpdateVis(false);
	}

	// Token: 0x060063C7 RID: 25543 RVA: 0x00252565 File Offset: 0x00250765
	private void OnFogOfWarRevealed(object _ = null)
	{
		this.UpdateVis(false);
	}

	// Token: 0x060063C8 RID: 25544 RVA: 0x0025256E File Offset: 0x0025076E
	private void OnNewTelescopeTarget(object _ = null)
	{
		this.UpdateVis(false);
	}

	// Token: 0x060063C9 RID: 25545 RVA: 0x00252577 File Offset: 0x00250777
	private void Update()
	{
		if (KInputManager.currentControllerIsGamepad && this.IsScreenActive())
		{
			this.mapScrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
		}
	}

	// Token: 0x060063CA RID: 25546 RVA: 0x002525A8 File Offset: 0x002507A8
	private void GenerateGridVis(out int minR, out int maxR, out int minQ, out int maxQ)
	{
		minR = int.MaxValue;
		maxR = int.MinValue;
		minQ = int.MaxValue;
		maxQ = int.MinValue;
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(this.cellVisPrefab, Vector3.zero, Quaternion.identity, this.cellVisContainer.transform);
			clusterMapVisualizer.rectTransform().SetLocalPosition(keyValuePair.Key.ToWorld());
			clusterMapVisualizer.gameObject.SetActive(true);
			ClusterMapHex component = clusterMapVisualizer.GetComponent<ClusterMapHex>();
			component.SetLocation(keyValuePair.Key);
			this.m_cellVisByLocation.Add(keyValuePair.Key, clusterMapVisualizer);
			minR = Mathf.Min(minR, component.location.R);
			maxR = Mathf.Max(maxR, component.location.R);
			minQ = Mathf.Min(minQ, component.location.Q);
			maxQ = Mathf.Max(maxQ, component.location.Q);
		}
		this.SetupVisGameObjects();
		this.UpdateVis(false);
	}

	// Token: 0x060063CB RID: 25547 RVA: 0x002526FC File Offset: 0x002508FC
	public Transform GetGridEntityNameTarget(ClusterGridEntity entity)
	{
		ClusterMapVisualizer clusterMapVisualizer;
		if (this.m_currentZoomScale >= 115f && this.m_gridEntityVis.TryGetValue(entity, out clusterMapVisualizer))
		{
			return clusterMapVisualizer.nameTarget;
		}
		return null;
	}

	// Token: 0x060063CC RID: 25548 RVA: 0x00252730 File Offset: 0x00250930
	public override void ScreenUpdate(bool topLevel)
	{
		float t = Mathf.Min(4f * Time.unscaledDeltaTime, 0.9f);
		this.m_currentZoomScale = Mathf.Lerp(this.m_currentZoomScale, this.m_targetZoomScale, t);
		Vector2 v = KInputManager.GetMousePos();
		Vector3 b = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		Vector3 a = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localPosition += (a - b) * this.m_currentZoomScale;
		this.MoveToNISPosition();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x060063CD RID: 25549 RVA: 0x00252804 File Offset: 0x00250A04
	private void FloatyAsteroidAnimation()
	{
		float num = 0f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			AsteroidGridEntity component = worldContainer.GetComponent<AsteroidGridEntity>();
			if (component != null && this.m_gridEntityVis.ContainsKey(component) && ClusterMapScreen.GetRevealLevel(component) == ClusterRevealLevel.Visible)
			{
				KAnimControllerBase firstAnimController = this.m_gridEntityVis[component].GetFirstAnimController();
				float y = this.floatCycleOffset + this.floatCycleScale * Mathf.Sin(this.floatCycleSpeed * (num + GameClock.Instance.GetTime()));
				firstAnimController.Offset = new Vector2(0f, y);
			}
			num += 1f;
		}
	}

	// Token: 0x060063CE RID: 25550 RVA: 0x002528DC File Offset: 0x00250ADC
	private void SetupVisGameObjects()
	{
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			foreach (ClusterGridEntity clusterGridEntity in keyValuePair.Value)
			{
				ClusterGrid.Instance.GetCellRevealLevel(keyValuePair.Key);
				ClusterRevealLevel isVisibleInFOW = clusterGridEntity.IsVisibleInFOW;
				ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(clusterGridEntity);
				if (clusterGridEntity.IsVisible && revealLevel != ClusterRevealLevel.Hidden && !this.m_gridEntityVis.ContainsKey(clusterGridEntity))
				{
					ClusterMapVisualizer original = null;
					GameObject gameObject = null;
					switch (clusterGridEntity.Layer)
					{
					case EntityLayer.Asteroid:
						original = this.terrainVisPrefab;
						gameObject = this.terrainVisContainer;
						break;
					case EntityLayer.Craft:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.POI:
						original = this.staticVisPrefab;
						gameObject = this.POIVisContainer;
						break;
					case EntityLayer.Telescope:
						original = this.staticVisPrefab;
						gameObject = this.telescopeVisContainer;
						break;
					case EntityLayer.Payload:
					case EntityLayer.Meteor:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.FX:
						original = this.staticVisPrefab;
						gameObject = this.FXVisContainer;
						break;
					case EntityLayer.Debri:
						original = this.staticVisPrefab;
						gameObject = this.DebriVisContainer;
						break;
					}
					ClusterNameDisplayScreen.Instance.AddNewEntry(clusterGridEntity);
					ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(original, gameObject.transform);
					clusterMapVisualizer.Init(clusterGridEntity, this.pathDrawer);
					clusterMapVisualizer.gameObject.SetActive(true);
					this.m_gridEntityAnims.Add(clusterGridEntity, clusterMapVisualizer);
					this.m_gridEntityVis.Add(clusterGridEntity, clusterMapVisualizer);
					clusterGridEntity.positionDirty = false;
					clusterGridEntity.Subscribe(1502190696, new Action<object>(this.RemoveDeletedEntities));
				}
			}
		}
		this.RemoveDeletedEntities(null);
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair2 in this.m_gridEntityVis)
		{
			ClusterGridEntity key = keyValuePair2.Key;
			if (key.Layer == EntityLayer.Asteroid)
			{
				int id = key.GetComponent<WorldContainer>().id;
				keyValuePair2.Value.alertVignette.worldID = id;
			}
		}
	}

	// Token: 0x060063CF RID: 25551 RVA: 0x00252B78 File Offset: 0x00250D78
	private void RemoveDeletedEntities(object obj = null)
	{
		foreach (ClusterGridEntity key in (from x in this.m_gridEntityVis.Keys
		where x == null || x.gameObject == (GameObject)obj
		select x).ToList<ClusterGridEntity>())
		{
			global::Util.KDestroyGameObject(this.m_gridEntityVis[key]);
			this.m_gridEntityVis.Remove(key);
			this.m_gridEntityAnims.Remove(key);
		}
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x00252C18 File Offset: 0x00250E18
	private void OnClusterLocationChanged(object data)
	{
		this.UpdateVis(false);
	}

	// Token: 0x060063D1 RID: 25553 RVA: 0x00252C24 File Offset: 0x00250E24
	public static ClusterRevealLevel GetRevealLevel(ClusterGridEntity entity)
	{
		ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(entity.Location);
		ClusterRevealLevel isVisibleInFOW = entity.IsVisibleInFOW;
		if (cellRevealLevel == ClusterRevealLevel.Visible || isVisibleInFOW == ClusterRevealLevel.Visible)
		{
			return ClusterRevealLevel.Visible;
		}
		if (cellRevealLevel == ClusterRevealLevel.Peeked && isVisibleInFOW == ClusterRevealLevel.Peeked)
		{
			return ClusterRevealLevel.Peeked;
		}
		return ClusterRevealLevel.Hidden;
	}

	// Token: 0x060063D2 RID: 25554 RVA: 0x00252C60 File Offset: 0x00250E60
	private void UpdateVis(bool onShow = false)
	{
		this.SetupVisGameObjects();
		this.UpdatePaths();
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair in this.m_gridEntityAnims)
		{
			ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(keyValuePair.Key);
			keyValuePair.Value.Show(revealLevel);
			bool selected = this.m_selectedEntity == keyValuePair.Key;
			keyValuePair.Value.Select(selected);
			if (keyValuePair.Key.positionDirty || onShow)
			{
				Vector3 position = ClusterGrid.Instance.GetPosition(keyValuePair.Key);
				keyValuePair.Value.rectTransform().SetLocalPosition(position);
				keyValuePair.Key.positionDirty = false;
			}
		}
		if (this.m_selectedEntity != null && this.m_gridEntityVis.ContainsKey(this.m_selectedEntity))
		{
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.m_selectedEntity];
			this.m_selectMarker.SetTargetTransform(clusterMapVisualizer.transform);
			this.m_selectMarker.gameObject.SetActive(true);
			clusterMapVisualizer.transform.SetAsLastSibling();
		}
		else
		{
			this.m_selectMarker.gameObject.SetActive(false);
		}
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair2 in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair2.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair2.Key;
			component.SetRevealed(ClusterGrid.Instance.GetCellRevealLevel(key));
		}
		this.UpdateHexToggleStates();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x060063D3 RID: 25555 RVA: 0x00252E24 File Offset: 0x00251024
	private void OnEntityDestroyed(object obj)
	{
		this.RemoveDeletedEntities(null);
	}

	// Token: 0x060063D4 RID: 25556 RVA: 0x00252E30 File Offset: 0x00251030
	private void UpdateHexToggleStates()
	{
		bool flag = this.m_hoveredHex != null && ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_hoveredHex.location, EntityLayer.Asteroid);
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair.Key;
			ClusterMapHex.ToggleState state;
			if (this.m_selectedHex != null && this.m_selectedHex.location == key)
			{
				state = ClusterMapHex.ToggleState.Selected;
			}
			else if (flag && this.m_hoveredHex.location.IsAdjacent(key))
			{
				state = ClusterMapHex.ToggleState.OrbitHighlight;
			}
			else
			{
				state = ClusterMapHex.ToggleState.Unselected;
			}
			component.UpdateToggleState(state);
		}
	}

	// Token: 0x060063D5 RID: 25557 RVA: 0x00252F08 File Offset: 0x00251108
	public void SelectEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (entity != null)
		{
			this.SetSelectedEntity(entity, frameDelay);
			ClusterMapHex component = this.m_cellVisByLocation[entity.Location].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component;
		}
		this.UpdateVis(false);
	}

	// Token: 0x060063D6 RID: 25558 RVA: 0x00252F4C File Offset: 0x0025114C
	public void SelectHex(ClusterMapHex newSelectionHex)
	{
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			List<ClusterGridEntity> visibleEntitiesAtCell = ClusterGrid.Instance.GetVisibleEntitiesAtCell(newSelectionHex.location);
			for (int i = visibleEntitiesAtCell.Count - 1; i >= 0; i--)
			{
				KSelectable component = visibleEntitiesAtCell[i].GetComponent<KSelectable>();
				if (component == null || !component.IsSelectable)
				{
					visibleEntitiesAtCell.RemoveAt(i);
				}
			}
			if (visibleEntitiesAtCell.Count == 0)
			{
				this.SetSelectedEntity(null, false);
			}
			else
			{
				int num = visibleEntitiesAtCell.IndexOf(this.m_selectedEntity);
				int index = 0;
				if (num >= 0)
				{
					index = (num + 1) % visibleEntitiesAtCell.Count;
				}
				this.SetSelectedEntity(visibleEntitiesAtCell[index], false);
			}
			this.m_selectedHex = newSelectionHex;
		}
		else if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			global::Debug.Assert(this.m_destinationSelector != null, "Selected a hex in SelectDestination mode with no ClusterDestinationSelector");
			if (ClusterGrid.Instance.GetPath(this.m_selectedHex.location, newSelectionHex.location, this.m_destinationSelector) != null)
			{
				this.SetMode(ClusterMapScreen.Mode.FinishingSelectDestination);
				this.m_destinationSelector.SetDestination(newSelectionHex.location);
				if (this.m_closeOnSelect)
				{
					ManagementMenu.Instance.CloseAll();
				}
				else
				{
					this.SetMode(ClusterMapScreen.Mode.Default);
				}
			}
		}
		this.UpdateVis(false);
	}

	// Token: 0x060063D7 RID: 25559 RVA: 0x00253073 File Offset: 0x00251273
	public bool HasCurrentHover()
	{
		return this.m_hoveredHex != null;
	}

	// Token: 0x060063D8 RID: 25560 RVA: 0x00253081 File Offset: 0x00251281
	public AxialI GetCurrentHoverLocation()
	{
		return this.m_hoveredHex.location;
	}

	// Token: 0x060063D9 RID: 25561 RVA: 0x0025308E File Offset: 0x0025128E
	public void OnHoverHex(ClusterMapHex newHoverHex)
	{
		this.m_hoveredHex = newHoverHex;
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			this.UpdateVis(false);
		}
		this.UpdateHexToggleStates();
	}

	// Token: 0x060063DA RID: 25562 RVA: 0x002530AD File Offset: 0x002512AD
	public void OnUnhoverHex(ClusterMapHex unhoveredHex)
	{
		if (this.m_hoveredHex == unhoveredHex)
		{
			this.m_hoveredHex = null;
			this.UpdateHexToggleStates();
		}
	}

	// Token: 0x060063DB RID: 25563 RVA: 0x002530CA File Offset: 0x002512CA
	public void SetLocationHighlight(AxialI location, bool highlight)
	{
		this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>().ChangeState(highlight ? 1 : 0);
	}

	// Token: 0x060063DC RID: 25564 RVA: 0x002530E9 File Offset: 0x002512E9
	public ClusterMapHex GetClusterMapHexAtLocation(AxialI location)
	{
		return this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>();
	}

	// Token: 0x060063DD RID: 25565 RVA: 0x002530FC File Offset: 0x002512FC
	private void UpdatePaths()
	{
		ClusterDestinationSelector clusterDestinationSelector = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<ClusterDestinationSelector>() : null;
		if (this.m_mode != ClusterMapScreen.Mode.SelectDestination || !(this.m_hoveredHex != null))
		{
			if (this.m_previewMapPath != null)
			{
				global::Util.KDestroyGameObject(this.m_previewMapPath);
				this.m_previewMapPath = null;
			}
			return;
		}
		global::Debug.Assert(this.m_destinationSelector != null, "In SelectDestination mode without a destination selector");
		AxialI myWorldLocation = this.m_destinationSelector.GetMyWorldLocation();
		string text;
		List<AxialI> path = ClusterGrid.Instance.GetPath(myWorldLocation, this.m_hoveredHex.location, this.m_destinationSelector, out text, false);
		if (path != null)
		{
			if (this.m_previewMapPath == null)
			{
				this.m_previewMapPath = this.pathDrawer.AddPath();
			}
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.GetSelectorGridEntity(this.m_destinationSelector)];
			this.m_previewMapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(clusterMapVisualizer.transform.localPosition, path));
			this.m_previewMapPath.SetColor(this.rocketPreviewPathColor);
		}
		else if (this.m_previewMapPath != null)
		{
			global::Util.KDestroyGameObject(this.m_previewMapPath);
			this.m_previewMapPath = null;
		}
		int num = (path != null) ? path.Count : -1;
		if (this.m_selectedEntity != null)
		{
			IClusterRange component = this.m_selectedEntity.GetComponent<IClusterRange>();
			int rangeInTiles = component.GetRangeInTiles();
			int maxRangeInTiles = component.GetMaxRangeInTiles();
			if (num > rangeInTiles && string.IsNullOrEmpty(text))
			{
				text = GameUtil.SafeStringFormat(UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE, new object[]
				{
					rangeInTiles,
					maxRangeInTiles
				});
			}
			bool repeat = clusterDestinationSelector.GetComponent<RocketClusterDestinationSelector>().Repeat;
			this.m_hoveredHex.SetDestinationStatus(text, num, rangeInTiles, repeat);
			return;
		}
		this.m_hoveredHex.SetDestinationStatus(text);
	}

	// Token: 0x060063DE RID: 25566 RVA: 0x002532D0 File Offset: 0x002514D0
	private ClusterGridEntity GetSelectorGridEntity(ClusterDestinationSelector selector)
	{
		ClusterGridEntity component = selector.GetComponent<ClusterGridEntity>();
		if (component == null)
		{
			RocketModuleCluster component2 = selector.GetComponent<RocketModuleCluster>();
			if (component2 != null)
			{
				component = component2.CraftInterface.GetComponent<ClusterGridEntity>();
			}
		}
		if (component != null && ClusterGrid.Instance.IsVisible(component))
		{
			return component;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(selector.GetMyWorldLocation(), EntityLayer.Asteroid);
		global::Debug.Assert(component != null || visibleEntityOfLayerAtCell != null, string.Format("{0} has no grid entity and isn't located at a visible asteroid at {1}", selector, selector.GetMyWorldLocation()));
		if (visibleEntityOfLayerAtCell)
		{
			return visibleEntityOfLayerAtCell;
		}
		return component;
	}

	// Token: 0x060063DF RID: 25567 RVA: 0x0025336C File Offset: 0x0025156C
	private void UpdateTearStatus()
	{
		ClusterPOIManager clusterPOIManager = null;
		if (ClusterManager.Instance != null)
		{
			clusterPOIManager = ClusterManager.Instance.GetComponent<ClusterPOIManager>();
		}
		if (clusterPOIManager != null)
		{
			TemporalTear temporalTear = clusterPOIManager.GetTemporalTear();
			if (temporalTear != null)
			{
				temporalTear.UpdateStatus();
			}
		}
	}

	// Token: 0x040043C7 RID: 17351
	public static ClusterMapScreen Instance;

	// Token: 0x040043C8 RID: 17352
	public GameObject cellVisContainer;

	// Token: 0x040043C9 RID: 17353
	public GameObject terrainVisContainer;

	// Token: 0x040043CA RID: 17354
	public GameObject mobileVisContainer;

	// Token: 0x040043CB RID: 17355
	public GameObject telescopeVisContainer;

	// Token: 0x040043CC RID: 17356
	public GameObject POIVisContainer;

	// Token: 0x040043CD RID: 17357
	public GameObject DebriVisContainer;

	// Token: 0x040043CE RID: 17358
	public GameObject FXVisContainer;

	// Token: 0x040043CF RID: 17359
	public ClusterMapVisualizer cellVisPrefab;

	// Token: 0x040043D0 RID: 17360
	public ClusterMapVisualizer terrainVisPrefab;

	// Token: 0x040043D1 RID: 17361
	public ClusterMapVisualizer mobileVisPrefab;

	// Token: 0x040043D2 RID: 17362
	public ClusterMapVisualizer staticVisPrefab;

	// Token: 0x040043D3 RID: 17363
	public Color rocketPathColor;

	// Token: 0x040043D4 RID: 17364
	public Color rocketSelectedPathColor;

	// Token: 0x040043D5 RID: 17365
	public Color rocketPreviewPathColor;

	// Token: 0x040043D6 RID: 17366
	private ClusterMapHex m_selectedHex;

	// Token: 0x040043D7 RID: 17367
	private ClusterMapHex m_hoveredHex;

	// Token: 0x040043D8 RID: 17368
	private ClusterGridEntity m_selectedEntity;

	// Token: 0x040043D9 RID: 17369
	public KButton closeButton;

	// Token: 0x040043DA RID: 17370
	private const float ZOOM_SCALE_MIN = 50f;

	// Token: 0x040043DB RID: 17371
	private const float ZOOM_SCALE_MAX = 150f;

	// Token: 0x040043DC RID: 17372
	private const float ZOOM_SCALE_INCREMENT = 25f;

	// Token: 0x040043DD RID: 17373
	private const float ZOOM_SCALE_SPEED = 4f;

	// Token: 0x040043DE RID: 17374
	private const float ZOOM_NAME_THRESHOLD = 115f;

	// Token: 0x040043DF RID: 17375
	private float m_currentZoomScale = 75f;

	// Token: 0x040043E0 RID: 17376
	private float m_targetZoomScale = 75f;

	// Token: 0x040043E1 RID: 17377
	private ClusterMapPath m_previewMapPath;

	// Token: 0x040043E2 RID: 17378
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityVis = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x040043E3 RID: 17379
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityAnims = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x040043E4 RID: 17380
	private Dictionary<AxialI, ClusterMapVisualizer> m_cellVisByLocation = new Dictionary<AxialI, ClusterMapVisualizer>();

	// Token: 0x040043E5 RID: 17381
	private Action<object> m_onDestinationChangedDelegate;

	// Token: 0x040043E6 RID: 17382
	private Action<object> m_onSelectObjectDelegate;

	// Token: 0x040043E7 RID: 17383
	[SerializeField]
	private KScrollRect mapScrollRect;

	// Token: 0x040043E8 RID: 17384
	[SerializeField]
	private float scrollSpeed = 15f;

	// Token: 0x040043E9 RID: 17385
	public GameObject selectMarkerPrefab;

	// Token: 0x040043EA RID: 17386
	public ClusterMapPathDrawer pathDrawer;

	// Token: 0x040043EB RID: 17387
	private SelectMarker m_selectMarker;

	// Token: 0x040043EC RID: 17388
	private bool movingToTargetNISPosition;

	// Token: 0x040043ED RID: 17389
	private Vector3 targetNISPosition;

	// Token: 0x040043EE RID: 17390
	private float targetNISZoom;

	// Token: 0x040043EF RID: 17391
	private AxialI selectOnMoveNISComplete;

	// Token: 0x040043F0 RID: 17392
	private ClusterMapScreen.Mode m_mode;

	// Token: 0x040043F1 RID: 17393
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x040043F2 RID: 17394
	private bool m_closeOnSelect;

	// Token: 0x040043F3 RID: 17395
	private Coroutine activeMoveToTargetRoutine;

	// Token: 0x040043F4 RID: 17396
	public float floatCycleScale = 4f;

	// Token: 0x040043F5 RID: 17397
	public float floatCycleOffset = 0.75f;

	// Token: 0x040043F6 RID: 17398
	public float floatCycleSpeed = 0.75f;

	// Token: 0x02001EE0 RID: 7904
	public enum Mode
	{
		// Token: 0x040090ED RID: 37101
		Default,
		// Token: 0x040090EE RID: 37102
		SelectDestination,
		// Token: 0x040090EF RID: 37103
		FinishingSelectDestination
	}
}
