using System;
using STRINGS;

// Token: 0x02000BC0 RID: 3008
public class ConditionHasAtmoSuit : ProcessCondition
{
	// Token: 0x06005A50 RID: 23120 RVA: 0x0020C450 File Offset: 0x0020A650
	public ConditionHasAtmoSuit(CommandModule module)
	{
		this.module = module;
		ManualDeliveryKG manualDeliveryKG = this.module.FindOrAdd<ManualDeliveryKG>();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.SetStorage(module.storage);
		manualDeliveryKG.RequestedItemTag = GameTags.AtmoSuit;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.refillMass = 0.1f;
		manualDeliveryKG.capacity = 1f;
	}

	// Token: 0x06005A51 RID: 23121 RVA: 0x0020C4C6 File Offset: 0x0020A6C6
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.AtmoSuit) < 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A52 RID: 23122 RVA: 0x0020C4EC File Offset: 0x0020A6EC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.NAME;
		}
		return UI.STARMAP.NOSUIT.NAME;
	}

	// Token: 0x06005A53 RID: 23123 RVA: 0x0020C507 File Offset: 0x0020A707
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.TOOLTIP;
		}
		return UI.STARMAP.NOSUIT.TOOLTIP;
	}

	// Token: 0x06005A54 RID: 23124 RVA: 0x0020C522 File Offset: 0x0020A722
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C61 RID: 15457
	private CommandModule module;
}
