using System;
using STRINGS;

// Token: 0x02000BCE RID: 3022
public class ConditionSufficientOxidizer : ProcessCondition
{
	// Token: 0x06005A99 RID: 23193 RVA: 0x0020D846 File Offset: 0x0020BA46
	public ConditionSufficientOxidizer(OxidizerTank oxidizerTank)
	{
		this.oxidizerTank = oxidizerTank;
	}

	// Token: 0x06005A9A RID: 23194 RVA: 0x0020D858 File Offset: 0x0020BA58
	public override ProcessCondition.Status EvaluateCondition()
	{
		RocketModuleCluster component = this.oxidizerTank.GetComponent<RocketModuleCluster>();
		if (component != null && component.CraftInterface != null)
		{
			Clustercraft component2 = component.CraftInterface.GetComponent<Clustercraft>();
			ClusterTraveler component3 = component.CraftInterface.GetComponent<ClusterTraveler>();
			if (component2 == null || component3 == null || component3.CurrentPath == null)
			{
				return ProcessCondition.Status.Failure;
			}
			int num = component3.RemainingTravelNodes();
			if (num == 0)
			{
				if (!component2.HasResourcesToMove(1, Clustercraft.CombustionResource.Oxidizer))
				{
					return ProcessCondition.Status.Failure;
				}
				return ProcessCondition.Status.Ready;
			}
			else
			{
				bool flag = component2.HasResourcesToMove(num * 2, Clustercraft.CombustionResource.Oxidizer);
				bool flag2 = component2.HasResourcesToMove(num, Clustercraft.CombustionResource.Oxidizer);
				if (flag)
				{
					return ProcessCondition.Status.Ready;
				}
				if (flag2)
				{
					return ProcessCondition.Status.Warning;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005A9B RID: 23195 RVA: 0x0020D8F8 File Offset: 0x0020BAF8
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A9C RID: 23196 RVA: 0x0020D938 File Offset: 0x0020BB38
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A9D RID: 23197 RVA: 0x0020D978 File Offset: 0x0020BB78
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C74 RID: 15476
	private OxidizerTank oxidizerTank;
}
