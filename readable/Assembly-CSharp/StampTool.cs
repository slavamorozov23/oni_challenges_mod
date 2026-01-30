using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009C4 RID: 2500
public class StampTool : InterfaceTool
{
	// Token: 0x060048AD RID: 18605 RVA: 0x001A3704 File Offset: 0x001A1904
	public static void DestroyInstance()
	{
		StampTool.Instance = null;
	}

	// Token: 0x060048AE RID: 18606 RVA: 0x001A370C File Offset: 0x001A190C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		StampTool.Instance = this;
		this.preview = new StampToolPreview(this, new IStampToolPreviewPlugin[]
		{
			new StampToolPreview_Placers(this.PlacerPrefab),
			new StampToolPreview_Area(),
			new StampToolPreview_SolidLiquidGas(),
			new StampToolPreview_Prefabs()
		});
	}

	// Token: 0x060048AF RID: 18607 RVA: 0x001A375D File Offset: 0x001A195D
	private void Update()
	{
		this.preview.Refresh(Grid.PosToCell(this.GetCursorPos()));
	}

	// Token: 0x060048B0 RID: 18608 RVA: 0x001A3778 File Offset: 0x001A1978
	public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
	{
		this.selectAffected = SelectAffected;
		this.deactivateOnStamp = DeactivateOnStamp;
		if (this.stampTemplate == template || template == null || template.cells == null)
		{
			return;
		}
		this.stampTemplate = template;
		PlayerController.Instance.ActivateTool(this);
		base.StartCoroutine(this.preview.Setup(template));
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x001A37CD File Offset: 0x001A19CD
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x001A37D9 File Offset: 0x001A19D9
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.Stamp(cursor_pos);
	}

	// Token: 0x060048B3 RID: 18611 RVA: 0x001A37F0 File Offset: 0x001A19F0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.BuildMenuKeyQ))
		{
			Vector3 cursorPos = this.GetCursorPos();
			DebugBaseTemplateButton.Instance.ClearSelection();
			if (this.stampTemplate.cells != null)
			{
				for (int i = 0; i < this.stampTemplate.cells.Count; i++)
				{
					DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int)(cursorPos.x + (float)this.stampTemplate.cells[i].location_x), (int)(cursorPos.y + (float)this.stampTemplate.cells[i].location_y)));
				}
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x001A3898 File Offset: 0x001A1A98
	private void Stamp(Vector2 pos)
	{
		if (!this.ready)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(-this.stampTemplate.info.size.X / 2f), 0);
		int cell2 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(this.stampTemplate.info.size.X / 2f), 0);
		int cell3 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(-this.stampTemplate.info.size.Y / 2f));
		int cell4 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(this.stampTemplate.info.size.Y / 2f));
		if (!Grid.IsValidBuildingCell(cell) || !Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell3))
		{
			return;
		}
		this.ready = false;
		bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (this.stampTemplate.cells != null)
		{
			this.preview.OnPlace();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.stampTemplate.cells.Count; i++)
			{
				for (int j = 0; j < 34; j++)
				{
					GameObject gameObject = Grid.Objects[Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[i].location_x), (int)(pos.y + (float)this.stampTemplate.cells[i].location_y)), j];
					if (gameObject != null && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
			foreach (GameObject gameObject2 in list)
			{
				if (gameObject2 != null)
				{
					Util.KDestroyGameObject(gameObject2);
				}
			}
		}
		TemplateLoader.Stamp(this.stampTemplate, pos, delegate
		{
			this.CompleteStamp(pauseOnComplete);
		});
		if (this.selectAffected)
		{
			DebugBaseTemplateButton.Instance.ClearSelection();
			if (this.stampTemplate.cells != null)
			{
				for (int k = 0; k < this.stampTemplate.cells.Count; k++)
				{
					DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[k].location_x), (int)(pos.y + (float)this.stampTemplate.cells[k].location_y)));
				}
			}
		}
		if (this.deactivateOnStamp)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x060048B5 RID: 18613 RVA: 0x001A3BA0 File Offset: 0x001A1DA0
	private void CompleteStamp(bool pause)
	{
		if (pause)
		{
			SpeedControlScreen.Instance.Pause(true, false);
		}
		this.ready = true;
		this.OnDeactivateTool(null);
	}

	// Token: 0x060048B6 RID: 18614 RVA: 0x001A3BBF File Offset: 0x001A1DBF
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (base.gameObject.activeSelf)
		{
			return;
		}
		this.preview.Cleanup();
		this.stampTemplate = null;
	}

	// Token: 0x04003057 RID: 12375
	public static StampTool Instance;

	// Token: 0x04003058 RID: 12376
	private StampToolPreview preview;

	// Token: 0x04003059 RID: 12377
	public TemplateContainer stampTemplate;

	// Token: 0x0400305A RID: 12378
	public GameObject PlacerPrefab;

	// Token: 0x0400305B RID: 12379
	private bool ready = true;

	// Token: 0x0400305C RID: 12380
	private bool selectAffected;

	// Token: 0x0400305D RID: 12381
	private bool deactivateOnStamp;
}
