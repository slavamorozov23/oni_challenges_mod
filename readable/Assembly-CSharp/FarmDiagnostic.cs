using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
public class FarmDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F5C RID: 16220 RVA: 0x00163BBC File Offset: 0x00161DBC
	public FarmDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_errand_farm";
		base.AddCriterion("CheckHasFarms", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKHASFARMS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasFarms)));
		base.AddCriterion("CheckPlanted", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKPLANTED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckPlanted)));
		base.AddCriterion("CheckWilting", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKWILTING, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckWilting)));
		base.AddCriterion("CheckOperational", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKOPERATIONAL, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckOperational)));
	}

	// Token: 0x06003F5D RID: 16221 RVA: 0x00163C7D File Offset: 0x00161E7D
	private void RefreshPlots()
	{
		this.plots = Components.PlantablePlots.GetItems(base.worldID).FindAll((PlantablePlot match) => match.HasDepositTag(GameTags.CropSeed));
	}

	// Token: 0x06003F5E RID: 16222 RVA: 0x00163CBC File Offset: 0x00161EBC
	private ColonyDiagnostic.DiagnosticResult CheckHasFarms()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.plots.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE;
		}
		return result;
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x00163D04 File Offset: 0x00161F04
	private ColonyDiagnostic.DiagnosticResult CheckPlanted()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		bool flag = false;
		using (List<PlantablePlot>.Enumerator enumerator = this.plots.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.plant != null)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE_PLANTED;
		}
		return result;
	}

	// Token: 0x06003F60 RID: 16224 RVA: 0x00163D94 File Offset: 0x00161F94
	private ColonyDiagnostic.DiagnosticResult CheckWilting()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (PlantablePlot plantablePlot in this.plots)
		{
			if (plantablePlot.plant != null && plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				StandardCropPlant component = plantablePlot.plant.GetComponent<StandardCropPlant>();
				if (component != null && component.smi.IsInsideState(component.smi.sm.alive.wilting) && component.smi.timeinstate > 15f)
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.WILTING;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x00163EA4 File Offset: 0x001620A4
	private ColonyDiagnostic.DiagnosticResult CheckOperational()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (PlantablePlot plantablePlot in this.plots)
		{
			if (plantablePlot.plant != null && !plantablePlot.HasTag(GameTags.Operational))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.INOPERATIONAL;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);
				break;
			}
		}
		return result;
	}

	// Token: 0x06003F62 RID: 16226 RVA: 0x00163F58 File Offset: 0x00162158
	public override string GetAverageValueString()
	{
		if (this.plots == null)
		{
			this.RefreshPlots();
		}
		return TrackerTool.Instance.GetWorldTracker<CropTracker>(base.worldID).GetCurrentValue().ToString() + "/" + this.plots.Count.ToString();
	}

	// Token: 0x06003F63 RID: 16227 RVA: 0x00163FB0 File Offset: 0x001621B0
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		this.RefreshPlots();
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NORMAL;
		}
		return diagnosticResult;
	}

	// Token: 0x0400275E RID: 10078
	private List<PlantablePlot> plots;
}
