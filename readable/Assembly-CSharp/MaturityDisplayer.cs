using System;
using System.Text;
using Klei.AI;
using STRINGS;

// Token: 0x02000C98 RID: 3224
public class MaturityDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x060062CE RID: 25294 RVA: 0x0024A651 File Offset: 0x00248851
	public MaturityDisplayer() : base(GameUtil.TimeSlice.PerCycle)
	{
		this.formatter = new MaturityDisplayer.MaturityAttributeFormatter();
	}

	// Token: 0x060062CF RID: 25295 RVA: 0x0024A668 File Offset: 0x00248868
	public override string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.Append(base.GetTooltipDescription(master, instance));
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component.IsGrowing())
		{
			float seconds = (instance.GetMax() - instance.value) / instance.GetDelta();
			if (component != null && component.IsGrowing())
			{
				stringBuilder.AppendFormat(CREATURES.STATS.MATURITY.TOOLTIP_GROWING_CROP, GameUtil.GetFormattedCycles(seconds, "F1", false), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
			}
			else
			{
				stringBuilder.AppendFormat(CREATURES.STATS.MATURITY.TOOLTIP_GROWING, GameUtil.GetFormattedCycles(seconds, "F1", false));
			}
		}
		else if (component.ReachedNextHarvest())
		{
			stringBuilder.Append(CREATURES.STATS.MATURITY.TOOLTIP_GROWN);
		}
		else
		{
			stringBuilder.Append(CREATURES.STATS.MATURITY.TOOLTIP_STALLED);
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060062D0 RID: 25296 RVA: 0x0024A748 File Offset: 0x00248948
	public override string GetDescription(Amount master, AmountInstance instance)
	{
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component != null && component.IsGrowing())
		{
			return string.Format(CREATURES.STATS.MATURITY.AMOUNT_DESC_FMT, master.Name, this.formatter.GetFormattedValue(base.ToPercent(instance.value, instance), GameUtil.TimeSlice.None), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
		}
		return base.GetDescription(master, instance);
	}

	// Token: 0x02001ECA RID: 7882
	public class MaturityAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600B4AD RID: 46253 RVA: 0x003EC146 File Offset: 0x003EA346
		public MaturityAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
		{
		}

		// Token: 0x0600B4AE RID: 46254 RVA: 0x003EC150 File Offset: 0x003EA350
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			float num = modifier.Value;
			GameUtil.TimeSlice timeSlice = base.DeltaTimeSlice;
			if (modifier.IsMultiplier)
			{
				num *= 100f;
				timeSlice = GameUtil.TimeSlice.None;
			}
			return this.GetFormattedValue(num, timeSlice);
		}
	}
}
