using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02001064 RID: 4196
	public class ElementExposureRule : ExposureRule
	{
		// Token: 0x060081E4 RID: 33252 RVA: 0x00341155 File Offset: 0x0033F355
		public ElementExposureRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x060081E5 RID: 33253 RVA: 0x00341164 File Offset: 0x0033F364
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x060081E6 RID: 33254 RVA: 0x00341174 File Offset: 0x0033F374
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x0400624A RID: 25162
		public SimHashes element;
	}
}
