using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008EB RID: 2283
public class RadiationDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F77 RID: 16247 RVA: 0x00164AFC File Offset: 0x00162CFC
	public RadiationDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<RadiationTracker>(worldID);
		this.trackerSampleCountSeconds = 150f;
		this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
		this.icon = "overlay_radiation";
		base.AddCriterion("CheckSick", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA.CHECKSICK, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckSick)));
		base.AddCriterion("CheckExposed", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA.CHECKEXPOSED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckExposure)));
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x00164B94 File Offset: 0x00162D94
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06003F79 RID: 16249 RVA: 0x00164B9B File Offset: 0x00162D9B
	public override string GetCurrentValueString()
	{
		return string.Format(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.AVERAGE_RADS, GameUtil.GetFormattedRads(TrackerTool.Instance.GetWorldTracker<RadiationTracker>(base.worldID).GetCurrentValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x00164BC7 File Offset: 0x00162DC7
	public override string GetAverageValueString()
	{
		return string.Format(UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.AVERAGE_RADS, GameUtil.GetFormattedRads(TrackerTool.Instance.GetWorldTracker<RadiationTracker>(base.worldID).GetCurrentValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06003F7B RID: 16251 RVA: 0x00164BF4 File Offset: 0x00162DF4
	private ColonyDiagnostic.DiagnosticResult CheckSick()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.NORMAL;
			foreach (MinionIdentity minionIdentity in worldItems)
			{
				RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
				if (smi != null && smi.sm.isSick.Get(smi))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_SICKNESS.FAIL;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.gameObject.transform.position, minionIdentity.gameObject);
				}
			}
		}
		return result;
	}

	// Token: 0x06003F7C RID: 16252 RVA: 0x00164CF8 File Offset: 0x00162EF8
	private ColonyDiagnostic.DiagnosticResult CheckExposure()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.PASS;
		foreach (MinionIdentity minionIdentity in worldItems)
		{
			RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
			if (smi != null)
			{
				RadiationMonitor sm = smi.sm;
				GameObject gameObject = minionIdentity.gameObject;
				Vector3 position = gameObject.transform.position;
				float p = sm.currentExposurePerCycle.Get(smi);
				float p2 = sm.radiationExposure.Get(smi);
				if (RadiationMonitor.COMPARE_LT_MINOR(smi, p) && RadiationMonitor.COMPARE_RECOVERY_IMMEDIATE(smi, p2))
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(position, gameObject);
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.FAIL_CONCERN;
				}
				if (RadiationMonitor.COMPARE_GTE_DEADLY(smi, p))
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(position, minionIdentity.gameObject);
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
					result.Message = UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.FAIL_WARNING;
				}
			}
		}
		return result;
	}
}
