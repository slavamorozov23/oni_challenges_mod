using System;
using STRINGS;

// Token: 0x02000BCF RID: 3023
public class InternalConstructionCompleteCondition : ProcessCondition
{
	// Token: 0x06005A9E RID: 23198 RVA: 0x0020D97B File Offset: 0x0020BB7B
	public InternalConstructionCompleteCondition(BuildingInternalConstructor.Instance target)
	{
		this.target = target;
	}

	// Token: 0x06005A9F RID: 23199 RVA: 0x0020D98A File Offset: 0x0020BB8A
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.target.IsRequestingConstruction() && !this.target.HasOutputInStorage())
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005AA0 RID: 23200 RVA: 0x0020D9A9 File Offset: 0x0020BBA9
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.FAILURE;
	}

	// Token: 0x06005AA1 RID: 23201 RVA: 0x0020D9C0 File Offset: 0x0020BBC0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
	}

	// Token: 0x06005AA2 RID: 23202 RVA: 0x0020D9D7 File Offset: 0x0020BBD7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C75 RID: 15477
	private BuildingInternalConstructor.Instance target;
}
