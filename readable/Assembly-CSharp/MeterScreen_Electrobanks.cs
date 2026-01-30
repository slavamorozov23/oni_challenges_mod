using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02000DA5 RID: 3493
public class MeterScreen_Electrobanks : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x06006CC3 RID: 27843 RVA: 0x00292200 File Offset: 0x00290400
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.LiveMinionIdentities.OnAdd += this.OnNewMinionAdded;
		List<MinionIdentity> allMinionsFromAllWorlds = this.GetAllMinionsFromAllWorlds();
		bool visibility;
		if (allMinionsFromAllWorlds != null)
		{
			visibility = (allMinionsFromAllWorlds.Find((MinionIdentity m) => m.model == BionicMinionConfig.MODEL) != null);
		}
		else
		{
			visibility = false;
		}
		this.SetVisibility(visibility);
		BionicBatteryMonitor.WattageModifier difficultyModifier = BionicBatteryMonitor.GetDifficultyModifier();
		this.bionicJoulesPerCycle = (difficultyModifier.value + 200f) * 600f;
	}

	// Token: 0x06006CC4 RID: 27844 RVA: 0x00292285 File Offset: 0x00290485
	protected override void OnCleanUp()
	{
		Components.LiveMinionIdentities.OnAdd -= this.OnNewMinionAdded;
		base.OnCleanUp();
	}

	// Token: 0x06006CC5 RID: 27845 RVA: 0x002922A3 File Offset: 0x002904A3
	private void OnNewMinionAdded(MinionIdentity id)
	{
		if (id.model == BionicMinionConfig.MODEL)
		{
			this.SetVisibility(true);
		}
	}

	// Token: 0x06006CC6 RID: 27846 RVA: 0x002922BE File Offset: 0x002904BE
	public void SetVisibility(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x06006CC7 RID: 27847 RVA: 0x002922CC File Offset: 0x002904CC
	protected override string OnTooltip()
	{
		this.per_electrobankType_UnitCount_Dictionary.Clear();
		float num = 0f;
		string formattedJoules = GameUtil.GetFormattedJoules(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(this.per_electrobankType_UnitCount_Dictionary, out num, ClusterManager.Instance.activeWorld.worldInventory, true), "F1", GameUtil.TimeSlice.None);
		this.Label.text = formattedJoules;
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_ELECTROBANK_JOULES, formattedJoules, GameUtil.GetFormattedJoules(this.bionicJoulesPerCycle, "F1", GameUtil.TimeSlice.None), GameUtil.GetFormattedUnits((float)((int)num), GameUtil.TimeSlice.None, true, "")), this.ToolTipStyle_Header);
		this.Tooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
		foreach (KeyValuePair<string, float> keyValuePair in (from x in this.per_electrobankType_UnitCount_Dictionary
		orderby x.Value descending
		select x).ToDictionary((KeyValuePair<string, float> t) => t.Key, (KeyValuePair<string, float> t) => t.Value))
		{
			GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
			this.Tooltip.AddMultiStringTooltip((prefab != null) ? string.Format("{0} ({1}): {2}", prefab.GetProperName(), GameUtil.GetFormattedUnits((float)((int)keyValuePair.Value), GameUtil.TimeSlice.None, true, ""), GameUtil.GetFormattedJoules(keyValuePair.Value * 120000f, "F1", GameUtil.TimeSlice.None)) : string.Format(UI.TOOLTIPS.METERSCREEN_INVALID_ELECTROBANK_TYPE, keyValuePair.Key), this.ToolTipStyle_Property);
		}
		return "";
	}

	// Token: 0x06006CC8 RID: 27848 RVA: 0x002924BC File Offset: 0x002906BC
	protected override void InternalRefresh()
	{
		if (!Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			return;
		}
		if (this.Label != null && WorldResourceAmountTracker<ElectrobankTracker>.Get() != null)
		{
			float num2;
			long num = (long)WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, out num2, ClusterManager.Instance.activeWorld.worldInventory, true);
			if (this.cachedJoules != num)
			{
				this.Label.text = GameUtil.GetFormattedJoules((float)num, "F1", GameUtil.TimeSlice.None);
				this.cachedJoules = num;
			}
		}
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor(((float)this.cachedJoules > (float)this.GetWorldMinionIdentities().Count * 120000f) ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<ElectrobankJoulesTracker>(ClusterManager.Instance.activeWorldId);
		if (worldTracker != null)
		{
			this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "joules");
		}
	}

	// Token: 0x04004A66 RID: 19046
	private long cachedJoules = -1L;

	// Token: 0x04004A67 RID: 19047
	private Dictionary<string, float> per_electrobankType_UnitCount_Dictionary = new Dictionary<string, float>();

	// Token: 0x04004A68 RID: 19048
	private float bionicJoulesPerCycle;
}
