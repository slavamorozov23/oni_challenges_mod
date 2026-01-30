using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020009A4 RID: 2468
public class ClusterMapSelectTool : InterfaceTool
{
	// Token: 0x06004726 RID: 18214 RVA: 0x0019BF80 File Offset: 0x0019A180
	public static void DestroyInstance()
	{
		ClusterMapSelectTool.Instance = null;
	}

	// Token: 0x06004727 RID: 18215 RVA: 0x0019BF88 File Offset: 0x0019A188
	protected override void OnPrefabInit()
	{
		ClusterMapSelectTool.Instance = this;
	}

	// Token: 0x06004728 RID: 18216 RVA: 0x0019BF90 File Offset: 0x0019A190
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06004729 RID: 18217 RVA: 0x0019BFB4 File Offset: 0x0019A1B4
	public KSelectable GetSelected()
	{
		return this.m_selected;
	}

	// Token: 0x0600472A RID: 18218 RVA: 0x0019BFBC File Offset: 0x0019A1BC
	public override bool ShowHoverUI()
	{
		return ClusterMapScreen.Instance.HasCurrentHover();
	}

	// Token: 0x0600472B RID: 18219 RVA: 0x0019BFC8 File Offset: 0x0019A1C8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x0600472C RID: 18220 RVA: 0x0019BFE0 File Offset: 0x0019A1E0
	private void UpdateHoveredSelectables()
	{
		this.m_hoveredSelectables.Clear();
		if (ClusterMapScreen.Instance.HasCurrentHover())
		{
			AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();
			List<KSelectable> collection = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(currentHoverLocation)
			select entity.GetComponent<KSelectable>() into selectable
			where selectable != null && selectable.IsSelectable
			select selectable).ToList<KSelectable>();
			this.m_hoveredSelectables.AddRange(collection);
		}
	}

	// Token: 0x0600472D RID: 18221 RVA: 0x0019C074 File Offset: 0x0019A274
	public override void LateUpdate()
	{
		this.UpdateHoveredSelectables();
		KSelectable kselectable = (this.m_hoveredSelectables.Count > 0) ? this.m_hoveredSelectables[0] : null;
		base.UpdateHoverElements(this.m_hoveredSelectables);
		if (!this.hasFocus)
		{
			base.ClearHover();
		}
		else if (kselectable != this.hover)
		{
			base.ClearHover();
			this.hover = kselectable;
			if (kselectable != null)
			{
				Game.Instance.Trigger(2095258329, kselectable.gameObject);
				kselectable.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x0019C117 File Offset: 0x0019A317
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x0019C145 File Offset: 0x0019A345
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x0019C160 File Offset: 0x0019A360
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.m_selected)
		{
			return;
		}
		if (this.m_selected != null)
		{
			this.m_selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == -1)
		{
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
		}
		this.m_selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x04002FC2 RID: 12226
	private List<KSelectable> m_hoveredSelectables = new List<KSelectable>();

	// Token: 0x04002FC3 RID: 12227
	private KSelectable m_selected;

	// Token: 0x04002FC4 RID: 12228
	public static ClusterMapSelectTool Instance;

	// Token: 0x04002FC5 RID: 12229
	private KSelectable delayedNextSelection;

	// Token: 0x04002FC6 RID: 12230
	private bool delayedSkipSound;
}
