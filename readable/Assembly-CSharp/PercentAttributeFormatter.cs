using System;
using Klei.AI;

// Token: 0x02000CA2 RID: 3234
public class PercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060062F5 RID: 25333 RVA: 0x0024B07F File Offset: 0x0024927F
	public PercentAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x060062F6 RID: 25334 RVA: 0x0024B089 File Offset: 0x00249289
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x060062F7 RID: 25335 RVA: 0x0024B09D File Offset: 0x0024929D
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x060062F8 RID: 25336 RVA: 0x0024B0B1 File Offset: 0x002492B1
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value * 100f, timeSlice);
	}
}
