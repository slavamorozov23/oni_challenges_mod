using System;
using STRINGS;

// Token: 0x02000BCA RID: 3018
public class ConditionProperlyFueled : ProcessCondition
{
	// Token: 0x06005A83 RID: 23171 RVA: 0x0020D0C8 File Offset: 0x0020B2C8
	public ConditionProperlyFueled(IFuelTank fuelTank)
	{
		this.fuelTank = fuelTank;
	}

	// Token: 0x06005A84 RID: 23172 RVA: 0x0020D0D8 File Offset: 0x0020B2D8
	public override ProcessCondition.Status EvaluateCondition()
	{
		RocketModuleCluster component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>();
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
				if (!component2.HasResourcesToMove(1, Clustercraft.CombustionResource.Fuel))
				{
					return ProcessCondition.Status.Failure;
				}
				return ProcessCondition.Status.Ready;
			}
			else
			{
				bool flag = component2.HasResourcesToMove(num * 2, Clustercraft.CombustionResource.Fuel);
				bool flag2 = component2.HasResourcesToMove(num, Clustercraft.CombustionResource.Fuel);
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

	// Token: 0x06005A85 RID: 23173 RVA: 0x0020D17C File Offset: 0x0020B37C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A86 RID: 23174 RVA: 0x0020D1BC File Offset: 0x0020B3BC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		Clustercraft component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				if (component.Destination == component.Location)
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY_NO_DESTINATION;
				}
				else
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY;
				}
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A87 RID: 23175 RVA: 0x0020D237 File Offset: 0x0020B437
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C6E RID: 15470
	private IFuelTank fuelTank;
}
