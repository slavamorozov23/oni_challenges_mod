using System;
using System.Text;
using Klei.AI;
using STRINGS;

// Token: 0x02000C91 RID: 3217
public class AsPercentAmountDisplayer : IAmountDisplayer
{
	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x060062B4 RID: 25268 RVA: 0x00249971 File Offset: 0x00247B71
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x060062B5 RID: 25269 RVA: 0x00249979 File Offset: 0x00247B79
	// (set) Token: 0x060062B6 RID: 25270 RVA: 0x00249986 File Offset: 0x00247B86
	public GameUtil.TimeSlice DeltaTimeSlice
	{
		get
		{
			return this.formatter.DeltaTimeSlice;
		}
		set
		{
			this.formatter.DeltaTimeSlice = value;
		}
	}

	// Token: 0x060062B7 RID: 25271 RVA: 0x00249994 File Offset: 0x00247B94
	public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice)
	{
		this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);
	}

	// Token: 0x060062B8 RID: 25272 RVA: 0x002499A9 File Offset: 0x00247BA9
	public string GetValueString(Amount master, AmountInstance instance)
	{
		return this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None);
	}

	// Token: 0x060062B9 RID: 25273 RVA: 0x002499C4 File Offset: 0x00247BC4
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None));
	}

	// Token: 0x060062BA RID: 25274 RVA: 0x002499EF File Offset: 0x00247BEF
	public virtual string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		return string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
	}

	// Token: 0x060062BB RID: 25275 RVA: 0x00249A10 File Offset: 0x00247C10
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.Append(this.GetTooltipDescription(master, instance));
		stringBuilder.Append("\n\n");
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			stringBuilder.AppendFormat(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			stringBuilder.AppendFormat(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			float modifierContribution = instance.deltaAttribute.GetModifierContribution(attributeModifier);
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060062BC RID: 25276 RVA: 0x00249B2D File Offset: 0x00247D2D
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x060062BD RID: 25277 RVA: 0x00249B3B File Offset: 0x00247D3B
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		if (modifier.IsMultiplier)
		{
			return GameUtil.GetFormattedPercent(modifier.Value * 100f, GameUtil.TimeSlice.None);
		}
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x060062BE RID: 25278 RVA: 0x00249B64 File Offset: 0x00247D64
	public string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return this.formatter.GetFormattedValue(value, timeSlice);
	}

	// Token: 0x060062BF RID: 25279 RVA: 0x00249B73 File Offset: 0x00247D73
	protected float ToPercent(float value, AmountInstance instance)
	{
		return 100f * value / instance.GetMax();
	}

	// Token: 0x04004304 RID: 17156
	protected StandardAttributeFormatter formatter;
}
