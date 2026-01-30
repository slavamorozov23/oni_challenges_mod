using System;
using STRINGS;
using UnityEngine;

// Token: 0x020008DC RID: 2268
public class BionicBatteryDiagnostic : BionicColonyDiagnostic
{
	// Token: 0x06003F2A RID: 16170 RVA: 0x00162D44 File Offset: 0x00160F44
	public BionicBatteryDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<ElectrobankJoulesTracker>(worldID);
		this.icon = "BionicPower";
		this.trackerSampleCountSeconds = 150f;
		this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
		base.AddCriterion("CheckEnoughBatteries", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA.CHECKENOUGHBATTERIES, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughBatteries)));
		base.AddCriterion("CheckPowerLevel", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA.CHECKPOWERLEVEL, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckPowerLevel)));
		BionicBatteryMonitor.WattageModifier difficultyModifier = BionicBatteryMonitor.GetDifficultyModifier();
		this.multiplier = (difficultyModifier.value + 200f) / 200f;
		this.recommendedJoulesPerBionic = 480000f * this.multiplier;
		this.bionicJoulesPerCycle = 120000f * this.multiplier;
	}

	// Token: 0x06003F2B RID: 16171 RVA: 0x00162E2C File Offset: 0x0016102C
	private ColonyDiagnostic.DiagnosticResult CheckEnoughBatteries()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.tracker.GetDataTimeLength() < 10f)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
		}
		else if (this.bionics.Count != 0)
		{
			if (this.tracker.GetAverageValue(this.trackerSampleCountSeconds) == 0f)
			{
				result.Message = UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_BATTERIES.NO_POWERBANKS;
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
			}
			else if ((float)this.bionics.Count * this.recommendedJoulesPerBionic > this.tracker.GetAverageValue(this.trackerSampleCountSeconds))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				float currentValue = this.tracker.GetCurrentValue();
				float f = this.bionicJoulesPerCycle * (float)this.bionics.Count;
				string formattedJoules = GameUtil.GetFormattedJoules(currentValue, "F1", GameUtil.TimeSlice.None);
				string formattedJoules2 = GameUtil.GetFormattedJoules(Mathf.Abs(f), "F1", GameUtil.TimeSlice.None);
				string text = UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_BATTERIES.LOW_POWERBANKS;
				text = text.Replace("{0}", formattedJoules);
				text = text.Replace("{1}", formattedJoules2);
				result.Message = text;
			}
		}
		return result;
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x00162F64 File Offset: 0x00161164
	private ColonyDiagnostic.DiagnosticResult CheckPowerLevel()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (MinionIdentity minionIdentity in this.bionics)
		{
			if (!minionIdentity.isNull)
			{
				BionicBatteryMonitor.Instance smi = minionIdentity.GetSMI<BionicBatteryMonitor.Instance>();
				if (!smi.IsNullOrStopped())
				{
					if (smi.IsInsideState(smi.sm.online.critical) && diagnosticResult.opinion != ColonyDiagnostic.DiagnosticResult.Opinion.Bad)
					{
						diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
						diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_POWERLEVEL.CRITICAL_MODE;
						diagnosticResult.clickThroughTarget = new global::Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
					}
					if (smi.IsInsideState(smi.sm.offline))
					{
						diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
						diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_POWERLEVEL.POWERLESS;
						diagnosticResult.clickThroughTarget = new global::Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
					}
				}
			}
		}
		return diagnosticResult;
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x00163094 File Offset: 0x00161294
	public override string GetCurrentValueString()
	{
		return GameUtil.GetFormattedJoules(this.tracker.GetCurrentValue(), "F1", GameUtil.TimeSlice.None);
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x001630AC File Offset: 0x001612AC
	protected override string GetDefaultResultMessage()
	{
		return UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_BATTERIES.PASS;
	}

	// Token: 0x04002746 RID: 10054
	private float bionicJoulesPerCycle;

	// Token: 0x04002747 RID: 10055
	private float recommendedJoulesPerBionic;

	// Token: 0x04002748 RID: 10056
	private float multiplier = 1f;
}
