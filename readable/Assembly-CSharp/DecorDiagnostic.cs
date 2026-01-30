using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020008E1 RID: 2273
public class DecorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F4F RID: 16207 RVA: 0x00163948 File Offset: 0x00161B48
	public DecorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_category_decor";
		base.AddCriterion("CheckDecor", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.CRITERIA.CHECKDECOR, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckDecor)));
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x00163998 File Offset: 0x00161B98
	private ColonyDiagnostic.DiagnosticResult CheckDecor()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		return result;
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x001639E8 File Offset: 0x00161BE8
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}
}
