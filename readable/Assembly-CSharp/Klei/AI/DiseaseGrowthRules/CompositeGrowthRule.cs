using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02001061 RID: 4193
	public class CompositeGrowthRule
	{
		// Token: 0x060081D9 RID: 33241 RVA: 0x00340F23 File Offset: 0x0033F123
		public string Name()
		{
			return this.name;
		}

		// Token: 0x060081DA RID: 33242 RVA: 0x00340F2C File Offset: 0x0033F12C
		public void Overlay(GrowthRule rule)
		{
			if (rule.underPopulationDeathRate != null)
			{
				this.underPopulationDeathRate = rule.underPopulationDeathRate.Value;
			}
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			if (rule.overPopulationHalfLife != null)
			{
				this.overPopulationHalfLife = rule.overPopulationHalfLife.Value;
			}
			if (rule.diffusionScale != null)
			{
				this.diffusionScale = rule.diffusionScale.Value;
			}
			if (rule.minCountPerKG != null)
			{
				this.minCountPerKG = rule.minCountPerKG.Value;
			}
			if (rule.maxCountPerKG != null)
			{
				this.maxCountPerKG = rule.maxCountPerKG.Value;
			}
			if (rule.minDiffusionCount != null)
			{
				this.minDiffusionCount = rule.minDiffusionCount.Value;
			}
			if (rule.minDiffusionInfestationTickCount != null)
			{
				this.minDiffusionInfestationTickCount = rule.minDiffusionInfestationTickCount.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x060081DB RID: 33243 RVA: 0x0034103C File Offset: 0x0033F23C
		public float GetHalfLifeForCount(int count, float kg)
		{
			int num = (int)(this.minCountPerKG * kg);
			int num2 = (int)(this.maxCountPerKG * kg);
			if (count < num)
			{
				return this.populationHalfLife;
			}
			if (count < num2)
			{
				return this.populationHalfLife;
			}
			return this.overPopulationHalfLife;
		}

		// Token: 0x0400623F RID: 25151
		public string name;

		// Token: 0x04006240 RID: 25152
		public float underPopulationDeathRate;

		// Token: 0x04006241 RID: 25153
		public float populationHalfLife;

		// Token: 0x04006242 RID: 25154
		public float overPopulationHalfLife;

		// Token: 0x04006243 RID: 25155
		public float diffusionScale;

		// Token: 0x04006244 RID: 25156
		public float minCountPerKG;

		// Token: 0x04006245 RID: 25157
		public float maxCountPerKG;

		// Token: 0x04006246 RID: 25158
		public int minDiffusionCount;

		// Token: 0x04006247 RID: 25159
		public byte minDiffusionInfestationTickCount;
	}
}
