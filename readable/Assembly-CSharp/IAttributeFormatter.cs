using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000C9B RID: 3227
public interface IAttributeFormatter
{
	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x060062D5 RID: 25301
	// (set) Token: 0x060062D6 RID: 25302
	GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x060062D7 RID: 25303
	string GetFormattedAttribute(AttributeInstance instance);

	// Token: 0x060062D8 RID: 25304
	string GetFormattedModifier(AttributeModifier modifier);

	// Token: 0x060062D9 RID: 25305
	string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice);

	// Token: 0x060062DA RID: 25306
	string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance);

	// Token: 0x060062DB RID: 25307
	string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters);
}
