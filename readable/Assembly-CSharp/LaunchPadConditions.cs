using System;
using System.Collections.Generic;

// Token: 0x02000B8F RID: 2959
public class LaunchPadConditions : KMonoBehaviour, IProcessConditionSet
{
	// Token: 0x06005868 RID: 22632 RVA: 0x00201AA8 File Offset: 0x001FFCA8
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (conditionType != ProcessCondition.ProcessConditionType.RocketStorage)
		{
			return null;
		}
		return this.conditions;
	}

	// Token: 0x06005869 RID: 22633 RVA: 0x00201AB8 File Offset: 0x001FFCB8
	public int PopulateConditionSet(ProcessCondition.ProcessConditionType conditionType, List<ProcessCondition> conditions)
	{
		int num = 0;
		if (conditionType == ProcessCondition.ProcessConditionType.RocketStorage)
		{
			conditions.AddRange(this.conditions);
			num += this.conditions.Count;
		}
		return num;
	}

	// Token: 0x0600586A RID: 22634 RVA: 0x00201AE6 File Offset: 0x001FFCE6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.conditions = new List<ProcessCondition>();
		this.conditions.Add(new TransferCargoCompleteCondition(base.gameObject));
	}

	// Token: 0x04003B5B RID: 15195
	private List<ProcessCondition> conditions;
}
