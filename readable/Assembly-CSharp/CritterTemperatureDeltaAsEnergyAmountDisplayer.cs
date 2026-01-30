using System;
using System.Text;
using Klei.AI;
using STRINGS;

// Token: 0x02000C9A RID: 3226
public class CritterTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
	// Token: 0x060062D3 RID: 25299 RVA: 0x0024A94B File Offset: 0x00248B4B
	public CritterTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice timeSlice) : base(unitClass, timeSlice, null, GameUtil.IdentityDescriptorTense.Normal)
	{
	}

	// Token: 0x060062D4 RID: 25300 RVA: 0x0024A958 File Offset: 0x00248B58
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		CritterTemperatureMonitor.Def def = instance.gameObject.GetDef<CritterTemperatureMonitor.Def>();
		PrimaryElement component = instance.gameObject.GetComponent<PrimaryElement>();
		stringBuilder.AppendFormat(master.description, new object[]
		{
			this.formatter.GetFormattedValue(def.temperatureColdUncomfortable, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureHotUncomfortable, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureColdDeadly, GameUtil.TimeSlice.None),
			this.formatter.GetFormattedValue(def.temperatureHotDeadly, GameUtil.TimeSlice.None)
		});
		float num = ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * component.Mass * 1000f;
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			stringBuilder.Append("\n\n");
			stringBuilder.AppendFormat(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (instance.deltaAttribute.Modifiers.Count > 0)
		{
			stringBuilder.Append("\n\n");
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
