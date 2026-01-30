using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000BC4 RID: 3012
public class ConditionHasNosecone : ProcessCondition
{
	// Token: 0x06005A65 RID: 23141 RVA: 0x0020CAF7 File Offset: 0x0020ACF7
	public ConditionHasNosecone(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06005A66 RID: 23142 RVA: 0x0020CB08 File Offset: 0x0020AD08
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().HasTag(GameTags.NoseRocketModule))
				{
					return ProcessCondition.Status.Ready;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005A67 RID: 23143 RVA: 0x0020CB6C File Offset: 0x0020AD6C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A68 RID: 23144 RVA: 0x0020CBAC File Offset: 0x0020ADAC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005A69 RID: 23145 RVA: 0x0020CBEC File Offset: 0x0020ADEC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C65 RID: 15461
	private LaunchableRocketCluster launchable;
}
