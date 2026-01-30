using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020008F2 RID: 2290
public class ToiletDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F8F RID: 16271 RVA: 0x001656BC File Offset: 0x001638BC
	public ToiletDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_region_toilet";
		this.tracker = TrackerTool.Instance.GetWorldTracker<WorkingToiletTracker>(worldID);
		this.NO_MINIONS_WITH_BLADDER = (base.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_PLANETOID);
		base.AddCriterion("CheckHasAnyToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKHASANYTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasAnyToilets)));
		base.AddCriterion("CheckEnoughToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKENOUGHTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughToilets)));
		base.AddCriterion("CheckBladders", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKBLADDERS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckBladders)));
	}

	// Token: 0x06003F90 RID: 16272 RVA: 0x00165788 File Offset: 0x00163988
	private ColonyDiagnostic.DiagnosticResult CheckHasAnyToilets()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else if (this.toilets.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_TOILETS;
		}
		return result;
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x001657F4 File Offset: 0x001639F4
	private ColonyDiagnostic.DiagnosticResult CheckEnoughToilets()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			if (this.tracker.GetDataTimeLength() > 10f && this.tracker.GetAverageValue(this.trackerSampleCountSeconds) <= 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_WORKING_TOILETS;
			}
		}
		return result;
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x00165898 File Offset: 0x00163A98
	private ColonyDiagnostic.DiagnosticResult CheckBladders()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			WorldContainer world = ClusterManager.Instance.GetWorld(base.worldID);
			foreach (PeeChoreMonitor.Instance instance in Components.CriticalBladders.Items)
			{
				int myWorldId = instance.master.gameObject.GetMyWorldId();
				if (myWorldId == base.worldID || world.GetChildWorldIds().Contains(myWorldId))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
					result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.TOILET_URGENT;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x00165990 File Offset: 0x00163B90
	private bool MinionFilter(MinionIdentity minion)
	{
		return minion.modifiers.amounts.Has(Db.Get().Amounts.Bladder);
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x001659B4 File Offset: 0x00163BB4
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS_WITH_BLADDER, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		this.RefreshData();
		return base.Evaluate();
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x001659ED File Offset: 0x00163BED
	private void RefreshData()
	{
		this.minionsWithBladders = Components.LiveMinionIdentities.GetWorldItems(base.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
		this.toilets = Components.Toilets.GetWorldItems(base.worldID, true);
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x00165A2C File Offset: 0x00163C2C
	public override string GetAverageValueString()
	{
		if (this.minionsWithBladders == null || this.minionsWithBladders.Count == 0)
		{
			this.RefreshData();
		}
		int num = this.toilets.Count;
		for (int i = 0; i < this.toilets.Count; i++)
		{
			if (!this.toilets[i].IsNullOrDestroyed() && !this.toilets[i].IsUsable())
			{
				num--;
			}
		}
		return num.ToString() + ":" + this.minionsWithBladders.Count.ToString();
	}

	// Token: 0x04002765 RID: 10085
	private const bool INCLUDE_CHILD_WORLDS = true;

	// Token: 0x04002766 RID: 10086
	private List<MinionIdentity> minionsWithBladders;

	// Token: 0x04002767 RID: 10087
	private List<IUsable> toilets;

	// Token: 0x04002768 RID: 10088
	private readonly string NO_MINIONS_WITH_BLADDER;
}
