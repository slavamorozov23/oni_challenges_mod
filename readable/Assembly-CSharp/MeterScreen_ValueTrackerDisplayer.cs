using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DAA RID: 3498
public abstract class MeterScreen_ValueTrackerDisplayer : KMonoBehaviour
{
	// Token: 0x06006CDB RID: 27867 RVA: 0x00292D6A File Offset: 0x00290F6A
	protected override void OnSpawn()
	{
		this.Tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		base.OnSpawn();
	}

	// Token: 0x06006CDC RID: 27868 RVA: 0x00292D8A File Offset: 0x00290F8A
	public void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.InternalRefresh();
	}

	// Token: 0x06006CDD RID: 27869
	protected abstract void InternalRefresh();

	// Token: 0x06006CDE RID: 27870
	protected abstract string OnTooltip();

	// Token: 0x06006CDF RID: 27871 RVA: 0x00292D98 File Offset: 0x00290F98
	public virtual void OnClick(BaseEventData base_ev_data)
	{
	}

	// Token: 0x06006CE0 RID: 27872 RVA: 0x00292D9C File Offset: 0x00290F9C
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x06006CE1 RID: 27873 RVA: 0x00292DED File Offset: 0x00290FED
	protected virtual List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		if (this.minionListCustomSortOperation != null)
		{
			this.worldLiveMinionIdentities = this.minionListCustomSortOperation(this.worldLiveMinionIdentities);
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x06006CE2 RID: 27874 RVA: 0x00292E24 File Offset: 0x00291024
	protected virtual List<MinionIdentity> GetAllMinionsFromAllWorlds()
	{
		List<MinionIdentity> list = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.Items
		where !x.IsNullOrDestroyed()
		select x);
		if (this.minionListCustomSortOperation != null)
		{
			this.worldLiveMinionIdentities = this.minionListCustomSortOperation(list);
		}
		return list;
	}

	// Token: 0x04004A6C RID: 19052
	public LocText Label;

	// Token: 0x04004A6D RID: 19053
	public ToolTip Tooltip;

	// Token: 0x04004A6E RID: 19054
	public GameObject diagnosticGraph;

	// Token: 0x04004A6F RID: 19055
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x04004A70 RID: 19056
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04004A71 RID: 19057
	protected Func<List<MinionIdentity>, List<MinionIdentity>> minionListCustomSortOperation;

	// Token: 0x04004A72 RID: 19058
	private List<MinionIdentity> worldLiveMinionIdentities;
}
