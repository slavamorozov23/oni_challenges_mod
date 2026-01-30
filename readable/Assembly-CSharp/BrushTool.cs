using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200099F RID: 2463
public class BrushTool : InterfaceTool
{
	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x060046D6 RID: 18134 RVA: 0x0019A621 File Offset: 0x00198821
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x060046D7 RID: 18135 RVA: 0x0019A629 File Offset: 0x00198829
	protected virtual void PlaySound()
	{
	}

	// Token: 0x060046D8 RID: 18136 RVA: 0x0019A62B File Offset: 0x0019882B
	protected virtual void clearVisitedCells()
	{
		this.visitedCells.Clear();
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x0019A638 File Offset: 0x00198838
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x0019A648 File Offset: 0x00198848
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x0019A6B0 File Offset: 0x001988B0
	public virtual void SetBrushSize(int radius)
	{
		if (radius == this.brushRadius)
		{
			return;
		}
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x0019A751 File Offset: 0x00198951
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x0019A774 File Offset: 0x00198974
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
			this.areaVisualizer.GetComponent<RectTransform>().SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x0019A827 File Offset: 0x00198A27
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x0019A830 File Offset: 0x00198A30
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

	// Token: 0x060046E0 RID: 18144 RVA: 0x0019A866 File Offset: 0x00198A66
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		this.dragging = true;
		this.downPos = cursor_pos;
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(true, null);
		}
		this.Paint();
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x0019A8A8 File Offset: 0x00198AA8
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		BrushTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis == BrushTool.DragAxis.Horizontal)
		{
			cursor_pos.y = this.downPos.y;
			this.dragAxis = BrushTool.DragAxis.None;
			return;
		}
		if (dragAxis != BrushTool.DragAxis.Vertical)
		{
			return;
		}
		cursor_pos.x = this.downPos.x;
		this.dragAxis = BrushTool.DragAxis.None;
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x0019A930 File Offset: 0x00198B30
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x0019A937 File Offset: 0x00198B37
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x0019A93E File Offset: 0x00198B3E
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x0019A948 File Offset: 0x00198B48
	private static int GetGridDistance(int cell, int center_cell)
	{
		Vector2I u = Grid.CellToXY(cell);
		Vector2I v = Grid.CellToXY(center_cell);
		Vector2I vector2I = u - v;
		return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x0019A980 File Offset: 0x00198B80
	private void Paint()
	{
		int count = this.visitedCells.Count;
		foreach (int num in this.cellsInRadius)
		{
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId && (!Grid.Foundation[num] || this.affectFoundation))
			{
				this.OnPaintCell(num, Grid.GetCellDistance(this.currentCell, num));
			}
		}
		if (this.lastCell != this.currentCell)
		{
			this.PlayDragSound();
		}
		if (count < this.visitedCells.Count)
		{
			this.PlaySound();
		}
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x0019AA44 File Offset: 0x00198C44
	protected virtual void PlayDragSound()
	{
		string dragSound = this.GetDragSound();
		if (!string.IsNullOrEmpty(dragSound))
		{
			string sound = GlobalAssets.GetSound(dragSound, false);
			if (sound != null)
			{
				Vector3 pos = Grid.CellToPos(this.currentCell);
				pos.z = 0f;
				int cellDistance = Grid.GetCellDistance(Grid.PosToCell(this.downPos), this.currentCell);
				EventInstance instance = SoundEvent.BeginOneShot(sound, pos, 1f, false);
				instance.setParameterByName("tileCount", (float)cellDistance, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x0019AAC4 File Offset: 0x00198CC4
	public override void OnMouseMove(Vector3 cursorPos)
	{
		int num = Grid.PosToCell(cursorPos);
		this.currentCell = num;
		base.OnMouseMove(cursorPos);
		this.cellsInRadius.Clear();
		foreach (Vector2 vector in this.brushOffsets)
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y));
			if (Grid.IsValidCell(num2) && (int)Grid.WorldIdx[num2] == ClusterManager.Instance.activeWorldId)
			{
				this.cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y)));
			}
		}
		if (!this.dragging)
		{
			return;
		}
		this.Paint();
		this.lastCell = this.currentCell;
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x0019ABB4 File Offset: 0x00198DB4
	protected virtual void OnPaintCell(int cell, int distFromOrigin)
	{
		if (!this.visitedCells.Contains(cell))
		{
			this.visitedCells.Add(cell);
		}
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x0019ABD0 File Offset: 0x00198DD0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.None;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x0019AC03 File Offset: 0x00198E03
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.Invalid;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x0019AC38 File Offset: 0x00198E38
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

	// Token: 0x060046ED RID: 18157 RVA: 0x0019AC9C File Offset: 0x00198E9C
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x0019ACC2 File Offset: 0x00198EC2
	public override void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
		base.OnFocus(focus);
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x0019ACEC File Offset: 0x00198EEC
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x0019ACF5 File Offset: 0x00198EF5
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x0019AD07 File Offset: 0x00198F07
	public override void LateUpdate()
	{
		base.LateUpdate();
	}

	// Token: 0x04002FA4 RID: 12196
	[SerializeField]
	private Texture2D brushCursor;

	// Token: 0x04002FA5 RID: 12197
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04002FA6 RID: 12198
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04002FA7 RID: 12199
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04002FA8 RID: 12200
	protected Vector3 placementPivot;

	// Token: 0x04002FA9 RID: 12201
	protected bool interceptNumberKeysForPriority;

	// Token: 0x04002FAA RID: 12202
	protected List<Vector2> brushOffsets = new List<Vector2>();

	// Token: 0x04002FAB RID: 12203
	protected bool affectFoundation;

	// Token: 0x04002FAC RID: 12204
	private bool dragging;

	// Token: 0x04002FAD RID: 12205
	protected int brushRadius = -1;

	// Token: 0x04002FAE RID: 12206
	private BrushTool.DragAxis dragAxis = BrushTool.DragAxis.Invalid;

	// Token: 0x04002FAF RID: 12207
	protected Vector3 downPos;

	// Token: 0x04002FB0 RID: 12208
	protected int currentCell;

	// Token: 0x04002FB1 RID: 12209
	protected int lastCell;

	// Token: 0x04002FB2 RID: 12210
	protected List<int> visitedCells = new List<int>();

	// Token: 0x04002FB3 RID: 12211
	protected HashSet<int> cellsInRadius = new HashSet<int>();

	// Token: 0x02001A06 RID: 6662
	private enum DragAxis
	{
		// Token: 0x04008042 RID: 32834
		Invalid = -1,
		// Token: 0x04008043 RID: 32835
		None,
		// Token: 0x04008044 RID: 32836
		Horizontal,
		// Token: 0x04008045 RID: 32837
		Vertical
	}
}
