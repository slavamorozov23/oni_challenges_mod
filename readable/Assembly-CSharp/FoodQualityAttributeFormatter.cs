using System;
using Klei.AI;

// Token: 0x02000C9E RID: 3230
public class FoodQualityAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060062E8 RID: 25320 RVA: 0x0024AEBD File Offset: 0x002490BD
	public FoodQualityAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x060062E9 RID: 25321 RVA: 0x0024AEC7 File Offset: 0x002490C7
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x060062EA RID: 25322 RVA: 0x0024AED6 File Offset: 0x002490D6
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetFormattedInt(modifier.Value, GameUtil.TimeSlice.None);
	}

	// Token: 0x060062EB RID: 25323 RVA: 0x0024AEE4 File Offset: 0x002490E4
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return Util.StripTextFormatting(GameUtil.GetFormattedFoodQuality((int)value));
	}
}
