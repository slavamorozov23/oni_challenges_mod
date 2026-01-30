using System;
using System.Text;
using Klei.AI;
using STRINGS;

// Token: 0x02000C8E RID: 3214
public class StandardAmountDisplayer : IAmountDisplayer
{
	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x060062A4 RID: 25252 RVA: 0x0024958F File Offset: 0x0024778F
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x060062A5 RID: 25253 RVA: 0x00249597 File Offset: 0x00247797
	// (set) Token: 0x060062A6 RID: 25254 RVA: 0x002495A4 File Offset: 0x002477A4
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

	// Token: 0x060062A7 RID: 25255 RVA: 0x002495B2 File Offset: 0x002477B2
	public StandardAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice, StandardAttributeFormatter formatter = null, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
	{
		this.tense = tense;
		if (formatter != null)
		{
			this.formatter = formatter;
			return;
		}
		this.formatter = new StandardAttributeFormatter(unitClass, deltaTimeSlice);
	}

	// Token: 0x060062A8 RID: 25256 RVA: 0x002495DC File Offset: 0x002477DC
	public virtual string GetValueString(Amount master, AmountInstance instance)
	{
		if (!master.showMax)
		{
			return this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None);
		}
		return string.Format("{0} / {1}", this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), this.formatter.GetFormattedValue(instance.GetMax(), GameUtil.TimeSlice.None));
	}

	// Token: 0x060062A9 RID: 25257 RVA: 0x00249632 File Offset: 0x00247832
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.GetValueString(master, instance));
	}

	// Token: 0x060062AA RID: 25258 RVA: 0x0024964C File Offset: 0x0024784C
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		if (master.description.IndexOf("{1}") > -1)
		{
			stringBuilder.AppendFormat(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
		}
		else
		{
			stringBuilder.AppendFormat(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		}
		stringBuilder.Append("\n\n");
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			stringBuilder.AppendFormat(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerSecond)
		{
			stringBuilder.AppendFormat(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedModifier(attributeModifier));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060062AB RID: 25259 RVA: 0x0024979F File Offset: 0x0024799F
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x060062AC RID: 25260 RVA: 0x002497AD File Offset: 0x002479AD
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x060062AD RID: 25261 RVA: 0x002497BB File Offset: 0x002479BB
	public string GetFormattedValue(float value, GameUtil.TimeSlice time_slice)
	{
		return this.formatter.GetFormattedValue(value, time_slice);
	}

	// Token: 0x04004302 RID: 17154
	protected StandardAttributeFormatter formatter;

	// Token: 0x04004303 RID: 17155
	public GameUtil.IdentityDescriptorTense tense;
}
