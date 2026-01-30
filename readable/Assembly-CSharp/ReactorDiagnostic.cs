using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008EC RID: 2284
public class ReactorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F7D RID: 16253 RVA: 0x00164E74 File Offset: 0x00163074
	public ReactorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "overlay_radiation";
		base.AddCriterion("CheckTemperature", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKTEMPERATURE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckTemperature)));
		base.AddCriterion("CheckCoolant", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKCOOLANT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckCoolant)));
	}

	// Token: 0x06003F7E RID: 16254 RVA: 0x00164EE9 File Offset: 0x001630E9
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06003F7F RID: 16255 RVA: 0x00164EF0 File Offset: 0x001630F0
	private ColonyDiagnostic.DiagnosticResult CheckTemperature()
	{
		List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
		foreach (Reactor reactor in worldItems)
		{
			if (reactor.FuelTemperature > 1254.8625f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
				result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_TEMPERATURE_WARNING;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
			}
		}
		return result;
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x00164FBC File Offset: 0x001631BC
	private ColonyDiagnostic.DiagnosticResult CheckCoolant()
	{
		List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
		foreach (Reactor reactor in worldItems)
		{
			if (reactor.On && reactor.ReserveCoolantMass <= 45f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_COOLANT_WARNING;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
			}
		}
		return result;
	}
}
