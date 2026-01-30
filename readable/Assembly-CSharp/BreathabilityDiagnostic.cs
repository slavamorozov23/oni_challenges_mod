using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008DE RID: 2270
public class BreathabilityDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F35 RID: 16181 RVA: 0x00163174 File Offset: 0x00161374
	public BreathabilityDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BreathabilityTracker>(worldID);
		this.trackerSampleCountSeconds = 50f;
		this.icon = "overlay_oxygen";
		base.AddCriterion("CheckSuffocation", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKSUFFOCATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckSuffocation)));
		base.AddCriterion("CheckLowBreathability", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKLOWBREATHABILITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLowBreathability)));
		base.AddCriterion("CheckBionicOxygen", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKLOWBIONICOXYGEN, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLowBionicOxygen)));
	}

	// Token: 0x06003F36 RID: 16182 RVA: 0x0016322C File Offset: 0x0016142C
	private ColonyDiagnostic.DiagnosticResult CheckSuffocation()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		if (worldItems.Count != 0)
		{
			using (List<MinionIdentity>.Enumerator enumerator = worldItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity cmp = enumerator.Current;
					SuffocationMonitor.Instance smi = cmp.GetSMI<SuffocationMonitor.Instance>();
					if (smi != null && smi.IsInsideState(smi.sm.noOxygen.suffocating))
					{
						return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.SUFFOCATING, new global::Tuple<Vector3, GameObject>(smi.transform.position, smi.gameObject));
					}
				}
				goto IL_9B;
			}
			goto IL_8D;
			IL_9B:
			return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
		}
		IL_8D:
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
	}

	// Token: 0x06003F37 RID: 16183 RVA: 0x001632F8 File Offset: 0x001614F8
	private ColonyDiagnostic.DiagnosticResult CheckLowBreathability()
	{
		if (Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count != 0 && this.tracker.GetAverageValue(this.trackerSampleCountSeconds) < 60f)
		{
			return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.POOR, null);
		}
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x00163358 File Offset: 0x00161558
	private ColonyDiagnostic.DiagnosticResult CheckLowBionicOxygen()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		if (worldItems.Count != 0)
		{
			foreach (MinionIdentity minionIdentity in worldItems)
			{
				if (minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
				{
					BionicOxygenTankMonitor.Instance smi = minionIdentity.GetSMI<BionicOxygenTankMonitor.Instance>();
					if (smi.OxygenPercentage <= 0f)
					{
						return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NEAR_OR_EMPTY_BIONIC_TANKS, new global::Tuple<Vector3, GameObject>(minionIdentity.transform.position, minionIdentity.gameObject));
					}
					if (smi.OxygenPercentage < 0.5f)
					{
						return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.POOR_BIONIC_TANKS, new global::Tuple<Vector3, GameObject>(minionIdentity.transform.position, minionIdentity.gameObject));
					}
				}
			}
		}
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x00163458 File Offset: 0x00161658
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}
}
