using System;
using System.Text;
using Klei.AI;
using STRINGS;

// Token: 0x02000C9F RID: 3231
public class QualityOfLifeAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060062EC RID: 25324 RVA: 0x0024AEF2 File Offset: 0x002490F2
	public QualityOfLifeAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x060062ED RID: 25325 RVA: 0x0024AEFC File Offset: 0x002490FC
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		return string.Format(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.DESC_FORMAT, this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None), this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x060062EE RID: 25326 RVA: 0x0024AF50 File Offset: 0x00249150
	public override string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.Append(base.GetTooltip(master, instance));
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		stringBuilder.Append("\n\n");
		stringBuilder.AppendFormat(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION, this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
		if (instance.GetTotalDisplayValue() - attributeInstance.GetTotalDisplayValue() >= 0f)
		{
			stringBuilder.Append("\n\n");
			stringBuilder.Append(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_OVER);
		}
		else
		{
			stringBuilder.Append("\n\n");
			stringBuilder.Append(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_UNDER);
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}
}
