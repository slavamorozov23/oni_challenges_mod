using System;
using Klei.AI;

// Token: 0x02000C9D RID: 3229
public class RadsPerCycleAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060062E5 RID: 25317 RVA: 0x0024AE94 File Offset: 0x00249094
	public RadsPerCycleAttributeFormatter() : base(GameUtil.UnitClass.Radiation, GameUtil.TimeSlice.PerCycle)
	{
	}

	// Token: 0x060062E6 RID: 25318 RVA: 0x0024AE9E File Offset: 0x0024909E
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x060062E7 RID: 25319 RVA: 0x0024AEAD File Offset: 0x002490AD
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return base.GetFormattedValue(value / 600f, timeSlice);
	}
}
