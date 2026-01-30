using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000BBF RID: 3007
public class ConditionHasAstronaut : ProcessCondition
{
	// Token: 0x06005A4B RID: 23115 RVA: 0x0020C3CB File Offset: 0x0020A5CB
	public ConditionHasAstronaut(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x06005A4C RID: 23116 RVA: 0x0020C3DC File Offset: 0x0020A5DC
	public override ProcessCondition.Status EvaluateCondition()
	{
		List<MinionStorage.Info> storedMinionInfo = this.module.GetComponent<MinionStorage>().GetStoredMinionInfo();
		if (storedMinionInfo.Count > 0 && storedMinionInfo[0].serializedMinion != null)
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005A4D RID: 23117 RVA: 0x0020C414 File Offset: 0x0020A614
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUT_TITLE;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x06005A4E RID: 23118 RVA: 0x0020C42F File Offset: 0x0020A62F
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HASASTRONAUT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x06005A4F RID: 23119 RVA: 0x0020C44A File Offset: 0x0020A64A
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C60 RID: 15456
	private CommandModule module;
}
