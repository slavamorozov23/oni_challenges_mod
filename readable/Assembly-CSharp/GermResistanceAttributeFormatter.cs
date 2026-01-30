using System;
using Klei.AI;

// Token: 0x02000CA0 RID: 3232
public class GermResistanceAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060062EF RID: 25327 RVA: 0x0024B00D File Offset: 0x0024920D
	public GermResistanceAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x060062F0 RID: 25328 RVA: 0x0024B017 File Offset: 0x00249217
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetGermResistanceModifierString(modifier.Value, false);
	}
}
