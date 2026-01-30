using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x020008E9 RID: 2281
public class MeteorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F71 RID: 16241 RVA: 0x00164704 File Offset: 0x00162904
	public MeteorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "meteors";
		base.AddCriterion("BombardmentUnderway", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.CRITERIA.CHECKUNDERWAY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckMeteorBombardment)));
	}

	// Token: 0x06003F72 RID: 16242 RVA: 0x00164754 File Offset: 0x00162954
	public ColonyDiagnostic.DiagnosticResult CheckMeteorBombardment()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.NORMAL, null);
		List<GameplayEventInstance> list = new List<GameplayEventInstance>();
		GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(base.worldID, ref list);
		for (int i = 0; i < list.Count; i++)
		{
			MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
			if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
				result.Message = string.Format(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.SHOWER_UNDERWAY, GameUtil.GetFormattedTime(statesInstance.BombardTimeRemaining(), "F0"));
			}
		}
		return result;
	}
}
