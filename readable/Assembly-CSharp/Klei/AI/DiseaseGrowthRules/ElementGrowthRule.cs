using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x0200105F RID: 4191
	public class ElementGrowthRule : GrowthRule
	{
		// Token: 0x060081D3 RID: 33235 RVA: 0x00340EC8 File Offset: 0x0033F0C8
		public ElementGrowthRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x060081D4 RID: 33236 RVA: 0x00340ED7 File Offset: 0x0033F0D7
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x060081D5 RID: 33237 RVA: 0x00340EE7 File Offset: 0x0033F0E7
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x0400623D RID: 25149
		public SimHashes element;
	}
}
