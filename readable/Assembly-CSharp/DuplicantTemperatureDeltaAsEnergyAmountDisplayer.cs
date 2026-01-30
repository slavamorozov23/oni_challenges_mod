using System;
using System.Text;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000C99 RID: 3225
public class DuplicantTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
	// Token: 0x060062D1 RID: 25297 RVA: 0x0024A7BA File Offset: 0x002489BA
	public DuplicantTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice timeSlice) : base(unitClass, timeSlice, null, GameUtil.IdentityDescriptorTense.Normal)
	{
	}

	// Token: 0x060062D2 RID: 25298 RVA: 0x0024A7C8 File Offset: 0x002489C8
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.AppendFormat(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), this.formatter.GetFormattedValue(DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL, GameUtil.TimeSlice.None));
		float num = ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * DUPLICANTSTATS.STANDARD.BaseStats.DEFAULT_MASS * 1000f;
		stringBuilder.Append("\n\n");
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			stringBuilder.AppendFormat(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			stringBuilder.AppendFormat(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(UI.CHANGEPERSECOND, GameUtil.GetFormattedJoules(instance.deltaAttribute.GetTotalDisplayValue() * num, "F1", GameUtil.TimeSlice.None));
		}
		for (int num2 = 0; num2 != instance.deltaAttribute.Modifiers.Count; num2++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num2];
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), GameUtil.GetFormattedHeatEnergyRate(attributeModifier.Value * num * 1f, GameUtil.HeatEnergyFormatterUnit.Automatic));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}
}
