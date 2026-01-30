using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DA9 RID: 3497
public abstract class MeterScreen_VTD_DuplicantIterator : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x06006CD6 RID: 27862 RVA: 0x00292C18 File Offset: 0x00290E18
	protected virtual void UpdateDisplayInfo(BaseEventData base_ev_data, IList<MinionIdentity> minions)
	{
		PointerEventData pointerEventData = base_ev_data as PointerEventData;
		if (pointerEventData == null)
		{
			return;
		}
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		PointerEventData.InputButton button = pointerEventData.button;
		if (button != PointerEventData.InputButton.Left)
		{
			if (button != PointerEventData.InputButton.Right)
			{
				return;
			}
			this.lastSelectedDuplicantIndex = -1;
		}
		else
		{
			if (worldMinionIdentities.Count < this.lastSelectedDuplicantIndex)
			{
				this.lastSelectedDuplicantIndex = -1;
			}
			if (worldMinionIdentities.Count > 0)
			{
				this.lastSelectedDuplicantIndex = (this.lastSelectedDuplicantIndex + 1) % worldMinionIdentities.Count;
				MinionIdentity minionIdentity = minions[this.lastSelectedDuplicantIndex];
				SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), Vector3.zero);
				return;
			}
		}
	}

	// Token: 0x06006CD7 RID: 27863 RVA: 0x00292CB0 File Offset: 0x00290EB0
	public override void OnClick(BaseEventData base_ev_data)
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		this.UpdateDisplayInfo(base_ev_data, worldMinionIdentities);
		this.OnTooltip();
		this.Tooltip.forceRefresh = true;
	}

	// Token: 0x06006CD8 RID: 27864 RVA: 0x00292CDF File Offset: 0x00290EDF
	protected void AddToolTipLine(string str, bool selected)
	{
		if (selected)
		{
			this.Tooltip.AddMultiStringTooltip("<color=#F0B310FF>" + str + "</color>", this.ToolTipStyle_Property);
			return;
		}
		this.Tooltip.AddMultiStringTooltip(str, this.ToolTipStyle_Property);
	}

	// Token: 0x06006CD9 RID: 27865 RVA: 0x00292D18 File Offset: 0x00290F18
	protected void AddToolTipAmountPercentLine(AmountInstance amount, MinionIdentity id, bool selected)
	{
		string str = id.GetComponent<KSelectable>().GetName() + ":  " + Mathf.Round(amount.value).ToString() + "%";
		this.AddToolTipLine(str, selected);
	}

	// Token: 0x04004A6B RID: 19051
	protected int lastSelectedDuplicantIndex = -1;
}
