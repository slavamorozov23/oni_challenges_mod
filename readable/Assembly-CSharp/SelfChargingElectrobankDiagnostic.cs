using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
public class SelfChargingElectrobankDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F8A RID: 16266 RVA: 0x0016541C File Offset: 0x0016361C
	public SelfChargingElectrobankDiagnostic(int worldID) : base(worldID, UI.SELFCHARGINGBATTERYDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "overlay_radiation";
		base.AddCriterion("CheckLifetime", new DiagnosticCriterion(UI.SELFCHARGINGBATTERYDIAGNOSTIC.CRITERIA.CHECKSELFCHARGINGBATTERYLIFE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLifetime)));
	}

	// Token: 0x06003F8B RID: 16267 RVA: 0x00165476 File Offset: 0x00163676
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1.Concat(DlcManager.DLC3);
	}

	// Token: 0x06003F8C RID: 16268 RVA: 0x00165488 File Offset: 0x00163688
	private ColonyDiagnostic.DiagnosticResult CheckLifetime()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.SELFCHARGINGBATTERYDIAGNOSTIC.NORMAL, null);
		foreach (SelfChargingElectrobank selfChargingElectrobank in Components.SelfChargingElectrobanks.GetItems(base.worldID))
		{
			if (!selfChargingElectrobank.IsNullOrDestroyed() && selfChargingElectrobank.LifetimeRemaining <= this.WARNING_LIFETIME)
			{
				diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				if (diagnosticResult.clickThroughObjects == null)
				{
					diagnosticResult.clickThroughObjects = new List<GameObject>();
				}
				diagnosticResult.clickThroughObjects.Add(selfChargingElectrobank.gameObject);
				diagnosticResult.Message = UI.SELFCHARGINGBATTERYDIAGNOSTIC.CRITERIA_BATTERYLIFE_WARNING;
			}
		}
		return diagnosticResult;
	}

	// Token: 0x04002764 RID: 10084
	private float WARNING_LIFETIME = 600f;
}
