using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02001060 RID: 4192
	public class TagGrowthRule : GrowthRule
	{
		// Token: 0x060081D6 RID: 33238 RVA: 0x00340EF9 File Offset: 0x0033F0F9
		public TagGrowthRule(Tag tag)
		{
			this.tag = tag;
		}

		// Token: 0x060081D7 RID: 33239 RVA: 0x00340F08 File Offset: 0x0033F108
		public override bool Test(Element e)
		{
			return e.HasTag(this.tag);
		}

		// Token: 0x060081D8 RID: 33240 RVA: 0x00340F16 File Offset: 0x0033F116
		public override string Name()
		{
			return this.tag.ProperName();
		}

		// Token: 0x0400623E RID: 25150
		public Tag tag;
	}
}
