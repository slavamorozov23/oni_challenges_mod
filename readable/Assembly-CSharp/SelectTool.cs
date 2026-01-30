using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020009C3 RID: 2499
public class SelectTool : InterfaceTool
{
	// Token: 0x0600489D RID: 18589 RVA: 0x001A3323 File Offset: 0x001A1523
	public static void DestroyInstance()
	{
		SelectTool.Instance = null;
	}

	// Token: 0x0600489E RID: 18590 RVA: 0x001A332C File Offset: 0x001A152C
	protected override void OnPrefabInit()
	{
		this.defaultLayerMask = (1 | LayerMask.GetMask(new string[]
		{
			"World",
			"Pickupable",
			"Place",
			"PlaceWithDepth",
			"BlockSelection",
			"Construction",
			"Selection"
		}));
		this.layerMask = this.defaultLayerMask;
		this.selectMarker = global::Util.KInstantiateUI<SelectMarker>(EntityPrefabs.Instance.SelectMarker, GameScreenManager.Instance.worldSpaceCanvas, false);
		this.selectMarker.gameObject.SetActive(false);
		this.populateHitsList = true;
		SelectTool.Instance = this;
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x001A33CE File Offset: 0x001A15CE
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x060048A0 RID: 18592 RVA: 0x001A33F2 File Offset: 0x001A15F2
	public void SetLayerMask(int mask)
	{
		this.layerMask = mask;
		base.ClearHover();
		this.LateUpdate();
	}

	// Token: 0x060048A1 RID: 18593 RVA: 0x001A3407 File Offset: 0x001A1607
	public void ClearLayerMask()
	{
		this.layerMask = this.defaultLayerMask;
	}

	// Token: 0x060048A2 RID: 18594 RVA: 0x001A3415 File Offset: 0x001A1615
	public int GetDefaultLayerMask()
	{
		return this.defaultLayerMask;
	}

	// Token: 0x060048A3 RID: 18595 RVA: 0x001A341D File Offset: 0x001A161D
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x060048A4 RID: 18596 RVA: 0x001A3434 File Offset: 0x001A1634
	public void Focus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		if (selectable != null)
		{
			pos = selectable.transform.GetPosition();
		}
		pos.z = -40f;
		pos += offset;
		WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(pos);
		if (worldFromPosition != null)
		{
			GameUtil.FocusCameraOnWorld(worldFromPosition.id, pos, 10f, null, true);
			return;
		}
		DebugUtil.DevLogError("DevError: specified camera focus position has null world - possible out of bounds location");
	}

	// Token: 0x060048A5 RID: 18597 RVA: 0x001A349F File Offset: 0x001A169F
	public void SelectAndFocus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		this.Focus(pos, selectable, offset);
		this.Select(selectable, false);
	}

	// Token: 0x060048A6 RID: 18598 RVA: 0x001A34B2 File Offset: 0x001A16B2
	public void SelectAndFocus(Vector3 pos, KSelectable selectable)
	{
		this.SelectAndFocus(pos, selectable, Vector3.zero);
	}

	// Token: 0x060048A7 RID: 18599 RVA: 0x001A34C1 File Offset: 0x001A16C1
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x001A34EF File Offset: 0x001A16EF
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x060048A9 RID: 18601 RVA: 0x001A350C File Offset: 0x001A170C
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.previousSelection)
		{
			return;
		}
		this.previousSelection = new_selected;
		if (this.selected != null)
		{
			this.selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
		{
			SelectToolHoverTextCard component = base.GetComponent<SelectToolHoverTextCard>();
			if (component != null)
			{
				int num = component.currentSelectedSelectableIndex;
				int recentNumberOfDisplayedSelectables = component.recentNumberOfDisplayedSelectables;
				if (recentNumberOfDisplayedSelectables != 0)
				{
					num = (num + 1) % recentNumberOfDisplayedSelectables;
					if (!skipSound)
					{
						if (recentNumberOfDisplayedSelectables == 1)
						{
							KFMOD.PlayUISound(GlobalAssets.GetSound("Select_empty", false));
						}
						else
						{
							EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Select_full", false), Vector3.zero, 1f);
							instance.setParameterByName("selection", (float)num, false);
							SoundEvent.EndOneShot(instance);
						}
						this.playedSoundThisFrame = true;
					}
				}
			}
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
			this.selectMarker.SetTargetTransform(gameObject.transform);
			this.selectMarker.gameObject.SetActive(!new_selected.DisableSelectMarker);
		}
		else if (this.selectMarker != null)
		{
			this.selectMarker.gameObject.SetActive(false);
		}
		this.selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x060048AA RID: 18602 RVA: 0x001A3678 File Offset: 0x001A1878
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		KSelectable objectUnderCursor = base.GetObjectUnderCursor<KSelectable>(true, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, this.selected);
		this.selectedCell = Grid.PosToCell(cursor_pos);
		this.Select(objectUnderCursor, false);
		if (DevToolSimDebug.Instance != null)
		{
			DevToolSimDebug.Instance.SetCell(this.selectedCell);
		}
		if (DevToolNavGrid.Instance != null)
		{
			DevToolNavGrid.Instance.SetCell(this.selectedCell);
		}
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x001A36F4 File Offset: 0x001A18F4
	public int GetSelectedCell()
	{
		return this.selectedCell;
	}

	// Token: 0x0400304F RID: 12367
	public KSelectable selected;

	// Token: 0x04003050 RID: 12368
	protected int cell_new;

	// Token: 0x04003051 RID: 12369
	private int selectedCell;

	// Token: 0x04003052 RID: 12370
	protected int defaultLayerMask;

	// Token: 0x04003053 RID: 12371
	public static SelectTool Instance;

	// Token: 0x04003054 RID: 12372
	private KSelectable delayedNextSelection;

	// Token: 0x04003055 RID: 12373
	private bool delayedSkipSound;

	// Token: 0x04003056 RID: 12374
	private KSelectable previousSelection;
}
