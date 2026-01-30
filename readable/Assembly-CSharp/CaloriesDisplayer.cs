using System;
using Klei.AI;

// Token: 0x02000C95 RID: 3221
public class CaloriesDisplayer : StandardAmountDisplayer
{
	// Token: 0x060062C8 RID: 25288 RVA: 0x0024A424 File Offset: 0x00248624
	public CaloriesDisplayer() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new CaloriesDisplayer.CaloriesAttributeFormatter();
	}

	// Token: 0x02001EC7 RID: 7879
	public class CaloriesAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600B4A9 RID: 46249 RVA: 0x003EC103 File Offset: 0x003EA303
		public CaloriesAttributeFormatter() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
		{
		}

		// Token: 0x0600B4AA RID: 46250 RVA: 0x003EC10D File Offset: 0x003EA30D
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			if (modifier.IsMultiplier)
			{
				return GameUtil.GetFormattedPercent(-modifier.Value * 100f, GameUtil.TimeSlice.None);
			}
			return base.GetFormattedModifier(modifier);
		}
	}
}
