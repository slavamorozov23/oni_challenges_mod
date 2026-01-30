using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008DB RID: 2267
public class BedDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F23 RID: 16163 RVA: 0x001628B4 File Offset: 0x00160AB4
	public BedDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_region_bedroom";
		base.AddCriterion("CheckEnoughBeds", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CRITERIA.CHECKENOUGHBEDS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughBeds)));
		base.AddCriterion("CheckReachability", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CRITERIA.CHECKREACHABILITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckReachability)));
		this.NO_MINIONS_WITH_STAMINA = (base.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NO_MINIONS_PLANETOID);
	}

	// Token: 0x06003F24 RID: 16164 RVA: 0x00162948 File Offset: 0x00160B48
	private ColonyDiagnostic.DiagnosticResult CheckEnoughBeds()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NORMAL;
		if (this.minionsWithStamina.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_STAMINA;
		}
		else if (Components.NormalBeds.GlobalCount < this.minionsWithStamina.Count)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NOT_ENOUGH_BEDS;
		}
		return result;
	}

	// Token: 0x06003F25 RID: 16165 RVA: 0x001629D8 File Offset: 0x00160BD8
	private ColonyDiagnostic.DiagnosticResult CheckReachability()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NORMAL;
		if (this.minionsWithStamina.Count == 0)
		{
			diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			diagnosticResult.Message = this.NO_MINIONS_WITH_STAMINA;
		}
		else
		{
			ListPool<Sleepable, BedDiagnostic>.PooledList pooledList = ListPool<Sleepable, BedDiagnostic>.Allocate();
			foreach (Sleepable sleepable in Components.NormalBeds.WorldItemsEnumerate(base.worldID, true))
			{
				if (sleepable.assignable != null && !sleepable.assignable.IsAssigned())
				{
					pooledList.Add(sleepable);
				}
			}
			foreach (MinionIdentity minionIdentity in this.minionsWithStamina)
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				AssignableSlotInstance slot = minionIdentity.assignableProxy.Get().GetComponent<Ownables>().GetSlot(Db.Get().AssignableSlots.Bed);
				if (!slot.IsAssigned() && diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
				{
					Sleepable sleepable2 = null;
					foreach (Sleepable sleepable3 in pooledList)
					{
						if (component.CanReach(sleepable3.approachable) && sleepable3.assignable.CanAutoAssignTo(minionIdentity))
						{
							sleepable2 = sleepable3;
							break;
						}
					}
					if (sleepable2 != null)
					{
						pooledList.Remove(sleepable2);
					}
					else
					{
						diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
						diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.MISSING_ASSIGNMENT;
						diagnosticResult.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.gameObject.transform.position, minionIdentity.gameObject);
					}
				}
				else if (slot.IsAssigned() && !component.CanReach(Grid.PosToCell(slot.assignable)))
				{
					diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CANT_REACH;
					diagnosticResult.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.gameObject.transform.position, minionIdentity.gameObject);
					break;
				}
			}
			pooledList.Recycle();
		}
		return diagnosticResult;
	}

	// Token: 0x06003F26 RID: 16166 RVA: 0x00162C70 File Offset: 0x00160E70
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS_WITH_STAMINA, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		this.RefreshData();
		return base.Evaluate();
	}

	// Token: 0x06003F27 RID: 16167 RVA: 0x00162CA9 File Offset: 0x00160EA9
	private void RefreshData()
	{
		this.minionsWithStamina = Components.LiveMinionIdentities.GetWorldItems(base.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x00162CCE File Offset: 0x00160ECE
	private bool MinionFilter(MinionIdentity minion)
	{
		return minion.modifiers.amounts.Has(Db.Get().Amounts.Stamina);
	}

	// Token: 0x06003F29 RID: 16169 RVA: 0x00162CF0 File Offset: 0x00160EF0
	public override string GetAverageValueString()
	{
		if (this.minionsWithStamina == null)
		{
			this.RefreshData();
		}
		return Components.NormalBeds.CountWorldItems(base.worldID, true).ToString() + "/" + this.minionsWithStamina.Count.ToString();
	}

	// Token: 0x04002743 RID: 10051
	private List<MinionIdentity> minionsWithStamina;

	// Token: 0x04002744 RID: 10052
	private const bool INCLUDE_CHILD_WORLDS = true;

	// Token: 0x04002745 RID: 10053
	private readonly string NO_MINIONS_WITH_STAMINA;
}
