using System;
using System.Collections.Generic;

// Token: 0x020008DD RID: 2269
public abstract class BionicColonyDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003F2F RID: 16175 RVA: 0x001630B8 File Offset: 0x001612B8
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06003F30 RID: 16176 RVA: 0x001630BF File Offset: 0x001612BF
	public BionicColonyDiagnostic(int worldID, string name) : base(worldID, name)
	{
		this.RefreshData();
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x001630D8 File Offset: 0x001612D8
	protected void RefreshData()
	{
		Components.Cmps<MinionIdentity> cmps;
		if (Components.LiveMinionIdentitiesByModel.TryGetValue(BionicMinionConfig.MODEL, out cmps))
		{
			this.bionics = cmps.GetWorldItems(base.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
			return;
		}
		this.bionics = new List<MinionIdentity>();
	}

	// Token: 0x06003F32 RID: 16178 RVA: 0x00163124 File Offset: 0x00161324
	protected virtual bool MinionFilter(MinionIdentity minion)
	{
		return true;
	}

	// Token: 0x06003F33 RID: 16179 RVA: 0x00163128 File Offset: 0x00161328
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (this.ignoreInIdleRockets && ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		this.RefreshData();
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = this.GetDefaultResultMessage();
		}
		return diagnosticResult;
	}

	// Token: 0x06003F34 RID: 16180
	protected abstract string GetDefaultResultMessage();

	// Token: 0x04002749 RID: 10057
	protected const bool INCLUDE_CHILD_WORLDS = true;

	// Token: 0x0400274A RID: 10058
	protected List<MinionIdentity> bionics;

	// Token: 0x0400274B RID: 10059
	protected bool ignoreInIdleRockets = true;
}
