using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02001063 RID: 4195
	public class ExposureRule
	{
		// Token: 0x060081E0 RID: 33248 RVA: 0x003410E4 File Offset: 0x0033F2E4
		public void Apply(ElemExposureInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (this.Test(elements[i]))
				{
					ElemExposureInfo elemExposureInfo = infoList[i];
					if (this.populationHalfLife != null)
					{
						elemExposureInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					infoList[i] = elemExposureInfo;
				}
			}
		}

		// Token: 0x060081E1 RID: 33249 RVA: 0x00341147 File Offset: 0x0033F347
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x060081E2 RID: 33250 RVA: 0x0034114A File Offset: 0x0033F34A
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x04006249 RID: 25161
		public float? populationHalfLife;
	}
}
