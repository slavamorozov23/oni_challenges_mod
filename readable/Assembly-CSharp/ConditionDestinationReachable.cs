using System;
using STRINGS;

// Token: 0x02000BBD RID: 3005
public class ConditionDestinationReachable : ProcessCondition
{
	// Token: 0x06005A37 RID: 23095 RVA: 0x0020BB93 File Offset: 0x00209D93
	public ConditionDestinationReachable(RocketModule module)
	{
		this.module = module;
		this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x0020BBB4 File Offset: 0x00209DB4
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (!this.module.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<RocketClusterDestinationSelector>().IsAtDestination())
				{
					result = ProcessCondition.Status.Ready;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (spacecraftDestination != null && this.CanReachSpacecraftDestination(spacecraftDestination) && spacecraftDestination.GetDestinationType().visitable)
			{
				result = ProcessCondition.Status.Ready;
			}
		}
		return result;
	}

	// Token: 0x06005A39 RID: 23097 RVA: 0x0020BC38 File Offset: 0x00209E38
	public bool CanReachSpacecraftDestination(SpaceDestination destination)
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		float rocketMaxDistance = this.module.GetComponent<CommandModule>().rocketStats.GetRocketMaxDistance();
		return (float)destination.OneBasedDistance * 10000f <= rocketMaxDistance;
	}

	// Token: 0x06005A3A RID: 23098 RVA: 0x0020BC7C File Offset: 0x00209E7C
	public SpaceDestination GetSpacecraftDestination()
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
		return SpacecraftManager.instance.GetSpacecraftDestination(id);
	}

	// Token: 0x06005A3B RID: 23099 RVA: 0x0020BCBC File Offset: 0x00209EBC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x06005A3C RID: 23100 RVA: 0x0020BD28 File Offset: 0x00209F28
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (status == ProcessCondition.Status.Ready)
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
				}
				else
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
				}
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x06005A3D RID: 23101 RVA: 0x0020BDA3 File Offset: 0x00209FA3
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C57 RID: 15447
	private LaunchableRocketRegisterType craftRegisterType;

	// Token: 0x04003C58 RID: 15448
	private RocketModule module;
}
