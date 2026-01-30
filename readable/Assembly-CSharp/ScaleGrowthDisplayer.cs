using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000C8F RID: 3215
public class ScaleGrowthDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x060062AE RID: 25262 RVA: 0x002497CA File Offset: 0x002479CA
	public ScaleGrowthDisplayer(GameUtil.TimeSlice deltaTimeSlice) : base(deltaTimeSlice)
	{
	}

	// Token: 0x060062AF RID: 25263 RVA: 0x002497D4 File Offset: 0x002479D4
	public override string GetDescription(Amount master, AmountInstance instance)
	{
		Tag key = instance.gameObject.PrefabID();
		string arg = CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME().ContainsKey(key) ? CREATURES.STATS.SCALEGROWTH.GET_DISPLAYED_NAME()[key] : master.Name;
		return string.Format("{0}: {1}", arg, this.formatter.GetFormattedValue(base.ToPercent(instance.value, instance), GameUtil.TimeSlice.None));
	}

	// Token: 0x060062B0 RID: 25264 RVA: 0x00249838 File Offset: 0x00247A38
	public override string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		Tag key = instance.gameObject.PrefabID();
		string text = CREATURES.STATS.SCALEGROWTH.GET_TOOLTIP_PREFIX().ContainsKey(key) ? CREATURES.STATS.SCALEGROWTH.GET_TOOLTIP_PREFIX()[key] : "";
		return string.Format(GameUtil.SafeStringFormat(master.description, new object[]
		{
			text
		}), this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
	}
}
