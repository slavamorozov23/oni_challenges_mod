using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000DA8 RID: 3496
public class MeterScreen_Stress : MeterScreen_VTD_DuplicantIterator
{
	// Token: 0x06006CD1 RID: 27857 RVA: 0x00292A4C File Offset: 0x00290C4C
	protected override void OnSpawn()
	{
		this.minionListCustomSortOperation = new Func<List<MinionIdentity>, List<MinionIdentity>>(this.SortByStressLevel);
		base.OnSpawn();
	}

	// Token: 0x06006CD2 RID: 27858 RVA: 0x00292A68 File Offset: 0x00290C68
	private List<MinionIdentity> SortByStressLevel(List<MinionIdentity> minions)
	{
		Amount stress_amount = Db.Get().Amounts.Stress;
		return (from x in minions
		orderby stress_amount.Lookup(x).value descending
		select x).ToList<MinionIdentity>();
	}

	// Token: 0x06006CD3 RID: 27859 RVA: 0x00292AA8 File Offset: 0x00290CA8
	protected override string OnTooltip()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_AVGSTRESS, Mathf.Round(maxStressInActiveWorld).ToString() + "%"), this.ToolTipStyle_Header);
		Amount stress = Db.Get().Amounts.Stress;
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		bool flag = this.lastSelectedDuplicantIndex >= 0 && this.lastSelectedDuplicantIndex < worldMinionIdentities.Count;
		for (int i = 0; i < worldMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = worldMinionIdentities[i];
			AmountInstance amount = stress.Lookup(minionIdentity);
			base.AddToolTipAmountPercentLine(amount, minionIdentity, flag && worldMinionIdentities[this.lastSelectedDuplicantIndex] == minionIdentity);
		}
		return "";
	}

	// Token: 0x06006CD4 RID: 27860 RVA: 0x00292B84 File Offset: 0x00290D84
	protected override void InternalRefresh()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.Label.text = Mathf.Round(maxStressInActiveWorld).ToString();
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<StressTracker>(ClusterManager.Instance.activeWorldId);
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor((worldTracker.GetCurrentValue() >= STRESS.ACTING_OUT_RESET) ? Constants.NEGATIVE_COLOR : Constants.NEUTRAL_COLOR);
		this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "stressData");
	}
}
