using System;
using System.Collections.Generic;
using System.Linq;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009B0 RID: 2480
[AddComponentMenu("KMonoBehaviour/scripts/InterfaceTool")]
public class InterfaceTool : KMonoBehaviour
{
	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x060047AF RID: 18351 RVA: 0x0019E8CA File Offset: 0x0019CACA
	public static InterfaceToolConfig ActiveConfig
	{
		get
		{
			if (InterfaceTool.interfaceConfigMap == null)
			{
				InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
			}
			return InterfaceTool.activeConfigs[InterfaceTool.activeConfigs.Count - 1];
		}
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x0019E8F0 File Offset: 0x0019CAF0
	public static void ToggleConfig(global::Action configKey)
	{
		if (InterfaceTool.interfaceConfigMap == null)
		{
			InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
		}
		InterfaceToolConfig item;
		if (!InterfaceTool.interfaceConfigMap.TryGetValue(configKey, out item))
		{
			global::Debug.LogWarning(string.Format("[InterfaceTool] No config is associated with Key: {0}!", configKey) + " Are you sure the configs were initialized properly!");
			return;
		}
		if (InterfaceTool.activeConfigs.BinarySearch(item, InterfaceToolConfig.ConfigComparer) <= 0)
		{
			global::Debug.Log(string.Format("[InterfaceTool] Pushing config with key: {0}", configKey));
			InterfaceTool.activeConfigs.Add(item);
			InterfaceTool.activeConfigs.Sort(InterfaceToolConfig.ConfigComparer);
			return;
		}
		global::Debug.Log(string.Format("[InterfaceTool] Popping config with key: {0}", configKey));
		InterfaceTool.activeConfigs.Remove(item);
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x0019E9A0 File Offset: 0x0019CBA0
	public static void InitializeConfigs(global::Action defaultKey, List<InterfaceToolConfig> configs)
	{
		string arg = (configs == null) ? "null" : configs.Count.ToString();
		global::Debug.Log(string.Format("[InterfaceTool] Initializing configs with values of DefaultKey: {0} Configs: {1}", defaultKey, arg));
		if (configs == null || configs.Count == 0)
		{
			InterfaceToolConfig interfaceToolConfig = ScriptableObject.CreateInstance<InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap = new Dictionary<global::Action, InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap[interfaceToolConfig.InputAction] = interfaceToolConfig;
			return;
		}
		InterfaceTool.interfaceConfigMap = configs.ToDictionary((InterfaceToolConfig x) => x.InputAction);
		InterfaceTool.ToggleConfig(defaultKey);
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x060047B2 RID: 18354 RVA: 0x0019EA39 File Offset: 0x0019CC39
	public HashedString ViewMode
	{
		get
		{
			return this.viewMode;
		}
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x0019EA41 File Offset: 0x0019CC41
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hoverTextConfiguration = base.GetComponent<HoverTextConfiguration>();
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x0019EA55 File Offset: 0x0019CC55
	public void ActivateTool()
	{
		this.OnActivateTool();
		this.OnMouseMove(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		Game.Instance.Trigger(1174281782, this);
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x0019EA80 File Offset: 0x0019CC80
	public virtual bool ShowHoverUI()
	{
		if (ManagementMenu.Instance == null || ManagementMenu.Instance.IsFullscreenUIActive())
		{
			return false;
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		if (OverlayScreen.Instance == null || !ClusterManager.Instance.IsPositionInActiveWorld(vector) || vector.x < 0f || vector.x > Grid.WidthInMeters || vector.y < 0f || vector.y > Grid.HeightInMeters)
		{
			return false;
		}
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		return current != null && !current.IsPointerOverGameObject();
	}

	// Token: 0x060047B6 RID: 18358 RVA: 0x0019EB24 File Offset: 0x0019CD24
	protected virtual void OnActivateTool()
	{
		if (OverlayScreen.Instance != null && this.viewMode != OverlayModes.None.ID && OverlayScreen.Instance.mode != this.viewMode)
		{
			OverlayScreen.Instance.ToggleOverlay(this.viewMode, true);
			InterfaceTool.toolActivatedViewMode = this.viewMode;
		}
		this.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x060047B7 RID: 18359 RVA: 0x0019EB98 File Offset: 0x0019CD98
	public void SetCurrentVirtualInputModuleMousMovementMode(bool mouseMovementOnly, Action<VirtualInputModule> extraActions = null)
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null && current.currentInputModule != null)
		{
			VirtualInputModule virtualInputModule = current.currentInputModule as VirtualInputModule;
			if (virtualInputModule != null)
			{
				virtualInputModule.mouseMovementOnly = mouseMovementOnly;
				if (extraActions != null)
				{
					extraActions(virtualInputModule);
				}
			}
		}
	}

	// Token: 0x060047B8 RID: 18360 RVA: 0x0019EBE8 File Offset: 0x0019CDE8
	public void DeactivateTool(InterfaceTool new_tool = null)
	{
		this.OnDeactivateTool(new_tool);
		if ((new_tool == null || new_tool == SelectTool.Instance) && InterfaceTool.toolActivatedViewMode != OverlayModes.None.ID && InterfaceTool.toolActivatedViewMode == SimDebugView.Instance.GetMode())
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			InterfaceTool.toolActivatedViewMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x0019EC53 File Offset: 0x0019CE53
	public virtual void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = null;
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x0019EC58 File Offset: 0x0019CE58
	protected virtual void OnDeactivateTool(InterfaceTool new_tool)
	{
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x0019EC5A File Offset: 0x0019CE5A
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x0019EC63 File Offset: 0x0019CE63
	public virtual string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x0019EC6C File Offset: 0x0019CE6C
	public virtual void OnMouseMove(Vector3 cursor_pos)
	{
		if (this.visualizer == null || !this.isAppFocused)
		{
			return;
		}
		cursor_pos = Grid.CellToPosCBC(Grid.PosToCell(cursor_pos), this.visualizerLayer);
		cursor_pos.z += -0.15f;
		this.visualizer.transform.SetLocalPosition(cursor_pos);
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x0019ECC8 File Offset: 0x0019CEC8
	public virtual void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x060047BF RID: 18367 RVA: 0x0019ECCA File Offset: 0x0019CECA
	public virtual void OnKeyUp(KButtonEvent e)
	{
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x0019ECCC File Offset: 0x0019CECC
	public virtual void OnLeftClickDown(Vector3 cursor_pos)
	{
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x0019ECCE File Offset: 0x0019CECE
	public virtual void OnLeftClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x0019ECD0 File Offset: 0x0019CED0
	public virtual void OnRightClickDown(Vector3 cursor_pos, KButtonEvent e)
	{
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x0019ECD2 File Offset: 0x0019CED2
	public virtual void OnRightClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x060047C4 RID: 18372 RVA: 0x0019ECD4 File Offset: 0x0019CED4
	public virtual void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
	}

	// Token: 0x060047C5 RID: 18373 RVA: 0x0019ECF8 File Offset: 0x0019CEF8
	protected Vector2 GetRegularizedPos(Vector2 input, bool minimize)
	{
		Vector3 vector = new Vector3(Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, 0f);
		return Grid.CellToPosCCC(Grid.PosToCell(input), Grid.SceneLayer.Background) + (minimize ? (-vector) : vector);
	}

	// Token: 0x060047C6 RID: 18374 RVA: 0x0019ED40 File Offset: 0x0019CF40
	protected Vector2 GetWorldRestrictedPosition(Vector2 input)
	{
		input.x = Mathf.Clamp(input.x, ClusterManager.Instance.activeWorld.minimumBounds.x, ClusterManager.Instance.activeWorld.maximumBounds.x);
		input.y = Mathf.Clamp(input.y, ClusterManager.Instance.activeWorld.minimumBounds.y, ClusterManager.Instance.activeWorld.maximumBounds.y);
		return input;
	}

	// Token: 0x060047C7 RID: 18375 RVA: 0x0019EDC4 File Offset: 0x0019CFC4
	protected void SetCursor(Texture2D new_cursor, Vector2 offset, CursorMode mode)
	{
		if (new_cursor != InterfaceTool.activeCursor && new_cursor != null)
		{
			InterfaceTool.activeCursor = new_cursor;
			try
			{
				Cursor.SetCursor(new_cursor, offset, mode);
				if (PlayerController.Instance.vim != null)
				{
					PlayerController.Instance.vim.SetCursor(new_cursor);
				}
			}
			catch (Exception ex)
			{
				string details = string.Format("SetCursor Failed new_cursor={0} offset={1} mode={2}", new_cursor, offset, mode);
				KCrashReporter.ReportDevNotification("SetCursor Failed", ex.StackTrace, details, false, null);
			}
		}
	}

	// Token: 0x060047C8 RID: 18376 RVA: 0x0019EE58 File Offset: 0x0019D058
	protected void UpdateHoverElements(List<KSelectable> hits)
	{
		if (this.hoverTextConfiguration != null)
		{
			this.hoverTextConfiguration.UpdateHoverElements(hits);
		}
	}

	// Token: 0x060047C9 RID: 18377 RVA: 0x0019EE74 File Offset: 0x0019D074
	public virtual void LateUpdate()
	{
		if (!this.populateHitsList)
		{
			this.UpdateHoverElements(null);
			return;
		}
		if (!this.isAppFocused)
		{
			return;
		}
		if (!Grid.IsValidCell(Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()))))
		{
			return;
		}
		this.hits.Clear();
		this.GetSelectablesUnderCursor(this.hits);
		KSelectable objectUnderCursor = this.GetObjectUnderCursor<KSelectable>(false, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, null);
		this.UpdateHoverElements(this.hits);
		if (!this.hasFocus && this.hoverOverride == null)
		{
			this.ClearHover();
		}
		else if (objectUnderCursor != this.hover)
		{
			this.ClearHover();
			this.hover = objectUnderCursor;
			if (objectUnderCursor != null)
			{
				Game.Instance.Trigger(2095258329, objectUnderCursor.gameObject);
				objectUnderCursor.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x060047CA RID: 18378 RVA: 0x0019EF78 File Offset: 0x0019D178
	public void GetSelectablesUnderCursor(List<KSelectable> hits)
	{
		if (this.hoverOverride != null)
		{
			hits.Add(this.hoverOverride);
		}
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 vector2 = new Vector2(vector.x, vector.y);
		int cell = Grid.PosToCell(vector);
		if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
		{
			return;
		}
		Game.Instance.statusItemRenderer.GetIntersections(vector2, hits);
		ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)vector2.x, (int)vector2.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		pooledList.Sort((ScenePartitionerEntry x, ScenePartitionerEntry y) => this.SortHoverCards(x, y));
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
			if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector2.x, vector2.y)))
			{
				KSelectable kselectable = kcollider2D.GetComponent<KSelectable>();
				if (kselectable == null)
				{
					kselectable = kcollider2D.GetComponentInParent<KSelectable>();
				}
				if (!(kselectable == null) && kselectable.isActiveAndEnabled && !hits.Contains(kselectable) && kselectable.IsSelectable)
				{
					hits.Add(kselectable);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x060047CB RID: 18379 RVA: 0x0019F11C File Offset: 0x0019D31C
	public void SetLinkCursor(bool set)
	{
		this.SetCursor(set ? Assets.GetTexture("cursor_hand") : this.cursor, set ? Vector2.zero : this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x060047CC RID: 18380 RVA: 0x0019F14C File Offset: 0x0019D34C
	protected T GetObjectUnderCursor<T>(bool cycleSelection, Func<T, bool> condition = null, Component previous_selection = null) where T : MonoBehaviour
	{
		this.intersections.Clear();
		this.GetObjectUnderCursor2D<T>(this.intersections, condition, this.layerMask);
		this.intersections.RemoveAll(new Predicate<InterfaceTool.Intersection>(InterfaceTool.is_component_null));
		if (this.intersections.Count <= 0)
		{
			this.prevIntersectionGroup.Clear();
			return default(T);
		}
		this.curIntersectionGroup.Clear();
		foreach (InterfaceTool.Intersection intersection in this.intersections)
		{
			this.curIntersectionGroup.Add(intersection.component);
		}
		if (!this.prevIntersectionGroup.Equals(this.curIntersectionGroup))
		{
			this.hitCycleCount = 0;
			this.prevIntersectionGroup = this.curIntersectionGroup;
		}
		this.intersections.Sort((InterfaceTool.Intersection a, InterfaceTool.Intersection b) => this.SortSelectables(a.component as KMonoBehaviour, b.component as KMonoBehaviour));
		int index = 0;
		if (cycleSelection)
		{
			index = this.hitCycleCount % this.intersections.Count;
			if (this.intersections[index].component != previous_selection || previous_selection == null)
			{
				index = 0;
				this.hitCycleCount = 0;
			}
			else
			{
				int num = this.hitCycleCount + 1;
				this.hitCycleCount = num;
				index = num % this.intersections.Count;
			}
		}
		return this.intersections[index].component as T;
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x0019F2CC File Offset: 0x0019D4CC
	private void GetObjectUnderCursor2D<T>(List<InterfaceTool.Intersection> intersections, Func<T, bool> condition, int layer_mask) where T : MonoBehaviour
	{
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 pos = new Vector2(vector.x, vector.y);
		if (this.hoverOverride != null)
		{
			intersections.Add(new InterfaceTool.Intersection
			{
				component = this.hoverOverride,
				distance = -100f
			});
		}
		int cell = Grid.PosToCell(vector);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			Game.Instance.statusItemRenderer.GetIntersections(pos, intersections);
			ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
			int x_bottomLeft = 0;
			int y_bottomLeft = 0;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
				if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector.x, vector.y)))
				{
					T t = kcollider2D.GetComponent<T>();
					if (t == null)
					{
						t = kcollider2D.GetComponentInParent<T>();
					}
					if (!(t == null) && (1 << t.gameObject.layer & layer_mask) != 0 && !(t == null) && (condition == null || condition(t)))
					{
						float num = t.transform.GetPosition().z - vector.z;
						bool flag = false;
						for (int i = 0; i < intersections.Count; i++)
						{
							InterfaceTool.Intersection intersection = intersections[i];
							if (intersection.component.gameObject == t.gameObject)
							{
								intersection.distance = Mathf.Min(intersection.distance, num);
								intersections[i] = intersection;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							intersections.Add(new InterfaceTool.Intersection
							{
								component = t,
								distance = num
							});
						}
					}
				}
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x0019F570 File Offset: 0x0019D770
	private int SortSelectables(KMonoBehaviour x, KMonoBehaviour y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		int num = x.transform.GetPosition().z.CompareTo(y.transform.GetPosition().z);
		if (num != 0)
		{
			return num;
		}
		return x.GetInstanceID().CompareTo(y.GetInstanceID());
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x0019F5E9 File Offset: 0x0019D7E9
	public void SetHoverOverride(KSelectable hover_override)
	{
		this.hoverOverride = hover_override;
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x0019F5F4 File Offset: 0x0019D7F4
	private int SortHoverCards(ScenePartitionerEntry x, ScenePartitionerEntry y)
	{
		KMonoBehaviour x2 = x.obj as KMonoBehaviour;
		KMonoBehaviour y2 = y.obj as KMonoBehaviour;
		return this.SortSelectables(x2, y2);
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x0019F621 File Offset: 0x0019D821
	private static bool is_component_null(InterfaceTool.Intersection intersection)
	{
		return !intersection.component;
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x0019F631 File Offset: 0x0019D831
	protected void ClearHover()
	{
		if (this.hover != null)
		{
			KSelectable kselectable = this.hover;
			this.hover = null;
			kselectable.Unhover();
			Game.Instance.Trigger(-1201923725, null);
		}
	}

	// Token: 0x04002FF4 RID: 12276
	private static Dictionary<global::Action, InterfaceToolConfig> interfaceConfigMap = null;

	// Token: 0x04002FF5 RID: 12277
	private static List<InterfaceToolConfig> activeConfigs = new List<InterfaceToolConfig>();

	// Token: 0x04002FF6 RID: 12278
	public const float MaxClickDistance = 0.02f;

	// Token: 0x04002FF7 RID: 12279
	public const float DepthBias = -0.15f;

	// Token: 0x04002FF8 RID: 12280
	public GameObject visualizer;

	// Token: 0x04002FF9 RID: 12281
	public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;

	// Token: 0x04002FFA RID: 12282
	public string placeSound;

	// Token: 0x04002FFB RID: 12283
	protected bool populateHitsList;

	// Token: 0x04002FFC RID: 12284
	[NonSerialized]
	public bool hasFocus;

	// Token: 0x04002FFD RID: 12285
	[SerializeField]
	protected Texture2D cursor;

	// Token: 0x04002FFE RID: 12286
	public Vector2 cursorOffset = new Vector2(2f, 2f);

	// Token: 0x04002FFF RID: 12287
	public System.Action OnDeactivate;

	// Token: 0x04003000 RID: 12288
	private static Texture2D activeCursor = null;

	// Token: 0x04003001 RID: 12289
	private static HashedString toolActivatedViewMode = OverlayModes.None.ID;

	// Token: 0x04003002 RID: 12290
	protected HashedString viewMode = OverlayModes.None.ID;

	// Token: 0x04003003 RID: 12291
	private HoverTextConfiguration hoverTextConfiguration;

	// Token: 0x04003004 RID: 12292
	private KSelectable hoverOverride;

	// Token: 0x04003005 RID: 12293
	public KSelectable hover;

	// Token: 0x04003006 RID: 12294
	protected int layerMask;

	// Token: 0x04003007 RID: 12295
	protected SelectMarker selectMarker;

	// Token: 0x04003008 RID: 12296
	private List<RaycastResult> castResults = new List<RaycastResult>();

	// Token: 0x04003009 RID: 12297
	private bool isAppFocused = true;

	// Token: 0x0400300A RID: 12298
	private List<KSelectable> hits = new List<KSelectable>();

	// Token: 0x0400300B RID: 12299
	protected bool playedSoundThisFrame;

	// Token: 0x0400300C RID: 12300
	private List<InterfaceTool.Intersection> intersections = new List<InterfaceTool.Intersection>();

	// Token: 0x0400300D RID: 12301
	private HashSet<Component> prevIntersectionGroup = new HashSet<Component>();

	// Token: 0x0400300E RID: 12302
	private HashSet<Component> curIntersectionGroup = new HashSet<Component>();

	// Token: 0x0400300F RID: 12303
	private int hitCycleCount;

	// Token: 0x02001A0F RID: 6671
	public struct Intersection
	{
		// Token: 0x0400806D RID: 32877
		public MonoBehaviour component;

		// Token: 0x0400806E RID: 32878
		public float distance;
	}
}
