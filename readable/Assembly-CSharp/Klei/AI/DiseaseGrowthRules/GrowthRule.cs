using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x0200105D RID: 4189
	public class GrowthRule
	{
		// Token: 0x060081CC RID: 33228 RVA: 0x00340D34 File Offset: 0x0033EF34
		public void Apply(ElemGrowthInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				Element element = elements[i];
				if (element.id != SimHashes.Vacuum && this.Test(element))
				{
					ElemGrowthInfo elemGrowthInfo = infoList[i];
					if (this.underPopulationDeathRate != null)
					{
						elemGrowthInfo.underPopulationDeathRate = this.underPopulationDeathRate.Value;
					}
					if (this.populationHalfLife != null)
					{
						elemGrowthInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					if (this.overPopulationHalfLife != null)
					{
						elemGrowthInfo.overPopulationHalfLife = this.overPopulationHalfLife.Value;
					}
					if (this.diffusionScale != null)
					{
						elemGrowthInfo.diffusionScale = this.diffusionScale.Value;
					}
					if (this.minCountPerKG != null)
					{
						elemGrowthInfo.minCountPerKG = this.minCountPerKG.Value;
					}
					if (this.maxCountPerKG != null)
					{
						elemGrowthInfo.maxCountPerKG = this.maxCountPerKG.Value;
					}
					if (this.minDiffusionCount != null)
					{
						elemGrowthInfo.minDiffusionCount = this.minDiffusionCount.Value;
					}
					if (this.minDiffusionInfestationTickCount != null)
					{
						elemGrowthInfo.minDiffusionInfestationTickCount = this.minDiffusionInfestationTickCount.Value;
					}
					infoList[i] = elemGrowthInfo;
				}
			}
		}

		// Token: 0x060081CD RID: 33229 RVA: 0x00340E90 File Offset: 0x0033F090
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x060081CE RID: 33230 RVA: 0x00340E93 File Offset: 0x0033F093
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x04006234 RID: 25140
		public float? underPopulationDeathRate;

		// Token: 0x04006235 RID: 25141
		public float? populationHalfLife;

		// Token: 0x04006236 RID: 25142
		public float? overPopulationHalfLife;

		// Token: 0x04006237 RID: 25143
		public float? diffusionScale;

		// Token: 0x04006238 RID: 25144
		public float? minCountPerKG;

		// Token: 0x04006239 RID: 25145
		public float? maxCountPerKG;

		// Token: 0x0400623A RID: 25146
		public int? minDiffusionCount;

		// Token: 0x0400623B RID: 25147
		public byte? minDiffusionInfestationTickCount;
	}
}
