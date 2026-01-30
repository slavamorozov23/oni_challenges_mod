using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02001062 RID: 4194
	public struct ElemExposureInfo
	{
		// Token: 0x060081DD RID: 33245 RVA: 0x00341081 File Offset: 0x0033F281
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.populationHalfLife);
		}

		// Token: 0x060081DE RID: 33246 RVA: 0x00341090 File Offset: 0x0033F290
		public static void SetBulk(ElemExposureInfo[] info, Func<Element, bool> test, ElemExposureInfo settings)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (test(elements[i]))
				{
					info[i] = settings;
				}
			}
		}

		// Token: 0x060081DF RID: 33247 RVA: 0x003410CB File Offset: 0x0033F2CB
		public float CalculateExposureDiseaseCountDelta(int disease_count, float dt)
		{
			return (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
		}

		// Token: 0x04006248 RID: 25160
		public float populationHalfLife;
	}
}
