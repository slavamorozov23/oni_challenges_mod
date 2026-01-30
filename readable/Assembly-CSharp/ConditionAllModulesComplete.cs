using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BBC RID: 3004
public class ConditionAllModulesComplete : ProcessCondition
{
	// Token: 0x06005A32 RID: 23090 RVA: 0x0020BA8F File Offset: 0x00209C8F
	public ConditionAllModulesComplete(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06005A33 RID: 23091 RVA: 0x0020BAA0 File Offset: 0x00209CA0
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Constructable>() != null)
				{
					return ProcessCondition.Status.Failure;
				}
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A34 RID: 23092 RVA: 0x0020BB10 File Offset: 0x00209D10
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A35 RID: 23093 RVA: 0x0020BB50 File Offset: 0x00209D50
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A36 RID: 23094 RVA: 0x0020BB90 File Offset: 0x00209D90
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C56 RID: 15446
	private ILaunchableRocket launchable;
}
