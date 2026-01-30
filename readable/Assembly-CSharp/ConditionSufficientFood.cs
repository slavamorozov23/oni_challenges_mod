using System;
using STRINGS;

// Token: 0x02000BCD RID: 3021
public class ConditionSufficientFood : ProcessCondition
{
	// Token: 0x06005A94 RID: 23188 RVA: 0x0020D7DB File Offset: 0x0020B9DB
	public ConditionSufficientFood(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x06005A95 RID: 23189 RVA: 0x0020D7EA File Offset: 0x0020B9EA
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.Edible) <= 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005A96 RID: 23190 RVA: 0x0020D80D File Offset: 0x0020BA0D
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.NAME;
		}
		return UI.STARMAP.NOFOOD.NAME;
	}

	// Token: 0x06005A97 RID: 23191 RVA: 0x0020D828 File Offset: 0x0020BA28
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.TOOLTIP;
		}
		return UI.STARMAP.NOFOOD.TOOLTIP;
	}

	// Token: 0x06005A98 RID: 23192 RVA: 0x0020D843 File Offset: 0x0020BA43
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C73 RID: 15475
	private CommandModule module;
}
