using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using STRINGS;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public class Navigator : StateMachineComponent<Navigator.StatesInstance>, ISaveLoadableDetails
{
	// Token: 0x1700017E RID: 382
	// (get) Token: 0x0600245B RID: 9307 RVA: 0x000D20C1 File Offset: 0x000D02C1
	// (set) Token: 0x0600245C RID: 9308 RVA: 0x000D20C9 File Offset: 0x000D02C9
	public KMonoBehaviour target { get; set; }

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x0600245D RID: 9309 RVA: 0x000D20D2 File Offset: 0x000D02D2
	// (set) Token: 0x0600245E RID: 9310 RVA: 0x000D20DA File Offset: 0x000D02DA
	public CellOffset[] targetOffsets { get; private set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x0600245F RID: 9311 RVA: 0x000D20E3 File Offset: 0x000D02E3
	// (set) Token: 0x06002460 RID: 9312 RVA: 0x000D20EB File Offset: 0x000D02EB
	public NavGrid NavGrid { get; private set; }

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06002461 RID: 9313 RVA: 0x000D20F4 File Offset: 0x000D02F4
	// (set) Token: 0x06002462 RID: 9314 RVA: 0x000D20FC File Offset: 0x000D02FC
	public PathGrid PathGrid { get; set; }

	// Token: 0x06002463 RID: 9315 RVA: 0x000D2108 File Offset: 0x000D0308
	public void Serialize(BinaryWriter writer)
	{
		byte currentNavType = (byte)this.CurrentNavType;
		writer.Write(currentNavType);
		writer.Write(this.distanceTravelledByNavType.Count);
		foreach (KeyValuePair<NavType, int> keyValuePair in this.distanceTravelledByNavType)
		{
			byte key = (byte)keyValuePair.Key;
			writer.Write(key);
			writer.Write(keyValuePair.Value);
		}
	}

	// Token: 0x06002464 RID: 9316 RVA: 0x000D2190 File Offset: 0x000D0390
	public void Deserialize(IReader reader)
	{
		NavType navType = (NavType)reader.ReadByte();
		if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 11))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				NavType key = (NavType)reader.ReadByte();
				int value = reader.ReadInt32();
				if (this.distanceTravelledByNavType.ContainsKey(key))
				{
					this.distanceTravelledByNavType[key] = value;
				}
			}
		}
		bool flag = false;
		NavType[] validNavTypes = this.NavGrid.ValidNavTypes;
		for (int j = 0; j < validNavTypes.Length; j++)
		{
			if (validNavTypes[j] == navType)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.CurrentNavType = navType;
		}
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x000D2238 File Offset: 0x000D0438
	protected override void OnPrefabInit()
	{
		this.transitionDriver = new TransitionDriver(this);
		this.targetLocator = Util.KInstantiate(Assets.GetPrefab(TargetLocator.ID), null, null).GetComponent<KPrefabID>();
		this.targetLocator.gameObject.SetActive(true);
		this.log = new LoggerFSS("Navigator", 35);
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
		this.NavGrid = Pathfinding.Instance.GetNavGrid(this.NavGridName);
		if (this.maxProbeRadiusX != 0 || this.maxProbeRadiusY != 0)
		{
			this.PathGrid = new PathGrid(this.maxProbeRadiusX * 2 + 1, this.maxProbeRadiusY * 2 + 1, true, this.NavGrid.ValidNavTypes);
		}
		else
		{
			this.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, this.NavGrid.ValidNavTypes);
		}
		this.distanceTravelledByNavType = new Dictionary<NavType, int>();
		for (int i = 0; i < 11; i++)
		{
			this.distanceTravelledByNavType.Add((NavType)i, 0);
		}
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x000D2340 File Offset: 0x000D0540
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Navigator>(1623392196, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(-1506500077, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(493375141, Navigator.OnRefreshUserMenuDelegate);
		base.Subscribe<Navigator>(-1503271301, Navigator.OnSelectObjectDelegate);
		base.Subscribe<Navigator>(856640610, Navigator.OnStoreDelegate);
		base.Subscribe<Navigator>(1502190696, Navigator.OnQueueDestroyDelegate);
		if (this.updateProber)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
		if (this.executePathProbeTaskAsync)
		{
			AsyncPathProber.Instance.Register(this);
		}
		this.cachedCell = Grid.PosToCell(this);
		this.SetCurrentNavType(this.CurrentNavType);
		this.OnBuildingTileChangedAction = new Action<int, object>(this.OnBuildingTileChanged);
		this.SubscribeUnstuckFunctions();
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x000D2410 File Offset: 0x000D0610
	private void SubscribeUnstuckFunctions()
	{
		if (this.CurrentNavType == NavType.Tube)
		{
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], this.OnBuildingTileChangedAction);
		}
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x000D2437 File Offset: 0x000D0637
	private void UnsubscribeUnstuckFunctions()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], this.OnBuildingTileChangedAction);
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x000D2458 File Offset: 0x000D0658
	private void OnBuildingTileChanged(int cell, object building)
	{
		if (this.CurrentNavType == NavType.Tube && building == null)
		{
			bool flag = cell == Grid.PosToCell(this);
			if (base.smi != null && flag)
			{
				this.SetCurrentNavType(NavType.Floor);
				this.UnsubscribeUnstuckFunctions();
			}
		}
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000D2495 File Offset: 0x000D0695
	protected override void OnCleanUp()
	{
		this.UnsubscribeUnstuckFunctions();
		base.OnCleanUp();
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000D24A3 File Offset: 0x000D06A3
	protected void OnQueueDestroy()
	{
		if (this.executePathProbeTaskAsync)
		{
			AsyncPathProber.Instance.Unregister(this);
		}
		if (this.reportOccupation)
		{
			MinionGroupProber.Get().Vacate(this.occupiedCells);
		}
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000D24D0 File Offset: 0x000D06D0
	public PathGrid TakeResult(ref AsyncPathProber.WorkResult result)
	{
		PathGrid pathGrid = this.PathGrid;
		this.PathGrid = result.pathGrid;
		if (this.reportOccupation)
		{
			List<int> reachableCells = this.occupiedCells;
			MinionGroupProber.Get().OccupyST(result.newlyReachableCells);
			MinionGroupProber.Get().VacateST(result.noLongerReachableCells);
			this.occupiedCells = result.reachableCells;
			result.reachableCells = reachableCells;
		}
		return pathGrid;
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x000D2531 File Offset: 0x000D0731
	public bool IsMoving()
	{
		return base.smi.IsInsideState(base.smi.sm.normal.moving);
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x000D2553 File Offset: 0x000D0753
	public bool GoTo(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, NavigationTactics.ReduceTravelDistance);
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000D258B File Offset: 0x000D078B
	public bool GoTo(int cell, CellOffset[] offsets, NavTactic tactic)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, tactic);
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000D25BF File Offset: 0x000D07BF
	public void UpdateTarget(int cell)
	{
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000D25DC File Offset: 0x000D07DC
	public bool GoTo(KMonoBehaviour target, CellOffset[] offsets, NavTactic tactic)
	{
		if (tactic == null)
		{
			tactic = NavigationTactics.ReduceTravelDistance;
		}
		base.smi.GoTo(base.smi.sm.normal.moving);
		base.smi.sm.moveTarget.Set(target.gameObject, base.smi, false);
		this.tactic = tactic;
		this.target = target;
		this.targetOffsets = offsets;
		this.ClearReservedCell();
		this.AdvancePath(true);
		return this.IsMoving();
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000D265E File Offset: 0x000D085E
	public void BeginTransition(NavGrid.Transition transition)
	{
		this.transitionDriver.EndTransition();
		base.smi.GoTo(base.smi.sm.normal.moving);
		this.transitionDriver.BeginTransition(this, transition, this.defaultSpeed);
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x000D26A0 File Offset: 0x000D08A0
	private bool ValidatePath(ref PathFinder.Path path, out bool atNextNode)
	{
		atNextNode = false;
		bool flag = false;
		if (path.IsValid())
		{
			int target_cell = Grid.PosToCell(this.target);
			flag = (this.reservedCell != NavigationReservations.InvalidReservation && this.CanReach(this.reservedCell));
			flag &= Grid.IsCellOffsetOf(this.reservedCell, target_cell, this.targetOffsets);
		}
		if (flag)
		{
			int num = Grid.PosToCell(this);
			flag = (num == path.nodes[0].cell && this.CurrentNavType == path.nodes[0].navType);
			flag |= (atNextNode = (num == path.nodes[1].cell && this.CurrentNavType == path.nodes[1].navType));
		}
		if (!flag)
		{
			return false;
		}
		PathFinderAbilities currentAbilities = this.GetCurrentAbilities();
		return PathFinder.ValidatePath(this.NavGrid, currentAbilities, ref path, this.flags);
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000D278C File Offset: 0x000D098C
	private bool TryBuildPathFromCache(int cachedCell, int reservedCell, ref PathFinder.Path path)
	{
		bool flag;
		return this.PathGrid.BuildPath(cachedCell, reservedCell, this.CurrentNavType, ref path) && this.ValidatePath(ref path, out flag);
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x000D27BC File Offset: 0x000D09BC
	public void AdvancePath(bool trigger_advance = true)
	{
		this.cachedCell = Grid.PosToCell(this);
		if (this.target == null)
		{
			if (!this.Stop(false, true))
			{
				base.Trigger(-766531887, null);
			}
		}
		else if (this.cachedCell == this.reservedCell && this.CurrentNavType != NavType.Tube)
		{
			this.Stop(true, true);
		}
		else
		{
			bool flag;
			if (!this.ValidatePath(ref this.path, out flag))
			{
				int root = Grid.PosToCell(this.target);
				int cellPreferences = this.tactic.GetCellPreferences(root, this.targetOffsets, this);
				this.SetReservedCell(cellPreferences);
				if (this.reservedCell == NavigationReservations.InvalidReservation)
				{
					this.path.Clear();
				}
				else if (!this.TryBuildPathFromCache(this.cachedCell, this.reservedCell, ref this.path))
				{
					PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(this.cachedCell, this.CurrentNavType, this.flags);
					PathFinder.UpdatePath(this.NavGrid, this.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(this.reservedCell), ref this.path);
					if (this.executePathProbeTaskAsync)
					{
						AsyncPathProber.Instance.ApplyNavigationFailedPenalty(this);
					}
				}
			}
			else if (flag)
			{
				this.path.nodes.RemoveAt(0);
			}
			if (this.path.IsValid())
			{
				this.BeginTransition(this.NavGrid.transitions[(int)this.path.nodes[1].transitionId]);
				this.distanceTravelledByNavType[this.CurrentNavType] = Mathf.Max(this.distanceTravelledByNavType[this.CurrentNavType] + 1, this.distanceTravelledByNavType[this.CurrentNavType]);
			}
			else if (this.path.HasArrived())
			{
				this.Stop(true, true);
			}
			else
			{
				this.ClearReservedCell();
				this.Stop(false, true);
			}
		}
		if (trigger_advance)
		{
			base.Trigger(1347184327, null);
		}
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000D29AF File Offset: 0x000D0BAF
	public NavGrid.Transition GetNextTransition()
	{
		return this.NavGrid.transitions[(int)this.path.nodes[1].transitionId];
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x000D29D8 File Offset: 0x000D0BD8
	public bool Stop(bool arrived_at_destination = false, bool play_idle = true)
	{
		this.target = null;
		this.targetOffsets = null;
		this.path.Clear();
		base.smi.sm.moveTarget.Set(null, base.smi);
		this.transitionDriver.EndTransition();
		if (play_idle)
		{
			HashedString idleAnim = this.NavGrid.GetIdleAnim(this.CurrentNavType);
			this.animController.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}
		if (arrived_at_destination)
		{
			base.smi.GoTo(base.smi.sm.normal.arrived);
			return true;
		}
		if (base.smi.GetCurrentState() == base.smi.sm.normal.moving)
		{
			this.ClearReservedCell();
			base.smi.GoTo(base.smi.sm.normal.failed);
			return true;
		}
		return false;
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x000D2AC1 File Offset: 0x000D0CC1
	private void SimEveryTick(float dt)
	{
		if (this.IsMoving())
		{
			this.transitionDriver.UpdateTransition(dt);
		}
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x000D2AD8 File Offset: 0x000D0CD8
	public void UpdateProbe(bool forceUpdate = false)
	{
		if (forceUpdate || !this.executePathProbeTaskAsync)
		{
			if (this.reportOccupation)
			{
				ListPool<int, Navigator>.PooledList pooledList = ListPool<int, Navigator>.Allocate();
				PathProber.Run(this, pooledList);
				MinionGroupProber.Get().Occupy(pooledList);
				MinionGroupProber.Get().Vacate(this.occupiedCells);
				this.occupiedCells.Clear();
				this.occupiedCells.AddRange(pooledList);
				pooledList.Recycle();
				return;
			}
			PathProber.Run(this, null);
		}
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000D2B45 File Offset: 0x000D0D45
	public void DrawPath()
	{
		if (base.gameObject.activeInHierarchy && this.IsMoving())
		{
			NavPathDrawer.Instance.DrawPath(this.animController.GetPivotSymbolPosition(), this.path);
		}
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000D2B77 File Offset: 0x000D0D77
	public void Pause(string reason)
	{
		base.smi.sm.isPaused.Set(true, base.smi, false);
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x000D2B97 File Offset: 0x000D0D97
	public void Unpause(string reason)
	{
		base.smi.sm.isPaused.Set(false, base.smi, false);
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x000D2BB7 File Offset: 0x000D0DB7
	private void OnDefeated(object data)
	{
		this.ClearReservedCell();
		this.Stop(false, false);
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x000D2BC8 File Offset: 0x000D0DC8
	private void ClearReservedCell()
	{
		if (this.reservedCell != NavigationReservations.InvalidReservation)
		{
			NavigationReservations.Instance.RemoveOccupancy(this.reservedCell);
			this.reservedCell = NavigationReservations.InvalidReservation;
		}
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x000D2BF2 File Offset: 0x000D0DF2
	private void SetReservedCell(int cell)
	{
		this.ClearReservedCell();
		this.reservedCell = cell;
		NavigationReservations.Instance.AddOccupancy(cell);
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x000D2C0C File Offset: 0x000D0E0C
	public int GetReservedCell()
	{
		return this.reservedCell;
	}

	// Token: 0x06002481 RID: 9345 RVA: 0x000D2C14 File Offset: 0x000D0E14
	public int GetAnchorCell()
	{
		return this.AnchorCell;
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000D2C1C File Offset: 0x000D0E1C
	public bool IsValidNavType(NavType nav_type)
	{
		return this.NavGrid.HasNavTypeData(nav_type);
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x000D2C2C File Offset: 0x000D0E2C
	public void SetCurrentNavType(NavType nav_type)
	{
		this.CurrentNavType = nav_type;
		this.AnchorCell = NavTypeHelper.GetAnchorCell(nav_type, Grid.PosToCell(this));
		NavGrid.NavTypeData navTypeData = this.NavGrid.GetNavTypeData(this.CurrentNavType);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Vector2 one = Vector2.one;
		if (navTypeData.flipX)
		{
			one.x = -1f;
		}
		if (navTypeData.flipY)
		{
			one.y = -1f;
		}
		component.navMatrix = Matrix2x3.Translate(navTypeData.animControllerOffset * 200f) * Matrix2x3.Rotate(navTypeData.rotation) * Matrix2x3.Scale(one);
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000D2CD0 File Offset: 0x000D0ED0
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (NavPathDrawer.Instance.GetNavigator() != this) ? new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME_OFF, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0.1f);
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", UI.USERMENUACTIONS.FOLLOWCAM.NAME, new System.Action(this.OnFollowCam), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP, true), 0.3f);
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000D2DD3 File Offset: 0x000D0FD3
	private void OnFollowCam()
	{
		if (CameraController.Instance.followTarget == base.transform)
		{
			CameraController.Instance.ClearFollowTarget();
			return;
		}
		CameraController.Instance.SetFollowTarget(base.transform);
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x000D2E07 File Offset: 0x000D1007
	private void OnDrawPaths()
	{
		if (NavPathDrawer.Instance.GetNavigator() != this)
		{
			NavPathDrawer.Instance.SetNavigator(this);
			return;
		}
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x000D2E31 File Offset: 0x000D1031
	private void OnSelectObject(object data)
	{
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x000D2E3D File Offset: 0x000D103D
	public void OnStore(object data)
	{
		if (data is Storage || (data != null && ((Boxed<bool>)data).value))
		{
			this.Stop(false, true);
		}
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x000D2E66 File Offset: 0x000D1066
	public PathFinderAbilities GetCurrentAbilities()
	{
		this.abilities.Refresh();
		return this.abilities;
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x000D2E79 File Offset: 0x000D1079
	public void SetAbilities(PathFinderAbilities abilities)
	{
		this.abilities = abilities;
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x000D2E82 File Offset: 0x000D1082
	public bool CanReach(IApproachable approachable)
	{
		return this.CanReach(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000D2E98 File Offset: 0x000D1098
	public bool CanReach(int cell, CellOffset[] offsets)
	{
		foreach (CellOffset offset in offsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			if (this.CanReach(cell2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000D2ED1 File Offset: 0x000D10D1
	public bool CanReach(int cell)
	{
		return this.GetNavigationCost(cell) != -1;
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x000D2EE0 File Offset: 0x000D10E0
	public int GetNavigationCost(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return this.PathGrid.GetCost(cell);
		}
		return -1;
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x000D2EF8 File Offset: 0x000D10F8
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathGrid.GetCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x000D2F08 File Offset: 0x000D1108
	public int GetNavigationCost(int cell, CellOffset[] offsets)
	{
		int num = -1;
		int num2 = offsets.Length;
		for (int i = 0; i < num2; i++)
		{
			int cell2 = Grid.OffsetCell(cell, offsets[i]);
			int navigationCost = this.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x000D2F50 File Offset: 0x000D1150
	public int GetNavigationCost(int cell, IReadOnlyList<CellOffset> offsets)
	{
		int num = -1;
		int count = offsets.Count;
		for (int i = 0; i < count; i++)
		{
			int cell2 = Grid.OffsetCell(cell, offsets[i]);
			int navigationCost = this.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x000D2F9B File Offset: 0x000D119B
	public int GetNavigationCost(IApproachable approachable)
	{
		return this.GetNavigationCost(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x06002493 RID: 9363 RVA: 0x000D2FB0 File Offset: 0x000D11B0
	public void RunQuery(PathFinderQuery query)
	{
		int cell = Grid.PosToCell(this);
		PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(cell, this.CurrentNavType, this.flags);
		PathFinder.Run(this.NavGrid, this.GetCurrentAbilities(), potential_path, query);
	}

	// Token: 0x06002494 RID: 9364 RVA: 0x000D2FEB File Offset: 0x000D11EB
	public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags |= new_flags;
	}

	// Token: 0x06002495 RID: 9365 RVA: 0x000D2FFB File Offset: 0x000D11FB
	public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags &= ~new_flags;
	}

	// Token: 0x06002496 RID: 9366 RVA: 0x000D300D File Offset: 0x000D120D
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06002497 RID: 9367 RVA: 0x000D300F File Offset: 0x000D120F
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x04001536 RID: 5430
	public bool DebugDrawPath;

	// Token: 0x0400153A RID: 5434
	[MyCmpAdd]
	public Facing facing;

	// Token: 0x0400153B RID: 5435
	public float defaultSpeed = 1f;

	// Token: 0x0400153C RID: 5436
	public TransitionDriver transitionDriver;

	// Token: 0x0400153D RID: 5437
	public string NavGridName;

	// Token: 0x0400153E RID: 5438
	public bool updateProber;

	// Token: 0x0400153F RID: 5439
	public int maxProbeRadiusX;

	// Token: 0x04001540 RID: 5440
	public int maxProbeRadiusY;

	// Token: 0x04001541 RID: 5441
	public PathFinder.PotentialPath.Flags flags;

	// Token: 0x04001542 RID: 5442
	private LoggerFSS log;

	// Token: 0x04001543 RID: 5443
	public Dictionary<NavType, int> distanceTravelledByNavType;

	// Token: 0x04001545 RID: 5445
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Move;

	// Token: 0x04001546 RID: 5446
	private PathFinderAbilities abilities;

	// Token: 0x04001547 RID: 5447
	[MyCmpReq]
	public KBatchedAnimController animController;

	// Token: 0x04001548 RID: 5448
	[NonSerialized]
	public PathFinder.Path path;

	// Token: 0x04001549 RID: 5449
	public NavType CurrentNavType;

	// Token: 0x0400154A RID: 5450
	private int AnchorCell;

	// Token: 0x0400154B RID: 5451
	private KPrefabID targetLocator;

	// Token: 0x0400154C RID: 5452
	public int cachedCell;

	// Token: 0x0400154D RID: 5453
	private int reservedCell = NavigationReservations.InvalidReservation;

	// Token: 0x0400154E RID: 5454
	private NavTactic tactic;

	// Token: 0x0400154F RID: 5455
	public bool reportOccupation;

	// Token: 0x04001550 RID: 5456
	public List<int> occupiedCells = new List<int>();

	// Token: 0x04001551 RID: 5457
	private Action<int, object> OnBuildingTileChangedAction;

	// Token: 0x04001552 RID: 5458
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04001553 RID: 5459
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001554 RID: 5460
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnSelectObjectDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnSelectObject(data);
	});

	// Token: 0x04001555 RID: 5461
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnStoreDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04001556 RID: 5462
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnQueueDestroyDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnQueueDestroy();
	});

	// Token: 0x04001557 RID: 5463
	public bool executePathProbeTaskAsync;

	// Token: 0x020014E2 RID: 5346
	public class ActiveTransition
	{
		// Token: 0x06009169 RID: 37225 RVA: 0x00370F68 File Offset: 0x0036F168
		public void Init(NavGrid.Transition transition, float default_speed)
		{
			this.x = transition.x;
			this.y = transition.y;
			this.isLooping = transition.isLooping;
			this.start = transition.start;
			this.end = transition.end;
			this.preAnim = transition.preAnim;
			this.anim = transition.anim;
			this.speed = default_speed;
			this.animSpeed = transition.animSpeed;
			this.navGridTransition = transition;
		}

		// Token: 0x0600916A RID: 37226 RVA: 0x00370FF0 File Offset: 0x0036F1F0
		public void Copy(Navigator.ActiveTransition other)
		{
			this.x = other.x;
			this.y = other.y;
			this.isLooping = other.isLooping;
			this.start = other.start;
			this.end = other.end;
			this.preAnim = other.preAnim;
			this.anim = other.anim;
			this.speed = other.speed;
			this.animSpeed = other.animSpeed;
			this.navGridTransition = other.navGridTransition;
		}

		// Token: 0x04006FDC RID: 28636
		public int x;

		// Token: 0x04006FDD RID: 28637
		public int y;

		// Token: 0x04006FDE RID: 28638
		public bool isLooping;

		// Token: 0x04006FDF RID: 28639
		public NavType start;

		// Token: 0x04006FE0 RID: 28640
		public NavType end;

		// Token: 0x04006FE1 RID: 28641
		public HashedString preAnim;

		// Token: 0x04006FE2 RID: 28642
		public HashedString anim;

		// Token: 0x04006FE3 RID: 28643
		public float speed;

		// Token: 0x04006FE4 RID: 28644
		public float animSpeed = 1f;

		// Token: 0x04006FE5 RID: 28645
		public Func<bool> isCompleteCB;

		// Token: 0x04006FE6 RID: 28646
		public NavGrid.Transition navGridTransition;
	}

	// Token: 0x020014E3 RID: 5347
	public class StatesInstance : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.GameInstance
	{
		// Token: 0x0600916C RID: 37228 RVA: 0x00371088 File Offset: 0x0036F288
		public StatesInstance(Navigator master) : base(master)
		{
		}
	}

	// Token: 0x020014E4 RID: 5348
	public class States : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator>
	{
		// Token: 0x0600916D RID: 37229 RVA: 0x00371094 File Offset: 0x0036F294
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal.stopped;
			this.saveHistory = true;
			this.normal.ParamTransition<bool>(this.isPaused, this.paused, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsTrue);
			this.normal.moving.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.BoxingTrigger<GameHashes>(1027377649, GameHashes.ObjectMovementWakeUp);
			}).Update("UpdateNavigator", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.SimEveryTick(dt);
			}, UpdateRate.SIM_EVERY_TICK, true).Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.BoxingTrigger<GameHashes>(1027377649, GameHashes.ObjectMovementSleep);
			});
			this.normal.arrived.TriggerOnEnter(GameHashes.DestinationReached, null).GoTo(this.normal.stopped);
			this.normal.failed.TriggerOnEnter(GameHashes.NavigationFailed, null).GoTo(this.normal.stopped);
			this.normal.stopped.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.master.SubscribeUnstuckFunctions();
			}).DoNothing().Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.master.UnsubscribeUnstuckFunctions();
			});
			this.paused.ParamTransition<bool>(this.isPaused, this.normal, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsFalse);
		}

		// Token: 0x04006FE7 RID: 28647
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.TargetParameter moveTarget;

		// Token: 0x04006FE8 RID: 28648
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter isPaused = new StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter(false);

		// Token: 0x04006FE9 RID: 28649
		public Navigator.States.NormalStates normal;

		// Token: 0x04006FEA RID: 28650
		public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State paused;

		// Token: 0x020028A5 RID: 10405
		public class NormalStates : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State
		{
			// Token: 0x0400B30A RID: 45834
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State moving;

			// Token: 0x0400B30B RID: 45835
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State arrived;

			// Token: 0x0400B30C RID: 45836
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State failed;

			// Token: 0x0400B30D RID: 45837
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State stopped;
		}
	}

	// Token: 0x020014E5 RID: 5349
	public class Scanner<T> where T : KMonoBehaviour
	{
		// Token: 0x0600916F RID: 37231 RVA: 0x00371229 File Offset: 0x0036F429
		public Scanner(int radius, ScenePartitionerLayer layer, Func<T, bool> filterFn)
		{
			this.radius = radius;
			this.layer = layer;
			this.filterFn = filterFn;
			this.offsets = Navigator.Scanner<T>.NO_OFFSETS;
			this.offsetsFn = null;
			this.early_out_threshold = null;
		}

		// Token: 0x06009170 RID: 37232 RVA: 0x00371264 File Offset: 0x0036F464
		public void SetConstantOffsets(CellOffset[] offsets)
		{
			this.offsets = offsets;
		}

		// Token: 0x06009171 RID: 37233 RVA: 0x0037126D File Offset: 0x0036F46D
		public void SetDynamicOffsetsFn(Action<T, List<CellOffset>> offsetsFn)
		{
			this.offsetsFn = offsetsFn;
		}

		// Token: 0x06009172 RID: 37234 RVA: 0x00371276 File Offset: 0x0036F476
		public void SetEarlyOutThreshold(int early_out_threshold)
		{
			this.early_out_threshold = new int?(early_out_threshold);
		}

		// Token: 0x06009173 RID: 37235 RVA: 0x00371284 File Offset: 0x0036F484
		private int NavCostFromConstantOffsets(Navigator navigator, T destinationObject, CellOffset[] offsets)
		{
			return navigator.GetNavigationCost(Grid.PosToCell(destinationObject.gameObject), offsets);
		}

		// Token: 0x06009174 RID: 37236 RVA: 0x003712A0 File Offset: 0x0036F4A0
		private int NavCostFromDynamicOffsets(Navigator navigator, T destinationObject, Action<T, List<CellOffset>> offsetsFn)
		{
			ListPool<CellOffset, Navigator>.PooledList pooledList = ListPool<CellOffset, Navigator>.Allocate();
			offsetsFn(destinationObject, pooledList);
			int navigationCost = navigator.GetNavigationCost(Grid.PosToCell(destinationObject.gameObject), pooledList);
			pooledList.Recycle();
			return navigationCost;
		}

		// Token: 0x06009175 RID: 37237 RVA: 0x003712D8 File Offset: 0x0036F4D8
		public T Scan(Vector2I gridPos, Navigator navigator)
		{
			ListPool<ScenePartitionerEntry, Navigator>.PooledList pooledList = ListPool<ScenePartitionerEntry, Navigator>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(gridPos.x - this.radius, gridPos.y - this.radius, this.radius * 2, this.radius * 2, this.layer, pooledList);
			T t = default(T);
			int num = -1;
			if (this.early_out_threshold != null)
			{
				pooledList.Shuffle<ScenePartitionerEntry>();
				if (this.offsetsFn != null)
				{
					for (int i = 0; i < pooledList.Count; i++)
					{
						T t2 = pooledList[i].obj as T;
						if (this.filterFn(t2))
						{
							int num2 = this.NavCostFromDynamicOffsets(navigator, t2, this.offsetsFn);
							if (num2 != -1 && (t == null || num2 < num))
							{
								t = t2;
								num = num2;
								if (num2 <= this.early_out_threshold.Value)
								{
									break;
								}
							}
						}
					}
				}
				else
				{
					for (int j = 0; j < pooledList.Count; j++)
					{
						T t3 = pooledList[j].obj as T;
						if (this.filterFn(t3))
						{
							int num3 = this.NavCostFromConstantOffsets(navigator, t3, this.offsets);
							if (num3 != -1 && (t == null || num3 < num))
							{
								t = t3;
								num = num3;
								if (num3 <= this.early_out_threshold.Value)
								{
									break;
								}
							}
						}
					}
				}
			}
			else if (this.offsetsFn != null)
			{
				for (int k = 0; k < pooledList.Count; k++)
				{
					T t4 = pooledList[k].obj as T;
					if (this.filterFn(t4))
					{
						int num4 = this.NavCostFromDynamicOffsets(navigator, t4, this.offsetsFn);
						if (num4 != -1 && (t == null || num4 < num))
						{
							t = t4;
							num = num4;
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < pooledList.Count; l++)
				{
					T t5 = pooledList[l].obj as T;
					if (this.filterFn(t5))
					{
						int num5 = this.NavCostFromConstantOffsets(navigator, t5, this.offsets);
						if (num5 != -1 && (t == null || num5 < num))
						{
							t = t5;
							num = num5;
						}
					}
				}
			}
			pooledList.Recycle();
			return t;
		}

		// Token: 0x04006FEB RID: 28651
		private static readonly CellOffset[] NO_OFFSETS = new CellOffset[]
		{
			new CellOffset(0, 0)
		};

		// Token: 0x04006FEC RID: 28652
		private readonly int radius;

		// Token: 0x04006FED RID: 28653
		private readonly ScenePartitionerLayer layer;

		// Token: 0x04006FEE RID: 28654
		private readonly Func<T, bool> filterFn;

		// Token: 0x04006FEF RID: 28655
		private CellOffset[] offsets;

		// Token: 0x04006FF0 RID: 28656
		private Action<T, List<CellOffset>> offsetsFn;

		// Token: 0x04006FF1 RID: 28657
		private int? early_out_threshold;
	}
}
