using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02001065 RID: 4197
	public class CompositeExposureRule
	{
		// Token: 0x060081E7 RID: 33255 RVA: 0x00341186 File Offset: 0x0033F386
		public string Name()
		{
			return this.name;
		}

		// Token: 0x060081E8 RID: 33256 RVA: 0x0034118E File Offset: 0x0033F38E
		public void Overlay(ExposureRule rule)
		{
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x060081E9 RID: 33257 RVA: 0x003411BB File Offset: 0x0033F3BB
		public float GetHalfLifeForCount(int count)
		{
			return this.populationHalfLife;
		}

		// Token: 0x0400624B RID: 25163
		public string name;

		// Token: 0x0400624C RID: 25164
		public float populationHalfLife;
	}
}
