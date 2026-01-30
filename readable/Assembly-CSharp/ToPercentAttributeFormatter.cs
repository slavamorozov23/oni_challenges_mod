using System;
using Klei.AI;

// Token: 0x02000CA1 RID: 3233
public class ToPercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060062F1 RID: 25329 RVA: 0x0024B025 File Offset: 0x00249225
	public ToPercentAttributeFormatter(float max, GameUtil.TimeSlice deltaTimeSlice = GameUtil.TimeSlice.None) : base(GameUtil.UnitClass.Percent, deltaTimeSlice)
	{
		this.max = max;
	}

	// Token: 0x060062F2 RID: 25330 RVA: 0x0024B041 File Offset: 0x00249241
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x060062F3 RID: 25331 RVA: 0x0024B055 File Offset: 0x00249255
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x060062F4 RID: 25332 RVA: 0x0024B069 File Offset: 0x00249269
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value / this.max * 100f, timeSlice);
	}

	// Token: 0x04004308 RID: 17160
	public float max = 1f;
}
