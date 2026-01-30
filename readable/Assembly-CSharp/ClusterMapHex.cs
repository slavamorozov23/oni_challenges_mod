using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CB4 RID: 3252
public class ClusterMapHex : MultiToggle, ICanvasRaycastFilter
{
	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06006399 RID: 25497 RVA: 0x00251514 File Offset: 0x0024F714
	// (set) Token: 0x0600639A RID: 25498 RVA: 0x0025151C File Offset: 0x0024F71C
	public AxialI location { get; private set; }

	// Token: 0x0600639B RID: 25499 RVA: 0x00251528 File Offset: 0x0024F728
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rectTransform = base.GetComponent<RectTransform>();
		this.onClick = new System.Action(this.TrySelect);
		this.onDoubleClick = new Func<bool>(this.TryGoTo);
		this.onEnter = new System.Action(this.OnHover);
		this.onExit = new System.Action(this.OnUnhover);
	}

	// Token: 0x0600639C RID: 25500 RVA: 0x0025158F File Offset: 0x0024F78F
	public void SetLocation(AxialI location)
	{
		this.location = location;
	}

	// Token: 0x0600639D RID: 25501 RVA: 0x00251598 File Offset: 0x0024F798
	public void SetRevealed(ClusterRevealLevel level)
	{
		this._revealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			this.fogOfWar.gameObject.SetActive(true);
			this.peekedTile.gameObject.SetActive(false);
			return;
		case ClusterRevealLevel.Peeked:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(true);
			return;
		case ClusterRevealLevel.Visible:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600639E RID: 25502 RVA: 0x00251627 File Offset: 0x0024F827
	public void SetDestinationStatus(string fail_reason)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x0600639F RID: 25503 RVA: 0x0025165C File Offset: 0x0024F85C
	public void SetDestinationStatus(string fail_reason, int pathLength, int rocketRange, bool repeat)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		if (pathLength > 0)
		{
			string text = repeat ? UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH_RETURN : UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH;
			if (repeat)
			{
				pathLength *= 2;
			}
			text = string.Format(text, pathLength, GameUtil.GetFormattedRocketRange(rocketRange, true));
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x060063A0 RID: 25504 RVA: 0x002516E4 File Offset: 0x0024F8E4
	public void UpdateToggleState(ClusterMapHex.ToggleState state)
	{
		int new_state_index = -1;
		switch (state)
		{
		case ClusterMapHex.ToggleState.Unselected:
			new_state_index = 0;
			break;
		case ClusterMapHex.ToggleState.Selected:
			new_state_index = 1;
			break;
		case ClusterMapHex.ToggleState.OrbitHighlight:
			new_state_index = 2;
			break;
		}
		base.ChangeState(new_state_index);
	}

	// Token: 0x060063A1 RID: 25505 RVA: 0x00251718 File Offset: 0x0024F918
	private void TrySelect()
	{
		if (DebugHandler.InstantBuildMode)
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.location, 0, 2);
		}
		ClusterMapScreen.Instance.SelectHex(this);
	}

	// Token: 0x060063A2 RID: 25506 RVA: 0x00251744 File Offset: 0x0024F944
	private bool TryGoTo()
	{
		List<WorldContainer> list = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(this.location)
		select entity.GetComponent<WorldContainer>() into x
		where x != null
		select x).ToList<WorldContainer>();
		if (list.Count == 1)
		{
			CameraController.Instance.ActiveWorldStarWipe(list[0].id, null);
			return true;
		}
		return false;
	}

	// Token: 0x060063A3 RID: 25507 RVA: 0x002517D4 File Offset: 0x0024F9D4
	private void OnHover()
	{
		this.m_tooltip.ClearMultiStringTooltip();
		string text = "";
		switch (this._revealLevel)
		{
		case ClusterRevealLevel.Hidden:
			text = UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX;
			break;
		case ClusterRevealLevel.Peeked:
		{
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.Asteroid);
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell2 = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.POI);
			text = ((hiddenEntitiesOfLayerAtCell.Count > 0 || hiddenEntitiesOfLayerAtCell2.Count > 0) ? UI.CLUSTERMAP.TOOLTIP_PEEKED_HEX_WITH_OBJECT : UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX);
			break;
		}
		case ClusterRevealLevel.Visible:
			if (ClusterGrid.Instance.GetEntitiesOnCell(this.location).Count == 0)
			{
				text = UI.CLUSTERMAP.TOOLTIP_EMPTY_HEX;
			}
			break;
		}
		if (!text.IsNullOrWhiteSpace())
		{
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(true);
		ClusterMapScreen.Instance.OnHoverHex(this);
	}

	// Token: 0x060063A4 RID: 25508 RVA: 0x002518B0 File Offset: 0x0024FAB0
	private void OnUnhover()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.OnUnhoverHex(this);
		}
	}

	// Token: 0x060063A5 RID: 25509 RVA: 0x002518CC File Offset: 0x0024FACC
	private void UpdateHoverColors(bool validDestination)
	{
		Color color_on_hover = validDestination ? this.hoverColorValid : this.hoverColorInvalid;
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].color_on_hover = color_on_hover;
			for (int j = 0; j < this.states[i].additional_display_settings.Length; j++)
			{
				this.states[i].additional_display_settings[j].color_on_hover = color_on_hover;
			}
		}
		base.RefreshHoverColor();
	}

	// Token: 0x060063A6 RID: 25510 RVA: 0x00251954 File Offset: 0x0024FB54
	public bool IsRaycastLocationValid(Vector2 inputPoint, Camera eventCamera)
	{
		Vector2 vector = this.rectTransform.position;
		float num = Mathf.Abs(inputPoint.x - vector.x);
		float num2 = Mathf.Abs(inputPoint.y - vector.y);
		Vector2 vector2 = this.rectTransform.lossyScale;
		return num <= vector2.x && num2 <= vector2.y && vector2.y * vector2.x - vector2.y / 2f * num - vector2.x * num2 >= 0f;
	}

	// Token: 0x040043B3 RID: 17331
	private RectTransform rectTransform;

	// Token: 0x040043B4 RID: 17332
	public Color hoverColorValid;

	// Token: 0x040043B5 RID: 17333
	public Color hoverColorInvalid;

	// Token: 0x040043B6 RID: 17334
	public Image fogOfWar;

	// Token: 0x040043B7 RID: 17335
	public Image peekedTile;

	// Token: 0x040043B8 RID: 17336
	public TextStyleSetting invalidDestinationTooltipStyle;

	// Token: 0x040043B9 RID: 17337
	public TextStyleSetting informationTooltipStyle;

	// Token: 0x040043BA RID: 17338
	[MyCmpGet]
	private ToolTip m_tooltip;

	// Token: 0x040043BB RID: 17339
	private ClusterRevealLevel _revealLevel;

	// Token: 0x02001EDD RID: 7901
	public enum ToggleState
	{
		// Token: 0x040090E4 RID: 37092
		Unselected,
		// Token: 0x040090E5 RID: 37093
		Selected,
		// Token: 0x040090E6 RID: 37094
		OrbitHighlight
	}
}
