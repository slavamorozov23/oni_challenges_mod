using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000D70 RID: 3440
public class CrewRationsEntry : CrewListEntry
{
	// Token: 0x06006AC9 RID: 27337 RVA: 0x00286B01 File Offset: 0x00284D01
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.rationMonitor = _identity.GetSMI<RationMonitor.Instance>();
		this.Refresh();
	}

	// Token: 0x06006ACA RID: 27338 RVA: 0x00286B1C File Offset: 0x00284D1C
	public override void Refresh()
	{
		base.Refresh();
		this.rationsEatenToday.text = GameUtil.GetFormattedCalories(this.rationMonitor.GetRationsAteToday(), GameUtil.TimeSlice.None, true);
		if (this.identity == null)
		{
			return;
		}
		foreach (AmountInstance amountInstance in this.identity.GetAmounts())
		{
			float min = amountInstance.GetMin();
			float max = amountInstance.GetMax();
			float num = max - min;
			string str = Mathf.RoundToInt((num - (max - amountInstance.value)) / num * 100f).ToString();
			if (amountInstance.amount == Db.Get().Amounts.Stress)
			{
				this.currentStressText.text = amountInstance.GetValueString();
				this.currentStressText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
				this.stressTrendImage.SetValue(amountInstance);
			}
			else if (amountInstance.amount == Db.Get().Amounts.Calories)
			{
				this.currentCaloriesText.text = str + "%";
				this.currentCaloriesText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
			else if (amountInstance.amount == Db.Get().Amounts.HitPoints)
			{
				this.currentHealthText.text = str + "%";
				this.currentHealthText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
		}
	}

	// Token: 0x04004978 RID: 18808
	public KButton incRationPerDayButton;

	// Token: 0x04004979 RID: 18809
	public KButton decRationPerDayButton;

	// Token: 0x0400497A RID: 18810
	public LocText rationPerDayText;

	// Token: 0x0400497B RID: 18811
	public LocText rationsEatenToday;

	// Token: 0x0400497C RID: 18812
	public LocText currentCaloriesText;

	// Token: 0x0400497D RID: 18813
	public LocText currentStressText;

	// Token: 0x0400497E RID: 18814
	public LocText currentHealthText;

	// Token: 0x0400497F RID: 18815
	public ValueTrendImageToggle stressTrendImage;

	// Token: 0x04004980 RID: 18816
	private RationMonitor.Instance rationMonitor;
}
