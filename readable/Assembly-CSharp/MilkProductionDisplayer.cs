using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000C90 RID: 3216
public class MilkProductionDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x060062B1 RID: 25265 RVA: 0x002498A2 File Offset: 0x00247AA2
	public MilkProductionDisplayer(GameUtil.TimeSlice deltaTimeSlice) : base(deltaTimeSlice)
	{
	}

	// Token: 0x060062B2 RID: 25266 RVA: 0x002498AC File Offset: 0x00247AAC
	public override string GetDescription(Amount master, AmountInstance instance)
	{
		Element element = ElementLoader.FindElementByHash(instance.gameObject.GetSMI<MilkProductionMonitor.Instance>().def.element);
		return string.Format("{0}: {1}", GameUtil.SafeStringFormat(CREATURES.STATS.MILKPRODUCTION.DISPLAYED_NAME, new object[]
		{
			element.name
		}), this.formatter.GetFormattedValue(base.ToPercent(instance.value, instance), GameUtil.TimeSlice.None));
	}

	// Token: 0x060062B3 RID: 25267 RVA: 0x00249918 File Offset: 0x00247B18
	public override string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		Element element = ElementLoader.FindElementByHash(instance.gameObject.GetSMI<MilkProductionMonitor.Instance>().def.element);
		return string.Format(GameUtil.SafeStringFormat(master.description, new object[]
		{
			element.name
		}), this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
	}
}
