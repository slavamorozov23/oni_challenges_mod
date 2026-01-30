using System;
using System.Collections.Generic;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000AB0 RID: 2736
[AddComponentMenu("KMonoBehaviour/scripts/PlayerController")]
public class PlayerController : KMonoBehaviour, IInputHandler
{
	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06004F53 RID: 20307 RVA: 0x001CCD6D File Offset: 0x001CAF6D
	public string handlerName
	{
		get
		{
			return "PlayerController";
		}
	}

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06004F54 RID: 20308 RVA: 0x001CCD74 File Offset: 0x001CAF74
	// (set) Token: 0x06004F55 RID: 20309 RVA: 0x001CCD7C File Offset: 0x001CAF7C
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06004F56 RID: 20310 RVA: 0x001CCD85 File Offset: 0x001CAF85
	public InterfaceTool ActiveTool
	{
		get
		{
			return this.activeTool;
		}
	}

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06004F57 RID: 20311 RVA: 0x001CCD8D File Offset: 0x001CAF8D
	// (set) Token: 0x06004F58 RID: 20312 RVA: 0x001CCD94 File Offset: 0x001CAF94
	public static PlayerController Instance { get; private set; }

	// Token: 0x06004F59 RID: 20313 RVA: 0x001CCD9C File Offset: 0x001CAF9C
	public static void DestroyInstance()
	{
		PlayerController.Instance = null;
	}

	// Token: 0x06004F5A RID: 20314 RVA: 0x001CCDA4 File Offset: 0x001CAFA4
	protected override void OnPrefabInit()
	{
		PlayerController.Instance = this;
		InterfaceTool.InitializeConfigs(this.defaultConfigKey, this.interfaceConfigs);
		this.vim = UnityEngine.Object.FindObjectOfType<VirtualInputModule>(true);
		for (int i = 0; i < this.tools.Length; i++)
		{
			GameObject gameObject = Util.KInstantiate(this.tools[i].gameObject, base.gameObject, null);
			this.tools[i] = gameObject.GetComponent<InterfaceTool>();
			this.tools[i].gameObject.SetActive(true);
			this.tools[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06004F5B RID: 20315 RVA: 0x001CCE35 File Offset: 0x001CB035
	protected override void OnSpawn()
	{
		if (this.tools.Length == 0)
		{
			return;
		}
		this.ActivateTool(this.tools[0]);
	}

	// Token: 0x06004F5C RID: 20316 RVA: 0x001CCE4F File Offset: 0x001CB04F
	private void InitializeConfigs()
	{
	}

	// Token: 0x06004F5D RID: 20317 RVA: 0x001CCE51 File Offset: 0x001CB051
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06004F5E RID: 20318 RVA: 0x001CCE60 File Offset: 0x001CB060
	public static Vector3 GetCursorPos(Vector3 mouse_pos)
	{
		RaycastHit raycastHit;
		Vector3 vector;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mouse_pos), out raycastHit, float.PositiveInfinity, Game.BlockSelectionLayerMask))
		{
			vector = raycastHit.point;
		}
		else
		{
			mouse_pos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
			vector = Camera.main.ScreenToWorldPoint(mouse_pos);
		}
		float num = vector.x;
		float num2 = vector.y;
		num = Mathf.Max(num, 0f);
		num = Mathf.Min(num, Grid.WidthInMeters);
		num2 = Mathf.Max(num2, 0f);
		num2 = Mathf.Min(num2, Grid.HeightInMeters);
		vector.x = num;
		vector.y = num2;
		return vector;
	}

	// Token: 0x06004F5F RID: 20319 RVA: 0x001CCF14 File Offset: 0x001CB114
	private void UpdateHover()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			this.activeTool.OnFocus(!current.IsPointerOverGameObject());
		}
	}

	// Token: 0x06004F60 RID: 20320 RVA: 0x001CCF44 File Offset: 0x001CB144
	private void Update()
	{
		this.UpdateDrag();
		if (this.activeTool && this.activeTool.enabled)
		{
			this.UpdateHover();
			Vector3 cursorPos = this.GetCursorPos();
			if (cursorPos != this.prevMousePos)
			{
				this.prevMousePos = cursorPos;
				this.activeTool.OnMouseMove(cursorPos);
			}
		}
		if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
		{
			this.DebugHidingCursor = !this.DebugHidingCursor;
			Cursor.visible = !this.DebugHidingCursor;
			HoverTextScreen.Instance.Show(!this.DebugHidingCursor);
		}
	}

	// Token: 0x06004F61 RID: 20321 RVA: 0x001CCFF3 File Offset: 0x001CB1F3
	private void OnCleanup()
	{
		Global.GetInputManager().usedMenus.Remove(this);
	}

	// Token: 0x06004F62 RID: 20322 RVA: 0x001CD006 File Offset: 0x001CB206
	private void LateUpdate()
	{
		if (this.queueStopDrag)
		{
			this.queueStopDrag = false;
			this.dragging = false;
			this.dragAction = global::Action.Invalid;
			this.dragDelta = Vector3.zero;
			this.worldDragDelta = Vector3.zero;
		}
	}

	// Token: 0x06004F63 RID: 20323 RVA: 0x001CD03C File Offset: 0x001CB23C
	public void ActivateTool(InterfaceTool tool)
	{
		if (this.activeTool == tool)
		{
			return;
		}
		this.DeactivateTool(tool);
		this.activeTool = tool;
		this.activeTool.enabled = true;
		this.activeTool.gameObject.SetActive(true);
		this.activeTool.ActivateTool();
		this.UpdateHover();
	}

	// Token: 0x06004F64 RID: 20324 RVA: 0x001CD094 File Offset: 0x001CB294
	public void ToolDeactivated(InterfaceTool tool)
	{
		if (this.activeTool == tool && this.activeTool != null)
		{
			this.DeactivateTool(null);
		}
		if (this.activeTool == null)
		{
			this.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x06004F65 RID: 20325 RVA: 0x001CD0D2 File Offset: 0x001CB2D2
	private void DeactivateTool(InterfaceTool new_tool = null)
	{
		if (this.activeTool != null)
		{
			this.activeTool.enabled = false;
			this.activeTool.gameObject.SetActive(false);
			InterfaceTool interfaceTool = this.activeTool;
			this.activeTool = null;
			interfaceTool.DeactivateTool(new_tool);
		}
	}

	// Token: 0x06004F66 RID: 20326 RVA: 0x001CD112 File Offset: 0x001CB312
	public bool IsUsingDefaultTool()
	{
		return this.tools.Length != 0 && this.activeTool == this.tools[0];
	}

	// Token: 0x06004F67 RID: 20327 RVA: 0x001CD132 File Offset: 0x001CB332
	private void StartDrag(global::Action action)
	{
		if (this.dragAction == global::Action.Invalid)
		{
			this.dragAction = action;
			this.startDragPos = KInputManager.GetMousePos();
			this.startDragTime = Time.unscaledTime;
		}
	}

	// Token: 0x06004F68 RID: 20328 RVA: 0x001CD15C File Offset: 0x001CB35C
	private void UpdateDrag()
	{
		this.dragDelta = Vector2.zero;
		Vector3 mousePos = KInputManager.GetMousePos();
		if (!this.dragging && this.CanDrag() && ((mousePos - this.startDragPos).sqrMagnitude > 36f || Time.unscaledTime - this.startDragTime > 0.3f))
		{
			this.dragging = true;
		}
		if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad && this.dragging)
		{
			return;
		}
		if (this.dragging)
		{
			this.dragDelta = mousePos - this.startDragPos;
			this.worldDragDelta = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(this.startDragPos);
			this.startDragPos = mousePos;
		}
	}

	// Token: 0x06004F69 RID: 20329 RVA: 0x001CD222 File Offset: 0x001CB422
	private void StopDrag(global::Action action)
	{
		if (this.dragAction == action)
		{
			this.queueStopDrag = true;
			if (KInputManager.currentControllerIsGamepad)
			{
				this.dragging = false;
			}
		}
	}

	// Token: 0x06004F6A RID: 20330 RVA: 0x001CD244 File Offset: 0x001CB444
	public void CancelDragging()
	{
		this.queueStopDrag = true;
		if (this.activeTool != null)
		{
			DragTool dragTool = this.activeTool as DragTool;
			if (dragTool != null)
			{
				dragTool.CancelDragging();
			}
		}
	}

	// Token: 0x06004F6B RID: 20331 RVA: 0x001CD281 File Offset: 0x001CB481
	public void OnCancelInput()
	{
		this.CancelDragging();
	}

	// Token: 0x06004F6C RID: 20332 RVA: 0x001CD28C File Offset: 0x001CB48C
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.ToggleScreenshotMode))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		if (DebugHandler.HideUI && e.TryConsume(global::Action.Escape))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StartDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StartDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StartDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
		pointerEventData.position = KInputManager.GetMousePos();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			current.RaycastAll(pointerEventData, list);
			if (list.Count > 0)
			{
				return;
			}
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
		{
			this.activeTool.OnLeftClickDown(this.GetCursorPos());
			return;
		}
		if (e.IsAction(global::Action.MouseRight))
		{
			this.activeTool.OnRightClickDown(this.GetCursorPos(), e);
			return;
		}
		this.activeTool.OnKeyDown(e);
	}

	// Token: 0x06004F6D RID: 20333 RVA: 0x001CD3C8 File Offset: 0x001CB5C8
	public void OnKeyUp(KButtonEvent e)
	{
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StopDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StopDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StopDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		if (!this.activeTool.hasFocus)
		{
			return;
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
		else
		{
			if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
	}

	// Token: 0x06004F6E RID: 20334 RVA: 0x001CD4F9 File Offset: 0x001CB6F9
	public bool ConsumeIfNotDragging(KButtonEvent e, global::Action action)
	{
		return (this.dragAction != action || !this.dragging) && e.TryConsume(action);
	}

	// Token: 0x06004F6F RID: 20335 RVA: 0x001CD515 File Offset: 0x001CB715
	public bool IsDragging()
	{
		return this.dragging && this.CanDrag();
	}

	// Token: 0x06004F70 RID: 20336 RVA: 0x001CD527 File Offset: 0x001CB727
	public bool CanDrag()
	{
		return this.draggingAllowed && this.dragAction > global::Action.Invalid;
	}

	// Token: 0x06004F71 RID: 20337 RVA: 0x001CD53C File Offset: 0x001CB73C
	public void AllowDragging(bool allow)
	{
		this.draggingAllowed = allow;
	}

	// Token: 0x06004F72 RID: 20338 RVA: 0x001CD545 File Offset: 0x001CB745
	public Vector3 GetDragDelta()
	{
		return this.dragDelta;
	}

	// Token: 0x06004F73 RID: 20339 RVA: 0x001CD54D File Offset: 0x001CB74D
	public Vector3 GetWorldDragDelta()
	{
		if (!this.draggingAllowed)
		{
			return Vector3.zero;
		}
		return this.worldDragDelta;
	}

	// Token: 0x04003509 RID: 13577
	[SerializeField]
	private global::Action defaultConfigKey;

	// Token: 0x0400350A RID: 13578
	[SerializeField]
	private List<InterfaceToolConfig> interfaceConfigs;

	// Token: 0x0400350C RID: 13580
	public InterfaceTool[] tools;

	// Token: 0x0400350D RID: 13581
	private InterfaceTool activeTool;

	// Token: 0x0400350E RID: 13582
	public VirtualInputModule vim;

	// Token: 0x04003510 RID: 13584
	private bool DebugHidingCursor;

	// Token: 0x04003511 RID: 13585
	private Vector3 prevMousePos = new Vector3(float.PositiveInfinity, 0f, 0f);

	// Token: 0x04003512 RID: 13586
	private const float MIN_DRAG_DIST_SQR = 36f;

	// Token: 0x04003513 RID: 13587
	private const float MIN_DRAG_TIME = 0.3f;

	// Token: 0x04003514 RID: 13588
	private global::Action dragAction;

	// Token: 0x04003515 RID: 13589
	private bool draggingAllowed = true;

	// Token: 0x04003516 RID: 13590
	private bool dragging;

	// Token: 0x04003517 RID: 13591
	private bool queueStopDrag;

	// Token: 0x04003518 RID: 13592
	private Vector3 startDragPos;

	// Token: 0x04003519 RID: 13593
	private float startDragTime;

	// Token: 0x0400351A RID: 13594
	private Vector3 dragDelta;

	// Token: 0x0400351B RID: 13595
	private Vector3 worldDragDelta;
}
