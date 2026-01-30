using System;
using STRINGS;

// Token: 0x02000BC5 RID: 3013
public class ConditionHasResource : ProcessCondition
{
	// Token: 0x06005A6A RID: 23146 RVA: 0x0020CBEF File Offset: 0x0020ADEF
	public ConditionHasResource(Storage storage, SimHashes resource, float thresholdMass)
	{
		this.storage = storage;
		this.resource = resource;
		this.thresholdMass = thresholdMass;
	}

	// Token: 0x06005A6B RID: 23147 RVA: 0x0020CC0C File Offset: 0x0020AE0C
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.storage.GetAmountAvailable(this.resource.CreateTag()) < this.thresholdMass)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A6C RID: 23148 RVA: 0x0020CC30 File Offset: 0x0020AE30
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.WARNING, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.FAILURE, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x06005A6D RID: 23149 RVA: 0x0020CCE0 File Offset: 0x0020AEE0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.WARNING, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.FAILURE, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x06005A6E RID: 23150 RVA: 0x0020CDB8 File Offset: 0x0020AFB8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C66 RID: 15462
	private Storage storage;

	// Token: 0x04003C67 RID: 15463
	private SimHashes resource;

	// Token: 0x04003C68 RID: 15464
	private float thresholdMass;
}
