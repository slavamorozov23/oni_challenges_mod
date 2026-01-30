using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009AB RID: 2475
public class DragTool : InterfaceTool
{
	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x0600476D RID: 18285 RVA: 0x0019D1E5 File Offset: 0x0019B3E5
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x0019D1ED File Offset: 0x0019B3ED
	protected virtual DragTool.Mode GetMode()
	{
		return this.mode;
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x0019D1F5 File Offset: 0x0019B3F5
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
		this.SetMode(this.mode);
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x0019D210 File Offset: 0x0019B410
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		if (KScreenManager.Instance != null)
		{
			KScreenManager.Instance.SetEventSystemEnabled(true);
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.RemoveCurrentAreaText();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x0019D248 File Offset: 0x0019B448
	protected override void OnPrefabInit()
	{
		Game.Instance.Subscribe(1634669191, new Action<object>(this.OnTutorialOpened));
		base.OnPrefabInit();
		if (this.visualizer != null)
		{
			this.visualizer = global::Util.KInstantiate(this.visualizer, null, null);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer = global::Util.KInstantiate(this.areaVisualizer, null, null);
			this.areaVisualizer.SetActive(false);
			this.areaVisualizerSpriteRenderer = this.areaVisualizer.GetComponent<SpriteRenderer>();
			this.areaVisualizer.transform.SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x0019D30C File Offset: 0x0019B50C
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x0019D315 File Offset: 0x0019B515
	protected override void OnCmpDisable()
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(false);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x0019D34C File Offset: 0x0019B54C
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.dragging = true;
		this.downPos = cursor_pos;
		this.cellChangedSinceDown = false;
		this.previousCursorPos = cursor_pos;
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			base.SetCurrentVirtualInputModuleMousMovementMode(true, delegate(VirtualInputModule module)
			{
				this.currentVirtualInputInUse = module;
			});
		}
		this.hasFocus = true;
		this.RemoveCurrentAreaText();
		if (this.areaVisualizerTextPrefab != null)
		{
			this.areaVisualizerText = NameDisplayScreen.Instance.AddAreaText("", this.areaVisualizerTextPrefab);
			NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>().color = this.areaColour;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.AddDragPoint(cursor_pos);
				return;
			}
		}
		else if (mode == DragTool.Mode.Box || mode == DragTool.Mode.Line)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(false);
			}
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(true);
				this.areaVisualizer.transform.SetPosition(cursor_pos);
				this.areaVisualizerSpriteRenderer.size = new Vector2(0.01f, 0.01f);
			}
		}
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x0019D4B9 File Offset: 0x0019B6B9
	public void RemoveCurrentAreaText()
	{
		if (this.areaVisualizerText != Guid.Empty)
		{
			NameDisplayScreen.Instance.RemoveWorldText(this.areaVisualizerText);
			this.areaVisualizerText = Guid.Empty;
		}
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x0019D4E8 File Offset: 0x0019B6E8
	public void CancelDragging()
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x0019D578 File Offset: 0x0019B778
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Line || Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)))
		{
			cursor_pos = this.SnapToLine(cursor_pos);
		}
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
			int num;
			int num2;
			Grid.PosToXY(this.downPos, out num, out num2);
			int num3 = num;
			int num4 = num2;
			int num5;
			int num6;
			Grid.PosToXY(cursor_pos, out num5, out num6);
			if (num5 < num)
			{
				global::Util.Swap<int>(ref num, ref num5);
			}
			if (num6 < num2)
			{
				global::Util.Swap<int>(ref num2, ref num6);
			}
			for (int i = num2; i <= num6; i++)
			{
				for (int j = num; j <= num5; j++)
				{
					int cell = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
					{
						int num7 = i - num4;
						int num8 = j - num3;
						num7 = Mathf.Abs(num7);
						num8 = Mathf.Abs(num8);
						this.OnDragTool(cell, num7 + num8);
					}
				}
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetConfirmSound(), false));
			this.OnDragComplete(this.downPos, cursor_pos);
		}
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x0019D707 File Offset: 0x0019B907
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x0019D70E File Offset: 0x0019B90E
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x0019D715 File Offset: 0x0019B915
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x0019D71C File Offset: 0x0019B91C
	protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
	{
		position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
		position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);
		return position;
	}

	// Token: 0x0600477C RID: 18300 RVA: 0x0019D77C File Offset: 0x0019B97C
	protected Vector3 SnapToLine(Vector3 cursorPos)
	{
		Vector3 vector = cursorPos - this.downPos;
		if (this.canChangeDragAxis || (!this.canChangeDragAxis && !this.cellChangedSinceDown) || this.dragAxis == DragTool.DragAxis.Invalid)
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
			if (Mathf.Abs(vector.x) < Mathf.Abs(vector.y))
			{
				this.dragAxis = DragTool.DragAxis.Vertical;
			}
			else
			{
				this.dragAxis = DragTool.DragAxis.Horizontal;
			}
		}
		DragTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis != DragTool.DragAxis.Horizontal)
		{
			if (dragAxis == DragTool.DragAxis.Vertical)
			{
				cursorPos.x = this.downPos.x;
				if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.y) > (float)(this.lineModeMaxLength - 1))
				{
					cursorPos.y = this.downPos.y + Mathf.Sign(vector.y) * (float)(this.lineModeMaxLength - 1);
				}
			}
		}
		else
		{
			cursorPos.y = this.downPos.y;
			if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.x) > (float)(this.lineModeMaxLength - 1))
			{
				cursorPos.x = this.downPos.x + Mathf.Sign(vector.x) * (float)(this.lineModeMaxLength - 1);
			}
		}
		return cursorPos;
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x0019D8B8 File Offset: 0x0019BAB8
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.dragging && (Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)) || this.GetMode() == DragTool.Mode.Line))
		{
			cursorPos = this.SnapToLine(cursorPos);
		}
		else
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
		}
		base.OnMouseMove(cursorPos);
		if (!this.dragging)
		{
			return;
		}
		if (Grid.PosToCell(cursorPos) != Grid.PosToCell(this.downPos))
		{
			this.cellChangedSinceDown = true;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode != DragTool.Mode.Brush)
		{
			if (mode - DragTool.Mode.Box <= 1)
			{
				Vector2 vector = Vector3.Max(this.downPos, cursorPos);
				Vector2 vector2 = Vector3.Min(this.downPos, cursorPos);
				vector = base.GetWorldRestrictedPosition(vector);
				vector2 = base.GetWorldRestrictedPosition(vector2);
				vector = base.GetRegularizedPos(vector, false);
				vector2 = base.GetRegularizedPos(vector2, true);
				Vector2 vector3 = vector - vector2;
				Vector2 vector4 = (vector + vector2) * 0.5f;
				this.areaVisualizer.transform.SetPosition(new Vector2(vector4.x, vector4.y));
				int num = (int)(vector.x - vector2.x + (vector.y - vector2.y) - 1f);
				if (this.areaVisualizerSpriteRenderer.size != vector3)
				{
					string sound = GlobalAssets.GetSound(this.GetDragSound(), false);
					if (sound != null)
					{
						Vector3 position = this.areaVisualizer.transform.GetPosition();
						position.z = 0f;
						EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
						instance.setParameterByName("tileCount", (float)num, false);
						SoundEvent.EndOneShot(instance);
					}
				}
				this.areaVisualizerSpriteRenderer.size = vector3;
				if (this.areaVisualizerText != Guid.Empty)
				{
					Vector2I vector2I = new Vector2I(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
					LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
					component.text = string.Format(UI.TOOLS.TOOL_AREA_FMT, vector2I.x, vector2I.y, vector2I.x * vector2I.y);
					Vector2 v = vector4;
					component.transform.SetPosition(v);
				}
			}
		}
		else
		{
			this.AddDragPoints(cursorPos, this.previousCursorPos);
			if (this.areaVisualizerText != Guid.Empty)
			{
				int dragLength = this.GetDragLength();
				LocText component2 = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
				component2.text = string.Format(UI.TOOLS.TOOL_LENGTH_FMT, dragLength);
				Vector3 vector5 = Grid.CellToPos(Grid.PosToCell(cursorPos));
				vector5 += new Vector3(0f, 1f, 0f);
				component2.transform.SetPosition(vector5);
			}
		}
		this.previousCursorPos = cursorPos;
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x0019DBB8 File Offset: 0x0019BDB8
	protected virtual void OnDragTool(int cell, int distFromOrigin)
	{
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x0019DBBA File Offset: 0x0019BDBA
	protected virtual void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
	{
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x0019DBBC File Offset: 0x0019BDBC
	protected virtual int GetDragLength()
	{
		return 0;
	}

	// Token: 0x06004781 RID: 18305 RVA: 0x0019DBC0 File Offset: 0x0019BDC0
	private void AddDragPoint(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			this.OnDragTool(cell, 0);
		}
	}

	// Token: 0x06004782 RID: 18306 RVA: 0x0019DC00 File Offset: 0x0019BE00
	private void AddDragPoints(Vector3 cursorPos, Vector3 previousCursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		Vector3 a = cursorPos - previousCursorPos;
		float magnitude = a.magnitude;
		float num = Grid.CellSizeInMeters * 0.25f;
		int num2 = 1 + (int)(magnitude / num);
		a.Normalize();
		for (int i = 0; i < num2; i++)
		{
			Vector3 cursorPos2 = previousCursorPos + a * ((float)i * num);
			this.AddDragPoint(cursorPos2);
		}
	}

	// Token: 0x06004783 RID: 18307 RVA: 0x0019DC75 File Offset: 0x0019BE75
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x0019DC95 File Offset: 0x0019BE95
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x0019DCB8 File Offset: 0x0019BEB8
	private void HandlePriortyKeysDown(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 > action || action > global::Action.Plan10 || !e.TryConsume(action))
		{
			return;
		}
		int num = action - global::Action.Plan1 + 1;
		if (num <= 9)
		{
			ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, num), true);
			return;
		}
		ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
	}

	// Token: 0x06004786 RID: 18310 RVA: 0x0019DD1C File Offset: 0x0019BF1C
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x0019DD44 File Offset: 0x0019BF44
	protected void SetMode(DragTool.Mode newMode)
	{
		this.mode = newMode;
		switch (this.mode)
		{
		case DragTool.Mode.Brush:
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(false);
			}
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			base.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Box:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Box;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Line:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Line;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		default:
			return;
		}
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x0019DE24 File Offset: 0x0019C024
	public override void OnFocus(bool focus)
	{
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(focus);
			}
			this.hasFocus = focus;
			return;
		}
		if (mode - DragTool.Mode.Box > 1)
		{
			return;
		}
		if (this.visualizer != null && !this.dragging)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = (focus || this.dragging);
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x0019DE98 File Offset: 0x0019C098
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x0019DEA1 File Offset: 0x0019C0A1
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x04002FD6 RID: 12246
	[SerializeField]
	private Texture2D boxCursor;

	// Token: 0x04002FD7 RID: 12247
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04002FD8 RID: 12248
	[SerializeField]
	private GameObject areaVisualizerTextPrefab;

	// Token: 0x04002FD9 RID: 12249
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04002FDA RID: 12250
	protected SpriteRenderer areaVisualizerSpriteRenderer;

	// Token: 0x04002FDB RID: 12251
	protected Guid areaVisualizerText;

	// Token: 0x04002FDC RID: 12252
	protected Vector3 placementPivot;

	// Token: 0x04002FDD RID: 12253
	protected bool interceptNumberKeysForPriority;

	// Token: 0x04002FDE RID: 12254
	private bool dragging;

	// Token: 0x04002FDF RID: 12255
	private Vector3 previousCursorPos;

	// Token: 0x04002FE0 RID: 12256
	private DragTool.Mode mode = DragTool.Mode.Box;

	// Token: 0x04002FE1 RID: 12257
	private DragTool.DragAxis dragAxis = DragTool.DragAxis.Invalid;

	// Token: 0x04002FE2 RID: 12258
	protected bool canChangeDragAxis = true;

	// Token: 0x04002FE3 RID: 12259
	protected int lineModeMaxLength = -1;

	// Token: 0x04002FE4 RID: 12260
	protected Vector3 downPos;

	// Token: 0x04002FE5 RID: 12261
	private bool cellChangedSinceDown;

	// Token: 0x04002FE6 RID: 12262
	private VirtualInputModule currentVirtualInputInUse;

	// Token: 0x02001A0D RID: 6669
	private enum DragAxis
	{
		// Token: 0x04008065 RID: 32869
		Invalid = -1,
		// Token: 0x04008066 RID: 32870
		None,
		// Token: 0x04008067 RID: 32871
		Horizontal,
		// Token: 0x04008068 RID: 32872
		Vertical
	}

	// Token: 0x02001A0E RID: 6670
	public enum Mode
	{
		// Token: 0x0400806A RID: 32874
		Brush,
		// Token: 0x0400806B RID: 32875
		Box,
		// Token: 0x0400806C RID: 32876
		Line
	}
}
