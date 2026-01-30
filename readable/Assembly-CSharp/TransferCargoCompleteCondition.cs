using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BD3 RID: 3027
public class TransferCargoCompleteCondition : ProcessCondition
{
	// Token: 0x06005AB3 RID: 23219 RVA: 0x0020DCAF File Offset: 0x0020BEAF
	public TransferCargoCompleteCondition(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x0020DCC0 File Offset: 0x0020BEC0
	public override ProcessCondition.Status EvaluateCondition()
	{
		LaunchPad component = this.target.GetComponent<LaunchPad>();
		CraftModuleInterface craftModuleInterface;
		if (component == null)
		{
			craftModuleInterface = this.target.GetComponent<Clustercraft>().ModuleInterface;
		}
		else
		{
			RocketModuleCluster landedRocket = component.LandedRocket;
			if (landedRocket == null)
			{
				return ProcessCondition.Status.Ready;
			}
			craftModuleInterface = landedRocket.CraftInterface;
		}
		if (!craftModuleInterface.HasCargoModule)
		{
			return ProcessCondition.Status.Ready;
		}
		if (!this.target.HasTag(GameTags.TransferringCargoComplete))
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005AB5 RID: 23221 RVA: 0x0020DD2F File Offset: 0x0020BF2F
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x0020DD4A File Offset: 0x0020BF4A
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x0020DD65 File Offset: 0x0020BF65
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C7E RID: 15486
	private GameObject target;
}
