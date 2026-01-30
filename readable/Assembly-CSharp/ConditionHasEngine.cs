using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BC2 RID: 3010
public class ConditionHasEngine : ProcessCondition
{
	// Token: 0x06005A5A RID: 23130 RVA: 0x0020C5EC File Offset: 0x0020A7EC
	public ConditionHasEngine(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06005A5B RID: 23131 RVA: 0x0020C5FC File Offset: 0x0020A7FC
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()))
		{
			if (gameObject.GetComponent<RocketEngine>() != null || gameObject.GetComponent<RocketEngineCluster>())
			{
				return ProcessCondition.Status.Ready;
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005A5C RID: 23132 RVA: 0x0020C67C File Offset: 0x0020A87C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A5D RID: 23133 RVA: 0x0020C6BC File Offset: 0x0020A8BC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A5E RID: 23134 RVA: 0x0020C6FC File Offset: 0x0020A8FC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C63 RID: 15459
	private ILaunchableRocket launchable;
}
